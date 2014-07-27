#region

using LeagueSharp.Common;

#endregion

namespace Marksman
{
    internal class Champion
    {
        public bool ComboActive;
        public Menu Config;
        public bool HarassActive;
        public string Id = "";
        public Orbwalking.Orbwalker Orbwalker;

        public T GetValue<T>(string item)
        {
            return Config.Item(item + Id).GetValue<T>();
        }

        public virtual void ComboMenu(Menu config)
        {
        }

        public virtual void HarassMenu(Menu config)
        {
        }

        public virtual void MiscMenu(Menu config)
        {
        }

        public virtual void DrawingMenu(Menu config)
        {
        }

        public virtual void MainMenu(Menu config)
        {
        }
    }
}