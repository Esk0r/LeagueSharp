#region
using System;
using System.Linq;
using LeagueSharp;
using LeagueSharp.Common;
#endregion

namespace Marksman
{
    internal class MissFortune : Champion
    {
        public static Spell Q, W, E;

        public MissFortune()
        {
            Utils.PrintMessage("MissFortune loaded.");

            Q = new Spell(SpellSlot.Q, 650);
            Q.SetTargetted(0.29f, 1400f);

            W = new Spell(SpellSlot.W);

            E = new Spell(SpellSlot.E, 800);
            E.SetSkillshot(0.5f, 100f, float.MaxValue, false, SkillshotType.SkillshotCircle);
        }

        public override void Orbwalking_AfterAttack(Obj_AI_Base unit, Obj_AI_Base vTarget)
        {
            if ((ComboActive || HarassActive) && unit.IsMe && (vTarget is Obj_AI_Hero))
            {
                var useQ = GetValue<bool>("UseQ" + (ComboActive ? "C" : "H"));
                var useW = GetValue<bool>("UseW" + (ComboActive ? "C" : "H"));

                if (useQ)
                {
                    Q.CastOnUnit(vTarget);
                }
                if (useW && W.IsReady())
                    W.CastOnUnit(ObjectManager.Player);
            }
        }

        public override void Drawing_OnDraw(EventArgs args)
        {
            Spell[] spellList = { Q, E };
            foreach (var spell in spellList)
            {
                var menuItem = GetValue<Circle>("Draw" + spell.Slot);
                if (menuItem.Active && spell.Level > 0)
                {
                    Utility.DrawCircle(ObjectManager.Player.Position, spell.Range, menuItem.Color);
                }
            }
        }

        public override void Game_OnGameUpdate(EventArgs args)
        {
            if (Q.IsReady() && GetValue<KeyBind>("UseQTH").Active)
            {
                if(ObjectManager.Player.HasBuff("Recall"))
                    return;
                var t = SimpleTs.GetTarget(Q.Range, SimpleTs.DamageType.Physical);
                if (t != null)
                    Q.Cast(t);
            }
                
            if (E.IsReady() && GetValue<KeyBind>("UseETH").Active)
            {
                if(ObjectManager.Player.HasBuff("Recall"))
                    return;
                var t = Orbwalker.GetTarget() ??
                        SimpleTs.GetTarget(E.Range + E.Range / 2, SimpleTs.DamageType.Physical);
                if (t != null)
                    E.CastIfHitchanceEquals(t, HitChance.High);
            }

            if (ComboActive || HarassActive)
            {
                var useQ = GetValue<bool>("UseQ" + (ComboActive ? "C" : "H"));
                var useW = GetValue<bool>("UseW" + (ComboActive ? "C" : "H"));
                var useE = GetValue<bool>("UseE" + (ComboActive ? "C" : "H"));

                if (Orbwalking.CanMove(100))
                {
                    if (Q.IsReady() && useQ)
                    {
                        var vTarget = Orbwalker.GetTarget() ?? 
                                      SimpleTs.GetTarget(Q.Range, SimpleTs.DamageType.Physical);
                        if (vTarget != null)
                        {
                            Q.CastOnUnit(vTarget);
                        }
                    }

                    if (E.IsReady() && useE)
                    {
                        var vTarget = Orbwalker.GetTarget() ??
                                      SimpleTs.GetTarget(E.Range + E.Range / 2, SimpleTs.DamageType.Physical);
                        if (vTarget != null)
                            E.CastIfHitchanceEquals(vTarget, HitChance.High);
                    }
                }
            }
        }

        public override bool ComboMenu(Menu config)
        {
            config.AddItem(new MenuItem("UseQC" + Id, "Use Q").SetValue(true));
            config.AddItem(new MenuItem("UseWC" + Id, "Use W").SetValue(true));
            config.AddItem(new MenuItem("UseEC" + Id, "Use E").SetValue(true));

            return true;
        }

        public override bool HarassMenu(Menu config)
        {
            config.AddItem(new MenuItem("UseQH" + Id, "Use Q").SetValue(true));
            config.AddItem(new MenuItem("UseWH" + Id, "Use W").SetValue(true));
            config.AddItem(new MenuItem("UseEH" + Id, "Use E").SetValue(true));
            config.AddItem(
                new MenuItem("UseQTH" + Id, "Use Q (Toggle)").SetValue(new KeyBind("H".ToCharArray()[0],
                    KeyBindType.Toggle))); 
            config.AddItem(
                new MenuItem("UseETH" + Id, "Use E (Toggle)").SetValue(new KeyBind("T".ToCharArray()[0],
                    KeyBindType.Toggle))); 
                    
            return true;
        }

        public override bool DrawingMenu(Menu config)
        {
            config.AddItem(
                new MenuItem("DrawQ" + Id, "Q range").SetValue(
                    new Circle(true, System.Drawing.Color.FromArgb(100, 255, 0, 255))));

            config.AddItem(
                new MenuItem("DrawE" + Id, "E range").SetValue(
                    new Circle(false, System.Drawing.Color.FromArgb(100, 255, 255, 255))));

            return true;
        }

        public override bool ExtrasMenu(Menu config)
        {

            return true;
        }

    }
}
