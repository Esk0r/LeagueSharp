#region

using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using LeagueSharp;
using LeagueSharp.Common;

#endregion

namespace Ryze
{
    internal class Program
    {
        public const string ChampionName = "Ryze";

        //Orbwalker instance
        public static Orbwalking.Orbwalker Orbwalker;

        //Spells
        public static List<Spell> SpellList = new List<Spell>();

        public static Spell Q;
        public static Spell W;
        public static Spell E;

        //Menu
        public static Menu Config;

        private static Obj_AI_Hero Player;

        private static void Main(string[] args)
        {
            CustomEvents.Game.OnGameLoad += Game_OnGameLoad;
        }

        private static void Game_OnGameLoad(EventArgs args)
        {
            Player = ObjectManager.Player;
            if (Player.ChampionName != ChampionName)
            {
                return;
            }

            //Create the spells
            Q = new Spell(SpellSlot.Q, 1000);
            Q.SetSkillshot(0.25f, 55f, 1700, true, SkillshotType.SkillshotLine);
            Q.MinHitChance = HitChance.Medium;

            W = new Spell(SpellSlot.W, 550);
            W.AddEnemyHitboxToRange = true;
            W.AddSelfHitboxToRange = true;

            E = new Spell(SpellSlot.E, 550);
            E.AddEnemyHitboxToRange = true;
            E.AddSelfHitboxToRange = true;

            SpellList.Add(Q);
            SpellList.Add(W);
            SpellList.Add(E);

            //Create the menu
            Config = new Menu(ChampionName, ChampionName, true);

            var targetSelectorMenu = new Menu("Target Selector", "Target Selector");
            TargetSelector.AddToMenu(targetSelectorMenu);
            Config.AddSubMenu(targetSelectorMenu);

            //Orbwalker submenu
            Config.AddSubMenu(new Menu("Orbwalking", "Orbwalking"));

            //Load the orbwalker and add it to the submenu.
            Orbwalker = new Orbwalking.Orbwalker(Config.SubMenu("Orbwalking"));

            Config.AddSubMenu(new Menu("Combo", "Combo"));
            Config.SubMenu("Combo")
                .AddItem(new MenuItem("ComboActive", "Combo!").SetValue(new KeyBind(32, KeyBindType.Press)));

            Config.AddSubMenu(new Menu("Harass", "Harass"));
            Config.SubMenu("Harass").AddItem(new MenuItem("UseQHarass", "Use Q").SetValue(true));
            Config.SubMenu("Harass")
                .AddItem(
                    new MenuItem("HarassActive", "Harass!").SetValue(new KeyBind('C', KeyBindType.Press)));

            Config.AddSubMenu(new Menu("Farm", "Farm"));
            Config.SubMenu("Farm")
                .AddItem(
                    new MenuItem("UseQFarm", "Use Q").SetValue(
                        new StringList(new[] { "Freeze", "LaneClear", "Both", "No" }, 2)));
            Config.SubMenu("Farm")
                .AddItem(
                    new MenuItem("UseWFarm", "Use W").SetValue(
                        new StringList(new[] { "Freeze", "LaneClear", "Both", "No" }, 3)));
            Config.SubMenu("Farm")
                .AddItem(
                    new MenuItem("UseEFarm", "Use E").SetValue(
                        new StringList(new[] { "Freeze", "LaneClear", "Both", "No" }, 1)));
            Config.SubMenu("Farm")
                .AddItem(
                    new MenuItem("FreezeActive", "Freeze!").SetValue(new KeyBind('C', KeyBindType.Press)));
            Config.SubMenu("Farm")
                .AddItem(
                    new MenuItem("LaneClearActive", "LaneClear!").SetValue(new KeyBind('V',
                        KeyBindType.Press)));

            Config.AddSubMenu(new Menu("JungleFarm", "JungleFarm"));
            Config.SubMenu("JungleFarm").AddItem(new MenuItem("UseQJFarm", "Use Q").SetValue(true));
            Config.SubMenu("JungleFarm").AddItem(new MenuItem("UseWJFarm", "Use W").SetValue(true));
            Config.SubMenu("JungleFarm").AddItem(new MenuItem("UseEJFarm", "Use E").SetValue(true));
            Config.SubMenu("JungleFarm")
                .AddItem(
                    new MenuItem("JungleFarmActive", "JungleFarm!").SetValue(new KeyBind('V',
                        KeyBindType.Press)));

            //Damage after combo:
            var dmgAfterComboItem = new MenuItem("DamageAfterCombo", "Draw damage after a rotation").SetValue(true);
            Utility.HpBarDamageIndicator.DamageToUnit += hero => (float)(Player.GetSpellDamage(hero, SpellSlot.Q) * 2 + Player.GetSpellDamage(hero, SpellSlot.W) + Player.GetSpellDamage(hero, SpellSlot.E));
            Utility.HpBarDamageIndicator.Enabled = dmgAfterComboItem.GetValue<bool>();
            dmgAfterComboItem.ValueChanged += delegate(object sender, OnValueChangeEventArgs eventArgs)
            {
                Utility.HpBarDamageIndicator.Enabled = eventArgs.GetNewValue<bool>();
            };

            Config.AddSubMenu(new Menu("Drawings", "Drawings"));
            Config.SubMenu("Drawings")
                .AddItem(new MenuItem("QRange", "Q range").SetValue(new Circle(true, Color.FromArgb(255, 255, 255, 255))));
            Config.SubMenu("Drawings")
                .AddItem(
                    new MenuItem("WRange", "W range").SetValue(new Circle(false, Color.FromArgb(255, 255, 255, 255))));
            Config.SubMenu("Drawings")
                .AddItem(
                    new MenuItem("ERange", "E range").SetValue(new Circle(false, Color.FromArgb(255, 255, 255, 255))));
            Config.SubMenu("Drawings").AddItem(dmgAfterComboItem);
            Config.AddToMainMenu();

            //Add the events we are going to use:
            Drawing.OnDraw += Drawing_OnDraw;
            Game.OnUpdate += Game_OnGameUpdate;
            Orbwalking.BeforeAttack += OrbwalkingOnBeforeAttack;
        }

        private static void OrbwalkingOnBeforeAttack(Orbwalking.BeforeAttackEventArgs args)
        {
            if (Config.Item("ComboActive").GetValue<KeyBind>().Active)
            {
                args.Process = !(W.IsReady() || E.IsReady() || !W.IsInRange(args.Target));
            }
        }

        private static void Drawing_OnDraw(EventArgs args)
        {
            //Draw the ranges of the spells.
            foreach (var spell in SpellList)
            {
                var menuItem = Config.Item(spell.Slot + "Range").GetValue<Circle>();
                if (menuItem.Active)
                {
                    Render.Circle.DrawCircle(Player.Position, spell.GetRange(ObjectManager.Player), menuItem.Color);
                }
            }
        }

        private static void Game_OnGameUpdate(EventArgs args)
        {
            if (Config.Item("ComboActive").GetValue<KeyBind>().Active)
            {
                Combo();
            }
            else
            {
                if (Config.Item("HarassActive").GetValue<KeyBind>().Active)
                {
                    Harass();
                }
                    

                var lc = Config.Item("LaneClearActive").GetValue<KeyBind>().Active;
                if (lc || Config.Item("FreezeActive").GetValue<KeyBind>().Active)
                {
                    Farm(lc);
                }


                if (Config.Item("JungleFarmActive").GetValue<KeyBind>().Active)
                {
                    JungleFarm();
                }
                    
            }
        }

        private static void Combo()
        {
            var coinChargeAmount = ObjectManager.Player.HasBuff("ryzeqiconnocharge") ? 0 : ObjectManager.Player.HasBuff("ryzeqiconhalfcharge") ? 1 : 2;

            //since W range is the same as auto attack range (550) -1 can be used :^)
            var target = TargetSelector.GetTarget(-1, TargetSelector.DamageType.Magical);
            var qTarget = TargetSelector.GetTarget(Q.Range, TargetSelector.DamageType.Magical);

            var castQ = coinChargeAmount == 0 || coinChargeAmount == 2 || !W.IsReady() && !E.IsReady() || qTarget != null && Q.IsKillable(qTarget) || target == null;
            if (castQ && Q.IsReady() && qTarget != null)
            {
                if (Q.Cast(qTarget) == Spell.CastStates.SuccessfullyCasted)
                {
                    return;
                }
            }

            if (target != null)
            {
                if (!(W.IsReady() && W.GetRange(target) - 50 < Player.Distance(target)))
                {
                    if (E.Cast(target) == Spell.CastStates.SuccessfullyCasted)
                    {
                        return;
                    }
                }

                if (W.Cast(target) == Spell.CastStates.SuccessfullyCasted)
                {
                    return;
                }
            }
        }

        private static void Harass()
        {
            var target = TargetSelector.GetTarget(Q.Range, TargetSelector.DamageType.Magical);

            if (target != null && Config.Item("UseQHarass").GetValue<bool>())
            {
                Q.Cast(target);
            }
        }

        private static void Farm(bool laneClear)
        {
            if (!Orbwalking.CanMove(40)) return;
            var allMinions = MinionManager.GetMinions(Player.ServerPosition, Q.Range);
            var useQi = Config.Item("UseQFarm").GetValue<StringList>().SelectedIndex;
            var useWi = Config.Item("UseWFarm").GetValue<StringList>().SelectedIndex;
            var useEi = Config.Item("UseEFarm").GetValue<StringList>().SelectedIndex;
            var useQ = (laneClear && (useQi == 1 || useQi == 2)) || (!laneClear && (useQi == 0 || useQi == 2));
            var useW = (laneClear && (useWi == 1 || useWi == 2)) || (!laneClear && (useWi == 0 || useWi == 2));
            var useE = (laneClear && (useEi == 1 || useEi == 2)) || (!laneClear && (useEi == 0 || useEi == 2));

            if (useQ && Q.IsReady())
            {
                foreach (var minion in allMinions)
                {
                    if (minion.IsValidTarget() &&
                        HealthPrediction.GetHealthPrediction(minion,
                            (int)(Player.Distance(minion) * 1000 / 1700)) <
                         Player.GetSpellDamage(minion, SpellSlot.Q))
                    {
                        Q.Cast(minion);
                        return;
                    }
                }
            }
            else if (useW && W.IsReady())
            {
                foreach (var minion in allMinions)
                {
                    if (minion.IsValidTarget(W.GetRange(minion)) &&
                        minion.Health < Player.GetSpellDamage(minion, SpellSlot.W))
                    {
                        W.CastOnUnit(minion);
                        return;
                    }
                }
            }
            else if (useE && E.IsReady())
            {
                foreach (var minion in allMinions)
                {
                    if (minion.IsValidTarget(E.GetRange(minion)) &&
                        HealthPrediction.GetHealthPrediction(minion,
                            (int)(Player.Distance(minion) * 1000 / 1000)) <
                        Player.GetSpellDamage(minion, SpellSlot.E) - 10)
                    {
                        E.CastOnUnit(minion);
                        return;
                    }
                }
            }

            if (laneClear)
            {
                foreach (var minion in allMinions)
                {
                    if (useQ)
                        Q.Cast(minion.Position);

                    if (useW)
                        W.CastOnUnit(minion);

                    if (useE)
                        E.CastOnUnit(minion);
                }
            }
        }

        private static void JungleFarm()
        {
            var mobs = MinionManager.GetMinions(Player.ServerPosition, W.Range,
                MinionTypes.All,
                MinionTeam.Neutral, MinionOrderTypes.MaxHealth);

            var QMobs = MinionManager.GetMinions(Player.ServerPosition, Q.Range,
                MinionTypes.All,
                MinionTeam.Neutral, MinionOrderTypes.MaxHealth);

            if (mobs.Count > 0)
            {
                var mob = mobs[0];
                W.CastOnUnit(mob);
                E.CastOnUnit(mob);
            }

            if (QMobs.Count > 0)
            {
                var mob = QMobs[0];
                Q.Cast(mob.Position);
            }
        }
    }
}
