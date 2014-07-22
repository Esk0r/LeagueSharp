#region

using System;
using System.Collections.Generic;
using System.Linq;
using LeagueSharp;
using LeagueSharp.Common;
using SharpDX;
using Color = System.Drawing.Color;
using GamePath = System.Collections.Generic.List<SharpDX.Vector2>;

#endregion

namespace Evade
{
    public enum SkillShotType
    {
        SkillshotCircle,
        SkillshotLine,
        SkillshotMissileLine,
        SkillshotCone,
    }

    public enum DetectionType
    {
        RecvPacket,
        ProcessSpell,
    }

    public struct SafePathResult
    {
        public FoundIntersection Intersection;
        public bool IsSafe;

        public SafePathResult(bool isSafe, FoundIntersection intersection)
        {
            IsSafe = isSafe;
            Intersection = intersection;
        }
    }

    public struct FoundIntersection
    {
        public Vector2 ComingFrom;
        public float Distance;
        public Vector2 Point;
        public int Time;
        public bool Valid;

        public FoundIntersection(float distance, int time, Vector2 point, Vector2 comingFrom)
        {
            Distance = distance;
            ComingFrom = comingFrom;
            Valid = (point.X != 0) && (point.Y != 0);
            Point = point + Config.GridSize*(ComingFrom - point).Normalized();
            Time = time;
        }
    }


    public class Skillshot
    {
        public Geometry.Circle Circle;
        public DetectionType DetectionType;
        public Vector2 Direction;

        public Vector2 End;
        public Geometry.Polygon EvadePolygon { get; set; }
        public Vector2 MissilePosition;
        public Geometry.Polygon Polygon;
        public Geometry.Rectangle Rectangle;
        public Geometry.Sector Sector;
        public SpellData SpellData;
        public Vector2 Start;
        public int StartTick;
        public Obj_AI_Base Unit;

        private bool _cachedValue;
        private int _cachedValueTick = 0;
        public Skillshot(DetectionType detectionType, SpellData spellData, int startT, Vector2 start, Vector2 end,
            Obj_AI_Base unit)
        {
            DetectionType = detectionType;
            SpellData = spellData;
            StartTick = startT;
            Start = start;
            End = end;
            MissilePosition = start;
            Direction = (end - start).Normalized();

            Unit = unit;

            //Create the spatial object for each type of skillshot.
            switch (spellData.Type)
            {
                case SkillShotType.SkillshotCircle:
                    Circle = new Geometry.Circle(end, spellData.Radius);
                    break;
                case SkillShotType.SkillshotLine:
                    Rectangle = new Geometry.Rectangle(Start, End, spellData.Radius);
                    break;
                case SkillShotType.SkillshotMissileLine:
                    Rectangle = new Geometry.Rectangle(Start, End, spellData.Radius);
                    break;
                case SkillShotType.SkillshotCone:
                    Sector = new Geometry.Sector(start, end - start, spellData.Radius * (float) Math.PI / 180, spellData.Range);
                    break;
            }

            UpdatePolygon(); //Create the polygon.
        }

        /// <summary>
        /// Returns the value from this skillshot menu.
        /// </summary>
        public T GetValue<T>(string name)
        {
            return Config.Menu.Item(name + SpellData.MenuItemName).GetValue<T>();
        }

        /// <summary>
        /// Returns if the skillshot has expired.
        /// </summary>
        public bool IsActive()
        {
            return Environment.TickCount <=
                   StartTick + SpellData.Delay + SpellData.ExtraDuration +
                   1000*(Start.Distance(End)/SpellData.MissileSpeed);
        }

        public bool Evade()
        {
            if (Environment.TickCount - _cachedValueTick < 100)
                return _cachedValue;

            if (!GetValue<bool>("IsDangerous") && Config.Menu.Item("OnlyDangerous").GetValue<KeyBind>().Active)
            {
                _cachedValue = false;
                _cachedValueTick = Environment.TickCount;
                return _cachedValue;
            }


            _cachedValue = GetValue<bool>("Enabled");
            _cachedValueTick = Environment.TickCount;

            return _cachedValue;
        }

        public void Game_OnGameUpdate()
        {
            //Update the missile position each time the game updates.
            if (SpellData.Type == SkillShotType.SkillshotMissileLine)
            {
                Rectangle = new Geometry.Rectangle(GetMissilePosition(0), End, SpellData.Radius);
                UpdatePolygon();
            }

            //Spells that update to the unit position.
            if (SpellData.MissileFollowsUnit)
            {
                if (Unit.IsVisible)
                {
                    End = Unit.ServerPosition.To2D();
                    Direction = (End - Start).Normalized();
                    UpdatePolygon();
                }
            }
        }

        public void UpdatePolygon()
        {
            switch (SpellData.Type)
            {
                case SkillShotType.SkillshotCircle:
                    Polygon = Circle.ToPolygon();
                    EvadePolygon = Circle.ToPolygon(Config.ExtraEvadeDistance);
                    break;
                case SkillShotType.SkillshotLine:
                    Polygon = Rectangle.ToPolygon();
                    EvadePolygon = Rectangle.ToPolygon(Config.ExtraEvadeDistance);
                    break;
                case SkillShotType.SkillshotMissileLine:
                    Polygon = Rectangle.ToPolygon();
                    EvadePolygon = Rectangle.ToPolygon(Config.ExtraEvadeDistance);
                    break;
                case SkillShotType.SkillshotCone:
                    Polygon = Sector.ToPolygon();
                    EvadePolygon = Sector.ToPolygon(Config.ExtraEvadeDistance);
                    break;
            }
        }

        /// <summary>
        /// Returns the missile position after time time.
        /// </summary>
        public Vector2 GetMissilePosition(int time)
        {
            var t = Math.Max(0, Environment.TickCount + time - StartTick - SpellData.Delay);
            t = (int) Math.Max(0, Math.Min(End.Distance(Start), t*SpellData.MissileSpeed/1000));
            return Start + Direction*t;
        }


        /// <summary>
        /// Returns if the skillshot will hit you when trying to blink to the point.
        /// </summary>
        public bool IsSafeToBlink(Vector2 point, int timeOffset, int delay = 0)
        {
            timeOffset = timeOffset/2;
            //Skillshots with missile
            if (SpellData.Type == SkillShotType.SkillshotMissileLine)
            {
                if (IsSafe(ObjectManager.Player.ServerPosition.To2D()))
                    return true;

                var missilePositionAfterBlink = GetMissilePosition(delay + timeOffset);
                var myPositionProjection = ObjectManager.Player.ServerPosition.To2D().ProjectOn(Start, End);
                
                if (missilePositionAfterBlink.Distance(End) < myPositionProjection.SegmentPoint.Distance(End))
                    return false;
                
                return true;
            }

           
            //skillshots without missile
               var timeToExplode = SpellData.ExtraDuration + SpellData.Delay +
                                    (int)(1000 * Start.Distance(End) / SpellData.MissileSpeed) -
                                    (Environment.TickCount - StartTick);

                return timeToExplode < timeOffset + delay;
        }

        /// <summary>
        /// Returns if the skillshot will hit the unit if the unit follows the path.
        /// </summary>
        public SafePathResult IsSafePath(GamePath path, int timeOffset, int speed = -1, int delay = 0,
            Obj_AI_Base unit = null)
        {
            var Distance = 0f;
            timeOffset += Game.Ping / 2;

            speed = (speed == -1) ? (int) ObjectManager.Player.MoveSpeed : speed;

            if (unit == null)
                unit = ObjectManager.Player;

            var allIntersections = new List<FoundIntersection>();
            for (var i = 0; i <= path.Count - 2; i++)
            {
                var from = path[i];
                var to = path[i + 1];
                var segmentIntersections = new List<FoundIntersection>();

                for (var j = 0; j <= Polygon.Points.Count - 1; j++)
                {
                    var sideStart = Polygon.Points[j];
                    var sideEnd = Polygon.Points[j == (Polygon.Points.Count - 1) ? 0 : j + 1];

                    var intersection = from.Intersection(to, sideStart, sideEnd);

                    if (intersection.Intersects)
                    {
                        segmentIntersections.Add(new FoundIntersection(Distance + intersection.Point.Distance(from),
                            (int) ((Distance + intersection.Point.Distance(from))*1000/speed), intersection.Point, from));
                    }
                }

                var sortedList = segmentIntersections.OrderBy(o => o.Distance).ToList();
                allIntersections.AddRange(sortedList);

                Distance += from.Distance(to);
            }

            //Skillshot with missile.
            if (SpellData.Type == SkillShotType.SkillshotMissileLine)
            {
                //Outside the skillshot
                if (IsSafe(ObjectManager.Player.ServerPosition.To2D()))
                {
                    //No intersections -> Safe
                    if (allIntersections.Count == 0)
                        return new SafePathResult(true, new FoundIntersection());

                    for (var i = 0; i <= allIntersections.Count - 1; i = i + 2)
                    {
                        var enterIntersection = allIntersections[i];
                        var enterIntersectionProjection = enterIntersection.Point.ProjectOn(Start, End).SegmentPoint;

                        //Intersection with no exit point.
                        if (i == allIntersections.Count - 1)
                        {
                            var missilePositionOnIntersection = GetMissilePosition(enterIntersection.Time - timeOffset);
                            return
                                new SafePathResult(
                                    (End.Distance(missilePositionOnIntersection) + 50 <=
                                     End.Distance(enterIntersectionProjection)) &&
                                    ObjectManager.Player.MoveSpeed < SpellData.MissileSpeed, allIntersections[0]);
                        }


                        var exitIntersection = allIntersections[i + 1];
                        var exitIntersectionProjection = exitIntersection.Point.ProjectOn(Start, End).SegmentPoint;

                        var missilePosOnEnter = GetMissilePosition(enterIntersection.Time - timeOffset);
                        var missilePosOnExit = GetMissilePosition(exitIntersection.Time + timeOffset);

                        //Missile didnt pass.
                        if (missilePosOnEnter.Distance(End) + 50 > enterIntersectionProjection.Distance(End))
                        {
                            if (missilePosOnExit.Distance(End) <= exitIntersectionProjection.Distance(End))
                            {
                                return new SafePathResult(false, allIntersections[0]);
                            }
                        }
                    }

                    return new SafePathResult(true, allIntersections[0]);
                }
                    //Inside the skillshot.
                if (allIntersections.Count == 0)
                    return new SafePathResult(false, new FoundIntersection());

                if (allIntersections.Count > 0)
                {
                    //Check only for the exit point
                    var exitIntersection = allIntersections[0];
                    var exitIntersectionProjection = exitIntersection.Point.ProjectOn(Start, End).SegmentPoint;

                    var missilePosOnExit = GetMissilePosition(exitIntersection.Time + timeOffset);
                    if (missilePosOnExit.Distance(End) <= exitIntersectionProjection.Distance(End))
                        return new SafePathResult(false, allIntersections[0]);
                }
            }

            //Skillshot without missile.

            if (IsSafe(ObjectManager.Player.ServerPosition.To2D()))
            {
                if (allIntersections.Count == 0)
                    return new SafePathResult(true, new FoundIntersection());

                if (SpellData.DonCross)
                    return new SafePathResult(false, allIntersections[0]);
            }
            else
            {
                if (allIntersections.Count == 0)
                    return new SafePathResult(false, new FoundIntersection());
            }

            var timeToExplode = SpellData.ExtraDuration + SpellData.Delay +
                                (int) (1000*Start.Distance(End)/SpellData.MissileSpeed) -
                                (Environment.TickCount - StartTick);
            var myPositionWhenExplodes = path.PositionAfter(timeToExplode, (int) ObjectManager.Player.MoveSpeed, delay);

            if (!IsSafe(myPositionWhenExplodes))
                return new SafePathResult(false, allIntersections[0]);

            return new SafePathResult(true, allIntersections[0]);
        }

        public bool IsSafe(Vector2 point)
        {
            return Polygon.IsOutside(point);
        }

        public bool IsDanger(Vector2 point)
        {
            return !IsSafe(point);
        }

        //Returns if the skillshot is about to hit the unit in the next time seconds.
        public bool IsAboutToHit(int time, Obj_AI_Base unit)
        {
            if (SpellData.Type == SkillShotType.SkillshotMissileLine)
            {
                var missilePos = GetMissilePosition(0);
                var missilePosAfterT = GetMissilePosition(time);

                //TODO: Check for minion collision etc.. in the future.
                var projection = ObjectManager.Player.ServerPosition.To2D().ProjectOn(missilePos, missilePosAfterT);

                if (projection.IsOnSegment &&
                    projection.SegmentPoint.Distance(ObjectManager.Player.ServerPosition) < SpellData.Radius)
                {
                    return true;
                }

                    return false;
            }

            if (!IsSafe(unit.ServerPosition.To2D()))
            {
                var timeToExplode = SpellData.ExtraDuration + SpellData.Delay +
                                    (int) (1000 * Start.Distance(End)/SpellData.MissileSpeed) -
                                    (Environment.TickCount - StartTick);
                if (timeToExplode <= time)
                {
                    return true;
                }
            }

            return false;
        }

        public void Draw(Color color, int width = 1)
        {
            if (!GetValue<bool>("Draw")) return;
            Polygon.Draw(color, width);
        }
    }
}