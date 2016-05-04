#region

using System;
using System.Collections.Generic;
using System.Linq;
using LeagueSharp;
using LeagueSharp.Common;
using SharpDX;

#endregion

namespace Syndra
{
    using System.Drawing;

    using Color = SharpDX.Color;

    internal static class Program
    {
        public const string ChampionName = "Syndra";

        //Orbwalker instance
        public static Orbwalking.Orbwalker Orbwalker;

        //Spells
        public static List<Spell> SpellList = new List<Spell>();

        public static Spell Q;

        public static Spell W;

        public static Spell E;

        public static Spell Eq;

        public static Spell R;

        public static SpellSlot IgniteSlot;

        public static AssassinManager AssassinManager;

        //Menu
        public static Menu Config, DrawMenu;

        private static int qeComboT;

        private static int weComboT;

        public static Obj_AI_Hero Player;

        // ReSharper disable once UnusedParameter.Local
        private static void Main(string[] args)
        {
            CustomEvents.Game.OnGameLoad += Game_OnGameLoad;
        }

        private static void Game_OnGameLoad(EventArgs args)
        {
            Player = ObjectManager.Player;

            if (Player.CharData.BaseSkinName != ChampionName) return;

            //Create the spells
            Q = new Spell(SpellSlot.Q, 790);
            W = new Spell(SpellSlot.W, 925);
            E = new Spell(SpellSlot.E, 700);
            R = new Spell(SpellSlot.R, 675);
            Eq = new Spell(SpellSlot.Q, Q.Range + 500);

            IgniteSlot = Player.GetSpellSlot("SummonerDot");

            Q.SetSkillshot(0.6f, 125f, float.MaxValue, false, SkillshotType.SkillshotCircle);
            W.SetSkillshot(0.25f, 140f, 1600f, false, SkillshotType.SkillshotCircle);
            E.SetSkillshot(0.25f, (float)(45 * 0.5), 2500f, false, SkillshotType.SkillshotCircle);
            Eq.SetSkillshot(float.MaxValue, 55f, 2000f, false, SkillshotType.SkillshotCircle);

            SpellList.Add(Q);
            SpellList.Add(W);
            SpellList.Add(E);
            SpellList.Add(R);

            //Create the menu
            Config = new Menu(ChampionName, ChampionName, true).SetFontStyle(FontStyle.Regular, Color.GreenYellow);

            //Orbwalker submenu
            Config.AddSubMenu(new Menu("Orbwalking", "Orbwalking"));

            //AssassinManager = new AssassinManager();
            //AssassinManager.Initialize();

            //Initialize the orbwalker and add it to the menu as submenu.
            Orbwalker = new Orbwalking.Orbwalker(Config.SubMenu("Orbwalking"));

            var menuKeys = new Menu("Keys", "Keys").SetFontStyle(FontStyle.Regular, Color.Aqua);
            {
                menuKeys.AddItem(
                    new MenuItem("Key.Combo", "Combo!").SetValue(
                        new KeyBind(Config.Item("Orbwalk").GetValue<KeyBind>().Key, KeyBindType.Press)))
                    .SetFontStyle(FontStyle.Regular, Color.GreenYellow);
                menuKeys.AddItem(
                    new MenuItem("Key.Harass", "Harass!").SetValue(
                        new KeyBind(Config.Item("Farm").GetValue<KeyBind>().Key, KeyBindType.Press)))
                    .SetFontStyle(FontStyle.Regular, Color.Coral);
                menuKeys.AddItem(
                    new MenuItem("Key.HarassT", "Harass (toggle)!").SetValue(
                        new KeyBind("Y".ToCharArray()[0], KeyBindType.Toggle)))
                    .SetFontStyle(FontStyle.Regular, Color.Coral)
                    .Permashow(true, "Syndra | Toggle Harass", Color.Aqua);
                menuKeys.AddItem(
                    new MenuItem("Key.Lane", "Lane Clear!").SetValue(
                        new KeyBind(Config.Item("LaneClear").GetValue<KeyBind>().Key, KeyBindType.Press)))
                    .SetFontStyle(FontStyle.Regular, Color.DarkKhaki);
                menuKeys.AddItem(
                    new MenuItem("Key.Jungle", "Jungle Farm!").SetValue(
                        new KeyBind(Config.Item("LaneClear").GetValue<KeyBind>().Key, KeyBindType.Press)))
                    .SetFontStyle(FontStyle.Regular, Color.DarkKhaki);
                menuKeys.AddItem(
                    new MenuItem("Key.InstantQE", "Instant Q-E to Enemy").SetValue(
                        new KeyBind("T".ToCharArray()[0], KeyBindType.Press)));
                Config.AddSubMenu(menuKeys);
            }

            var menuCombo = new Menu("Combo", "Combo");
            {
                menuCombo.AddItem(new MenuItem("UseQCombo", "Use Q").SetValue(true));
                menuCombo.AddItem(new MenuItem("UseWCombo", "Use W").SetValue(true));
                menuCombo.AddItem(new MenuItem("UseECombo", "Use E").SetValue(true));
                menuCombo.AddItem(new MenuItem("UseQECombo", "Use QE").SetValue(true));
                menuCombo.AddItem(new MenuItem("UseRCombo", "Use R").SetValue(true));
                menuCombo.AddItem(new MenuItem("UseIgniteCombo", "Use Ignite").SetValue(true));
                Config.AddSubMenu(menuCombo);
            }

            var menuHarass = new Menu("Harass", "Harass");
            {
                menuHarass.AddItem(new MenuItem("UseQHarass", "Use Q").SetValue(true));

                menuHarass.AddItem(new MenuItem("UseWHarass", "Use W").SetValue(false));
                menuHarass.AddItem(new MenuItem("UseEHarass", "Use E").SetValue(false));
                menuHarass.AddItem(new MenuItem("UseQEHarass", "Use QE").SetValue(false));
                menuHarass.AddItem(
                    new MenuItem("Harass.Mana", "Don't harass if mana < %").SetValue(new Slider(0)));
                Config.AddSubMenu(menuHarass);
            }

            var menuFarm = new Menu("Lane Farm", "Farm");
            {
                menuFarm.AddItem(new MenuItem("EnabledFarm", "Enable! (On/Off: Mouse Scroll)").SetValue(true))
                    .Permashow(true, "Syndra | Farm Mode Active", Color.Aqua);
                menuFarm.AddItem(
                    new MenuItem("UseQFarm", "Use Q").SetValue(
                        new StringList(new[] { "Freeze", "LaneClear", "Both", "No" }, 2)));
                menuFarm.AddItem(
                    new MenuItem("UseWFarm", "Use W").SetValue(
                        new StringList(new[] { "Freeze", "LaneClear", "Both", "No" }, 1)));
                menuFarm.AddItem(
                    new MenuItem("UseEFarm", "Use E").SetValue(
                        new StringList(new[] { "Freeze", "LaneClear", "Both", "No" }, 3)));
                menuFarm.AddItem(
                    new MenuItem("FreezeActive", "Freeze!").SetValue(
                        new KeyBind(Config.Item("Farm").GetValue<KeyBind>().Key, KeyBindType.Press)));
                menuFarm.AddItem(new MenuItem("Lane.Mana", "Don't harass if mana < %").SetValue(new Slider(0)));
                Config.AddSubMenu(menuFarm);
            }

            var menuJungle = new Menu("Jungle Farm", "JungleFarm");
            {
                menuJungle.AddItem(new MenuItem("UseQJFarm", "Use Q").SetValue(true));
                menuJungle.AddItem(new MenuItem("UseWJFarm", "Use W").SetValue(true));
                menuJungle.AddItem(new MenuItem("UseEJFarm", "Use E").SetValue(true));
                Config.AddSubMenu(menuJungle);
            }

            var menuMisc = new Menu("Misc", "Misc");
            {
                menuMisc.AddItem(new MenuItem("InterruptSpells", "Interrupt spells").SetValue(true));
                menuMisc.AddItem(
                    new MenuItem("CastQE", "QE closest to cursor").SetValue(
                        new KeyBind('T', KeyBindType.Press)));

                menuMisc.AddSubMenu(new Menu("Dont use R on", "DontUlt"));
                foreach (var enemy in ObjectManager.Get<Obj_AI_Hero>().Where(enemy => enemy.Team != Player.Team))
                    menuMisc.SubMenu("DontUlt")
                        .AddItem(
                            new MenuItem("DontUlt" + enemy.CharData.BaseSkinName, enemy.CharData.BaseSkinName).SetValue(
                                false));
                Config.AddSubMenu(menuMisc);
            }


            DrawMenu = new Menu("Drawings", "Drawings");
            {
                DrawMenu.AddItem(
                    new MenuItem("QRange", "Q range").SetValue(
                        new Circle(false, System.Drawing.Color.FromArgb(100, 255, 0, 255))));
                DrawMenu.AddItem(
                    new MenuItem("WRange", "W range").SetValue(
                        new Circle(true, System.Drawing.Color.FromArgb(100, 255, 0, 255))));
                DrawMenu.AddItem(
                    new MenuItem("ERange", "E range").SetValue(
                        new Circle(false, System.Drawing.Color.FromArgb(100, 255, 0, 255))));
                DrawMenu.AddItem(
                    new MenuItem("RRange", "R range").SetValue(
                        new Circle(false, System.Drawing.Color.FromArgb(100, 255, 0, 255))));
                DrawMenu.AddItem(
                    new MenuItem("QERange", "QE range").SetValue(
                        new Circle(true, System.Drawing.Color.FromArgb(100, 255, 0, 255))));

                var dmgAfterComboItem = new MenuItem("DamageAfterCombo", "Draw Damage After Combo").SetValue(true);
                Utility.HpBarDamageIndicator.DamageToUnit = GetComboDamage;
                Utility.HpBarDamageIndicator.Enabled = dmgAfterComboItem.GetValue<bool>();
                dmgAfterComboItem.ValueChanged += delegate(object sender, OnValueChangeEventArgs eventArgs)
                    {
                        Utility.HpBarDamageIndicator.Enabled = eventArgs.GetNewValue<bool>();
                    };

                DrawMenu.AddItem(dmgAfterComboItem);
                ManaBarIndicator.Initialize();
                Config.AddSubMenu(DrawMenu);
            }
            Config.AddToMainMenu();

            //Add the events we are going to use:
            Game.OnUpdate += Game_OnGameUpdate;
            Game.OnWndProc += Game_OnWndProc;
            Orbwalking.BeforeAttack += Orbwalking_BeforeAttack;

            Obj_AI_Base.OnProcessSpellCast += Obj_AI_Hero_OnProcessSpellCast;
            Interrupter2.OnInterruptableTarget += Interrupter2_OnInterruptableTarget;

            Drawing.OnDraw += Drawing_OnDraw;
            Game.PrintChat(ChampionName + " Loaded!");
        }

        private static void Game_OnWndProc(WndEventArgs args)
        {
            if (args.Msg != 0x20a) return;

            if (ObjectManager.Player.InShop() || ObjectManager.Player.InFountain()) return;

            Config.SubMenu("Farm")
                .Item("EnabledFarm")
                .SetValue(!Config.SubMenu("Farm").Item("EnabledFarm").GetValue<bool>());
        }

        private static void Interrupter2_OnInterruptableTarget(
            Obj_AI_Hero sender,
            Interrupter2.InterruptableTargetEventArgs args)
        {
            if (!Config.Item("InterruptSpells").GetValue<bool>()) return;

            if (Player.Distance(sender) < E.Range && E.IsReady())
            {
                Q.Cast(sender.ServerPosition);
                E.Cast(sender.ServerPosition);
            }
            else if (Player.Distance(sender) < Eq.Range && E.IsReady() && Q.IsReady())
            {
                UseQe(sender);
            }
        }

        // ReSharper disable once InconsistentNaming
        private static void Orbwalking_BeforeAttack(Orbwalking.BeforeAttackEventArgs args)
        {
            if (Config.Item("Key.Combo").GetValue<KeyBind>().Active)
            {
                args.Process = !(Q.IsReady() || W.IsReady());
            }
        }

        private static void InstantQe2Enemy()
        {
            ObjectManager.Player.IssueOrder(GameObjectOrder.MoveTo, Game.CursorPos);
            var t = TargetSelector.GetTarget(Eq.Range, TargetSelector.DamageType.Magical);
            if (t.IsValidTarget() && E.IsReady() && Q.IsReady())
            {
                UseQe(t);
            }
        }

        private static void Combo()
        {
            UseSpells(
                Config.Item("UseQCombo").GetValue<bool>(),
                Config.Item("UseWCombo").GetValue<bool>(),
                Config.Item("UseECombo").GetValue<bool>(),
                Config.Item("UseRCombo").GetValue<bool>(),
                Config.Item("UseQECombo").GetValue<bool>(),
                Config.Item("UseIgniteCombo").GetValue<bool>(),
                false);
        }

        private static void Harass()
        {
            if (Player.ManaPercent < Config.Item("Harass.Mana").GetValue<Slider>().Value)
            {
                return;
            }

            UseSpells(
                Config.Item("UseQHarass").GetValue<bool>(),
                Config.Item("UseWHarass").GetValue<bool>(),
                Config.Item("UseEHarass").GetValue<bool>(),
                false,
                Config.Item("UseQEHarass").GetValue<bool>(),
                false,
                true);
        }

        private static void UseE(Obj_AI_Base enemy)
        {
            foreach (var orb in OrbManager.GetOrbs(true))
                if (Player.Distance(orb) < E.Range + 100)
                {
                    var startPoint = orb.To2D().Extend(Player.ServerPosition.To2D(), 100);
                    var endPoint = Player.ServerPosition.To2D()
                        .Extend(orb.To2D(), Player.Distance(orb) > 200 ? 1300 : 1000);
                    Eq.Delay = E.Delay + Player.Distance(orb) / E.Speed;
                    Eq.From = orb;
                    var enemyPred = Eq.GetPrediction(enemy);
                    if (enemyPred.Hitchance >= HitChance.High
                        && enemyPred.UnitPosition.To2D().Distance(startPoint, endPoint, false)
                        < Eq.Width + enemy.BoundingRadius)
                    {
                        E.Cast(orb, true);
                        W.LastCastAttemptT = Utils.TickCount;
                        return;
                    }
                }
        }

        private static void UseQe(Obj_AI_Base enemy)
        {
            Eq.Delay = E.Delay + Q.Range / E.Speed;
            Eq.From = Player.ServerPosition.To2D().Extend(enemy.ServerPosition.To2D(), Q.Range).To3D();

            var prediction = Eq.GetPrediction(enemy);
            if (prediction.Hitchance >= HitChance.High)
            {
                Q.Cast(Player.ServerPosition.To2D().Extend(prediction.CastPosition.To2D(), Q.Range - 100));
                qeComboT = Utils.TickCount;
                W.LastCastAttemptT = Utils.TickCount;
            }
        }

        private static Vector3 GetGrabableObjectPos(bool onlyOrbs)
        {
            if (!onlyOrbs)
            {
                foreach (var minion in ObjectManager.Get<Obj_AI_Minion>().Where(minion => minion.IsValidTarget(W.Range)))
                {
                    return minion.ServerPosition;
                }
            }
            return OrbManager.GetOrbToGrab((int)W.Range);
        }

        private static float GetComboDamage(Obj_AI_Base enemy)
        {
            var damage = 0d;
            damage += Q.IsReady(420) ? Q.GetDamage(enemy) : 0;
            damage += W.IsReady() ? W.GetDamage(enemy) : 0;
            damage += E.IsReady() ? E.GetDamage(enemy) : 0;
            
            if (IgniteSlot != SpellSlot.Unknown && Player.Spellbook.CanUseSpell(IgniteSlot) == SpellState.Ready)
            {
                damage += ObjectManager.Player.GetSummonerSpellDamage(enemy, Damage.SummonerSpell.Ignite);
            }

            if (R.IsReady())
            {
                damage += Math.Min(7, Player.Spellbook.GetSpell(SpellSlot.R).Ammo) * Player.GetSpellDamage(enemy, SpellSlot.R, 1);
            }
            return (float)damage;
        }

        private static void UseSpells(bool useQ, bool useW, bool useE, bool useR, bool useQe,
            bool useIgnite, bool isHarass)
        {
            var qTarget = TargetSelector.GetTarget(Q.Range + (isHarass ? Q.Width / 3 : Q.Width), TargetSelector.DamageType.Magical);
            var wTarget = TargetSelector.GetTarget(W.Range + W.Width, TargetSelector.DamageType.Magical);
            var rTarget = TargetSelector.GetTarget(R.Range, TargetSelector.DamageType.Magical);
            var qeTarget = TargetSelector.GetTarget(Eq.Range, TargetSelector.DamageType.Magical);
            var comboDamage = rTarget != null ? GetComboDamage(rTarget) : 0;

            //Q
            if (qTarget != null && useQ)
            {
                Q.Cast(qTarget, false, true);
            }

            //E
            if (Utils.TickCount - W.LastCastAttemptT > Game.Ping + 150 && E.IsReady() && useE)
            {
                foreach (var enemy in HeroManager.Enemies)
                {
                    if (enemy.IsValidTarget(Eq.Range))
                    {
                        UseE(enemy);
                    }
                }
            }
                

            //W
            if (useW)
            {
                if (Player.Spellbook.GetSpell(SpellSlot.W).ToggleState == 1 && W.IsReady() && qeTarget != null)
                {
                    var gObjectPos = GetGrabableObjectPos(wTarget == null);

                    if (gObjectPos.To2D().IsValid() && Utils.TickCount - W.LastCastAttemptT > Game.Ping + 300
                        && Utils.TickCount - E.LastCastAttemptT > Game.Ping + 600)
                    {
                        W.Cast(gObjectPos);
                        W.LastCastAttemptT = Utils.TickCount;
                    }
                }
                else if (wTarget != null && Player.Spellbook.GetSpell(SpellSlot.W).ToggleState != 1 && W.IsReady()
                         && Utils.TickCount - W.LastCastAttemptT > Game.Ping + 100)
                {
                    if (OrbManager.WObject(false) != null)
                    {
                        W.From = OrbManager.WObject(false).ServerPosition;
                        W.Cast(wTarget, false, true);
                    }
                }
            }


            if (rTarget != null && useR)
            {
                useR = (Config.Item("DontUlt" + rTarget.CharData.BaseSkinName) != null
                        && Config.Item("DontUlt" + rTarget.CharData.BaseSkinName).GetValue<bool>() == false);
            }
                

            if (rTarget != null && useR && R.IsReady() && comboDamage > rTarget.Health && !rTarget.IsZombie && !Q.IsReady())
            {
                R.Cast(rTarget);
            }

            //Ignite
            if (rTarget != null && useIgnite && IgniteSlot != SpellSlot.Unknown
                && Player.Spellbook.CanUseSpell(IgniteSlot) == SpellState.Ready)
            {
                if (comboDamage > rTarget.Health)
                {
                    Player.Spellbook.CastSpell(IgniteSlot, rTarget);
                }
            }

            //QE
            if (wTarget == null && qeTarget != null && Q.IsReady() && E.IsReady() && useQe)
            {
                UseQe(qeTarget);
            }

            //WE
            if (wTarget == null && qeTarget != null && E.IsReady() && useE && OrbManager.WObject(true) != null)
            {
                Eq.Delay = E.Delay + Q.Range / W.Speed;
                Eq.From = Player.ServerPosition.To2D().Extend(qeTarget.ServerPosition.To2D(), Q.Range).To3D();
                var prediction = Eq.GetPrediction(qeTarget);
                if (prediction.Hitchance >= HitChance.High)
                {
                    W.Cast(Player.ServerPosition.To2D().Extend(prediction.CastPosition.To2D(), Q.Range - 100));
                    weComboT = Utils.TickCount;
                }
            }
        }

        private static void Obj_AI_Hero_OnProcessSpellCast(Obj_AI_Base sender, GameObjectProcessSpellCastEventArgs args)
        {
            if (!sender.IsMe)
            {
                return;
            }

            if (Utils.TickCount - qeComboT < 500 && args.SData.Name.Equals("SyndraQ", StringComparison.InvariantCultureIgnoreCase))
            {
                W.LastCastAttemptT = Utils.TickCount + 400;
                E.Cast(args.End, true);
            }

            if (Utils.TickCount - weComboT < 500
                && (args.SData.Name.Equals("SyndraW", StringComparison.InvariantCultureIgnoreCase) || args.SData.Name.Equals("SyndraWCast", StringComparison.InvariantCultureIgnoreCase)))
            {
                W.LastCastAttemptT = Utils.TickCount + 400;
                E.Cast(args.End, true);
            }
        }

        private static void Farm(bool laneClear)
        {
            if (!Config.Item("EnabledFarm").GetValue<bool>())
            {
                return;
            }

            if (Player.ManaPercent < Config.Item("Lane.Mana").GetValue<Slider>().Value)
            {
                return;
            }
            if (!Orbwalking.CanMove(40))
            {
                return;
            }

            var rangedMinionsQ = MinionManager.GetMinions(ObjectManager.Player.ServerPosition, Q.Range + Q.Width + 30, MinionTypes.Ranged);
            var allMinionsQ = MinionManager.GetMinions(ObjectManager.Player.ServerPosition, Q.Range + Q.Width + 30);
            var rangedMinionsW = MinionManager.GetMinions(ObjectManager.Player.ServerPosition, W.Range + W.Width + 30, MinionTypes.Ranged);
            var allMinionsW = MinionManager.GetMinions(ObjectManager.Player.ServerPosition, W.Range + W.Width + 30);

            var useQi = Config.Item("UseQFarm").GetValue<StringList>().SelectedIndex;
            var useWi = Config.Item("UseWFarm").GetValue<StringList>().SelectedIndex;
            var useQ = (laneClear && (useQi == 1 || useQi == 2)) || (!laneClear && (useQi == 0 || useQi == 2));
            var useW = (laneClear && (useWi == 1 || useWi == 2)) || (!laneClear && (useWi == 0 || useWi == 2));

            if (useQ && Q.IsReady())
                if (laneClear)
                {
                    var fl1 = Q.GetCircularFarmLocation(rangedMinionsQ, Q.Width);
                    var fl2 = Q.GetCircularFarmLocation(allMinionsQ, Q.Width);

                    if (fl1.MinionsHit >= 3)
                    {
                        Q.Cast(fl1.Position);
                    }

                    else if (fl2.MinionsHit >= 2 || allMinionsQ.Count == 1)
                    {
                        Q.Cast(fl2.Position);
                    }
                }
                else
                {
                    foreach (
                        var minion in
                            allMinionsQ.Where(
                                minion =>
                                !Orbwalking.InAutoAttackRange(minion) && minion.Health < 0.75 * Q.GetDamage(minion)))
                    {
                        Q.Cast(minion);
                    }
                }

            if (useW && W.IsReady() && allMinionsW.Count > 3)
            {
                if (laneClear)
                {
                    if (Player.Spellbook.GetSpell(SpellSlot.W).ToggleState == 1)
                    {
                        //WObject
                        var gObjectPos = GetGrabableObjectPos(false);

                        if (gObjectPos.To2D().IsValid() && Utils.TickCount - W.LastCastAttemptT > Game.Ping + 150)
                        {
                            W.Cast(gObjectPos);
                        }
                    }
                    else if (Player.Spellbook.GetSpell(SpellSlot.W).ToggleState != 1)
                    {
                        var fl1 = Q.GetCircularFarmLocation(rangedMinionsW, W.Width);
                        var fl2 = Q.GetCircularFarmLocation(allMinionsW, W.Width);

                        if (fl1.MinionsHit >= 3 && W.IsInRange(fl1.Position.To3D()))
                        {
                            W.Cast(fl1.Position);
                        }

                        else if (fl2.MinionsHit >= 1 && W.IsInRange(fl2.Position.To3D()) && fl1.MinionsHit <= 2)
                        {
                            W.Cast(fl2.Position);
                        }
                    }
                }
            }
        }

        private static void JungleFarm()
        {
            var useQ = Config.Item("UseQJFarm").GetValue<bool>();
            var useW = Config.Item("UseWJFarm").GetValue<bool>();
            var useE = Config.Item("UseEJFarm").GetValue<bool>();

            var mobs = MinionManager.GetMinions(ObjectManager.Player.ServerPosition, W.Range, 
                MinionTypes.All, MinionTeam.Neutral, MinionOrderTypes.MaxHealth);

            if (mobs.Count > 0)
            {
                var mob = mobs[0];

                if (Q.IsReady() && useQ)
                {
                    Q.Cast(mob);
                }

                if (W.IsReady() && useW && Utils.TickCount - Q.LastCastAttemptT > 800)
                {
                    W.Cast(mob);
                }

                if (useE && E.IsReady())
                {
                    E.Cast(mob);
                }
            }
        }

        private static void Game_OnGameUpdate(EventArgs args)
        {
            if (Player.IsDead)
            {
                return;
            }

            //Update the R range
            R.Range = R.Level == 3 ? 750 : 675;

            if (Config.Item("CastQE").GetValue<KeyBind>().Active && E.IsReady() && Q.IsReady())
            {
                foreach (
                    var enemy in
                        HeroManager.Enemies
                            .Where(
                                enemy =>
                                enemy.IsValidTarget(Eq.Range) && Game.CursorPos.Distance(enemy.ServerPosition) < 300))
                {
                    UseQe(enemy);
                }
            }

            if (Config.Item("Key.Combo").GetValue<KeyBind>().Active)
            {
                Combo();
            }
            else
            {
                if (Config.Item("Key.Harass").GetValue<KeyBind>().Active
                    || Config.Item("Key.HarassT").GetValue<KeyBind>().Active) Harass();

                var lc = Config.Item("Key.Lane").GetValue<KeyBind>().Active;
                if (lc || Config.Item("FreezeActive").GetValue<KeyBind>().Active) Farm(lc);

                if (Config.Item("Key.Jungle").GetValue<KeyBind>().Active) JungleFarm();
            }

            if (Config.Item("Key.InstantQE").GetValue<KeyBind>().Active)
            {
                InstantQe2Enemy();
            }
        }

        private static void Drawing_OnDraw(EventArgs args)
        {
            if (ObjectManager.Player.IsDead)
            {
                return;
            }

            //Draw the ranges of the spells.
            var menuItem = Config.Item("QERange").GetValue<Circle>();
            if (menuItem.Active) Render.Circle.DrawCircle(Player.Position, Eq.Range, menuItem.Color);

            foreach (var spell in SpellList)
            {
                menuItem = Config.Item(spell.Slot + "Range").GetValue<Circle>();
                if (menuItem.Active)
                {
                    Render.Circle.DrawCircle(Player.Position, spell.Range, menuItem.Color);
                }
            }

            if (OrbManager.WObject(false) != null)
            Render.Circle.DrawCircle(OrbManager.WObject(false).Position, 100, System.Drawing.Color.White);
        }
    }
}