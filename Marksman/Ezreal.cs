#region
using System;
using System.Linq;
using LeagueSharp;
using LeagueSharp.Common;
using SharpDX.Direct3D9;
using Font = SharpDX.Direct3D9.Font;
#endregion

namespace Marksman
{
    internal class Ezreal : Champion
    {
        public static Spell Q;
        public static Spell E;
        public static Spell W;
        public static Spell R;
        public static Font vText;

        public Ezreal()
        {
            Q = new Spell(SpellSlot.Q, 1190);
            Q.SetSkillshot(0.25f, 60f, 2000f, true, SkillshotType.SkillshotLine);

            W = new Spell(SpellSlot.W, 800);
            W.SetSkillshot(0.25f, 80f, 1600f, false, SkillshotType.SkillshotLine);

            E = new Spell(SpellSlot.E);
            
            R = new Spell(SpellSlot.R, 2500);
            R.SetSkillshot(1f, 160f, 2000f, false, SkillshotType.SkillshotLine);

            Utility.HpBarDamageIndicator.DamageToUnit = GetComboDamage;
            Utility.HpBarDamageIndicator.Enabled = true;

            vText = new Font(
                Drawing.Direct3DDevice,
                new FontDescription
                {
                    FaceName = "Courier new",
                    Height = 15,
                    OutputPrecision = FontPrecision.Default,
                    Quality = FontQuality.Default,
                });

            Utils.PrintMessage("Ezreal loaded.");
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

            var drawRMin = Program.Config.SubMenu("Combo").Item("DrawRMin").GetValue<Circle>();
            if (drawRMin.Active)
            {
                var minRRange = Program.Config.SubMenu("Combo").Item("UseRCMinRange").GetValue<Slider>().Value;
                Render.Circle.DrawCircle(ObjectManager.Player.Position, minRRange, drawRMin.Color, 2);
            }

            var drawRMax = Program.Config.SubMenu("Combo").Item("DrawRMax").GetValue<Circle>();
            if (drawRMax.Active)
            {
                var maxRRange = Program.Config.SubMenu("Combo").Item("UseRCMaxRange").GetValue<Slider>().Value;
                Render.Circle.DrawCircle(ObjectManager.Player.Position, maxRRange, drawRMax.Color, 2);
            }

            if (Program.Config.Item("DrawHarassToggleStatus").GetValue<bool>())
            {
                DrawHarassToggleStatus();
            }

            if (Program.Config.Item("ShowKillableStatus").GetValue<bool>())
            {
                ShowKillableStatus();
            }
        }

        public override void Game_OnGameUpdate(EventArgs args)
        {
            Obj_AI_Hero t;

            if (Q.IsReady() && Program.Config.Item("UseQTH").GetValue<KeyBind>().Active && ToggleActive)
            {
                if (ObjectManager.Player.HasBuff("Recall"))
                    return;

                t = TargetSelector.GetTarget(Q.Range, TargetSelector.DamageType.Physical);

                var useQt = (Program.Config.Item("DontQToggleHarass" + t.ChampionName) != null &&
                             Program.Config.Item("DontQToggleHarass" + t.ChampionName).GetValue<bool>() == false);
                if (useQt)
                    Q.Cast(t);
            }

            if (W.IsReady() && Program.Config.Item("UseWTH").GetValue<KeyBind>().Active && ToggleActive)
            {
                if (ObjectManager.Player.HasBuff("Recall"))
                    return;
                t = TargetSelector.GetTarget(W.Range, TargetSelector.DamageType.Physical);
                var useWt = (Program.Config.Item("DontWToggleHarass" + t.ChampionName) != null &&
                             Program.Config.Item("DontWToggleHarass" + t.ChampionName).GetValue<bool>() == false);
                if (useWt)
                    W.Cast(t);
            }

            if (ComboActive || HarassActive)
            {
                var useQ = GetValue<bool>("UseQ" + (ComboActive ? "C" : "H"));
                var useW = GetValue<bool>("UseW" + (ComboActive ? "C" : "H"));
                var useR = Program.Config.SubMenu("Combo").Item("UseRC").GetValue<bool>();

                if (Orbwalking.CanMove(100))
                {
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

                    if (R.IsReady() && useR)
                    {
                        var minRRange = Program.Config.SubMenu("Combo").Item("UseRCMinRange").GetValue<Slider>().Value;
                        var maxRRange = Program.Config.SubMenu("Combo").Item("UseRCMaxRange").GetValue<Slider>().Value;

                        t = TargetSelector.GetTarget(maxRRange, TargetSelector.DamageType.Physical);

                        if (Q.IsReady() && t.IsValidTarget(Q.Range) && Q.GetPrediction(t).CollisionObjects.Count == 0 &&
                            t.Health < ObjectManager.Player.GetSpellDamage(t, SpellSlot.Q))
                            return;
                        
                        if (t.IsValidTarget() && ObjectManager.Player.Distance(t) >= minRRange &&
                            t.Health <= ObjectManager.Player.GetSpellDamage(t, SpellSlot.R))
                        {
                            R.Cast(t);
                        }
                    }
                }
            }

            if (LaneClearActive)
            {
                var useQ = GetValue<bool>("UseQL");

                if (Q.IsReady() && useQ)
                {
                    var vMinions = MinionManager.GetMinions(ObjectManager.Player.Position, Q.Range);
                    foreach (Obj_AI_Base minions in
                        vMinions.Where(
                            minions => minions.Health < ObjectManager.Player.GetSpellDamage(minions, SpellSlot.Q)))
                        Q.Cast(minions);
                }
            }

            if (R.IsReady() && GetValue<KeyBind>("CastR").Active)
            {
                t = TargetSelector.GetTarget(R.Range, TargetSelector.DamageType.Physical);
                if (t.IsValidTarget())
                    R.Cast(t);
            }
        }

        private static float GetComboDamage(Obj_AI_Hero t)
        {
            var fComboDamage = 0f;

            if (Q.IsReady())
                fComboDamage += (float) ObjectManager.Player.GetSpellDamage(t, SpellSlot.Q);

            if (W.IsReady())
                fComboDamage += (float) ObjectManager.Player.GetSpellDamage(t, SpellSlot.W);

            if (E.IsReady())
                fComboDamage += (float) ObjectManager.Player.GetSpellDamage(t, SpellSlot.E);

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

            return fComboDamage;
        }

        public override bool ComboMenu(Menu config)
        {
            config.AddItem(new MenuItem("UseQC" + Id, "Q").SetValue(true));
            config.AddItem(new MenuItem("UseWC" + Id, "W").SetValue(true));

            var xRMenu = new Menu("R", "ComboR");
            {
                xRMenu.AddItem(new MenuItem("UseRC", "Use").SetValue(true));
                xRMenu.AddItem(new MenuItem("UseRCMinRange", "Min. Range").SetValue(new Slider(200, 200, 1000)));
                xRMenu.AddItem(new MenuItem("UseRCMaxRange", "Max. Range").SetValue(new Slider(500, 500, 2000)));
                xRMenu.AddItem(
                    new MenuItem("DrawRMin", "Draw Min. R Range").SetValue(
                        new Circle(true, System.Drawing.Color.DarkRed)));
                xRMenu.AddItem(
                    new MenuItem("DrawRMax", "Draw Max. R Range").SetValue(
                        new Circle(true, System.Drawing.Color.DarkMagenta)));

                config.AddSubMenu(xRMenu);
            }
            return true;
        }

        public override bool HarassMenu(Menu config)
        {
            config.AddItem(new MenuItem("UseQH" + Id, "Q").SetValue(true));
            config.AddItem(new MenuItem("UseWH" + Id, "W").SetValue(true));

            config.AddSubMenu(new Menu("Don't Q Toggle to", "DontQToggleHarass"));
            config.AddSubMenu(new Menu("Don't W Toggle to", "DontWToggleHarass"));
            {
                foreach (var enemy in
                    ObjectManager.Get<Obj_AI_Hero>().Where(enemy => enemy.Team != ObjectManager.Player.Team))
                {
                    config.SubMenu("DontQToggleHarass")
                        .AddItem(
                            new MenuItem("DontQToggleHarass" + enemy.ChampionName, enemy.ChampionName).SetValue(false));

                    config.SubMenu("DontWToggleHarass")
                        .AddItem(
                            new MenuItem("DontWToggleHarass" + enemy.ChampionName, enemy.ChampionName).SetValue(false));
                }
            }

            config.AddItem(
                new MenuItem("UseQTH", "Q (Toggle)").SetValue(new KeyBind("H".ToCharArray()[0], KeyBindType.Toggle)));
            config.AddItem(
                new MenuItem("UseWTH", "W (Toggle)").SetValue(new KeyBind("J".ToCharArray()[0], KeyBindType.Toggle)));
            config.AddItem(new MenuItem("DrawHarassToggleStatus", "Draw Toggle Status").SetValue(true));
            return true;
        }

        public override bool MiscMenu(Menu config)
        {
            config.AddItem(
                new MenuItem("CastR" + Id, "Cast R (2000 Range)").SetValue(
                    new KeyBind("T".ToCharArray()[0], KeyBindType.Press)));
            return true;
        }

        private static void DrawHarassToggleStatus()
        {
            var xHarassStatus = "";
            if (Program.Config.Item("UseQTH").GetValue<KeyBind>().Active)
                xHarassStatus += "Q - ";

            if (Program.Config.Item("UseWTH").GetValue<KeyBind>().Active)
                xHarassStatus += "W - ";

            if (xHarassStatus.Length < 1)
            {
                xHarassStatus = "";
            }
            else
            {
                xHarassStatus = "Toggle: " + xHarassStatus;
            }

            xHarassStatus = xHarassStatus.Substring(0, xHarassStatus.Length - 3);

            Utils.DrawText(
                vText, xHarassStatus, (int) ObjectManager.Player.HPBarPosition.X + 145,
                (int) ObjectManager.Player.HPBarPosition.Y + 5, SharpDX.Color.White);
        }

        private static void ShowKillableStatus()
        {
            var t = TargetSelector.GetTarget(2000, TargetSelector.DamageType.Physical);
            if (t.IsValidTarget(2000) && t.Health < GetComboDamage(t))
            {
                const string xComboText = ">> Kill <<";
                Utils.DrawText(
                    vText, xComboText, (int) t.HPBarPosition.X + 145, (int) t.HPBarPosition.Y + 5, SharpDX.Color.White);
            }
        }

        public override bool DrawingMenu(Menu config)
        {
            config.AddItem(
                new MenuItem("DrawQ" + Id, "Q range").SetValue(
                    new Circle(true, System.Drawing.Color.FromArgb(100, 255, 0, 255))));
            config.AddItem(
                new MenuItem("DrawW" + Id, "W range").SetValue(
                    new Circle(false, System.Drawing.Color.FromArgb(100, 255, 255, 255))));
            config.AddItem(new MenuItem("ShowKillableStatus", "Show Killable Status").SetValue(true));

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
