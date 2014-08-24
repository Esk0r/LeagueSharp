#region

using System;
using System.Linq;

using LeagueSharp;
using LeagueSharp.Common;

#endregion

namespace Marksman
{
    internal class Lucian : Champion
    {
        public static Spell Q;
        public static Spell Q1;
        public static Spell Q2;
        public static Spell W;

        public static float X1 = 0;
        public static float Y1 = 0;
        public static float X2 = 0;
        public static float Y2 = 0;

        public Lucian()
        {
            Utils.PrintMessage("Lucian loaded.");

            Q = new Spell(SpellSlot.Q, 630);
            Q1 = new Spell(SpellSlot.Q, 1150);
            Q2 = new Spell(SpellSlot.Q, 1600);
            W = new Spell(SpellSlot.W, 1000);

            Q.SetSkillshot(0.25f, 65f, 1200f, false, SkillshotType.SkillshotCircle);
            W.SetSkillshot(0.15f, 80f, 1000f, true, SkillshotType.SkillshotLine);
        }

        public bool LucianHasPassive
        {
            get { return ObjectManager.Player.Buffs.Any(buff => buff.Name == "lucianpassivebuff"); }
        }

        public override void Orbwalking_AfterAttack(Obj_AI_Base unit, Obj_AI_Base target)
        {
            if ((ComboActive || HarassActive) && unit.IsMe && (target is Obj_AI_Hero))
            {
                var useQ = GetValue<bool>("UseQ" + (ComboActive ? "C" : "H"));

                if (useQ)
                {
                    Q.CastOnUnit(target);
                }
            }
        }

        public override void Drawing_OnDraw(EventArgs args)
        {
            Spell[] spellList = { Q, W };
            foreach (var spell in spellList)
            {
                var menuItem = GetValue<Circle>("Draw" + spell.Slot);
                if (menuItem.Active)
                {
                    Utility.DrawCircle(ObjectManager.Player.Position, spell.Range, menuItem.Color);
                }
            }
        }

        public static Obj_AI_Base GetBestMinionForExtendedQ()
        {
            var vTarget2 = SimpleTs.GetTarget(Q1.Range, SimpleTs.DamageType.Physical);

            var targetX = vTarget2.ServerPosition.X;
            var targetY = vTarget2.ServerPosition.Y;

            var lucianX = ObjectManager.Player.ServerPosition.X;
            var lucianY = ObjectManager.Player.ServerPosition.Y;

            const int xWidth = 47;
            const int xHeight = 47;

            var vMinions = MinionManager.GetMinions(
                ObjectManager.Player.ServerPosition, Q.Range, MinionTypes.All, MinionTeam.NotAlly, MinionOrderTypes.None);

            foreach (var vMinion in vMinions.Where(vMinion => vMinion.IsValidTarget(Q.Range)))
            {
                X1 = lucianX > targetX ? lucianX : targetX;
                X2 = lucianX > targetX ? targetX : lucianX;

                Y1 = lucianY > targetY ? lucianY : targetY;
                Y2 = lucianY > targetY ? targetY : lucianY;

                var minionMustBeX1 = X1 - (X1 - X2) / 2 + xWidth;
                var minionMustBeX2 = X2 + (X1 - X2) / 2 - xWidth;

                var minionMustBeY1 = Y1 - (Y1 - Y2) / 2 + xHeight;
                var minionMustBeY2 = Y2 + (Y1 - Y2) / 2 - xHeight;

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
                var useQExtended = GetValue<bool>("UseQExtended" + (ComboActive ? "C" : "H"));


                if (ObjectManager.Player.Spellbook.GetSpell(SpellSlot.R).Level > 0)
                {
                    Config.Item("GHOSTBLADE")
                        .SetValue(ObjectManager.Player.Spellbook.GetSpell(SpellSlot.R).Name == "LucianR");
                }

                if (Orbwalking.CanMove(100))
                {
                    if (Q.IsReady() && useQExtended)
                    {
                        var vTarget = Orbwalker.GetTarget() ??
                                      SimpleTs.GetTarget(Q1.Range, SimpleTs.DamageType.Physical);
                        var bestminion = GetBestMinionForExtendedQ();
                        if (vTarget != null && bestminion != null)
                        {
                            Q.CastOnUnit(bestminion);
                        }
                    }
                    else if (Q.IsReady() && useQ && !LucianHasPassive)
                    {
                        var vTarget = Orbwalker.GetTarget() ?? SimpleTs.GetTarget(Q.Range, SimpleTs.DamageType.Physical);
                        if (vTarget != null)
                        {
                            Q.Cast(vTarget);
                        }
                    }

                    if (W.IsReady() && useW)
                    {
                        var vTarget = Orbwalker.GetTarget() ?? SimpleTs.GetTarget(W.Range, SimpleTs.DamageType.Physical);
                        if (vTarget != null)
                        {
                            if (!LucianHasPassive)
                            {
                                W.Cast(vTarget);
                            }
                        }
                    }
                }
            }
        }

        public override void ComboMenu(Menu config)
        {
            config.AddItem(new MenuItem("UseQC" + Id, "Use Q").SetValue(true));
            config.AddItem(new MenuItem("UseWC" + Id, "Use W").SetValue(true));
            config.AddItem(new MenuItem("UseQExtendedC" + Id, "Use Extended Q").SetValue(true));
        }

        public override void HarassMenu(Menu config)
        {
            config.AddItem(new MenuItem("UseQH" + Id, "Use Q").SetValue(true));
            config.AddItem(new MenuItem("UseWH" + Id, "Use W").SetValue(true));
            config.AddItem(new MenuItem("UseQExtendedH" + Id, "Use Extended Q").SetValue(true));
        }

        public override void MiscMenu(Menu config) { }

        public override void DrawingMenu(Menu config)
        {
            config.AddItem(
                new MenuItem("DrawQ" + Id, "Q range").SetValue(
                    new Circle(true, System.Drawing.Color.FromArgb(100, 255, 0, 255))));

            config.AddItem(
                new MenuItem("DrawW" + Id, "W range").SetValue(
                    new Circle(false, System.Drawing.Color.FromArgb(100, 255, 255, 255))));
        }
    }
}