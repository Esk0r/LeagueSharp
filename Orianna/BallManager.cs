using System;
using System.Linq;
using LeagueSharp;
using LeagueSharp.Common;
using SharpDX;

namespace Orianna
{
    public static class BallManager
    {
        public static Vector3 BallPosition { get; private set; }
        private static int _sTick = Utils.GameTimeTickCount;

        static BallManager()
        {
            Game.OnUpdate += Game_OnGameUpdate;
            Obj_AI_Hero.OnProcessSpellCast += Obj_AI_Hero_OnProcessSpellCast;
            BallPosition = ObjectManager.Player.Position;
        }

        static void Obj_AI_Hero_OnProcessSpellCast(Obj_AI_Base sender, GameObjectProcessSpellCastEventArgs args)
        {
            if (sender.IsMe)
            {
                switch (args.SData.Name)
                {
                    case "OrianaIzunaCommand":
                        Utility.DelayAction.Add((int)(BallPosition.Distance(args.End) / 1.2 - 70 - Game.Ping), () => BallPosition = args.End);
                        BallPosition = Vector3.Zero;
                        _sTick = Utils.GameTimeTickCount;
                        break;

                    case "OrianaRedactCommand":
                        BallPosition = Vector3.Zero;
                        _sTick = Utils.GameTimeTickCount;
                    break;
                }
            }
        }

        static void Game_OnGameUpdate(EventArgs args)
        {
            if (Utils.GameTimeTickCount - _sTick > 300 && ObjectManager.Player.HasBuff("OrianaGhostSelf"))
            {
                BallPosition = ObjectManager.Player.Position;
            }

            foreach (var ally in HeroManager.Allies)
            {
                if (ally.HasBuff("OrianaGhost"))
                {
                    BallPosition = ally.Position;
                }
            }
        }
    }
}
