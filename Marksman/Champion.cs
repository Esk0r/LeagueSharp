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

        public virtual void LaneClearMenu(Menu config)
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

        public virtual void Drawing_OnDraw(EventArgs args)
        {
        }

        public virtual void Game_OnGameUpdate(EventArgs args)
        {
        }

        public virtual void Orbwalking_AfterAttack(Obj_AI_Base unit, Obj_AI_Base target)
        {
        }

        public virtual void Orbwalking_BeforeAttack(Orbwalking.BeforeAttackEventArgs args)
        {
        }
    }
}