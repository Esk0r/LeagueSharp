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
        public static Spell Q;
        public static Spell E;

        public Vayne()
        {
            Q = new Spell(SpellSlot.Q, 300f);
            E = new Spell(SpellSlot.E, 650f);

            E.SetTargetted(0.25f, 2200f);

            AntiGapcloser.OnEnemyGapcloser += AntiGapcloser_OnEnemyGapcloser;
            Interrupter.OnPossibleToInterrupt += Interrupter_OnPossibleToInterrupt;

            Utils.PrintMessage("Vayne loaded");
        }

        public class VayneData
        {
            public static int GetSilverBuffMarkedCount
            {
                get
                {
                    if (GetSilverBuffMarkedEnemy == null)
                        return 0;

                    return
                        GetSilverBuffMarkedEnemy.Buffs.Where(buff => buff.Name == "vaynesilvereddebuff")
                            .Select(xBuff => xBuff.Count)
                            .FirstOrDefault();
                }
            }

            public static Obj_AI_Hero GetSilverBuffMarkedEnemy
            {
                get
                {
                    return
                        ObjectManager.Get<Obj_AI_Hero>()
                            .Where(
                                enemy =>
                                    !enemy.IsDead &&
                                    enemy.IsValidTarget(
                                        (Q.IsReady() ? Q.Range : 0) +
                                        Orbwalking.GetRealAutoAttackRange(ObjectManager.Player)))
                            .FirstOrDefault(
                                enemy => enemy.Buffs.Any(buff => buff.Name == "vaynesilvereddebuff" && buff.Count > 0));
                }
            }            
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
            if ((!ComboActive && !HarassActive) || !Orbwalking.CanMove(100))
            {
                if (GetValue<bool>("FocusW"))
                {
                    var silverBuffMarkedEnemy = VayneData.GetSilverBuffMarkedEnemy;
                    if (silverBuffMarkedEnemy != null)
                    {
                        TargetSelector.SetTarget(silverBuffMarkedEnemy);
                    }
                    else
                    {
                        var attackRange = Orbwalking.GetRealAutoAttackRange(ObjectManager.Player);
                        TargetSelector.SetTarget(
                            TargetSelector.GetTarget(attackRange, TargetSelector.DamageType.Physical));
                    }
                }

                var useE = GetValue<bool>("UseE" + (ComboActive ? "C" : "H"));

                if (Q.IsReady() && GetValue<bool>("CompleteSilverBuff"))
                {
                    if (VayneData.GetSilverBuffMarkedEnemy != null && VayneData.GetSilverBuffMarkedCount == 2)
                    {
                        Q.Cast(Game.CursorPos);
                    }
                }

                if (E.IsReady() && useE)
                {
                    foreach (var hero in
                        from hero in ObjectManager.Get<Obj_AI_Hero>().Where(hero => hero.IsValidTarget(550f))
                        let prediction = E.GetPrediction(hero)
                        where
                            NavMesh.GetCollisionFlags(
                                prediction.UnitPosition.To2D()
                                    .Extend(
                                        ObjectManager.Player.ServerPosition.To2D(),
                                        -GetValue<Slider>("PushDistance").Value)
                                    .To3D()).HasFlag(CollisionFlags.Wall) ||
                            NavMesh.GetCollisionFlags(
                                prediction.UnitPosition.To2D()
                                    .Extend(
                                        ObjectManager.Player.ServerPosition.To2D(),
                                        -(GetValue<Slider>("PushDistance").Value / 2))
                                    .To3D()).HasFlag(CollisionFlags.Wall)
                        select hero)
                    {
                        E.Cast(hero);
                    }
                }
            }

            if (LaneClearActive)
            {
                var useQ = GetValue<bool>("UseQL");

                if (Q.IsReady() && useQ)
                {
                    var vMinions = MinionManager.GetMinions(ObjectManager.Player.Position, Q.Range);
                    foreach (var minions in
                        vMinions.Where(
                            minions => minions.Health < ObjectManager.Player.GetSpellDamage(minions, SpellSlot.Q)))
                        Q.Cast(minions);
                }
            }
        }

        public override void Orbwalking_AfterAttack(AttackableUnit unit, AttackableUnit target)
        {
            var useQ =
                GetValue<bool>(
                    "UseQ" +
                    (ComboActive
                        ? Orbwalker.ActiveMode == Orbwalking.OrbwalkingMode.Combo ? "C" : ""
                        : Orbwalker.ActiveMode == Orbwalking.OrbwalkingMode.Mixed ? "H" : ""));
            if (unit.IsMe && useQ)
                Q.Cast(Game.CursorPos);
        }

        public override bool ComboMenu(Menu config)
        {
            config.AddItem(new MenuItem("UseQC" + Id, "Use Q").SetValue(true));
            config.AddItem(new MenuItem("UseEC" + Id, "Use E").SetValue(true));
            config.AddItem(new MenuItem("FocusW" + Id, "Force Focus Marked Enemy").SetValue(true));
            return true;
        }

        public override bool HarassMenu(Menu config)
        {
            config.AddItem(new MenuItem("UseQH" + Id, "Use Q").SetValue(true));
            config.AddItem(new MenuItem("UseEH" + Id, "Use E").SetValue(true));
            return true;
        }

        public override bool MiscMenu(Menu config)
        {
            config.AddItem(new MenuItem("UseET" + Id, "Use E (Toggle)").SetValue(new KeyBind("T".ToCharArray()[0], KeyBindType.Toggle)));
            config.AddItem(new MenuItem("UseEInterrupt" + Id, "Use E To Interrupt").SetValue(true));
            config.AddItem(new MenuItem("PushDistance" + Id, "E Push Distance").SetValue(new Slider(425, 475, 300)));
            config.AddItem(new MenuItem("CompleteSilverBuff" + Id, "Complete Silver Buff With Q").SetValue(true));
            return true;
        }

        public override bool ExtrasMenu(Menu config)
        {

            return true;
        }

        public override bool LaneClearMenu(Menu config)
        {
            config.AddItem(new MenuItem("UseQL" + Id, "Use Q").SetValue(true));
            return true;
        }

        public override bool DrawingMenu(Menu config)
        {
            config.AddItem(
                new MenuItem("DrawE" + Id, "E range").SetValue(
                    new Circle(true, System.Drawing.Color.FromArgb(100, 255, 0, 255))));

            return true;
        }

        public override void Drawing_OnDraw(EventArgs args)
        {
            var menuItem = GetValue<Circle>("DrawE");
            if (menuItem.Active)
            {
                Render.Circle.DrawCircle(ObjectManager.Player.Position, E.Range, menuItem.Color, 1);
            }
        }
    }
}
