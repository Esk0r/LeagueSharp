#region

using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.AccessControl;
using System.Threading;
using LeagueSharp;
using LeagueSharp.Common;
using SharpDX;
using Color = System.Drawing.Color;

#endregion

namespace Xerath
{
    internal class Program
    {
        public const string ChampionName = "Xerath";

        //Orbwalker instance
        public static Orbwalking.Orbwalker Orbwalker;

        //Spells
        public static List<Spell> SpellList = new List<Spell>();

        public static Spell Q;
        public static Spell W;
        public static Spell E;
        public static Spell R;

        //Menu
        public static Menu Config;

        private static Obj_AI_Hero Player;

        private static Vector2 PingLocation;
        private static int LastPingT = 0;
        private static bool AttacksEnabled
        {
            get
            {
                if (IsCastingR)
                {
                    return false;
                }


                if (!ObjectManager.Player.CanAttack)
                {
                    return false;
                }


                if (Config.Item("ComboActive").GetValue<KeyBind>().Active)
                {
                    return IsPassiveUp || (!Q.IsReady() && !W.IsReady() && !E.IsReady());
                }
                    

                return true;
            }
        }

        public static bool IsPassiveUp
        {
            get { return ObjectManager.Player.HasBuff("xerathascended2onhit"); }
        }

        public static bool IsCastingR
        {
            get
            {
                return ObjectManager.Player.HasBuff("XerathLocusOfPower2") ||
                       (ObjectManager.Player.LastCastedSpellName().Equals("XerathLocusOfPower2", StringComparison.InvariantCultureIgnoreCase) &&
                        Utils.TickCount - ObjectManager.Player.LastCastedSpellT() < 500);
            }
        }

        public static class RCharge
        {
            public static int CastT;
            public static int Index;
            public static Vector3 Position;
            public static bool TapKeyPressed;
        }

        private static void Main(string[] args)
        {
            CustomEvents.Game.OnGameLoad += Game_OnGameLoad;
        }

        private static void Game_OnGameLoad(EventArgs args)
        {
            Player = ObjectManager.Player;

            if (Player.ChampionName != ChampionName) return;

            //Create the spells
            Q = new Spell(SpellSlot.Q, 1550);
            W = new Spell(SpellSlot.W, 1000);
            E = new Spell(SpellSlot.E, 1150);
            R = new Spell(SpellSlot.R, 675);

            Q.SetSkillshot(0.6f, 95f, float.MaxValue, false, SkillshotType.SkillshotLine);
            W.SetSkillshot(0.7f, 125f, float.MaxValue, false, SkillshotType.SkillshotCircle);
            E.SetSkillshot(0.25f, 60f, 1400f, true, SkillshotType.SkillshotLine);
            R.SetSkillshot(0.7f, 130f, float.MaxValue, false, SkillshotType.SkillshotCircle);

            Q.SetCharged("XerathArcanopulseChargeUp", "XerathArcanopulseChargeUp", 750, 1550, 1.5f);

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
            TargetSelector.AddToMenu(targetSelectorMenu);
            Config.AddSubMenu(targetSelectorMenu);

            //Load the orbwalker and add it to the menu as submenu.
            Orbwalker = new Orbwalking.Orbwalker(Config.SubMenu("Orbwalking"));

            //Combo menu:
            Config.AddSubMenu(new Menu("Combo", "Combo"));
            Config.SubMenu("Combo").AddItem(new MenuItem("UseQCombo", "Use Q").SetValue(true));
            Config.SubMenu("Combo").AddItem(new MenuItem("UseWCombo", "Use W").SetValue(true));
            Config.SubMenu("Combo").AddItem(new MenuItem("UseECombo", "Use E").SetValue(true));
            Config.SubMenu("Combo")
                .AddItem(
                    new MenuItem("ComboActive", "Combo!").SetValue(
                        new KeyBind(Config.Item("Orbwalk").GetValue<KeyBind>().Key, KeyBindType.Press)));

            //Misc
            Config.AddSubMenu(new Menu("R", "R"));
            Config.SubMenu("R").AddItem(new MenuItem("EnableRUsage", "Auto use charges").SetValue(true));
            Config.SubMenu("R").AddItem(new MenuItem("rMode", "Mode").SetValue(new StringList(new[] { "Normal", "Custom delays", "OnTap"})));
            Config.SubMenu("R").AddItem(new MenuItem("rModeKey", "OnTap key").SetValue(new KeyBind('T', KeyBindType.Press)));
            Config.SubMenu("R").AddSubMenu(new Menu("Custom delays", "Custom delays"));
            for (int i = 1; i <= 5; i++)
                Config.SubMenu("R").SubMenu("Custom delays").AddItem(new MenuItem("Delay"+i, "Delay"+i).SetValue(new Slider(0, 1500, 0)));
            Config.SubMenu("R").AddItem(new MenuItem("PingRKillable", "Ping on killable targets (only local)").SetValue(true));
            Config.SubMenu("R").AddItem(new MenuItem("BlockMovement", "Block right click while casting R").SetValue(false));
            Config.SubMenu("R").AddItem(new MenuItem("OnlyNearMouse", "Focus only targets near mouse").SetValue(false));
            Config.SubMenu("R").AddItem(new MenuItem("MRadius", "Radius").SetValue(new Slider(700, 1500, 300)));

            //Harass menu:
            Config.AddSubMenu(new Menu("Harass", "Harass"));
            Config.SubMenu("Harass").AddItem(new MenuItem("UseQHarass", "Use Q").SetValue(true));
            Config.SubMenu("Harass").AddItem(new MenuItem("UseWHarass", "Use W").SetValue(false));
            Config.SubMenu("Harass")
                .AddItem(
                    new MenuItem("HarassActive", "Harass!").SetValue(
                        new KeyBind(Config.Item("Farm").GetValue<KeyBind>().Key, KeyBindType.Press)));
            Config.SubMenu("Harass")
                .AddItem(
                    new MenuItem("HarassActiveT", "Harass (toggle)!").SetValue(new KeyBind('Y',
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
            Config.SubMenu("JungleFarm")
                .AddItem(
                    new MenuItem("JungleFarmActive", "JungleFarm!").SetValue(
                        new KeyBind(Config.Item("LaneClear").GetValue<KeyBind>().Key, KeyBindType.Press)));

            //Misc
            Config.AddSubMenu(new Menu("Misc", "Misc"));
            Config.SubMenu("Misc").AddItem(new MenuItem("InterruptSpells", "Interrupt spells").SetValue(true));
            Config.SubMenu("Misc").AddItem(new MenuItem("AutoEGC", "AutoE gapclosers").SetValue(true));
            Config.SubMenu("Misc").AddItem(new MenuItem("UseVHHC", "Use very high hit chance").SetValue(true));

            //Damage after combo:
            var dmgAfterComboItem = new MenuItem("DamageAfterR", "Draw damage after (3 - 5)xR").SetValue(true);
            Utility.HpBarDamageIndicator.DamageToUnit += hero => (float)Player.GetSpellDamage(hero, SpellSlot.R) * new int[] {0, 3, 4, 5 }[Player.GetSpell(SpellSlot.R).Level];
            Utility.HpBarDamageIndicator.Enabled = dmgAfterComboItem.GetValue<bool>();
            dmgAfterComboItem.ValueChanged += delegate(object sender, OnValueChangeEventArgs eventArgs)
            {
                Utility.HpBarDamageIndicator.Enabled = eventArgs.GetNewValue<bool>();
            };

            //Drawings menu:
            Config.AddSubMenu(new Menu("Drawings", "Drawings"));
            Config.SubMenu("Drawings")
                .AddItem(
                    new MenuItem("QRange", "Q range").SetValue(new Circle(true, Color.FromArgb(150, Color.DodgerBlue))));
            Config.SubMenu("Drawings")
                .AddItem(
                    new MenuItem("WRange", "W range").SetValue(new Circle(true, Color.FromArgb(150, Color.DodgerBlue))));
            Config.SubMenu("Drawings")
                .AddItem(
                    new MenuItem("ERange", "E range").SetValue(new Circle(false, Color.FromArgb(150, Color.DodgerBlue))));
            Config.SubMenu("Drawings")
                .AddItem(
                    new MenuItem("RRange", "R range").SetValue(new Circle(false, Color.FromArgb(150, Color.DodgerBlue))));
            Config.SubMenu("Drawings")
                .AddItem(
                    new MenuItem("RRangeM", "R range (minimap)").SetValue(new Circle(false,
                        Color.FromArgb(150, Color.DodgerBlue))));
            Config.SubMenu("Drawings")
                .AddItem(dmgAfterComboItem);
            Config.AddToMainMenu();

            //Add the events we are going to use:
            Game.OnUpdate += Game_OnGameUpdate;
            Drawing.OnDraw += Drawing_OnDraw;
            Drawing.OnEndScene += Drawing_OnEndScene;
            Interrupter2.OnInterruptableTarget += Interrupter2_OnInterruptableTarget;
            AntiGapcloser.OnEnemyGapcloser += AntiGapcloser_OnEnemyGapcloser;
            Obj_AI_Hero.OnProcessSpellCast += Obj_AI_Hero_OnProcessSpellCast;
            Game.OnWndProc += Game_OnWndProc;
            Orbwalking.BeforeAttack += OrbwalkingOnBeforeAttack;
            Obj_AI_Hero.OnIssueOrder += Obj_AI_Hero_OnIssueOrder;
        }

        static void Interrupter2_OnInterruptableTarget(Obj_AI_Hero sender, Interrupter2.InterruptableTargetEventArgs args)
        {
            if (!Config.Item("InterruptSpells").GetValue<bool>()) return;
                  
            if (Player.Distance(sender) < E.Range)
            {
                E.Cast(sender);
            }
        }

        static void Obj_AI_Hero_OnIssueOrder(Obj_AI_Base sender, GameObjectIssueOrderEventArgs args)
        {
            if (IsCastingR && Config.Item("BlockMovement").GetValue<bool>())
            {
                args.Process = false;
            }
        }

        private static void OrbwalkingOnBeforeAttack(Orbwalking.BeforeAttackEventArgs args)
        {
            args.Process = AttacksEnabled;
        }

        static void AntiGapcloser_OnEnemyGapcloser(ActiveGapcloser gapcloser)
        {
            if (!Config.Item("AutoEGC").GetValue<bool>()) return;

            if (Player.Distance(gapcloser.Sender) < E.Range)
            {
                E.Cast(gapcloser.Sender);
            }
        }

        static void Game_OnWndProc(WndEventArgs args)
        {
            if (args.Msg == (uint)WindowsMessages.WM_KEYUP)
                RCharge.TapKeyPressed = true;
        }

        static void Obj_AI_Hero_OnProcessSpellCast(Obj_AI_Base sender, GameObjectProcessSpellCastEventArgs args)
        {
            if (!sender.IsMe)
            {
                return;
            }

            if (args.SData.Name.Equals("XerathLocusOfPower2", StringComparison.InvariantCultureIgnoreCase))
            {
                RCharge.CastT = 0;
                RCharge.Index = 0;
                RCharge.Position = new Vector3();
                RCharge.TapKeyPressed = false;
            }
            else if (args.SData.Name.Equals("XerathLocusPulse", StringComparison.InvariantCultureIgnoreCase))
            {
                RCharge.CastT = Utils.TickCount;
                RCharge.Index++;
                RCharge.Position = args.End;
                RCharge.TapKeyPressed = false;
            }
        }

        private static void Combo()
        {
            UseSpells(Config.Item("UseQCombo").GetValue<bool>(), Config.Item("UseWCombo").GetValue<bool>(),
                Config.Item("UseECombo").GetValue<bool>());
        }

        private static void Harass()
        {
            UseSpells(Config.Item("UseQHarass").GetValue<bool>(), Config.Item("UseWHarass").GetValue<bool>(),
                false);
        }

        private static void UseSpells(bool useQ, bool useW, bool useE)
        {
            var qTarget = TargetSelector.GetTarget(Q.ChargedMaxRange, TargetSelector.DamageType.Magical);
            var wTarget = TargetSelector.GetTarget(W.Range + W.Width * 0.5f, TargetSelector.DamageType.Magical);
            var eTarget = TargetSelector.GetTarget(E.Range, TargetSelector.DamageType.Magical);

            Hacks.DisableCastIndicator = Q.IsCharging && useQ;

            if (eTarget != null && useE && E.IsReady())
            {
                if (Player.Distance(eTarget) < E.Range * 0.4f)
                    E.Cast(eTarget);
                else if ((!useW || !W.IsReady()))
                    E.Cast(eTarget);
            }

            if (useQ && Q.IsReady() && qTarget != null)
            {
                if (Q.IsCharging)
                {
                    Q.Cast(qTarget, false, false);
                }
                else if (!useW || !W.IsReady() || Player.Distance(qTarget) > W.Range)
                {
                    Q.StartCharging();
                }
            }

            if (wTarget != null && useW && W.IsReady())
                W.Cast(wTarget, false, true);
        }

        private static Obj_AI_Hero GetTargetNearMouse(float distance)
        {
            Obj_AI_Hero bestTarget = null;
            var bestRatio = 0f;

            if (TargetSelector.SelectedTarget.IsValidTarget() && !TargetSelector.IsInvulnerable(TargetSelector.SelectedTarget, TargetSelector.DamageType.Magical, true) &&
                (Game.CursorPos.Distance(TargetSelector.SelectedTarget.ServerPosition) < distance && ObjectManager.Player.Distance(TargetSelector.SelectedTarget) < R.Range))
            {
                return TargetSelector.SelectedTarget;
            }

            foreach (var hero in ObjectManager.Get<Obj_AI_Hero>())
            {
                if (!hero.IsValidTarget(R.Range) || TargetSelector.IsInvulnerable(hero, TargetSelector.DamageType.Magical, true) || Game.CursorPos.Distance(hero.ServerPosition) > distance)
                {
                    continue;
                }

                var damage = (float)ObjectManager.Player.CalcDamage(hero, Damage.DamageType.Magical, 100);
                var ratio = damage / (1 + hero.Health) * TargetSelector.GetPriority(hero);

                if (ratio > bestRatio)
                {
                    bestRatio = ratio;
                    bestTarget = hero;
                }
            }

            return bestTarget;
        }

        private static void WhileCastingR()
        {
            if (!Config.Item("EnableRUsage").GetValue<bool>()) return;
            var rMode = Config.Item("rMode").GetValue<StringList>().SelectedIndex;

            var rTarget = Config.Item("OnlyNearMouse").GetValue<bool>() ? GetTargetNearMouse(Config.Item("MRadius").GetValue<Slider>().Value) : TargetSelector.GetTarget(R.Range, TargetSelector.DamageType.Magical);

            if (rTarget != null)
            {
                //Wait at least 0.6f if the target is going to die or if the target is to far away
                if(rTarget.Health - R.GetDamage(rTarget) < 0)
                    if (Utils.TickCount - RCharge.CastT <= 700) return;

                if ((RCharge.Index != 0 && rTarget.Distance(RCharge.Position) > 1000))
                    if (Utils.TickCount - RCharge.CastT <= Math.Min(2500, rTarget.Distance(RCharge.Position) - 1000)) return;

                switch (rMode)
                {
                    case 0://Normal
                        R.Cast(rTarget, true);
                        break;

                    case 1://Selected delays.
                        var delay = Config.Item("Delay" + (RCharge.Index + 1)).GetValue<Slider>().Value;
                        if (Utils.TickCount - RCharge.CastT > delay)
                            R.Cast(rTarget, true);
                        break;

                    case 2://On tap
                        if (RCharge.TapKeyPressed)
                            R.Cast(rTarget, true);
                        break;
                }
            }
        }

        private static void Farm(bool laneClear)
        {
            var allMinionsQ = MinionManager.GetMinions(ObjectManager.Player.ServerPosition, Q.ChargedMaxRange,
                MinionTypes.All);
            var rangedMinionsW = MinionManager.GetMinions(ObjectManager.Player.ServerPosition, W.Range + W.Width + 30,
                MinionTypes.Ranged);

            var useQi = Config.Item("UseQFarm").GetValue<StringList>().SelectedIndex;
            var useWi = Config.Item("UseWFarm").GetValue<StringList>().SelectedIndex;
            var useQ = (laneClear && (useQi == 1 || useQi == 2)) || (!laneClear && (useQi == 0 || useQi == 2));
            var useW = (laneClear && (useWi == 1 || useWi == 2)) || (!laneClear && (useWi == 0 || useWi == 2));

            Hacks.DisableCastIndicator = Q.IsCharging && useQi != 0;

            if (useW && W.IsReady())
            {
                var locW = W.GetCircularFarmLocation(rangedMinionsW, W.Width * 0.75f);
                if (locW.MinionsHit >= 3 && W.IsInRange(locW.Position.To3D()))
                {
                    W.Cast(locW.Position);
                    return;
                }
                else
                {
                    var locW2 = W.GetCircularFarmLocation(allMinionsQ, W.Width * 0.75f);
                    if (locW2.MinionsHit >= 1 && W.IsInRange(locW.Position.To3D()))
                    {
                        W.Cast(locW.Position);
                        return;
                    }
                        
                }
            }

            if (useQ && Q.IsReady())
            {
                if (Q.IsCharging)
                {
                    var locQ = Q.GetLineFarmLocation(allMinionsQ);
                    if (allMinionsQ.Count == allMinionsQ.Count(m => Player.Distance(m) < Q.Range) && locQ.MinionsHit > 0 && locQ.Position.IsValid())
                        Q.Cast(locQ.Position);
                }
                else if (allMinionsQ.Count > 0)
                    Q.StartCharging();
            }
        }

        private static void JungleFarm()
        {
            var useQ = Config.Item("UseQJFarm").GetValue<bool>();
            var useW = Config.Item("UseWJFarm").GetValue<bool>();
            var mobs = MinionManager.GetMinions(ObjectManager.Player.ServerPosition, W.Range, MinionTypes.All,
                MinionTeam.Neutral, MinionOrderTypes.MaxHealth);

            if (mobs.Count > 0)
            {
                var mob = mobs[0];
                if (useW && W.IsReady())
                {
                    W.Cast(mob);
                }
                else if (useQ && Q.IsReady())
                {
                    if (!Q.IsCharging)
                        Q.StartCharging();
                    else
                        Q.Cast(mob);
                }
            }
        }

        private static void Ping(Vector2 position)
        {
            if (Utils.TickCount - LastPingT < 30 * 1000)
            {
                return;
            }
            
            LastPingT = Utils.TickCount;
            PingLocation = position;
            SimplePing();
            
            Utility.DelayAction.Add(150, SimplePing);
            Utility.DelayAction.Add(300, SimplePing);
            Utility.DelayAction.Add(400, SimplePing);
            Utility.DelayAction.Add(800, SimplePing);
        }

        private static void SimplePing()
        {
            Game.ShowPing(PingCategory.Fallback, PingLocation, true);
        }

        private static void Game_OnGameUpdate(EventArgs args)
        {
            if (Player.IsDead) return;

            if (Config.SubMenu("Misc").Item("UseVHHC").GetValue<bool>())
            {
                Q.MinHitChance = HitChance.VeryHigh;
                W.MinHitChance = HitChance.VeryHigh;
                E.MinHitChance = HitChance.VeryHigh;
                R.MinHitChance = HitChance.VeryHigh;
            }
            else
            {
                Q.MinHitChance = HitChance.High;
                W.MinHitChance = HitChance.High;
                E.MinHitChance = HitChance.High;
                R.MinHitChance = HitChance.High;
            }
            Orbwalker.SetMovement(true);

            //Update the R range
            R.Range = 1200 * R.Level + 2000; 

            if (IsCastingR)
            {
                Orbwalker.SetMovement(false);
                WhileCastingR();
                return;
            }

            if (R.IsReady() && Config.Item("PingRKillable").GetValue<bool>())
            {
                foreach (var enemy in HeroManager.Enemies.Where(h => h.IsValidTarget() && (float)Player.GetSpellDamage(h, SpellSlot.R) * new int[] { 0, 3, 4, 5 }[Player.GetSpell(SpellSlot.R).Level] > h.Health))
                {
                    Ping(enemy.Position.To2D());
                }
            }

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

        private static void Drawing_OnEndScene(EventArgs args)
        {
            if (R.Level == 0) return;
            var menuItem = Config.Item(R.Slot + "RangeM").GetValue<Circle>();
            if (menuItem.Active)
                Utility.DrawCircle(Player.Position, R.Range, menuItem.Color, 1, 23, true);
        }

        private static void Drawing_OnDraw(EventArgs args)
        {
            if (IsCastingR)
            {
                if (Config.Item("OnlyNearMouse").GetValue<bool>())
                {
                    Render.Circle.DrawCircle(Game.CursorPos, Config.Item("MRadius").GetValue<Slider>().Value, Color.White);
                }
            }

            //Draw the ranges of the spells.
            foreach (var spell in SpellList)
            {
                var menuItem = Config.Item(spell.Slot + "Range").GetValue<Circle>();
                if (menuItem.Active && (spell.Slot != SpellSlot.R || R.Level > 0))
                    Render.Circle.DrawCircle(Player.Position, spell.Range, menuItem.Color);
            }
        }
    }
}
