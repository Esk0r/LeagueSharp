using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LeagueSharp;
using LeagueSharp.Common;
using SharpDX;
using Color = System.Drawing.Color;

namespace Evade.Pathfinding
{
    public static class PathFollower
    {
        public static List<Vector2> Path = new List<Vector2>();

        static PathFollower()
        {
            Game.OnUpdate += Game_OnUpdate;
        }

        static void Game_OnUpdate(EventArgs args)
        {
            if (Path.Count > 0)
            {
                while (Path.Count > 0 && Program.PlayerPosition.Distance(Path[0]) < 80)
                {
                    Path.RemoveAt(0);
                }
                
                if (Path.Count > 0)
                {
                    ObjectManager.Player.SendMovePacket(Path[0]);
                }
            }
        }

        public static void Follow(List<Vector2> path)
        {
            Path = path;
            Game_OnUpdate(new EventArgs());
        }

        public static void Stop()
        {
            Path = new List<Vector2>();
        }
    }
}
