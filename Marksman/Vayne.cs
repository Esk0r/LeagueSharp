#region

using System;
using LeagueSharp;
using LeagueSharp.Common;
using SharpDX;

#endregion

namespace Marksman
{
    internal class Vayne : Champion
    {
        public Spell E;
        public Spell Q;
        public Spell QE;

        public Vayne()
        {
            Utils.PrintMessage("Vayne loaded");

            Q = new Spell(SpellSlot.Q, 0f);
            E = new Spell(SpellSlot.E, 550f);
            QE = new Spell(SpellSlot.Q, 750f);

            E.SetTargetted(0.25f, 2200f);
            QE.SetTargetted(0.50f, 2200f);

            AntiGapcloser.OnEnemyGapcloser += AntiGapcloser_OnEnemyGapcloser;
            Interrupter.OnPosibleToInterrupt += Interrupter_OnPosibleToInterrupt;
        }

        public void AntiGapcloser_OnEnemyGapcloser(ActiveGapcloser gapcloser)
        {
            if (E.IsReady() && gapcloser.Sender.IsValidTarget(E.Range))
            {
                E.CastOnUnit(gapcloser.Sender);
            }
        }

        public void Interrupter_OnPosibleToInterrupt(Obj_AI_Base unit, InterruptableSpell spell)
        {
            if (GetValue<bool>("UseEInterrupt") && unit.IsValidTarget(550f))
            {
                E.Cast(unit);
            }
        }


        public override void Game_OnGameUpdate(EventArgs args)
        {
            if ((!E.IsReady()) ||
                ((Orbwalker.ActiveMode != Orbwalking.OrbwalkingMode.Combo || !GetValue<bool>("UseEC")) &&
                 !GetValue<KeyBind>("UseET").Active))
            {
                return;
            }

            var normalTarget = SimpleTs.GetTarget(E.Range, SimpleTs.DamageType.Physical);
            var qeTarget = SimpleTs.GetTarget(200 + E.Range, SimpleTs.DamageType.Physical);
            if (normalTarget.IsValid)
            {
                if (IsStunable(normalTarget))
                {
                    E.Cast(normalTarget);
                }
            }
            else if (qeTarget.IsValid)
            {
                if (!IsStunable(qeTarget) || !GetValue<bool>("UseQEC") || !Q.IsReady())
                {
                    return;
                }
                Q.Cast(qeTarget);
                Utility.DelayAction.Add(250, () => E.Cast(qeTarget));
            }
        }


        public override void Orbwalking_AfterAttack(Obj_AI_Base unit, Obj_AI_Base target)
        {
            if (unit.IsMe && Orbwalker.ActiveMode == Orbwalking.OrbwalkingMode.Combo && GetValue<bool>("UseQC"))
            {
                CastTumble((Obj_AI_Hero) target);
            }
        }

        public bool IsStunable(Obj_AI_Hero enemy)
        {
            var prediction =
                Vector3.DistanceSquared(enemy.ServerPosition, ObjectManager.Player.ServerPosition) > 550 * 550
                    ? QE.GetPrediction(enemy)
                    : E.GetPrediction(enemy);

            if (prediction.Hitchance.Equals(HitChance.OutOfRange))
            {
                return false;
            }

            for (var i = 0; i < GetValue<Slider>("PushDistance").Value; i += (int) enemy.BoundingRadius)
            {
                if (
                    NavMesh.GetCollisionFlags(
                        prediction.UnitPosition.To2D().Extend(ObjectManager.Player.ServerPosition.To2D(), -i).To3D())
                        .HasFlag(CollisionFlags.Wall))
                {
                    return true;
                }
            }
            return false;
        }

        public void CastTumble(Obj_AI_Hero target)
        {
            var posAfterTumble =
                ObjectManager.Player.ServerPosition.To2D().Extend(Game.CursorPos.To2D(), 300).To3D();
            var distanceAfterTumble = Vector3.DistanceSquared(posAfterTumble, target.ServerPosition);
            if (distanceAfterTumble < 550 * 550 && distanceAfterTumble > 100 * 100)
            {
                Q.Cast(Game.CursorPos);
            }
        }

        public override void ComboMenu(Menu config)
        {
            config.AddItem(new MenuItem("UseQC" + Id, "Use Q").SetValue(true));
            config.AddItem(new MenuItem("UseEC" + Id, "Use E").SetValue(true));
            config.AddItem(new MenuItem("UseQEC" + Id, "Use QE if not in E range").SetValue(false));
        }

        public override void MiscMenu(Menu config)
        {
            config.AddItem(
                new MenuItem("UseET" + Id, "Use E (Toggle)").SetValue(
                    new KeyBind("T".ToCharArray()[0], KeyBindType.Toggle)));
            config.AddItem(new MenuItem("UseEInterrupt" + Id, "Use E To Interrupt").SetValue(true));
            config.AddItem(new MenuItem("PushDistance" + Id, "E Push Distance").SetValue(new Slider(425, 475, 300)));
        }
    }
}