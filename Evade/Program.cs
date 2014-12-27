﻿// Copyright 2014 - 2014 Esk0r
// Program.cs is part of Evade.
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
using System.IO;
using System.Linq;
using LeagueSharp;
using LeagueSharp.Common;
using SharpDX;
using Color = System.Drawing.Color;
using GamePath = System.Collections.Generic.List<SharpDX.Vector2>;

#endregion

namespace Evade
{
    internal class Program
    {
        public static SpellList<Skillshot> DetectedSkillshots = new SpellList<Skillshot>();

        private static bool _evading;

        private static Vector2 _evadePoint;

        public static bool NoSolutionFound = false;

        public static Vector2 EvadeToPoint = new Vector2();
        public static Vector2 EvadeToPoint2 = new Vector2();


        public static int LastWardJumpAttempt = 0;
        public static Vector2 PlayerDestination = new Vector2();
        public static Vector2 PreviousTickPosition = new Vector2();
        private static readonly Random RandomN = new Random();
        private static int LastSentMovePacketT = 0;
        private static int LastSentMovePacketT2 = 0;

        private static int LastSMovePacketT = 0;

        public static bool Evading
        {
            get { return _evading; } //
            set
            {
                if (value == true)
                {
                    LastSentMovePacketT = 0;
                    ObjectManager.Player.SendMovePacket(EvadePoint);
                }

                Global.IsEvading = value;
                _evading = value;
            }
        }

        public static Vector2 EvadePoint
        {
            get { return _evadePoint; }
            set
            {
                if (value.IsValid())
                {
                    ObjectManager.Player.SendMovePacket(value);
                }
                _evadePoint = value;
            }
        }

        private static void Main(string[] args)
        {
            if (Game.Mode == GameMode.Running)
            {
                Game_OnGameStart(new EventArgs());
            }
           
            Game.OnGameStart += Game_OnGameStart;
        }

        private static bool IsSpellShielded(Obj_AI_Hero unit)
        {
            if (ObjectManager.Player.HasBuffOfType(BuffType.SpellShield))
            {
                return true;
            }

            if (ObjectManager.Player.HasBuffOfType(BuffType.SpellImmunity))
            {
                return true;
            }

            //Sivir E
            if (unit.LastCastedSpellName() == "SivirE" && (Environment.TickCount - unit.LastCastedSpellT()) < 300)
            {
                return true;
            }

            //Morganas E
            if (unit.LastCastedSpellName() == "BlackShield" && (Environment.TickCount - unit.LastCastedSpellT()) < 300)
            {
                return true;
            }

            //Nocturnes E
            if (unit.LastCastedSpellName() == "NocturneShit" && (Environment.TickCount - unit.LastCastedSpellT()) < 300)
            {
                return true;
            }


            return false;
        }

        private static void Game_OnGameStart(EventArgs args)
        {
            //Add the game events.
            Game.OnGameUpdate += Game_OnOnGameUpdate;
            Obj_AI_Hero.OnIssueOrder += ObjAiHeroOnOnIssueOrder;
            Spellbook.OnCastSpell += Spellbook_OnCastSpell;
            //Set up the OnDetectSkillshot Event.
            SkillshotDetector.OnDetectSkillshot += OnDetectSkillshot;
            SkillshotDetector.OnDeleteMissile += SkillshotDetectorOnOnDeleteMissile;

            //For skillshot drawing.
            Drawing.OnDraw += Drawing_OnDraw;

            //Ondash event.
            CustomEvents.Unit.OnDash += UnitOnOnDash;

            DetectedSkillshots.OnAdd += DetectedSkillshots_OnAdd;

            //Create the menu to allow the user to change the config.
            Config.CreateMenu();

            //Initialze the collision
            Collision.Init();

            Game.PrintChat("<font color=\"#00BFFF\">Evade# -</font> <font color=\"#FFFFFF\">Loaded</font>");


            if (Config.PrintSpellData)
            {
                foreach (var hero in ObjectManager.Get<Obj_AI_Hero>())
                {
                    foreach (var spell in hero.Spellbook.Spells)
                    {
                        Console.WriteLine(
                             "Slot  " + spell.Slot + " " + spell.SData.Name + " w:" + spell.SData.LineWidth + " s:" + spell.SData.MissileSpeed + " r: " +
                            spell.SData.CastRange[0]);
                    }
                }
                Console.WriteLine(ObjectManager.Player.Spellbook.GetSpell(SpellSlot.W).Name);
            }
        }
        private static void DetectedSkillshots_OnAdd(object sender, EventArgs e)
        {
            Evading = false;
        }

        private static void SkillshotDetectorOnOnDeleteMissile(Skillshot skillshot, Obj_SpellMissile missile)
        {
            if (skillshot.SpellData.SpellName == "VelkozQ")
            {
                var spellData = SpellDatabase.GetByName("VelkozQSplit");
                var direction = skillshot.Direction.Perpendicular();
                if (DetectedSkillshots.Count(s => s.SpellData.SpellName == "VelkozQSplit") == 0)
                {
                    for (var i = -1; i <= 1; i = i + 2)
                    {
                        var skillshotToAdd = new Skillshot(
                            DetectionType.ProcessSpell, spellData, Environment.TickCount, missile.Position.To2D(),
                            missile.Position.To2D() + i * direction * spellData.Range, skillshot.Unit);
                        DetectedSkillshots.Add(skillshotToAdd);
                    }
                }
            }
        }

        private static void OnDetectSkillshot(Skillshot skillshot)
        {
            //Check if the skillshot is already added.
            var alreadyAdded = false;

            if (Config.Menu.Item("DisableFow").GetValue<bool>() && !skillshot.Unit.IsVisible)
            {
                return;
            }
                
            foreach (var item in DetectedSkillshots)
            {
                if (item.SpellData.SpellName == skillshot.SpellData.SpellName &&
                    (item.Unit.NetworkId == skillshot.Unit.NetworkId &&
                     (skillshot.Direction).AngleBetween(item.Direction) < 5 &&
                     (skillshot.Start.Distance(item.Start) < 100 || skillshot.SpellData.FromObjects.Length == 0)))
                {
                    alreadyAdded = true;
                }
            }

            //Check if the skillshot is from an ally.
            if (skillshot.Unit.Team == ObjectManager.Player.Team && !Config.TestOnAllies)
            {
                return;
            }

            //Check if the skillshot is too far away.
            if (skillshot.Start.Distance(ObjectManager.Player.ServerPosition.To2D()) >
                (skillshot.SpellData.Range + skillshot.SpellData.Radius + 1000) * 1.5)
            {
                return;
            }

            //Add the skillshot to the detected skillshot list.
            if (!alreadyAdded)
            {
                //Multiple skillshots like twisted fate Q.
                if (skillshot.DetectionType == DetectionType.ProcessSpell)
                {
                    if (skillshot.SpellData.MultipleNumber != -1)
                    {
                        var originalDirection = skillshot.Direction;

                        for (var i = -(skillshot.SpellData.MultipleNumber - 1) / 2;
                            i <= (skillshot.SpellData.MultipleNumber - 1) / 2;
                            i++)
                        {
                            var end = skillshot.Start +
                                      skillshot.SpellData.Range *
                                      originalDirection.Rotated(skillshot.SpellData.MultipleAngle * i);
                            var skillshotToAdd = new Skillshot(
                                skillshot.DetectionType, skillshot.SpellData, skillshot.StartTick, skillshot.Start, end,
                                skillshot.Unit);

                            DetectedSkillshots.Add(skillshotToAdd);
                        }
                        return;
                    }

                    if (skillshot.SpellData.SpellName == "UFSlash")
                    {
                        skillshot.SpellData.MissileSpeed = 1600 + (int) skillshot.Unit.MoveSpeed;
                    }

                    if (skillshot.SpellData.Invert)
                    {
                        var newDirection = -(skillshot.End - skillshot.Start).Normalized();
                        var end = skillshot.Start + newDirection * skillshot.Start.Distance(skillshot.End);
                        var skillshotToAdd = new Skillshot(
                            skillshot.DetectionType, skillshot.SpellData, skillshot.StartTick, skillshot.Start, end,
                            skillshot.Unit);
                        DetectedSkillshots.Add(skillshotToAdd);
                        return;
                    }

                    if (skillshot.SpellData.Centered)
                    {
                        var start = skillshot.Start - skillshot.Direction * skillshot.SpellData.Range;
                        var end = skillshot.Start + skillshot.Direction * skillshot.SpellData.Range;
                        var skillshotToAdd = new Skillshot(
                            skillshot.DetectionType, skillshot.SpellData, skillshot.StartTick, start, end,
                            skillshot.Unit);
                        DetectedSkillshots.Add(skillshotToAdd);
                        return;
                    }

                    if (skillshot.SpellData.SpellName == "SyndraE" || skillshot.SpellData.SpellName == "syndrae5")
                    {
                        var angle = 60;
                        var edge1 =
                            (skillshot.End - skillshot.Unit.ServerPosition.To2D()).Rotated(
                                -angle / 2 * (float) Math.PI / 180);
                        var edge2 = edge1.Rotated(angle * (float) Math.PI / 180);

                        foreach (var minion in ObjectManager.Get<Obj_AI_Minion>())
                        {
                            var v = minion.ServerPosition.To2D() - skillshot.Unit.ServerPosition.To2D();
                            if (minion.Name == "Seed" && edge1.CrossProduct(v) > 0 && v.CrossProduct(edge2) > 0 &&
                                minion.Distance(skillshot.Unit) < 800 &&
                                (minion.Team != ObjectManager.Player.Team || Config.TestOnAllies))
                            {
                                var start = minion.ServerPosition.To2D();
                                var end = skillshot.Unit.ServerPosition.To2D()
                                    .Extend(
                                        minion.ServerPosition.To2D(),
                                        skillshot.Unit.Distance(minion) > 200 ? 1300 : 1000);

                                var skillshotToAdd = new Skillshot(
                                    skillshot.DetectionType, skillshot.SpellData, skillshot.StartTick, start, end,
                                    skillshot.Unit);
                                DetectedSkillshots.Add(skillshotToAdd);
                            }
                        }
                        return;
                    }

                    if (skillshot.SpellData.SpellName == "AlZaharCalloftheVoid")
                    {
                        var start = skillshot.End - skillshot.Direction.Perpendicular() * 400;
                        var end = skillshot.End + skillshot.Direction.Perpendicular() * 400;
                        var skillshotToAdd = new Skillshot(
                            skillshot.DetectionType, skillshot.SpellData, skillshot.StartTick, start, end,
                            skillshot.Unit);
                        DetectedSkillshots.Add(skillshotToAdd);
                        return;
                    }

                    if (skillshot.SpellData.SpellName == "ZiggsQ")
                    {
                        var d1 = skillshot.Start.Distance(skillshot.End);
                        var d2 = d1 * 0.4f;
                        var d3 = d2 * 0.69f;


                        var bounce1SpellData = SpellDatabase.GetByName("ZiggsQBounce1");
                        var bounce2SpellData = SpellDatabase.GetByName("ZiggsQBounce2");

                        var bounce1Pos = skillshot.End + skillshot.Direction * d2;
                        var bounce2Pos = bounce1Pos + skillshot.Direction * d3;

                        bounce1SpellData.Delay =
                            (int) (skillshot.SpellData.Delay + d1 * 1000f / skillshot.SpellData.MissileSpeed + 500);
                        bounce2SpellData.Delay =
                            (int) (bounce1SpellData.Delay + d2 * 1000f / bounce1SpellData.MissileSpeed + 500);

                        var bounce1 = new Skillshot(
                            skillshot.DetectionType, bounce1SpellData, skillshot.StartTick, skillshot.End, bounce1Pos,
                            skillshot.Unit);
                        var bounce2 = new Skillshot(
                            skillshot.DetectionType, bounce2SpellData, skillshot.StartTick, bounce1Pos, bounce2Pos,
                            skillshot.Unit);

                        DetectedSkillshots.Add(bounce1);
                        DetectedSkillshots.Add(bounce2);
                    }

                    if (skillshot.SpellData.SpellName == "ZiggsR")
                    {
                        skillshot.SpellData.Delay =
                            (int) (1500 + 1500 * skillshot.End.Distance(skillshot.Start) / skillshot.SpellData.Range);
                    }

                    if (skillshot.SpellData.SpellName == "JarvanIVDragonStrike")
                    {
                        var endPos = new Vector2();

                        foreach (var s in DetectedSkillshots)
                        {
                            if (s.Unit.NetworkId == skillshot.Unit.NetworkId && s.SpellData.Slot == SpellSlot.E)
                            {
                                endPos = s.End;
                            }
                        }

                        foreach (var m in ObjectManager.Get<Obj_AI_Minion>())
                        {
                            if (m.BaseSkinName == "jarvanivstandard" && m.Team == skillshot.Unit.Team &&
                                skillshot.IsDanger(m.Position.To2D()))
                            {
                                endPos = m.Position.To2D();
                            }
                        }

                        if (!endPos.IsValid())
                        {
                            return;
                        }

                        skillshot.End = endPos + 200 * (endPos - skillshot.Start).Normalized();
                        skillshot.Direction = (skillshot.End - skillshot.Start).Normalized();
                    }
                }

                if (skillshot.SpellData.SpellName == "OriannasQ")
                {
                    var endCSpellData = SpellDatabase.GetByName("OriannaQend");

                    var skillshotToAdd = new Skillshot(
                        skillshot.DetectionType, endCSpellData, skillshot.StartTick, skillshot.Start, skillshot.End,
                        skillshot.Unit);

                    DetectedSkillshots.Add(skillshotToAdd);
                }


                //Dont allow fow detection.
                if (skillshot.SpellData.DisableFowDetection && skillshot.DetectionType == DetectionType.RecvPacket)
                {
                    return;
                }
#if DEBUG
                Console.WriteLine(Environment.TickCount + "Adding new skillshot: " + skillshot.SpellData.SpellName);
#endif

                DetectedSkillshots.Add(skillshot);
            }
        }

        private static void Game_OnOnGameUpdate(EventArgs args)
        {
            //Set evading to false after blinking
            if (PreviousTickPosition.IsValid() &&
                ObjectManager.Player.ServerPosition.To2D().Distance(PreviousTickPosition) > 200)
            {
                Evading = false;
            }
            
            PreviousTickPosition = ObjectManager.Player.ServerPosition.To2D();

            //Remove the detected skillshots that have expired.
            DetectedSkillshots.RemoveAll(skillshot => !skillshot.IsActive());

            //Trigger OnGameUpdate on each skillshot.
            foreach (var skillshot in DetectedSkillshots)
            {
                skillshot.Game_OnGameUpdate();
            }

            //Evading disabled
            if (!Global.Evade)
            {
                return;
            }

            //Avoid sending move/cast packets while dead.
            if (ObjectManager.Player.IsDead)
            {
                return;
            }

            //Avoid sending move/cast packets while channeling important spells.
            if (ObjectManager.Player.IsChannelingImportantSpell())
            {
                return;
            }

            /*Avoid evading while stunned or immobile.*/
            if (Utils.ImmobileTime(ObjectManager.Player) - Environment.TickCount > Game.Ping / 2 + 70)
            {
                Evading = false;
                return;
            }

            /**/
            if (EvadeToPoint.IsValid() && DetectedSkillshots.Count > 0)
            {
                if (Environment.TickCount - LastSentMovePacketT2 > 1000 / 10)
                {
                    ObjectManager.Player.IssueOrder(GameObjectOrder.MoveTo, EvadeToPoint.To3D());
                    LastSentMovePacketT2 = Environment.TickCount;
                }
            }

            //Shield allies.
            foreach (var ally in ObjectManager.Get<Obj_AI_Hero>())
            {
                if (ally.IsValidTarget(1000, false))
                {
                    var shieldAlly = Config.Menu.Item("shield" + ally.ChampionName);
                    if (shieldAlly != null && shieldAlly.GetValue<bool>())
                    {
                        var allySafeResult = IsSafe(ally.ServerPosition.To2D());

                        if (!allySafeResult.IsSafe)
                        {
                            var dangerLevel = 0;

                            foreach (var skillshot in allySafeResult.SkillshotList)
                            {
                                dangerLevel = Math.Max(dangerLevel, skillshot.GetValue<Slider>("DangerLevel").Value);
                            }

                            foreach (var evadeSpell in EvadeSpellDatabase.Spells)
                            {
                                if (evadeSpell.IsShield && evadeSpell.CanShieldAllies &&
                                    ally.Distance(ObjectManager.Player) < evadeSpell.MaxRange &&
                                    dangerLevel >= evadeSpell.DangerLevel &&
                                    ObjectManager.Player.Spellbook.CanUseSpell(evadeSpell.Slot) == SpellState.Ready &&
                                    IsAboutToHit(ally, evadeSpell.Delay))
                                {
                                    ObjectManager.Player.Spellbook.CastSpell(evadeSpell.Slot, ally);
                                }
                            }
                        }
                    }
                }
            }


            //Spell Shielded
            if (IsSpellShielded(ObjectManager.Player))
            {
                return;
            }

            NoSolutionFound = false;

            var currentPath = ObjectManager.Player.GetWaypoints();
            var safeResult = IsSafe(ObjectManager.Player.ServerPosition.To2D());
            var safePath = IsSafePath(currentPath, 100);

            //Continue evading
            if (Evading && IsSafe(EvadePoint).IsSafe)
            {
                if (safeResult.IsSafe)
                {
                    //We are safe, stop evading.
                    Evading = false;
                }
                else
                {
                    if (Environment.TickCount - LastSentMovePacketT > 1000/15)
                    {
                        LastSentMovePacketT = Environment.TickCount;
                        ObjectManager.Player.SendMovePacket(EvadePoint);
                    }
                    return;
                }
            }
                //Stop evading if the point is not safe.
            else if (Evading)
            {
                Evading = false;
            }

            //The path is not safe.
            if (!safePath.IsSafe)
            {
                //Inside the danger polygon.
                if (!safeResult.IsSafe)
                {
                    //Search for an evade point:
                    TryToEvade(safeResult.SkillshotList, EvadeToPoint.IsValid() ? EvadeToPoint : Game.CursorPos.To2D());
                }
                    //Outside the danger polygon.
                else
                {
                    //Stop at the edge of the skillshot.
                    ObjectManager.Player.SendMovePacket(safePath.Intersection.Point);
                }
            }
        }

        static void Spellbook_OnCastSpell(GameObject sender, SpellbookCastSpellEventArgs args)
        {
            if (sender.IsValid && sender.IsMe)
            {
                if (args.Slot == SpellSlot.Recall)
                {
                    EvadeToPoint = new Vector2();
                }
            }
        }

        /// <summary>
        /// Used to block the movement to avoid entering in dangerous areas.
        /// </summary>
        /// 
        private static void ObjAiHeroOnOnIssueOrder(Obj_AI_Base sender, GameObjectIssueOrderEventArgs args)
        {
            if (!sender.IsMe)
            {
                return;
            }

            //Don't block the movement packets if cant find an evade point.
            if (NoSolutionFound)
            {
                return;
            }

            //Evading disabled
            if (!Global.Evade)
            {
                return;
            }

            if (EvadeSpellDatabase.Spells.Any(evadeSpell => evadeSpell.Name == "Walking" && !evadeSpell.Enabled))
            {
                return;
            }

            //Spell Shielded
            if (IsSpellShielded(ObjectManager.Player))
            {
                return;
            }

            if (args.Order == GameObjectOrder.MoveTo)
            {
                EvadeToPoint.X = args.TargetPosition.X;
                EvadeToPoint.Y = args.TargetPosition.Y;
            }
            else
            {
                EvadeToPoint.X = 0;
                EvadeToPoint.Y = 0;
            }
            
            var myPath =
                ObjectManager.Player.GetPath(
                    new Vector3(args.TargetPosition.X, args.TargetPosition.Y, ObjectManager.Player.ServerPosition.Z)).To2DList();
            var safeResult = IsSafe(ObjectManager.Player.ServerPosition.To2D());


            //If we are evading:
            if (Evading || !safeResult.IsSafe)
            {
                var rcSafePath = IsSafePath(myPath, Config.EvadingRouteChangeTimeOffset);
                if (args.Order == GameObjectOrder.MoveTo)
                {
                    if (Evading &&
                        Environment.TickCount - Config.LastEvadePointChangeT > Config.EvadePointChangeInterval)
                    {
                        //Update the evade point to the closest one:
                        var points = Evader.GetEvadePoints(-1, 0, false, true);
                        if (points.Count > 0)
                        {
                            var to = new Vector2(args.TargetPosition.X, args.TargetPosition.Y);
                            EvadePoint = to.Closest(points);
                            Evading = true;
                            Config.LastEvadePointChangeT = Environment.TickCount;
                        }
                    }

                    //If the path is safe let the user follow it.
                    if (rcSafePath.IsSafe && IsSafe(myPath[myPath.Count - 1]).IsSafe && args.Order == GameObjectOrder.MoveTo)
                    {
                        EvadePoint = myPath[myPath.Count - 1];
                        Evading = true;
                    }
                }

                //Block the packets if we are evading or not safe.
                args.Process = false;
                return;
            }

            var safePath = IsSafePath(myPath, Config.CrossingTimeOffset);

            //Not evading, outside the skillshots.
            //The path is not safe, stop in the intersection point.
            if (!safePath.IsSafe && args.Order != GameObjectOrder.AttackUnit)
            {
                if (safePath.Intersection.Valid)
                {
                    if (ObjectManager.Player.Distance(safePath.Intersection.Point) > 75)
                    {
                        ObjectManager.Player.SendMovePacket(safePath.Intersection.Point);
                    }
                    else
                    {
                        if (/*DetectedSkillshots.Count == 1 &&*/ Environment.TickCount - LastSMovePacketT > 400)
                        {
                            LastSMovePacketT = Environment.TickCount;

                            var perpendicular =
                                (ObjectManager.Player.ServerPosition.To2D() - safePath.Intersection.Point)
                                    .Normalized();
                            var direction = perpendicular.Perpendicular();

                            var p = ObjectManager.Player.ServerPosition.To2D() + 1 * perpendicular + 150 * direction;
                            var p2 = ObjectManager.Player.ServerPosition.To2D() + 1 * perpendicular - 150 * direction;

                            if (!IsSafePath(ObjectManager.Player.GetPath(p.To3D()).To2DList(), 100).IsSafe)
                            {
                                p = new Vector2();
                            }

                            if (!IsSafePath(ObjectManager.Player.GetPath(p2.To3D()).To2DList(), 100).IsSafe)
                            {
                                p2 = new Vector2();
                            }

                            EvadeToPoint2 = (p.IsValid() && (p.Distance(EvadeToPoint) < p2.Distance(EvadeToPoint))) ? p : p2;

                            if (EvadeToPoint2.IsValid())
                            {
                                ObjectManager.Player.SendMovePacket(EvadeToPoint2);
                            }
                        }
                    }
                }
                args.Process = false;
            }

            //AutoAttacks.
            if (!safePath.IsSafe && args.Order == GameObjectOrder.AttackUnit)
            {
                var target = args.Target;
                if (target != null && target.IsValid<Obj_AI_Base>() && target.IsVisible)
                {
                    //Out of attack range.
                    if (ObjectManager.Player.ServerPosition.To2D().Distance(((Obj_AI_Base)target).ServerPosition) >
                        ObjectManager.Player.AttackRange + ObjectManager.Player.BoundingRadius +
                        target.BoundingRadius)
                    {
                        if (safePath.Intersection.Valid)
                        {
                            ObjectManager.Player.SendMovePacket(safePath.Intersection.Point);
                        }
                        args.Process = false;
                    }
                }
            }
        }

        private static void UnitOnOnDash(Obj_AI_Base sender, Dash.DashItem args)
        {
            if (sender.IsMe)
            {
                if (Config.PrintSpellData)
                {
                    Console.WriteLine(
                        Environment.TickCount + "DASH: Speed: " + args.Speed + " Width:" +
                        args.EndPos.Distance(args.StartPos));
                }

                Utility.DelayAction.Add(args.Duration, delegate { Evading = false; });
            }
        }

        /// <summary>
        /// Returns true if the point is not inside the detected skillshots.
        /// </summary>
        public static IsSafeResult IsSafe(Vector2 point)
        {
            var result = new IsSafeResult();
            result.SkillshotList = new List<Skillshot>();

            foreach (var skillshot in DetectedSkillshots)
            {
                if (skillshot.Evade() && skillshot.IsDanger(point))
                {
                    result.SkillshotList.Add(skillshot);
                }
            }

            result.IsSafe = (result.SkillshotList.Count == 0);

            return result;
        }

        /// <summary>
        /// Returns if the unit will get hit by skillshots taking the path.
        /// </summary>
        public static SafePathResult IsSafePath(GamePath path,
            int timeOffset,
            int speed = -1,
            int delay = 0,
            Obj_AI_Base unit = null)
        {
            var IsSafe = true;
            var intersections = new List<FoundIntersection>();
            var intersection = new FoundIntersection();

            foreach (var skillshot in DetectedSkillshots)
            {
                if (skillshot.Evade())
                {
                    var sResult = skillshot.IsSafePath(path, timeOffset, speed, delay, unit);
                    IsSafe = (IsSafe) ? sResult.IsSafe : false;

                    if (sResult.Intersection.Valid)
                    {
                        intersections.Add(sResult.Intersection);
                    }
                }
            }

            //Return the first intersection
            if (!IsSafe)
            {
                var sortedList = intersections.OrderBy(o => o.Distance).ToList();

                return new SafePathResult(false, sortedList.Count > 0 ? sortedList[0] : intersection);
            }

            return new SafePathResult(IsSafe, intersection);
        }

        /// <summary>
        /// Returns if you can blink to the point without being hit.
        /// </summary>
        public static bool IsSafeToBlink(Vector2 point, int timeOffset, int delay)
        {
            foreach (var skillshot in DetectedSkillshots)
            {
                if (skillshot.Evade())
                {
                    if (!skillshot.IsSafeToBlink(point, timeOffset, delay))
                    {
                        return false;
                    }
                }
            }
            return true;
        }

        /// <summary>
        /// Returns true if some detected skillshot is about to hit the unit.
        /// </summary>
        public static bool IsAboutToHit(Obj_AI_Base unit, int time)
        {
            time += 150;
            foreach (var skillshot in DetectedSkillshots)
            {
                if (skillshot.Evade())
                {
                    if (skillshot.IsAboutToHit(time, unit))
                    {
                        return true;
                    }
                }
            }
            return false;
        }

        private static void TryToEvade(List<Skillshot> HitBy, Vector2 to)
        {
            var dangerLevel = 0;

            foreach (var skillshot in HitBy)
            {
                dangerLevel = Math.Max(dangerLevel, skillshot.GetValue<Slider>("DangerLevel").Value);
            }

            foreach (var evadeSpell in EvadeSpellDatabase.Spells)
            {
                if (evadeSpell.Enabled && evadeSpell.DangerLevel <= dangerLevel)
                {
                    //SpellShields
                    if (evadeSpell.IsSpellShield &&
                        ObjectManager.Player.Spellbook.CanUseSpell(evadeSpell.Slot) == SpellState.Ready)
                    {
                        if (IsAboutToHit(ObjectManager.Player, evadeSpell.Delay))
                        {
                            ObjectManager.Player.Spellbook.CastSpell(evadeSpell.Slot, ObjectManager.Player);
                        }

                        //Let the user move freely inside the skillshot.
                        NoSolutionFound = true;
                        return;
                    }

                    //Walking
                    if (evadeSpell.Name == "Walking")
                    {
                        var points = Evader.GetEvadePoints();
                        if (points.Count > 0)
                        {
                            EvadePoint = to.Closest(points);
                            var nEvadePoint = EvadePoint.Extend(ObjectManager.Player.ServerPosition.To2D(), -100);
                            if (
                                Program.IsSafePath(
                                    ObjectManager.Player.GetPath(nEvadePoint.To3D()).To2DList(),
                                    Config.EvadingSecondTimeOffset, (int) ObjectManager.Player.MoveSpeed, 100).IsSafe)
                            {
                                EvadePoint = nEvadePoint;
                            }
                            Evading = true;
                            return;
                        }
                    }

                    if (evadeSpell.IsReady())
                    {
                        //MovementSpeed Buff
                        if (evadeSpell.IsMovementSpeedBuff)
                        {
                            var points = Evader.GetEvadePoints((int) evadeSpell.MoveSpeedTotalAmount());

                            if (points.Count > 0)
                            {
                                EvadePoint = to.Closest(points);
                                Evading = true;

                                if (evadeSpell.IsSummonerSpell)
                                {
                                    ObjectManager.Player.Spellbook.CastSpell(
                                        evadeSpell.Slot, ObjectManager.Player);
                                }
                                else
                                {
                                    ObjectManager.Player.Spellbook.CastSpell(evadeSpell.Slot, ObjectManager.Player);
                                }

                                return;
                            }
                        }

                        //Dashes
                        if (evadeSpell.IsDash)
                        {
                            //Targetted dashes
                            if (evadeSpell.IsTargetted) //Lesinga W.
                            {
                                var targets = Evader.GetEvadeTargets(
                                    evadeSpell.ValidTargets, evadeSpell.Speed, evadeSpell.Delay, evadeSpell.MaxRange,
                                    false, false);

                                if (targets.Count > 0)
                                {
                                    var closestTarget = Utils.Closest(targets, to);
                                    EvadePoint = closestTarget.ServerPosition.To2D();
                                    Evading = true;

                                    if (evadeSpell.IsSummonerSpell)
                                    {
                                        ObjectManager.Player.Spellbook.CastSpell(evadeSpell.Slot, closestTarget);
                                    }
                                    else
                                    {
                                        ObjectManager.Player.Spellbook.CastSpell(evadeSpell.Slot, closestTarget);
                                    }

                                    return;
                                }
                                if (Environment.TickCount - LastWardJumpAttempt < 250)
                                {
                                    //Let the user move freely inside the skillshot.
                                    NoSolutionFound = true;
                                    return;
                                }

                                if (evadeSpell.IsTargetted &&
                                    evadeSpell.ValidTargets.Contains(SpellValidTargets.AllyWards) &&
                                    Config.Menu.Item("WardJump" + evadeSpell.Name).GetValue<bool>())
                                {
                                    var wardSlot = Items.GetWardSlot();
                                    if (wardSlot != null)
                                    {
                                        var points = Evader.GetEvadePoints(evadeSpell.Speed, evadeSpell.Delay, false);

                                        // Remove the points out of range
                                        points.RemoveAll(
                                            item => item.Distance(ObjectManager.Player.ServerPosition) > 600);

                                        if (points.Count > 0)
                                        {
                                            //Dont dash just to the edge:
                                            for (var i = 0; i < points.Count; i++)
                                            {
                                                var k =
                                                    (int)
                                                        (600 -
                                                         ObjectManager.Player.ServerPosition.To2D().Distance(points[i]));

                                                k = k - new Random(Environment.TickCount).Next(k);
                                                var extended = points[i] +
                                                               k *
                                                               (points[i] - ObjectManager.Player.ServerPosition.To2D())
                                                                   .Normalized();
                                                if (IsSafe(extended).IsSafe)
                                                {
                                                    points[i] = extended;
                                                }
                                            }

                                            var ePoint = to.Closest(points);
                                            ObjectManager.Player.Spellbook.CastSpell(wardSlot.SpellSlot, ePoint.To3D());
                                            LastWardJumpAttempt = Environment.TickCount;
                                            //Let the user move freely inside the skillshot.
                                            NoSolutionFound = true;
                                            return;
                                        }
                                    }
                                }
                            }
                                //Skillshot type dashes.
                            else
                            {
                                var points = Evader.GetEvadePoints(evadeSpell.Speed, evadeSpell.Delay, false);

                                // Remove the points out of range
                                points.RemoveAll(
                                    item => item.Distance(ObjectManager.Player.ServerPosition) > evadeSpell.MaxRange);

                                //If the spell has a fixed range (Vaynes Q), calculate the real dashing location. TODO: take into account walls in the future.
                                if (evadeSpell.FixedRange)
                                {
                                    for (var i = 0; i < points.Count; i++)
                                    {
                                        points[i] = ObjectManager.Player.ServerPosition.To2D()
                                            .Extend(points[i], evadeSpell.MaxRange);
                                    }

                                    for (var i = points.Count - 1; i > 0; i--)
                                    {
                                        if (!IsSafe(points[i]).IsSafe)
                                        {
                                            points.RemoveAt(i);
                                        }
                                    }
                                }
                                else
                                {
                                    for (var i = 0; i < points.Count; i++)
                                    {
                                        var k =
                                            (int)
                                                (evadeSpell.MaxRange -
                                                 ObjectManager.Player.ServerPosition.To2D().Distance(points[i]));
                                        k -= Math.Max(RandomN.Next(k) - 100, 0);
                                        var extended = points[i] +
                                                       k *
                                                       (points[i] - ObjectManager.Player.ServerPosition.To2D())
                                                           .Normalized();
                                        if (IsSafe(extended).IsSafe)
                                        {
                                            points[i] = extended;
                                        }
                                    }
                                }

                                if (points.Count > 0)
                                {
                                    EvadePoint = to.Closest(points);
                                    Evading = true;

                                    if (!evadeSpell.Invert)
                                    {
                                        if (evadeSpell.RequiresPreMove)
                                        {
                                            ObjectManager.Player.SendMovePacket(EvadePoint);
                                            var theSpell = evadeSpell;
                                            Utility.DelayAction.Add(
                                                Game.Ping / 2 + 100,
                                                delegate
                                                {
                                                    ObjectManager.Player.Spellbook.CastSpell(
                                                        theSpell.Slot, EvadePoint.To3D());
                                                });
                                        }
                                        else
                                        {
                                            ObjectManager.Player.Spellbook.CastSpell(evadeSpell.Slot, EvadePoint.To3D());
                                        }
                                    }
                                    else
                                    {
                                        var castPoint = ObjectManager.Player.ServerPosition.To2D() -
                                                        (EvadePoint - ObjectManager.Player.ServerPosition.To2D());
                                        ObjectManager.Player.Spellbook.CastSpell(evadeSpell.Slot, castPoint.To3D());
                                    }

                                    return;
                                }
                            }
                        }

                        //Blinks
                        if (evadeSpell.IsBlink)
                        {
                            //Targetted blinks
                            if (evadeSpell.IsTargetted)
                            {
                                var targets = Evader.GetEvadeTargets(
                                    evadeSpell.ValidTargets, int.MaxValue, evadeSpell.Delay, evadeSpell.MaxRange, true,
                                    false);

                                if (targets.Count > 0)
                                {
                                    if (IsAboutToHit(ObjectManager.Player, evadeSpell.Delay))
                                    {
                                        var closestTarget = Utils.Closest(targets, to);
                                        EvadePoint = closestTarget.ServerPosition.To2D();
                                        Evading = true;

                                        if (evadeSpell.IsSummonerSpell)
                                        {
                                            ObjectManager.Player.Spellbook.CastSpell(
                                                evadeSpell.Slot, closestTarget);
                                        }
                                        else
                                        {
                                            ObjectManager.Player.Spellbook.CastSpell(evadeSpell.Slot, closestTarget);
                                        }
                                    }

                                    //Let the user move freely inside the skillshot.
                                    NoSolutionFound = true;
                                    return;
                                }
                                if (Environment.TickCount - LastWardJumpAttempt < 250)
                                {
                                    //Let the user move freely inside the skillshot.
                                    NoSolutionFound = true;
                                    return;
                                }

                                if (evadeSpell.IsTargetted &&
                                    evadeSpell.ValidTargets.Contains(SpellValidTargets.AllyWards) &&
                                    Config.Menu.Item("WardJump" + evadeSpell.Name).GetValue<bool>())
                                {
                                    var wardSlot = Items.GetWardSlot();
                                    if (wardSlot != null)
                                    {
                                        var points = Evader.GetEvadePoints(int.MaxValue, evadeSpell.Delay, true);

                                        // Remove the points out of range
                                        points.RemoveAll(
                                            item => item.Distance(ObjectManager.Player.ServerPosition) > 600);

                                        if (points.Count > 0)
                                        {
                                            //Dont blink just to the edge:
                                            for (var i = 0; i < points.Count; i++)
                                            {
                                                var k =
                                                    (int)
                                                        (600 -
                                                         ObjectManager.Player.ServerPosition.To2D().Distance(points[i]));

                                                k = k - new Random(Environment.TickCount).Next(k);
                                                var extended = points[i] +
                                                               k *
                                                               (points[i] - ObjectManager.Player.ServerPosition.To2D())
                                                                   .Normalized();
                                                if (IsSafe(extended).IsSafe)
                                                {
                                                    points[i] = extended;
                                                }
                                            }

                                            var ePoint = to.Closest(points);
                                            ObjectManager.Player.Spellbook.CastSpell(wardSlot.SpellSlot, ePoint.To3D());
                                            LastWardJumpAttempt = Environment.TickCount;
                                            //Let the user move freely inside the skillshot.
                                            NoSolutionFound = true;
                                            return;
                                        }
                                    }
                                }
                            }

                                //Skillshot type blinks.
                            else
                            {
                                var points = Evader.GetEvadePoints(int.MaxValue, evadeSpell.Delay, true);

                                // Remove the points out of range
                                points.RemoveAll(
                                    item => item.Distance(ObjectManager.Player.ServerPosition) > evadeSpell.MaxRange);


                                //Dont blink just to the edge:
                                for (var i = 0; i < points.Count; i++)
                                {
                                    var k =
                                        (int)
                                            (evadeSpell.MaxRange -
                                             ObjectManager.Player.ServerPosition.To2D().Distance(points[i]));

                                    k = k - new Random(Environment.TickCount).Next(k);
                                    var extended = points[i] +
                                                   k *
                                                   (points[i] - ObjectManager.Player.ServerPosition.To2D()).Normalized();
                                    if (IsSafe(extended).IsSafe)
                                    {
                                        points[i] = extended;
                                    }
                                }


                                if (points.Count > 0)
                                {
                                    if (IsAboutToHit(ObjectManager.Player, evadeSpell.Delay))
                                    {
                                        EvadePoint = to.Closest(points);
                                        Evading = true;
                                        if (evadeSpell.IsSummonerSpell)
                                        {
                                            ObjectManager.Player.Spellbook.CastSpell(
                                                evadeSpell.Slot, EvadePoint.To3D());
                                        }
                                        else
                                        {
                                            ObjectManager.Player.Spellbook.CastSpell(evadeSpell.Slot, EvadePoint.To3D());
                                        }
                                    }

                                    //Let the user move freely inside the skillshot.
                                    NoSolutionFound = true;
                                    return;
                                }
                            }
                        }

                        //Invulnerabilities, like Fizz's E
                        if (evadeSpell.IsInvulnerability)
                        {
                            if (evadeSpell.IsTargetted)
                            {
                                var targets = Evader.GetEvadeTargets(
                                    evadeSpell.ValidTargets, int.MaxValue, 0, evadeSpell.MaxRange, true, false, true);

                                if (targets.Count > 0)
                                {
                                    if (IsAboutToHit(ObjectManager.Player, evadeSpell.Delay))
                                    {
                                        var closestTarget = Utils.Closest(targets, to);
                                        EvadePoint = closestTarget.ServerPosition.To2D();
                                        Evading = true;
                                        ObjectManager.Player.Spellbook.CastSpell(evadeSpell.Slot, closestTarget);
                                    }

                                    //Let the user move freely inside the skillshot.
                                    NoSolutionFound = true;
                                    return;
                                }
                            }
                            else
                            {
                                if (IsAboutToHit(ObjectManager.Player, evadeSpell.Delay))
                                {
                                    if (evadeSpell.SelfCast)
                                    {
                                        ObjectManager.Player.Spellbook.CastSpell(evadeSpell.Slot);
                                    }
                                    else
                                    {
                                        ObjectManager.Player.Spellbook.CastSpell(
                                            evadeSpell.Slot, ObjectManager.Player.ServerPosition);
                                    }
                                }
                            }


                            //Let the user move freely inside the skillshot.
                            NoSolutionFound = true;
                            return;
                        }
                    }

                    //Zhonyas
                    if (evadeSpell.Name == "Zhonyas" && (Items.CanUseItem("ZhonyasHourglass")))
                    {
                        if (IsAboutToHit(ObjectManager.Player, 100))
                        {
                            Items.UseItem("ZhonyasHourglass");
                        }

                        //Let the user move freely inside the skillshot.
                        NoSolutionFound = true;

                        return;
                    }

                    //Shields
                    if (evadeSpell.IsShield &&
                        ObjectManager.Player.Spellbook.CanUseSpell(evadeSpell.Slot) == SpellState.Ready)
                    {
                        if (IsAboutToHit(ObjectManager.Player, evadeSpell.Delay))
                        {
                            ObjectManager.Player.Spellbook.CastSpell(evadeSpell.Slot, ObjectManager.Player);
                        }

                        //Let the user move freely inside the skillshot.
                        NoSolutionFound = true;
                        return;
                    }
                }
            }

            NoSolutionFound = true;
        }

        private static void Drawing_OnDraw(EventArgs args)
        {
            if (!Config.Menu.Item("EnableDrawings").GetValue<bool>())
            {
                return;
            }
            var Border = Config.Menu.Item("Border").GetValue<Slider>().Value;
            var missileColor = Config.Menu.Item("MissileColor").GetValue<Color>();

            //Draw the polygon for each skillshot.
            foreach (var skillshot in DetectedSkillshots)
            {
                skillshot.Draw(
                    (skillshot.Evade() && Global.Evade)
                        ? Config.Menu.Item("EnabledColor").GetValue<Color>()
                        : Config.Menu.Item("DisabledColor").GetValue<Color>(), missileColor, Border);
            }


            if (Config.TestOnAllies)
            {
                var myPath = ObjectManager.Player.GetWaypoints();

                for (var i = 0; i < myPath.Count - 1; i++)
                {
                    var A = myPath[i];
                    var B = myPath[i + 1];
                    var SA = Drawing.WorldToScreen(A.To3D());
                    var SB = Drawing.WorldToScreen(B.To3D());
                     Drawing.DrawLine(SA.X, SA.Y, SB.X, SB.Y, 1, Color.White);
                }

                Drawing.DrawCircle(EvadePoint.To3D(), 300, Color.White);
            }

        }

        public struct IsSafeResult
        {
            public bool IsSafe;
            public List<Skillshot> SkillshotList;
        }
    }
}
