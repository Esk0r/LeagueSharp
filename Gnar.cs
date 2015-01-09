#region
using System;
using System.Drawing;
using LeagueSharp;
using LeagueSharp.Common;
#endregion

namespace Marksman
{
    internal class Gnar : Champion
    {
        private static readonly Obj_AI_Hero vGnar = ObjectManager.Player;
        public Spell Q;
        public Spell W;
        public Spell E;

        public Gnar()
        {
            Utils.PrintMessage("Gnar loaded.");

            Q = new Spell(SpellSlot.Q, 1100);
            Q.SetSkillshot(0.5f, 50f, 1200f, false, SkillshotType.SkillshotLine);

            W = new Spell(SpellSlot.W, 600);

            E = new Spell(SpellSlot.E, 500);
            E.SetSkillshot(0.5f, 50f, 1200f, false, SkillshotType.SkillshotCircle);
        }

        public override void Game_OnGameUpdate(EventArgs args)
        {
            if (!Orbwalking.CanMove(100))
                return;

            var useQ = GetValue<bool>("UseQ" + (ComboActive ? "C" : "H"));
            if (ComboActive || HarassActive)
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

        public override void Orbwalking_AfterAttack(AttackableUnit unit, AttackableUnit target)
        {
            var t = target as Obj_AI_Hero;
            if (t != null && (ComboActive || HarassActive) && unit.IsMe)
            {
                var useQ = GetValue<bool>("UseQ" + (ComboActive ? "C" : "H"));
                var useW = GetValue<bool>("UseW" + (ComboActive ? "C" : "H"));

                if (W.IsReady() && useW)
                {
                    ObjectManager.Player.Spellbook.CastSpell(SpellSlot.W);
                }
                else if (Q.IsReady() && useQ)
                {
                    Q.Cast(t);
                }
            }
        }

        public override void Drawing_OnDraw(EventArgs args)
        {
            Spell[] spellList = { Q, W, E };
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

        public override bool DrawingMenu(Menu config)
        {
            config.AddItem(
                new MenuItem("DrawQ" + Id, "Q range").SetValue(new Circle(true, Color.FromArgb(100, 255, 0, 255))));
            config.AddItem(
                new MenuItem("DrawW" + Id, "W range").SetValue(new Circle(true, Color.FromArgb(100, 255, 0, 255))));
            config.AddItem(
                new MenuItem("DrawE" + Id, "E range").SetValue(new Circle(true, Color.FromArgb(100, 255, 0, 255))));
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
    }
}
