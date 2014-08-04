#region

using System;
using System.Drawing;
using System.Linq;
using LeagueSharp;
using LeagueSharp.Common;
using SharpDX;

#endregion

namespace Marksman {
    class Caitlyn : Champion {
        public Spell Q;
        public Spell W;
        public Spell R;
        public int aaRange = 650;

        public Caitlyn() {
            Utils.PrintMessage("Caitlyn loaded.");

            Q = new Spell(SpellSlot.Q, 1300);
            Q.SetSkillshot(Q.Delay, Q.Width, Q.Speed, false, Prediction.SkillshotType.SkillshotLine);

            // This needs testing...
            W = new Spell(SpellSlot.W, 800);
            W.SetSkillshot(0f, 10f, 1400f, false, Prediction.SkillshotType.SkillshotCircle);

            R = new Spell(SpellSlot.R, 2500);
        }

        public override void Drawing_OnDraw(EventArgs args) {
            Spell[] spellList = { Q };
            foreach (var spell in spellList) {
                var menuItem = GetValue<Circle>("Draw" + spell.Slot);
                if (menuItem.Active)
                    Utility.DrawCircle(ObjectManager.Player.Position, spell.Range, menuItem.Color);
            }
        }

        public override void Game_OnGameUpdate(EventArgs args) {

            if (ComboActive || HarassActive) {

                var useQ = GetValue<bool>("UseQ" + (ComboActive ? "C" : "H"));
                var useQOR = GetValue<bool>("UseQOR" + (ComboActive ? "C" : "H"));

                if (Orbwalking.CanMove(100) && useQ && Q.IsReady()) {
                    var t = SimpleTs.GetTarget(Q.Range, SimpleTs.DamageType.Physical);

                    if (t != null) {
                        if (useQOR) {
                            if (Vector3.Distance(ObjectManager.Player.Position, t.Position) >= aaRange) {
                                Q.Cast(t);
                            }
                        } else {
                            Q.Cast(t); 
                        }
                    }
                }
            }


            var autoWi = GetValue<bool>("AutoWI");
            var autoWd = GetValue<bool>("AutoWD");
            if (autoWd || autoWi) {
                foreach (var enemy in ObjectManager.Get<Obj_AI_Hero>().Where(enemy => enemy.IsValidTarget(W.Range))) {
                    if (autoWi)
                        W.CastIfHitchanceEquals(enemy, Prediction.HitChance.Immobile);

                    if (autoWd)
                        W.CastIfHitchanceEquals(enemy, Prediction.HitChance.Immobile);
                }
            }

            if (R.IsReady()) {
                var castR = GetValue<KeyBind>("CastR").Active;
                var target = SimpleTs.GetTarget(R.Range, SimpleTs.DamageType.Physical);
                // Prioritizes the target, if the target is not killable with ult, check others.
                var ultTarget = GetUltTarget(target, castR);
                // If the target is not null ult on them.
                if (ultTarget != null) {
                    R.Cast(ultTarget);
                }
            }
        }

        /*
         * Gets the target to ult. Prioritizes using SimpleTS, if that target can't die, check the rest of the team.
         * If no one can die by ult it returns null.
         */
        public Obj_AI_Hero GetUltTarget(Obj_AI_Hero target, bool castR) {
            if (DamageLib.getDmg(target, DamageLib.SpellType.R, DamageLib.StageType.FirstDamage) > target.Health) {
                if (castR || GetValue<bool>("UseRC")) {
                    if (GetValue<bool>("UseROR") && GetValue<bool>("UseRC")) {
                        if (Vector3.Distance(ObjectManager.Player.Position, target.Position) >= aaRange) {
                            return target;
                        }
                    } else {
                        return target;
                    }
                } else {
                    return null;
                }
                if (DamageLib.getDmg(target, DamageLib.SpellType.R, DamageLib.StageType.FirstDamage) > target.Health) {
                    float[] playerPos = Drawing.WorldToScreen(ObjectManager.Player.Position);
                    Drawing.DrawText(playerPos[0] - 50, playerPos[1] + 35, System.Drawing.Color.White, "Hit R To kill " + target.Name + "!");
                }
            } else {
                foreach (var enemy in ObjectManager.Get<Obj_AI_Hero>().Where(enemy => enemy.IsValidTarget(R.Range))) {
                    if (castR || GetValue<bool>("UseRC") && (DamageLib.getDmg(enemy, DamageLib.SpellType.R, DamageLib.StageType.FirstDamage) > enemy.Health)) {
                        if (GetValue<bool>("UseROR") && GetValue<bool>("UseRC")) {
                            if (Vector3.Distance(ObjectManager.Player.Position, enemy.Position) >= aaRange) {
                                return enemy;
                            }
                        } else {
                            return enemy;
                        }
                    } else {
                        return null;
                    }
                    if (DamageLib.getDmg(enemy, DamageLib.SpellType.R, DamageLib.StageType.FirstDamage) > enemy.Health) {
                        float[] playerPos = Drawing.WorldToScreen(ObjectManager.Player.Position);
                        Drawing.DrawText(playerPos[0] - 50, playerPos[1] + 35, System.Drawing.Color.White, "Hit R To kill " + enemy.Name + "!");
                    }
                }
            }
            // No one selected.
            return null;
        }

        public override void ComboMenu(Menu config) {
            config.AddItem(new MenuItem("UseQC" + Id, "Use Q").SetValue(true));
            config.AddItem(new MenuItem("UseRC" + Id, "Use R").SetValue(true));
            config.AddItem(new MenuItem("space", "----- Options -----").SetValue(true));
            config.AddItem(new MenuItem("UseQORC" + Id, "Use Q out of AA Range").SetValue(true));
            config.AddItem(new MenuItem("UseROR" + Id, "Use R out of AA Range").SetValue(true));
        }

        public override void HarassMenu(Menu config) {
            config.AddItem(new MenuItem("UseQH" + Id, "Use Q").SetValue(false));
            config.AddItem(new MenuItem("space", "----- Options -----").SetValue(true));
            config.AddItem(new MenuItem("UseQORH" + Id, "Use Q out of AA Range").SetValue(true));
        }

        public override void MiscMenu(Menu config) {
            config.AddItem(
                new MenuItem("CastR" + Id, "Cast R (2500 Range)").SetValue(new KeyBind("R".ToCharArray()[0],
                    KeyBindType.Press)));
            // Use Q out of AA range.
            config.AddItem(new MenuItem("AutoWI" + Id, "Auto-W on immobile").SetValue(true));
            config.AddItem(new MenuItem("AutoWD" + Id, "Auto-W on dashing").SetValue(true));
        }

        public override void DrawingMenu(Menu config) {
            config.AddItem(
                new MenuItem("DrawQ" + Id, "Q range").SetValue(new Circle(false, System.Drawing.Color.FromArgb(100, 255, 255, 255))));
        }
    }
}
