#region

using System;
using System.Drawing;
using System.Linq;
using LeagueSharp;
using LeagueSharp.Common;

#endregion

namespace Marksman
{
    internal class Jinx : Champion
    {
        public Spell E;
        public Spell Q;
        private float _qAddRange;
        public Spell R;
        public Spell W;

        public Jinx()
        {
            Utils.PrintMessage("Jinx loaded.");

            Q = new Spell(SpellSlot.Q, float.MaxValue);
            W = new Spell(SpellSlot.W, 1500f);
            E = new Spell(SpellSlot.E, 900f);
            R = new Spell(SpellSlot.R, 25000f);

            W.SetSkillshot(0.6f, 60f, 2000f, true, Prediction.SkillshotType.SkillshotLine);
            E.SetSkillshot(0.7f, 120f, 1750f, false, Prediction.SkillshotType.SkillshotCircle);
            R.SetSkillshot(0.6f, 140f, 1700f, false, Prediction.SkillshotType.SkillshotLine);
        }

        public override void Drawing_OnDraw(EventArgs args)
        {
            Spell[] spellList = { W };
            foreach (var spell in spellList)
            {
                var menuItem = GetValue<Circle>("Draw" + spell.Slot);
                if (menuItem.Active)
                    Utility.DrawCircle(ObjectManager.Player.Position, spell.Range, menuItem.Color);
            }
        }

        public override void Game_OnGameUpdate(EventArgs args)
        {
            _qAddRange = 50 + 25 * ObjectManager.Player.Spellbook.GetSpell(SpellSlot.Q).Level;

            var autoEi = GetValue<bool>("AutoEI");
            var autoEs = GetValue<bool>("AutoES");
            var autoEd = GetValue<bool>("AutoED");

            if (autoEd || autoEi || autoEs)
            {
                foreach (var enemy in ObjectManager.Get<Obj_AI_Hero>().Where(enemy => enemy.IsValidTarget(E.Range)))
                {
                    if (autoEs && E.IsReady() && enemy.HasBuffOfType(BuffType.Slow))
                        E.CastIfHitchanceEquals(enemy, Prediction.HitChance.HighHitchance);
                    if (autoEi && E.IsReady() && (enemy.IsStunned || enemy.HasBuffOfType(BuffType.Snare)))
                        E.CastIfHitchanceEquals(enemy, Prediction.HitChance.HighHitchance);
                    if (autoEd && E.IsReady() && enemy.IsDashing())
                        E.CastIfHitchanceEquals(enemy, Prediction.HitChance.Dashing);
                }
            }

            if (GetValue<KeyBind>("CastR").Active && R.IsReady())
            {
                var target = SimpleTs.GetTarget(1500, SimpleTs.DamageType.Physical);

                if (target.IsValidTarget())
                    if (DamageLib.getDmg(target, DamageLib.SpellType.R, DamageLib.StageType.FirstDamage) > target.Health)
                        R.Cast(target, false, true);
            }

            if (LaneClearActive && HasFishBones())
                Q.Cast();

            if ((!ComboActive && !HarassActive) || !Orbwalking.CanMove(100))
                return;

            var useQ = GetValue<bool>("UseQ" + (ComboActive ? "C" : "H"));
            var swapDistance = GetValue<bool>("SwapDistance");
            var swapAoe = GetValue<bool>("SwapAOE");
            var useW = GetValue<bool>("UseW" + (ComboActive ? "C" : "H"));
            var useR = GetValue<bool>("UseRC");
            var checkRok = GetValue<bool>("ROverKill");

            if (useW && W.IsReady())
            {
                var t = SimpleTs.GetTarget(W.Range, SimpleTs.DamageType.Physical);

                if (t.IsValidTarget())
                    W.Cast(t);
            }

            if (useQ)
            {
                var t = SimpleTs.GetTarget(Q.Range, SimpleTs.DamageType.Physical);

                if (t.IsValidTarget())
                {
                    if (swapDistance && Q.IsReady())
                    {
                        var distance = t.ServerPosition.Distance(ObjectManager.Player.ServerPosition);
                        var powPowRange = GetRealPowPowRange(t);

                        if (distance > powPowRange && distance < (powPowRange + _qAddRange) && !HasFishBones())
                            Q.Cast();
                        else if (distance < powPowRange && HasFishBones())
                            Q.Cast();
                    }

                    if (swapAoe && Q.IsReady())
                        if (GetPowPowStacks() > 2 && !HasFishBones() && CountEnemies(t, 150) > 1)
                            Q.Cast();
                }
            }

            if (useR && R.IsReady())
            {
                var minR = GetValue<Slider>("MinRRange").Value;
                var maxR = GetValue<Slider>("MaxRRange").Value;
                var t = SimpleTs.GetTarget(maxR, SimpleTs.DamageType.Physical);

                if (t.IsValidTarget())
                {
                    var distance = t.ServerPosition.Distance(ObjectManager.Player.ServerPosition);

                    if (!checkRok)
                    {
                        if (DamageLib.getDmg(t, DamageLib.SpellType.R, DamageLib.StageType.FirstDamage) > t.Health)
                            R.Cast(t, false, true);
                    }
                    else if (checkRok && distance > minR)
                    {
                        var aDamage = DamageLib.getDmg(t, DamageLib.SpellType.AD);
                        var wDamage = DamageLib.getDmg(t, DamageLib.SpellType.W, DamageLib.StageType.FirstDamage);
                        var rDamage = DamageLib.getDmg(t, DamageLib.SpellType.R, DamageLib.StageType.FirstDamage);
                        var powPowRange = GetRealPowPowRange(t);

                        if (distance < (powPowRange + _qAddRange) && !(aDamage * 3.5 > t.Health))
                        {
                            if (!W.IsReady() || !(wDamage > t.Health) || distance > W.Range ||
                                W.GetPrediction(t).CollisionUnitsList.Count > 0)
                                if (CountAlliesNearTarget(t, 700) <= 2)
                                    if (rDamage > t.Health)
                                        R.Cast(t, false, true);
                        }
                        else if (distance > (powPowRange + _qAddRange))
                        {
                            if (!W.IsReady() || !(wDamage > t.Health) || distance > W.Range ||
                                W.GetPrediction(t).CollisionUnitsList.Count > 0)
                                if (CountAlliesNearTarget(t, 700) <= 2)
                                    if (rDamage > t.Health)
                                        R.Cast(t, false, true);
                        }
                    }
                }
            }
        }

        public override void Orbwalking_AfterAttack(Obj_AI_Base unit, Obj_AI_Base target)
        {
            if ((ComboActive || HarassActive) && unit.IsMe && (target is Obj_AI_Hero))
            {
                var useW = GetValue<bool>("UseW" + (ComboActive ? "C" : "H"));

                if (useW && W.IsReady())
                    W.Cast(target);
            }
        }

        public bool HasFishBones()
        {
            return Math.Abs(ObjectManager.Player.AttackRange - 525) > float.Epsilon;
        }

        public int CountEnemies(Obj_AI_Hero target, float range)
        {
            return
                ObjectManager.Get<Obj_AI_Hero>()
                    .Count(
                        hero =>
                            hero.IsValidTarget() && hero.Team != ObjectManager.Player.Team &&
                            hero.ServerPosition.Distance(target.ServerPosition) <= range);
        }

        public int CountAlliesNearTarget(Obj_AI_Hero target, float range)
        {
            return
                ObjectManager.Get<Obj_AI_Hero>()
                    .Count(
                        hero =>
                            hero.Team == ObjectManager.Player.Team &&
                            hero.ServerPosition.Distance(target.ServerPosition) <= range);
        }

        private static int GetPowPowStacks()
        {
            return (from buff in ObjectManager.Player.Buffs
                where buff.DisplayName.ToLower() == "jinxqramp"
                select buff.Count).FirstOrDefault();
        }

        public float GetRealPowPowRange(Obj_AI_Hero target)
        {
            return 525 + ObjectManager.Player.BoundingRadius + target.BoundingRadius -
                   ((target.Path.Length > 0) ? 20 : 10);
        }

        public override void ComboMenu(Menu config)
        {
            config.AddItem(new MenuItem("UseQC" + Id, "Use Q").SetValue(true));
            config.AddItem(new MenuItem("UseWC" + Id, "Use W").SetValue(true));
            config.AddItem(new MenuItem("UseRC" + Id, "Use R").SetValue(true));
        }

        public override void HarassMenu(Menu config)
        {
            config.AddItem(new MenuItem("UseQH" + Id, "Use Q").SetValue(true));
            config.AddItem(new MenuItem("UseWH" + Id, "Use W").SetValue(false));
        }

        public override void DrawingMenu(Menu config)
        {
            config.AddItem(
                new MenuItem("DrawW" + Id, "W range").SetValue(new Circle(false, Color.FromArgb(100, 255, 255, 255))));
        }

        public override void MiscMenu(Menu config)
        {
            config.AddItem(
                new MenuItem("CastR" + Id, "Cast R (2000 Range)").SetValue(new KeyBind("T".ToCharArray()[0],
                    KeyBindType.Press)));
            config.AddItem(new MenuItem("ROverKill" + Id, "Check R Overkill").SetValue(true));
            config.AddItem(new MenuItem("MinRRange" + Id, "Min R range").SetValue(new Slider(300, 0, 1500)));
            config.AddItem(new MenuItem("MaxRRange" + Id, "Max R range").SetValue(new Slider(1700, 0, 4000)));
            config.AddItem(new MenuItem("SwapDistance" + Id, "Swap Q for distance").SetValue(true));
            config.AddItem(new MenuItem("SwapAOE" + Id, "Swap Q for AOE").SetValue(false));
            config.AddItem(new MenuItem("AutoEI" + Id, "Auto-E on immobile").SetValue(true));
            config.AddItem(new MenuItem("AutoES" + Id, "Auto-E on slowed").SetValue(true));
            config.AddItem(new MenuItem("AutoED" + Id, "Auto-E on dashing").SetValue(true));
        }
    }
}