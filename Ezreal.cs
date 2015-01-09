#region

using System;
using System.Configuration;
using System.Drawing;
using System.Linq;
using LeagueSharp;
using LeagueSharp.Common;

#endregion

namespace Marksman
{
    internal class Ezreal : Champion
    {
        public Spell Q;
        public Spell R;
        public Spell W;

        public static Items.Item Dfg = new Items.Item(3128, 750);
        public Ezreal()
        {
            Utils.PrintMessage("Ezreal loaded.");

            Q = new Spell(SpellSlot.Q, 1200);
            Q.SetSkillshot(0.25f, 60f, 2000f, true, SkillshotType.SkillshotLine);

            W = new Spell(SpellSlot.W, 850);
            W.SetSkillshot(0.25f, 80f, 1600f, false, SkillshotType.SkillshotLine);

            R = new Spell(SpellSlot.R, 2500);
            R.SetSkillshot(1f, 160f, 2000f, false, SkillshotType.SkillshotLine);

        }

        public override void Orbwalking_AfterAttack(AttackableUnit unit, AttackableUnit target)
        {
            var t = target as Obj_AI_Hero;
            if (t != null && (ComboActive || HarassActive) && unit.IsMe)
            {
                var useQ = GetValue<bool>("UseQ" + (ComboActive ? "C" : "H"));
                var useW = GetValue<bool>("UseW" + (ComboActive ? "C" : "H"));

                if (Q.IsReady() && useQ)
                {
                    Q.Cast(t);
                }
                else if (W.IsReady() && useW)
                {
                    W.Cast(t);
                }
            }
        }

        public override void Drawing_OnDraw(EventArgs args)
        {
            Spell[] spellList = { Q, W };
            foreach (var spell in spellList)
            {
                var menuItem = GetValue<Circle>("Draw" + spell.Slot);
                if (menuItem.Active)
                    Render.Circle.DrawCircle(ObjectManager.Player.Position, spell.Range, menuItem.Color);
            }
        }

        public override void Game_OnGameUpdate(EventArgs args)
        {
            Obj_AI_Hero t;

            if (Q.IsReady() &&  GetValue<KeyBind>("UseQTH").Active && ToggleActive)
            {
                if(ObjectManager.Player.HasBuff("Recall"))
                    return;
                t = TargetSelector.GetTarget(Q.Range, TargetSelector.DamageType.Physical);
                if (t != null)
                    Q.Cast(t);
            }

            if (W.IsReady() &&  GetValue<KeyBind>("UseWTH").Active && ToggleActive)
            {
                if(ObjectManager.Player.HasBuff("Recall"))
                    return;
                t = TargetSelector.GetTarget(W.Range, TargetSelector.DamageType.Physical);
                if (t != null)
                    W.Cast(t);
            }

            if (ComboActive || HarassActive)
            {
                var useQ = GetValue<bool>("UseQ" + (ComboActive ? "C" : "H"));
                var useW = GetValue<bool>("UseW" + (ComboActive ? "C" : "H"));

                if (Orbwalking.CanMove(100))
                {
                    if (Dfg.IsReady())
                    {
                        t = TargetSelector.GetTarget(W.Range, TargetSelector.DamageType.Magical);
                        Dfg.Cast(t);
                    }

                    if (Q.IsReady() && useQ)
                    {
                        t = TargetSelector.GetTarget(Q.Range, TargetSelector.DamageType.Physical);
                        if (t != null)
                            Q.Cast(t);
                    }

                    if (W.IsReady() && useW)
                    {
                        t = TargetSelector.GetTarget(W.Range, TargetSelector.DamageType.Physical);
                        if (t != null)
                            W.Cast(t);
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


            if (!R.IsReady() || !GetValue<KeyBind>("CastR").Active) return;
            t = TargetSelector.GetTarget(R.Range, TargetSelector.DamageType.Physical);
            if (t != null)
                R.Cast(t);
        }

        public override bool ComboMenu(Menu config)
        {
            config.AddItem(new MenuItem("UseQC" + Id, "Use Q").SetValue(true));
            config.AddItem(new MenuItem("UseWC" + Id, "Use W").SetValue(true));
            return true;
        }

        public override bool HarassMenu(Menu config)
        {
            config.AddItem(new MenuItem("UseQH" + Id, "Use Q").SetValue(true));
            config.AddItem(new MenuItem("UseWH" + Id, "Use W").SetValue(true));
            config.AddItem(
                new MenuItem("UseQTH" + Id, "Use Q (Toggle)").SetValue(new KeyBind("H".ToCharArray()[0],
                    KeyBindType.Toggle)));
            config.AddItem(
                new MenuItem("UseWTH" + Id, "Use W (Toggle)").SetValue(new KeyBind("J".ToCharArray()[0],
                    KeyBindType.Toggle)));
            return true;
        }

        public override bool MiscMenu(Menu config)
        {
            config.AddItem(
                new MenuItem("CastR" + Id, "Cast R (2000 Range)").SetValue(new KeyBind("T".ToCharArray()[0],
                    KeyBindType.Press)));
            return true;
        }

        public override bool DrawingMenu(Menu config)
        {
            config.AddItem(
                new MenuItem("DrawQ" + Id, "Q range").SetValue(new Circle(true, Color.FromArgb(100, 255, 0, 255))));
            config.AddItem(
                new MenuItem("DrawW" + Id, "W range").SetValue(new Circle(false, Color.FromArgb(100, 255, 255, 255))));
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
