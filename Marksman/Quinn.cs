#region
using System;
using System.Linq;
using LeagueSharp;
using LeagueSharp.Common;
using SharpDX;
#endregion

namespace Marksman
{
    internal class Quinn : Champion
    {
        public Spell Q;
        public Spell E;

        public Quinn()
        {
            Utils.PrintMessage("Quinn loaded.");

            Q = new Spell(SpellSlot.Q, 1025);
            Q.SetSkillshot(0.25f, 160f, 1150, true, Prediction.SkillshotType.SkillshotLine);

            E = new Spell(SpellSlot.E, 800);
            E.SetSkillshot(0.15f, 80f, 2000f, false, Prediction.SkillshotType.SkillshotCircle);
            E.SetTargetted(0.15f, 2000f);
        }

        public override void Orbwalking_AfterAttack(Obj_AI_Base unit, Obj_AI_Base target)
        {
            if ((ComboActive || HarassActive) && !unit.IsMe && (target is Obj_AI_Hero))
            {
                var useQ = GetValue<bool>("UseQ" + (ComboActive ? "C" : "H"));
                var useE = GetValue<bool>("UseE" + (ComboActive ? "C" : "H"));

                if (Q.IsReady() && useQ)
                {
                    Q.Cast(target);
                }
                else if (E.IsReady() && useE)
                {
                    E.CastOnUnit(target);

                }
            }
        }

        public override void Drawing_OnDraw(EventArgs args)
        {
            Spell[] spellList = { Q, E };
            foreach (var spell in spellList)
            {
                var menuItem = GetValue<Circle>("Draw" + spell.Slot);
                if (menuItem.Active && spell.Level > 0)
                    Utility.DrawCircle(ObjectManager.Player.Position, spell.Range, menuItem.Color);
            }
        }

        public static bool isPositionSafe(Obj_AI_Hero target, Spell spell)
        {
            // if (Vector3.Distance(ObjectManager.Player.Position, target.Position) <= E.Range && Vector3.Distance(ObjectManager.Player.Position, target.Position) > 0)
            // {
            Vector2 predPos = spell.GetPrediction(target).Position.To2D();
            Vector2 myPos = ObjectManager.Player.Position.To2D();
            Vector2 newPos = (target.Position.To2D() - myPos);
            newPos.Normalize();

            Vector2 checkPos = predPos + newPos * (spell.Range - Vector2.Distance(predPos, myPos));
            Obj_Turret closestTower = null;

            foreach (var tower in ObjectManager.Get<Obj_Turret>().Where(tower => tower.IsValid && !tower.IsDead && tower.Health != 0))
            {
                if (Vector3.Distance(tower.Position, ObjectManager.Player.Position) < 1450)
                {
                    closestTower = tower;
                }
            }

            if (closestTower == null)
            {
                // No towers in range
                return true;
            }

            if (Vector2.Distance(closestTower.Position.To2D(), checkPos) <= 910)
            {
                // Within tower range
                return false;
            }
            //}

            return true;
        }


        public override void Game_OnGameUpdate(EventArgs args)
        {
            Obj_AI_Hero vTarget;

            if (ComboActive || HarassActive)
            {
                var useQ = GetValue<bool>("UseQ" + (ComboActive ? "C" : "H"));
                var useE = GetValue<bool>("UseE" + (ComboActive ? "C" : "H"));
                var useET = GetValue<bool>("UseET" + (ComboActive ? "C" : "H"));

                if (Orbwalking.CanMove(100))
                {
                    if (E.IsReady() && useE)
                    {
                        vTarget = SimpleTs.GetTarget(E.Range, SimpleTs.DamageType.Physical);
                        if (vTarget != null)
                        {
                            if (!useET)
                                E.CastOnUnit(vTarget);
                            else if (isPositionSafe(vTarget, E))
                                E.CastOnUnit(vTarget);
                        }
                    }

                    if (Q.IsReady() && useQ && !E.IsReady())
                    {
                        vTarget = SimpleTs.GetTarget(Q.Range, SimpleTs.DamageType.Physical);
                        if (vTarget != null)
                            Q.Cast(vTarget);
                    }
                }
            }
        }

        public override void ComboMenu(Menu config)
        {
            config.AddItem(new MenuItem("UseQC" + Id, "Use Q").SetValue(true));
            config.AddItem(new MenuItem("UseEC" + Id, "Use E").SetValue(true));
            config.AddItem(new MenuItem("UseETC" + Id, "Do not Under Turret E").SetValue(true));
        }

        public override void HarassMenu(Menu config)
        {
            config.AddItem(new MenuItem("UseQH" + Id, "Use Q").SetValue(true));
            config.AddItem(new MenuItem("UseEH" + Id, "Use E").SetValue(true));
            config.AddItem(new MenuItem("UseETH" + Id, "Do not Under Turret E").SetValue(true));
        }

        public override void MiscMenu(Menu config)
        {
        }

        public override void DrawingMenu(Menu config)
        {
            config.AddItem(
                new MenuItem("DrawQ" + Id, "Q range").SetValue(new Circle(true, System.Drawing.Color.FromArgb(100, 255, 0, 255))));
            config.AddItem(
                new MenuItem("DrawE" + Id, "E range").SetValue(new Circle(false, System.Drawing.Color.FromArgb(100, 255, 255, 255))));
        }
    }
}
