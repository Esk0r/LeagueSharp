#region

using System;
using System.Drawing;
using System.Linq;
using System.Net.Sockets;
using LeagueSharp;
using LeagueSharp.Common;

#endregion

namespace Marksman
{
    internal class Urgot : Champion
    {
        public Spell Q;
        public Spell QEx;
        public Spell E;
        public Spell R;

        public bool ShowUlt;
        public string UltTarget;

        public Urgot()
        {
            Utils.PrintMessage("Urgot loaded.");

            Q = new Spell(SpellSlot.Q, 1000);
            QEx = new Spell(SpellSlot.Q, 1750);
            E = new Spell(SpellSlot.E, 900);
            R = new Spell(SpellSlot.R, 700);

            Q.SetSkillshot(0.10f, 100f, 1600f, true, SkillshotType.SkillshotLine);
            QEx.SetSkillshot(0.10f, 100f, 1600f, false, SkillshotType.SkillshotLine);
            E.SetSkillshot(0.283f, 0f, 1750f, false, SkillshotType.SkillshotCircle);
            R.SetTargetted(1f, 100f);
        }

        public void AntiGapcloser_OnEnemyGapcloser(ActiveGapcloser gapcloser)
        {
            if (E.IsReady() && gapcloser.Sender.IsValidTarget(E.Range))
                E.CastOnUnit(gapcloser.Sender);
        }

        public static bool UnderAllyTurret(Obj_AI_Base vTarget)
        {
            using (var enumerator = ObjectManager.Get<Obj_AI_Turret>().GetEnumerator())
            {
                while (enumerator.MoveNext())
                {
                    Obj_AI_Turret current = enumerator.Current;
                    if (current == null || !current.IsValid || current.Health <= 0f ||
                        SharpDX.Vector2.Distance(vTarget.Position.To2D(), current.Position.To2D()) >= 950f &&
                        current.Name.Contains("TurretShrine")) 
                    {
                        continue;
                    }
                    return true;
                }
                return false;
            }
        }

        public static bool TeleportTurret(Obj_AI_Hero vTarget)
        {
            return
                ObjectManager.Get<Obj_AI_Hero>()
                    .Any(player => !player.IsDead && player.IsMe && UnderAllyTurret(ObjectManager.Player));
        }

        public static int UnderTurretEnemyMinion
        {
            get { return ObjectManager.Get<Obj_AI_Minion>().Count(xMinion => xMinion.IsEnemy && UnderAllyTurret(xMinion)); }
        }

        public override void Drawing_OnDraw(EventArgs args)
        {
            Spell[] spellList = { Q, QEx, E, R };
            foreach (var spell in spellList)
            {
                var menuItem = GetValue<Circle>("Draw" + spell.Slot);
                if (menuItem.Active)
                    Utility.DrawCircle(ObjectManager.Player.Position, spell.Range, menuItem.Color);
            }

            var drawUlt = GetValue<Circle>("DrawUlt");
            if (drawUlt.Active && ShowUlt)
            {
                //var playerPos = Drawing.WorldToScreen(ObjectManager.Player.Position);
                //Drawing.DrawText(playerPos.X - 65, playerPos.Y + 20, drawUlt.Color, "Hit R To kill " + UltTarget + "!");
            }
        }

        public override void Game_OnGameUpdate(EventArgs args)
        {
            R.Range = 150 * R.Level + 400;

            Obj_AI_Hero vTarget;

            if ((!ComboActive && !HarassActive) || !Orbwalking.CanMove(100)) return;

            var useQ = GetValue<bool>("UseQ" + (ComboActive ? "C" : "H"));
            var useE = GetValue<bool>("UseEC");
            var useR = GetValue<bool>("UseRC");

            if (Q.IsReady() && useQ)
            {
                vTarget = SimpleTs.GetTarget(Q.Range, SimpleTs.DamageType.Physical);
                if (vTarget != null)
                    Q.Cast(vTarget, false, true);
            }

            if (E.IsReady() && useE)
            {
                vTarget = SimpleTs.GetTarget(E.Range, SimpleTs.DamageType.Physical);
                if (vTarget != null)
                    E.Cast(vTarget);
            }

            if (R.IsReady() && useR)
            {
                vTarget = SimpleTs.GetTarget(R.Range, SimpleTs.DamageType.Physical);
                if (vTarget != null && vTarget.Health <= R.GetDamage(vTarget) &&
                    !Orbwalking.InAutoAttackRange(vTarget))
                {
                    R.CastOnUnit(vTarget);
                }
            }
        }

        public override void Orbwalking_AfterAttack(Obj_AI_Base unit, Obj_AI_Base target)
        {
            if ((!ComboActive && !HarassActive) || unit.IsMe || (!(target is Obj_AI_Hero))) return;

            var useQ = GetValue<bool>("UseQ" + (ComboActive ? "C" : "H"));
            if (useQ)
                Q.Cast(target, false, true);
        }

        public override bool ComboMenu(Menu config)
        {
            config.AddItem(new MenuItem("UseQC" + Id, "Use Q").SetValue(true));
            config.AddItem(new MenuItem("UseEC" + Id, "Use E").SetValue(true));
            config.AddItem(new MenuItem("UseRC" + Id, "Use R").SetValue(true));
            config.AddSubMenu(new Menu("Don't Use Ult on", "DontUlt"));
           /* foreach (var enemy in ObjectManager.Get<Obj_AI_Hero>().Where(enemy => enemy.Team != ObjectManager.Player.Team))
            {
                Config.SubMenu("Combo")
                    .AddItem(
                        new MenuItem(string.Format("DontUlt{0}", enemy.BaseSkinName), enemy.BaseSkinName).SetValue(false));
            }
            * */
            return true;
        }

        public override bool HarassMenu(Menu config)
        {
            config.AddItem(new MenuItem("UseQH" + Id, "Use Q").SetValue(true));
            return true;
        }

        public override bool DrawingMenu(Menu config)
        {
            config.AddItem(
                new MenuItem("DrawQ" + Id, "Q range").SetValue(new Circle(true, Color.LightGray)));
            config.AddItem(
                new MenuItem("DrawE" + Id, "E range").SetValue(new Circle(false, Color.LightGray)));
            config.AddItem(
                new MenuItem("DrawR" + Id, "R range").SetValue(new Circle(false, Color.LightGray)));
            config.AddItem(
                new MenuItem("DrawQEx" + Id, "Corrosive Charge").SetValue(new Circle(true, Color.LightGray)));
            return true;
        }

        public override bool MiscMenu(Menu config)
        {
            config.AddItem(new MenuItem("UltOption1" + Id, "Move Target Under Turret").SetValue(true));
            config.AddItem(new MenuItem("UltOption2" + Id, "Move Target My Team").SetValue(true));
            return true;
        }
    }
}