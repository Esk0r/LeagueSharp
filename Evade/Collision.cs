using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LeagueSharp;
using LeagueSharp.Common;
using SharpDX;

namespace Evade
{
    public enum CollisionObjectTypes
    {
        Minion,
        Champions,
        YasuoWall,
    }

    class FastPredResult
    {
        public bool IsMoving;
        public Vector2 CurrentPos;
        public Vector2 PredictedPos;

    }

    static class Collision
    {
        private static int WallCastT;
        private static Vector2 YasuoWallCastedPos;
        public static void Init()
        {
            Obj_AI_Hero.OnProcessSpellCast += Obj_AI_Hero_OnProcessSpellCast;
        }

       
        static void Obj_AI_Hero_OnProcessSpellCast(Obj_AI_Base sender, GameObjectProcessSpellCastEventArgs args)
        {
            if (sender.IsValid && sender.Team == ObjectManager.Player.Team && args.SData.Name == "YasuoWMovingWall")

            {
                WallCastT = Environment.TickCount;
                YasuoWallCastedPos = sender.ServerPosition.To2D();
            }
        }

        public static FastPredResult FastPrediction(Vector2 from, Obj_AI_Base unit, int delay, int speed)
        {
            var tDelay = delay / 1000f + (from.Distance(unit) / speed);
            var d = tDelay * unit.MoveSpeed;
            var path = unit.GetWaypoints();

            if (path.PathLength() > d)
            {
                return new FastPredResult
                {
                    IsMoving = true,
                    CurrentPos = unit.ServerPosition.To2D(),
                    PredictedPos = path.CutPath((int)d)[0],
                }; 
            }
            return new FastPredResult
            {
                IsMoving = false,
                CurrentPos = path[path.Count - 1],
                PredictedPos = path[path.Count - 1],
            }; 
        }

        public static Vector2 GetCollisionPoint(Skillshot skillshot)
        {
            var collisions = new List<Vector2>();
            var from = skillshot.GetMissilePosition(0);
            skillshot.ForceDisabled = false;
            foreach (var cObject in skillshot.SpellData.CollisionObjects)
            {
                    
                    switch (cObject)
                    {
                            case CollisionObjectTypes.Minion:

                            if (!Config.Menu.Item("MinionCollision").GetValue<bool>()) break;

                                foreach (var minion in MinionManager.GetMinions(from.To3D(), 1200, MinionTypes.All, skillshot.Unit.Team == ObjectManager.Player.Team ? MinionTeam.NotAlly : MinionTeam.NotAllyForEnemy))
                                {
                                    var pred = FastPrediction(from, minion, Math.Max(0, skillshot.SpellData.Delay - (Environment.TickCount - skillshot.StartTick)), skillshot.SpellData.MissileSpeed);
                                    var pos = pred.PredictedPos;

                                    if (pos.Distance(skillshot.GetMissilePosition(0), skillshot.End, true, true) < Math.Pow(skillshot.SpellData.RawRadius + (!pred.IsMoving ? (minion.BoundingRadius - 15) : 0), 2))
                                    {
                                        collisions.Add(pos);
                                    }
                                }

                            break;

                            case CollisionObjectTypes.Champions:
                            if (!Config.Menu.Item("HeroCollision").GetValue<bool>()) break;
                                foreach (var hero in ObjectManager.Get<Obj_AI_Hero>().Where(h => (h.IsValidTarget(float.MaxValue, false) && h.Team == ObjectManager.Player.Team && !h.IsMe || Config.TestOnAllies && h.Team != ObjectManager.Player.Team )))
                                {
                                    var pred = FastPrediction(from, hero, Math.Max(0, skillshot.SpellData.Delay - (Environment.TickCount - skillshot.StartTick)), skillshot.SpellData.MissileSpeed);
                                    var pos = pred.PredictedPos;

                                    if (pos.Distance(skillshot.GetMissilePosition(0), skillshot.End, true, true) < Math.Pow(skillshot.SpellData.RawRadius + 20, 2))
                                    {
                                        collisions.Add(pos);
                                    }
                                }
                            break;

                            case CollisionObjectTypes.YasuoWall:
                            if (!Config.Menu.Item("YasuoCollision").GetValue<bool>()) break;
                            GameObject wall = null;
                            foreach (var gameObject in ObjectManager.Get<GameObject>())
                            {
                                if (gameObject.IsValid &&
                                    System.Text.RegularExpressions.Regex.IsMatch(gameObject.Name, "_w_windwall.\\.troy", System.Text.RegularExpressions.RegexOptions.IgnoreCase)
                                    )
                                {
                                    wall = gameObject;
                                }
                            }
                            if (wall == null) break;
                            var level = wall.Name.Substring(wall.Name.Length - 6, 1);
                            var wallWidth = (300 + 50 * Convert.ToInt32(level));


                            var wallDirection = (wall.Position.To2D() - YasuoWallCastedPos).Normalized().Perpendicular();
                            var wallStart = wall.Position.To2D() + wallWidth / 2 * wallDirection;
                            var wallEnd = wallStart - wallWidth * wallDirection;
                            var wallPolygon = new Geometry.Rectangle(wallStart, wallEnd, 75).ToPolygon();
                            var intersection = new Vector2();
                            var intersections = new List<Vector2>();

                            for (int i = 0; i < wallPolygon.Points.Count; i++)
                            {
                                var inter = wallPolygon.Points[i].Intersection(wallPolygon.Points[i != wallPolygon.Points.Count - 1 ? i + 1 : 0], from, skillshot.End);
                                if (inter.Intersects)
                                    intersections.Add(inter.Point);
                            }
                           
                            if (intersections.Count > 0)
                            {
                                intersection = intersections.OrderBy(item => item.Distance(from)).ToList()[0];
                                var collisionT = Environment.TickCount + Math.Max(0, skillshot.SpellData.Delay - (Environment.TickCount - skillshot.StartTick)) + 100 + (1000 * intersection.Distance(from)) / skillshot.SpellData.MissileSpeed;
                                if (collisionT - WallCastT < 4000)
                                {
                                    if(skillshot.SpellData.Type != SkillShotType.SkillshotMissileLine)
                                    skillshot.ForceDisabled = true;
                                    collisions.Add(intersection);
                                }
                                    
                            }

                            break;
                    }
            }

            return collisions.Count > 0 ? collisions.OrderBy(c => c.Distance(skillshot.Start)).ToList()[0].ProjectOn(skillshot.End, skillshot.Start).LinePoint : skillshot.End;
        }
    }
}
