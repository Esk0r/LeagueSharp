using System.Collections.Generic;
using System.Linq;
using LeagueSharp;
using LeagueSharp.Common;

// ReSharper disable InconsistentNaming

namespace Azir
{
    class AzirWalker : Orbwalking.Orbwalker
    {
        private const int _soldierAARange = 250;

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
            return CustomInAutoattackRange(target) != 0;
        }

        public int CustomInAutoattackRange(AttackableUnit target)
        {
            if (Orbwalking.InAutoAttackRange(target))
            {
                return 1;
            }

            if (!target.IsValidTarget())
            {
                return 0;
            }

            //Azir's soldiers can't attack structures.
            if (!(target is Obj_AI_Base))
            {
                return 0;
            }

            var soldierAArange = _soldierAARange + 65 + target.BoundingRadius;
            soldierAArange *= soldierAArange;
            return SoldiersManager.ActiveSoldiers.Any(soldier => soldier.Distance(target, true) <= soldierAArange) ? 2 : 0;
        }

        public override AttackableUnit GetTarget()
        {
            if (ActiveMode == Orbwalking.OrbwalkingMode.LaneClear || ActiveMode == Orbwalking.OrbwalkingMode.Mixed ||
                ActiveMode == Orbwalking.OrbwalkingMode.LastHit)
            {
                foreach (var minion in
                    ObjectManager.Get<Obj_AI_Minion>()
                        .Where(
                            minion =>
                                minion.IsValidTarget() &&
                                minion.Health <
                                3 *
                                (ObjectManager.Player.BaseAttackDamage + ObjectManager.Player.FlatPhysicalDamageMod))
                    )
                {
                    var r = CustomInAutoattackRange(minion);
                    if (r == 0) continue;
                    var t = (int)(Program.Player.AttackCastDelay * 1000) - 100 + Game.Ping / 2;
                    var predHealth = HealthPrediction.GetHealthPrediction(minion, t, 0);

                    var damage = (r == 1) ? Program.Player.GetAutoAttackDamage(minion, true) : Program.Player.GetSpellDamage(minion, SpellSlot.W);
                    if (minion.Team == GameObjectTeam.Neutral || !MinionManager.IsMinion(minion, true)) continue;
                    if (predHealth > 0 && predHealth <= damage)
                    {
                        return minion;
                    }
                }
            }

            if(ActiveMode != Orbwalking.OrbwalkingMode.LastHit)
            {
                var posibleTargets = new Dictionary<Obj_AI_Base, float>();
                var autoAttackTarget = TargetSelector.GetTarget(-1, TargetSelector.DamageType.Physical);
                if (autoAttackTarget.IsValidTarget())
                {
                    posibleTargets.Add(autoAttackTarget, GetDamageValue(autoAttackTarget, false));
                }

                foreach (var soldierTarget in SoldiersManager.ActiveSoldiers.Select(soldier => TargetSelector.GetTarget(_soldierAARange + 65 + 65, TargetSelector.DamageType.Magical, true, null, soldier.ServerPosition)).Where(soldierTarget => soldierTarget.IsValidTarget()))
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

                if(posibleTargets.Count > 0)
                {
                    return posibleTargets.MinOrDefault(p => p.Value).Key;
                }
                var soldiers = SoldiersManager.ActiveSoldiers;
                if(soldiers.Count > 0)
                {
                    var minions = MinionManager.GetMinions(1100, MinionTypes.All, MinionTeam.NotAlly);
                    var validEnemiesPosition = HeroManager.Enemies.Where(e => e.IsValidTarget(1100)).Select(e => e.ServerPosition.To2D()).ToList();
                    const int AAWidthSqr = 100*100;
                    //Try to harass using minions
                    foreach (var soldier in soldiers)
                    {
                        foreach (var minion in minions)
                        {
                            var soldierAArange = _soldierAARange + 65 + minion.BoundingRadius;
                            soldierAArange *= soldierAArange;
                            if (!(soldier.Distance(minion, true) < soldierAArange)) continue;
                            var p1 = minion.Position.To2D();
                            var p2 = soldier.Position.To2D().Extend(minion.Position.To2D(), 375);
                            if (validEnemiesPosition.Any(enemyPosition => enemyPosition.Distance(p1, p2, true, true) < AAWidthSqr))
                            {
                                return minion;
                            }
                        }
                    }
                }
            }

            /* turrets / inhibitors / nexus */
            if (ActiveMode == Orbwalking.OrbwalkingMode.LaneClear)
            {
                /* turrets */
                foreach (var turret in
                    ObjectManager.Get<Obj_AI_Turret>().Where(t => t.IsValidTarget() && Orbwalking.InAutoAttackRange(t)))
                {
                    return turret;
                }

                /* inhibitor */
                foreach (var turret in
                    ObjectManager.Get<Obj_BarracksDampener>().Where(t => t.IsValidTarget() && Orbwalking.InAutoAttackRange(t)))
                {
                    return turret;
                }

                /* nexus */
                foreach (var nexus in
                    ObjectManager.Get<Obj_HQ>().Where(t => t.IsValidTarget() && Orbwalking.InAutoAttackRange(t)))
                {
                    return nexus;
                }
            }

            /*Jungle minions*/
            if (ActiveMode != Orbwalking.OrbwalkingMode.LaneClear && ActiveMode != Orbwalking.OrbwalkingMode.Mixed)
                return ActiveMode == Orbwalking.OrbwalkingMode.LaneClear
                    ? (ObjectManager.Get<Obj_AI_Minion>()
                        .Where(minion => minion.IsValidTarget() && InAutoAttackRange(minion))).MaxOrDefault(
                            m => CustomInAutoattackRange(m)*m.Health)
                    : null;
            AttackableUnit result = ObjectManager.Get<Obj_AI_Minion>()
                .Where(
                    mob =>
                        mob.IsValidTarget() && Orbwalking.InAutoAttackRange(mob) && mob.Team == GameObjectTeam.Neutral)
                .MaxOrDefault(mob => mob.MaxHealth);
            if (result != null)
            {
                return result;
            }

            return ActiveMode == Orbwalking.OrbwalkingMode.LaneClear ? (ObjectManager.Get<Obj_AI_Minion>().Where(minion => minion.IsValidTarget() && InAutoAttackRange(minion))).MaxOrDefault(m => CustomInAutoattackRange(m) * m.Health) : null;
        }
    }
}
