#region

using System.Collections.Generic;
using LeagueSharp;
using LeagueSharp.Common;
using System;
using System.Drawing;

#endregion

namespace Zilean
{
    class Program
    {
        public const string ChampionName = "Zilean";

        public static Orbwalking.Orbwalker Orbwalker;

        public static List<Spell> SpellList = new List<Spell>();

        public static Spell Q;
        public static Spell W;
        public static Spell E;
        public static Spell R;

        public static Menu Config;

        private static void Main(string[] args)
        {
            CustomEvents.Game.OnGameLoad += Game_OnGameLoad;
        }

        private static void Game_OnGameLoad(System.EventArgs args)
        {
            if (ObjectManager.Player.BaseSkinName != ChampionName) return;

            Q = new Spell(SpellSlot.Q, 700);
            W = new Spell(SpellSlot.W, 600);
            E = new Spell(SpellSlot.E, 700);
            R = new Spell(SpellSlot.R, 900);

            SpellList.Add(Q);
            SpellList.Add(W);
            SpellList.Add(E);
            SpellList.Add(R);

            Config = new Menu(ChampionName, ChampionName, true);

            Config.AddSubMenu(new Menu("Orbwalking", "Orbwalking"));
            Orbwalker = new Orbwalking.Orbwalker(Config.SubMenu("Orbwalking"));

            Config.AddSubMenu(new Menu("Ult to Save Lives", "SaveLives"));
            Config.SubMenu("SaveLives").AddItem(new MenuItem("RSave", "Save People?").SetValue(true));
            foreach (Obj_AI_Hero Champ in ObjectManager.Get<Obj_AI_Hero>())
                if (Champ.IsAlly)
                    Config.SubMenu("SaveLives").AddItem(new MenuItem("Save" + Champ.BaseSkinName,string.Format("Save {0}", Champ.BaseSkinName)).SetValue(true));
            
            Config.SubMenu("SaveLives").AddItem(new MenuItem("WRefresh", "Use W to Refresh Ult").SetValue(true));
            Config.SubMenu("SaveLives").AddItem(new MenuItem("HPPercent", "HP Percent to Trigger").SetValue(new Slider(15, 100, 0)));

            Config.AddSubMenu(new Menu("Combo", "Combo"));
            Config.SubMenu("Combo").AddItem(new MenuItem("UseQCombo", "Use Q").SetValue(true));
            Config.SubMenu("Combo").AddItem(new MenuItem("UseWCombo", "Use W").SetValue(true));
            Config.SubMenu("Combo").AddItem(new MenuItem("UseECombo", "Use E").SetValue(true));
            Config.SubMenu("Combo").AddItem(new MenuItem("ComboActive", "Combo!").SetValue(new KeyBind(32, KeyBindType.Press)));

            //Config.AddSubMenu(new Menu("Harass", "Harass"));
            //Config.SubMenu("Harass").AddItem(new MenuItem("UseQHarass", "Use Q").SetValue(true));
            //Config.SubMenu("Harass").AddItem(new MenuItem("UseWHarass", "Use W").SetValue(true));
            //Config.SubMenu("Harass").AddItem(new MenuItem("UseEHarass", "Use E").SetValue(true));
            //Config.SubMenu("Harass").AddItem(new MenuItem("HarassActive", "Harass!").SetValue(new KeyBind("C".ToCharArray()[0], KeyBindType.Press)));

            Config.AddSubMenu(new Menu("Lane Clear", "Laneclear"));
            Config.SubMenu("Laneclear").AddItem(new MenuItem("UseQFarm", "Use Q").SetValue(true));
            //Config.SubMenu("Laneclear").AddItem(new MenuItem("UseEFarm", "Use E").SetValue(true));
            Config.SubMenu("Laneclear").AddItem(new MenuItem("LaneClearActive", "LaneClear!").SetValue(new KeyBind("V".ToCharArray()[0], KeyBindType.Press)));

            //Config.AddSubMenu(new Menu("Last Hit Q", "LastHitQ"));
            //Config.SubMenu("LastHitQ").AddItem(new MenuItem("LastHitQ", "Use Q to Last Hit").SetValue(true));
            //Config.SubMenu("LastHitQ").AddItem(new MenuItem("LastHitActive", "Last Hit!").SetValue(new KeyBind("X".ToCharArray()[0], KeyBindType.Press)));

            Config.AddSubMenu(new Menu("Drawing", "Drawing"));
            Config.SubMenu("Drawing").AddItem(new MenuItem("QRange", "Q range").SetValue(new Circle(true, Color.FromArgb(255, 255, 255, 255))));
            Config.SubMenu("Drawing").AddItem(new MenuItem("WRange", "W range").SetValue(new Circle(false, Color.FromArgb(255, 255, 255, 255))));
            Config.SubMenu("Drawing").AddItem(new MenuItem("ERange", "E range").SetValue(new Circle(false, Color.FromArgb(255, 255, 255, 255))));
            Config.SubMenu("Drawing").AddItem(new MenuItem("RRange", "R range").SetValue(new Circle(false, Color.FromArgb(255, 255, 255, 255))));

            Config.AddToMainMenu();

            Drawing.OnDraw += Drawing_OnDraw;
            Game.OnGameUpdate += Game_OnGameUpdate;
        }

        private static void Game_OnGameUpdate(EventArgs args)
        {
            //Prevent program functions if recalling
            if (ObjectManager.Player.HasBuff("Recall"))
                return;
            // Refresh R with W
            //if (Config.Item("RSave").GetValue<bool>() && !R.IsReady() && W.IsReady() && Config.Item("WRefresh").GetValue<bool>())
              //  ObjectManager.Player.Spellbook.CastSpell(SpellSlot.W);
            // Save folks with R
            //if (Config.Item("RSave").GetValue<bool>())
                //Game.PrintChat(string.Format("Calling Save Lives"));
            SaveLives();
            // Combo
            if (Config.Item("ComboActive").GetValue<KeyBind>().Active)
            {
                //Game.PrintChat(string.Format("Spacebar held down calling COMBO"));
                Combo();
            }
            //  Laneclear
            if (Config.Item("LaneClearActive").GetValue<KeyBind>().Active)
            {
                LaneClear();
            }

        }

        private static void LaneClear()
        {
            //Not positive about this...
            var Minions = MinionManager.GetMinions(ObjectManager.Player.ServerPosition, Q.Range, MinionTypes.All, MinionTeam.Enemy, MinionOrderTypes.Health);
            foreach (var minion in Minions)
                if (minion.IsValidTarget(Q.Range) && HealthPrediction.GetHealthPrediction(minion, (int)Q.Delay) < (DamageLib.getDmg(minion, DamageLib.SpellType.Q) * 0.9))
                        Q.CastOnUnit(minion, false);
        }

        private static void Combo()
        {
            Game.PrintChat(string.Format("COMBO Called"));
            //Such and easy combo, if someone is in range, nuke em.. lol.  Willing to take suggestions to make it better if its needed.
            var target = SimpleTs.GetTarget(900, SimpleTs.DamageType.Magical);
            if (target.IsValid)
                //Game.PrintChat(string.Format("Found a valid target: {0}", target.BaseSkinName));
            {
                if (Config.Item("UseQCombo").GetValue<bool>() && Q.IsReady() && (ObjectManager.Player.ServerPosition.Distance(target.Position) < Q.Range))
                    Q.CastOnUnit(target, false);
                if (Config.Item("UseECombo").GetValue<bool>() && E.IsReady() && (ObjectManager.Player.ServerPosition.Distance(target.Position) < E.Range))
                    E.CastOnUnit(target, false);
                if (Config.Item("UseWCombo").GetValue<bool>() && W.IsReady() && (ObjectManager.Player.ServerPosition.Distance(target.Position) < Q.Range))
                    if (!Q.IsReady() || !E.IsReady())
                        ObjectManager.Player.Spellbook.CastSpell(SpellSlot.W);
            }

        }

        private static void SaveLives()
        {
            if (Config.Item("RSave").GetValue<bool>() && !R.IsReady() && W.IsReady() && Config.Item("WRefresh").GetValue<bool>())
                ObjectManager.Player.Spellbook.CastSpell(SpellSlot.W);
            //Could use some better logic here.. but for now this "works"...  Maybe use Evade instead?  
            //Game.PrintChat(string.Format("Called Save Lives"));
            if (Config.Item("RSave").GetValue<bool>())
            foreach (Obj_AI_Hero Champ in ObjectManager.Get<Obj_AI_Hero>())
                if (Champ.IsAlly)
                if (Config.Item("Save" + Champ.BaseSkinName).GetValue<bool>() && R.IsReady())
                    if (Champ.Health < (Champ.MaxHealth * (Config.Item("HPPercent").GetValue<Slider>().Value * 0.01)))
                        if ((!Champ.IsDead) && (!Champ.IsInvulnerable))
                            R.CastOnUnit(Champ, false);
        }

        private static void Drawing_OnDraw(EventArgs args)
        {
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