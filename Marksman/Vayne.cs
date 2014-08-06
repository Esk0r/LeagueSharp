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

            Interrupter.OnPosibleToInterrupt += Interrupter_OnPosibleToInterrupt;
        }

        public void Interrupter_OnPosibleToInterrupt(Obj_AI_Base unit, InterruptableSpell spell)
        {
            if (GetValue<bool>("UseEInterrupt") && unit.IsValidTarget(550f))
                E.Cast(unit);
        }

        public override void Game_OnGameUpdate(EventArgs args)
        {
            if ((!E.IsReady()) ||
                ((Orbwalker.ActiveMode != Orbwalking.OrbwalkingMode.Combo || !GetValue<bool>("UseEC")) &&
                 !GetValue<KeyBind>("UseET").Active)) return;

            foreach (var hero in ObjectManager.Get<Obj_AI_Hero>().Where(hero => hero.IsValidTarget(550f)))
            {
                var prediction = E.GetPrediction(hero);
                if (prediction.HitChance >= Prediction.HitChance.HighHitchance)
                    if (NavMesh.GetCollisionFlags(
                        prediction.Position.To2D()
                            .Extend(ObjectManager.Player.ServerPosition.To2D(), -GetValue<Slider>("PushDistance").Value)
                            .To3D())
                        .HasFlag(CollisionFlags.Wall))
                        E.Cast(hero);
            }
        }

        public override void Orbwalking_AfterAttack(Obj_AI_Base unit, Obj_AI_Base target)
        {
            if (Orbwalker.ActiveMode == Orbwalking.OrbwalkingMode.Combo && GetValue<bool>("UseQC"))
                Q.Cast(Game.CursorPos);
        }

        public override void ComboMenu(Menu config)
        {
            config.AddItem(new MenuItem("UseQC" + Id, "Use Q").SetValue(true));
            config.AddItem(new MenuItem("UseEC" + Id, "Use E").SetValue(true));
            config.AddItem(
                new MenuItem("UseET" + Id, "Use E (Toggle)").SetValue(
                    new KeyBind("T".ToCharArray()[0], KeyBindType.Toggle)));
        }

        public override void MiscMenu(Menu config)
        {
            config.AddItem(new MenuItem("UseEInterrupt" + Id, "Use E To Interrupt").SetValue(true));
            config.AddItem(
                new MenuItem("PushDistance" + Id, "E Push Distance").SetValue(new Slider(425, 475, 300)));
        }
    }
}