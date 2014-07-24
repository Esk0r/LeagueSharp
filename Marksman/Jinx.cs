#region

using System;
using LeagueSharp;
using LeagueSharp.Common;
using SharpDX;
using Color = System.Drawing.Color;
#endregion

namespace Marksman
{
    internal class Jinx : Champion
    {
        public Spell W;
        public Spell E;
        public Spell R;

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
            Drawing.OnDraw += Drawing_OnDraw;
        }


        private void Game_OnGameUpdate(EventArgs args)
        {
            if (ComboActive || HarassActive)
            {
                var useW = GetValue<bool>("UseW" + (ComboActive ? "C" : "H"));

                if (Orbwalking.CanMove(100))
                {
                    if (W.IsReady() && useW)
                    {
                        var t = Orbwalker.GetTarget() ?? SimpleTs.GetTarget(W.Range, SimpleTs.DamageType.Physical);

                        if (t != null)
                        {
                            W.Cast(t);
                        }
                    }
                }
            }

            

            var AutoEI = GetValue<bool>("AutoEI");
            var AutoED = GetValue<bool>("AutoED");
            if (AutoED || AutoEI)
            {
                foreach (var enemy in ObjectManager.Get<Obj_AI_Hero>())
                {
                    if (enemy.IsValidTarget())
                    {
                        if(AutoEI)
                            E.CastIfHitchanceEquals(enemy, Prediction.HitChance.Immobile);

                        if (AutoED)
                            E.CastIfHitchanceEquals(enemy, Prediction.HitChance.Immobile);

                    }
                }
            }

            if (R.IsReady())
            {
                var castR = GetValue<KeyBind>("CastR").Active;
                if (castR || (Orbwalker.GetTarget() != null && GetValue<bool>("UseRC") && DamageLib.getDmg(Orbwalker.GetTarget(), DamageLib.SpellType.R, DamageLib.StageType.FirstDamage) > Orbwalker.GetTarget().Health))
                {
                    var t = SimpleTs.GetTarget(R.Range, SimpleTs.DamageType.Physical);
                    if (t != null)
                    {
                        R.Cast(t);
                    }
                }
            }
        }


        void Drawing_OnDraw(EventArgs args)
        {
            Spell[] SpellList = new[] { W };
            foreach (var spell in SpellList)
            {
                var menuItem = GetValue<Circle>("Draw" + spell.Slot);
                if (menuItem.Active)
                {
                    Utility.DrawCircle(ObjectManager.Player.Position, spell.Range, menuItem.Color);
                }
            }
        }

        public override void ComboMenu(Menu config)
        {
            config.AddItem(new MenuItem("UseWC" + Id, "Use W").SetValue(true));
            config.AddItem(new MenuItem("UseRC" + Id, "Use R").SetValue(true));
        }

        public override void HarassMenu(Menu config)
        {
            config.AddItem(new MenuItem("UseWH" + Id, "Use W").SetValue(true));
        }

        public override void MiscMenu(Menu config)
        {
            config.AddItem(new MenuItem("CastR" + Id, "Cast R (2000 Range)").SetValue(new KeyBind("T".ToCharArray()[0], KeyBindType.Press)));
            config.AddItem(new MenuItem("AutoEI" + Id, "Auto-E on immobile").SetValue(true));
            config.AddItem(new MenuItem("AutoED" + Id, "Auto-E on dashing").SetValue(true));
        }

        public override void DrawingMenu(Menu config)
        {
            config.AddItem(new MenuItem("DrawW" + Id, "W range").SetValue(new Circle(false, Color.FromArgb(100, 255, 255, 255))));
        }
    }
}