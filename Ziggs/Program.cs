#region

using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Input;
using LeagueSharp;
using LeagueSharp.Common;
using SharpDX;
using Color = System.Drawing.Color;

#endregion

namespace Ziggs
{
    internal class Program
    {
        public static string ChampionName = "Ziggs";
        public static Orbwalking.Orbwalker Orbwalker;
        public static List<Spell> SpellList = new List<Spell>();
        public static Spell Q1;
        public static Spell Q2;
        public static Spell Q3;
        public static Spell W;
        public static Spell E;
        public static Spell R;
        public static Menu Config;

        public static int LastWToMouseT = 0;
        public static int UseSecondWT = 0;

        private static void Main(string[] args)
        {
            CustomEvents.Game.OnGameLoad += Game_OnGameLoad;
        }

        private static void Game_OnGameLoad(EventArgs args)
        {
            if (ObjectManager.Player.ChampionName != ChampionName)
            {
                return;
            }

            Q1 = new Spell(SpellSlot.Q, 850f);
            Q2 = new Spell(SpellSlot.Q, 1125f);
            Q3 = new Spell(SpellSlot.Q, 1400f);

            W = new Spell(SpellSlot.W, 1000f);
            E = new Spell(SpellSlot.E, 900f);
            R = new Spell(SpellSlot.R, 5300f);

            Q1.SetSkillshot(0.3f, 130f, 1700f, false, SkillshotType.SkillshotCircle);
            Q2.SetSkillshot(0.25f + Q1.Delay, 130f, 1700f, false, SkillshotType.SkillshotCircle);
            Q3.SetSkillshot(0.3f + Q2.Delay, 130f, 1700f, false, SkillshotType.SkillshotCircle);

            W.SetSkillshot(0.25f, 275f, 1750f, false, SkillshotType.SkillshotCircle);
            E.SetSkillshot(0.5f, 100f, 1750f, false, SkillshotType.SkillshotCircle);
            R.SetSkillshot(1f, 500f, float.MaxValue, false, SkillshotType.SkillshotCircle);

            SpellList.Add(Q1);
            SpellList.Add(Q2);
            SpellList.Add(Q3);
            SpellList.Add(W);
            SpellList.Add(E);
            SpellList.Add(R);

            Config = new Menu(ChampionName, ChampionName, true);

            var targetSelectorMenu = new Menu("Target Selector", "Target Selector");
            TargetSelector.AddToMenu(targetSelectorMenu);
            Config.AddSubMenu(targetSelectorMenu);

            Config.AddSubMenu(new Menu("Orbwalking", "Orbwalking"));
            Orbwalker = new Orbwalking.Orbwalker(Config.SubMenu("Orbwalking"));

            Config.AddSubMenu(new Menu("Combo", "Combo"));
            Config.SubMenu("Combo").AddItem(new MenuItem("UseQCombo", "Use Q").SetValue(true));
            Config.SubMenu("Combo").AddItem(new MenuItem("UseWCombo", "Use W").SetValue(true));
            Config.SubMenu("Combo").AddItem(new MenuItem("UseECombo", "Use E").SetValue(true));
            Config.SubMenu("Combo").AddItem(new MenuItem("UseRCombo", "Use R").SetValue(true));
            Config.SubMenu("Combo")
                .AddItem(new MenuItem("ComboActive", "Combo!").SetValue(new KeyBind(32, KeyBindType.Press)));

            Config.AddSubMenu(new Menu("Harass", "Harass"));
            Config.SubMenu("Harass").AddItem(new MenuItem("UseQHarass", "Use Q").SetValue(true));
            Config.SubMenu("Harass").AddItem(new MenuItem("UseWHarass", "Use W").SetValue(false));
            Config.SubMenu("Harass").AddItem(new MenuItem("UseEHarass", "Use E").SetValue(false));
            Config.SubMenu("Harass")
                .AddItem(new MenuItem("ManaSliderHarass", "Mana To Harass").SetValue(new Slider(50, 100, 0)));
            Config.SubMenu("Harass")
                .AddItem(
                    new MenuItem("HarassActive", "Harass!").SetValue(
                        new KeyBind("C".ToCharArray()[0], KeyBindType.Press)));

            Config.AddSubMenu(new Menu("Farm", "Farm"));
            Config.SubMenu("Farm")
                .AddItem(
                    new MenuItem("UseQFarm", "Use Q").SetValue(
                        new StringList(new[] { "Freeze", "LaneClear", "Both", "No" }, 2)));
            Config.SubMenu("Farm")
                .AddItem(
                    new MenuItem("UseWFarm", "Use W").SetValue(
                        new StringList(new[] { "Freeze", "LaneClear", "Both", "No" }, 2)));
            Config.SubMenu("Farm")
                .AddItem(
                    new MenuItem("UseEFarm", "Use E").SetValue(
                        new StringList(new[] { "Freeze", "LaneClear", "Both", "No" }, 1)));
            Config.SubMenu("Farm")
                .AddItem(new MenuItem("ManaSliderFarm", "Mana To Farm").SetValue(new Slider(25, 100, 0)));
            Config.SubMenu("Farm")
                .AddItem(
                    new MenuItem("FreezeActive", "Freeze!").SetValue(
                        new KeyBind("C".ToCharArray()[0], KeyBindType.Press)));
            Config.SubMenu("Farm")
                .AddItem(
                    new MenuItem("LaneClearActive", "LaneClear!").SetValue(
                        new KeyBind("V".ToCharArray()[0], KeyBindType.Press)));

            Config.AddSubMenu(new Menu("JungleFarm", "JungleFarm"));
            Config.SubMenu("JungleFarm").AddItem(new MenuItem("UseQJFarm", "Use Q").SetValue(true));
            Config.SubMenu("JungleFarm").AddItem(new MenuItem("UseEJFarm", "Use E").SetValue(true));
            Config.SubMenu("JungleFarm")
                .AddItem(
                    new MenuItem("JungleFarmActive", "JungleFarm!").SetValue(
                        new KeyBind("V".ToCharArray()[0], KeyBindType.Press)));

            Config.AddSubMenu(new Menu("Misc", "Misc"));
            Config.SubMenu("Misc")
                .AddItem(
                    new MenuItem("WToMouse", "W to mouse").SetValue(
                        new KeyBind("T".ToCharArray()[0], KeyBindType.Press)));
            Config.SubMenu("Misc").AddItem(new MenuItem("Peel", "Use W defensively").SetValue(true));


            Config.AddSubMenu(new Menu("Drawings", "Drawings"));
            Config.SubMenu("Drawings")
                .AddItem(
                    new MenuItem("DrawQRange", "Draw Q Range").SetValue(
                        new Circle(true, Color.FromArgb(100, 255, 0, 255))));
            Config.SubMenu("Drawings")
                .AddItem(
                    new MenuItem("DrawWRange", "Draw W Range").SetValue(
                        new Circle(true, Color.FromArgb(100, 255, 255, 255))));
            Config.SubMenu("Drawings")
                .AddItem(
                    new MenuItem("DrawERange", "Draw E Range").SetValue(
                        new Circle(false, Color.FromArgb(100, 255, 255, 255))));
            Config.SubMenu("Drawings")
                .AddItem(
                    new MenuItem("DrawRRange", "Draw R Range").SetValue(
                        new Circle(false, Color.FromArgb(100, 255, 255, 255))));
            Config.SubMenu("Drawings")
                .AddItem(
                    new MenuItem("DrawRRangeM", "Draw R Range (Minimap)").SetValue(
                        new Circle(false, Color.FromArgb(100, 255, 255, 255))));

            Config.AddToMainMenu();

            Game.OnUpdate += Game_OnGameUpdate;
            Drawing.OnDraw += Drawing_OnDraw;

            AntiGapcloser.OnEnemyGapcloser += AntiGapcloser_OnEnemyGapcloser;
            Interrupter2.OnInterruptableTarget += Interrupter2_OnInterruptableTarget;
        }

        static void Interrupter2_OnInterruptableTarget(Obj_AI_Hero sender, Interrupter2.InterruptableTargetEventArgs args)
        {
            if (sender.IsAlly)
            {
                return;
            }

            W.Cast(sender);
        }

        private static void AntiGapcloser_OnEnemyGapcloser(ActiveGapcloser gapcloser)
        {
            W.Cast(gapcloser.Sender);
        }

        private static void Drawing_OnDraw(EventArgs args)
        {
            var qValue = Config.Item("DrawQRange").GetValue<Circle>();
            if (qValue.Active)
            {
                Render.Circle.DrawCircle(ObjectManager.Player.Position, Q3.Range, qValue.Color);
            }

            var wValue = Config.Item("DrawWRange").GetValue<Circle>();
            if (wValue.Active)
            {
                Render.Circle.DrawCircle(ObjectManager.Player.Position, W.Range, wValue.Color);
            }

            var eValue = Config.Item("DrawERange").GetValue<Circle>();
            if (eValue.Active)
            {
                Render.Circle.DrawCircle(ObjectManager.Player.Position, E.Range, eValue.Color);
            }

            var rValue = Config.Item("DrawRRange").GetValue<Circle>();
            if (rValue.Active)
            {
                Render.Circle.DrawCircle(ObjectManager.Player.Position, R.Range, rValue.Color);
            }

            var rValueM = Config.Item("DrawRRangeM").GetValue<Circle>();
            if (rValueM.Active)
            {
                Utility.DrawCircle(ObjectManager.Player.Position, R.Range, rValueM.Color, 2, 30, true);
            }
        }

        private static void Game_OnGameUpdate(EventArgs args)
        {
            //Combo & Harass
            if (Config.Item("ComboActive").GetValue<KeyBind>().Active ||
                (Config.Item("HarassActive").GetValue<KeyBind>().Active &&
                 (ObjectManager.Player.Mana / ObjectManager.Player.MaxMana * 100) >
                 Config.Item("ManaSliderHarass").GetValue<Slider>().Value))
            {
                var target = TargetSelector.GetTarget(1200f, TargetSelector.DamageType.Magical);
                if (target != null)
                {
                    var comboActive = Config.Item("ComboActive").GetValue<KeyBind>().Active;
                    var harassActive = Config.Item("HarassActive").GetValue<KeyBind>().Active;

                    if (((comboActive && Config.Item("UseQCombo").GetValue<bool>()) ||
                         (harassActive && Config.Item("UseQHarass").GetValue<bool>())) && Q1.IsReady())
                    {
                        CastQ(target);
                    }

                    if (((comboActive && Config.Item("UseWCombo").GetValue<bool>()) ||
                         (harassActive && Config.Item("UseWHarass").GetValue<bool>())) && W.IsReady())
                    {
                        var prediction = W.GetPrediction(target);
                        if (prediction.Hitchance >= HitChance.High)
                        {
                            if (ObjectManager.Player.ServerPosition.Distance(prediction.UnitPosition) < W.Range &&
                                ObjectManager.Player.ServerPosition.Distance(prediction.UnitPosition) > W.Range - 250 &&
                                prediction.UnitPosition.Distance(ObjectManager.Player.ServerPosition) >
                                target.Distance(ObjectManager.Player))
                            {
                                var cp =
                                    ObjectManager.Player.ServerPosition.To2D()
                                        .Extend(prediction.UnitPosition.To2D(), W.Range)
                                        .To3D();
                                W.Cast(cp);
                                UseSecondWT = Utils.TickCount;
                            }
                        }
                    }

                    if (((comboActive && Config.Item("UseECombo").GetValue<bool>()) ||
                         (harassActive && Config.Item("UseEHarass").GetValue<bool>())) && E.IsReady())
                    {
                        E.Cast(target, false, true);
                    }

                    var useR = Config.Item("UseRCombo").GetValue<bool>();

                    //R at close range
                    if (comboActive && useR && R.IsReady() &&
                        (ObjectManager.Player.GetSpellDamage(target, SpellSlot.Q) +
                         ObjectManager.Player.GetSpellDamage(target, SpellSlot.W) +
                         ObjectManager.Player.GetSpellDamage(target, SpellSlot.E) +
                         ObjectManager.Player.GetSpellDamage(target, SpellSlot.R) > target.Health) &&
                        ObjectManager.Player.Distance(target) <= Q2.Range)
                    {
                        R.Delay = 2000 + 1500 * target.Distance(ObjectManager.Player) / 5300;
                        R.Cast(target, true, true);
                    }

                    //R aoe in teamfights
                    if (comboActive && useR && R.IsReady())
                    {
                        var alliesarround = 0;
                        var n = 0;
                        foreach (var ally in ObjectManager.Get<Obj_AI_Hero>())
                        {
                            if (ally.IsAlly && !ally.IsMe && ally.IsValidTarget(float.MaxValue, false) &&
                                ally.Distance(target) < 700)
                            {
                                alliesarround++;
                                if (Utils.TickCount - ally.LastCastedSpellT() < 1500)
                                {
                                    n++;
                                }
                            }
                        }

                        if (n < Math.Max(alliesarround / 2 - 1, 1))
                        {
                            return;
                        }

                        switch (alliesarround)
                        {
                            case 2:
                                R.CastIfWillHit(target, 2);
                                break;
                            case 3:
                                R.CastIfWillHit(target, 3);
                                break;
                            case 4:
                                R.CastIfWillHit(target, 4);
                                break;
                        }
                    }

                    //R if killable
                    if (comboActive && useR && R.IsReady() &&
                        ObjectManager.Player.GetSpellDamage(target, SpellSlot.R) > target.Health)
                    {
                        R.Delay = 2000 + 1500 * target.Distance(ObjectManager.Player) / 5300;
                        R.Cast(target, true, true);
                    }
                }
            }

            if (Utils.TickCount - UseSecondWT < 500 &&
                ObjectManager.Player.Spellbook.GetSpell(SpellSlot.W).Name == "ziggswtoggle")
            {
                W.Cast(ObjectManager.Player.ServerPosition, true);
            }

            //Farm
            var lc = Config.Item("LaneClearActive").GetValue<KeyBind>().Active;
            if (lc || Config.Item("FreezeActive").GetValue<KeyBind>().Active)
            {
                Farm(lc);
            }

            //Jungle farm.
            if (Config.Item("JungleFarmActive").GetValue<KeyBind>().Active)
            {
                JungleFarm();
            }

            //W to mouse
            var castToMouse = Config.Item("WToMouse").GetValue<KeyBind>().Active;
            if (castToMouse || Utils.TickCount - LastWToMouseT < 400)
            {
                var pos = ObjectManager.Player.ServerPosition.To2D().Extend(Game.CursorPos.To2D(), -150).To3D();
                W.Cast(pos, true);
                if (castToMouse)
                {
                    LastWToMouseT = Utils.TickCount;
                }
            }

            //Peel from melees
            if (Config.Item("Peel").GetValue<bool>())
            {
                foreach (var pos in from enemy in ObjectManager.Get<Obj_AI_Hero>()
                    where
                        enemy.IsValidTarget() &&
                        enemy.Distance(ObjectManager.Player) <=
                        enemy.BoundingRadius + enemy.AttackRange + ObjectManager.Player.BoundingRadius &&
                        enemy.IsMelee()
                    let direction =
                        (enemy.ServerPosition.To2D() - ObjectManager.Player.ServerPosition.To2D()).Normalized()
                    let pos = ObjectManager.Player.ServerPosition.To2D()
                    select pos + Math.Min(200, Math.Max(50, enemy.Distance(ObjectManager.Player) / 2)) * direction)
                {
                    W.Cast(pos.To3D(), true);
                    UseSecondWT = Utils.TickCount;
                }
            }
        }

        private static void CastQ(Obj_AI_Base target)
        {
            PredictionOutput prediction;

            if (ObjectManager.Player.Distance(target) < Q1.Range)
            {
                var oldrange = Q1.Range;
                Q1.Range = Q2.Range;
                prediction = Q1.GetPrediction(target, true);
                Q1.Range = oldrange;
            }
            else if (ObjectManager.Player.Distance(target) < Q2.Range)
            {
                var oldrange = Q2.Range;
                Q2.Range = Q3.Range;
                prediction = Q2.GetPrediction(target, true);
                Q2.Range = oldrange;
            }
            else if (ObjectManager.Player.Distance(target) < Q3.Range)
            {
                prediction = Q3.GetPrediction(target, true);
            }
            else
            {
                return;
            }

            if (prediction.Hitchance >= HitChance.High)
            {
                if (ObjectManager.Player.ServerPosition.Distance(prediction.CastPosition) <= Q1.Range + Q1.Width)
                {
                    Vector3 p;
                    if (ObjectManager.Player.ServerPosition.Distance(prediction.CastPosition) > 300)
                    {
                        p = prediction.CastPosition -
                            100 *
                            (prediction.CastPosition.To2D() - ObjectManager.Player.ServerPosition.To2D()).Normalized()
                                .To3D();
                    }
                    else
                    {
                        p = prediction.CastPosition;
                    }

                    Q1.Cast(p);
                }
                else if (ObjectManager.Player.ServerPosition.Distance(prediction.CastPosition) <=
                         ((Q1.Range + Q2.Range) / 2))
                {
                    var p = ObjectManager.Player.ServerPosition.To2D()
                        .Extend(prediction.CastPosition.To2D(), Q1.Range - 100);

                    if (!CheckQCollision(target, prediction.UnitPosition, p.To3D()))
                    {
                        Q1.Cast(p.To3D());
                    }
                }
                else
                {
                    var p = ObjectManager.Player.ServerPosition.To2D() +
                            Q1.Range *
                            (prediction.CastPosition.To2D() - ObjectManager.Player.ServerPosition.To2D()).Normalized
                                ();

                    if (!CheckQCollision(target, prediction.UnitPosition, p.To3D()))
                    {
                        Q1.Cast(p.To3D());
                    }
                }
            }
        }

        private static bool CheckQCollision(Obj_AI_Base target, Vector3 targetPosition, Vector3 castPosition)
        {
            var direction = (castPosition.To2D() - ObjectManager.Player.ServerPosition.To2D()).Normalized();
            var firstBouncePosition = castPosition.To2D();
            var secondBouncePosition = firstBouncePosition +
                                       direction * 0.4f *
                                       ObjectManager.Player.ServerPosition.To2D().Distance(firstBouncePosition);
            var thirdBouncePosition = secondBouncePosition +
                                      direction * 0.6f * firstBouncePosition.Distance(secondBouncePosition);

            //TODO: Check for wall collision.

            if (thirdBouncePosition.Distance(targetPosition.To2D()) < Q1.Width + target.BoundingRadius)
            {
                //Check the second one.
                foreach (var minion in ObjectManager.Get<Obj_AI_Minion>())
                {
                    if (minion.IsValidTarget(3000))
                    {
                        var predictedPos = Q2.GetPrediction(minion);
                        if (predictedPos.UnitPosition.To2D().Distance(secondBouncePosition) <
                            Q2.Width + minion.BoundingRadius)
                        {
                            return true;
                        }
                    }
                }
            }

            if (secondBouncePosition.Distance(targetPosition.To2D()) < Q1.Width + target.BoundingRadius ||
                thirdBouncePosition.Distance(targetPosition.To2D()) < Q1.Width + target.BoundingRadius)
            {
                //Check the first one
                foreach (var minion in ObjectManager.Get<Obj_AI_Minion>())
                {
                    if (minion.IsValidTarget(3000))
                    {
                        var predictedPos = Q1.GetPrediction(minion);
                        if (predictedPos.UnitPosition.To2D().Distance(firstBouncePosition) <
                            Q1.Width + minion.BoundingRadius)
                        {
                            return true;
                        }
                    }
                }

                return false;
            }

            return true;
        }

        private static void Farm(bool laneClear)
        {
            if (!Orbwalking.CanMove(40))
            {
                return;
            }
            if (Config.Item("ManaSliderFarm").GetValue<Slider>().Value >
                ObjectManager.Player.Mana / ObjectManager.Player.MaxMana * 100)
            {
                return;
            }

            var rangedMinions = MinionManager.GetMinions(
                ObjectManager.Player.ServerPosition, Q2.Range, MinionTypes.Ranged);
            var allMinions = MinionManager.GetMinions(ObjectManager.Player.ServerPosition, Q2.Range);

            var useQi = Config.Item("UseQFarm").GetValue<StringList>().SelectedIndex;
            var useWi = Config.Item("UseWFarm").GetValue<StringList>().SelectedIndex;
            var useEi = Config.Item("UseEFarm").GetValue<StringList>().SelectedIndex;
            var useQ = (laneClear && (useQi == 1 || useQi == 2)) || (!laneClear && (useQi == 0 || useQi == 2));
            var useW = (laneClear && (useWi == 1 || useWi == 2)) || (!laneClear && (useWi == 0 || useWi == 2));
            var useE = (laneClear && (useEi == 1 || useEi == 2)) || (!laneClear && (useEi == 0 || useEi == 2));

            if (laneClear)
            {
                if (Q1.IsReady() && useQ)
                {
                    var rangedLocation = Q2.GetCircularFarmLocation(rangedMinions);
                    var location = Q2.GetCircularFarmLocation(allMinions);

                    var bLocation = (location.MinionsHit > rangedLocation.MinionsHit + 1) ? location : rangedLocation;

                    if (bLocation.MinionsHit > 0)
                    {
                        Q2.Cast(bLocation.Position.To3D());
                    }
                }

                if (W.IsReady() && useW)
                {
                    var dmgpct = new[] { 25, 27.5, 30, 32.5, 35 }[W.Level - 1];

                    var killableTurret =
                        ObjectManager.Get<Obj_AI_Turret>()
                            .Find(x => x.IsEnemy && ObjectManager.Player.Distance(x.Position) <= W.Range && x.HealthPercent < dmgpct);
                    if (killableTurret != null)
                    {
                        W.Cast(killableTurret.Position);
                    }
                }

                if (E.IsReady() && useE)
                {
                    var rangedLocation = E.GetCircularFarmLocation(rangedMinions, E.Width * 2);
                    var location = E.GetCircularFarmLocation(allMinions, E.Width * 2);

                    var bLocation = (location.MinionsHit > rangedLocation.MinionsHit + 1) ? location : rangedLocation;

                    if (bLocation.MinionsHit > 2)
                    {
                        E.Cast(bLocation.Position.To3D());
                    }
                }
            }
            else
            {
                if (useQ && Q1.IsReady())
                {
                    foreach (var minion in allMinions)
                    {
                        if (!Orbwalking.InAutoAttackRange(minion))
                        {
                            var Qdamage = ObjectManager.Player.GetSpellDamage(minion, SpellSlot.Q) * 0.75;

                            if (Qdamage > Q1.GetHealthPrediction(minion))
                            {
                                Q2.Cast(minion);
                            }
                        }
                    }
                }

                if (E.IsReady() && useE)
                {
                    var rangedLocation = E.GetCircularFarmLocation(rangedMinions, E.Width * 2);
                    var location = E.GetCircularFarmLocation(allMinions, E.Width * 2);

                    var bLocation = (location.MinionsHit > rangedLocation.MinionsHit + 1) ? location : rangedLocation;

                    if (bLocation.MinionsHit > 2)
                    {
                        E.Cast(bLocation.Position.To3D());
                    }
                }
            }
        }

        private static void JungleFarm()
        {
            var useQ = Config.Item("UseQJFarm").GetValue<bool>();
            var useE = Config.Item("UseEJFarm").GetValue<bool>();

            var mobs = MinionManager.GetMinions(
                ObjectManager.Player.ServerPosition, Q1.Range, MinionTypes.All, MinionTeam.Neutral,
                MinionOrderTypes.MaxHealth);

            if (mobs.Count > 0)
            {
                var mob = mobs[0];

                if (useQ && Q1.IsReady())
                {
                    Q1.Cast(mob);
                }


                if (useE)
                {
                    E.Cast(mob);
                }
            }
        }
    }
}
