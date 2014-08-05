#region

using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using LeagueSharp;
using LeagueSharp.Common;

#endregion

namespace Xerath
{
    internal class Program
    {
        public const string ChampionName = "Xerath";

        //Orbwalker instance
        public static Orbwalking.Orbwalker Orbwalker;

        //Spells
        public static List<Spell> SpellList = new List<Spell>();

        public static Spell Q;
        public static Spell W;
        public static Spell E;
        public static Spell R;

        //Menu
        public static Menu Config;

        private static Obj_AI_Hero Player;

        public static bool IsPassiveUp
        {
            get { return ObjectManager.Player.HasBuff("xerathascended2onhit", true); }
        }

        public static bool IsCastingR
        {
            get
            {
                return ObjectManager.Player.HasBuff("XerathLocusOfPower2", true) ||
                       (ObjectManager.Player.LastCastedSpellName() == "XerathLocusOfPower2" &&
                        Environment.TickCount - ObjectManager.Player.LastCastedSpellT() < 500);
            }
        }

        private static void Main(string[] args)
        {
            CustomEvents.Game.OnGameLoad += Game_OnGameLoad;
        }

        private static void Game_OnGameLoad(EventArgs args)
        {
            Player = ObjectManager.Player;

            if (Player.BaseSkinName != ChampionName) return;

            //Create the spells
            Q = new Spell(SpellSlot.Q, 1550);
            W = new Spell(SpellSlot.W, 1000);
            E = new Spell(SpellSlot.E, 1150);
            R = new Spell(SpellSlot.R, 675);

            Q.SetSkillshot(0.6f, 100f, float.MaxValue, false, Prediction.SkillshotType.SkillshotLine);
            W.SetSkillshot(0.7f, 125f, float.MaxValue, false, Prediction.SkillshotType.SkillshotCircle);
            E.SetSkillshot(0.25f, 60f, 1400f, true, Prediction.SkillshotType.SkillshotLine);
            R.SetSkillshot(0.7f, 120f, float.MaxValue, false, Prediction.SkillshotType.SkillshotCircle);

            Q.SetCharged("XerathArcanopulseChargeUp", "XerathArcanopulseChargeUp", 750, 1550, 1.5f);

            SpellList.Add(Q);
            SpellList.Add(W);
            SpellList.Add(E);
            SpellList.Add(R);

            //Create the menu
            Config = new Menu(ChampionName, ChampionName, true);

            //Orbwalker submenu
            Config.AddSubMenu(new Menu("Orbwalking", "Orbwalking"));

            //Add the target selector to the menu as submenu.
            var targetSelectorMenu = new Menu("Target Selector", "Target Selector");
            SimpleTs.AddToMenu(targetSelectorMenu);
            Config.AddSubMenu(targetSelectorMenu);

            //Load the orbwalker and add it to the menu as submenu.
            Orbwalker = new Orbwalking.Orbwalker(Config.SubMenu("Orbwalking"));

            //Combo menu:
            Config.AddSubMenu(new Menu("Combo", "Combo"));
            Config.SubMenu("Combo").AddItem(new MenuItem("UseQCombo", "Use Q").SetValue(true));
            Config.SubMenu("Combo").AddItem(new MenuItem("UseWCombo", "Use W").SetValue(true));
            Config.SubMenu("Combo").AddItem(new MenuItem("UseECombo", "Use E").SetValue(true));
            Config.SubMenu("Combo").AddItem(new MenuItem("UseRCombo", "Use R").SetValue(true));
            Config.SubMenu("Combo")
                .AddItem(
                    new MenuItem("ComboActive", "Combo!").SetValue(
                        new KeyBind(Config.Item("Orbwalk").GetValue<KeyBind>().Key, KeyBindType.Press)));

            //Harass menu:
            Config.AddSubMenu(new Menu("Harass", "Harass"));
            Config.SubMenu("Harass").AddItem(new MenuItem("UseQHarass", "Use Q").SetValue(true));
            Config.SubMenu("Harass").AddItem(new MenuItem("UseWHarass", "Use W").SetValue(false));
            Config.SubMenu("Harass")
                .AddItem(
                    new MenuItem("HarassActive", "Harass!").SetValue(
                        new KeyBind(Config.Item("Farm").GetValue<KeyBind>().Key, KeyBindType.Press)));
            Config.SubMenu("Harass")
                .AddItem(
                    new MenuItem("HarassActiveT", "Harass (toggle)!").SetValue(new KeyBind("Y".ToCharArray()[0],
                        KeyBindType.Toggle)));

            //Farming menu:
            Config.AddSubMenu(new Menu("Farm", "Farm"));
            Config.SubMenu("Farm")
                .AddItem(
                    new MenuItem("UseQFarm", "Use Q").SetValue(
                        new StringList(new[] { "Freeze", "LaneClear", "Both", "No" }, 2)));
            Config.SubMenu("Farm")
                .AddItem(
                    new MenuItem("UseWFarm", "Use W").SetValue(
                        new StringList(new[] { "Freeze", "LaneClear", "Both", "No" }, 1)));
            Config.SubMenu("Farm")
                .AddItem(
                    new MenuItem("FreezeActive", "Freeze!").SetValue(
                        new KeyBind(Config.Item("Farm").GetValue<KeyBind>().Key, KeyBindType.Press)));
            Config.SubMenu("Farm")
                .AddItem(
                    new MenuItem("LaneClearActive", "LaneClear!").SetValue(
                        new KeyBind(Config.Item("LaneClear").GetValue<KeyBind>().Key, KeyBindType.Press)));

            //JungleFarm menu:
            Config.AddSubMenu(new Menu("JungleFarm", "JungleFarm"));
            Config.SubMenu("JungleFarm").AddItem(new MenuItem("UseQJFarm", "Use Q").SetValue(true));
            Config.SubMenu("JungleFarm").AddItem(new MenuItem("UseWJFarm", "Use W").SetValue(true));
            Config.SubMenu("JungleFarm")
                .AddItem(
                    new MenuItem("JungleFarmActive", "JungleFarm!").SetValue(
                        new KeyBind(Config.Item("LaneClear").GetValue<KeyBind>().Key, KeyBindType.Press)));

            //Misc
            Config.AddSubMenu(new Menu("Misc", "Misc"));
            Config.SubMenu("Misc").AddItem(new MenuItem("InterruptSpells", "Interrupt spells").SetValue(true));

            //Drawings menu:
            Config.AddSubMenu(new Menu("Drawings", "Drawings"));
            Config.SubMenu("Drawings")
                .AddItem(
                    new MenuItem("QRange", "Q range").SetValue(new Circle(true, Color.FromArgb(150, Color.DodgerBlue))));
            Config.SubMenu("Drawings")
                .AddItem(
                    new MenuItem("WRange", "W range").SetValue(new Circle(true, Color.FromArgb(150, Color.DodgerBlue))));
            Config.SubMenu("Drawings")
                .AddItem(
                    new MenuItem("ERange", "E range").SetValue(new Circle(false, Color.FromArgb(150, Color.DodgerBlue))));
            Config.SubMenu("Drawings")
                .AddItem(
                    new MenuItem("RRange", "R range").SetValue(new Circle(false, Color.FromArgb(150, Color.DodgerBlue))));
            Config.SubMenu("Drawings")
                .AddItem(
                    new MenuItem("RRangeM", "R range (minimap)").SetValue(new Circle(false,
                        Color.FromArgb(150, Color.DodgerBlue))));
            Config.AddToMainMenu();

            //Add the events we are going to use:
            Game.OnGameUpdate += Game_OnGameUpdate;
            Drawing.OnDraw += Drawing_OnDraw;
            Drawing.OnEndScene += Drawing_OnEndScene;
            Interrupter.OnPosibleToInterrupt += Interrupter_OnPosibleToInterrupt;

            Game.PrintChat(ChampionName + " Loaded!");
        }

        private static void Interrupter_OnPosibleToInterrupt(Obj_AI_Base unit, InterruptableSpell spell)
        {
            if (!Config.Item("InterruptSpells").GetValue<bool>()) return;

            if (Player.Distance(unit) < E.Range && E.IsReady())
            {
                E.Cast(unit);
            }
        }

        private static void Combo()
        {
            Orbwalker.SetAttacks(IsPassiveUp || (!Q.IsReady() && !W.IsReady() && !E.IsReady() && !R.IsReady()));
            UseSpells(Config.Item("UseQCombo").GetValue<bool>(), Config.Item("UseWCombo").GetValue<bool>(),
                Config.Item("UseECombo").GetValue<bool>());
        }

        private static void Harass()
        {
            UseSpells(Config.Item("UseQHarass").GetValue<bool>(), Config.Item("UseWHarass").GetValue<bool>(),
                false);
        }

        private static void UseSpells(bool useQ, bool useW, bool useE)
        {
            var qTarget = SimpleTs.GetTarget(Q.ChargedMaxRange, SimpleTs.DamageType.Magical);
            var wTarget = SimpleTs.GetTarget(W.Range + W.Width * 0.5f, SimpleTs.DamageType.Magical);
            var eTarget = SimpleTs.GetTarget(E.Range, SimpleTs.DamageType.Magical);

            if (eTarget != null && useE && E.IsReady())
            {
                if (Player.Distance(eTarget) < E.Range * 0.4f)
                    E.Cast(eTarget);
                else if ((!useW || !W.IsReady()))
                    E.Cast(eTarget);
            }

            if (useQ && Q.IsReady())
            {
                if (Q.IsCharging)
                {
                    Q.Cast(qTarget, false, true);
                }
                else if (qTarget != null && (!useW || !W.IsReady() || Player.Distance(qTarget) > W.Range))
                {
                    Q.StartCharging();
                }
            }

            if (wTarget != null && useW && W.IsReady())
                W.Cast(wTarget, false, true);
        }

        private static void WhileCastingR()
        {
            var rTarget = SimpleTs.GetTarget(R.Range, SimpleTs.DamageType.Magical);
            if (rTarget != null)
            {
                R.Cast(rTarget, true);
            }
        }

        private static void Farm(bool laneClear)
        {
            var allMinionsQ = MinionManager.GetMinions(ObjectManager.Player.ServerPosition, Q.ChargedMaxRange,
                MinionTypes.All);
            var rangedMinionsW = MinionManager.GetMinions(ObjectManager.Player.ServerPosition, W.Range + W.Width + 30,
                MinionTypes.Ranged);

            var useQi = Config.Item("UseQFarm").GetValue<StringList>().SelectedIndex;
            var useWi = Config.Item("UseWFarm").GetValue<StringList>().SelectedIndex;
            var useQ = (laneClear && (useQi == 1 || useQi == 2)) || (!laneClear && (useQi == 0 || useQi == 2));
            var useW = (laneClear && (useWi == 1 || useWi == 2)) || (!laneClear && (useWi == 0 || useWi == 2));


            if (useQ && Q.IsReady())
            {
                if (Q.IsCharging)
                {
                    var locQ = Q.GetLineFarmLocation(allMinionsQ);
                    if (allMinionsQ.Count == allMinionsQ.Count(m => Player.Distance(m) < Q.Range) && locQ.MinionsHit > 0)
                        Q.Cast(locQ.Position);
                }
                else if (allMinionsQ.Count > 0)
                    Q.StartCharging();
            }

            if (useW && W.IsReady())
            {
                var locW = W.GetCircularFarmLocation(rangedMinionsW, W.Width * 0.75f);
                if (locW.MinionsHit >= 3)
                    W.Cast(locW.Position);
                else
                {
                    var locW2 = W.GetCircularFarmLocation(allMinionsQ, W.Width * 0.75f);
                    if (locW2.MinionsHit >= 1)
                        W.Cast(locW.Position);
                }
            }
        }

        private static void JungleFarm()
        {
            var useQ = Config.Item("UseQJFarm").GetValue<bool>();
            var useW = Config.Item("UseWJFarm").GetValue<bool>();
            var mobs = MinionManager.GetMinions(ObjectManager.Player.ServerPosition, W.Range, MinionTypes.All,
                MinionTeam.Neutral, MinionOrderTypes.MaxHealth);

            if (mobs.Count > 0)
            {
                var mob = mobs[0];
                if (useW && W.IsReady())
                {
                    W.Cast(mob);
                }
                else if (useQ && Q.IsReady())
                {
                    if (!Q.IsCharging)
                        Q.StartCharging();
                    else
                        Q.Cast(mob);
                }
            }
        }

        private static void Game_OnGameUpdate(EventArgs args)
        {
            if (Player.IsDead) return;
            Orbwalker.SetAttacks(true);
            Orbwalker.SetMovement(true);

            //Update the R range
            R.Range = 2000 + R.Level * 1200;

            if (IsCastingR)
            {
                Orbwalker.SetMovement(false);
                Orbwalker.SetAttacks(false);
                WhileCastingR();
                return;
            }


            if (Config.Item("ComboActive").GetValue<KeyBind>().Active)
            {
                Combo();
            }
            else
            {
                if (Config.Item("HarassActive").GetValue<KeyBind>().Active ||
                    Config.Item("HarassActiveT").GetValue<KeyBind>().Active)
                    Harass();

                var lc = Config.Item("LaneClearActive").GetValue<KeyBind>().Active;
                if (lc || Config.Item("FreezeActive").GetValue<KeyBind>().Active)
                    Farm(lc);

                if (Config.Item("JungleFarmActive").GetValue<KeyBind>().Active)
                    JungleFarm();
            }
        }

        private static void Drawing_OnEndScene(EventArgs args)
        {
            if (R.Level == 0) return;
            var menuItem = Config.Item(R.Slot + "RangeM").GetValue<Circle>();
            if (menuItem.Active)
                Utility.DrawCircle(Player.Position, R.Range, menuItem.Color, 2, 30, true);
        }

        private static void Drawing_OnDraw(EventArgs args)
        {
            //Draw the ranges of the spells.
            foreach (var spell in SpellList)
            {
                var menuItem = Config.Item(spell.Slot + "Range").GetValue<Circle>();
                if (menuItem.Active)
                    Utility.DrawCircle(Player.Position, spell.Range, menuItem.Color);
            }
        }
    }
}