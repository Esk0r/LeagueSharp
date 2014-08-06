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
        public int ultStack = 0;
        public int aaRange = 500;
        public int wAddRange = 125;
        public int rRange = 1300;

        public Kogmaw() {
            Utils.PrintMessage("Kog'Maw loaded.");

            Q = new Spell(SpellSlot.Q, 1000);
            Q.SetSkillshot(Q.Delay, Q.Width, Q.Speed, true, Prediction.SkillshotType.SkillshotLine);

            W = new Spell(SpellSlot.W, 703);

            E = new Spell(SpellSlot.E, 1280);
            E.SetSkillshot(E.Delay, E.Width, E.Speed, false, Prediction.SkillshotType.SkillshotLine);

            R = new Spell(SpellSlot.R, 1200);
            R.SetSkillshot(R.Delay, R.Width, R.Speed, false, Prediction.SkillshotType.SkillshotCircle);

            CustomEvents.Unit.OnLevelUpSpell += Unit_OnLevelUpSpell;
        }

        public override void Orbwalking_AfterAttack(Obj_AI_Base unit, Obj_AI_Base target) {
            if ((ComboActive || HarassActive) && unit.IsMe && (target is Obj_AI_Hero)) {
                var useQ = GetValue<bool>("UseQ" + (ComboActive ? "C" : "H"));
                var useR = GetValue<bool>("UseR" + (ComboActive ? "C" : "H"));
                var rLim = GetValue<Slider>("Rlim" + (ComboActive ? "C" : "H"));

                if (useQ && Q.IsReady()) {
                    Q.Cast(target);
                }

                if (useR && R.IsReady()) {
                    UpdateUltStacks();
                    // Cast R if rLim is not met :D
                    if (ultStack < rLim.Value) {
                        Console.WriteLine("Casting Harrass R");
                        R.Cast(target);
                    }
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

        public override void Game_OnGameUpdate(EventArgs args) {
           
            if ((!ComboActive && !HarassActive) || !Orbwalking.CanMove(100)) return;

            var useQ = GetValue<bool>("UseQ" + (ComboActive ? "C" : "H"));
            var useR = GetValue<bool>("UseR" + (ComboActive ? "C" : "H"));
            var rLim = GetValue<Slider>("Rlim" + (ComboActive ? "C" : "H"));
            var useE = GetValue<bool>("UseEC") && ComboActive;
            var useW = GetValue<bool>("UseWC") && ComboActive;

            if (Orbwalking.CanMove(50)) {

                var target = SimpleTs.GetTarget(R.Range, SimpleTs.DamageType.Physical);

                if (useE && E.IsReady() && target.IsValidTarget()) {
                    E.Cast(target);
                }

                if (useQ && Q.IsReady() && target.IsValidTarget()) {
                    Q.Cast(target);
                }

                if (useR && R.IsReady() && target.IsVisible && target.IsEnemy) {
                    UpdateUltStacks();

                    if (ultStack < rLim.Value) {
                        Console.WriteLine("Casting Combo R");
                        R.Cast(target);
                    }
                }

                if (useW && (W.IsReady() && Vector3.Distance(ObjectManager.Player.Position, target.Position) < (aaRange + wAddRange) && target.IsValidTarget())) {
                    ObjectManager.Player.Spellbook.CastSpell(SpellSlot.W);
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
                                hero.Health - R.GetDamage(hero, DamageLib.SpellType.R) + 20 > 0))
                R.Cast(hero);
        }

        void UpdateUltStacks() {
            foreach (var buff in ObjectManager.Player.Buffs) {

                if (buff.Name == "kogmawlivingartillerycost") {
                    ultStack = buff.Count;
                } else {
                    ultStack = 0;
                }

            }
        }

        void Unit_OnLevelUpSpell(Obj_AI_Base sender, CustomEvents.Unit.OnLevelUpSpellEventArgs args) {
            switch (R.Level) {
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

            switch (W.Level) {
                case 1:
                    wAddRange = 130;
                    break;
                case 2:
                    wAddRange = 150;
                    break;
                case 3:
                    wAddRange = 170;
                    break;
                case 4:
                    wAddRange = 190;
                    break;
                case 5:
                    wAddRange = 210;
                    break;
            }
        }

        public override void ComboMenu(Menu config) {
            config.AddItem(new MenuItem("UseQC" + Id, "Use Q").SetValue(true));
            config.AddItem(new MenuItem("UseWC" + Id, "Use W").SetValue(true));
            config.AddItem(new MenuItem("UseEC" + Id, "Use E").SetValue(true));
            config.AddItem(new MenuItem("UseRC" + Id, "Use R").SetValue(true));
            config.AddItem(new MenuItem("spacer", "------- Options -------"));
            config.AddItem(new MenuItem("RlimC" + Id, "R Limiter").SetValue(new Slider(1, 5, 1)));
        }

        public override void HarassMenu(Menu config) {
            config.AddItem(new MenuItem("UseQH" + Id, "Use Q").SetValue(false));
            config.AddItem(new MenuItem("UseRH" + Id, "Use R").SetValue(true));
            config.AddItem(new MenuItem("spacer", "------- Options -------"));
            config.AddItem(new MenuItem("RlimH" + Id, "R Limiter").SetValue(new Slider(1, 5, 1)));
        }

        public override void DrawingMenu(Menu config) {
            config.AddItem(
                new MenuItem("DrawQ" + Id, "Q range").SetValue(new Circle(true, System.Drawing.Color.FromArgb(100, 255, 0, 255))));

        }

        public override void MiscMenu(Menu config) {
            config.AddItem(new MenuItem("UseRM" + Id, "Use R to Killsteal").SetValue(true));
        }
    }
}
