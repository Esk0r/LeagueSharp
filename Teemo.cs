#region

using System;
using System.Drawing;
using System.Linq;
using LeagueSharp;
using LeagueSharp.Common;

#endregion

namespace Marksman
{
    internal class Teemo : Champion
    {
        public Spell Q;
        public Spell R;

        public Teemo()
        {
            Utils.PrintMessage("Teemo loaded.");

            Q = new Spell(SpellSlot.Q, 680);
            R = new Spell(SpellSlot.R, 230);
            Q.SetTargetted(0f, 2000f);
            R.SetSkillshot(0.1f, 75f, float.MaxValue, false, SkillshotType.SkillshotCircle);
        }

        public override void Orbwalking_AfterAttack(AttackableUnit unit, AttackableUnit target)
        {
            var t = target as Obj_AI_Hero;
            if (t != null && (ComboActive || HarassActive) && unit.IsMe)
            {
                var useQ = GetValue<bool>("UseQ" + (ComboActive ? "C" : "H"));

                if (useQ && Q.IsReady())
                    Q.CastOnUnit(t);
            }
        }

        public override void Drawing_OnDraw(EventArgs args)
        {
            Spell[] spellList = { Q };
            foreach (var spell in spellList)
            {
                var menuItem = GetValue<Circle>("Draw" + spell.Slot);
                if (menuItem.Active)
                    Render.Circle.DrawCircle(ObjectManager.Player.Position, spell.Range, menuItem.Color);
            }
        }

        public override void Game_OnGameUpdate(EventArgs args)
        {
            if (Q.IsReady() && GetValue<KeyBind>("UseQTH").Active && ToggleActive)
            {
                if(ObjectManager.Player.HasBuff("Recall"))
                    return;
                var qTarget = TargetSelector.GetTarget(Q.Range, TargetSelector.DamageType.Physical);
                if (Q.IsReady() && qTarget.IsValidTarget())
                    Q.CastOnUnit(qTarget);
            }           
                        
            if (Orbwalking.CanMove(100) && (ComboActive || HarassActive))
            {
                var useQ = GetValue<bool>("UseQ" + (ComboActive ? "C" : "H"));
                if (useQ)
                {
                    var qTarget = TargetSelector.GetTarget(Q.Range, TargetSelector.DamageType.Physical);
                    if (Q.IsReady() && qTarget.IsValidTarget())
                        Q.CastOnUnit(qTarget);
                }
            }

            if (GetValue<bool>("UseQM") && Q.IsReady())
            {
                foreach (
                    var hero in
                        ObjectManager.Get<Obj_AI_Hero>()
                            .Where(
                                hero =>
                                    hero.IsValidTarget(Q.Range) &&
                                    ObjectManager.Player.GetSpellDamage(hero, SpellSlot.Q) - 20 > hero.Health))
                    Q.CastOnUnit(hero);
            }

            if (GetValue<bool>("UseRC") && R.IsReady() && ComboActive)
            {
                foreach (
                    var hero in
                    ObjectManager.Get<Obj_AI_Hero>()
                        .Where(
                            hero =>
                                hero.IsValidTarget(R.Range)))
                    R.Cast(hero, false, true);
                
                if (R.IsReady() && GetValue<bool>("AutoRI"))
                {
                    var t = TargetSelector.GetTarget(R.Range, TargetSelector.DamageType.Physical);
                    if (t.IsValidTarget(R.Range) &&
                        (t.HasBuffOfType(BuffType.Stun) || t.HasBuffOfType(BuffType.Snare) ||
                        t.HasBuffOfType(BuffType.Taunt) || t.HasBuff("zhonyasringshield") ||
                        t.HasBuff("Recall")))
                    {
                        R.Cast(t.Position);
                    }                
                }                    
            }
            

            if (LaneClearActive)
            {
                bool useQ = GetValue<bool>("UseQL");

                if (Q.IsReady() && useQ)
                {
                    var vMinions = MinionManager.GetMinions(ObjectManager.Player.Position, Q.Range);
                    foreach (
                        Obj_AI_Base minions in
                            vMinions.Where(
                                minions => minions.Health < ObjectManager.Player.GetSpellDamage(minions, SpellSlot.Q)))
                        Q.Cast(minions);
                }
            }
        }

        public override bool ComboMenu(Menu config)
        {
            config.AddItem(new MenuItem("UseQC" + Id, "Use Q").SetValue(true));
            config.AddItem(new MenuItem("UseRC" + Id, "Use R").SetValue(false));
            return true;
        }

        public override bool HarassMenu(Menu config)
        {
            config.AddItem(new MenuItem("UseQH" + Id, "Use Q").SetValue(false));
            config.AddItem(
                new MenuItem("UseQTH" + Id, "Use Q (Toggle)").SetValue(new KeyBind("H".ToCharArray()[0],
                    KeyBindType.Toggle)));             
            return true;
        }

        public override bool DrawingMenu(Menu config)
        {
            config.AddItem(
                new MenuItem("DrawQ" + Id, "Q range").SetValue(new Circle(true, Color.FromArgb(100, 255, 0, 255))));
            return true;
        }

        public override bool MiscMenu(Menu config)
        {
            config.AddItem(new MenuItem("UseQM" + Id, "Use Q KS").SetValue(true));
            return true;
        }

        public override bool ExtrasMenu(Menu config)
        {
            config.AddItem(new MenuItem("AutoRI" + Id, "Use R").SetValue(true));
            return true;
        }

        public override bool LaneClearMenu(Menu config)
        {
            config.AddItem(new MenuItem("UseQL" + Id, "Use Q").SetValue(true));
            return true;
        }
    }
}
