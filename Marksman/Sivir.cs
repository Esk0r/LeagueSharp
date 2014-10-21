#region

using System;
using System.Drawing;
using System.Linq;
using LeagueSharp;
using LeagueSharp.Common;

#endregion

namespace Marksman
{
    internal class Sivir : Champion
    {
        public Spell Q;
        public Spell W;
        public Spell E;

        public Menu MenuSuppSpells;


        public Sivir()
        {
            Utils.PrintMessage("Sivir loaded.");

            Q = new Spell(SpellSlot.Q, 1250);
            Q.SetSkillshot(0.25f, 90f, 1350f, false, SkillshotType.SkillshotLine);

            W = new Spell(SpellSlot.W, 1050);
            E = new Spell(SpellSlot.E);
            
            Obj_AI_Base.OnProcessSpellCast += Game_OnProcessSpellCast;
        }

        public override void Game_OnGameUpdate(EventArgs args)
        {
            if (GetValue<bool>("AutoQ"))
            {
                foreach (var enemy in ObjectManager.Get<Obj_AI_Hero>().Where(h => h.IsValidTarget(Q.Range)))
                {
                    Q.CastIfHitchanceEquals(enemy, HitChance.Immobile);
                }
            }


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
            Spell[] spellList = { Q };
            foreach (var spell in spellList)
            {
                var menuItem = GetValue<Circle>("Draw" + spell.Slot);
                if (menuItem.Active)
                    Utility.DrawCircle(ObjectManager.Player.Position, spell.Range, menuItem.Color);
            }
        }

        public override bool ComboMenu(Menu config)
        {
            config.AddItem(new MenuItem("UseQC" + Id, "Use Q").SetValue(true));
            config.AddItem(new MenuItem("UseWC" + Id, "Use W").SetValue(true));
            return true;
        }

        public override bool HarassMenu(Menu config)
        {
            config.AddItem(new MenuItem("UseQH" + Id, "Use Q").SetValue(false));
            config.AddItem(new MenuItem("UseWH" + Id, "Use W").SetValue(false));
            return true;
        }

        public override bool MiscMenu(Menu config)
        {
            config.AddItem(new MenuItem("AutoQ" + Id, "Auto Q on stunned targets").SetValue(true));

            MenuSuppSpells = new Menu("Can be blocked spells", "menuSuppSpells");
            foreach (var enemy in ObjectManager.Get<Obj_AI_Hero>().Where(enemy => enemy.IsEnemy))
            {
                foreach (var ccList in SpellList.CCList.Where(ccList => ccList.HeroName == enemy.SkinName))
                {
                    MenuSuppSpells.AddItem(new MenuItem(ccList.SDataName,
                        enemy.SkinName + " " + ccList.SpellMenuName)).SetValue(true);
                }
            }
            Config.AddSubMenu(MenuSuppSpells);
            return true;
        }

        public override bool DrawingMenu(Menu config)
        {
            config.AddItem(
                new MenuItem("DrawQ" + Id, "Q range").SetValue(new Circle(true, Color.FromArgb(100, 255, 0, 255))));
            return true;
        }

        public override bool ExtrasMenu(Menu config)
        {

            return true;
        }

        private void Game_OnProcessSpellCast(Obj_AI_Base sender, GameObjectProcessSpellCastEventArgs args)
        {
            if (sender.Type != GameObjectType.obj_AI_Hero || !sender.IsEnemy) 
                return;
            
            foreach (var spell in from t in MenuSuppSpells.Items
                from spell in SpellList.CCList
                    where t.Name == args.SData.Name && t.Name == spell.SDataName && t.GetValue<bool>()
                select spell) 
            {
                switch (spell.Type)
                {
                    case Skilltype.Circle:
                        if (ObjectManager.Player.Distance(args.End) <= 250f)
                        {
                            if (E.IsReady())
                                E.Cast();
                        }
                        break;
                    case Skilltype.Line:
                        if (ObjectManager.Player.Distance(args.End) <= 100f)
                        {
                            if (E.IsReady())
                                E.Cast();
                        }
                        break;
                    case Skilltype.Unknown:
                        if (ObjectManager.Player.Distance(args.End) <= 500f ||
                            ObjectManager.Player.Distance(sender.Position) <= 500)
                        {
                            if (E.IsReady())
                                E.Cast();
                        }
                        break;
                }
            }
        }
    }
}
