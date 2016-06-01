#region

using System;
using System.Collections.Generic;
using System.Data.SqlTypes;
using System.Linq;
using System.Reflection.Emit;
using System.Security.AccessControl;
using LeagueSharp;
using LeagueSharp.Common;
using SharpDX;
using Color = System.Drawing.Color;

#endregion

namespace Orianna
{
    internal class Program
    {
        public const string ChampionName = "Orianna";

        public static Orbwalking.Orbwalker Orbwalker;

        public static Spell Q;
        public static Spell W;
        public static Spell E;
        public static Spell R;

        public static bool QIsReady;
        public static bool WIsReady;
        public static bool EIsReady;
        public static bool RIsReady;

        public static Menu Config;

        private static Obj_AI_Hero Player;

        private static Dictionary<string, string> InitiatorsList = new Dictionary<string, string>
        {
            {"aatroxq", "Aatrox"},
            {"akalishadowdance", "Akali"},
            {"headbutt", "Alistar"},
            {"bandagetoss", "Amumu"},
            {"dianateleport", "Diana"},
            {"ekkoe", "ekko"},
            {"elisespidereinitial", "Elise"},
            {"crowstorm", "FiddleSticks"},
            {"fioraq", "Fiora"},
            {"gnare", "Gnar"},
            {"gnarbige", "Gnar"},
            {"gragase", "Gragas"},
            {"hecarimult", "Hecarim"},
            {"ireliagatotsu", "Irelia"},
            {"jarvanivdragonstrike", "JarvanIV"},
            {"jaxleapstrike", "Jax"},
            {"riftwalk", "Kassadin"},
            {"katarinae", "Katarina"},
            {"kennenlightningrush", "Kennen"},
            {"khazixe", "KhaZix"},
            {"khazixelong", "KhaZix"},
            {"blindmonkqtwo", "LeeSin"},
            {"leonazenithblademissle", "Leona"},
            {"lissandrae", "Lissandra"},
            {"ufslash", "Malphite"},
            {"maokaiunstablegrowth", "Maokai"},
            {"monkeykingnimbus", "MonkeyKing"},
            {"monkeykingspintowin", "MonkeyKing"},
            {"summonerflash", "MonkeyKing"},
            {"nocturneparanoia", "Nocturne"},
            {"olafragnarok", "Olaf"},
            {"poppyheroiccharge", "Poppy"},
            {"renektonsliceanddice", "Renekton"},
            {"rengarr", "Rengar"},
            {"reksaieburrowed", "RekSai"},
            {"sejuaniarcticassault", "Sejuani"},
            {"shenshadowdash", "Shen"},
            {"shyvanatransformcast", "Shyvana"},
            {"shyvanatransformleap", "Shyvana"},
            {"sionr", "Sion"},
            {"taloncutthroat", "Talon"},
            {"threshqleap", "Thresh"},
            {"slashcast", "Tryndamere"},
            {"udyrbearstance", "Udyr"},
            {"urgotswap2", "Urgot"},
            {"viq", "Vi"},
            {"vir", "Vi"},
            {"volibearq", "Volibear"},
            {"infiniteduress", "Warwick"},
            {"yasuorknockupcombow", "Yasuo"},
            {"zace", "Zac"}
        };

        private static void Main(string[] args)
        {
            CustomEvents.Game.OnGameLoad += Game_OnGameLoad;
        }

        private static void Game_OnGameLoad(EventArgs args)
        {
            Player = ObjectManager.Player;

            if (Player.ChampionName != ChampionName) return;

            Q = new Spell(SpellSlot.Q, 825);
            W = new Spell(SpellSlot.W, 245);
            E = new Spell(SpellSlot.E, 1095);
            R = new Spell(SpellSlot.R, 380);

            Q.SetSkillshot(0f, 130f, 1400f, false, SkillshotType.SkillshotCircle);
            W.SetSkillshot(0.25f, 240f, float.MaxValue, false, SkillshotType.SkillshotCircle);
            E.SetSkillshot(0.25f, 80f, 1700f, false, SkillshotType.SkillshotLine);
            R.SetSkillshot(0.6f, 375f, float.MaxValue, false, SkillshotType.SkillshotCircle);


            Config = new Menu(ChampionName, ChampionName, true);
            TargetSelector.AddToMenu(Config.SubMenu("Target Selector"));
            Orbwalker = new Orbwalking.Orbwalker(Config.SubMenu("Orbwalking"));

            #region Combo
            Config.SubMenu("Combo").AddItem(new MenuItem("UseQCombo", "Use Q").SetValue(true));
            Config.SubMenu("Combo").AddItem(new MenuItem("UseWCombo", "Use W").SetValue(true));
            Config.SubMenu("Combo").AddItem(new MenuItem("UseECombo", "Use E").SetValue(true));
            Config.SubMenu("Combo").AddItem(new MenuItem("UseRCombo", "Use R").SetValue(true));
            Config.SubMenu("Combo").AddItem(new MenuItem("UseRNCombo", "Use R on at least").SetValue(new StringList(new string[]{"1 target", "2 target", "3 target", "4 target", "5 target"}, 0)));
            Config.SubMenu("Combo").AddItem(new MenuItem("UseRImportant", "-> Or if hero priority >=")).SetValue(new Slider(5, 1, 5)); // 5 for e.g adc's
            Config.SubMenu("Combo")
                .AddItem(
                    new MenuItem("ComboActive", "Combo!").SetValue(
                        new KeyBind(Config.Item("Orbwalk").GetValue<KeyBind>().Key, KeyBindType.Press)));
            #endregion

            #region Misc
            Config.SubMenu("Misc").AddItem(new MenuItem("AutoW", "Auto W if it'll hit").SetValue(new StringList(new string[] { "No", ">=1 target", ">=2 target", ">=3 target", ">=4 target", ">=5 target" }, 2)));
            Config.SubMenu("Misc").AddItem(new MenuItem("AutoR", "Auto R if it'll hit").SetValue(new StringList(new string[] { "No", ">=1 target", ">=2 target", ">=3 target", ">=4 target", ">=5 target" }, 3)));
            Config.SubMenu("Misc").AddItem(new MenuItem("AutoEInitiators", "Auto E initiators").SetValue(true));

            HeroManager.Allies.ForEach(
                delegate(Obj_AI_Hero hero)
                {
                    InitiatorsList.ToList().ForEach(
                        delegate(KeyValuePair<string, string> pair)
                        {
                            if (string.Equals(hero.ChampionName, pair.Value, StringComparison.InvariantCultureIgnoreCase))
                            {
                                Config.SubMenu("Misc")
                                    .SubMenu("Initiator's List")
                                    .AddItem(new MenuItem(pair.Key, pair.Value + " - " + pair.Key).SetValue(true));
                            }
                        });
                });
            
            Config.SubMenu("Misc").AddItem(new MenuItem("InterruptSpells", "Interrupt spells using R").SetValue(true));
            Config.SubMenu("Misc").AddItem(new MenuItem("BlockR", "Block R if it won't hit").SetValue(false));
            #endregion

            #region Harass
            //Harass menu:
            Config.SubMenu("Harass").AddItem(new MenuItem("UseQHarass", "Use Q").SetValue(true));
            Config.SubMenu("Harass").AddItem(new MenuItem("UseWHarass", "Use W").SetValue(false));
            Config.SubMenu("Harass").AddItem(new MenuItem("HarassManaCheck", "Don't harass if mana < %").SetValue(new Slider(0, 0, 100)));
            Config.SubMenu("Harass")
                .AddItem(
                    new MenuItem("HarassActive", "Harass!").SetValue(
                        new KeyBind(Config.Item("Farm").GetValue<KeyBind>().Key, KeyBindType.Press)));
            Config.SubMenu("Harass")
                .AddItem(
                    new MenuItem("HarassActiveT", "Harass (toggle)!").SetValue(new KeyBind("Y".ToCharArray()[0],
                        KeyBindType.Toggle)));
            #endregion

            #region Farming
            //Farming menu:
            Config.SubMenu("Farm").AddItem(new MenuItem("EnabledFarm", "Enable! (On/Off: Mouse Scroll)").SetValue(true));
            Config.SubMenu("Farm")
                .AddItem(
                    new MenuItem("UseQFarm", "Use Q").SetValue(
                        new StringList(new[] { "Freeze", "LaneClear", "Both", "No" }, 2)));
            Config.SubMenu("Farm")
                .AddItem(
                    new MenuItem("UseWFarm", "Use W").SetValue(
                        new StringList(new[] { "Freeze", "LaneClear", "Both", "No" }, 1)));
            Config.SubMenu("Farm")
                .AddItem(
                    new MenuItem("UseEFarm", "Use E").SetValue(
                        new StringList(new[] { "Freeze", "LaneClear", "Both", "No" }, 1)));
            Config.SubMenu("Farm").AddItem(new MenuItem("LaneClearManaCheck", "Don't LaneClear if mana < %").SetValue(new Slider(0, 0, 100)));
            
            Config.SubMenu("Farm")
                .AddItem(
                    new MenuItem("FreezeActive", "Freeze!").SetValue(
                        new KeyBind(Config.Item("Farm").GetValue<KeyBind>().Key, KeyBindType.Press)));
            Config.SubMenu("Farm")
                .AddItem(
                    new MenuItem("LaneClearActive", "LaneClear!").SetValue(
                        new KeyBind(Config.Item("LaneClear").GetValue<KeyBind>().Key, KeyBindType.Press)));

            //JungleFarm menu:
            Config.AddSubMenu(new Menu("JungleFarm", "JungleFarm"));
            Config.SubMenu("JungleFarm").AddItem(new MenuItem("UseQJFarm", "Use Q").SetValue(true));
            Config.SubMenu("JungleFarm").AddItem(new MenuItem("UseWJFarm", "Use W").SetValue(true));
            Config.SubMenu("JungleFarm").AddItem(new MenuItem("UseEJFarm", "Use E").SetValue(true));
            Config.SubMenu("JungleFarm")
                .AddItem(
                    new MenuItem("JungleFarmActive", "JungleFarm!").SetValue(
                        new KeyBind(Config.Item("LaneClear").GetValue<KeyBind>().Key, KeyBindType.Press)));
            #endregion

            #region Drawings
            //Damage after combo:
            var dmgAfterComboItem = new MenuItem("DamageAfterR", "Draw damage after combo").SetValue(true);
            Utility.HpBarDamageIndicator.DamageToUnit += hero => GetComboDamage(hero);
            Utility.HpBarDamageIndicator.Enabled = dmgAfterComboItem.GetValue<bool>();
            dmgAfterComboItem.ValueChanged += delegate(object sender, OnValueChangeEventArgs eventArgs)
            {
                Utility.HpBarDamageIndicator.Enabled = eventArgs.GetNewValue<bool>();
            };

            
            //Drawings menu:
            Config.SubMenu("Drawings")
                .AddItem(
                    new MenuItem("QRange", "Q range").SetValue(new Circle(true, Color.FromArgb(150, Color.DodgerBlue))));
            Config.SubMenu("Drawings")
                .AddItem(
                    new MenuItem("WRange", "W range").SetValue(new Circle(false, Color.FromArgb(150, Color.DodgerBlue))));
            Config.SubMenu("Drawings")
                .AddItem(
                    new MenuItem("ERange", "E range").SetValue(new Circle(false, Color.FromArgb(150, Color.DodgerBlue))));
            Config.SubMenu("Drawings")
                .AddItem(
                    new MenuItem("RRange", "R range").SetValue(new Circle(false, Color.FromArgb(150, Color.DodgerBlue))));
            Config.SubMenu("Drawings")
                .AddItem(
                    new MenuItem("QOnBallRange", "Draw ball position").SetValue(new Circle(true, Color.FromArgb(150, Color.DodgerBlue))));
            Config.SubMenu("Drawings")
                .AddItem(dmgAfterComboItem);
            Config.SubMenu("Drawings")
                .AddItem(
                    new MenuItem("HarassActiveTPermashow", "Show harass permashow").SetValue(true)).ValueChanged += (s, ar) =>
                    {
                        if (ar.GetNewValue<bool>())
                        {
                            Config.Item("HarassActiveT").Permashow(true, "HarassActive");
                        }
                        else
                        {
                            Config.Item("HarassActiveT").Permashow(false);
                        }
                    };

            Config.Item("HarassActiveT").Permashow(Config.Item("HarassActiveTPermashow").GetValue<bool>(), "HarassActive");

            Config.SubMenu("Drawings")
                .AddItem(
                    new MenuItem("EnabledFarmPermashow", "Show farm permashow").SetValue(true)).ValueChanged +=
                (s, ar) =>
                {
                    if (ar.GetNewValue<bool>())
                    {
                        Config.Item("EnabledFarm").Permashow(true, "Enabled Farm");
                    }
                    else
                    {
                        Config.Item("EnabledFarm").Permashow(false);
                    }
                };
            Config.Item("EnabledFarm").Permashow(Config.Item("EnabledFarmPermashow").GetValue<bool>(), "Enabled Farm");
            
            #endregion

            Config.AddToMainMenu();

            Obj_AI_Hero.OnProcessSpellCast += Obj_AI_Hero_OnProcessSpellCast;
            Game.OnUpdate += Game_OnGameUpdate;
            Game.OnWndProc += Game_OnWndProc;
            Drawing.OnDraw += Drawing_OnDraw;
            Spellbook.OnCastSpell += Spellbook_OnCastSpell;
            Interrupter2.OnInterruptableTarget += Interrupter2_OnInterruptableTarget;
        }
        
        private static void Game_OnWndProc(WndEventArgs args)
        {
            if (args.Msg != 0x20a)
                return;

            Config.SubMenu("Farm")
                .Item("EnabledFarm")
                .SetValue(!Config.SubMenu("Farm").Item("EnabledFarm").GetValue<bool>());
        }


        static void Interrupter2_OnInterruptableTarget(Obj_AI_Hero sender, Interrupter2.InterruptableTargetEventArgs args)
        {
            if (!Config.Item("InterruptSpells").GetValue<bool>())
            {
                return;
            }

            if (args.DangerLevel <= Interrupter2.DangerLevel.Medium)
            {
                return;
            }

            if (sender.IsAlly)
            {
                return;
            }

            if (RIsReady)
            {
                Q.Cast(sender, true);
                if (BallManager.BallPosition.Distance(sender.ServerPosition, true) < R.Range * R.Range)
                {
                    R.Cast(Player.ServerPosition, true);
                }
            }
        }

        static void Obj_AI_Hero_OnProcessSpellCast(Obj_AI_Base sender, GameObjectProcessSpellCastEventArgs args)
        {
            if (!(sender is Obj_AI_Hero))
            {
                return;
            }

            if (!Config.SubMenu("Misc").Item("AutoEInitiators").GetValue<bool>())
            {
                return;
            }

            var spellName = args.SData.Name.ToLower();
            if (!InitiatorsList.ContainsKey(spellName))
            {
                return;
            }

            var item = Config.SubMenu("Misc").SubMenu("Initiator's List").Item(spellName);
            if (item == null || !item.GetValue<bool>())
            {
                return;
            }

            if (!EIsReady)
            {
                return;
            }

            if (sender.IsAlly && Player.Distance(sender, true) < E.Range * E.Range)
            {
                E.CastOnUnit(sender);
            }
        }

        static void Spellbook_OnCastSpell(Spellbook sender, SpellbookCastSpellEventArgs args)
        {
            if (!Config.Item("BlockR").GetValue<bool>())
            {
                return;
            }

            if (args.Slot == SpellSlot.R && GetHits(R).Item1 == 0)
            {
                args.Process = false;
            }
        }

        private static void Farm(bool laneClear)
        {
            if (!Config.Item("EnabledFarm").GetValue<bool>())
            {
                return;
            }

            var allMinions = MinionManager.GetMinions(ObjectManager.Player.ServerPosition, Q.Range + W.Width,
                MinionTypes.All);
            var rangedMinions = MinionManager.GetMinions(ObjectManager.Player.ServerPosition, Q.Range + W.Width,
                MinionTypes.Ranged);

            var useQi = Config.Item("UseQFarm").GetValue<StringList>().SelectedIndex;
            var useWi = Config.Item("UseWFarm").GetValue<StringList>().SelectedIndex;
            var useEi = Config.Item("UseWFarm").GetValue<StringList>().SelectedIndex;

            var useQ = (laneClear && (useQi == 1 || useQi == 2)) || (!laneClear && (useQi == 0 || useQi == 2));
            var useW = (laneClear && (useWi == 1 || useWi == 2)) || (!laneClear && (useWi == 0 || useWi == 2));
            var useE = (laneClear && (useEi == 1 || useEi == 2)) || (!laneClear && (useEi == 0 || useEi == 2));

            if (useQ && QIsReady)
            {
                if (useW)
                {
                    var qLocation = Q.GetCircularFarmLocation(allMinions, W.Range);
                    var q2Location = Q.GetCircularFarmLocation(rangedMinions, W.Range);
                    var bestLocation = (qLocation.MinionsHit > q2Location.MinionsHit + 1) ? qLocation : q2Location;

                    if (bestLocation.MinionsHit > 0)
                    {
                        Q.Cast(bestLocation.Position, true);
                        return;
                    }
                }
                else
                {
                    foreach (var minion in allMinions.FindAll(m => !Orbwalking.InAutoAttackRange(m)))
                    {
                        if (HealthPrediction.GetHealthPrediction(minion, Math.Max((int)(minion.ServerPosition.Distance(BallManager.BallPosition) / Q.Speed * 1000) - 100, 0)) < 50)
                        {
                            Q.Cast(minion.ServerPosition, true);
                            return;
                        }
                    }
                }
            }

            if (useW && WIsReady)
            {
                var n = 0;
                var d = 0;
                foreach (var m in allMinions)
                {
                    if (m.Distance(BallManager.BallPosition) <= W.Range)
                    {
                        n++;
                        if (W.GetDamage(m) > m.Health)
                        {
                            d++;
                        }
                    }
                }
                if (n >= 3 || d >= 2)
                {
                    W.Cast(Player.ServerPosition, true);
                    return;
                }
            }

            if (useE && EIsReady)
            {
                if (W.CountHits(allMinions, Player.ServerPosition) >= 3)
                {
                    E.CastOnUnit(Player, true);
                    return;
                }
            }
        }

        private static void JungleFarm()
        {
            var useQ = Config.Item("UseQJFarm").GetValue<bool>();
            var useW = Config.Item("UseWJFarm").GetValue<bool>();
            var useE = Config.Item("UseEJFarm").GetValue<bool>();

            var mobs = MinionManager.GetMinions(Player.ServerPosition, Q.Range, MinionTypes.All,
                MinionTeam.Neutral, MinionOrderTypes.MaxHealth);

            if (mobs.Count > 0)
            {
                var mob = mobs[0];
                if (useW && WIsReady && W.WillHit(mob.ServerPosition, BallManager.BallPosition))
                {
                    W.Cast(Player.ServerPosition, true);
                }
                else if (useQ && QIsReady)
                {
                    Q.Cast(mob, true);
                }
                else if (useE && EIsReady && (!WIsReady || !useW))
                {
                    var closestAlly = HeroManager.Allies
                        .Where(h =>  h.IsValidTarget(E.Range, false))
                        .MinOrDefault(h => h.Distance(mob));
                    if (closestAlly != null)
                    {
                        E.CastOnUnit(closestAlly, true);
                    }
                }
            }
        }

        public static Tuple<int, Vector3> GetBestQLocation(Obj_AI_Hero mainTarget)
        {
            var points = new List<Vector2>();
            var qPrediction = Q.GetPrediction(mainTarget);
            if (qPrediction.Hitchance < HitChance.VeryHigh)
            {
                return new Tuple<int, Vector3>(1, Vector3.Zero);
            }
            points.Add(qPrediction.UnitPosition.To2D());

            foreach (var enemy in HeroManager.Enemies.Where(h => h.IsValidTarget(Q.Range + R.Range)))
            {
                var prediction = Q.GetPrediction(enemy);
                if (prediction.Hitchance >= HitChance.High)
                {
                   points.Add(prediction.UnitPosition.To2D()); 
                }
            }

            for (int j = 0; j < 5; j++)
            {
                var mecResult = MEC.GetMec(points);
                
                if (mecResult.Radius < (R.Range - 75) && points.Count >= 3 && RIsReady)
                {
                    return new Tuple<int, Vector3>(3, mecResult.Center.To3D());
                }

                if (mecResult.Radius < (W.Range - 75) && points.Count >= 2 && WIsReady)
                {
                    return new Tuple<int, Vector3>(2, mecResult.Center.To3D());
                }

                if (points.Count == 1)
                {
                    return new Tuple<int, Vector3>(1, mecResult.Center.To3D());
                }

                if (mecResult.Radius < Q.Width && points.Count == 2)
                {
                    return new Tuple<int, Vector3>(2, mecResult.Center.To3D());
                }

                float maxdist = -1;
                var maxdistindex = 1;
                for (var i = 1; i < points.Count; i++)
                {
                    var distance = Vector2.DistanceSquared(points[i], points[0]);
                    if (distance > maxdist || maxdist.CompareTo(-1) == 0)
                    {
                        maxdistindex = i;
                        maxdist = distance;
                    }
                }
                points.RemoveAt(maxdistindex);
            }

            return new Tuple<int, Vector3>(1, points[0].To3D());
        }

        static void Combo()
        {
            var target = TargetSelector.GetTarget(Q.Range + Q.Width, TargetSelector.DamageType.Magical);
            
            if (target == null)
            {
                return;
            }

            var useQ = Config.Item("UseQCombo").GetValue<bool>();
            var useW = Config.Item("UseWCombo").GetValue<bool>();
            var useE = Config.Item("UseECombo").GetValue<bool>();
            var useR = Config.Item("UseRCombo").GetValue<bool>();

            var minRTargets = Config.Item("UseRNCombo").GetValue<StringList>().SelectedIndex + 1;

            if (useW && WIsReady)
            {
                CastW(1);
            }

            if (Utility.CountEnemiesInRange((int)(Q.Range + R.Width)) <= 1)
            {
                if(useR && GetComboDamage(target) > target.Health && RIsReady)
                {
                    CastR(minRTargets, true);
                }

                if(useQ && QIsReady)
                {
                    CastQ(target);
                }

                if(useE && EIsReady)
                {
                    foreach (var ally in HeroManager.Allies.Where(h => h.IsValidTarget(E.Range, false)))
                    {
                        if (ally.Position.CountEnemiesInRange(300) >= 1)
                        {
                            E.CastOnUnit(ally, true);
                        }

                        CastE(ally, 1);
                    }
                }
            }
            else
            {
                if (useR && RIsReady)
                {
                    if (BallManager.BallPosition.CountEnemiesInRange(800) > 1)
                    {
                        var rCheck = GetHits(R);
                        var pk = 0;
                        var k = 0;
                        if (rCheck.Item1 >= 2)
                        {
                            foreach (var hero in rCheck.Item2)
                            {
                                var comboDamage = GetComboDamage(hero);
                                if ((hero.Health - comboDamage) < 0.4 * hero.MaxHealth || comboDamage >= 0.4 * hero.MaxHealth)
                                {
                                    pk++;
                                }

                                if ((hero.Health - comboDamage) < 0)
                                {
                                    k++;
                                }
                            }

                            if (rCheck.Item1 >= BallManager.BallPosition.CountEnemiesInRange(800) || pk >= 2 ||
                                k >= 1)
                            {
                                if (rCheck.Item1 >= minRTargets)
                                {
                                    R.Cast(Player.ServerPosition, true);
                                }
                            }
                        }
                    }

                    else if (GetComboDamage(target) > target.Health)
                    {
                        CastR(minRTargets, true);
                    }
                }

                if(useQ && QIsReady)
                {
                    var qLoc = GetBestQLocation(target);
                    if(qLoc.Item1 > 1)
                    {
                        Q.Cast(qLoc.Item2, true);
                    }
                    else
                    {
                        CastQ(target);
                    }
                }

                if(useE && EIsReady)
                {
                    if (BallManager.BallPosition.CountEnemiesInRange(800) <= 2)
                    {
                        CastE(Player, 1);
                    }
                    else
                    {
                        CastE(Player, 2);
                    }

                    foreach (var ally in HeroManager.Allies.Where(h => h.IsValidTarget(E.Range, false)))
                    {
                        if (ally.Position.CountEnemiesInRange(300) >= 2)
                        {
                            E.CastOnUnit(ally, true);
                        }
                    }
                }
            }
        }

        static void Harass()
        {
            if (Player.ManaPercent < Config.Item("HarassManaCheck").GetValue<Slider>().Value)
            	return;
           	
            var target = TargetSelector.GetTarget(Q.Range, TargetSelector.DamageType.Magical);
            if (target != null)
            {
                if (Config.Item("UseQHarass").GetValue<bool>() && QIsReady)
                {
                    CastQ(target);
                    return;
                }

                if (Config.Item("UseWHarass").GetValue<bool>() && WIsReady)
                {
                    CastW(1);
                }
            }
        }

        static void Game_OnGameUpdate(EventArgs args)
        {
            QIsReady = (Player.Spellbook.CanUseSpell(Q.Slot) == SpellState.Ready || Player.Spellbook.CanUseSpell(Q.Slot) == SpellState.Surpressed);
            WIsReady = (Player.Spellbook.CanUseSpell(W.Slot) == SpellState.Ready || Player.Spellbook.CanUseSpell(W.Slot) == SpellState.Surpressed);
            EIsReady = (Player.Spellbook.CanUseSpell(E.Slot) == SpellState.Ready || Player.Spellbook.CanUseSpell(E.Slot) == SpellState.Surpressed);
            RIsReady = (Player.Spellbook.CanUseSpell(R.Slot) == SpellState.Ready || Player.Spellbook.CanUseSpell(R.Slot) == SpellState.Surpressed);

            if (Player.IsDead)
            {
                return;
            }

            if (BallManager.BallPosition == Vector3.Zero)
            {
                return;
            }

            Q.From = BallManager.BallPosition;
            Q.RangeCheckFrom = Player.ServerPosition;
            W.From = BallManager.BallPosition;
            W.RangeCheckFrom = BallManager.BallPosition;
            E.From = BallManager.BallPosition;
            R.From = BallManager.BallPosition;
            R.RangeCheckFrom = BallManager.BallPosition;

            var autoWminTargets = Config.Item("AutoW").GetValue<StringList>().SelectedIndex;
            if (autoWminTargets > 0)
            {
                CastW(autoWminTargets);
            }

            var autoRminTargets = Config.Item("AutoR").GetValue<StringList>().SelectedIndex;
            if (autoRminTargets > 0)
            {
                CastR(autoRminTargets);
            }

            if (Config.Item("ComboActive").GetValue<KeyBind>().Active)
            {
                Combo();
            }
            else
            {
                if (Config.Item("HarassActive").GetValue<KeyBind>().Active ||
                    (Config.Item("HarassActiveT").GetValue<KeyBind>().Active && !Player.HasBuff("Recall")))
                {
                    Harass();
                }
                    

                var lc = Config.Item("LaneClearActive").GetValue<KeyBind>().Active;
                if (lc || Config.Item("FreezeActive").GetValue<KeyBind>().Active)
                {
                    Farm(lc && (Player.Mana * 100 / Player.MaxMana >= Config.Item("LaneClearManaCheck").GetValue<Slider>().Value));
                }
                    
                if (Config.Item("JungleFarmActive").GetValue<KeyBind>().Active)
                {
                    JungleFarm();
                }
            }
        }

        public static float GetComboDamage(Obj_AI_Hero target)
        {
            var result = 0f;
            if(QIsReady)
            {
                result += 2 * Q.GetDamage(target);
            }

            if(WIsReady)
            {
                result += W.GetDamage(target);
            }

            if(RIsReady)
            {
                result += R.GetDamage(target);
            }

            result += 2 * (float) Player.GetAutoAttackDamage(target);

            return result;
        }

        public static Tuple<int, List<Obj_AI_Hero>> GetHits(Spell spell)
        {
            var hits = new List<Obj_AI_Hero>();
            var range = spell.Range * spell.Range;
            foreach (var enemy in HeroManager.Enemies.Where(h => h.IsValidTarget() && BallManager.BallPosition.Distance(h.ServerPosition, true) < range))
	        {
                if (spell.WillHit(enemy, BallManager.BallPosition) && BallManager.BallPosition.Distance(enemy.ServerPosition, true) < spell.Width * spell.Width)
                {
                    hits.Add(enemy);
                }
	        }
            return new Tuple<int,List<Obj_AI_Hero>>(hits.Count, hits);
        }

        public static Tuple<int, List<Obj_AI_Hero>> GetEHits(Vector3 to)
        {
            var hits = new List<Obj_AI_Hero>();
            var oldERange = E.Range;
            E.Range = 10000; //avoid the range check
            foreach (var enemy in HeroManager.Enemies.Where(h => h.IsValidTarget(2000)))
            {
                if (E.WillHit(enemy, to))
                {
                    hits.Add(enemy);
                }
            }
            E.Range = oldERange;
            return new Tuple<int, List<Obj_AI_Hero>>(hits.Count, hits);
        }

        public static bool CastQ(Obj_AI_Hero target)
        {
            var qPrediction = Q.GetPrediction(target);

            if(qPrediction.Hitchance < HitChance.VeryHigh)
            {
                return false;
            }

            if(EIsReady)
            {
                var directTravelTime = BallManager.BallPosition.Distance(qPrediction.CastPosition) / Q.Speed;
                var bestEQTravelTime = float.MaxValue;

                Obj_AI_Hero eqTarget = null;

                foreach (var ally in HeroManager.Allies.Where(h => h.IsValidTarget(E.Range, false)))
                {
                    var t = BallManager.BallPosition.Distance(ally.ServerPosition) / E.Speed + ally.Distance(qPrediction.CastPosition) / Q.Speed;
                    if(t < bestEQTravelTime)
                    {
                        eqTarget = ally;
                        bestEQTravelTime = t;
                    }
                }

                if (eqTarget != null && bestEQTravelTime < directTravelTime * 1.3f && (BallManager.BallPosition.Distance(eqTarget.ServerPosition, true) > 10000))
                {
                    E.CastOnUnit(eqTarget, true);
                    return true;
                }
            }
            
            if (!target.IsFacing(Player) && target.Path.Count() >= 1) // target is running
            {
                var targetBehind = Q.GetPrediction(target).CastPosition +
                                   Vector3.Normalize(target.ServerPosition - BallManager.BallPosition) * target.MoveSpeed / 2;
                Q.Cast(targetBehind, true);
                return true;
            }
            
            Q.Cast(qPrediction.CastPosition, true);
            return true;
        }

        public static bool CastW(int minTargets)
        {
            var hits = GetHits(W);
            if(hits.Item1 >= minTargets)
            {
                W.Cast(Player.ServerPosition, true);
                return true;
            }
            return false;
        }

        public static bool CastE(Obj_AI_Hero target, int minTargets)
        {
            if (GetEHits(target.ServerPosition).Item1 >= minTargets)
            {
                E.CastOnUnit(target, true);
                return true;
            }
            return false;
        }

        public static bool CastR(int minTargets, bool prioriy = false)
        {
            if (GetHits(R).Item1 >= minTargets || prioriy && GetHits(R)
                    .Item2.Any(
                        hero =>
                            Config.Item("TargetSelector" + hero.ChampionName + "Priority") != null && Config.Item("TargetSelector" + hero.ChampionName + "Priority").GetValue<Slider>().Value >=
                            Config.Item("UseRImportant").GetValue<Slider>().Value))
            {
                R.Cast(Player.ServerPosition, true);
                return true;
            }

            return false;
        }

        public static int GetNumberOfMinionsHitByE(Obj_AI_Hero target)
        {
            var minions = MinionManager.GetMinions(BallManager.BallPosition, 2000);
            return E.CountHits(minions, target.ServerPosition);
        }


        private static void Drawing_OnDraw(EventArgs args)
        {
            var qCircle = Config.SubMenu("Drawings").Item("QRange").GetValue<Circle>();
            if(qCircle.Active)
            {
                Render.Circle.DrawCircle(Player.Position, Q.Range, qCircle.Color);
            }

            var wCircle = Config.SubMenu("Drawings").Item("WRange").GetValue<Circle>();
            if (wCircle.Active)
            {
                Render.Circle.DrawCircle(BallManager.BallPosition, W.Range, wCircle.Color);
            }

            var eCircle = Config.SubMenu("Drawings").Item("ERange").GetValue<Circle>();
            if (eCircle.Active)
            {
                Render.Circle.DrawCircle(Player.Position, E.Range, eCircle.Color);
            }

            var rCircle = Config.SubMenu("Drawings").Item("RRange").GetValue<Circle>();
            if (rCircle.Active)
            {
                Render.Circle.DrawCircle(BallManager.BallPosition, R.Range, rCircle.Color);
            }

            var q2Circle = Config.SubMenu("Drawings").Item("QOnBallRange").GetValue<Circle>();
            if (q2Circle.Active)
            {
                Render.Circle.DrawCircle(BallManager.BallPosition, Q.Width, q2Circle.Color, 5, true);
            }
        }

    }
}
