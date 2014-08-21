#region
using System;
using System.Linq;
using LeagueSharp;
using LeagueSharp.Common;
using SharpDX;
#endregion

namespace Marksman
{
    internal class Lucian : Champion
    {
        // Champion
        private static readonly Obj_AI_Hero vLucian = ObjectManager.Player;
        // Spells
        public static Spell Q = new Spell(SpellSlot.Q, 630);
        public static Spell Q1 = new Spell(SpellSlot.Q, 1150);
        public static Spell W = new Spell(SpellSlot.W, 1000);

        public static float x1 = 0;
        public static float y1 = 0;
        public static float x2 = 0;
        public static float y2 = 0;

        public bool LucianHasPassive { get { return vLucian.Buffs.Any(buff => buff.Name == "lucianpassivebuff"); } }
        public static Spell Q2 = new Spell(SpellSlot.Q, 1600);

        public Lucian()
        {
            Utils.PrintMessage("Lucian loaded.");

            Q.SetSkillshot(0.25f, 65f, 1200f, false, Prediction.SkillshotType.SkillshotCircle);
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

        public static Obj_AI_Base GetBestMinionForExtendedQ()
        {
            var vTarget2 = SimpleTs.GetTarget(Q1.Range, SimpleTs.DamageType.Physical);

            var targetX = vTarget2.ServerPosition.X;
            var targetY = vTarget2.ServerPosition.Y;

            var lucianX = vLucian.ServerPosition.X;
            var lucianY = vLucian.ServerPosition.Y;

            var xWidth = 47;
            var xHeight = 47;

            var vMinions = MinionManager.GetMinions(vLucian.ServerPosition, Q.Range, MinionTypes.All, MinionTeam.NotAlly, MinionOrderTypes.None);

            foreach (var vMinion in vMinions.Where(vMinion => vMinion.IsValidTarget(Q.Range)))
            {
                x1 = lucianX > targetX ? lucianX : targetX;
                x2 = lucianX > targetX ? targetX : lucianX;

                y1 = lucianY > targetY ? lucianY : targetY;
                y2 = lucianY > targetY ? targetY : lucianY;

                float minionMustBeX1 = x1 - (x1 - x2) / 2 + xWidth;
                float minionMustBeX2 = x2 + (x1 - x2) / 2 - xWidth;

                float minionMustBeY1 = y1 - (y1 - y2) / 2 + xHeight;
                float minionMustBeY2 = y2 + (y1 - y2) / 2 - xHeight;

                if (vMinion.ServerPosition.X < minionMustBeX1 && vMinion.ServerPosition.X > minionMustBeX2 &&
                    vMinion.ServerPosition.Y < minionMustBeY1 && vMinion.ServerPosition.Y > minionMustBeY2)
                {
                    return vMinion;
                }
            }
            return null;
        }
        public override void Game_OnGameUpdate(EventArgs args)
        {
            if (ComboActive || HarassActive)
            {
                var useQ = GetValue<bool>("UseQ" + (ComboActive ? "C" : "H"));
                var useW = GetValue<bool>("UseW" + (ComboActive ? "C" : "H"));
                var UseQExtended = GetValue<bool>("UseQExtended" + (ComboActive ? "C" : "H"));


                if (vLucian.Spellbook.GetSpell(SpellSlot.R).Level > 0)
                {
                    if (vLucian.Spellbook.GetSpell(SpellSlot.R).Name != "LucianR")
                        Config.Item("GHOSTBLADE").SetValue<bool>(false);
                    else
                        Config.Item("GHOSTBLADE").SetValue<bool>(true);
                }

                if (Orbwalking.CanMove(50))
                {
                    if (Q.IsReady() && UseQExtended)
                    {
                        var vTarget = Orbwalker.GetTarget() ?? SimpleTs.GetTarget(Q1.Range, SimpleTs.DamageType.Physical);
                        var bestminion = GetBestMinionForExtendedQ();
                        if (vTarget != null && bestminion != null)
                            Q.CastOnUnit(bestminion);
                    }
                    else if (Q.IsReady() && useQ && !LucianHasPassive)
                    {
                        var vTarget = Orbwalker.GetTarget() ?? SimpleTs.GetTarget(Q.Range, SimpleTs.DamageType.Physical);
                        if (vTarget != null)
                            Q.Cast(vTarget);
                    }

                    if (W.IsReady() && useW)
                    {
                        var vTarget = Orbwalker.GetTarget() ?? SimpleTs.GetTarget(W.Range, SimpleTs.DamageType.Physical);
                        if (vTarget != null)
                            if (!LucianHasPassive)
                                W.Cast(vTarget);
                    }
                }
            }
        }

        public override void ComboMenu(Menu config)
        {
            config.AddItem(new MenuItem("UseQC" + Id, "Use Q").SetValue(true));
            config.AddItem(new MenuItem("UseWC" + Id, "Use W").SetValue(true));
            config.AddItem(new MenuItem("UseQExtendedC" + Id, "Use Extended Q")
              .SetValue(true));
        }

        public override void HarassMenu(Menu config)
        {
            config.AddItem(new MenuItem("UseQH" + Id, "Use Q").SetValue(true));
            config.AddItem(new MenuItem("UseWH" + Id, "Use W").SetValue(true));
            config.AddItem(new MenuItem("UseQExtendedH" + Id, "Use Extended Q")
              .SetValue(true));
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
