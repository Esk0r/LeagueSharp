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
        public Spell R;
        public Spell W;

        public Jinx()
        {
            Utils.PrintMessage("Jinx by [Credits in Github] loaded.");

            Q = new Spell(SpellSlot.Q, float.MaxValue);
            W = new Spell(SpellSlot.W, 1500f);
            E = new Spell(SpellSlot.E, 900f);
            R = new Spell(SpellSlot.R, 25000f);

            W.SetSkillshot(0.6f, 60f, 3300f, true, SkillshotType.SkillshotLine);
            E.SetSkillshot(0.7f, 120f, 1750f, false, SkillshotType.SkillshotCircle);
            R.SetSkillshot(0.6f, 140f, 1700f, false, SkillshotType.SkillshotLine);
        }

        public float QAddRange
        {
            get { return 50 + 25 * ObjectManager.Player.Spellbook.GetSpell(SpellSlot.Q).Level; }
        }

        private static bool FishBoneActive
        {
            get { return Math.Abs(ObjectManager.Player.AttackRange - 525f) > float.Epsilon; }
        }

        private static int PowPowStacks
        {
            get
            {
                return
                    ObjectManager.Player.Buffs.Where(buff => buff.DisplayName.ToLower() == "jinxqramp")
                        .Select(buff => buff.Count)
                        .FirstOrDefault();
            }
        }

        public override void Drawing_OnDraw(EventArgs args)
        {
            Spell[] spellList = { W };
            var drawQbound = GetValue<Circle>("DrawQBound");

            foreach (var spell in spellList)
            {
                var menuItem = GetValue<Circle>("Draw" + spell.Slot);
                if (menuItem.Active)
                {
                    Utility.DrawCircle(ObjectManager.Player.Position, spell.Range, menuItem.Color);
                }
            }

            if (drawQbound.Active)
            {
                if (FishBoneActive)
                {
                    Utility.DrawCircle(
                        ObjectManager.Player.Position, 525f + ObjectManager.Player.BoundingRadius + 65f,
                        drawQbound.Color);
                }
                else
                {
                    Utility.DrawCircle(
                        ObjectManager.Player.Position,
                        525f + ObjectManager.Player.BoundingRadius + 65f + QAddRange + 20f, drawQbound.Color);
                }
            }
        }

        public override void Game_OnGameUpdate(EventArgs args)
        {
            var autoEi = GetValue<bool>("AutoEI");
            var autoEs = GetValue<bool>("AutoES");
            var autoEd = GetValue<bool>("AutoED");

            if (autoEs || autoEi || autoEd)
            {
                foreach (
                    var enemy in ObjectManager.Get<Obj_AI_Hero>().Where(enemy => enemy.IsValidTarget(E.Range - 150)))
                {
                    if (autoEs && E.IsReady() && enemy.HasBuffOfType(BuffType.Slow))
                    {
                        var castPosition =
                            Prediction.GetPrediction(
                                new PredictionInput
                                {
                                    Unit = enemy,
                                    Delay = 0.7f,
                                    Radius = 120f,
                                    Speed = 1750f,
                                    Range = 900f,
                                    Type = SkillshotType.SkillshotCircle,
                                }).CastPosition;


                        if (GetSlowEndTime(enemy) >= (Game.Time + E.Delay + 0.5f))
                        {
                            E.Cast(castPosition);
                        }
                    }

                    if (autoEi && E.IsReady() &&
                        (enemy.HasBuffOfType(BuffType.Stun) || enemy.HasBuffOfType(BuffType.Snare) ||
                         enemy.HasBuffOfType(BuffType.Charm) || enemy.HasBuffOfType(BuffType.Fear) ||
                         enemy.HasBuffOfType(BuffType.Taunt)))
                    {
                        E.CastIfHitchanceEquals(enemy, HitChance.High);
                    }

                    if (autoEd && E.IsReady() && enemy.IsDashing())
                    {
                        E.CastIfHitchanceEquals(enemy, HitChance.Dashing);
                    }
                }
            }

            if (GetValue<KeyBind>("CastR").Active && R.IsReady())
            {
                var target = SimpleTs.GetTarget(1500, SimpleTs.DamageType.Physical);

                if (target.IsValidTarget())
                {
                    if (DamageLib.getDmg(target, DamageLib.SpellType.R, DamageLib.StageType.FirstDamage) > target.Health)
                    {
                        R.Cast(target, false, true);
                    }
                }
            }

            if (GetValue<bool>("SwapQ") && FishBoneActive &&
                (LaneClearActive ||
                 (HarassActive && SimpleTs.GetTarget(675f + QAddRange, SimpleTs.DamageType.Physical) == null)))
            {
                Q.Cast();
            }

            if ((!ComboActive && !HarassActive) || !Orbwalking.CanMove(100))
            {
                return;
            }

            var useQ = GetValue<bool>("UseQ" + (ComboActive ? "C" : "H"));
            var useW = GetValue<bool>("UseW" + (ComboActive ? "C" : "H"));
            var useR = GetValue<bool>("UseRC");

            if (useW && W.IsReady())
            {
                var t = SimpleTs.GetTarget(W.Range, SimpleTs.DamageType.Physical);
                var minW = GetValue<Slider>("MinWRange").Value;

                if (t.IsValidTarget() && GetRealDistance(t) >= minW)
                {
                    if (W.Cast(t) == Spell.CastStates.SuccessfullyCasted)
                    {
                        return;
                    }
                }
            }

            if (useQ)
            {
                foreach (var t in
                    ObjectManager.Get<Obj_AI_Hero>()
                        .Where(t => t.IsValidTarget(GetRealPowPowRange(t) + QAddRange + 20f)))
                {
                    var swapDistance = GetValue<bool>("SwapDistance");
                    var swapAoe = GetValue<bool>("SwapAOE");
                    var distance = GetRealDistance(t);
                    var powPowRange = GetRealPowPowRange(t);

                    if (swapDistance && Q.IsReady())
                    {
                        if (distance > powPowRange && !FishBoneActive)
                        {
                            if (Q.Cast())
                            {
                                return;
                            }
                        }
                        else if (distance < powPowRange && FishBoneActive)
                        {
                            if (Q.Cast())
                            {
                                return;
                            }
                        }
                    }

                    if (swapAoe && Q.IsReady())
                    {
                        if (distance > powPowRange && PowPowStacks > 2 && !FishBoneActive && CountEnemies(t, 150) > 1)
                        {
                            if (Q.Cast())
                            {
                                return;
                            }
                        }
                    }
                }
            }

            if (useR && R.IsReady())
            {
                var checkRok = GetValue<bool>("ROverKill");
                var minR = GetValue<Slider>("MinRRange").Value;
                var maxR = GetValue<Slider>("MaxRRange").Value;
                var t = SimpleTs.GetTarget(maxR, SimpleTs.DamageType.Physical);

                if (t.IsValidTarget())
                {
                    var distance = GetRealDistance(t);

                    if (!checkRok)
                    {
                        if (DamageLib.getDmg(t, DamageLib.SpellType.R, DamageLib.StageType.FirstDamage) > t.Health)
                        {
                            if (R.Cast(t, false, true) == Spell.CastStates.SuccessfullyCasted) { }
                        }
                    }
                    else if (checkRok && distance > minR)
                    {
                        var aDamage = DamageLib.getDmg(t, DamageLib.SpellType.AD);
                        var wDamage = DamageLib.getDmg(t, DamageLib.SpellType.W, DamageLib.StageType.FirstDamage);
                        var rDamage = DamageLib.getDmg(t, DamageLib.SpellType.R, DamageLib.StageType.FirstDamage);
                        var powPowRange = GetRealPowPowRange(t);

                        if (distance < (powPowRange + QAddRange) && !(aDamage * 3.5 > t.Health))
                        {
                            if (!W.IsReady() || !(wDamage > t.Health) || W.GetPrediction(t).CollisionObjects.Count > 0)
                            {
                                if (CountAlliesNearTarget(t, 500) <= 3)
                                {
                                    if (rDamage > t.Health && !ObjectManager.Player.IsAutoAttacking &&
                                        !ObjectManager.Player.IsChanneling)
                                    {
                                        if (R.Cast(t, false, true) == Spell.CastStates.SuccessfullyCasted) { }
                                    }
                                }
                            }
                        }
                        else if (distance > (powPowRange + QAddRange))
                        {
                            if (!W.IsReady() || !(wDamage > t.Health) || distance > W.Range ||
                                W.GetPrediction(t).CollisionObjects.Count > 0)
                            {
                                if (CountAlliesNearTarget(t, 500) <= 3)
                                {
                                    if (rDamage > t.Health && !ObjectManager.Player.IsAutoAttacking &&
                                        !ObjectManager.Player.IsChanneling)
                                    {
                                        if (R.Cast(t, false, true) == Spell.CastStates.SuccessfullyCasted) { }
                                    }
                                }
                            }
                        }
                    }
                }
            }
        }

        public override void Orbwalking_AfterAttack(Obj_AI_Base unit, Obj_AI_Base target)
        {
            if ((ComboActive || HarassActive) && unit.IsMe && (target is Obj_AI_Hero))
            {
                var useQ = GetValue<bool>("UseQ" + (ComboActive ? "C" : "H"));
                var useW = GetValue<bool>("UseW" + (ComboActive ? "C" : "H"));

                if (useW && W.IsReady())
                {
                    var t = SimpleTs.GetTarget(W.Range, SimpleTs.DamageType.Physical);
                    var minW = GetValue<Slider>("MinWRange").Value;

                    if (t.IsValidTarget() && GetRealDistance(t) >= minW)
                    {
                        if (W.Cast(t) == Spell.CastStates.SuccessfullyCasted)
                        {
                            return;
                        }
                    }
                }

                if (useQ)
                {
                    foreach (var t in
                        ObjectManager.Get<Obj_AI_Hero>()
                            .Where(t => t.IsValidTarget(GetRealPowPowRange(t) + QAddRange + 20f)))
                    {
                        var swapDistance = GetValue<bool>("SwapDistance");
                        var swapAoe = GetValue<bool>("SwapAOE");
                        var distance = GetRealDistance(t);
                        var powPowRange = GetRealPowPowRange(t);

                        if (swapDistance && Q.IsReady())
                        {
                            if (distance > powPowRange && !FishBoneActive)
                            {
                                if (Q.Cast())
                                {
                                    return;
                                }
                            }
                            else if (distance < powPowRange && FishBoneActive)
                            {
                                if (Q.Cast())
                                {
                                    return;
                                }
                            }
                        }

                        if (swapAoe && Q.IsReady())
                        {
                            if (distance > powPowRange && PowPowStacks > 2 && !FishBoneActive &&
                                CountEnemies(t, 150) > 1)
                            {
                                if (Q.Cast())
                                {
                                    return;
                                }
                            }
                        }
                    }
                }
            }
        }

        private static int CountEnemies(Obj_AI_Base target, float range)
        {
            return
                ObjectManager.Get<Obj_AI_Hero>()
                    .Count(
                        hero =>
                            hero.IsValidTarget() && hero.Team != ObjectManager.Player.Team &&
                            hero.ServerPosition.Distance(target.ServerPosition) <= range);
        }

        private int CountAlliesNearTarget(Obj_AI_Base target, float range)
        {
            return
                ObjectManager.Get<Obj_AI_Hero>()
                    .Count(
                        hero =>
                            hero.Team == ObjectManager.Player.Team &&
                            hero.ServerPosition.Distance(target.ServerPosition) <= range);
        }

        private static float GetRealPowPowRange(GameObject target)
        {
            return 525f + ObjectManager.Player.BoundingRadius + target.BoundingRadius;
        }

        private static float GetRealDistance(GameObject target)
        {
            return ObjectManager.Player.Position.Distance(target.Position) + ObjectManager.Player.BoundingRadius +
                   target.BoundingRadius;
        }

        private static float GetSlowEndTime(Obj_AI_Base target)
        {
            return
                target.Buffs.OrderByDescending(buff => buff.EndTime - Game.Time)
                    .Where(buff => buff.Type == BuffType.Slow)
                    .Select(buff => buff.EndTime)
                    .FirstOrDefault();
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

        public override void LaneClearMenu(Menu config)
        {
            config.AddItem(new MenuItem("SwapQ" + Id, "Always swap to Minigun").SetValue(false));
        }

        public override void MiscMenu(Menu config)
        {
            config.AddItem(new MenuItem("SwapDistance" + Id, "Swap Q for distance").SetValue(true));
            config.AddItem(new MenuItem("SwapAOE" + Id, "Swap Q for AOE").SetValue(false));
            config.AddItem(new MenuItem("MinWRange" + Id, "Min W range").SetValue(new Slider(525 + 65 * 2, 0, 1200)));
            config.AddItem(new MenuItem("AutoEI" + Id, "Auto-E on immobile").SetValue(true));
            config.AddItem(new MenuItem("AutoES" + Id, "Auto-E on slowed").SetValue(true));
            config.AddItem(new MenuItem("AutoED" + Id, "Auto-E on dashing").SetValue(false));
            config.AddItem(
                new MenuItem("CastR" + Id, "Cast R (2000 Range)").SetValue(
                    new KeyBind("T".ToCharArray()[0], KeyBindType.Press)));
            config.AddItem(new MenuItem("ROverKill" + Id, "Check R Overkill").SetValue(true));
            config.AddItem(new MenuItem("MinRRange" + Id, "Min R range").SetValue(new Slider(300, 0, 1500)));
            config.AddItem(new MenuItem("MaxRRange" + Id, "Max R range").SetValue(new Slider(1700, 0, 4000)));
        }

        public override void DrawingMenu(Menu config)
        {
            config.AddItem(
                new MenuItem("DrawQBound" + Id, "Draw Q bound").SetValue(
                    new Circle(true, Color.FromArgb(100, 255, 0, 0))));
            config.AddItem(
                new MenuItem("DrawW" + Id, "W range").SetValue(new Circle(false, Color.FromArgb(100, 255, 255, 255))));
        }
    }
}