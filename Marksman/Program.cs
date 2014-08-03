#region

using System;
using System.Linq;
using LeagueSharp;
using LeagueSharp.Common;

#endregion

namespace Marksman
{
    internal class Program
    {
        public static Menu Config;
        public static Champion CClass;

        private static void Main(string[] args)
        {
            CustomEvents.Game.OnGameLoad += Game_OnGameLoad;
        }

        private static void Game_OnGameLoad(EventArgs args)
        {
            Config = new Menu("Marksman", "Marksman", true);

            CClass = new Champion();
            if (ObjectManager.Player.BaseSkinName == "Ezreal")
                CClass = new Ezreal();

            if (ObjectManager.Player.BaseSkinName == "Jinx")
                CClass = new Jinx();

            if (ObjectManager.Player.BaseSkinName == "Sivir")
                CClass = new Sivir();
            
            if (ObjectManager.Player.BaseSkinName == "Tristana")
                CClass = new Tristana();
            
            CClass.Id = ObjectManager.Player.BaseSkinName;
            CClass.Config = Config;

            var targetSelectorMenu = new Menu("Target Selector", "Target Selector");
            SimpleTs.AddToMenu(targetSelectorMenu);
            Config.AddSubMenu(targetSelectorMenu);

            var orbwalking = Config.AddSubMenu(new Menu("Orbwalking", "Orbwalking"));
            CClass.Orbwalker = new Orbwalking.Orbwalker(orbwalking);

            var items = Config.AddSubMenu(new Menu("Items", "Items"));
            items.AddItem(new MenuItem("BOTRK", "BOTRK").SetValue(true));

            var combo = Config.AddSubMenu(new Menu("Combo", "Combo"));
            CClass.ComboMenu(combo);

            var harass = Config.AddSubMenu(new Menu("Harass", "Harass"));
            CClass.HarassMenu(harass);

            var misc = Config.AddSubMenu(new Menu("Misc", "Misc"));
            CClass.MiscMenu(misc);

            var drawing = Config.AddSubMenu(new Menu("Drawings", "Drawings"));
            CClass.DrawingMenu(drawing);

            CClass.MainMenu(Config);

            Config.AddToMainMenu();

            Drawing.OnDraw += Drawing_OnDraw;
            Game.OnGameUpdate += Game_OnGameUpdate;
        }


        private static void Drawing_OnDraw(EventArgs args)
        {
            CClass.Drawing_OnDraw(args);
            return;

            var y = 10;
            foreach (
                var t in
                    ObjectManager.Player.Buffs.Select(
                        b => b.DisplayName + " - " + b.IsActive + " - " + (b.EndTime > Game.Time) + " - " + b.IsPositive)
                )
            {
                Drawing.DrawText(0, y, System.Drawing.Color.Wheat, t);
                y = y + 16;
            }
        }

        private static void Game_OnGameUpdate(EventArgs args)
        {
            //Update the combo and harass values.
            CClass.ComboActive = CClass.Config.Item("Orbwalk").GetValue<KeyBind>().Active;
            CClass.HarassActive = CClass.Config.Item("Farm").GetValue<KeyBind>().Active;

            //Items
            var botrk = Config.Item("BOTRK").GetValue<bool>();
            var target = CClass.Orbwalker.GetTarget();

            if (botrk)
            {
                if (target != null && target.Type == ObjectManager.Player.Type &&
                    target.ServerPosition.Distance(ObjectManager.Player.ServerPosition) < 450)
                {
                    var hasCutGlass = Items.HasItem(3144);
                    var hasBotrk = Items.HasItem(3153);

                    if (hasBotrk || hasCutGlass)
                    {
                        var itemId = hasCutGlass ? 3144 : 3153;
                        var damage = DamageLib.getDmg(target, DamageLib.SpellType.BOTRK);
                        if (hasCutGlass || ObjectManager.Player.Health + damage < ObjectManager.Player.MaxHealth)
                            Items.UseItem(itemId, target);
                    }
                }
            }
        }
    }
}