using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SharpDX;
using LeagueSharp;
using LeagueSharp.Common;

namespace Azir
{
    internal static class Jumper
    {
        private static Vector2 _point;
        private static int _reqTick;
        private static int _jumpType; // 1 => W -> E | 2 => W -> Q -> E

        static Jumper()
        {
            Game.OnGameUpdate += Game_OnGameUpdate;
        }

        static void Game_OnGameUpdate(EventArgs args)
        {
            if (Environment.TickCount - _reqTick < 1000)
            {
                var extended = ObjectManager.Player.ServerPosition.To2D().Extend(_point, Program.Q.Range);

                if (SoldiersManager.AllSoldiers2.Count == 0 || _jumpType == 1 && SoldiersManager.AllSoldiers2.Min(s => s.Distance(extended, true)) >= Program.Player.Distance(extended, true))
                {
                    Program.W.Cast(_point);
                    return;
                }

                if (_jumpType == 2)
                {
                    if (Program.Q.IsReady())
                    {
                        Program.Q.Cast(extended.To3D(), true);
                        return;
                    }
                }

                if (Program.E.IsReady())
                {
                    Program.E.Cast(extended.To3D(), true);
                    return;
                }
            }
        }

        public static void Jump()
        {
            if(Program.E.IsReady() && Environment.TickCount - _reqTick > 300)
            {
                _jumpType = Program.Q.Instance.State != SpellState.Cooldown ? 2 : 1;
                _point = Game.CursorPos.To2D();
                _reqTick = Environment.TickCount;
            }
        }
    }
}
