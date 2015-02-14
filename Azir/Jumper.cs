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
        public static void Jump()
        {
            if(Program.E.IsReady())
            {
                var extended = ObjectManager.Player.ServerPosition.To2D().Extend(Game.CursorPos.To2D(), Program.Q.Range);

                if (Program.W.IsReady() && (SoldiersManager.AllSoldiers2.Count == 0 || Program.Q.Instance.State == SpellState.Cooldown && SoldiersManager.AllSoldiers2.Min(s => s.Distance(extended, true)) >= Program.Player.Distance(extended, true)))
                {
                    Program.W.Cast(extended);

                    if(Program.Q.Instance.State != SpellState.Cooldown)
                    {
                        var extended2 = ObjectManager.Player.ServerPosition.To2D().Extend(Game.CursorPos.To2D(), Program.W.Range);
                        if(Utility.IsWall(extended2))
                        {
                            Utility.DelayAction.Add(300, () => Program.E.Cast(extended, true));
                            Utility.DelayAction.Add(250, () => Program.Q.Cast(extended, true));
                        }
                        else
                        {
                            Utility.DelayAction.Add(200, () => Program.E.Cast(extended, true));
                            Utility.DelayAction.Add(250, () => Program.Q.Cast(extended, true));
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
                    if(closestSoldier.Distance(extended, true) < ObjectManager.Player.Distance(extended, true) && ObjectManager.Player.Distance(closestSoldier, true) < Program.W.RangeSqr)
                    {
                              //E first
                    }
                }

                  /*
                var closestSoldier = SoldiersManager.AllSoldiers2.MinOrDefault(s => s.Distance(_point, true));
                if (closestSoldier.Distance(_point, true) < 200 * 200)
                {
                    Program.E.Cast(closestSoldier.Position, true);
                    _reqTick = 0;
                    return;
                }

                if (Program.E.IsReady() && SoldiersManager.AllSoldiers2.Max(s => s.Distance(extended, true)) < Math.Pow(Program.Q.Range - 400, 2))
                {
                    Program.E.Cast(extended.To3D(), true);
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
                }  */
            }
        }
    }
}
