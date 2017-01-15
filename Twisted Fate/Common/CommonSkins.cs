using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LeagueSharp;
using LeagueSharp.Common;
namespace TwistedFate.Common
{
    internal class CommonSkins
    {
        public static Menu MenuLocal { get; private set; }

        public static void Init(Menu MenuParent)
        {
            MenuLocal = new Menu("Skins", "MenuSkin");
            {
                MenuLocal.AddItem(new MenuItem("Settings.Skin", "Skin:").SetValue(false)).ValueChanged +=
                    (sender, args) =>
                    {
                        if (!args.GetNewValue<bool>())
                        {
                            ObjectManager.Player.SetSkin(ObjectManager.Player.CharData.BaseSkinName, ObjectManager.Player.BaseSkinId);
                        }
                    };

                string[] strSkins = new[]
                {
                    "Classic Twisted Fate", "PAX Twisted Fate", "Jack of Hearts Twisted Fate", "The Magnificent Twisted Fate",
                    "Tango Twisted Fate", "High Noon Fate", "Musketeer Twisted Fate", "Underworld Twisted Fate",
                    "Red Card Twisted Fate", "Cutpurse Twisted Fate"
                };

                MenuLocal.AddItem(new MenuItem("Settings.SkinID", "Skin Name:").SetValue(new StringList(strSkins, 0)));
            }
            MenuParent.AddSubMenu(MenuLocal);

            Game.OnUpdate += GameOnOnUpdate;
        }

        private static void GameOnOnUpdate(EventArgs args)
        {
            if (MenuLocal.Item("Settings.Skin").GetValue<bool>())
            {
                ObjectManager.Player.SetSkin(ObjectManager.Player.CharData.BaseSkinName, MenuLocal.Item("Settings.SkinID").GetValue<StringList>().SelectedIndex);
            }
        }
    }
}
