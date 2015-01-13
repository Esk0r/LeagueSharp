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
        public static Spell Q;
        public static Spell R;
        public static Spell W;

        public static Items.Item Dfg = new Items.Item(3128, 750);

        public Ezreal()
        {
            Utils.PrintMessage("Ezreal loaded.");

            Q = new Spell(SpellSlot.Q, 1190);
            Q.SetSkillshot(0.25f, 60f, 2000f, true, SkillshotType.SkillshotLine);

            W = new Spell(SpellSlot.W, 800);
            W.SetSkillshot(0.25f, 80f, 1600f, false, SkillshotType.SkillshotLine);

            R = new Spell(SpellSlot.R, 2500);
            R.SetSkillshot(1f, 160f, 2000f, false, SkillshotType.SkillshotLine);

            Utility.HpBarDamageIndicator.DamageToUnit = GetComboDamage;
            Utility.HpBarDamageIndicator.Enabled = true;
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

            if (Q.IsReady() && GetValue<KeyBind>("UseQTH").Active && ToggleActive)
            {
                if (ObjectManager.Player.HasBuff("Recall"))
                    return;

                t = TargetSelector.GetTarget(Q.Range, TargetSelector.DamageType.Physical);

                var useQt = (Program.Config.Item("DontQToggleHarass" + t.ChampionName) != null &&
                             Program.Config.Item("DontQToggleHarass" + t.ChampionName).GetValue<bool>() == false);
                if (t != null && useQt)
                    Q.Cast(t);
            }

            if (W.IsReady() && GetValue<KeyBind>("UseWTH").Active && ToggleActive)
            {
                if (ObjectManager.Player.HasBuff("Recall"))
                    return;
                t = TargetSelector.GetTarget(W.Range, TargetSelector.DamageType.Physical);
                var useWt = (Program.Config.Item("DontWToggleHarass" + t.ChampionName) != null &&
                             Program.Config.Item("DontWToggleHarass" + t.ChampionName).GetValue<bool>() == false);
                if (t != null && useWt)
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
                    foreach (Obj_AI_Base minions in
                        vMinions.Where(
                            minions => minions.Health < ObjectManager.Player.GetSpellDamage(minions, SpellSlot.Q)))
                        Q.Cast(minions);
                }
            }


            if (!R.IsReady() || !GetValue<KeyBind>("CastR").Active)
                return;
            t = TargetSelector.GetTarget(R.Range, TargetSelector.DamageType.Physical);
            if (t != null)
                R.Cast(t);
        }

        private static float GetComboDamage(Obj_AI_Hero t)
        {
            float fComboDamage = 0f;

            if (Q.IsReady())
                fComboDamage += (float) ObjectManager.Player.GetSpellDamage(t, SpellSlot.Q);

            if (W.IsReady())
                fComboDamage += (float) ObjectManager.Player.GetSpellDamage(t, SpellSlot.W);

            if (R.IsReady())
                fComboDamage += (float) ObjectManager.Player.GetSpellDamage(t, SpellSlot.R);

            if (ObjectManager.Player.GetSpellSlot("summonerdot") != SpellSlot.Unknown &&
                ObjectManager.Player.Spellbook.CanUseSpell(ObjectManager.Player.GetSpellSlot("summonerdot")) ==
                SpellState.Ready && ObjectManager.Player.Distance(t) < 550)
                fComboDamage += (float) ObjectManager.Player.GetSummonerSpellDamage(t, Damage.SummonerSpell.Ignite);

            if (Items.CanUseItem(3144) && ObjectManager.Player.Distance(t) < 550)
                fComboDamage += (float) ObjectManager.Player.GetItemDamage(t, Damage.DamageItems.Bilgewater);

            if (Items.CanUseItem(3153) && ObjectManager.Player.Distance(t) < 550)
                fComboDamage += (float) ObjectManager.Player.GetItemDamage(t, Damage.DamageItems.Botrk);

            if (Items.CanUseItem(3128) && ObjectManager.Player.Distance(t) < 550)
                fComboDamage += (float) ObjectManager.Player.GetItemDamage(t, Damage.DamageItems.Dfg);

            return fComboDamage;
        }

        public override bool ComboMenu(Menu config)
        {
            config.AddItem(new MenuItem("UseQC" + Id, "Q").SetValue(true));
            config.AddItem(new MenuItem("UseWC" + Id, "W").SetValue(true));
            return true;
        }

        public override bool HarassMenu(Menu config)
        {
            config.AddItem(new MenuItem("UseQH" + Id, "Q").SetValue(true));
            config.AddItem(new MenuItem("UseWH" + Id, "W").SetValue(true));

            config.AddSubMenu(new Menu("Don't Q Toggle to", "DontQToggleHarass"));
            {
                foreach (
                    var enemy in
                        ObjectManager.Get<Obj_AI_Hero>().Where(enemy => enemy.Team != ObjectManager.Player.Team))
                {
                    config.SubMenu("DontQToggleHarass")
                        .AddItem(
                            new MenuItem("DontQToggleHarass" + enemy.ChampionName, enemy.ChampionName).SetValue(false));
                }
            }

            config.AddSubMenu(new Menu("Don't W Toggle to", "DontWToggleHarass"));
            {
                foreach (
                    var enemy in
                        ObjectManager.Get<Obj_AI_Hero>().Where(enemy => enemy.Team != ObjectManager.Player.Team))
                {
                    config.SubMenu("DontWToggleHarass")
                        .AddItem(
                            new MenuItem("DontWToggleHarass" + enemy.ChampionName, enemy.ChampionName).SetValue(false));
                }
            }

            config.AddItem(
                new MenuItem("UseQTH" + Id, "Q (Toggle)").SetValue(
                    new KeyBind("H".ToCharArray()[0], KeyBindType.Toggle)));
            config.AddItem(
                new MenuItem("UseWTH" + Id, "W (Toggle)").SetValue(
                    new KeyBind("J".ToCharArray()[0], KeyBindType.Toggle)));
            return true;
        }

        public override bool MiscMenu(Menu config)
        {
            config.AddItem(
                new MenuItem("CastR" + Id, "Cast R (2000 Range)").SetValue(
                    new KeyBind("T".ToCharArray()[0], KeyBindType.Press)));
            return true;
        }


        public override bool DrawingMenu(Menu config)
        {
            config.AddItem(
                new MenuItem("DrawQ" + Id, "Q range").SetValue(new Circle(true, Color.FromArgb(100, 255, 0, 255))));
            config.AddItem(
                new MenuItem("DrawW" + Id, "W range").SetValue(new Circle(false, Color.FromArgb(100, 255, 255, 255))));

            var dmgAfterComboItem = new MenuItem("DamageAfterCombo", "Damage After Combo").SetValue(true);
            Config.AddItem(dmgAfterComboItem);

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