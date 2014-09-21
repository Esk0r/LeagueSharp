#region

using System;
using System.Linq;
using LeagueSharp;
using LeagueSharp.Common;

#endregion

namespace Marksman
{
    internal class Vayne : Champion
    {
        public Spell E;
        public Spell Q;

        public Vayne()
        {
            Utils.PrintMessage("Vayne loaded");

            Q = new Spell(SpellSlot.Q, 0f);
            E = new Spell(SpellSlot.E, float.MaxValue);

            E.SetTargetted(0.25f, 2200f);

            AntiGapcloser.OnEnemyGapcloser += AntiGapcloser_OnEnemyGapcloser;
            Interrupter.OnPossibleToInterrupt += Interrupter_OnPossibleToInterrupt;
        }

        public void AntiGapcloser_OnEnemyGapcloser(ActiveGapcloser gapcloser)
        {
            if (E.IsReady() && gapcloser.Sender.IsValidTarget(E.Range))
                E.CastOnUnit(gapcloser.Sender);
        }

        public void Interrupter_OnPossibleToInterrupt(Obj_AI_Base unit, InterruptableSpell spell)
        {
            if (GetValue<bool>("UseEInterrupt") && unit.IsValidTarget(550f))
                E.Cast(unit);
        }

        public override void Game_OnGameUpdate(EventArgs args)
        {
            if ((!E.IsReady()) ||
                ((Orbwalker.ActiveMode != Orbwalking.OrbwalkingMode.Combo || !GetValue<bool>("UseEC")) &&
                 !GetValue<KeyBind>("UseET").Active)) return;

            foreach (var hero in from hero in ObjectManager.Get<Obj_AI_Hero>().Where(hero => hero.IsValidTarget(550f))
                let prediction = E.GetPrediction(hero)
                where NavMesh.GetCollisionFlags(
                    prediction.UnitPosition.To2D()
                        .Extend(ObjectManager.Player.ServerPosition.To2D(), -GetValue<Slider>("PushDistance").Value)
                        .To3D())
                    .HasFlag(CollisionFlags.Wall) || NavMesh.GetCollisionFlags(
                        prediction.UnitPosition.To2D()
                            .Extend(ObjectManager.Player.ServerPosition.To2D(),
                                -(GetValue<Slider>("PushDistance").Value / 2))
                            .To3D())
                        .HasFlag(CollisionFlags.Wall)
                select hero)
            {
                E.Cast(hero);
            }
        }

        public override void Orbwalking_AfterAttack(Obj_AI_Base unit, Obj_AI_Base target)
        {
            if (unit.IsMe && Orbwalker.ActiveMode == Orbwalking.OrbwalkingMode.Combo && GetValue<bool>("UseQC"))
                Q.Cast(Game.CursorPos);
        }

        public override void ComboMenu(Menu config)
        {
            config.AddItem(new MenuItem("UseQC" + Id, "Use Q").SetValue(true));
            config.AddItem(new MenuItem("UseEC" + Id, "Use E").SetValue(true));
        }

        public override void MiscMenu(Menu config)
        {
            config.AddItem(
                new MenuItem("UseET" + Id, "Use E (Toggle)").SetValue(
                    new KeyBind("T".ToCharArray()[0], KeyBindType.Toggle)));
            config.AddItem(new MenuItem("UseEInterrupt" + Id, "Use E To Interrupt").SetValue(true));
            config.AddItem(
                new MenuItem("PushDistance" + Id, "E Push Distance").SetValue(new Slider(425, 475, 300)));
        }
    }
}
