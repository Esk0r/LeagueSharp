#region
using System;
using System.Drawing;
using System.Linq;
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
            Console.Clear();
            Utils.PrintMessage("Gnar loaded.");

            Q = new Spell(SpellSlot.Q, 1100);
            Q.SetSkillshot(0.5f, 50f, 1200f, false, SkillshotType.SkillshotLine);

            W = new Spell(SpellSlot.W, 600);
            E = new Spell(SpellSlot.E, 500);
            E.SetSkillshot(0.5f, 50f, 1200f, false, SkillshotType.SkillshotCircle);
        }

        public override void Game_OnGameUpdate(EventArgs args)
        {
            if (vGnar.Spellbook.GetSpell(SpellSlot.Q).Level > 0)
            if (ComboActive || HarassActive)
            {
                var useQ = GetValue<bool>("UseQ" + (ComboActive ? "C" : "H"));

                if (Orbwalking.CanMove(100))
                {
                    if (Q.IsReady() && useQ)
                    {
                        var t = SimpleTs.GetTarget(Q.Range, SimpleTs.DamageType.Physical);
                        if (t != null)
                        {
                            Q.Cast(t, false, true);
                        }
                    }
                }
            }
        }

        public override void Orbwalking_AfterAttack(Obj_AI_Base unit, Obj_AI_Base target)
        {
            if ((ComboActive || HarassActive) && unit.IsMe && (target is Obj_AI_Hero))
            {
                var useQ = GetValue<bool>("UseQ" + (ComboActive ? "C" : "H"));
                var useW = GetValue<bool>("UseW" + (ComboActive ? "C" : "H"));

                if (W.IsReady() && useW)
                {
                    ObjectManager.Player.Spellbook.CastSpell(SpellSlot.W);
                }
                else if (Q.IsReady() && useQ)
                {
                    Q.Cast(target);
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
                    Utility.DrawCircle(ObjectManager.Player.Position, spell.Range, menuItem.Color);
            }
        }

        public override void ComboMenu(Menu config)
        {
            config.AddItem(new MenuItem("UseQC" + Id, "Use Q").SetValue(true));
            config.AddItem(new MenuItem("UseWC" + Id, "Use W").SetValue(true));
        }

        public override void HarassMenu(Menu config)
        {
            config.AddItem(new MenuItem("UseQH" + Id, "Use Q").SetValue(false));
            config.AddItem(new MenuItem("UseWH" + Id, "Use W").SetValue(false));
        }

        public override void DrawingMenu(Menu config)
        {
            config.AddItem(
                new MenuItem("DrawQ" + Id, "Q range").SetValue(new Circle(true, Color.FromArgb(100, 255, 0, 255))));

            config.AddItem(
                new MenuItem("DrawW" + Id, "W range").SetValue(new Circle(true, Color.FromArgb(100, 255, 0, 255))));
 
            config.AddItem(
                new MenuItem("DrawE" + Id, "E range").SetValue(new Circle(true, Color.FromArgb(100, 255, 0, 255))));
        }
    }
}