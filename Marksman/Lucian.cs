#region

using System;
using System.Linq;
using LeagueSharp;
using LeagueSharp.Common;
using SharpDX;

#endregion

namespace Marksman
{
    internal class Lucian : Champion
    {
        public static Spell Q;
        public static Spell Q2;
        public static Spell W;

        public Lucian()
        {
            Utils.PrintMessage("Lucian loaded.");

            Q = new Spell(SpellSlot.Q, 630);
            Q2 = new Spell(SpellSlot.Q, 1100);
            W = new Spell(SpellSlot.W, 1000);

            Q.SetSkillshot(0.25f, 65f, 1200f, false, SkillshotType.SkillshotLine);
            W.SetSkillshot(0.15f, 80f, 1000f, true, SkillshotType.SkillshotLine);
        }

        public bool LucianHasPassive
        {
            get { return ObjectManager.Player.Buffs.Any(buff => buff.Name == "lucianpassivebuff"); }
        }

        public static Obj_AI_Base QMinion
        {
            get
            {
                var vTarget = SimpleTs.GetTarget(Q2.Range, SimpleTs.DamageType.Physical);
                var vMinions = MinionManager.GetMinions(
                    ObjectManager.Player.ServerPosition, Q.Range, MinionTypes.All, MinionTeam.NotAlly,
                    MinionOrderTypes.None);

                return (from vMinion in vMinions.Where(vMinion => vMinion.IsValidTarget(Q.Range))
                    let endPoint =
                        vMinion.ServerPosition.To2D()
                            .Extend(ObjectManager.Player.ServerPosition.To2D(), -Q2.Range)
                            .To3D()
                    where
                        Intersection(
                            ObjectManager.Player.ServerPosition.To2D(), endPoint.To2D(), vTarget.ServerPosition.To2D(),
                            vTarget.BoundingRadius + Q.Width / 2)
                    select vMinion).FirstOrDefault();
            }
        }

        public override void Orbwalking_AfterAttack(Obj_AI_Base unit, Obj_AI_Base target)
        {
            if ((!ComboActive && !HarassActive) || !unit.IsMe || (!(target is Obj_AI_Hero)))
            {
                return;
            }

            if (GetValue<bool>("UseQ" + (ComboActive ? "C" : "H")) && Q.IsReady() && !LucianHasPassive)
            {
                Q.CastOnUnit(target);
            }
        }

        public override void Drawing_OnDraw(EventArgs args)
        {
            Spell[] spellList = { Q, W };
            foreach (var spell in spellList)
            {
                var menuItem = GetValue<Circle>("Draw" + spell.Slot);
                if (menuItem.Active && spell.Level > 0)
                {
                    Utility.DrawCircle(ObjectManager.Player.Position, spell.Range, menuItem.Color);
                }
            }
        }

        public static bool Intersection(Vector2 p1, Vector2 p2, Vector2 pC, float radius)
            /* Credits to DETUKS https://github.com/detuks/LeagueSharp/blob/master/YasuoSharp/YasMath.cs */
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

        public override void Game_OnGameUpdate(EventArgs args)
        {
            if (!ComboActive && !HarassActive)
            {
                return;
            }

            var useQ = GetValue<bool>("UseQ" + (ComboActive ? "C" : "H"));
            var useW = GetValue<bool>("UseW" + (ComboActive ? "C" : "H"));
            var useQExtended = GetValue<bool>("UseQExtended" + (ComboActive ? "C" : "H"));

            if (ObjectManager.Player.Spellbook.GetSpell(SpellSlot.R).Level > 0)
            {
                Config.Item("GHOSTBLADE")
                    .SetValue(ObjectManager.Player.Spellbook.GetSpell(SpellSlot.R).Name == "LucianR");
            }

            if (!Orbwalking.CanMove(100))
            {
                return;
            }

            if (Q.IsReady() && useQExtended)
            {
                var vTarget = Orbwalker.GetTarget() ?? SimpleTs.GetTarget(Q2.Range, SimpleTs.DamageType.Physical);

                if (vTarget != null && QMinion != null)
                {
                    Q.CastOnUnit(QMinion);
                }
            }
            else if (Q.IsReady() && useQ && !LucianHasPassive)
            {
                var vTarget = Orbwalker.GetTarget() ?? SimpleTs.GetTarget(Q.Range, SimpleTs.DamageType.Physical);
                if (vTarget != null)
                {
                    Q.Cast(vTarget);
                }
            }

            if (W.IsReady() && useW)
            {
                var vTarget = Orbwalker.GetTarget() ?? SimpleTs.GetTarget(W.Range, SimpleTs.DamageType.Physical);
                if (vTarget != null)
                {
                    if (ObjectManager.Player.Distance(vTarget) <= ObjectManager.Player.AttackRange)
                    {
                        if (!LucianHasPassive)
                        {
                            W.Cast(vTarget);
                        }
                    }
                    else
                    {
                        W.Cast(vTarget);
                    }
                }
            }
        }

        public override void ComboMenu(Menu config)
        {
            config.AddItem(new MenuItem("UseQC" + Id, "Use Q").SetValue(true));
            config.AddItem(new MenuItem("UseWC" + Id, "Use W").SetValue(true));
            config.AddItem(new MenuItem("UseQExtendedC" + Id, "Use Extended Q").SetValue(true));
        }

        public override void HarassMenu(Menu config)
        {
            config.AddItem(new MenuItem("UseQH" + Id, "Use Q").SetValue(true));
            config.AddItem(new MenuItem("UseWH" + Id, "Use W").SetValue(true));
            config.AddItem(new MenuItem("UseQExtendedH" + Id, "Use Extended Q").SetValue(true));
        }

        public override void MiscMenu(Menu config) { }

        public override void DrawingMenu(Menu config)
        {
            config.AddItem(
                new MenuItem("DrawQ" + Id, "Q range").SetValue(
                    new Circle(true, System.Drawing.Color.FromArgb(100, 255, 0, 255))));

            config.AddItem(
                new MenuItem("DrawW" + Id, "W range").SetValue(
                    new Circle(false, System.Drawing.Color.FromArgb(100, 255, 255, 255))));
        }
    }
}