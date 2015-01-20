#region
using System;
using System.Drawing;
using System.Linq;
using LeagueSharp;
using LeagueSharp.Common;
#endregion

namespace Marksman
{
    internal class Sivir : Champion
    {
        public Spell Q;
        public Spell W;
        public Spell E;
        private Menu _menuSupportedSpells;

        public Sivir()
        {
            Utils.PrintMessage("Sivir loaded.");

            Q = new Spell(SpellSlot.Q, 1250);
            Q.SetSkillshot(0.25f, 90f, 1350f, false, SkillshotType.SkillshotLine);

            W = new Spell(SpellSlot.W, 593);

            E = new Spell(SpellSlot.E);
            Obj_AI_Base.OnProcessSpellCast += Game_OnProcessSpellCast;
        }

        public override void Game_OnGameUpdate(EventArgs args)
        {
            if (GetValue<bool>("AutoQ"))
            {
                var t = TargetSelector.GetTarget(Q.Range, TargetSelector.DamageType.Physical);
                if (Q.IsReady() && t.IsValidTarget())
                {
                    int xDelay = 0;
                    if ((t.HasBuffOfType(BuffType.Slow) || t.HasBuffOfType(BuffType.Stun) ||
                         t.HasBuffOfType(BuffType.Snare) || t.HasBuffOfType(BuffType.Fear) ||
                         t.HasBuffOfType(BuffType.Taunt)))
                    {
                        xDelay = 50;
                        Utility.DelayAction.Add(xDelay, () => Q.Cast(t, false, true));
                    }

                    if (t.HasBuffOfType(BuffType.Charm))
                        xDelay = 200;

                    Utility.DelayAction.Add(xDelay, () => Q.Cast(t, false, true));
                }
            }

            if (ComboActive || HarassActive)
            {
                var useQ = GetValue<bool>("UseQ" + (ComboActive ? "C" : "H"));

                if (Orbwalking.CanMove(100))
                {
                    if (Q.IsReady() && useQ)
                    {
                        var t = TargetSelector.GetTarget(Q.Range, TargetSelector.DamageType.Physical);
                        if (t != null)
                        {
                            Q.Cast(t, false, true);
                        }
                    }
                }
            }
        }

        public override void Orbwalking_AfterAttack(AttackableUnit unit, AttackableUnit target)
        {
            var t = target as Obj_AI_Hero;
            if (t != null && (ComboActive || HarassActive) && unit.IsMe)
            {
                var useQ = GetValue<bool>("UseQ" + (ComboActive ? "C" : "H"));
                var useW = GetValue<bool>("UseW" + (ComboActive ? "C" : "H"));

                if (W.IsReady() && useW)
                {
                    W.Cast();
                }
                else if (Q.IsReady() && useQ)
                {
                    Q.Cast(t);
                }
            }
        }

        public override void Drawing_OnDraw(EventArgs args)
        {
            Spell[] spellList = { Q };
            foreach (var spell in spellList)
            {
                var menuItem = GetValue<Circle>("Draw" + spell.Slot);
                if (menuItem.Active)
                    Render.Circle.DrawCircle(ObjectManager.Player.Position, spell.Range, menuItem.Color);
            }
        }

        public override bool ComboMenu(Menu config)
        {
            config.AddItem(new MenuItem("UseQC" + Id, "Use Q").SetValue(true));
            config.AddItem(new MenuItem("UseWC" + Id, "Use W").SetValue(true));
            return true;
        }

        public override bool HarassMenu(Menu config)
        {
            config.AddItem(new MenuItem("UseQH" + Id, "Use Q").SetValue(false));
            config.AddItem(new MenuItem("UseWH" + Id, "Use W").SetValue(false));
            return true;
        }

        public override bool MiscMenu(Menu config)
        {
            config.AddItem(new MenuItem("AutoQ" + Id, "Auto Q on Stun/Slow/Fear/Taunt/Snare").SetValue(true));

            _menuSupportedSpells = new Menu("E Supported Spells", "suppspells");

            foreach (var xEnemy in ObjectManager.Get<Obj_AI_Hero>().Where(enemy => enemy.IsEnemy))
            {
                Obj_AI_Hero enemy = xEnemy;
                foreach (var ccList in SpellList.BuffList.Where(xList => xList.ChampionName == enemy.ChampionName))
                {
                    _menuSupportedSpells.AddItem(new MenuItem(ccList.BuffName, ccList.DisplayName)).SetValue(true);
                }
            }
            Program.Config.AddSubMenu(_menuSupportedSpells);

            return true;
        }

        public override bool DrawingMenu(Menu config)
        {
            config.AddItem(
                new MenuItem("DrawQ" + Id, "Q range").SetValue(new Circle(true, Color.FromArgb(100, 255, 0, 255))));
            return true;
        }

        public override bool ExtrasMenu(Menu config)
        {

            return true;
        }

        public override bool LaneClearMenu(Menu config)
        {
            return true;
        }

        private void Game_OnProcessSpellCast(Obj_AI_Base sender, GameObjectProcessSpellCastEventArgs args)
        {
            if (sender.Type == GameObjectType.obj_AI_Hero && sender.IsEnemy)
            {
                foreach (var spell in
                    _menuSupportedSpells.Items.SelectMany(
                        t =>
                            SpellList.BuffList.Where(
                                xSpell => xSpell.CanBlockWith.ToList().Contains(BlockableSpells.SivirE))
                                .Where(
                                    spell => t.Name == args.SData.Name && t.Name == spell.BuffName && t.GetValue<bool>()))
                    )
                {
                    switch (spell.SkillType)
                    {
                        case SkillShotType.SkillshotCircle:
                            if (ObjectManager.Player.Distance(args.End) <= 250f)
                            {
                                if (E.IsReady())
                                    E.Cast();
                            }
                            break;
                        case SkillShotType.SkillshotLine:
                            if (ObjectManager.Player.Distance(args.End) <= 100f)
                            {
                                if (E.IsReady())
                                    E.Cast();
                            }
                            break;
                        case SkillShotType.SkillshotUnknown:
                            if (ObjectManager.Player.Distance(args.End) <= 500f ||
                                ObjectManager.Player.Distance(sender.Position) <= 500)
                            {
                                if (E.IsReady())
                                    E.Cast();
                            }
                            break;
                    }
                }
            }
        }

    }
}
