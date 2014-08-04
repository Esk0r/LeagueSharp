#region

using System;
using System.Drawing;
using LeagueSharp;
using LeagueSharp.Common;
using SharpDX;

#endregion

namespace Marksman {

    class Graves : Champion {

        public Spell Q;
        public Spell W;
        public Spell R;

        public Graves() {
            Utils.PrintMessage("Graves loaded.");
            // Q likes to shoot a bit too far away, so moving the range inward.
            Q = new Spell(SpellSlot.Q, 1000);
            Q.SetSkillshot(0.3f, 10f, 1300f, false, Prediction.SkillshotType.SkillshotCone);

            W = new Spell(SpellSlot.W, 1100);
            W.SetSkillshot(0.3f, 250f, 1650f, false, Prediction.SkillshotType.SkillshotCircle);

            R = new Spell(SpellSlot.R, 1000);
            R.SetSkillshot(0.5f, 100f, 1200f, true, Prediction.SkillshotType.SkillshotLine);

        }

        public override void Game_OnGameUpdate(EventArgs args) {
            if (ComboActive || HarassActive) {
                var useQ = GetValue<bool>("UseQ" + (ComboActive ? "C" : "H"));
                var useW = GetValue<bool>("UseW" + (ComboActive ? "C" : "H"));
                var useR = GetValue<bool>("UseR" + (ComboActive ? "C" : "H"));

                if (Orbwalking.CanMove(100)) {
                    // Start With Q
                    if (Q.IsReady() && useQ) {
                        var t = SimpleTs.GetTarget(Q.Range, SimpleTs.DamageType.Physical);
                        if (t != null) {
                            Q.Cast(t, false, true);
                        }
                    }
                }

                if (Orbwalking.CanMove(100)) {
                    // W
                    if (W.IsReady() && useW) {
                        var t = SimpleTs.GetTarget(Q.Range, SimpleTs.DamageType.Physical);
                        if (t != null) {
                            W.Cast(t, false, true);
                        }
                    }
                }

                if (Orbwalking.CanMove(100)) {
                    // Only on the combo menu here.
                    if (R.IsReady() && useR) {
                        var t = SimpleTs.GetTarget(R.Range, SimpleTs.DamageType.Physical);
                        if (t != null) {
                            if ((DamageLib.getDmg(t, DamageLib.SpellType.AD, DamageLib.StageType.FirstDamage) < t.Health && Vector3.Distance(ObjectManager.Player.Position, t.Position) < 510) && DamageLib.getDmg(t, DamageLib.SpellType.R, DamageLib.StageType.FirstDamage) > t.Health) {
                                R.Cast(t, false, true);
                            }
                        }
                    }
                }

            }
        }

        public override void Orbwalking_AfterAttack(Obj_AI_Base unit, Obj_AI_Base target) {
            if ((ComboActive || HarassActive) && unit.IsMe && (target is Obj_AI_Hero)) {
                var useQ = GetValue<bool>("UseQ" + (ComboActive ? "C" : "H"));
                var useW = GetValue<bool>("UseW" + (ComboActive ? "C" : "H"));

                if (W.IsReady() && useW) {
                    W.Cast(target);
                }

                if (Q.IsReady() && useQ) {
                    Q.Cast(target);
                }
            }
        }

        public override void Drawing_OnDraw(EventArgs args) {
            Spell[] spellList = { Q };
            foreach (var spell in spellList) {
                var menuItem = GetValue<Circle>("Draw" + spell.Slot);
                if (menuItem.Active)
                    Utility.DrawCircle(ObjectManager.Player.Position, spell.Range, menuItem.Color);
            }
        }

        public override void ComboMenu(Menu config) {
            config.AddItem(new MenuItem("UseQC" + Id, "Use Q").SetValue(true));
            config.AddItem(new MenuItem("UseRC" + Id, "Use R").SetValue(true));
            config.AddItem(new MenuItem("UseWC" + Id, "Use W").SetValue(true));
        }

        public override void HarassMenu(Menu config) {
            config.AddItem(new MenuItem("UseQH" + Id, "Use Q").SetValue(true));
            config.AddItem(new MenuItem("UseWH" + Id, "Use W").SetValue(false));
        }

        public override void DrawingMenu(Menu config) {
            config.AddItem(
                new MenuItem("DrawQ" + Id, "Q range").SetValue(new Circle(true, System.Drawing.Color.FromArgb(100, 255, 0, 255))));
        }

    }

}
