#region

using System;
using LeagueSharp;
using LeagueSharp.Common;
using SharpDX;
using Color = System.Drawing.Color;
#endregion

namespace Marksman
{
    internal class Ezreal : Champion
    {
        public Spell Q;
        public Spell R;
        public Spell W;


        public Ezreal()
        {
            Utils.PrintMessage("Ezreal loaded.");

            Q = new Spell(SpellSlot.Q, 1200);
            Q.SetSkillshot(0.25f, 60f, 2000f, true, Prediction.SkillshotType.SkillshotLine);

            W = new Spell(SpellSlot.W, 1050);
            W.SetSkillshot(0.25f, 80f, 1600f, false, Prediction.SkillshotType.SkillshotLine);

            R = new Spell(SpellSlot.R, 2500);
            R.SetSkillshot(1f, 160f, 2000f, false, Prediction.SkillshotType.SkillshotLine);

            Orbwalking.AfterAttack += Orbwalking_AfterAttack;
            Game.OnGameUpdate += Game_OnGameUpdate;
            Drawing.OnDraw += Drawing_OnDraw;
        }

        private void Game_OnGameUpdate(EventArgs args)
        {
            if (ComboActive || HarassActive)
            {
                var useQ = GetValue<bool>("UseQ" + (ComboActive ? "C" : "H"));
                var useW = GetValue<bool>("UseW" + (ComboActive ? "C" : "H"));

                if (Orbwalking.CanMove(100) )
                {
                    if (Q.IsReady() && useQ)
                    {
                        var t = SimpleTs.GetTarget(Q.Range, SimpleTs.DamageType.Physical);
                        if (t != null)
                        {
                            Q.Cast(t);
                        }
                    }

                    if (W.IsReady() && useW)
                    {
                        var t = SimpleTs.GetTarget(W.Range, SimpleTs.DamageType.Physical);
                        if (t != null)
                        {
                            W.Cast(t);
                        }
                    }
                }
            }

            if (R.IsReady())
            {
                var CastR = GetValue<KeyBind>("CastR").Active;
                if (CastR)
                {
                    var t = SimpleTs.GetTarget(R.Range, SimpleTs.DamageType.Physical);
                    if (t != null)
                    {
                        R.Cast(t);
                    }
                }
            }
            
        }

        private void Orbwalking_AfterAttack(Obj_AI_Base unit, Obj_AI_Base target)
        {
            if ((ComboActive || HarassActive) && unit.IsMe)
            {
                var useQ = GetValue<bool>("UseQ" + (ComboActive ? "C" : "H"));
                var useW = GetValue<bool>("UseW" + (ComboActive ? "C" : "H"));

                if (Q.IsReady() && useQ)
                {
                    Q.Cast(target);
                }
                else if (W.IsReady() && useW)
                {
                    W.Cast(target);
                }
            }
        }

        void Drawing_OnDraw(EventArgs args)
        {
            Spell[] SpellList = new[] {Q, W};
            foreach (var spell in SpellList)
            {
                var menuItem =GetValue<Circle>("Draw" + spell.Slot);
                if (menuItem.Enabled)
                {
                   Utility.DrawCircle(ObjectManager.Player.Position, spell.Range, menuItem.Color);
                }
            }
        }

        public override void ComboMenu(Menu config)
        {
            config.AddItem(new MenuItem("UseQC" + Id, "Use Q").SetValue(true));
            config.AddItem(new MenuItem("UseWC" + Id, "Use W").SetValue(true));
        }

        public override void HarassMenu(Menu config)
        {
            config.AddItem(new MenuItem("UseQH" + Id, "Use Q").SetValue(true));
            config.AddItem(new MenuItem("UseWH" + Id, "Use W").SetValue(true));
        }

        public override void MiscMenu(Menu config)
        {
            config.AddItem(new MenuItem("CastR" + Id, "Cast R (2000 Range)").SetValue(new KeyBind("T".ToCharArray()[0], KeyBindType.Press)));
        }

        public override void DrawingMenu(Menu config)
        {
            config.AddItem(new MenuItem("DrawQ" + Id, "Q range").SetValue(new Circle(true, Color.FromArgb(100,255,0,255))));
            config.AddItem(new MenuItem("DrawW" + Id, "W range").SetValue(new Circle(false, Color.FromArgb(100,255,255,255))));
        }
    }
}