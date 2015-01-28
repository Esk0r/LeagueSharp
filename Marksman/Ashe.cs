#region
using System;
using LeagueSharp;
using LeagueSharp.Common;
using Color = System.Drawing.Color;
using SharpDX.Direct3D9;
using Font = SharpDX.Direct3D9.Font;

#endregion

namespace Marksman
{
    internal class Ashe : Champion
    {
        public static Spell Q;
        public static Spell W;
        public static Spell E;
        public static Spell R;
        public static Font vText;

        public Ashe()
        {
            Q = new Spell(SpellSlot.Q);
            W = new Spell(SpellSlot.W, 1200);
            E = new Spell(SpellSlot.E, 2500);
            R = new Spell(SpellSlot.R, 20000);

            W.SetSkillshot(250f, (float) (24.32f * Math.PI / 180), 902f, true, SkillshotType.SkillshotCone);
            E.SetSkillshot(377f, 299f, 1400f, false, SkillshotType.SkillshotLine);
            R.SetSkillshot(250f, 130f, 1600f, false, SkillshotType.SkillshotLine);

            Interrupter.OnPossibleToInterrupt += Game_OnPossibleToInterrupt;
            Obj_AI_Base.OnProcessSpellCast += Game_OnProcessSpell;

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

            Utils.PrintMessage("Ashe loaded.");
        }

        public void Game_OnPossibleToInterrupt(Obj_AI_Base unit, InterruptableSpell spell)
        {
            if (R.IsReady() && Config.Item("RInterruptable" + Id).GetValue<bool>() && unit.IsValidTarget(1500))
            {
                R.Cast(unit);
            }
        }

        private static float GetComboDamage(Obj_AI_Hero t)
        {
            var fComboDamage = 0f;

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

            return fComboDamage;
        }

        private static void DrawHarassToggleStatus()
        {
            var xHarassStatus = "";
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

        public void Game_OnProcessSpell(Obj_AI_Base unit, GameObjectProcessSpellCastEventArgs spell)
        {
            if (!Config.Item("EFlash" + Id).GetValue<bool>() || unit.Team == ObjectManager.Player.Team)
                return;

            if (spell.SData.Name.ToLower() == "summonerflash")
                E.Cast(spell.End);
        }

        public override void Game_OnGameUpdate(EventArgs args)
        {
            if (W.IsReady() && Program.Config.Item("UseWTH").GetValue<KeyBind>().Active)
            {
                if (ObjectManager.Player.HasBuff("Recall"))
                    return;
                var t = TargetSelector.GetTarget(W.Range, TargetSelector.DamageType.Physical);
                if (t != null)
                    W.Cast(t);
            }
            //Combo
            if (ComboActive)
            {
                var useQ = Config.Item("UseQC" + Id).GetValue<bool>();
                var useW = Config.Item("UseWC" + Id).GetValue<bool>();
                var t = TargetSelector.GetTarget(W.Range, TargetSelector.DamageType.Physical);

                if (useQ && Q.IsReady())
                {
                    if (!IsQActive &&
                        ObjectManager.Player.CountEnemiesInRange(Orbwalking.GetRealAutoAttackRange(t) + 350) > 0)

                    {
                        Q.Cast();
                    }
                    else
                    {
                        Q.Cast();
                    }
                }

                if (useW && W.IsReady() && t.IsValidTarget())
                {
                    W.Cast(t);
                }

                var useR = Program.Config.SubMenu("Combo").Item("UseRC").GetValue<bool>();
                if (useR && R.IsReady())
                {
                    var minRRange = Program.Config.SubMenu("Combo").Item("UseRCMinRange").GetValue<Slider>().Value;
                    var maxRRange = Program.Config.SubMenu("Combo").Item("UseRCMaxRange").GetValue<Slider>().Value;

                    t = TargetSelector.GetTarget(maxRRange, TargetSelector.DamageType.Physical);
                    if (!t.IsValidTarget())
                        return;

                    var aaDamage = Orbwalking.InAutoAttackRange(t)
                        ? ObjectManager.Player.GetAutoAttackDamage(t, true)
                        : 0;

                    if (t.Health > aaDamage && t.Health <= ObjectManager.Player.GetSpellDamage(t, SpellSlot.R) &&
                        ObjectManager.Player.Distance(t) >= minRRange)
                    {
                        R.Cast(t);
                    }
                }
            }

            //Harass
            if (HarassActive)
            {
                var target = TargetSelector.GetTarget(1200, TargetSelector.DamageType.Physical);
                if (target == null)
                    return;

                if (!IsQActive && Config.Item("UseQH" + Id).GetValue<bool>())
                    Q.Cast();

                if (Config.Item("UseWH" + Id).GetValue<bool>() && W.IsReady())
                    W.Cast(target);
            }

            //Manual cast R
            if (Config.Item("RManualCast" + Id).GetValue<KeyBind>().Active)
            {
                var rTarget = TargetSelector.GetTarget(2000, TargetSelector.DamageType.Physical);
                R.Cast(rTarget);
            }
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
            config.AddItem(
                new MenuItem("UseWTH", "Use W (Toggle)").SetValue(new KeyBind("H".ToCharArray()[0], KeyBindType.Toggle)));
            return true;
        }

        public override bool LaneClearMenu(Menu config)
        {

            return true;
        }

        public override bool DrawingMenu(Menu config)
        {
            config.AddItem(new MenuItem("DrawW" + Id, "W range").SetValue(new Circle(true, Color.CornflowerBlue)));
            config.AddItem(
                new MenuItem("DrawE" + Id, "E range").SetValue(new Circle(true, Color.FromArgb(100, 255, 0, 255))));
            config.AddItem(new MenuItem("DrawHarassToggleStatus", "Draw Toggle Status").SetValue(true));
            return true;
        }

        public override bool MiscMenu(Menu config)
        {
            config.AddItem(new MenuItem("RInterruptable" + Id, "Auto R Interruptable Spells").SetValue(true));
            config.AddItem(new MenuItem("EFlash" + Id, "Use E against Flashes").SetValue(true));
            config.AddItem(new MenuItem("RManualCast" + Id, "Cast R Manually(2000 range)"))
                .SetValue(new KeyBind('T', KeyBindType.Press));
            return true;
        }

        public bool IsQActive
        {
            get { return ObjectManager.Player.HasBuff("FrostShot"); }
        }

        public override void Drawing_OnDraw(EventArgs args)
        {
            var drawW = Config.Item("DrawW" + Id).GetValue<Circle>();
            if (drawW.Active)
            {
                Render.Circle.DrawCircle(ObjectManager.Player.Position, W.Range, drawW.Color);
            }

            var drawE = Config.Item("DrawE" + Id).GetValue<Circle>();
            if (drawE.Active)
            {
                Render.Circle.DrawCircle(ObjectManager.Player.Position, E.Range, drawE.Color);
            }

            if (Program.Config.Item("DrawHarassToggleStatus").GetValue<bool>())
            {
                DrawHarassToggleStatus();
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
        }

        public override bool ExtrasMenu(Menu config)
        {

            return true;
        }
    }
}
