#region
using System;
using System.Linq;
using LeagueSharp;
using LeagueSharp.Common;
using SharpDX;
using Color = System.Drawing.Color;
#endregion

namespace Marksman
{
    internal class Lucian : Champion
    {
        public static Spell Q;
        public static Spell Q2;
        public static Spell W;
        public static Spell E;

        public static bool DoubleHit = false;
        private static int xAttackLeft = 0;
        private static float xPassiveUsedTime;

        public Lucian()
        {
            Utils.PrintMessage("Lucian loaded.");

            Q = new Spell(SpellSlot.Q, 675);
            Q2 = new Spell(SpellSlot.Q, 1100);
            W = new Spell(SpellSlot.W, 1000);

            Q.SetSkillshot(0.25f, 65f, 1100f, false, SkillshotType.SkillshotLine);
            W.SetSkillshot(0.30f, 80f, 1600f, true, SkillshotType.SkillshotLine);
            E = new Spell(SpellSlot.E, 475);
            
            xPassiveUsedTime = Game.Time;

            Obj_AI_Base.OnProcessSpellCast += Game_OnProcessSpell;
        }

        public static bool IsPositionSafeForE(Obj_AI_Hero target, Spell spell)
        {
            var predPos = spell.GetPrediction(target).UnitPosition.To2D();
            var myPos = ObjectManager.Player.Position.To2D();
            var newPos = (target.Position.To2D() - myPos);
            newPos.Normalize();

            var checkPos = predPos + newPos * (spell.Range - Vector2.Distance(predPos, myPos));
            Obj_Turret closestTower = null;

            foreach (var tower in ObjectManager.Get<Obj_Turret>()
                .Where(tower => tower.IsValid && !tower.IsDead && Math.Abs(tower.Health) > float.Epsilon)
                .Where(tower => Vector3.Distance(tower.Position, ObjectManager.Player.Position) < 1450))
            {
                closestTower = tower;
            }

            if (closestTower == null)
                return true;

            if (Vector2.Distance(closestTower.Position.To2D(), checkPos) <= 910)
                return false;

            return true;
        }

        public static Obj_AI_Base QMinion
        {
            get
            {
                var vTarget = TargetSelector.GetTarget(Q2.Range, TargetSelector.DamageType.Physical);
                var vMinions = MinionManager.GetMinions(
                    ObjectManager.Player.ServerPosition, Q.Range, MinionTypes.All, MinionTeam.NotAlly,
                    MinionOrderTypes.None);

                return (from vMinion in vMinions.Where(vMinion => vMinion.IsValidTarget(Q.Range))
                        let endPoint =
                            vMinion.ServerPosition.To2D()
                                .Extend(ObjectManager.Player.ServerPosition.To2D(), -Q2.Range)
                                .To3D()
                        where
                            vMinion.Distance(vTarget) <= vTarget.Distance(ObjectManager.Player) &&
                            Intersection(ObjectManager.Player.ServerPosition.To2D(), endPoint.To2D(),
                                vTarget.ServerPosition.To2D(), vTarget.BoundingRadius + Q.Width / 4)
                        select vMinion).FirstOrDefault();
            }
        }

        public override void Drawing_OnDraw(EventArgs args)
        {
            Spell[] spellList = { Q, Q2, W, E };
            foreach (var spell in spellList)
            {
                var menuItem = GetValue<Circle>("Draw" + spell.Slot);
                if (!menuItem.Active || spell.Level < 0) return;

                Render.Circle.DrawCircle(ObjectManager.Player.Position, spell.Range, menuItem.Color);
            }
        }

        public static bool Intersection(Vector2 p1, Vector2 p2, Vector2 pC, float radius)
        {
            var p3 = new Vector2(pC.X + radius, pC.Y + radius);

            var m = ((p2.Y - p1.Y) / (p2.X - p1.X));
            var constant = (m * p1.X) - p1.Y;
            var b = -(2f * ((m * constant) + p3.X + (m * p3.Y)));
            var a = (1 + (m * m));
            var c = ((p3.X * p3.X) + (p3.Y * p3.Y) - (radius * radius) + (2f * constant * p3.Y) + (constant * constant));
            var d = ((b * b) - (4f * a * c));

            return d > 0;
        }

        public void Game_OnProcessSpell(Obj_AI_Base unit, GameObjectProcessSpellCastEventArgs spell)
        {
            if (!unit.IsMe) return;
            if (spell.SData.Name.Contains("summoner")) return;
            if (!Config.Item("Passive" + Id).GetValue<bool>()) return;
            
            if (spell.SData.Name.ToLower().Contains("lucianq") || spell.SData.Name.ToLower().Contains("lucianw") ||
                spell.SData.Name.ToLower().Contains("luciane") || spell.SData.Name.ToLower().Contains("lucianr"))
            {
                xAttackLeft = 1;
                xPassiveUsedTime = Game.Time;
            }

            if (spell.SData.Name.ToLower().Contains("lucianpassiveattack"))
            {
                Utility.DelayAction.Add(500, () =>
                {
                    xAttackLeft -= 1;
                });
            }
        }

        public override void Game_OnGameUpdate(EventArgs args)
        {
            if (ObjectManager.Player.IsDead)
            {
                xAttackLeft = 0;
                return;
            }
            
            if (Game.Time > xPassiveUsedTime + 3 && xAttackLeft == 1)
            {
                xAttackLeft = 0;
            }
            if (Config.Item("Passive" + Id).GetValue<bool>() && xAttackLeft > 0)
                return;

            if (Q.IsReady() && GetValue<KeyBind>("UseQTH").Active && ToggleActive)
            {
                if (ObjectManager.Player.HasBuff("Recall"))
                    return;

                var t = TargetSelector.GetTarget(Q.Range, TargetSelector.DamageType.Physical);
                if (t != null)
                    Q.CastOnUnit(t);
            }

            if (Q.IsReady() && GetValue<KeyBind>("UseQExtendedTH").Active && ToggleActive)
            {
                if (ObjectManager.Player.HasBuff("Recall"))
                    return;

                var t = TargetSelector.GetTarget(Q.Range, TargetSelector.DamageType.Physical);
                if (t.IsValidTarget() && QMinion.IsValidTarget())
                {
                    if (ObjectManager.Player.Distance(t) > Q.Range)
                        Q.CastOnUnit(QMinion);
                }
            }

            if ((!ComboActive && !HarassActive)) return;

            var useQ = Config.Item("UseQ" + (ComboActive ? "C" : "H") + Id).GetValue<bool>();
            var useW = Config.Item("UseW" + (ComboActive ? "C" : "H") + Id).GetValue<bool>();
            var useE = Config.Item("UseE" + (ComboActive ? "C" : "H") + Id).GetValue<bool>();
            var useQExtended = Config.Item("UseQExtended" + (ComboActive ? "C" : "H") + Id).GetValue<bool>();

//            if (ObjectManager.Player.Spellbook.GetSpell(SpellSlot.R).Level > 0)
//                Config.Item("GHOSTBLADE")
//                    .SetValue(ObjectManager.Player.Spellbook.GetSpell(SpellSlot.R).Name == "LucianR");

            if (useQExtended && Q.IsReady())
            {
                var t = TargetSelector.GetTarget(Q2.Range, TargetSelector.DamageType.Physical);
                if (t.IsValidTarget() && QMinion.IsValidTarget())
                {
                    if (!Orbwalking.InAutoAttackRange(t))
                        Q.CastOnUnit(QMinion);
                }
            }

            if (useQ && Q.IsReady())
            {
                var t = TargetSelector.GetTarget(Q.Range, TargetSelector.DamageType.Physical);
                if (t.IsValidTarget())
                {
                    Q.CastOnUnit(t);
                }
            }

            if (useW && W.IsReady())
            {
                var t = TargetSelector.GetTarget(W.Range, TargetSelector.DamageType.Physical);
                if (t.IsValidTarget())
                {
                    W.Cast(t);
                }
            }

            if (useE && E.IsReady())
            {
                var t = TargetSelector.GetTarget(Q.Range, TargetSelector.DamageType.Physical);
                if (t != null)
                {
                    E.Cast(Game.CursorPos);
                }
            }
        }

        public override bool ComboMenu(Menu config)
        {
            config.AddItem(new MenuItem("UseQC" + Id, "Use Q").SetValue(true));
            config.AddItem(new MenuItem("UseQExtendedC" + Id, "Use Extended Q").SetValue(true));
            config.AddItem(new MenuItem("Cx", ""));
            config.AddItem(new MenuItem("UseWC" + Id, "Use W").SetValue(true));
            config.AddItem(new MenuItem("UseEC" + Id, "Use E").SetValue(true));
            return true;
        }

        public override bool HarassMenu(Menu config)
        {
            config.AddItem(new MenuItem("UseQH" + Id, "Use Q").SetValue(true));
            config.AddItem(
                new MenuItem("UseQTH" + Id, "Use Q (Toggle)").SetValue(new KeyBind("T".ToCharArray()[0],
                    KeyBindType.Toggle)));
            config.AddItem(new MenuItem("Cx",""));
            config.AddItem(new MenuItem("UseQExtendedH" + Id, "Use Extended Q").SetValue(true));
            config.AddItem(
                new MenuItem("UseQExtendedTH" + Id, "Use Ext. Q (Toggle)").SetValue(new KeyBind("H".ToCharArray()[0],
                    KeyBindType.Toggle)));
            config.AddItem(new MenuItem("Cx", ""));
            config.AddItem(new MenuItem("UseWH" + Id, "Use W").SetValue(true));
            config.AddItem(new MenuItem("UseEH" + Id, "Use E").SetValue(true));
            return true;
        }

        public override bool MiscMenu(Menu config)
        {
            config.AddItem(new MenuItem("Passive" + Id, "Check Passive").SetValue(true));
            return true;
        }

        public override bool DrawingMenu(Menu config)
        {
            config.AddItem(new MenuItem("DrawQ" + Id, "Q range").SetValue(new Circle(true, Color.Gray)));
            config.AddItem(new MenuItem("DrawQ2" + Id, "Ext. Q range").SetValue(new Circle(true, Color.Gray)));
            config.AddItem(new MenuItem("DrawW" + Id, "W range").SetValue(new Circle(false, Color.Gray)));
            config.AddItem(new MenuItem("DrawE" + Id, "E range").SetValue(new Circle(false, Color.Gray)));

            return true;
        }

        public override bool ExtrasMenu(Menu config)
        {

            return true;
        }

        public override bool LaneClearMenu(Menu config)
        {

             return true;
        }
    }
}
