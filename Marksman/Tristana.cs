#region

using System;
using System.Drawing;
using System.Linq;
using LeagueSharp;
using LeagueSharp.Common;

#endregion

namespace Marksman
{
    internal class Tristana : Champion
    {
        public Spell E;
        public Spell Q;
        public Spell R;

        public Tristana()
        {
            Utils.PrintMessage("Tristana loaded.");
            
            Q = new Spell(SpellSlot.Q, 703);
            E = new Spell(SpellSlot.E, 703);
            R = new Spell(SpellSlot.R, 703);

            AntiGapcloser.OnEnemyGapcloser += AntiGapcloser_OnEnemyGapcloser;
            Interrupter.OnPossibleToInterrupt += Interrupter_OnPossibleToInterrupt;
        }

        public void AntiGapcloser_OnEnemyGapcloser(ActiveGapcloser gapcloser)
        {
            if (R.IsReady() && gapcloser.Sender.IsValidTarget(R.Range) && GetValue<bool>("UseRMG"))
                R.CastOnUnit(gapcloser.Sender);
        }

        public void Interrupter_OnPossibleToInterrupt(Obj_AI_Base unit, InterruptableSpell spell)
        {
            if (R.IsReady() && unit.IsValidTarget(R.Range) && GetValue<bool>("UseRMI"))
                R.CastOnUnit(unit);
        }

        public override void Orbwalking_AfterAttack(Obj_AI_Base unit, Obj_AI_Base target)
        {
            if ((ComboActive || HarassActive) && unit.IsMe && (target is Obj_AI_Hero))
            {
                var useQ = GetValue<bool>("UseQ" + (ComboActive ? "C" : "H"));
                var useE = GetValue<bool>("UseE" + (ComboActive ? "C" : "H"));

                if (useQ && Q.IsReady())
                    Q.CastOnUnit(ObjectManager.Player);

                if (useE && E.IsReady())
                    E.CastOnUnit(target);
            }
        }

        public override void Drawing_OnDraw(EventArgs args)
        {
            Spell[] spellList = { E};
            foreach (var spell in spellList)
            {
                var menuItem = GetValue<Circle>("Draw" + spell.Slot);
                if (menuItem.Active)
                    Utility.DrawCircle(ObjectManager.Player.Position, spell.Range, menuItem.Color);
            }
        }

        public override void Game_OnGameUpdate(EventArgs args)
        {
            //Update E and R range depending on level; 550 + 9 Ã— ( Tristana's level - 1)
            E.Range = 550 + 9 * (ObjectManager.Player.Level - 1);
            R.Range = 550 + 9 * (ObjectManager.Player.Level - 1);
            if (Orbwalking.CanMove(100) && (ComboActive || HarassActive))
            {
                var useE = GetValue<bool>("UseE" + (ComboActive ? "C" : "H"));
                if (useE)
                {
                    var eTarget = SimpleTs.GetTarget(E.Range, SimpleTs.DamageType.Physical);
                    if (E.IsReady() && eTarget.IsValidTarget())
                        E.CastOnUnit(eTarget);
                }
            }

            //Killsteal
            if (!ComboActive || !GetValue<bool>("UseRM") || !R.IsReady()) return;
            foreach (
                var hero in
                    ObjectManager.Get<Obj_AI_Hero>()
                        .Where(
                            hero =>
                                hero.IsValidTarget(R.Range) &&
                                DamageLib.getDmg(hero, DamageLib.SpellType.R) - 20 > hero.Health))
                R.CastOnUnit(hero);
        }

        public override void ComboMenu(Menu config)
        {
            config.AddItem(new MenuItem("UseQC" + Id, "Use Q").SetValue(true));
            config.AddItem(new MenuItem("UseEC" + Id, "Use E").SetValue(true));
        }

        public override void HarassMenu(Menu config)
        {
            config.AddItem(new MenuItem("UseQH" + Id, "Use Q").SetValue(false));
            config.AddItem(new MenuItem("UseEH" + Id, "Use E").SetValue(true));
        }

        public override void DrawingMenu(Menu config)
        {
            config.AddItem(
                new MenuItem("DrawE" + Id, "E range").SetValue(new Circle(true, Color.FromArgb(100, 255, 0, 255))));

        }

        public override void MiscMenu(Menu config)
        {
            config.AddItem(new MenuItem("UseRM" + Id, "Use R KillSteal").SetValue(true));
            config.AddItem(new MenuItem("UseRMG" + Id, "Use R Gapclosers").SetValue(true));
            config.AddItem(new MenuItem("UseRMI" + Id, "Use R Interrupt").SetValue(true));
        }
    }
}
