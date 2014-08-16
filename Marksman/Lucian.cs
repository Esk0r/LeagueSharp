#region

using System;
using System.Collections.Generic;
using System.Linq;
using LeagueSharp;
using LeagueSharp.Common;

#endregion

namespace Marksman
{
    internal class Lucian : Champion
    {
        // Champion
        private static readonly Obj_AI_Hero vLucian = ObjectManager.Player;

        // Spells
        private static readonly List<Spell> spellList = new List<Spell>();
        private static readonly int maxQ = 1200;
        public Spell E;
        public Spell Q;
        public Spell R;
        public Spell W;

        public Lucian()
        {
            Utils.PrintMessage("Lucian loaded.");

            Q = new Spell(SpellSlot.Q, 600);
            W = new Spell(SpellSlot.W, 1000);

            Q.SetSkillshot(0.25f, 65f, float.MaxValue, false, Prediction.SkillshotType.SkillshotCircle);
            W.SetSkillshot(0.15f, 80f, 1000f, true, Prediction.SkillshotType.SkillshotLine);
        }

        public override void Orbwalking_AfterAttack(Obj_AI_Base unit, Obj_AI_Base target)
        {
            if ((ComboActive || HarassActive) && unit.IsMe && (target is Obj_AI_Hero))
            {
                var useQ = GetValue<bool>("UseQ" + (ComboActive ? "C" : "H"));

                if (useQ && Q.IsReady())
                    Q.CastOnUnit(target);
            }
        }

        public override void Drawing_OnDraw(EventArgs args)
        {
            Spell[] spellList = { Q, W };
            foreach (var spell in spellList)
            {
                var menuItem = GetValue<Circle>("Draw" + spell.Slot);
                if (menuItem.Active && spell.Level > 0)
                    Utility.DrawCircle(vLucian.Position, spell.Range, menuItem.Color);
            }
        }

        public static bool HavePassive(Obj_AI_Hero vTarget, string vPassiveName)
        {
            return false;
        }

        public override void Game_OnGameUpdate(EventArgs args)
        {
            if (ComboActive || HarassActive)
            {
                var useQ = GetValue<bool>("UseQ" + (ComboActive ? "C" : "H"));
                var useW = GetValue<bool>("UseW" + (ComboActive ? "C" : "H"));


                var xBuffActive = (from buff in vLucian.Buffs
                    where buff.DisplayName.ToLower() == "lucianpassivebuff"
                    select buff.IsActive).FirstOrDefault();

                if (Orbwalking.CanMove(50))
                {
                    Obj_AI_Hero vTarget;
                    if (Q.IsReady() && useQ && !xBuffActive)
                    {
                        vTarget = SimpleTs.GetTarget(Q.Range, SimpleTs.DamageType.Physical);
                        if (vTarget != null)
                            Q.Cast(vTarget);
                    }

                    if (W.IsReady() && useW)
                    {
                        vTarget = SimpleTs.GetTarget(W.Range, SimpleTs.DamageType.Physical);
                        if (vTarget != null)
                            W.Cast(vTarget);
                    }
                }
            }
        }

        public override void ComboMenu(Menu config)
        {
            config.AddItem(new MenuItem("UseQC" + Id, "Use Q").SetValue(true));
            config.AddItem(new MenuItem("UseWC" + Id, "Use W").SetValue(true));
        }

        public override void HarassMenu(Menu config)
        {
            config.AddItem(new MenuItem("UseQH" + Id, "Use Q").SetValue(true));
            config.AddItem(new MenuItem("UseWH" + Id, "Use W").SetValue(true));
        }

        public override void MiscMenu(Menu config)
        {
        }

        public override void DrawingMenu(Menu config)
        {
            config.AddItem(
                new MenuItem("DrawQ" + Id, "Q range").SetValue(new Circle(true,
                    System.Drawing.Color.FromArgb(100, 255, 0, 255))));
            config.AddItem(
                new MenuItem("DrawW" + Id, "W range").SetValue(new Circle(false,
                    System.Drawing.Color.FromArgb(100, 255, 255, 255))));
        }
    }
}