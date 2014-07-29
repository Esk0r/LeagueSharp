#region

using System.Collections.Generic;
using LeagueSharp;
using LeagueSharp.Common;
using System;
using System.Linq;
using System.Drawing;

#endregion

namespace Karthus
{
    class Program
    {
        public const string ChampionName = "Karthus";

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

        private static void Main(string[] args)
        {
            CustomEvents.Game.OnGameLoad +=Game_OnGameLoad;
        }

        private static void Game_OnGameLoad(System.EventArgs args)
        {
            if (ObjectManager.Player.BaseSkinName != ChampionName) return;

            //Create the spells
            Q = new Spell(SpellSlot.Q, 875);
            W = new Spell(SpellSlot.W, 1000);
            E = new Spell(SpellSlot.E, 500);
            R = new Spell(SpellSlot.R, 25000);

            SpellList.Add(Q);
            SpellList.Add(W);
            SpellList.Add(E);
            SpellList.Add(R);

            //Create the menu
            Config = new Menu(ChampionName, ChampionName, true);

            //Orbwalker submenu
            Config.AddSubMenu(new Menu("Orbwalking", "Orbwalking"));

            //Load the orbwalker and add it to the submenu.
            Orbwalker = new Orbwalking.Orbwalker(Config.SubMenu("Orbwalking"));

            Config.AddSubMenu(new Menu("Combo", "Combo"));
            Config.SubMenu("Combo").AddItem(new MenuItem("ComboActive", "Combo!").SetValue(new KeyBind(32, KeyBindType.Press)));
            Config.SubMenu("Combo").AddItem(new MenuItem("UseQCombo", "Use Q").SetValue(true));
            Config.SubMenu("Combo").AddItem(new MenuItem("UseWCombo", "Use W").SetValue(true));
            Config.SubMenu("Combo").AddItem(new MenuItem("UseECombo", "Use E").SetValue(true));

            Config.AddSubMenu(new Menu("Harass", "Harass"));
            Config.SubMenu("Harass").AddItem(new MenuItem("UseQHarass", "Use Q").SetValue(true));
            Config.SubMenu("Harass").AddItem(new MenuItem("UseWHarass", "Use W").SetValue(true));
            Config.SubMenu("Harass").AddItem(new MenuItem("UseEHarass", "Use E").SetValue(true));
            Config.SubMenu("Harass").AddItem(new MenuItem("HarassActive", "Harass!").SetValue(new KeyBind("C".ToCharArray()[0], KeyBindType.Press)));

            Config.AddSubMenu(new Menu("Farm", "Farm"));
            Config.SubMenu("Farm").AddItem(new MenuItem("UseQFarm", "Use Q").SetValue(true));
            Config.SubMenu("Farm").AddItem(new MenuItem("UseEFarm", "Use E").SetValue(true));
            Config.SubMenu("Farm").AddItem(new MenuItem("LaneClearActive", "LaneClear!").SetValue(new KeyBind("V".ToCharArray()[0], KeyBindType.Press)));

            Config.AddSubMenu(new Menu("Auto Ult", "AutoUlt"));
            Config.SubMenu("AutoUlt").AddItem(new MenuItem("AutoUlt", "Auto Ult for Kills").SetValue(true));

            Config.AddSubMenu(new Menu("Drawings", "Drawings"));
            Config.SubMenu("Drawings").AddItem(new MenuItem("QRange", "Q range").SetValue(new Circle(true, Color.FromArgb(255, 255, 255, 255))));
            Config.SubMenu("Drawings").AddItem(new MenuItem("WRange", "W range").SetValue(new Circle(false, Color.FromArgb(255, 255, 255, 255))));
            Config.SubMenu("Drawings").AddItem(new MenuItem("ERange", "E range").SetValue(new Circle(false, Color.FromArgb(255, 255, 255, 255))));
            Config.SubMenu("Drawings").AddItem(new MenuItem("RRange", "R range").SetValue(new Circle(false, Color.FromArgb(255, 255, 255, 255))));

            Config.AddToMainMenu();

            Drawing.OnDraw += Drawing_OnDraw;
            Game.OnGameUpdate += Game_OnGameUpdate;
         
        }

        private static void Game_OnGameUpdate(System.EventArgs args)
        {
            Orbwalker.SetAttacks(true);
            //  DEBUG Game.PrintChat(string.Format("E Toggle State:{0}", ObjectManager.Player.Spellbook.GetSpell(SpellSlot.E).ToggleState));

            if (Config.Item("AutoUlt").GetValue<bool>() && R.IsReady())
            {
                AutoULt();
            }
            
            if (Config.Item("ComboActive").GetValue<KeyBind>().Active)
            {
                Combo();
            }
            else
            {
                if (Config.Item("HarassActive").GetValue<KeyBind>().Active)
                    Harass();

                if (Config.Item("LaneClearActive").GetValue<KeyBind>().Active)
                    Farm();
            }
        }

        private static void AutoULt()
        {
            //  Game.PrintChat(string.Format("Auto Ult has been called"));  //For debug purposes
            var target = SimpleTs.GetTarget(25000, SimpleTs.DamageType.Magical);
            if (DamageLib.getDmg(target, DamageLib.SpellType.R) > (target.Health*1.05))
            {
                Game.PrintChat(string.Format("Auto Ult is attempting to cast"));  //For debug purposes
                ObjectManager.Player.Spellbook.CastSpell(SpellSlot.R);
            }
        }

        private static void Combo()
        {
            var target = SimpleTs.GetTarget(1000, SimpleTs.DamageType.Magical);
            if (target != null)
            {
                var QPrediction = Q.GetPrediction(target);
                var WPrediction = W.GetPrediction(target);
                var EPrediction = E.GetPrediction(target);

                if (Q.IsReady() && Config.Item("UseQCombo").GetValue<bool>())
                    if (QPrediction.HitChance >= Prediction.HitChance.HighHitchance)
                        if (ObjectManager.Player.ServerPosition.Distance(QPrediction.Position) < Q.Range)
                            Q.Cast(QPrediction.Position, false);
                
                if (W.IsReady() && Config.Item("UseWCombo").GetValue<bool>())
                    if (WPrediction.HitChance >= Prediction.HitChance.HighHitchance)
                        if (ObjectManager.Player.ServerPosition.Distance(WPrediction.Position) < W.Range)
                            W.Cast(WPrediction.Position, false);
                
                if (E.IsReady() && (EPrediction.TargetsHit >= 1) && Config.Item("UseECombo").GetValue<bool>() && (ObjectManager.Player.Spellbook.GetSpell(SpellSlot.E).ToggleState == 1))
                    ObjectManager.Player.Spellbook.CastSpell(SpellSlot.E);
                
                if (E.IsReady() && (EPrediction.TargetsHit <= 0) && Config.Item("UseECombo").GetValue<bool>() && (ObjectManager.Player.Spellbook.GetSpell(SpellSlot.E).ToggleState == 2))
                    ObjectManager.Player.Spellbook.CastSpell(SpellSlot.E);
            }

        }

        private static void Farm()
        {
            var Minions = MinionManager.GetMinions(ObjectManager.Player.ServerPosition, Q.Range, MinionTypes.All, MinionTeam.Enemy,MinionOrderTypes.Health);
            var QLocation = Q.GetCircularFarmLocation(Minions);
            var ELocation = E.GetCircularFarmLocation(Minions);
            if ((QLocation.MinionsHit >= 1) && Q.IsReady() && Config.Item("UseQFarm").GetValue<bool>())
                Q.Cast(QLocation.Position.To3D(), false);
            if ((ELocation.MinionsHit >= 2) && E.IsReady() && Config.Item("UseEFarm").GetValue<bool>() && (ObjectManager.Player.Spellbook.GetSpell(SpellSlot.E).ToggleState == 1))
                E.Cast(ObjectManager.Player.ServerPosition, false);
            if ((ELocation.MinionsHit <= 1) && E.IsReady() && Config.Item("UseEFarm").GetValue<bool>() && (ObjectManager.Player.Spellbook.GetSpell(SpellSlot.E).ToggleState == 2))
                E.Cast(ObjectManager.Player.ServerPosition, false);
        }

        private static void Harass()
        {
            var target = SimpleTs.GetTarget(1000, SimpleTs.DamageType.Magical);
            if (target != null)
            {
                var QPrediction = Q.GetPrediction(target);
                var WPrediction = W.GetPrediction(target);
                var EPrediction = E.GetPrediction(target);

                if (Q.IsReady() && Config.Item("UseQHarass").GetValue<bool>())
                    if (QPrediction.HitChance >= Prediction.HitChance.HighHitchance)
                        if (ObjectManager.Player.ServerPosition.Distance(QPrediction.Position) < Q.Range)
                            Q.Cast(QPrediction.Position, false);

                if (W.IsReady() && Config.Item("UseWHarass").GetValue<bool>())
                    if (WPrediction.HitChance >= Prediction.HitChance.HighHitchance)
                        if (ObjectManager.Player.ServerPosition.Distance(WPrediction.Position) < W.Range)
                            W.Cast(WPrediction.Position, false);

                if (E.IsReady() && (EPrediction.TargetsHit >= 1) && Config.Item("UseEHarass").GetValue<bool>() && (ObjectManager.Player.Spellbook.GetSpell(SpellSlot.E).ToggleState == 1))
                    ObjectManager.Player.Spellbook.CastSpell(SpellSlot.E);

                if (E.IsReady() && (EPrediction.TargetsHit <= 0) && Config.Item("UseEHarass").GetValue<bool>() && (ObjectManager.Player.Spellbook.GetSpell(SpellSlot.E).ToggleState == 2))
                    ObjectManager.Player.Spellbook.CastSpell(SpellSlot.E);
            }
            
        }
        
        private static void Drawing_OnDraw(System.EventArgs args)
        {
            //Draw the ranges of the spells.
            foreach (var spell in SpellList)
            {
                var menuItem = Config.Item(spell.Slot + "Range").GetValue<Circle>();
                if (menuItem.Active)
                {
                    Utility.DrawCircle(ObjectManager.Player.Position, spell.Range, menuItem.Color);
                }
            }
        }
    }
}
