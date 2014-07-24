#region

using System;
using LeagueSharp;
using LeagueSharp.Common;

#endregion

namespace Marksman
{
    internal class Program
    {
        public static Menu Config;
        public static Champion cClass;

        private static void Main(string[] args)
        {
            CustomEvents.Game.OnGameLoad += Game_OnGameLoad;

            Drawing.OnDraw += Drawing_OnDraw;
        }

        static void Drawing_OnDraw(EventArgs args)
        {
            return;
            
            var y = 10;
            foreach (var b in ObjectManager.Player.Buffs)
            {
                var t = b.DisplayName + " - " + b.IsActive + " - " + (b.EndTime > Game.Time ) + " - " + b.IsPositive;
                Drawing.DrawText(0, y, System.Drawing.Color.Wheat, t);
                y = y + 16;
            }
        }

        private static void Game_OnGameLoad(EventArgs args)
        {
            Config = new Menu("Marksman", "Marksman", true);

            cClass = new Champion();  
            if (ObjectManager.Player.BaseSkinName == "Ezreal")
                cClass = new Ezreal();

            if (ObjectManager.Player.BaseSkinName == "Jinx")
                cClass = new Jinx();

            cClass.Config = Config;

            var orbwalking = Config.AddSubMenu(new Menu("Orbwalking", "Orbwalking"));
            cClass.Orbwalker = new Orbwalking.Orbwalker(orbwalking);

            var items = Config.AddSubMenu(new Menu("Items", "Items"));
            items.AddItem(new MenuItem("BOTRK", "BOTRK").SetValue(true));

            var combo = Config.AddSubMenu(new Menu("Combo", "Combo"));
            cClass.ComboMenu(combo);

            var harass = Config.AddSubMenu(new Menu("Harass", "Harass"));
            cClass.HarassMenu(harass);

            var misc = Config.AddSubMenu(new Menu("Misc", "Misc"));
            cClass.MiscMenu(misc);

            var drawing = Config.AddSubMenu(new Menu("Drawings", "Drawings"));
            cClass.DrawingMenu(drawing);

            cClass.MainMenu(Config);

            Config.AddToMainMenu();

            Game.OnGameUpdate += Game_OnGameUpdate;
        }



        private static void Game_OnGameUpdate(EventArgs args)
        {
            //Update the combo and harass values.
            cClass.ComboActive = cClass.Config.Item("Orbwalk").GetValue<KeyBind>().Active;
            cClass.HarassActive = cClass.Config.Item("Farm").GetValue<KeyBind>().Active;

            //Items
            var botrk = Config.Item("BOTRK").GetValue<bool>();

            var target = cClass.Orbwalker.GetTarget();

            if (botrk)
            {
                if (target != null && target.ServerPosition.Distance(ObjectManager.Player.ServerPosition) < 450)
                {
                    var hasCutGlass = Items.HasItem(3144);
                    var hasBOTRK = Items.HasItem(3153);

                    if (hasBOTRK || hasCutGlass)
                    {
                        var itemId = hasCutGlass ? 3144 : 3153;
                        var damage = DamageLib.getDmg(target, DamageLib.SpellType.BOTRK);
                        if(hasCutGlass || ObjectManager.Player.Health + damage < ObjectManager.Player.MaxHealth)
                        Items.UseItem(itemId, target);
                    }
                }
            }
        }
    }
}