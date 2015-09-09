﻿using System.Diagnostics.CodeAnalysis;
using System.Linq;
using SharpDX;
using LeagueSharp;
using LeagueSharp.Common;

namespace Azir
{
    [SuppressMessage("ReSharper", "InconsistentNaming")]
    internal static class Jumper
    {
        private static int CastQT;
        private static Vector2 CastQLocation;

        private static int CastET;
        private static Vector2 CastELocation;

        static Jumper()
        {
            Obj_AI_Base.OnProcessSpellCast += Obj_AI_Base_OnProcessSpellCast;
        }

        static void Obj_AI_Base_OnProcessSpellCast(Obj_AI_Base sender, GameObjectProcessSpellCastEventArgs args)
        {
            if (sender.IsMe)
            {
                if(args.SData.Name == "AzirE" && Utils.TickCount - CastQT < 500)
                {
                    Program.Q.Cast(CastQLocation, true);
                    CastQT = 0;
                }

                if (args.SData.Name == "AzirQ" && Utils.TickCount - CastET < 500)
                {
                    Program.E.Cast(CastELocation, true);
                    CastET = 0;
                }
            }
        }

        public static void Jump()
        {
            if (Program.E.IsReady())
            {
                var extended = ObjectManager.Player.ServerPosition.To2D().Extend(Game.CursorPos.To2D(), Program.Q.Range - 25);

                if (Program.W.IsReady() && (SoldiersManager.AllSoldiers2.Count == 0 || Program.Q.Instance.State == SpellState.Cooldown && SoldiersManager.AllSoldiers2.Min(s => s.Distance(extended, true)) >= Program.Player.Distance(extended, true)))
                {
                    Program.W.Cast(extended);

                    if(Program.Q.Instance.State != SpellState.Cooldown)
                    {
                        var extended2 = ObjectManager.Player.ServerPosition.To2D().Extend(Game.CursorPos.To2D(), Program.W.Range);
                        if (extended2.IsWall())
                        {
                            Utility.DelayAction.Add(250, () => Program.Q.Cast(extended, true));
                            CastET = Utils.TickCount + 250;
                            CastELocation = extended;
                        }
                        else
                        {
                            Utility.DelayAction.Add(250, () => Program.E.Cast(extended, true));
                            CastQT = Utils.TickCount + 250;
                            CastQLocation = extended;
                        }
                    }
                    else
                    {
                        Utility.DelayAction.Add(100, () => Program.E.Cast(extended, true));
                    }
                    return;
                }

                if(SoldiersManager.AllSoldiers2.Count > 0 && Program.Q.IsReady())
                {
                    var closestSoldier = SoldiersManager.AllSoldiers2.MinOrDefault(s => s.Distance(extended, true));
                    if(closestSoldier.Distance(extended, true) < ObjectManager.Player.Distance(extended, true) && ObjectManager.Player.Distance(closestSoldier, true) > Program.W.RangeSqr)
                    {
                        Utility.DelayAction.Add(250, () => Program.E.Cast(extended, true));
                        CastQT = Utils.TickCount + 250;
                        CastQLocation = extended;
                    }
                    else
                    {
                        Utility.DelayAction.Add(250, () => Program.Q.Cast(extended, true));
                        Utility.DelayAction.Add(600, () => Program.E.Cast(extended, true));
                    }
                }
            }
        }
    }
}
