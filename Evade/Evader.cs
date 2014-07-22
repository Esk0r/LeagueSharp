#region

using System.Collections.Generic;
using LeagueSharp;
using LeagueSharp.Common;
using SharpDX;
using Path = System.Collections.Generic.List<ClipperLib.IntPoint>;
using Paths = System.Collections.Generic.List<System.Collections.Generic.List<ClipperLib.IntPoint>>;
using GamePath = System.Collections.Generic.List<SharpDX.Vector2>;

#endregion

namespace Evade
{
    public static class Evader
    {
        /// <summary>
        /// Returns the posible evade points.
        /// </summary>
        public static GamePath GetEvadePoints(int speed = -1, int delay = 0, bool isBlink = false, bool onlyGood = false)
        {
            speed = speed == -1 ? (int) ObjectManager.Player.MoveSpeed : speed;

            var goodCandidates = new GamePath();
            var badCandidates = new GamePath();

            var polygonList = new List<Geometry.Polygon>();

            foreach (var skillshot in Program.DetectedSkillshots)
            {
                if (skillshot.Evade())
                {
                    polygonList.Add(skillshot.EvadePolygon);
                }
            }

            //Create the danger polygon:
            var dangerPolygons = Geometry.ClipPolygons(polygonList).ToPolygons();
            var myPosition = ObjectManager.Player.ServerPosition.To2D();

            //Scan the sides of each polygon to find the safe area.
            foreach (var poly in dangerPolygons)
            {
                for (var i = 0; i <= poly.Points.Count - 1; i++)
                {
                    var sideStart = poly.Points[i];
                    var sideEnd = poly.Points[(i == poly.Points.Count - 1) ? 0 : i + 1];
                    var direction = (sideEnd - sideStart).Normalized();
                    var originalCandidate = myPosition.ProjectOn(sideStart, sideEnd).SegmentPoint;
                    if(Vector2.DistanceSquared(originalCandidate, myPosition) < 1000 * 1000)
                    { 
                        for (var j = -Config.DiagonalEvadePointsCount; j <= Config.DiagonalEvadePointsCount; j++)
                        {
                            var candidate = originalCandidate + j*Config.DiagonalEvadePointsStep*direction;
                            var pathToPoint = ObjectManager.Player.GetPath(candidate.To3D()).To2DList();

                            if (!isBlink)
                            {
                                if (Program.IsSafePath(pathToPoint, Config.EvadingFirstTimeOffset, speed, delay).IsSafe)
                                {
                                    goodCandidates.Add(candidate);
                                }

                                if (Program.IsSafePath(pathToPoint, Config.EvadingSecondTimeOffset, speed, delay).IsSafe && j == 0)
                                {
                                    badCandidates.Add(candidate);
                                }
                            }
                            else
                            {
                                if (Program.IsSafeToBlink(pathToPoint[pathToPoint.Count - 1], Config.EvadingFirstTimeOffset,
                                    delay))
                                {
                                    goodCandidates.Add(candidate);
                                }

                                if (Program.IsSafeToBlink(pathToPoint[pathToPoint.Count - 1], Config.EvadingSecondTimeOffset,
                                    delay))
                                {
                                    badCandidates.Add(candidate);
                                }
                            }

                        }
                    }
                }
            }


            return (goodCandidates.Count > 0) ? goodCandidates : (onlyGood ? new GamePath() : badCandidates);
        }
    }
}