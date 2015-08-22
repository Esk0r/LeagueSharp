using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using ClipperLib;
using Evade.Pathfinding;
using LeagueSharp;
using LeagueSharp.Common;
using SharpDX;
using Color = System.Drawing.Color;

namespace Evade.Pathfinding
{
    public static class Pathfinding
    {
        public static List<Vector2> PathFind(Vector2 start, Vector2 end)
        {
            var result = new List<Vector2>();

            try
            {
                var innerPolygonList = new List<Geometry.Polygon>();
                var outerPolygonList = new List<Geometry.Polygon>();

                var takeClosestPath = false;

                foreach (var skillshot in Program.DetectedSkillshots)
                {
                    if (skillshot.Evade())
                    {
                        innerPolygonList.Add(skillshot.PathFindingInnerPolygon);
                        outerPolygonList.Add(skillshot.PathFindingPolygon);
                    }
                }

                var innerPolygons = Geometry.ClipPolygons(innerPolygonList).ToPolygons();
                var outerPolygons = Geometry.ClipPolygons(outerPolygonList).ToPolygons();


                if (outerPolygons.Aggregate(false, (current, poly) => current || !poly.IsOutside(end)))
                {
                    end = Evader.GetClosestOutsidePoint(end, outerPolygons);
                }

                if (outerPolygons.Aggregate(false, (current, poly) => current || !poly.IsOutside(start)))
                {
                    start = Evader.GetClosestOutsidePoint(start, outerPolygons);
                }

                if (Utils.CanReach(start, end, innerPolygons, true))
                {
                    return new List<Vector2> { start, end };
                }

                outerPolygons.Add(new Geometry.Polygon { Points = new List<Vector2> { start, end } });

                var nodes = new List<Node>();

                foreach (var pol in outerPolygons)
                {
                    for (int i = 0; i < pol.Points.Count; i++)
                    {
                        if (pol.Points.Count == 2 || !Utils.IsVertexConcave(pol.Points, i))
                        {
                            var node = nodes.FirstOrDefault(node1 => node1.Point == pol.Points[i]);
                            if (node == null)
                            {
                                node = new Node(pol.Points[i]);
                            }
                            
                            nodes.Add(node);
                            foreach (var polygon in outerPolygons)
                            {
                                foreach (var point in polygon.Points)
                                {
                                    if (Utils.CanReach(pol.Points[i], point, innerPolygons, true))
                                    {
                                        var nodeToAdd = nodes.FirstOrDefault(node1 => node1.Point == point);
                                        if (nodeToAdd == null)
                                        {
                                            nodeToAdd = new Node(point);
                                        }
                                        nodes.Add(nodeToAdd);
                                        node.Neightbours.Add(nodeToAdd);
                                    }
                                }
                            }
                        }
                    }
                }

                var startNode = nodes.FirstOrDefault(n => n.Point == start);
                var endNode = nodes.FirstOrDefault(n => n.Point == end);

                if (endNode == null)
                {
                    return result;
                }

                Func<Node, Node, double> distance = (node1, node2) => node1.Point.Distance(node2.Point);
                Func<Node, double> estimate = t => t.Point.Distance(endNode.Point);

                var foundPath = FindPath(startNode, endNode, distance, estimate);

                if (foundPath == null)
                {
                    return result;
                }

                result.AddRange(foundPath.Select(node => node.Point));
                result.Reverse();

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }
            
            return result;
        }

        public static Path<Node> FindPath(
            Node start,
            Node destination,
            Func<Node, Node, double> distance,
            Func<Node, double> estimate)
        {
            var closed = new HashSet<Vector2>();
            var queue = new PriorityQueue<double, Path<Node>>();
            queue.Enqueue(0, new Path<Node>(start));

            while (!queue.IsEmpty)
            {
                var path = queue.Dequeue();
                if (closed.Contains(path.LastStep.Point))
                {
                    continue;
                }

                if (path.LastStep.Point.Equals(destination.Point))
                    return path;

                closed.Add(path.LastStep.Point);
                foreach (Node n in path.LastStep.Neightbours)
                {
                    double d = distance(path.LastStep, n);
                    var newPath = path.AddStep(n, d);
                    queue.Enqueue(newPath.TotalCost + estimate(n), newPath);
                }
            }

            return null;
        }
    }
}
