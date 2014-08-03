#region

using System;
using System.Drawing;
using System.Linq;
using LeagueSharp;
using LeagueSharp.Common;

#endregion

namespace Marksman
{
    internal class Jinx : Champion
    {
        public Spell E;
        public Spell R;
        public Spell W;

        public Jinx()
        {
            Utils.PrintMessage("Jinx loaded.");

            W = new Spell(SpellSlot.W, 1500);
            W.SetSkillshot(0.6f, 60f, 3000f, true, Prediction.SkillshotType.SkillshotLine);

            E = new Spell(SpellSlot.E, 1200);
            E.SetSkillshot(0.6f, 140f, 1700f, true, Prediction.SkillshotType.SkillshotCircle);

            R = new Spell(SpellSlot.R, 2500);
            R.SetSkillshot(0.6f, 140f, 1700f, false, Prediction.SkillshotType.SkillshotLine);

            Game.OnGameUpdate += Game_OnGameUpdate;
        }

        public override void Drawing_OnDraw(EventArgs args)
        {
            Spell[] spellList = { W };
            foreach (var spell in spellList)
            {
                var menuItem = GetValue<Circle>("Draw" + spell.Slot);
                if (menuItem.Active)
                    Utility.DrawCircle(ObjectManager.Player.Position, spell.Range, menuItem.Color);
            }
        }

        private void Game_OnGameUpdate(EventArgs args)
        {
            if (ComboActive || HarassActive)
            {
                var useW = GetValue<bool>("UseW" + (ComboActive ? "C" : "H"));

                if (Orbwalking.CanMove(100) && useW && W.IsReady())
                {
                    var t = Orbwalker.GetTarget() ?? SimpleTs.GetTarget(W.Range, SimpleTs.DamageType.Physical);

                    if (t != null)
                        W.Cast(t);
                }
            }


            var autoEi = GetValue<bool>("AutoEI");
            var autoEd = GetValue<bool>("AutoED");
            if (autoEd || autoEi)
            {
                foreach (var enemy in ObjectManager.Get<Obj_AI_Hero>().Where(enemy => enemy.IsValidTarget(E.Range)))
                {
                    if (autoEi)
                        E.CastIfHitchanceEquals(enemy, Prediction.HitChance.Immobile);

                    if (autoEd)
                        E.CastIfHitchanceEquals(enemy, Prediction.HitChance.Immobile);
                }
            }

            if (R.IsReady())
            {
                var castR = GetValue<KeyBind>("CastR").Active;
                var target = SimpleTs.GetTarget(1500, SimpleTs.DamageType.Physical);
                if (target != null)
                    if (castR ||
                        (GetValue<bool>("UseRC") &&
                         DamageLib.getDmg(target, DamageLib.SpellType.R, DamageLib.StageType.FirstDamage) >
                         target.Health))
                        R.Cast(target);
            }
        }

        public override void ComboMenu(Menu config)
        {
            config.AddItem(new MenuItem("UseWC" + Id, "Use W").SetValue(true));
            config.AddItem(new MenuItem("UseRC" + Id, "Use R").SetValue(true));
        }

        public override void HarassMenu(Menu config)
        {
            config.AddItem(new MenuItem("UseWH" + Id, "Use W").SetValue(false));
        }

        public override void MiscMenu(Menu config)
        {
            config.AddItem(
                new MenuItem("CastR" + Id, "Cast R (2000 Range)").SetValue(new KeyBind("T".ToCharArray()[0],
                    KeyBindType.Press)));
            config.AddItem(new MenuItem("AutoEI" + Id, "Auto-E on immobile").SetValue(true));
            config.AddItem(new MenuItem("AutoED" + Id, "Auto-E on dashing").SetValue(true));
        }

        public override void DrawingMenu(Menu config)
        {
            config.AddItem(
                new MenuItem("DrawW" + Id, "W range").SetValue(new Circle(false, Color.FromArgb(100, 255, 255, 255))));
        }
    }
}