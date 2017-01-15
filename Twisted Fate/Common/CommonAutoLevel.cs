using System;
using System.Drawing;
using System.Linq;
using LeagueSharp;
using System.Collections.Generic;
using LeagueSharp.Common;
using Color = SharpDX.Color;
using System.Windows.Input;

namespace TwistedFate.Common
{
    internal class CommonAutoLevel
    {
        public static Menu MenuLocal;

        public static int[] SpellLevels;

        public static void Init(Menu parentMenu)
        {
            MenuLocal = new Menu("Auto Level", "Auto Level").SetFontStyle(FontStyle.Regular, Color.Aquamarine);
            MenuLocal.AddItem(new MenuItem("AutoLevel.Set", "at Start:").SetValue(new StringList(new[] { "Allways Off", "Allways On", "Remember Last Settings" }, 2)));
            MenuLocal.AddItem(new MenuItem("AutoLevel.Active", "Auto Level Active!").SetValue(new KeyBind("L".ToCharArray()[0],KeyBindType.Toggle))).Permashow(true, ObjectManager.Player.ChampionName + " | " + "Auto Level Up", Color.GreenYellow);

            SpellLevels = new[] { 2, 1, 3, 1, 1, 4, 1, 2, 1, 2, 4, 2, 2, 3, 3, 4, 3, 3 };
            MenuLocal.AddItem(new MenuItem("AutoLevel." + ObjectManager.Player.ChampionName, GetLevelList(SpellLevels)));

            switch (MenuLocal.Item("AutoLevel.Set").GetValue<StringList>().SelectedIndex)
            {
                case 0:
                    MenuLocal.Item("AutoLevel.Active")
                        .SetValue(new KeyBind("L".ToCharArray()[0], KeyBindType.Toggle));
                    break;

                case 1:
                    MenuLocal.Item("AutoLevel.Active")
                        .SetValue(new KeyBind("L".ToCharArray()[0], KeyBindType.Toggle, true));
                    break;
            }

            parentMenu.AddSubMenu(MenuLocal);

            Game.OnUpdate += Game_OnUpdate;

        }

        private static string GetLevelList(int[] spellLevels)
        {
            var a = new[] { "Q", "W", "E", "R" };
            var b = spellLevels.Aggregate("", (c, i) => c + (a[i - 1] + " - "));
            return b != "" ? b.Substring(0, b.Length - (17 * 3)) : "";
        }

        private static int GetRandomDelay
        {
            get
            {
                var rnd = new Random(DateTime.Now.Millisecond);
                return rnd.Next(750, 1000);
            }

        }

        private static void Game_OnUpdate(EventArgs args)
        {
            if (!MenuLocal.Item("AutoLevel.Active").GetValue<KeyBind>().Active)
            {
                return;
            }

            var qLevel = ObjectManager.Player.Spellbook.GetSpell(SpellSlot.Q).Level;
            var wLevel = ObjectManager.Player.Spellbook.GetSpell(SpellSlot.W).Level;
            var eLevel = ObjectManager.Player.Spellbook.GetSpell(SpellSlot.E).Level;
            var rLevel = ObjectManager.Player.Spellbook.GetSpell(SpellSlot.R).Level;

            if (qLevel + wLevel + eLevel + rLevel >= ObjectManager.Player.Level)
            {
                return;
            }

            var level = new[] { 0, 0, 0, 0 };
            for (var i = 0; i < ObjectManager.Player.Level; i++)
            {
                level[SpellLevels[i] - 1] = level[SpellLevels[i] - 1] + 1;
            }

            if (qLevel < level[0])
            {

                Utility.DelayAction.Add(GetRandomDelay, () => ObjectManager.Player.Spellbook.LevelSpell(SpellSlot.Q));
            }

            if (wLevel < level[1])
            {
                Utility.DelayAction.Add(GetRandomDelay, () => ObjectManager.Player.Spellbook.LevelSpell(SpellSlot.W));
            }

            if (eLevel < level[2])
            {
                Utility.DelayAction.Add(GetRandomDelay, () => ObjectManager.Player.Spellbook.LevelSpell(SpellSlot.E));
            }

            if (rLevel < level[3])
            {
                Utility.DelayAction.Add(GetRandomDelay, () => ObjectManager.Player.Spellbook.LevelSpell(SpellSlot.R));
            }
        }
    }
}