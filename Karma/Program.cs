#region

using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using LeagueSharp;
using LeagueSharp.Common;

#endregion

namespace Karma
{
    internal class Program
    {
        private const string ChampionName = "Karma";

        private static Orbwalking.Orbwalker _orbwalker;

        private static readonly List<Spell> SpellList = new List<Spell>();

        private static Spell _q;
        private static Spell _w;
        private static Spell _e;
        private static Spell _r;

        private static Menu _config;

        private static bool MantraIsActive
        {
            get { return ObjectManager.Player.HasBuff("KarmaMantra"); }
        }

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

            _q = new Spell(SpellSlot.Q, 1050f);
            _w = new Spell(SpellSlot.W, 700f);
            _e = new Spell(SpellSlot.E, 800f);
            _r = new Spell(SpellSlot.R);

            _q.SetSkillshot(0.25f, 60f, 1700f, true, SkillshotType.SkillshotLine);
            _w.SetTargetted(0.25f, 2200f);
            _e.SetTargetted(0.25f, float.MaxValue);

            SpellList.Add(_q);
            SpellList.Add(_w);
            SpellList.Add(_e);
            SpellList.Add(_r);

            _config = new Menu(ChampionName, ChampionName, true);

            var targetSelectorMenu = new Menu("Target Selector", "Target Selector");
            SimpleTs.AddToMenu(targetSelectorMenu);
            _config.AddSubMenu(targetSelectorMenu);

            _config.AddSubMenu(new Menu("Orbwalking", "Orbwalking"));
            _orbwalker = new Orbwalking.Orbwalker(_config.SubMenu("Orbwalking"));

            _config.AddSubMenu(new Menu("Combo", "Combo"));
            _config.SubMenu("Combo").AddItem(new MenuItem("UseQCombo", "Use Q").SetValue(true));
            _config.SubMenu("Combo").AddItem(new MenuItem("UseWCombo", "Use W").SetValue(true));
            _config.SubMenu("Combo").AddItem(new MenuItem("UseRCombo", "Use R").SetValue(true));
            _config.SubMenu("Combo")
                .AddItem(
                    new MenuItem("ComboActive", "Combo!").SetValue(
                        new KeyBind(_config.Item("Orbwalk").GetValue<KeyBind>().Key, KeyBindType.Press)));

            _config.AddSubMenu(new Menu("Harass", "Harass"));
            _config.SubMenu("Harass").AddItem(new MenuItem("UseQHarass", "Use Q").SetValue(true));
            _config.SubMenu("Harass").AddItem(new MenuItem("UseWHarass", "Use W").SetValue(false));
            _config.SubMenu("Harass").AddItem(new MenuItem("UseRHarass", "Use R").SetValue(true));
            _config.SubMenu("Harass")
                .AddItem(
                    new MenuItem("HarassActive", "Harass!").SetValue(
                        new KeyBind(_config.Item("Farm").GetValue<KeyBind>().Key, KeyBindType.Press)));

            _config.AddSubMenu(new Menu("Misc", "Misc"));
            _config.SubMenu("Misc").AddItem(new MenuItem("UseEDefense", "Use E For Defense").SetValue(true));

            _config.AddSubMenu(new Menu("Drawings", "Drawings"));
            _config.SubMenu("Drawings")
                .AddItem(
                    new MenuItem("QRange", "Q Range").SetValue(new Circle(true, Color.FromArgb(255, 255, 255, 255))));
            _config.SubMenu("Drawings")
                .AddItem(
                    new MenuItem("WRange", "W Range").SetValue(new Circle(false, Color.FromArgb(255, 255, 255, 255))));
            _config.SubMenu("Drawings")
                .AddItem(new MenuItem("WRootRange", "W Root Range").SetValue(new Circle(true, Color.MintCream)));

            _config.SubMenu("Drawings")
                .AddItem(
                    new MenuItem("ERange", "E Range").SetValue(new Circle(false, Color.FromArgb(255, 255, 255, 255))));

            _config.AddToMainMenu();
            AntiGapcloser.OnEnemyGapcloser += AntiGapcloser_OnEnemyGapcloser;
            Drawing.OnDraw += Drawing_OnDraw;
            Game.OnGameUpdate += Game_OnGameUpdate;
            Interrupter.OnPosibleToInterrupt += Interrupter_OnPosibleToInterrupt;
        }

        private static void AntiGapcloser_OnEnemyGapcloser(ActiveGapcloser gapcloser)
        {
            if (gapcloser.Sender.IsValidTarget(300f))
            {
                _e.Cast(ObjectManager.Player);
                _q.Cast(gapcloser.Sender);
            }
        }

        private static void Drawing_OnDraw(EventArgs args)
        {
            var menuItem = _config.Item("WRootRange").GetValue<Circle>();
            if (menuItem.Active)
            {
                foreach (var enemy in
                    ObjectManager.Get<Obj_AI_Hero>().Where(h => h.IsValidTarget() && h.HasBuff("KarmaSpiritBind")))
                {
                    var distance = (1 - Math.Min(Math.Max(850 - ObjectManager.Player.Distance(enemy), 0), 450) / 450);
                
                
                    Render.Circle.DrawCircle(
                        ObjectManager.Player.Position, 850, Color.FromArgb((int)(50 * distance), menuItem.Color), -420,
                        true);
                        Utility.DrawCircle(
                        ObjectManager.Player.Position, 850, Color.FromArgb((int)(255 * distance), menuItem.Color), 10);
                
                    break;
                }
            }

            foreach (var spell in SpellList)
            {
                menuItem = _config.Item(spell.Slot + "Range").GetValue<Circle>();
                if (menuItem.Active)
                {
                    Utility.DrawCircle(ObjectManager.Player.Position, spell.Range, menuItem.Color);
                }
            }
        }

        private static void Game_OnGameUpdate(EventArgs args)
        {
            _q.Width = MantraIsActive ? 80f : 60f; // Mantra increases the q line width
            _q.Range = MantraIsActive ? 1250f : 1050f;
            if (_config.Item("UseEDefense").GetValue<bool>())
            {
                foreach (var hero in
                    ObjectManager.Get<Obj_AI_Hero>()
                        .Where(
                            hero =>
                                hero.IsValidTarget(_e.Range, false) && hero.IsAlly &&
                                ObjectManager.Get<Obj_AI_Hero>().Count(h => h.IsValidTarget() && h.Distance(hero) < 400) >
                                1))
                {
                    _e.Cast(hero);
                }
            }

            if (!_config.Item("ComboActive").GetValue<KeyBind>().Active &&
                !_config.Item("HarassActive").GetValue<KeyBind>().Active)
            {
                return;
            }

            if (_config.Item("HarassActive").GetValue<KeyBind>().Active &&
                ObjectManager.Player.Mana - ObjectManager.Player.Spellbook.GetManaCost(SpellSlot.W) -
                ObjectManager.Player.Spellbook.GetManaCost(SpellSlot.E) < 0)
            {
                return;
            }

            var qTarget = SimpleTs.GetTarget(_q.Range, SimpleTs.DamageType.Magical);
            var wTarget = SimpleTs.GetTarget(_w.Range, SimpleTs.DamageType.Magical);

            var qActive =
                _config.Item("UseQ" + (_config.Item("ComboActive").GetValue<KeyBind>().Active ? "Combo" : "Harass"))
                    .GetValue<bool>();
            var wActive =
                _config.Item("UseW" + (_config.Item("ComboActive").GetValue<KeyBind>().Active ? "Combo" : "Harass"))
                    .GetValue<bool>();
            var rActive =
                _config.Item("UseR" + (_config.Item("ComboActive").GetValue<KeyBind>().Active ? "Combo" : "Harass"))
                    .GetValue<bool>();

            if (wActive && wTarget != null && _w.IsReady())
            {
                if ((ObjectManager.Player.Health / ObjectManager.Player.MaxHealth) /
                    (qTarget.Health / qTarget.MaxHealth) < 1)
                {
                    if (rActive)
                    {
                        _r.Cast();
                    }

                    if (!rActive || !_r.IsReady())
                    {
                        _w.Cast(wTarget);
                    }
                }
            }

            if (qActive && qTarget != null && _q.IsReady())
            {
                if (rActive)
                {
                    _r.Cast();
                }

                if (!rActive || !_r.IsReady())
                {
                    var qPrediction = _q.GetPrediction(qTarget);
                    if (qPrediction.Hitchance >= HitChance.High)
                    {
                        _q.Cast(qPrediction.CastPosition);
                    }
                    else if (qPrediction.Hitchance == HitChance.Collision)
                    {
                        var minionsHit = qPrediction.CollisionObjects;
                        var closest =
                            minionsHit.Where(m => m.NetworkId != ObjectManager.Player.NetworkId)
                                .OrderBy(m => m.Distance(ObjectManager.Player))
                                .FirstOrDefault();

                        if (closest != null && closest.Distance(qPrediction.UnitPosition) < 200)
                        {
                            _q.Cast(qPrediction.CastPosition);
                        }
                    }
                }
            }

            if (wActive && wTarget != null)
            {
                _w.Cast(wTarget);
            }
        }

        private static void Interrupter_OnPosibleToInterrupt(Obj_AI_Base unit, InterruptableSpell spell)
        {
            if (unit.IsValidTarget(1000f) && spell.DangerLevel == InterruptableDangerLevel.High && _e.IsReady())
            {
                _r.Cast();

                if (!_r.IsReady())
                {
                    _e.Cast(ObjectManager.Player);
                }
            }
        }
    }
}