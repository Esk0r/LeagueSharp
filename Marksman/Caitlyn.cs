#region

using System;
using System.Drawing;
using System.Linq;
using LeagueSharp;
using LeagueSharp.Common;
#endregion

// Base done by Chulbul-Pandey, Drawings and improvements added by Dibes.
namespace Marksman {
    internal class Caitlyn : Champion {
        public Spell Q;
        public Spell E;
        public Spell R;
        // Vars for ulting
        public string ultTarget;
        public bool showUlt;

        public Caitlyn() {

            Utils.PrintMessage("Caitlyn loaded.");

            Q = new Spell(SpellSlot.Q, 1240);
            Q.SetSkillshot(0.25f, 60f, 2000f, false, Prediction.SkillshotType.SkillshotLine);

            E = new Spell(SpellSlot.E, 800);
            E.SetSkillshot(0.25f, 80f, 1600f, true, Prediction.SkillshotType.SkillshotLine);

            R = new Spell(SpellSlot.R, 2000);


            AntiGapcloser.OnEnemyGapcloser += AntiGapcloser_OnEnemyGapcloser;
            Interrupter.OnPosibleToInterrupt += Interrupter_OnPosibleToInterrupt;
        }

        public void AntiGapcloser_OnEnemyGapcloser(ActiveGapcloser gapcloser) {
            if (E.IsReady() && gapcloser.Sender.IsValidTarget(E.Range))
                E.CastOnUnit(gapcloser.Sender);
        }

        public void Interrupter_OnPosibleToInterrupt(Obj_AI_Base unit, InterruptableSpell spell) {
            if (GetValue<bool>("UseEInterrupt") && unit.IsValidTarget(800f))
                E.Cast(unit);
        }

        public override void Orbwalking_AfterAttack(Obj_AI_Base unit, Obj_AI_Base target) {
            if ((ComboActive || HarassActive) && !unit.IsMe && (target is Obj_AI_Hero)) {
                var useQ = GetValue<bool>("UseQ" + (ComboActive ? "C" : "H"));

                if (Q.IsReady() && useQ) {
                    Q.Cast(target);
                }
            }
        }

        public override void Drawing_OnDraw(EventArgs args) {
            Spell[] spellList = { Q, E, R };            
            foreach (var spell in spellList) {
                var menuItem = GetValue<Circle>("Draw" + spell.Slot);
                if (menuItem.Active)
                    Utility.DrawCircle(ObjectManager.Player.Position, spell.Range, menuItem.Color);
            }

            var drawUlt = GetValue<Circle>("DrawUlt");

            if (drawUlt.Active && showUlt) {
                float[] playerPos = Drawing.WorldToScreen(ObjectManager.Player.Position);
                Drawing.DrawText(playerPos[0] - 65, playerPos[1] + 20, drawUlt.Color, "Hit R To kill " + ultTarget + "!");
            }
        }

        public override void Game_OnGameUpdate(EventArgs args) {
            R.Range = 500 * R.Level + 1500;

            Obj_AI_Hero vTarget;

            if (ComboActive || HarassActive) {
                var useQ = GetValue<bool>("UseQ" + (ComboActive ? "C" : "H"));
                var useE = GetValue<bool>("UseEC");
                var useR = GetValue<bool>("UseRC");

                if (Orbwalking.CanMove(100)) {
                     
                    if (Q.IsReady() && useQ) {
                        vTarget = SimpleTs.GetTarget(Q.Range, SimpleTs.DamageType.Physical);
                        if (vTarget != null)
                            Q.Cast(vTarget);
                    } else if (E.IsReady() && useE) {
                        vTarget = SimpleTs.GetTarget(E.Range, SimpleTs.DamageType.Physical);
                        if (vTarget != null && vTarget.Health <= E.GetDamage(vTarget))
                            E.Cast(vTarget);
                    }

                }

                if (R.IsReady() && useR) {
                    vTarget = SimpleTs.GetTarget(R.Range, SimpleTs.DamageType.Physical);
                    if (vTarget != null && vTarget.Health <= R.GetDamage(vTarget) && !Orbwalking.InAutoAttackRange(vTarget)) {
                        R.CastOnUnit(vTarget);
                    }
                }
            }

            var useEQ = GetValue<KeyBind>("UseEQC").Active;

            if (R.IsReady()) {
                vTarget = SimpleTs.GetTarget(R.Range, SimpleTs.DamageType.Physical);
                if (vTarget != null && vTarget.Health <= R.GetDamage(vTarget)) {
                    if (GetValue<KeyBind>("UltHelp").Active) {
                        R.Cast(vTarget);
                    }
                    ultTarget = vTarget.ChampionName;
                    showUlt = true;
                } else {
                    showUlt = false;
                }
            } else {
                showUlt = false;
            }

            if (useEQ && E.IsReady() && Q.IsReady()) /* Use EQ Combo */ {
                vTarget = SimpleTs.GetTarget(E.Range, SimpleTs.DamageType.Physical);
                if (vTarget != null) {
                    E.Cast(vTarget);
                    Q.Cast(vTarget);
                }
            }
        }

        public override void ComboMenu(Menu config) {
            config.AddItem(new MenuItem("UseQC" + Id, "Use Q").SetValue(true));
            config.AddItem(new MenuItem("UseEC" + Id, "Use E").SetValue(true));
            config.AddItem(new MenuItem("UseRC" + Id, "Use R").SetValue(true));
        }

        public override void HarassMenu(Menu config) {
            config.AddItem(new MenuItem("UseQH" + Id, "Use Q").SetValue(true));
        }

        public override void MiscMenu(Menu config) {
            config.AddItem(new MenuItem("UseEQC" + Id, "Use E-Q Combo").SetValue(new KeyBind("T".ToCharArray()[0], KeyBindType.Press)));
            config.AddItem(new MenuItem("UltHelp" + Id, "Ult Helper").SetValue(new KeyBind("R".ToCharArray()[0], KeyBindType.Press)));
        }

        public override void DrawingMenu(Menu config) {
            config.AddItem(
                new MenuItem("DrawQ" + Id, "Q range").SetValue(new Circle(true, Color.FromArgb(100, 255, 0, 255))));
            config.AddItem(
                new MenuItem("DrawE" + Id, "E range").SetValue(new Circle(false, Color.FromArgb(100, 255, 255, 255))));
            config.AddItem(
                new MenuItem("DrawR" + Id, "R range").SetValue(new Circle(false, Color.FromArgb(100, 255, 255, 255))));
            config.AddItem(
                new MenuItem("DrawUlt" + Id, "Ult Text").SetValue(new Circle(true, Color.FromArgb(255, 255, 255, 255))));
        }
    }
}