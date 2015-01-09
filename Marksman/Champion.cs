#region

using System;
using LeagueSharp;
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
        public bool LaneClearActive;
        public bool ToggleActive;
        public Orbwalking.Orbwalker Orbwalker;

        public T GetValue<T>(string item)
        {
            return Config.Item(item + Id).GetValue<T>();
        }

        public virtual bool ComboMenu(Menu config)
        {
            return false;
        }

        public virtual bool HarassMenu(Menu config)
        {
            return false;
        }

        public virtual bool LaneClearMenu(Menu config)
        {
            return false;
        }

        public virtual bool MiscMenu(Menu config)
        {
            return false;
        }

        public virtual bool ExtrasMenu(Menu config)
        {
            return false;
        }
        
        public virtual bool DrawingMenu(Menu config)
        {
            return false;
        }

        public virtual bool MainMenu(Menu config)
        {
            return false;
        }

        public virtual void Drawing_OnDraw(EventArgs args)
        {
        }

        public virtual void Game_OnGameUpdate(EventArgs args)
        {
        }

        public virtual void Orbwalking_AfterAttack(AttackableUnit unit, AttackableUnit target)
        {
        }

        public virtual void Orbwalking_BeforeAttack(Orbwalking.BeforeAttackEventArgs args)
        {
        }
    }
}
