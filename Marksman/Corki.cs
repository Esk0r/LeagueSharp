#region

using System;
using System.Linq;
using LeagueSharp;
using LeagueSharp.Common;

#endregion

namespace Marksman
{
    internal class Corki : Champion
    {
        public Spell E;
        public Spell Q;
        public Spell R1;
        public Spell R2;

        public Corki()
        {
            Utils.PrintMessage("Corki loaded");

            Q = new Spell(SpellSlot.Q, 825f);
            E = new Spell(SpellSlot.E, 600f);
            R1 = new Spell(SpellSlot.R, 1300f);
            R2 = new Spell(SpellSlot.R, 1500f);

            Q.SetSkillshot(0.3f, 120f, 1000f, false, SkillshotType.SkillshotCircle);
            E.SetSkillshot(0f, (float)(45 * Math.PI / 180), 1500, false, SkillshotType.SkillshotCone);
            R1.SetSkillshot(0.2f, 40f, 2000f, true, SkillshotType.SkillshotLine);
            R2.SetSkillshot(0.2f, 40f, 2000f, true, SkillshotType.SkillshotLine);
        }

        public override void Drawing_OnDraw(EventArgs args)
        {
            Spell[] spellList = { Q, E, R1 };
            foreach (var spell in spellList)
            {
                var menuItem = GetValue<Circle>("Draw" + spell.Slot);
                if (menuItem.Active)
                    Render.Circle.DrawCircle(ObjectManager.Player.Position, spell.Range, menuItem.Color);
            }
        }

        public override void Game_OnGameUpdate(EventArgs args)
        {
            if (R1.IsReady() && GetValue<bool>("UseRM"))
            {
                var bigRocket = HasBigRocket();
                foreach (
                    var hero in
                        ObjectManager.Get<Obj_AI_Hero>()
                            .Where(
                                hero =>
                                    hero.IsValidTarget(bigRocket ? R2.Range : R1.Range) &&
                                    R1.GetDamage(hero) * (bigRocket ? 1.5f : 1f) > hero.Health))
                {
                    if (bigRocket)
                        R2.Cast(hero, false, true);
                    else
                        R1.Cast(hero, false, true);
                }
            }

            if ((!ComboActive && !HarassActive) || !Orbwalking.CanMove(100)) return;

            var useQ = GetValue<bool>("UseQ" + (ComboActive ? "C" : "H"));
            var useE = GetValue<bool>("UseE" + (ComboActive ? "C" : "H"));
            var useR = GetValue<bool>("UseR" + (ComboActive ? "C" : "H"));
            var rLim = GetValue<Slider>("Rlim" + (ComboActive ? "C" : "H")).Value;

            if (useQ && Q.IsReady())
            {
                var t = TargetSelector.GetTarget(Q.Range, TargetSelector.DamageType.Magical);
                if (t != null)
                    if (Q.Cast(t, false, true) == Spell.CastStates.SuccessfullyCasted)
                        return;
            }

            if (useE && E.IsReady())
            {
                var t = TargetSelector.GetTarget(E.Range, TargetSelector.DamageType.Physical);
                if (t != null)
                    if (E.Cast(t, false, true) == Spell.CastStates.SuccessfullyCasted)
                        return;
            }

            if (useR && R1.IsReady() && ObjectManager.Player.Spellbook.GetSpell(SpellSlot.R).Ammo > rLim)
            {
                var bigRocket = HasBigRocket();
                var t = TargetSelector.GetTarget(bigRocket ? R2.Range : R1.Range, TargetSelector.DamageType.Magical);
                if (t != null)
                    if (bigRocket)
                        R2.Cast(t, false, true);
                    else
                        R1.Cast(t, false, true);
            }
        }

        public override void Orbwalking_AfterAttack(AttackableUnit unit, AttackableUnit target)
        {
            var t = target as Obj_AI_Hero;
            if (t == null || (!ComboActive && !HarassActive) || !unit.IsMe) 
                return;

            var useQ = GetValue<bool>("UseQ" + (ComboActive ? "C" : "H"));
            var useE = GetValue<bool>("UseE" + (ComboActive ? "C" : "H"));
            var useR = GetValue<bool>("UseR" + (ComboActive ? "C" : "H"));
            var rLim = GetValue<Slider>("Rlim" + (ComboActive ? "C" : "H")).Value;

            if (useQ && Q.IsReady())
                if (Q.Cast(t, false, true) == Spell.CastStates.SuccessfullyCasted)
                    return;

            if (useE && E.IsReady())
                if (E.Cast(t, false, true) == Spell.CastStates.SuccessfullyCasted)
                    return;

            if (useR && R1.IsReady() && ObjectManager.Player.Spellbook.GetSpell(SpellSlot.R).Ammo > rLim)
                if (HasBigRocket())
                    R2.Cast(t, false, true);
                else
                    R1.Cast(t, false, true);
        }

        public bool HasBigRocket()
        {
            return ObjectManager.Player.Buffs.Any(buff => buff.DisplayName.ToLower() == "corkimissilebarragecounterbig");
        }

        public override bool ComboMenu(Menu config)
        {
            config.AddItem(new MenuItem("UseQC" + Id, "Use Q").SetValue(true));
            config.AddItem(new MenuItem("UseEC" + Id, "Use E").SetValue(true));
            config.AddItem(new MenuItem("UseRC" + Id, "Use R").SetValue(true));
            config.AddItem(new MenuItem("RlimC" + Id, "Keep R Stacks").SetValue(new Slider(0, 0, 7)));
            return true;
        }

        public override bool HarassMenu(Menu config)
        {
            config.AddItem(new MenuItem("UseQH" + Id, "Use Q").SetValue(true));
            config.AddItem(new MenuItem("UseEH" + Id, "Use E").SetValue(false));
            config.AddItem(new MenuItem("UseRH" + Id, "Use R").SetValue(true));
            config.AddItem(new MenuItem("RlimH" + Id, "Keep R Stacks").SetValue(new Slider(3, 0, 7)));
            return true;
        }

        public override bool DrawingMenu(Menu config)
        {
            config.AddItem(
                new MenuItem("DrawQ" + Id, "Q range").SetValue(new Circle(true,
                    System.Drawing.Color.FromArgb(100, 255, 0, 255))));
            config.AddItem(
                new MenuItem("DrawE" + Id, "E range").SetValue(new Circle(false,
                    System.Drawing.Color.FromArgb(100, 255, 0, 255))));
            config.AddItem(
                new MenuItem("DrawR" + Id, "R range").SetValue(new Circle(false,
                    System.Drawing.Color.FromArgb(100, 255, 0, 255))));
            return true;
        }

        public override bool MiscMenu(Menu config)
        {
            config.AddItem(new MenuItem("UseRM" + Id, "Use R To Killsteal").SetValue(true));
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
