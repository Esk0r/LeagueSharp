#region

using System;
using System.Drawing;
using System.Linq;
using LeagueSharp;
using LeagueSharp.Common;

#endregion

namespace Marksman.Champions
{
    internal class MissFortune : Champion
    {
        public static Spell Q, W, E;

        public MissFortune()
        {
            Q = new Spell(SpellSlot.Q, 650);
            Q.SetTargetted(0.29f, 1400f);

            W = new Spell(SpellSlot.W);

            E = new Spell(SpellSlot.E, 800);
            E.SetSkillshot(0.5f, 100f, float.MaxValue, false, SkillshotType.SkillshotCircle);
            Utils.Utils.PrintMessage("MissFortune loaded.");
        }

        public override void Orbwalking_AfterAttack(AttackableUnit unit, AttackableUnit vTarget)
        {
            var t = vTarget as Obj_AI_Hero;
            if (t != null && (ComboActive || HarassActive) && unit.IsMe)
            {
                var useQ = GetValue<bool>("UseQ" + (ComboActive ? "C" : "H"));
                var useW = GetValue<bool>("UseW" + (ComboActive ? "C" : "H"));

                if (useQ)
                    Q.CastOnUnit(t);

                if (useW && W.IsReady())
                    W.CastOnUnit(ObjectManager.Player);
            }
        }

        public override void Drawing_OnDraw(EventArgs args)
        {
            Spell[] spellList = {Q, E};
            foreach (var spell in spellList)
            {
                var menuItem = GetValue<Circle>("Draw" + spell.Slot);
                if (menuItem.Active && spell.Level > 0)
                {
                    Render.Circle.DrawCircle(ObjectManager.Player.Position, spell.Range, menuItem.Color);
                }
            }
        }

        private static void CastQ()
        {
            if (!Q.IsReady())
                return;

            var t = TargetSelector.GetTarget(Q.Range + 450, TargetSelector.DamageType.Physical);
            if (t.IsValidTarget(Q.Range))
            {
                Q.CastOnUnit(t);
            }
        }

        public override void Game_OnGameUpdate(EventArgs args)
        {
            if (Q.IsReady() && GetValue<KeyBind>("UseQTH").Active)
            {
                if (ObjectManager.Player.HasBuff("Recall"))
                    return;
                CastQ();
            }

            if (E.IsReady() && GetValue<KeyBind>("UseETH").Active)
            {
                var t = TargetSelector.GetTarget(E.Range, TargetSelector.DamageType.Physical);
                if (t.IsValidTarget() && (t.HasBuffOfType(BuffType.Stun) || t.HasBuffOfType(BuffType.Snare) ||
                                          t.HasBuffOfType(BuffType.Charm) || t.HasBuffOfType(BuffType.Fear) ||
                                          t.HasBuffOfType(BuffType.Taunt) || t.HasBuff("zhonyasringshield") ||
                                          t.HasBuff("Recall")))
                {
                    E.CastIfHitchanceEquals(t, HitChance.Low);
                }
            }

            if (ComboActive || HarassActive)
            {
                var useQ = GetValue<bool>("UseQ" + (ComboActive ? "C" : "H"));
                var useE = GetValue<bool>("UseE" + (ComboActive ? "C" : "H"));

                if (Q.IsReady() && useQ)
                {
                    CastQ();
                }

                if (E.IsReady() && useE)
                {
                    var t = TargetSelector.GetTarget(E.Range, TargetSelector.DamageType.Physical);
                    if (ObjectManager.Player.Distance(t) > 600)
                        E.CastIfHitchanceEquals(t, t.Path.Count() > 1 ? HitChance.High : HitChance.Medium);
                    else
                        E.CastIfHitchanceEquals(t, HitChance.Low);
                }
            }

            if (LaneClearActive)
            {
                var useQ = GetValue<bool>("UseQL");

                if (Q.IsReady() && useQ)
                {
                    var vMinions = MinionManager.GetMinions(ObjectManager.Player.Position, Q.Range);
                    foreach (
                        var minions in
                            vMinions.Where(
                                minions =>
                                    minions.Health < ObjectManager.Player.GetSpellDamage(minions, SpellSlot.Q) - 20))
                        Q.Cast(minions);
                }
            }
        }

        public override bool ComboMenu(Menu config)
        {
            config.AddItem(new MenuItem("UseQC" + Id, "Use Q").SetValue(true));
            config.AddItem(new MenuItem("UseWC" + Id, "Use W").SetValue(true));
            config.AddItem(new MenuItem("UseEC" + Id, "Use E").SetValue(true));

            return true;
        }

        public override bool HarassMenu(Menu config)
        {
            config.AddItem(new MenuItem("UseQH" + Id, "Use Q").SetValue(true));
            config.AddItem(new MenuItem("UseWH" + Id, "Use W").SetValue(true));
            config.AddItem(new MenuItem("UseEH" + Id, "Use E").SetValue(true));
            config.AddItem(
                new MenuItem("UseQTH" + Id, "Use Q (Toggle)").SetValue(new KeyBind("H".ToCharArray()[0],
                    KeyBindType.Toggle)));
            config.AddItem(
                new MenuItem("UseETH" + Id, "Use E (Toggle)").SetValue(new KeyBind("T".ToCharArray()[0],
                    KeyBindType.Toggle)));

            return true;
        }

        public override bool DrawingMenu(Menu config)
        {
            config.AddItem(
                new MenuItem("DrawQ" + Id, "Q range").SetValue(
                    new Circle(true, Color.FromArgb(100, 255, 0, 255))));

            config.AddItem(
                new MenuItem("DrawE" + Id, "E range").SetValue(
                    new Circle(false, Color.FromArgb(100, 255, 255, 255))));

            return true;
        }

        public override bool ExtrasMenu(Menu config)
        {
            return true;
        }

        public override bool LaneClearMenu(Menu config)
        {
            config.AddItem(new MenuItem("UseQL" + Id, "Use Q").SetValue(true));
            return true;
        }
    }
}