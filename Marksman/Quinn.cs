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
        public static float ValorMinDamage = 0;
        public static float ValorMaxDamage = 0;
        public Spell E;
        public Spell Q;
        public Spell R;

        public Quinn()
        {
            Utils.PrintMessage("Quinn loaded.");

            Q = new Spell(SpellSlot.Q, 1010);
            E = new Spell(SpellSlot.E, 800);
            R = new Spell(SpellSlot.R, 550);

            Q.SetSkillshot(0.25f, 160f, 1150, true, SkillshotType.SkillshotLine);
            E.SetTargetted(0.25f, 2000f);
        }

        public override void Orbwalking_AfterAttack(Obj_AI_Base unit, Obj_AI_Base target)
        {
            if ((!ComboActive && !HarassActive) || unit.IsMe || (!(target is Obj_AI_Hero))) return;

            if (Q.IsReady() && GetValue<bool>("UseQ" + (ComboActive ? "C" : "H")))
                Q.Cast(target, false, true);
        }

        public override void Drawing_OnDraw(EventArgs args)
        {
            Spell[] spellList = { Q, E };
            foreach (var spell in spellList)
            {
                var menuItem = GetValue<Circle>("Draw" + spell.Slot);
                if (menuItem.Active && spell.Level > 0)
                    Utility.DrawCircle(ObjectManager.Player.Position, spell.Range, menuItem.Color);

                if (menuItem.Active && spell.Level > 0 && IsValorMode())
                    Utility.DrawCircle(ObjectManager.Player.Position, R.Range, menuItem.Color);
            }
        }

        public static bool IsPositionSafe(Obj_AI_Hero target, Spell spell) // use underTurret and .Extend for this please
        {
            var predPos = spell.GetPrediction(target).UnitPosition.To2D();
            var myPos = ObjectManager.Player.Position.To2D();
            var newPos = (target.Position.To2D() - myPos);
            newPos.Normalize();

            var checkPos = predPos + newPos * (spell.Range - Vector2.Distance(predPos, myPos));
            Obj_Turret closestTower = null;

            foreach (
                var tower in
                    ObjectManager.Get<Obj_Turret>().Where(tower => tower.IsValid && !tower.IsDead && Math.Abs(tower.Health) > float.Epsilon))
            {
                if (Vector3.Distance(tower.Position, ObjectManager.Player.Position) < 1450)
                    closestTower = tower;
            }

            if (closestTower == null)
                return true;

            if (Vector2.Distance(closestTower.Position.To2D(), checkPos) <= 910)
                return false;

            return true;
        }

        public static bool ThisIsNotPantheon(Obj_AI_Hero target)
            /* Quinn's Spell E can do nothing when Pantheon's passive is active. I'll add this property for Patheon's Passive. */
        {
            return target.Buffs.All(buff => buff.Name != "pantheonpassivebuff");
        }

        private static bool IsValorMode() // use spell name here, and NEVER ever compare such things with tostring
        {
            //4198404 Transforming Human -> Valor
            //4198407 Valor Mode Active.
            //4194311 Human Mode Active.
            return ObjectManager.Player.CharacterState.ToString() == "4198407";
        }

        public static void calculateValorDamage()
        {
            if (ObjectManager.Player.Spellbook.GetSpell(SpellSlot.R).Level > 0)
            {
                ValorMinDamage = ObjectManager.Player.Spellbook.GetSpell(SpellSlot.R).Level * 50 + 50;
                ValorMinDamage += ObjectManager.Player.BaseAttackDamage * 50;

                ValorMaxDamage = ObjectManager.Player.Spellbook.GetSpell(SpellSlot.R).Level * 100 + 100;
                ValorMaxDamage += ObjectManager.Player.BaseAttackDamage * 100;
            }
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
                            if (vTarget.Health <= E.GetDamage(vTarget) + Q.GetDamage(vTarget) * 2) /* Enemy killable */
                                E.CastOnUnit(vTarget);
                            else if (!useET)
                                E.CastOnUnit(vTarget);
                            else if (IsPositionSafe(vTarget, E))
                                E.CastOnUnit(vTarget);
                        }
                    }

                    if (Q.IsReady() && useQ)
                    {
                        vTarget = SimpleTs.GetTarget(Q.Range, SimpleTs.DamageType.Physical);
                        if (vTarget != null)
                            Q.Cast(vTarget);
                    }

                    if (IsValorMode() && !E.IsReady())
                    {
                        vTarget = SimpleTs.GetTarget(R.Range, SimpleTs.DamageType.Physical);
                        if (vTarget != null)
                        {
                            calculateValorDamage();
                            if (vTarget.Health >= ValorMinDamage && vTarget.Health <= ValorMaxDamage)
                                R.Cast();
                        }
                    }
                }
            }
        }

        public override void ComboMenu(Menu config)
        {
            config.AddItem(new MenuItem("UseQC" + Id, "Use Q").SetValue(true));
            config.AddItem(new MenuItem("UseEC" + Id, "Use E").SetValue(true));
            config.AddItem(new MenuItem("UseETC" + Id, "Do not Under Turret E").SetValue(true));
            config.AddItem(new MenuItem("UseETK" + Id, "Use E Under Turret If Enemy Killable")
                .SetValue(true));
        }

        public override void HarassMenu(Menu config)
        {
            config.AddItem(new MenuItem("UseQH" + Id, "Use Q").SetValue(true));
            config.AddItem(new MenuItem("UseEH" + Id, "Use E").SetValue(true));
            config.AddItem(new MenuItem("UseETH" + Id, "Do not Under Turret E").SetValue(true));
        }

        public override void DrawingMenu(Menu config)
        {
            config.AddItem(
                new MenuItem("DrawQ" + Id, "Q range").SetValue(new Circle(true,
                    System.Drawing.Color.FromArgb(100, 255, 0, 255))));
            config.AddItem(
                new MenuItem("DrawE" + Id, "E range").SetValue(new Circle(false,
                    System.Drawing.Color.FromArgb(100, 255, 255, 255))));
        }
    }
}