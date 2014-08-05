using LeagueSharp;
using LeagueSharp.Common;
using SharpDX;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;


namespace Marksman {
    class Kogmaw : Champion {
        public Spell E;
        public Spell W;
        public Spell Q;
        public Spell R;
        public int aaRange = 500;
        public int wAddRange = 125;
        public int rRange = 1300;

        public Kogmaw() {
            Utils.PrintMessage("Kog'Maw loaded.");

            Q = new Spell(SpellSlot.Q, 1000);
            Q.SetSkillshot(Q.Delay, Q.Width, Q.Speed, Q.Collision, Prediction.SkillshotType.SkillshotLine);

            W = new Spell(SpellSlot.W, 703);

            E = new Spell(SpellSlot.E, 1280);
            E.SetSkillshot(E.Delay, E.Width, E.Speed, E.Collision, Prediction.SkillshotType.SkillshotLine);

            R = new Spell(SpellSlot.R, 1200);
            R.SetSkillshot(R.Delay, R.Width, R.Speed, R.Collision, Prediction.SkillshotType.SkillshotCircle);
        }

        public override void Orbwalking_AfterAttack(Obj_AI_Base unit, Obj_AI_Base target) {
            if ((ComboActive || HarassActive) && unit.IsMe && (target is Obj_AI_Hero)) {
                var useQ = GetValue<bool>("UseQ" + (ComboActive ? "C" : "H"));
                var useE = GetValue<bool>("UseE" + (ComboActive ? "C" : "H"));

                if (useQ && Q.IsReady())
                    Q.Cast(target);

                if (useE && E.IsReady())
                    E.Cast(target);
            }
        }

        public override void Drawing_OnDraw(EventArgs args) {
            Spell[] spellList = { E };
            foreach (var spell in spellList) {
                var menuItem = GetValue<Circle>("Draw" + spell.Slot);
                if (menuItem.Active)
                    Utility.DrawCircle(ObjectManager.Player.Position, spell.Range, menuItem.Color);
            }
        }

        public override void Game_OnGameUpdate(EventArgs args) {
            // Update R range on kogmaw as the skill is leveled.
            switch(R.Level) {
                case 1:
                    R.Range = 1200;
                    break;
                case 2:
                    R.Range = 1500;
                    break;
                case 3:
                    R.Range = 1800;
                    break;
                default:
                    R.Range = 1200;
                    break;
            }

            if (ComboActive || HarassActive) {
                var useQ = GetValue<bool>("UseQC");
                var useE = GetValue<bool>("UseEC");
                var useR = GetValue<bool>("UseRC");
                var useW = GetValue<bool>("UseWC");

                if (Orbwalking.CanMove(100)) {
                    var target = SimpleTs.GetTarget(Q.Range, SimpleTs.DamageType.Physical);
                    
                    if (useQ) {
                        if (Q.IsReady() && target.IsValidTarget()) {
                            Q.Cast(target);
                        }
                    }

                    if (useE) {
                        if (E.IsReady() && target.IsValidTarget()) {
                            E.Cast(target);
                        }
                    }

                    if (useR) {
                        if (R.IsReady() && target.IsValidTarget()) {
                            R.Cast(target);
                        }
                    }

                    if (useW) {
                        if (W.IsReady() && Vector3.Distance(ObjectManager.Player.Position, target.Position) < (aaRange + wAddRange) && target.IsValidTarget()) {
                            W.Cast(target);
                        }
                    }

                }
            }

            //Killsteal
            if (!ComboActive || !GetValue<bool>("UseRM") || !R.IsReady()) return;
            foreach (
                var hero in
                    ObjectManager.Get<Obj_AI_Hero>()
                        .Where(
                            hero =>
                                hero.IsValidTarget(R.Range) &&
                                DamageLib.getDmg(hero, DamageLib.SpellType.R) - 20 > hero.Health))
                R.Cast(hero);
        }

        public override void ComboMenu(Menu config) {
            config.AddItem(new MenuItem("UseQC" + Id, "Use Q").SetValue(true));
            config.AddItem(new MenuItem("UseWC" + Id, "Use W").SetValue(true));
            config.AddItem(new MenuItem("UseEC" + Id, "Use E").SetValue(true));
            config.AddItem(new MenuItem("UseRC" + Id, "Use R").SetValue(true));
        }

        public override void HarassMenu(Menu config) {
            config.AddItem(new MenuItem("UseQH" + Id, "Use Q").SetValue(false));
            config.AddItem(new MenuItem("UseRH" + Id, "Use R").SetValue(true));
        }

        public override void DrawingMenu(Menu config) {
            config.AddItem(
                new MenuItem("DrawE" + Id, "E range").SetValue(new Circle(true, System.Drawing.Color.FromArgb(100, 255, 0, 255))));

        }

        public override void MiscMenu(Menu config) {
            config.AddItem(new MenuItem("UseRM" + Id, "Use R").SetValue(true));
        }
    }
}
