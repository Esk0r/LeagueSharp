#region

using System;
using System.Drawing;
using LeagueSharp;
using LeagueSharp.Common;

#endregion

namespace Marksman
{
    internal class Caitlyn : Champion // Base done by Chulbul-Pandey, Drawings and improvements added by Dibes.
    {
        public Spell E;
        public Spell Q;
        public Spell R;

        public bool ShowUlt;
        public string UltTarget;

        public Caitlyn()
        {
            Utils.PrintMessage("Caitlyn loaded.");

            Q = new Spell(SpellSlot.Q, 1240);
            E = new Spell(SpellSlot.E, 800);
            R = new Spell(SpellSlot.R, 2000);

            Q.SetSkillshot(0.25f, 60f, 2000f, false, SkillshotType.SkillshotLine);
            E.SetSkillshot(0.25f, 80f, 1600f, true, SkillshotType.SkillshotLine);

            AntiGapcloser.OnEnemyGapcloser += AntiGapcloser_OnEnemyGapcloser;
        }

        public void AntiGapcloser_OnEnemyGapcloser(ActiveGapcloser gapcloser)
        {
            if (E.IsReady() && gapcloser.Sender.IsValidTarget(E.Range))
                E.CastOnUnit(gapcloser.Sender);
        }

        public override void Drawing_OnDraw(EventArgs args)
        {
            Spell[] spellList = { Q, E, R };
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
            R.Range = 500 * R.Level + 1500;

            Obj_AI_Hero vTarget;

            if (R.IsReady())
            {
                vTarget = SimpleTs.GetTarget(R.Range, SimpleTs.DamageType.Physical);
                if (vTarget.IsValidTarget(R.Range) && vTarget.Health <= R.GetDamage(vTarget))
                {
                    if (GetValue<KeyBind>("UltHelp").Active)
                        R.Cast(vTarget);

                    UltTarget = vTarget.ChampionName;
                    ShowUlt = true;
                }
                else
                {
                    ShowUlt = false;
                }
            }
            else
            {
                ShowUlt = false;
            }

            if (GetValue<KeyBind>("UseEQC").Active && E.IsReady() && Q.IsReady())
            {
                vTarget = SimpleTs.GetTarget(E.Range, SimpleTs.DamageType.Physical);
                if (vTarget.IsValidTarget(E.Range))
                {
                    E.Cast(vTarget);
                    Q.Cast(vTarget, false, true);
                }
            }
            // PQ you broke it D:
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
            else if (E.IsReady() && useE)
            {
                vTarget = SimpleTs.GetTarget(E.Range, SimpleTs.DamageType.Physical);
                if (vTarget != null && vTarget.Health <= E.GetDamage(vTarget))
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

        public override void ComboMenu(Menu config)
        {
            config.AddItem(new MenuItem("UseQC" + Id, "Use Q").SetValue(true));
            config.AddItem(new MenuItem("UseEC" + Id, "Use E").SetValue(true));
            config.AddItem(new MenuItem("UseRC" + Id, "Use R").SetValue(true));
        }

        public override void HarassMenu(Menu config)
        {
            config.AddItem(new MenuItem("UseQH" + Id, "Use Q").SetValue(true));
        }

        public override void DrawingMenu(Menu config)
        {
            config.AddItem(
                new MenuItem("DrawQ" + Id, "Q range").SetValue(new Circle(true, Color.FromArgb(100, 255, 0, 255))));
            config.AddItem(
                new MenuItem("DrawE" + Id, "E range").SetValue(new Circle(false, Color.FromArgb(100, 255, 255, 255))));
            config.AddItem(
                new MenuItem("DrawR" + Id, "R range").SetValue(new Circle(false, Color.FromArgb(100, 255, 255, 255))));
            config.AddItem(
                new MenuItem("DrawUlt" + Id, "Ult Text").SetValue(new Circle(true, Color.FromArgb(255, 255, 255, 255))));
        }

        public override void MiscMenu(Menu config)
        {
            config.AddItem(
                new MenuItem("UltHelp" + Id, "Ult Target on R").SetValue(new KeyBind("R".ToCharArray()[0],
                    KeyBindType.Press)));
            config.AddItem(
                new MenuItem("UseEQC" + Id, "Use E-Q Combo").SetValue(new KeyBind("T".ToCharArray()[0],
                    KeyBindType.Press)));
        }
    }
}