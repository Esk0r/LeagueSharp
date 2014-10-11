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

        public static bool DoubleHit = false;

        public Lucian()
        {
            Utils.PrintMessage("Lucian loaded.");

            Q = new Spell(SpellSlot.Q, 675);
            Q2 = new Spell(SpellSlot.Q, 1100);
            W = new Spell(SpellSlot.W, 1000);

            Q.SetSkillshot(0.25f, 65f, 1100f, false, SkillshotType.SkillshotLine);
            W.SetSkillshot(0.30f, 80f, 1600f, true, SkillshotType.SkillshotLine);

            Obj_AI_Base.OnProcessSpellCast += Game_OnProcessSpell;
        }

        public void Game_OnProcessSpell(Obj_AI_Base unit, GameObjectProcessSpellCastEventArgs spell)
        {
            if (!unit.IsMe) return;
            if (spell.SData.Name.Contains("summoner")) return;

            if (spell.SData.Name.Contains("Attack"))
            {
                Utility.DelayAction.Add(50, () =>
                {
                    DoubleHit = false;
                    Utility.DelayAction.ActionList.Clear();
                });

            }
            else if (spell.SData.Name.Contains("Lucian") && !spell.SData.Name.Contains("Attack"))
            {
                Orbwalking.ResetAutoAttackTimer();

                Utility.DelayAction.Add(6000, () =>
                {
                    if (DoubleHit)
                        DoubleHit = false;
                });

                DoubleHit = true;
            }
        }

        public bool LucianHasPassive()
        {
            return Config.Item("Passive" + Id).GetValue<bool>() && DoubleHit;
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

        public override void Drawing_OnDraw(EventArgs args)
        {
            Spell[] spellList = { Q, W };
            foreach (var spell in spellList)
            {
                var menuItem = GetValue<Circle>("Draw" + spell.Slot);
                if (!menuItem.Active || spell.Level < 0) return;

                Utility.DrawCircle(ObjectManager.Player.Position, spell.Range, menuItem.Color);
            }

            if (!GetValue<Circle>("DrawQ2").Active && Q.Level < 0) return;

            Utility.DrawCircle(ObjectManager.Player.Position, Q2.Range, GetValue<Circle>("DrawQ2").Color);
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
            var mana = ObjectManager.Player.MaxMana * (Config.Item("ManaH" + Id).GetValue<Slider>().Value / 100.0);

            if ((!ComboActive && !HarassActive) || (HarassActive && !(ObjectManager.Player.Mana > mana))) return;

            var useQ = Config.Item("UseQ" + (ComboActive ? "C" : "H") + Id).GetValue<bool>();
            var useW = Config.Item("UseW" + (ComboActive ? "C" : "H") + Id).GetValue<bool>();
            var useQExtended = Config.Item("UseQExtended" + (ComboActive ? "C" : "H") + Id).GetValue<bool>();

            if (ObjectManager.Player.Spellbook.GetSpell(SpellSlot.R).Level > 0)
                Config.Item("GHOSTBLADE")
                    .SetValue(ObjectManager.Player.Spellbook.GetSpell(SpellSlot.R).Name == "LucianR");

            if (useQExtended)
            {
                var vTarget = SimpleTs.GetTarget(Q2.Range, SimpleTs.DamageType.Physical);

                if (vTarget.IsValidTarget() && QMinion.IsValidTarget())
                {
                    if (ObjectManager.Player.Distance(vTarget) > ObjectManager.Player.AttackRange)
                        Q.CastOnUnit(QMinion);
                }
            }

            if (useQ)
            {
                var vTarget = SimpleTs.GetTarget(Q.Range, SimpleTs.DamageType.Physical);

                if (vTarget.IsValidTarget())
                {
                    if (!LucianHasPassive())
                        Q.CastOnUnit(vTarget);
                }
            }

            if (useW)
            {
                var vTarget = SimpleTs.GetTarget(W.Range, SimpleTs.DamageType.Physical);

                if (vTarget.IsValidTarget())
                {
                    if (ObjectManager.Player.Distance(vTarget) <= ObjectManager.Player.AttackRange)
                    {
                        if (!LucianHasPassive())
                            W.Cast(vTarget);
                    }
                    else
                        W.Cast(vTarget);
                }
            }
        }

        public override bool ComboMenu(Menu config)
        {
            config.AddItem(new MenuItem("UseQC" + Id, "Use Q").SetValue(true));
            config.AddItem(new MenuItem("UseWC" + Id, "Use W").SetValue(true));
            config.AddItem(new MenuItem("UseQExtendedC" + Id, "Use Extended Q").SetValue(true));
            return true;
        }

        public override bool HarassMenu(Menu config)
        {
            config.AddItem(new MenuItem("UseQH" + Id, "Use Q").SetValue(true));
            config.AddItem(new MenuItem("UseWH" + Id, "Use W").SetValue(true));
            config.AddItem(new MenuItem("UseQExtendedH" + Id, "Use Extended Q").SetValue(true));
            config.AddItem(new MenuItem("ManaH" + Id, "Min Mana").SetValue(new Slider(50)));
            return true;
        }

        public override bool MiscMenu(Menu config)
        {
            config.AddItem(new MenuItem("Passive" + Id, "Take in consideration Passive").SetValue(true));
            return true;
        }

        public override bool DrawingMenu(Menu config)
        {
            config.AddItem(new MenuItem("DrawQ" + Id, "Q range").SetValue(new Circle(true, Color.CornflowerBlue)));
            config.AddItem(new MenuItem("DrawQ2" + Id, "Extended Q range").SetValue(new Circle(true, Color.CornflowerBlue)));
            config.AddItem(new MenuItem("DrawW" + Id, "W range").SetValue(new Circle(false, Color.CornflowerBlue)));
            return true;
        }
    }
}
