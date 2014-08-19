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
        private static readonly Obj_AI_Hero vQuinn = ObjectManager.Player;

        public Spell Q;
        public Spell E;
        public Spell R;

        public static float ValorMinDamage = 0;
        public static float ValorMaxDamage = 0;
        
        public Quinn()
        {
            Utils.PrintMessage("Quinn loaded.");

            Q = new Spell(SpellSlot.Q, 1010);
            E = new Spell(SpellSlot.E, 800);
            R = new Spell(SpellSlot.R, 550);

            Q.SetSkillshot(0.25f, 160f, 1150, true, Prediction.SkillshotType.SkillshotLine);
            E.SetSkillshot(0.15f, 80f, 2000f, false, Prediction.SkillshotType.SkillshotCircle);
        }

        public override void Orbwalking_AfterAttack(Obj_AI_Base unit, Obj_AI_Base vTarget)
        {
            if ((ComboActive || HarassActive) && !unit.IsMe && (vTarget is Obj_AI_Hero))
            {
                var useQ = GetValue<bool>("UseQ" + (ComboActive ? "C" : "H"));
                var useE = GetValue<bool>("UseE" + (ComboActive ? "C" : "H"));
                
                if (Q.IsReady() && useQ)
                    Q.Cast(vTarget);
            }
        }
        
        public override void Drawing_OnDraw(EventArgs args)
        {
            Spell[] spellList = { Q, E };
            foreach (var spell in spellList)
            {
                var menuItem = GetValue<Circle>("Draw" + spell.Slot);
                if (menuItem.Active && spell.Level > 0)
                    Utility.DrawCircle(vQuinn.Position, spell.Range, menuItem.Color);

                if (menuItem.Active && spell.Level > 0 && IsValorMode())
                    Utility.DrawCircle(vQuinn.Position, R.Range, menuItem.Color);
            }
        }
        
        public static bool IsPositionSafe(Obj_AI_Hero target, Spell spell)
        {
            Vector2 predPos = spell.GetPrediction(target).Position.To2D();
            Vector2 myPos = vQuinn.Position.To2D();
            Vector2 newPos = (target.Position.To2D() - myPos);
            newPos.Normalize();
            
            Vector2 checkPos = predPos + newPos * (spell.Range - Vector2.Distance(predPos, myPos));
            Obj_Turret closestTower = null;
            
            foreach (Obj_Turret tower in ObjectManager.Get<Obj_Turret>().Where(tower => tower.IsValid && !tower.IsDead && tower.Health != 0))
            {
                if (Vector3.Distance(tower.Position, vQuinn.Position) < 1450)
                    closestTower = tower;
            }
            
            if (closestTower == null)
                return true;
            
            if (Vector2.Distance(closestTower.Position.To2D(), checkPos) <= 910)
                return false;
            
            return true;
        }
        
        public static bool ThisIsNotPantheon(Obj_AI_Hero vTarget) /* Quinn's Spell E can do nothing when Pantheon's passive is active. I'll add this property for Patheon's Passive. */
        {
            if (vTarget.ChampionName.ToLower() == "pantheon" && vTarget.IsEnemy)
            {
                foreach (var buff in vTarget.Buffs)
                {
                    return buff.Name != "pantheonpassivebuff";
                }
            }
            return true;
        }

        private static bool IsValorMode()
        {
            //4198404 Transforming Human -> Valor
            //4198407 Valor Mode Active.
            //4194311 Human Mode Active.
            return vQuinn.CharacterState.ToString() == "4198407";
        }

        public static void calculateValorDamage()
        {
            if (vQuinn.Spellbook.GetSpell(SpellSlot.R).Level > 0)
            {
                ValorMinDamage = vQuinn.Spellbook.GetSpell(SpellSlot.R).Level * 50 + 50;
                ValorMinDamage += vQuinn.BaseAttackDamage * 50;

                ValorMaxDamage = vQuinn.Spellbook.GetSpell(SpellSlot.R).Level * 100 + 100;
                ValorMaxDamage += vQuinn.BaseAttackDamage * 100;
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

        public override void MiscMenu(Menu config)
        {
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
