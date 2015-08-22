// Copyright 2014 - 2014 Esk0r
// Geometry.cs is part of Evade.
// 
// Evade is free software: you can redistribute it and/or modify
// it under the terms of the GNU General Public License as published by
// the Free Software Foundation, either version 3 of the License, or
// (at your option) any later version.
// 
// Evade is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE. See the
// GNU General Public License for more details.
// 
// You should have received a copy of the GNU General Public License
// along with Evade. If not, see <http://www.gnu.org/licenses/>.

#region

using System;
using System.Collections.Generic;
using ClipperLib;
using LeagueSharp.Common;
using SharpDX;
using Color = System.Drawing.Color;
using Path = System.Collections.Generic.List<ClipperLib.IntPoint>;
using Paths = System.Collections.Generic.List<System.Collections.Generic.List<ClipperLib.IntPoint>>;
using GamePath = System.Collections.Generic.List<SharpDX.Vector2>;

#endregion

namespace Evade
{
    /// <summary>
    /// Class that contains the geometry related methods.
    /// </summary>
    public static class Geometry
    {
        private const int CircleLineSegmentN = 22;

        public static Vector3 SwitchYZ(this Vector3 v)
        {
            return new Vector3(v.X, v.Z, v.Y);
        }

        //Clipper
        public static List<Polygon> ToPolygons(this Paths v)
        {
            var result = new List<Polygon>();

            foreach (var path in v)
            {
                result.Add(path.ToPolygon());
            }

            return result;
        }

        /// <summary>
        /// Returns the position on the path after t milliseconds at speed speed.
        /// </summary>
        public static Vector2 PositionAfter(this GamePath self, int t, int speed, int delay = 0)
        {
            var distance = Math.Max(0, t - delay) * speed / 1000;
            for (var i = 0; i <= self.Count - 2; i++)
            {
                var from = self[i];
                var to = self[i + 1];
                var d = (int) to.Distance(from);
                if (d > distance)
                {
                    return from + distance * (to - from).Normalized();
                }
                distance -= d;
            }
            return self[self.Count - 1];
        }

        public static Polygon ToPolygon(this Path v)
        {
            var polygon = new Polygon();
            foreach (var point in v)
            {
                polygon.Add(new Vector2(point.X, point.Y));
            }
            return polygon;
        }


        public static Paths ClipPolygons(List<Polygon> polygons)
        {
            var subj = new Paths(polygons.Count);
            var clip = new Paths(polygons.Count);

            foreach (var polygon in polygons)
            {
                subj.Add(polygon.ToClipperPath());
                clip.Add(polygon.ToClipperPath());
            }

            var solution = new Paths();
            var c = new Clipper();
            c.AddPaths(subj, PolyType.ptSubject, true);
            c.AddPaths(clip, PolyType.ptClip, true);
            c.Execute(ClipType.ctUnion, solution, PolyFillType.pftPositive, PolyFillType.pftEvenOdd);

            return solution;
        }


        public class Circle
        {
            public Vector2 Center;
            public float Radius;

            public Circle(Vector2 center, float radius)
            {
                Center = center;
                Radius = radius;
            }

            public Polygon ToPolygon(int offset = 0, float overrideWidth = -1)
            {
                var result = new Polygon();
                var outRadius = (overrideWidth > 0
                    ? overrideWidth
                    : (offset + Radius) / (float) Math.Cos(2 * Math.PI / CircleLineSegmentN));

                var step = 2 * Math.PI / CircleLineSegmentN;
                var angle = (double) Radius;
                for (var i = 0; i <= CircleLineSegmentN; i++)
                {
                    angle += step;
                    var point = new Vector2(
                        Center.X + outRadius * (float) Math.Cos(angle), Center.Y + outRadius * (float) Math.Sin(angle));
                    result.Add(point);
                }

                return result;
            }
        }

        public class Polygon
        {
            public List<Vector2> Points = new List<Vector2>();

            public void Add(Vector2 point)
            {
                Points.Add(point);
            }

            public Path ToClipperPath()
            {
                var result = new Path(Points.Count);

                foreach (var point in Points)
                {
                    result.Add(new IntPoint(point.X, point.Y));
                }

                return result;
            }

            public bool IsOutside(Vector2 point)
            {
                var p = new IntPoint(point.X, point.Y);
                return Clipper.PointInPolygon(p, ToClipperPath()) != 1;
            }
            public int PointInPolygon(Vector2 point)
            {
                var p = new IntPoint(point.X, point.Y);
                return Clipper.PointInPolygon(p, ToClipperPath());
            }

            public void Draw(Color color, int width = 1)
            {
                for (var i = 0; i <= Points.Count - 1; i++)
                {
                    var nextIndex = (Points.Count - 1 == i) ? 0 : (i + 1);
                    Utils.DrawLineInWorld(Points[i].To3D(), Points[nextIndex].To3D(), width, color);
                }
            }
        }

        public class Rectangle
        {
            public Vector2 Direction;
            public Vector2 Perpendicular;
            public Vector2 REnd;
            public Vector2 RStart;
            public float Width;

            public Rectangle(Vector2 start, Vector2 end, float width)
            {
                RStart = start;
                REnd = end;
                Width = width;
                Direction = (end - start).Normalized();
                Perpendicular = Direction.Perpendicular();
            }

            public Polygon ToPolygon(int offset = 0, float overrideWidth = -1)
            {
                var result = new Polygon();

                result.Add(
                    RStart + (overrideWidth > 0 ? overrideWidth : Width + offset) * Perpendicular - offset * Direction);
                result.Add(
                    RStart - (overrideWidth > 0 ? overrideWidth : Width + offset) * Perpendicular - offset * Direction);
                result.Add(
                    REnd - (overrideWidth > 0 ? overrideWidth : Width + offset) * Perpendicular + offset * Direction);
                result.Add(
                    REnd + (overrideWidth > 0 ? overrideWidth : Width + offset) * Perpendicular + offset * Direction);

                return result;
            }
        }

        public class Ring
        {
            public Vector2 Center;
            public float Radius;
            public float RingRadius; //actually radius width.

            public Ring(Vector2 center, float radius, float ringRadius)
            {
                Center = center;
                Radius = radius;
                RingRadius = ringRadius;
            }

            public Polygon ToPolygon(int offset = 0)
            {
                var result = new Polygon();

                var outRadius = (offset + Radius + RingRadius) / (float) Math.Cos(2 * Math.PI / CircleLineSegmentN);
                var innerRadius = Radius - RingRadius - offset;

                for (var i = 0; i <= CircleLineSegmentN; i++)
                {
                    var angle = i * 2 * Math.PI / CircleLineSegmentN;
                    var point = new Vector2(
                        Center.X - outRadius * (float) Math.Cos(angle), Center.Y - outRadius * (float) Math.Sin(angle));
                    result.Add(point);
                }

                for (var i = 0; i <= CircleLineSegmentN; i++)
                {
                    var angle = i * 2 * Math.PI / CircleLineSegmentN;
                    var point = new Vector2(
                        Center.X + innerRadius * (float) Math.Cos(angle),
                        Center.Y - innerRadius * (float) Math.Sin(angle));
                    result.Add(point);
                }


                return result;
            }
        }

        /// <summary>
        /// Probably only valid for diana
        /// </summary>
        public class Arc
        {
            public Vector2 Start { get; private set; }
            public Vector2 End { get; private set; }

            public int HitBox { get; private set; }
            private float Distance { get; set; }
            public Arc(Vector2 start, Vector2 end, int hitbox)
            {
                Start = start;
                End = end;
                HitBox = hitbox;
                Distance = Start.Distance(End);
            }

            public Polygon ToPolygon(int offset = 0)
            {
                offset += HitBox;
                var result = new Polygon();

                var innerRadius = -0.1562f * Distance + 687.31f;
                var outerRadius = 0.35256f * Distance + 133f;

                outerRadius = outerRadius / (float)Math.Cos(2 * Math.PI / CircleLineSegmentN);

                var innerCenters = LeagueSharp.Common.Geometry.CircleCircleIntersection(Start, End, innerRadius, innerRadius);
                var outerCenters = LeagueSharp.Common.Geometry.CircleCircleIntersection(Start, End, outerRadius, outerRadius);

                var innerCenter = innerCenters[0];
                var outerCenter = outerCenters[0];

                Render.Circle.DrawCircle(innerCenter.To3D(), 100, Color.White);

                var direction = (End - outerCenter).Normalized();
                var end = (Start - outerCenter).Normalized();
                var maxAngle = (float)(direction.AngleBetween(end) * Math.PI / 180);
                
                var step = -maxAngle / CircleLineSegmentN;
                //outercircle
                for (int i = 0; i < CircleLineSegmentN; i++)
                {
                    var angle = step * i;
                    var point = outerCenter + (outerRadius + 15 + offset) * direction.Rotated(angle);
                    result.Add(point);
                }

                direction = (Start - innerCenter).Normalized();
                end = (End - innerCenter).Normalized();
                maxAngle = (float)(direction.AngleBetween(end) * Math.PI / 180);
                step = maxAngle / CircleLineSegmentN;
                //outercircle
                for (int i = 0; i < CircleLineSegmentN; i++)
                {
                    var angle = step * i;
                    var point = innerCenter + Math.Max(0, innerRadius - offset - 100) * direction.Rotated(angle);
                    result.Add(point);
                }

                return result;
            }
        }

        public class Sector
        {
            public float Angle;
            public Vector2 Center;
            public Vector2 Direction;
            public float Radius;

            public Sector(Vector2 center, Vector2 direction, float angle, float radius)
            {
                Center = center;
                Direction = direction;
                Angle = angle;
                Radius = radius;
            }

            public Polygon ToPolygon(int offset = 0)
            {
                var result = new Polygon();
                var outRadius = (Radius + offset) / (float) Math.Cos(2 * Math.PI / CircleLineSegmentN);

                result.Add(Center);
                var Side1 = Direction.Rotated(-Angle * 0.5f);

                for (var i = 0; i <= CircleLineSegmentN; i++)
                {
                    var cDirection = Side1.Rotated(i * Angle / CircleLineSegmentN).Normalized();
                    result.Add(new Vector2(Center.X + outRadius * cDirection.X, Center.Y + outRadius * cDirection.Y));
                }

                return result;
            }
        }
    }
}