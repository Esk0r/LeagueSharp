#region

using System;
using System.Linq;
using LeagueSharp;
using LeagueSharp.Common;

#endregion

namespace Marksman
{
    internal class Graves : Champion
    {
        public Spell Q;
        public Spell R;
        public Spell W;

        public Graves()
        {
            Utils.PrintMessage("Graves loaded.");

            Q = new Spell(SpellSlot.Q, 1000); // Q likes to shoot a bit too far away, so moving the range inward.
            Q.SetSkillshot(Q.Delay, Q.Width, Q,Speed, false, Prediction.SkillshotType.SkillshotCone);

            W = new Spell(SpellSlot.W, 1100);
            W.SetSkillshot(W.Delay, W.Width, W.Speed, false, Prediction.SkillshotType.SkillshotCircle);

            R = new Spell(SpellSlot.R, 1000);
            R.SetSkillshot(R.Delay, R.Width, R.Speed, true, Prediction.SkillshotType.SkillshotLine);
        }

        public override void Game_OnGameUpdate(EventArgs args)
        {
            if ((!ComboActive && !HarassActive) || !Orbwalking.CanMove(100)) return;
            var useQ = GetValue<bool>("UseQ" + (ComboActive ? "C" : "H"));
            var useW = GetValue<bool>("UseW" + (ComboActive ? "C" : "H"));
            var useR = GetValue<bool>("UseR" + (ComboActive ? "C" : "H"));

            if (Q.IsReady() && useQ)
            {
                var t = SimpleTs.GetTarget(Q.Range, SimpleTs.DamageType.Physical);
                if (t != null)
                    Q.Cast(t, false, true);
            }

            if (W.IsReady() && useW)
            {
                var t = SimpleTs.GetTarget(W.Range, SimpleTs.DamageType.Physical);
                if (t != null)
                    W.Cast(t, false, true);
            }

            if (R.IsReady() && useR)
            {
                foreach (
                    var hero in
                        ObjectManager.Get<Obj_AI_Hero>()
                            .Where(
                                hero =>
                                    hero.IsValidTarget(R.Range) &&
                                    DamageLib.getDmg(hero, DamageLib.SpellType.R, DamageLib.StageType.FirstDamage) - 20 > hero.Health))
                    R.Cast(hero, false, true);
            }
        }

        public override void Orbwalking_AfterAttack(Obj_AI_Base unit, Obj_AI_Base target)
        {
            if ((ComboActive || HarassActive) && unit.IsMe && (target is Obj_AI_Hero))
            {
                var useQ = GetValue<bool>("UseQ" + (ComboActive ? "C" : "H"));
                var useW = GetValue<bool>("UseW" + (ComboActive ? "C" : "H"));

                if (Q.IsReady() && useQ)
                    Q.Cast(target);

                if (W.IsReady() && useW)
                    W.Cast(target);
            }
        }

        public override void Drawing_OnDraw(EventArgs args)
        {
            Spell[] spellList = { Q };
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
            config.AddItem(new MenuItem("UseRC" + Id, "Use R").SetValue(true));
            config.AddItem(new MenuItem("UseWC" + Id, "Use W").SetValue(true));
        }

        public override void HarassMenu(Menu config)
        {
            config.AddItem(new MenuItem("UseQH" + Id, "Use Q").SetValue(true));
            config.AddItem(new MenuItem("UseWH" + Id, "Use W").SetValue(false));
        }

        public override void DrawingMenu(Menu config)
        {
            config.AddItem(
                new MenuItem("DrawQ" + Id, "Q range").SetValue(new Circle(true,
                    System.Drawing.Color.FromArgb(100, 255, 0, 255))));
        }
    }
}
