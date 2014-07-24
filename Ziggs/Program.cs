#region

using System;
using System.Collections.Generic;
using LeagueSharp;
using LeagueSharp.Common;
using SharpDX;
using Color = System.Drawing.Color;

#endregion

//Ziggs.
//by lepqm && Esk0r

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

        private static void Main(string[] args)
        {
            CustomEvents.Game.OnGameLoad += Game_OnGameLoad;
        }

        private static void Game_OnGameLoad(EventArgs args)
        {
            if (ObjectManager.Player.BaseSkinName != ChampionName) return;

            Q1 = new Spell(SpellSlot.Q, 850);
            Q2 = new Spell(SpellSlot.Q, 1125);
            Q3 = new Spell(SpellSlot.Q, 1400);

            W = new Spell(SpellSlot.W, 1000);
            E = new Spell(SpellSlot.E, 900);
            R = new Spell(SpellSlot.R, 5300);

            Q1.SetSkillshot(0.3f, 130f, 1700f, false, Prediction.SkillshotType.SkillshotCircle);
            Q2.SetSkillshot(0.25f + Q1.Delay, 130f, 1700f, false, Prediction.SkillshotType.SkillshotCircle);
            Q3.SetSkillshot(0.25f + Q2.Delay, 130f, 1700f, false, Prediction.SkillshotType.SkillshotCircle);

            W.SetSkillshot(0.25f, 325f, 1700f, false, Prediction.SkillshotType.SkillshotCircle);
            E.SetSkillshot(0.25f, 100f, 2500f, false, Prediction.SkillshotType.SkillshotCircle);
            R.SetSkillshot(1f, 500f, float.MaxValue, false, Prediction.SkillshotType.SkillshotCircle);

            SpellList.Add(Q1);
            SpellList.Add(Q2);
            SpellList.Add(Q3);
            SpellList.Add(W);
            SpellList.Add(E);
            SpellList.Add(R);

            Config = new Menu(ChampionName, ChampionName, true);

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
                    new MenuItem("HarassActive", "Harass!").SetValue(new KeyBind("C".ToCharArray()[0], KeyBindType.Press)));

            Config.AddSubMenu(new Menu("Farm", "Farm"));
            Config.SubMenu("Farm")
                .AddItem(
                    new MenuItem("UseQFarm", "Use Q").SetValue(
                        new StringList(new[] {"Freeze", "LaneClear", "Both", "No"}, 2)));
            Config.SubMenu("Farm")
                .AddItem(
                    new MenuItem("UseEFarm", "Use E").SetValue(
                        new StringList(new[] {"Freeze", "LaneClear", "Both", "No"}, 1)));
            Config.SubMenu("Farm")
                .AddItem(new MenuItem("ManaSliderFarm", "Mana To Farm").SetValue(new Slider(25, 100, 0)));
            Config.SubMenu("Farm")
                .AddItem(
                    new MenuItem("FreezeActive", "Freeze!").SetValue(new KeyBind("C".ToCharArray()[0], KeyBindType.Press)));
            Config.SubMenu("Farm")
                .AddItem(
                    new MenuItem("LaneClearActive", "LaneClear!").SetValue(new KeyBind("V".ToCharArray()[0],
                        KeyBindType.Press)));

            Config.AddSubMenu(new Menu("Drawings", "Drawings"));
            Config.SubMenu("Drawings")
                .AddItem(
                    new MenuItem("DrawQRange", "Draw Q Range").SetValue(new Circle(true,
                        Color.FromArgb(100, 255, 0, 255))));
            Config.SubMenu("Drawings")
                .AddItem(
                    new MenuItem("DrawWRange", "Draw W Range").SetValue(new Circle(true,
                        Color.FromArgb(100, 255, 255, 255))));
            Config.SubMenu("Drawings")
                .AddItem(
                    new MenuItem("DrawERange", "Draw E Range").SetValue(new Circle(false,
                        Color.FromArgb(100, 255, 255, 255))));
            Config.SubMenu("Drawings")
                .AddItem(
                    new MenuItem("DrawRRange", "Draw R Range").SetValue(new Circle(false,
                        Color.FromArgb(100, 255, 255, 255))));
            Config.SubMenu("Drawings")
                .AddItem(
                    new MenuItem("DrawRRangeM", "Draw R Range (Minimap)").SetValue(new Circle(false,
                        Color.FromArgb(100, 255, 255, 255))));

            Config.AddToMainMenu();

            Game.OnGameUpdate += Game_OnGameUpdate;
            Drawing.OnDraw += Drawing_OnDraw;

            Interrupter.OnPosibleToInterrupt += Interrupter_OnPosibleToInterrupt;

        }

        private static void Drawing_OnDraw(EventArgs args)
        {
            var qValue = Config.Item("DrawQRange").GetValue<Circle>();
            if (qValue.Enabled)
                Utility.DrawCircle(ObjectManager.Player.Position, Q3.Range,
                    qValue.Color);

            var wValue = Config.Item("DrawWRange").GetValue<Circle>();
            if (wValue.Enabled)
                Utility.DrawCircle(ObjectManager.Player.Position, W.Range,
                    wValue.Color);

            var eValue = Config.Item("DrawERange").GetValue<Circle>();
            if (eValue.Enabled)
                Utility.DrawCircle(ObjectManager.Player.Position, E.Range,
                    eValue.Color);

            var rValue = Config.Item("DrawRRange").GetValue<Circle>();
            if (rValue.Enabled)
                Utility.DrawCircle(ObjectManager.Player.Position, R.Range,
                    rValue.Color);

            var rValueM = Config.Item("DrawRRangeM").GetValue<Circle>();
            if (rValueM.Enabled)
                Utility.DrawCircle(ObjectManager.Player.Position, R.Range,
                    rValueM.Color, 2, 30, true);
        }

        private static void Interrupter_OnPosibleToInterrupt(Obj_AI_Base unit, InterruptableSpell spell)
        {
            W.Cast(unit);
        }

        private static void Game_OnGameUpdate(EventArgs args)
        {
            if (Config.Item("ComboActive").GetValue<KeyBind>().Active ||
                (Config.Item("HarassActive").GetValue<KeyBind>().Active &&
                 (ObjectManager.Player.Mana/ObjectManager.Player.MaxMana*100) >
                 Config.Item("ManaSliderHarass").GetValue<Slider>().Value))
            {
                var target = SimpleTs.GetTarget(1200f, SimpleTs.DamageType.Magical);
                if (target != null)
                {
                    if (((Config.Item("ComboActive").GetValue<KeyBind>().Active &&
                          Config.Item("UseQCombo").GetValue<bool>()) ||
                         (Config.Item("HarassActive").GetValue<KeyBind>().Active &&
                          Config.Item("UseQHarass").GetValue<bool>())) && Q1.IsReady())
                    {
                        CastQ(target);
                        return;
                    }

                    if (((Config.Item("ComboActive").GetValue<KeyBind>().Active &&
                          Config.Item("UseWCombo").GetValue<bool>()) ||
                         (Config.Item("HarassActive").GetValue<KeyBind>().Active &&
                          Config.Item("UseWHarass").GetValue<bool>())) && W.IsReady())
                    {
                        W.Cast(target, true, true);
                        return;
                    }

                    if (((Config.Item("ComboActive").GetValue<KeyBind>().Active &&
                          Config.Item("UseECombo").GetValue<bool>()) ||
                         (Config.Item("HarassActive").GetValue<KeyBind>().Active &&
                          Config.Item("UseEHarass").GetValue<bool>())) && E.IsReady())
                    {
                        E.Cast(target, true, true);
                        return;
                    }

                    if (Config.Item("ComboActive").GetValue<KeyBind>().Active &&
                        Config.Item("UseRCombo").GetValue<bool>() && R.IsReady() &&
                        (DamageLib.getDmg(target, DamageLib.SpellType.Q) +
                         DamageLib.getDmg(target, DamageLib.SpellType.W) +
                         DamageLib.getDmg(target, DamageLib.SpellType.E) +
                         DamageLib.getDmg(target, DamageLib.SpellType.R) > target.Health))
                    {
                        R.Delay = 2000 + 1500*target.Distance(ObjectManager.Player)/5300;
                        R.Cast(target, true, true);
                        return;
                    }
                }
            }

            var lc = Config.Item("LaneClearActive").GetValue<KeyBind>().Active;
            if (lc || Config.Item("FreezeActive").GetValue<KeyBind>().Active)
                Farm(lc);
        }

        private static void CastQ(Obj_AI_Base target)
        {
            Prediction.PredictionOutput prediction;

            if (ObjectManager.Player.Distance(target) < Q1.Range)
            {
                prediction = Q1.GetPrediction(target, true);
            }
            else if (ObjectManager.Player.Distance(target) < Q2.Range)
            {
                prediction = Q2.GetPrediction(target, true);
            }
            else if (ObjectManager.Player.Distance(target) < Q3.Range)
            {
                prediction = Q3.GetPrediction(target, true);
            }
            else
            {
                return;
            }

            if (prediction.HitChance >= Prediction.HitChance.HighHitchance)
            {
                if (ObjectManager.Player.ServerPosition.Distance(prediction.CastPosition) <= Q1.Range + Q1.Width)
                {
                    Q1.Cast(prediction.CastPosition -
                            100*
                            (prediction.CastPosition.To2D() - ObjectManager.Player.ServerPosition.To2D()).Normalized()
                                .To3D());
                }
                else if (ObjectManager.Player.ServerPosition.Distance(prediction.CastPosition) <=
                         ((Q1.Range + Q2.Range)/2))
                {
                    var p = ObjectManager.Player.ServerPosition.To2D() +
                            (Q1.Range - 100)*
                            (prediction.CastPosition.To2D() - ObjectManager.Player.ServerPosition.To2D()).Normalized
                                ();

                    if (!CheckQCollision(target, prediction.Position, p.To3D()))
                        Q1.Cast(p.To3D());
                }
                else
                {
                    var p = ObjectManager.Player.ServerPosition.To2D() +
                            Q1.Range*
                            (prediction.CastPosition.To2D() - ObjectManager.Player.ServerPosition.To2D()).Normalized
                                ();

                    if (!CheckQCollision(target, prediction.Position, p.To3D()))
                        Q1.Cast(p.To3D());
                }
            }
        }

        private static bool CheckQCollision(Obj_AI_Base target, Vector3 targetPosition, Vector3 castPosition)
        {
            var direction = (castPosition.To2D() - ObjectManager.Player.ServerPosition.To2D()).Normalized();
            var firstBouncePosition = castPosition.To2D();
            var secondBouncePosition = firstBouncePosition +
                                       direction*0.4f*
                                       ObjectManager.Player.ServerPosition.To2D().Distance(firstBouncePosition);
            var thirdBouncePosition = secondBouncePosition +
                                      direction*0.6f*firstBouncePosition.Distance(secondBouncePosition);

            //TODO: Check for wall collision.

            if (thirdBouncePosition.Distance(targetPosition.To2D()) < Q1.Width + target.BoundingRadius)
            {
                //Check the second one.
                foreach (var minion in ObjectManager.Get<Obj_AI_Minion>())
                {
                    if (minion.IsValidTarget(3000))
                    {
                        var predictedPos = Q2.GetPrediction(minion);
                        if (predictedPos.Position.To2D().Distance(secondBouncePosition) <
                            Q2.Width + minion.BoundingRadius)
                            return true;
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
                        if (predictedPos.Position.To2D().Distance(firstBouncePosition) <
                            Q1.Width + minion.BoundingRadius)
                            return true;
                    }
                }

                return false;
            }

            return true;
        }

        private static void Farm(bool laneClear)
        {
            if (!Orbwalking.CanMove(40)) return;
            if (Config.Item("ManaSliderFarm").GetValue<Slider>().Value >
                ObjectManager.Player.Mana/ObjectManager.Player.MaxMana*100) return;

            var rangedMinions = MinionManager.GetMinions(ObjectManager.Player.ServerPosition, Q2.Range,
                MinionTypes.Ranged, MinionTeam.NotAlly);
            var allMinions = MinionManager.GetMinions(ObjectManager.Player.ServerPosition, Q2.Range, MinionTypes.All, MinionTeam.NotAlly);

            var useQi = Config.Item("UseQFarm").GetValue<StringList>().SelectedIndex;
            var useEi = Config.Item("UseEFarm").GetValue<StringList>().SelectedIndex;
            var useQ = (laneClear && (useQi == 1 || useQi == 2)) || (!laneClear && (useQi == 0 || useQi == 2));
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

                if (E.IsReady() && useE)
                {
                    var rangedLocation = E.GetCircularFarmLocation(rangedMinions, E.Width*2);
                    var location = E.GetCircularFarmLocation(allMinions, E.Width*2);

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
                    foreach (var minion in allMinions)
                    {
                        if (!Orbwalking.InAutoAttackRange(minion))
                        {
                            var Qdamage = DamageLib.getDmg(minion, DamageLib.SpellType.Q)*0.75;

                            if (Qdamage > Q1.GetHealthPrediction(minion))
                            {
                                Q2.Cast(minion);
                            }
                        }
                    }

                if (E.IsReady() && useE)
                {
                    var rangedLocation = E.GetCircularFarmLocation(rangedMinions);
                    var location = E.GetCircularFarmLocation(allMinions);

                    var bLocation = (location.MinionsHit > rangedLocation.MinionsHit + 1) ? location : rangedLocation;

                    if (bLocation.MinionsHit > 2)
                    {
                        E.Cast(bLocation.Position.To3D());
                    }
                }
            }
        }
    }
}