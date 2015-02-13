using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LeagueSharp;
using LeagueSharp.Common;

namespace Azir
{
    class AzirWalker : Orbwalking.Orbwalker
    {
        private const int _soldierAARange = 200;

        public AzirWalker(Menu attachToMenu) : base(attachToMenu)
        {
            
        }

        private static float GetDamageValue(Obj_AI_Base target, bool soldierAttack)
        {
            var d = soldierAttack ? Program.Player.GetSpellDamage(target, SpellSlot.W) : Program.Player.GetAutoAttackDamage(target);
            return target.Health / (float)d;
        }

        public override bool InAutoAttackRange(AttackableUnit target)
        {
            if (Orbwalking.InAutoAttackRange(target))
            {
                return true;
            }

            if (!target.IsValidTarget())
            {
                return false;
            }

            //Azir's soldiers can't attack structures.
            if (!(target is Obj_AI_Base))
            {
                return false;
            }

            var soldierAArange = _soldierAARange + 65 + target.BoundingRadius;
            soldierAArange *= soldierAArange;
            foreach (var soldier in SoldiersManager.ActiveSoldiers)
            {
                if (soldier.Distance(target, true) <= soldierAArange)
                {
                    return true;
                }
            }

            return false;
        }

        public override AttackableUnit GetTarget()
        {
            if(ActiveMode != Orbwalking.OrbwalkingMode.LastHit)
            {
                var posibleTargets = new Dictionary<Obj_AI_Base, float>();
                var autoAttackTarget = TargetSelector.GetTarget(-1, TargetSelector.DamageType.Physical);
                if (autoAttackTarget.IsValidTarget())
                {
                    posibleTargets.Add(autoAttackTarget, GetDamageValue(autoAttackTarget, false));
                }

                foreach (var soldier in SoldiersManager.ActiveSoldiers)
                {
                    var soldierTarget = TargetSelector.GetTarget(_soldierAARange + 65 + 65, TargetSelector.DamageType.Magical, true, null, soldier.ServerPosition);
                    if (soldierTarget.IsValidTarget())
                    {
                        if(posibleTargets.ContainsKey(soldierTarget))
                        {
                            posibleTargets[soldierTarget] *= 1.25f;
                        }
                        else
                        {
                            posibleTargets.Add(soldierTarget, GetDamageValue(soldierTarget, true));
                        }
                    }
                }

                if(posibleTargets.Count > 0)
                {
                    return posibleTargets.MinOrDefault(p => p.Value).Key;
                }
                var soldiers = SoldiersManager.ActiveSoldiers;
                if(soldiers.Count > 0)
                {
                    var minions = MinionManager.GetMinions(1100, MinionTypes.All, MinionTeam.NotAlly);
                    var validEnemiesPosition = HeroManager.Enemies.Where(e => e.IsValidTarget(1100)).Select(e => e.ServerPosition.To2D()).ToList();
                    const int AAWidthSqr = 50*50;
                    //Try to harass using minions
                    foreach (var soldier in soldiers)
                    {
                        foreach (var minion in minions)
                        {
                            var soldierAArange = _soldierAARange + 65 + minion.BoundingRadius;
                            soldierAArange *= soldierAArange;
                            if(soldier.Distance(minion, true) < soldierAArange)
                            {
                                var p1 = minion.Position.To2D();
                                var p2 = soldier.Position.To2D().Extend(minion.Position.To2D(), 400);
                                foreach (var enemyPosition in validEnemiesPosition)
                                {
                                    if (enemyPosition.Distance(p1, p2, true, true) < AAWidthSqr)
                                    {
                                        return minion;
                                    }
                                }
                            }
                        }
                    }
                }
            }
            
            return base.GetTarget();
        }
    }
}
