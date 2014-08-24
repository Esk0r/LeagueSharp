#region

using System;
using System.Drawing;
using System.Linq;
using LeagueSharp;
using LeagueSharp.Common;

#endregion

namespace Marksman
{
    internal class Twitch : Champion
    {
        public Spell W;
        public Spell E;
        public int ExpungeBuffStacks = 0;

        public Twitch()
        {
            Utils.PrintMessage("Twitch loaded.");

            W = new Spell(SpellSlot.W, 950);
            W.SetSkillshot(0.25f, 120f, 1400f, true, SkillshotType.SkillshotCircle);
            E = new Spell(SpellSlot.E, 1200);
        }

        public override void Orbwalking_AfterAttack(Obj_AI_Base unit, Obj_AI_Base target)
        {
            if ((ComboActive || HarassActive) && unit.IsMe && (target is Obj_AI_Hero))
            {
                var useW = GetValue<bool>("UseW" + (ComboActive ? "C" : "H"));

                if (useW && W.IsReady())
                    W.Cast(target, false, true);
            }
        }

        public override void Drawing_OnDraw(EventArgs args)
        {
            Spell[] spellList = { W};
            foreach (var spell in spellList)
            {
                var menuItem = GetValue<Circle>("Draw" + spell.Slot);
                if (menuItem.Active)
                    Utility.DrawCircle(ObjectManager.Player.Position, spell.Range, menuItem.Color);
            }
        }

        public override void Game_OnGameUpdate(EventArgs args)
        {
            if (Orbwalking.CanMove(100) && (ComboActive || HarassActive))
            {
                var useW = GetValue<bool>("UseW" + (ComboActive ? "C" : "H"));
                var useE = GetValue<bool>("UseE" + (ComboActive ? "C" : "H"));
                
                if (useW)
                {
                    var wTarget = SimpleTs.GetTarget(W.Range, SimpleTs.DamageType.Physical);
                    if (W.IsReady() && wTarget.IsValidTarget())
                        W.Cast(wTarget, false, true);
                }
                
                if (useE)
                {
                    var eTarget = SimpleTs.GetTarget(E.Range, SimpleTs.DamageType.Physical);
                    //foreach (var buff in eTarget.Buffs) { Game.PrintChat(buff.DisplayName); }
                    if (E.IsReady() && eTarget.IsValidTarget() && eTarget.HasBuff("TwitchDeadlyVenom"))
                    ExpungeBuffStacks = (from buff in eTarget.Buffs
                                 where buff.DisplayName.ToLower() == "twitchdeadlyvenom"
                                 select buff.Count).FirstOrDefault();
                    if (ExpungeBuffStacks > 5)
                       E.Cast();
                     
                }
            }

            if (GetValue<bool>("UseEM") && E.IsReady())
            {
                foreach (
                    var hero in
                        ObjectManager.Get<Obj_AI_Hero>()
                            .Where(
                                hero =>
                                    hero.IsValidTarget(E.Range) &&
                                    DamageLib.getDmg(hero, DamageLib.SpellType.E) - 15 > hero.Health))
                    E.Cast();
            }

        }

        public override void ComboMenu(Menu config)
        {
            config.AddItem(new MenuItem("UseWC" + Id, "Use W").SetValue(true));
            config.AddItem(new MenuItem("UseEC" + Id, "Use E max Stacks").SetValue(false));
        }

        public override void HarassMenu(Menu config)
        {
            config.AddItem(new MenuItem("UseWH" + Id, "Use W").SetValue(false));
            config.AddItem(new MenuItem("UseEH" + Id, "Use E at max Stacks").SetValue(false));
        }

        public override void DrawingMenu(Menu config)
        {
            config.AddItem(
                new MenuItem("DrawW" + Id, "W range").SetValue(new Circle(true, Color.FromArgb(100, 255, 0, 255))));
        }

        public override void MiscMenu(Menu config)
        {
            config.AddItem(new MenuItem("UseEM" + Id, "Use E KS").SetValue(true));
        }
    }
}
