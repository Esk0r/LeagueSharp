#region LICENSE

// Copyright 2014 - 2014 LeagueSharp
// Program.cs is part of Velkoz Assembly.
// 
// Velkoz Assembly is free software: you can redistribute it and/or modify
// it under the terms of the GNU General Public License as published by
// the Free Software Foundation, either version 3 of the License, or
// (at your option) any later version.
// 
// Velkoz Assembly is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE. See the
// GNU General Public License for more details.
// 
// You should have received a copy of the GNU General Public License
// along with Velkoz Assembly. If not, see <http://www.gnu.org/licenses/>.

#endregion

#region

using System;
using System.Collections.Generic;
using System.Linq;
using LeagueSharp;
using LeagueSharp.Common;
using SharpDX;
using Color = System.Drawing.Color;

#endregion

namespace Velkoz
{
    internal class Program
    {
        public const string ChampionName = "Velkoz";

        //Orbwalker instance
        public static Orbwalking.Orbwalker Orbwalker;

        //Spells
        public static List<Spell> SpellList = new List<Spell>();

        public static Spell Q;
        public static Spell QSplit;
        public static Spell QDummy;
        public static Spell W;
        public static Spell E;
        public static Spell R;

        public static SpellSlot IgniteSlot;

        public static MissileClient QMissile;

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

            if (Player.BaseSkinName != ChampionName) return;

            //Create the spells
            Q = new Spell(SpellSlot.Q, 1200);
            QSplit = new Spell(SpellSlot.Q, 1100);
            QDummy = new Spell(SpellSlot.Q, (float)Math.Sqrt(Math.Pow(Q.Range, 2) + Math.Pow(QSplit.Range, 2)));
            W = new Spell(SpellSlot.W, 1200);
            E = new Spell(SpellSlot.E, 800);
            R = new Spell(SpellSlot.R, 1550);

            IgniteSlot = Player.GetSpellSlot("SummonerDot");


            Q.SetSkillshot(0.25f, 50f, 1300f, true, SkillshotType.SkillshotLine);
            QSplit.SetSkillshot(0.25f, 55f, 2100, true, SkillshotType.SkillshotLine);
            QDummy.SetSkillshot(0.25f, 55f, float.MaxValue, false, SkillshotType.SkillshotLine);
            W.SetSkillshot(0.25f, 85f, 1700f, false, SkillshotType.SkillshotLine);
            E.SetSkillshot(0.5f, 100f, 1500f, false, SkillshotType.SkillshotCircle);
            R.SetSkillshot(0.3f, 1f, float.MaxValue, false, SkillshotType.SkillshotLine);


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
            Config.SubMenu("Combo").AddItem(new MenuItem("UseRCombo", "Use R").SetValue(true));
            Config.SubMenu("Combo").AddItem(new MenuItem("UseIgniteCombo", "Use Ignite").SetValue(true));
            Config.SubMenu("Combo")
                .AddItem(
                    new MenuItem("ComboActive", "Combo!").SetValue(
                        new KeyBind(Config.Item("Orbwalk").GetValue<KeyBind>().Key, KeyBindType.Press)));

            //Harass menu:
            Config.AddSubMenu(new Menu("Harass", "Harass"));
            Config.SubMenu("Harass").AddItem(new MenuItem("UseQHarass", "Use Q").SetValue(true));
            Config.SubMenu("Harass").AddItem(new MenuItem("UseWHarass", "Use W").SetValue(false));
            Config.SubMenu("Harass").AddItem(new MenuItem("UseEHarass", "Use E").SetValue(false));
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
            Config.SubMenu("Farm").AddItem(new MenuItem("UseQFarm", "Use Q").SetValue(false));
            Config.SubMenu("Farm").AddItem(new MenuItem("UseWFarm", "Use W").SetValue(false));
            Config.SubMenu("Farm").AddItem(new MenuItem("UseEFarm", "Use E").SetValue(false));

            Config.SubMenu("Farm")
                .AddItem(
                    new MenuItem("LaneClearActive", "Farm!").SetValue(
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
            Config.AddToMainMenu();

            //Add the events we are going to use:
            Game.OnUpdate += Game_OnGameUpdate;
            Drawing.OnDraw += Drawing_OnDraw;
            Interrupter2.OnInterruptableTarget += Interrupter2_OnInterruptableTarget;
            GameObject.OnCreate += Obj_SpellMissile_OnCreate;
            Spellbook.OnUpdateChargedSpell += Spellbook_OnUpdateChargedSpell;
            Game.PrintChat(ChampionName + " Loaded!");
        }

        static void Interrupter2_OnInterruptableTarget(Obj_AI_Hero sender, Interrupter2.InterruptableTargetEventArgs args)
        {
            if (!Config.Item("InterruptSpells").GetValue<bool>()) return;
            E.Cast(sender);
        }

        private static void Obj_SpellMissile_OnCreate(GameObject sender, EventArgs args)
        {
            if (!(sender is MissileClient)) return;
            var missile = (MissileClient)sender;
            if (missile.SpellCaster != null && missile.SpellCaster.IsValid && missile.SpellCaster.IsMe &&
                missile.SData.Name.Equals("VelkozQMissile", StringComparison.InvariantCultureIgnoreCase))
            {
                QMissile = missile;
            }
        }

        static void Spellbook_OnUpdateChargedSpell(Spellbook sender, SpellbookUpdateChargedSpellEventArgs args)
        {
            if (sender.Owner.IsMe)
            {
                args.Process =
                        !(Config.Item("ComboActive").GetValue<KeyBind>().Active &&
                          Config.Item("UseRCombo").GetValue<bool>());
            }
        }

        private static void Combo()
        {
            Orbwalker.SetAttack(!(Q.IsReady() || W.IsReady() || E.IsReady()));
            UseSpells(Config.Item("UseQCombo").GetValue<bool>(), Config.Item("UseWCombo").GetValue<bool>(),
                Config.Item("UseECombo").GetValue<bool>(), Config.Item("UseRCombo").GetValue<bool>(),
                Config.Item("UseIgniteCombo").GetValue<bool>());
        }

        private static void Harass()
        {
            UseSpells(Config.Item("UseQHarass").GetValue<bool>(), Config.Item("UseWHarass").GetValue<bool>(),
                Config.Item("UseEHarass").GetValue<bool>(), false, false);
        }

        private static float GetComboDamage(Obj_AI_Base enemy)
        {
            var damage = 0d;

            if (Q.IsReady() && Q.GetCollision(ObjectManager.Player.ServerPosition.To2D(), new List<Vector2> { enemy.ServerPosition.To2D() }).Count == 0)
                damage += Player.GetSpellDamage(enemy, SpellSlot.Q);

            if (W.IsReady())
                damage += W.Instance.Ammo *
                          Player.GetSpellDamage(enemy, SpellSlot.W);

            if (E.IsReady())
                damage += Player.GetSpellDamage(enemy, SpellSlot.E);

            if (IgniteSlot != SpellSlot.Unknown && Player.Spellbook.CanUseSpell(IgniteSlot) == SpellState.Ready)
                damage += Player.GetSummonerSpellDamage(enemy, Damage.SummonerSpell.Ignite);

            if (R.IsReady())
                damage += 7 * Player.GetSpellDamage(enemy, SpellSlot.R) / 10;

            return (float)damage;
        }

        private static void UseSpells(bool useQ, bool useW, bool useE, bool useR, bool useIgnite)
        {
            var qTarget = TargetSelector.GetTarget(Q.Range, TargetSelector.DamageType.Magical);
            var qDummyTarget = TargetSelector.GetTarget(QDummy.Range, TargetSelector.DamageType.Magical);
            var wTarget = TargetSelector.GetTarget(W.Range, TargetSelector.DamageType.Magical);
            var eTarget = TargetSelector.GetTarget(E.Range, TargetSelector.DamageType.Magical);
            var rTarget = TargetSelector.GetTarget(R.Range, TargetSelector.DamageType.Magical);


            if (useW && wTarget != null && W.IsReady())
            {
                W.Cast(wTarget);
                return;
            }

            if (useE && eTarget != null && E.IsReady())
            {
                E.Cast(eTarget);
                return;
            }

            if (useQ && qTarget != null && Q.IsReady() && Q.Instance.ToggleState == 0)
            {
                if (Q.Cast(qTarget) == Spell.CastStates.SuccessfullyCasted)
                    return;
            }

            if (qDummyTarget != null && useQ && Q.IsReady() && Q.Instance.ToggleState == 0)
            {
                if (qTarget != null) qDummyTarget = qTarget;
                QDummy.Delay = Q.Delay + Q.Range / Q.Speed * 1000 + QSplit.Range / QSplit.Speed * 1000;

                var predictedPos = QDummy.GetPrediction(qDummyTarget);
                if (predictedPos.Hitchance >= HitChance.High)
                {
                    for (var i = -1; i < 1; i = i + 2)
                    {
                        var alpha = 28 * (float)Math.PI / 180;
                        var cp = ObjectManager.Player.ServerPosition.To2D() +
                                 (predictedPos.CastPosition.To2D() - ObjectManager.Player.ServerPosition.To2D()).Rotated
                                     (i * alpha);
                        if (
                            Q.GetCollision(ObjectManager.Player.ServerPosition.To2D(), new List<Vector2> { cp }).Count ==
                            0 &&
                            QSplit.GetCollision(cp, new List<Vector2> { predictedPos.CastPosition.To2D() }).Count == 0)
                        {
                            Q.Cast(cp);
                            return;
                        }
                    }
                }
            }

            if (qTarget != null && useIgnite && IgniteSlot != SpellSlot.Unknown &&
                Player.Spellbook.CanUseSpell(IgniteSlot) == SpellState.Ready)
            {
                if (Player.Distance(qTarget) < 650 && GetComboDamage(qTarget) > qTarget.Health)
                {
                    Player.Spellbook.CastSpell(IgniteSlot, qTarget);
                }
            }

            if (useR && rTarget != null && R.IsReady() &&
                Player.GetSpellDamage(rTarget, SpellSlot.R) / 10 * (Player.Distance(rTarget) < (R.Range - 500) ? 10 : 6) > rTarget.Health &&
                (LastCastedSpell.LastCastPacketSent.Slot != SpellSlot.R ||
                 Utils.TickCount - LastCastedSpell.LastCastPacketSent.Tick > 350))
            {
                R.Cast(rTarget);
            }
        }

        private static void Farm()
        {
            if (!Orbwalking.CanMove(40)) return;

            var rangedMinionsE = MinionManager.GetMinions(ObjectManager.Player.ServerPosition, E.Range + E.Width,
                MinionTypes.Ranged);
            var allMinionsW = MinionManager.GetMinions(ObjectManager.Player.ServerPosition, W.Range, MinionTypes.All);

            var useQ = Config.Item("UseQFarm").GetValue<bool>();
            var useW = Config.Item("UseWFarm").GetValue<bool>();
            var useE = Config.Item("UseEFarm").GetValue<bool>();


            if (useQ && allMinionsW.Count > 0 && Q.Instance.ToggleState == 0 && Q.IsReady())
            {
                Q.Cast(allMinionsW[0]);
            }

            if (useW && W.IsReady())
            {
                var wPos = W.GetLineFarmLocation(allMinionsW);
                if (wPos.MinionsHit >= 3)
                    W.Cast(wPos.Position);
            }

            if (useE && E.IsReady())
            {
                var ePos = E.GetCircularFarmLocation(rangedMinionsE);
                if (ePos.MinionsHit >= 3)
                    E.Cast(ePos.Position);
            }
        }

        private static void JungleFarm()
        {
            var useQ = Config.Item("UseQJFarm").GetValue<bool>();
            var useW = Config.Item("UseWJFarm").GetValue<bool>();
            var useE = Config.Item("UseEJFarm").GetValue<bool>();

            var mobs = MinionManager.GetMinions(ObjectManager.Player.ServerPosition, W.Range, MinionTypes.All,
                MinionTeam.Neutral, MinionOrderTypes.MaxHealth);

            if (mobs.Count > 0)
            {
                var mob = mobs[0];
                if (useQ && Q.Instance.ToggleState == 0 && Q.IsReady())
                    Q.Cast(mob);

                if (useW && W.IsReady())
                    W.Cast(mob);

                if (useE && E.IsReady())
                    E.Cast(mob);
            }
        }

        private static void Game_OnGameUpdate(EventArgs args)
        {
            if (Player.IsDead) return;
            if (Player.IsChannelingImportantSpell())
            {
                var endPoint = new Vector2();
                foreach (var obj in ObjectManager.Get<GameObject>())
                {
                    if (obj != null && obj.IsValid && obj.Name.Contains("Velkoz_") &&
                        obj.Name.Contains("_R_Beam_End"))
                    {
                        endPoint = Player.ServerPosition.To2D() +
                                   R.Range * (obj.Position - Player.ServerPosition).To2D().Normalized();
                        break;
                    }
                }

                if (endPoint.IsValid())
                {
                    var targets = new List<Obj_AI_Base>();

                    foreach (var enemy in ObjectManager.Get<Obj_AI_Hero>().Where(h => h.IsValidTarget(R.Range)))
                    {
                        if (enemy.ServerPosition.To2D().Distance(Player.ServerPosition.To2D(), endPoint, true) < 400)
                            targets.Add(enemy);
                    }
                    if (targets.Count > 0)
                    {
                        var target = targets.OrderBy(t => t.Health / Q.GetDamage(t)).ToList()[0];
                        ObjectManager.Player.Spellbook.UpdateChargedSpell(SpellSlot.R, target.ServerPosition, false, false);
                    }
                    else
                    {
                        ObjectManager.Player.Spellbook.UpdateChargedSpell(SpellSlot.R, Game.CursorPos, false, false);
                    }
                }

                return;
            }


            if (QMissile != null && QMissile.IsValid && Q.Instance.ToggleState == 2 &&
                Utils.TickCount - Q.LastCastAttemptT < 2000)
            {
                var qMissilePosition = QMissile.Position.To2D();
                var perpendicular = (QMissile.EndPosition - QMissile.StartPosition).To2D().Normalized().Perpendicular();

                var lineSegment1End = qMissilePosition + perpendicular * QSplit.Range;
                var lineSegment2End = qMissilePosition - perpendicular * QSplit.Range;

                var potentialTargets = new List<Obj_AI_Base>();
                foreach (
                    var enemy in
                        ObjectManager.Get<Obj_AI_Hero>()
                            .Where(
                                h =>
                                    h.IsValidTarget() &&
                                    h.ServerPosition.To2D()
                                        .Distance(qMissilePosition, QMissile.EndPosition.To2D(), true) < 700))
                {
                    potentialTargets.Add(enemy);
                }

                QSplit.UpdateSourcePosition(qMissilePosition.To3D(), qMissilePosition.To3D());

                foreach (
                    var enemy in
                        ObjectManager.Get<Obj_AI_Hero>()
                            .Where(
                                h =>
                                    h.IsValidTarget() &&
                                    (potentialTargets.Count == 0 ||
                                     h.NetworkId == potentialTargets.OrderBy(t => t.Health / Q.GetDamage(t)).ToList()[0].NetworkId) &&
                                    (h.ServerPosition.To2D().Distance(qMissilePosition, QMissile.EndPosition.To2D(), true) > Q.Width + h.BoundingRadius)))
                {
                    var prediction = QSplit.GetPrediction(enemy);
                    var d1 = prediction.UnitPosition.To2D().Distance(qMissilePosition, lineSegment1End, true);
                    var d2 = prediction.UnitPosition.To2D().Distance(qMissilePosition, lineSegment2End, true);
                    if (prediction.Hitchance >= HitChance.High &&
                        (d1 < QSplit.Width + enemy.BoundingRadius || d2 < QSplit.Width + enemy.BoundingRadius))
                    {
                        Q.Cast();
                    }
                }
            }

            Orbwalker.SetAttack(true);
            if (Config.Item("ComboActive").GetValue<KeyBind>().Active)
            {
                Combo();
            }
            else
            {
                if (Config.Item("HarassActive").GetValue<KeyBind>().Active ||
                    Config.Item("HarassActiveT").GetValue<KeyBind>().Active)
                    Harass();

                if (Config.Item("LaneClearActive").GetValue<KeyBind>().Active)
                    Farm();

                if (Config.Item("JungleFarmActive").GetValue<KeyBind>().Active)
                    JungleFarm();
            }
        }

        private static void Drawing_OnDraw(EventArgs args)
        {
            foreach (var spell in SpellList)
            {
                var menuItem = Config.Item(spell.Slot + "Range").GetValue<Circle>();
                if (menuItem.Active)
                    Render.Circle.DrawCircle(Player.Position, spell.Range, menuItem.Color);
            }
        }
    }
}
