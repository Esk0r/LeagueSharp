#region

using System;
using System.Collections.Generic;
using System.Linq;
using LeagueSharp;
using LeagueSharp.Common;
using SharpDX;
using Color = System.Drawing.Color;

#endregion

namespace Syndra
{
    internal class Program
    {
        public const string ChampionName = "Syndra";

        //Orbwalker instance
        public static Orbwalking.Orbwalker Orbwalker;

        //Spells
        public static List<Spell> SpellList = new List<Spell>();

        public static Spell Q;
        public static Spell W;
        public static Spell E;
        public static Spell EQ;
        public static Spell R;

        public static Items.Item DFG;

        //Menu
        public static Menu Config;
        private static int QEComboT;
        private static int WEComboT;
        private static int FarmscsT;

        private static Obj_AI_Hero Player;

        private static void Main(string[] args)
        {
            CustomEvents.Game.OnGameLoad += Game_OnGameLoad;
        }

        private static void Game_OnGameLoad(EventArgs args)
        {
            Player = ObjectManager.Player;

            if (Player.BaseSkinName != ChampionName) return;

            //Create the spells
            Q = new Spell(SpellSlot.Q, 790);
            W = new Spell(SpellSlot.W, 925);
            E = new Spell(SpellSlot.E, 700);
            R = new Spell(SpellSlot.R, 675);
            EQ = new Spell(SpellSlot.Q, Q.Range + 500);

            DFG = new Items.Item(3128, 750);

            Q.SetSkillshot(0.6f, 125f, float.MaxValue, false, Prediction.SkillshotType.SkillshotCircle);
            W.SetSkillshot(0.25f, 125f, 1450f, false, Prediction.SkillshotType.SkillshotCircle);
            E.SetSkillshot(0.25f, (float)(45 * 0.5), 2500f, true, Prediction.SkillshotType.SkillshotCircle);
            EQ.SetSkillshot(float.MaxValue, 60f, 1600f, true, Prediction.SkillshotType.SkillshotCircle);

            SpellList.Add(Q);
            SpellList.Add(W);
            SpellList.Add(E);
            SpellList.Add(R);

            //Create the menu
            Config = new Menu(ChampionName, ChampionName, true);

            //Orbwalker submenu
            Config.AddSubMenu(new Menu("Orbwalking", "Orbwalking"));

            //Add the target selector to the menu as submenu.
            var targetSelectorMenu = new Menu("Target Selector", "Target Selector");
            SimpleTs.AddToMenu(targetSelectorMenu);
            Config.AddSubMenu(targetSelectorMenu);

            //Load the orbwalker and add it to the menu as submenu.
            Orbwalker = new Orbwalking.Orbwalker(Config.SubMenu("Orbwalking"));

            //Combo menu:
            Config.AddSubMenu(new Menu("Combo", "Combo"));
            Config.SubMenu("Combo").AddItem(new MenuItem("UseQCombo", "Use Q").SetValue(true));
            Config.SubMenu("Combo").AddItem(new MenuItem("UseWCombo", "Use W").SetValue(true));
            Config.SubMenu("Combo").AddItem(new MenuItem("UseECombo", "Use E").SetValue(true));
            Config.SubMenu("Combo").AddItem(new MenuItem("UseQECombo", "Use QE").SetValue(true));
            Config.SubMenu("Combo").AddItem(new MenuItem("UseRCombo", "Use R").SetValue(true));
            Config.SubMenu("Combo")
                .AddItem(
                    new MenuItem("ComboActive", "Combo!").SetValue(
                        new KeyBind(Config.Item("Orbwalk").GetValue<KeyBind>().Key, KeyBindType.Press)));

            //Harass menu:
            Config.AddSubMenu(new Menu("Harass", "Harass"));
            Config.SubMenu("Harass").AddItem(new MenuItem("UseQHarass", "Use Q").SetValue(true));
            Config.SubMenu("Harass").AddItem(new MenuItem("UseWHarass", "Use W").SetValue(false));
            Config.SubMenu("Harass").AddItem(new MenuItem("UseEHarass", "Use E").SetValue(false));
            Config.SubMenu("Harass").AddItem(new MenuItem("UseQEHarass", "Use QE").SetValue(false));
            Config.SubMenu("Harass")
                .AddItem(
                    new MenuItem("HarassActive", "Harass!").SetValue(
                        new KeyBind(Config.Item("Farm").GetValue<KeyBind>().Key, KeyBindType.Press)));
            Config.SubMenu("Harass")
                .AddItem(
                    new MenuItem("HarassActiveT", "Harass (toggle)!").SetValue(new KeyBind("Y".ToCharArray()[0],
                        KeyBindType.Toggle)));

            //Farming menu:
            Config.AddSubMenu(new Menu("Farm", "Farm"));
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
                        new StringList(new[] { "Freeze", "LaneClear", "Both", "No" }, 3)));
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

            //Misc
            Config.AddSubMenu(new Menu("Misc", "Misc"));
            Config.SubMenu("Misc").AddItem(new MenuItem("InterruptSpells", "Interrupt spells").SetValue(true));
            Config.SubMenu("Misc")
                .AddItem(
                    new MenuItem("CastQE", "QE closest to cursor").SetValue(new KeyBind("T".ToCharArray()[0],
                        KeyBindType.Press)));
            Config.SubMenu("Misc").AddSubMenu(new Menu("Dont use R on", "DontUlt"));

            foreach (var enemy in ObjectManager.Get<Obj_AI_Hero>().Where(enemy => enemy.Team != Player.Team))
                Config.SubMenu("Misc")
                    .SubMenu("DontUlt")
                    .AddItem(new MenuItem("DontUlt" + enemy.BaseSkinName, enemy.BaseSkinName).SetValue(false));

            //Drawings menu:
            Config.AddSubMenu(new Menu("Drawings", "Drawings"));
            Config.SubMenu("Drawings")
                .AddItem(new MenuItem("QRange", "Q range").SetValue(new Circle(false, Color.FromArgb(100, 255, 0, 255))));
            Config.SubMenu("Drawings")
                .AddItem(new MenuItem("WRange", "W range").SetValue(new Circle(true, Color.FromArgb(100, 255, 0, 255))));
            Config.SubMenu("Drawings")
                .AddItem(new MenuItem("ERange", "E range").SetValue(new Circle(false, Color.FromArgb(100, 255, 0, 255))));
            Config.SubMenu("Drawings")
                .AddItem(new MenuItem("RRange", "R range").SetValue(new Circle(false, Color.FromArgb(100, 255, 0, 255))));
            Config.SubMenu("Drawings")
                .AddItem(new MenuItem("QERange", "QE range").SetValue(new Circle(true, Color.FromArgb(100, 255, 0, 255))));
            Config.AddToMainMenu();

            //Add the events we are going to use:
            Game.OnGameUpdate += Game_OnGameUpdate;
            Obj_AI_Base.OnProcessSpellCast += Obj_AI_Hero_OnProcessSpellCast;
            Drawing.OnDraw += Drawing_OnDraw;
            Interrupter.OnPosibleToInterrupt += Interrupter_OnPosibleToInterrupt;

            Game.PrintChat(ChampionName + " Loaded!");
        }

        private static void Interrupter_OnPosibleToInterrupt(Obj_AI_Base unit, InterruptableSpell spell)
        {
            if (!Config.Item("InterruptSpells").GetValue<bool>()) return;
          
            if (Player.Distance(unit) < E.Range && E.IsReady())
            {
                Q.Cast(unit.ServerPosition);
                E.Cast(unit.ServerPosition);
            }
            else if (Player.Distance(unit) < EQ.Range && E.IsReady() && Q.IsReady())
            {
                UseQE(unit);
            }
        }

        private static void Combo()
        {
            Orbwalker.SetAttacks(!(Q.IsReady() || W.IsReady()));
            UseSpells(Config.Item("UseQCombo").GetValue<bool>(), Config.Item("UseWCombo").GetValue<bool>(),
                Config.Item("UseECombo").GetValue<bool>(), Config.Item("UseRCombo").GetValue<bool>(),
                Config.Item("UseQECombo").GetValue<bool>());
        }

        private static void Harass()
        {
            UseSpells(Config.Item("UseQHarass").GetValue<bool>(), Config.Item("UseWHarass").GetValue<bool>(),
                Config.Item("UseEHarass").GetValue<bool>(), false, Config.Item("UseQEHarass").GetValue<bool>());
        }

        private static void UseE(Obj_AI_Base enemy)
        {
            foreach (var orb in OrbManager.GetOrbs(true))
                if (Player.Distance(orb) < E.Range + 100)
                {
                    var startPoint = orb.To2D().Extend(Player.ServerPosition.To2D(), 100);
                    var endPoint = Player.ServerPosition.To2D()
                        .Extend(orb.To2D(), Player.Distance(orb) > 200 ? 1300 : 1000);
                    EQ.Delay = E.Delay + Player.Distance(orb) / E.Speed;
                    EQ.From = orb;
                    var enemyPred = EQ.GetPrediction(enemy);
                    if (enemyPred.HitChance >= Prediction.HitChance.HighHitchance &&
                        enemyPred.Position.To2D().Distance(startPoint, endPoint, false) <
                        EQ.Width + enemy.BoundingRadius)
                    {
                        E.Cast(orb, true);
                        W.LastCastAttemptT = Environment.TickCount;
                        return;
                    }
                }
        }

        private static void UseQE(Obj_AI_Base enemy)
        {
            EQ.Delay = E.Delay + Q.Range / E.Speed;
            EQ.From = Player.ServerPosition.To2D().Extend(enemy.ServerPosition.To2D(), Q.Range).To3D();
            var prediction = EQ.GetPrediction(enemy);
            if (prediction.HitChance >= Prediction.HitChance.HighHitchance)
            {
                Q.Cast(Player.ServerPosition.To2D().Extend(prediction.CastPosition.To2D(), Q.Range - 100));
                QEComboT = Environment.TickCount;
                W.LastCastAttemptT = Environment.TickCount;
            }
        }

        private static Vector3 GetGrabableObjectPos(bool onlyOrbs)
        {
            if (!onlyOrbs)
                foreach (var minion in ObjectManager.Get<Obj_AI_Minion>().Where(minion => minion.IsValidTarget(W.Range))
                    )
                    return minion.ServerPosition;

            return OrbManager.GetOrbToGrab((int)W.Range);
        }

        private static float GetComboDamage(Obj_AI_Base enemy)
        {
            var damage = 0d;

            damage += DamageLib.getDmg(enemy, DamageLib.SpellType.Q);

            if (DFG.IsReady())
                damage += DamageLib.getDmg(enemy, DamageLib.SpellType.DFG) / 1.2;

            if (W.IsReady())
                damage += DamageLib.getDmg(enemy, DamageLib.SpellType.W);

            if (E.IsReady())
                damage += DamageLib.getDmg(enemy, DamageLib.SpellType.E);

            var igniteSlot = Player.GetSpellSlot("SummonerDot");
            
            if(igniteSlot != SpellSlot.Unknown && Player.SummonerSpellbook.CanUseSpell(igniteSlot) == SpellState.Ready)
                damage += DamageLib.getDmg(enemy, DamageLib.SpellType.IGNITE);
                

            if (R.IsReady())
                damage += Player.Spellbook.GetSpell(SpellSlot.R).Ammo * DamageLib.getDmg(enemy, DamageLib.SpellType.R);

            return (float)damage * (DFG.IsReady() ? 1.2f : 1);
        }

        private static void UseSpells(bool useQ, bool useW, bool useE, bool useR, bool useQE)
        {
            var qTarget = SimpleTs.GetTarget(Q.Range + Q.Width, SimpleTs.DamageType.Magical);
            var wTarget = SimpleTs.GetTarget(W.Range + W.Width, SimpleTs.DamageType.Magical);
            var rTarget = SimpleTs.GetTarget(R.Range, SimpleTs.DamageType.Magical);
            var qeTarget = SimpleTs.GetTarget(EQ.Range, SimpleTs.DamageType.Magical);

            //Q
            if (qTarget != null && useQ)
                Q.Cast(qTarget, false, true);

            //E
            if (Environment.TickCount - W.LastCastAttemptT > Game.Ping + 150 && E.IsReady() && useE)
                foreach (var enemy in ObjectManager.Get<Obj_AI_Hero>())
                {
                    if (enemy.IsValidTarget(EQ.Range))
                        UseE(enemy);
                }

            //W
            if (useW)
                if (Player.Spellbook.GetSpell(SpellSlot.W).ToggleState == 1 && W.IsReady() && qeTarget != null)
                {
                    //WObject
                    var gObjectPos = GetGrabableObjectPos(wTarget == null);

                    if (gObjectPos.To2D().IsValid() && Environment.TickCount - W.LastCastAttemptT > Game.Ping + 400)
                    {
                        W.Cast(gObjectPos);
                        W.LastCastAttemptT = Environment.TickCount;
                    }
                }
                else if (wTarget != null && Player.Spellbook.GetSpell(SpellSlot.W).ToggleState != 1 && W.IsReady() &&
                         Environment.TickCount - W.LastCastAttemptT > Game.Ping + 150)
                {
                    if (OrbManager.WObject(false) != null)
                    {
                        W.From = OrbManager.WObject(false).ServerPosition;
                        W.Cast(wTarget, false, true);
                    }
                }
            if (rTarget != null)
                useR = (Config.Item("DontUlt" + rTarget.BaseSkinName) != null &&
                        Config.Item("DontUlt" + rTarget.BaseSkinName).GetValue<bool>() == false)
                    ? useR
                    : false;

            //DFG (and ult if ready)
            if (rTarget != null && useR && GetComboDamage(rTarget) > rTarget.Health && DFG.IsReady())
            {
                DFG.Cast(rTarget);
                if(R.IsReady())
                {       
                    R.Cast(rTarget);
                }
                
            }

            //R
            if (rTarget != null && useR && R.IsReady() && !Q.IsReady() && !DFG.IsReady())
            {
                if (GetComboDamage(rTarget) >
                    rTarget.Health)
                {
                    R.Cast(rTarget);
                }
            }

            //QE
            if (wTarget == null && qeTarget != null && Q.IsReady() && E.IsReady() && useQE)
                UseQE(qeTarget);

            //WE
            if (wTarget == null && qeTarget != null && E.IsReady() && useE && OrbManager.WObject(true) != null)
            {
                EQ.Delay = E.Delay + W.Range / E.Speed;
                EQ.From = Player.ServerPosition.To2D().Extend(qeTarget.ServerPosition.To2D(), W.Range).To3D();
                var prediction = EQ.GetPrediction(qeTarget);
                if (prediction.HitChance >= Prediction.HitChance.HighHitchance)
                {
                    W.Cast(Player.ServerPosition.To2D().Extend(prediction.CastPosition.To2D(), W.Range - 100));
                    WEComboT = Environment.TickCount;
                }
            }
        }

        private static void Obj_AI_Hero_OnProcessSpellCast(Obj_AI_Base sender, GameObjectProcessSpellCastEventArgs args)
        {
            if (sender.IsMe && Environment.TickCount - QEComboT < 500 &&
                (args.SData.Name == "SyndraQ"))
            {
                W.LastCastAttemptT = Environment.TickCount + 400;
                E.Cast(args.End, true);
            }

            if (sender.IsMe && Environment.TickCount - WEComboT < 500 &&
                (args.SData.Name == "SyndraW" || args.SData.Name == "syndraw2"))
            {
                W.LastCastAttemptT = Environment.TickCount + 400;
                E.Cast(args.End, true);
            }
        }

        private static void Farm(bool laneClear)
        {
            if (!Orbwalking.CanMove(40)) return;
            if (Environment.TickCount - FarmscsT < 500) return;

            var rangedMinionsQ = MinionManager.GetMinions(ObjectManager.Player.ServerPosition, Q.Range + Q.Width + 30,
                MinionTypes.Ranged);
            var allMinionsQ = MinionManager.GetMinions(ObjectManager.Player.ServerPosition, Q.Range + Q.Width + 30,
                MinionTypes.All);
            var rangedMinionsW = MinionManager.GetMinions(ObjectManager.Player.ServerPosition, W.Range + W.Width + 30,
                MinionTypes.Ranged);
            var allMinionsW = MinionManager.GetMinions(ObjectManager.Player.ServerPosition, W.Range + W.Width + 30,
                MinionTypes.All);

            var useQi = Config.Item("UseQFarm").GetValue<StringList>().SelectedIndex;
            var useWi = Config.Item("UseWFarm").GetValue<StringList>().SelectedIndex;
            var useEi = Config.Item("UseEFarm").GetValue<StringList>().SelectedIndex;
            var useQ = (laneClear && (useQi == 1 || useQi == 2)) || (!laneClear && (useQi == 0 || useQi == 2));
            var useW = (laneClear && (useWi == 1 || useWi == 2)) || (!laneClear && (useWi == 0 || useWi == 2));
            var useE = (laneClear && (useEi == 1 || useEi == 2)) || (!laneClear && (useEi == 0 || useEi == 2));

            if (useQ && Q.IsReady())
                if (laneClear)
                {
                    var fl1 = Q.GetCircularFarmLocation(rangedMinionsQ, Q.Width);
                    var fl2 = Q.GetCircularFarmLocation(allMinionsQ, Q.Width);
                    if (Player.Spellbook.GetSpell(SpellSlot.W).ToggleState == 1)
                        if (fl1.MinionsHit >= 3)
                        {
                            FarmscsT = Environment.TickCount;
                            Q.Cast(fl1.Position);
                        }

                        else if (fl2.MinionsHit >= 3 || allMinionsQ.Count == 1)
                        {
                            FarmscsT = Environment.TickCount;
                            Q.Cast(fl2.Position);
                        }
                }
                else
                    foreach (var minion in allMinionsQ)
                        if (!Orbwalking.InAutoAttackRange(minion) &&
                            minion.Health < 0.75 * DamageLib.getDmg(minion, DamageLib.SpellType.Q))
                            Q.Cast(minion);

            if (useW && W.IsReady())
            {
                if (Environment.TickCount - FarmscsT < 1000) return;
                if (laneClear)
                {
                    if (Player.Spellbook.GetSpell(SpellSlot.W).ToggleState == 1)
                    {
                        //WObject
                        var gObjectPos = GetGrabableObjectPos(false);

                        if (gObjectPos.To2D().IsValid() && Environment.TickCount - W.LastCastAttemptT > Game.Ping + 400)
                        {
                            W.Cast(gObjectPos);
                            FarmscsT = Environment.TickCount;
                        }
                    }
                    else if (Player.Spellbook.GetSpell(SpellSlot.W).ToggleState != 1 &&
                             Environment.TickCount - W.LastCastAttemptT > Game.Ping + 150)
                    {
                        var fl1 = Q.GetCircularFarmLocation(rangedMinionsW, W.Width);
                        var fl2 = Q.GetCircularFarmLocation(allMinionsW, W.Width);

                        if (fl1.MinionsHit >= 3)
                        {
                            FarmscsT = Environment.TickCount;
                            W.Cast(fl1.Position);
                        }

                        else if (fl2.MinionsHit >= 1)
                        {
                            FarmscsT = Environment.TickCount;
                            W.Cast(fl2.Position);
                        }
                    }
                }
            }
        }

        private static void JungleFarm()
        {
            if (Environment.TickCount - FarmscsT < 500) return;

            var useQ = Config.Item("UseQJFarm").GetValue<bool>();
            var useW = Config.Item("UseWJFarm").GetValue<bool>();
            var useE = Config.Item("UseEJFarm").GetValue<bool>();

            var mobs = MinionManager.GetMinions(ObjectManager.Player.ServerPosition, W.Range, MinionTypes.All,
                MinionTeam.Neutral, MinionOrderTypes.MaxHealth);

            if (mobs.Count > 0)
            {
                var mob = mobs[0];

                if (Q.IsReady() && useQ)
                {
                    FarmscsT = Environment.TickCount;
                    Q.Cast(mob);
                }


                if (W.IsReady() && useW && GetGrabableObjectPos(true).To2D().IsValid())
                {
                    FarmscsT = Environment.TickCount;
                    W.Cast(GetGrabableObjectPos(true));
                }

                if (useE && E.IsReady())
                {
                    FarmscsT = Environment.TickCount;
                    E.Cast(mob);
                }
            }
        }


        private static void Game_OnGameUpdate(EventArgs args)
        {
            if (Player.IsDead) return;
            Orbwalker.SetAttacks(true);
            //Update the R range
            R.Range = R.Level == 3 ? 750 : 675;

            if (Config.Item("CastQE").GetValue<KeyBind>().Active && E.IsReady() && Q.IsReady())
                foreach (var enemy in ObjectManager.Get<Obj_AI_Hero>())
                    if (enemy.IsValidTarget(EQ.Range) && Game.CursorPos.Distance(enemy.ServerPosition) < 300)
                        UseQE(enemy);

            if (Config.Item("ComboActive").GetValue<KeyBind>().Active)
            {
                Combo();
            }
            else
            {
                if (Config.Item("HarassActive").GetValue<KeyBind>().Active ||
                    Config.Item("HarassActiveT").GetValue<KeyBind>().Active)
                    Harass();

                var lc = Config.Item("LaneClearActive").GetValue<KeyBind>().Active;
                if (lc || Config.Item("FreezeActive").GetValue<KeyBind>().Active)
                    Farm(lc);

                if (Config.Item("JungleFarmActive").GetValue<KeyBind>().Active)
                    JungleFarm();
            }
        }

        private static void Drawing_OnDraw(EventArgs args)
        {
            //Draw the ranges of the spells.
            var menuItem = Config.Item("QERange").GetValue<Circle>();
            if (menuItem.Active) Utility.DrawCircle(Player.Position, EQ.Range, menuItem.Color);

            foreach (var spell in SpellList)
            {
                menuItem = Config.Item(spell.Slot + "Range").GetValue<Circle>();
                if (menuItem.Active)
                    Utility.DrawCircle(Player.Position, spell.Range, menuItem.Color);
            }
        }
    }
}
