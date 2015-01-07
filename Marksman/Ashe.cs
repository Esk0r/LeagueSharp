#region
using System;
using LeagueSharp;
using LeagueSharp.Common;
using SharpDX;
using Color = System.Drawing.Color;
#endregion

namespace Marksman
{
    internal class Ashe : Champion
    {
        public static Spell Q;
        public static Spell W;
        public static Spell E;
        public static Spell R;

        public Ashe()
        {
            Q = new Spell(SpellSlot.Q);
            W = new Spell(SpellSlot.W, 1200);
            E = new Spell(SpellSlot.E, 2500);
            R = new Spell(SpellSlot.R, 20000);

            W.SetSkillshot(250f, (float) (24.32f*Math.PI/180), 902f, true, SkillshotType.SkillshotCone);
            E.SetSkillshot(377f, 299f, 1400f, false, SkillshotType.SkillshotLine);
            R.SetSkillshot(250f, 130f, 1600f, false, SkillshotType.SkillshotLine);
            
            Interrupter.OnPossibleToInterrupt += Game_OnPossibleToInterrupt;
            Obj_AI_Base.OnProcessSpellCast += Game_OnProcessSpell;
            
            Utils.PrintMessage("Ashe loaded.");
        }

        public void Game_OnPossibleToInterrupt(Obj_AI_Base unit, InterruptableSpell spell)
        {
            if (R.IsReady() && Config.Item("RInterruptable" + Id).GetValue<bool>() && unit.IsValidTarget(1500))
            {
                R.Cast(unit);
            }
        }

        public void Game_OnProcessSpell(Obj_AI_Base unit, GameObjectProcessSpellCastEventArgs spell)
        {
            if (!Config.Item("EFlash" + Id).GetValue<bool>() || unit.Team == ObjectManager.Player.Team) 
                return;

            if (spell.SData.Name.ToLower() == "summonerflash")
                E.Cast(spell.End);
        }

        public override void Game_OnGameUpdate(EventArgs args)
        {
            if (W.IsReady() && GetValue<KeyBind>("UseWTH").Active)
            {
                if(ObjectManager.Player.HasBuff("Recall"))
                    return;
                var t = TargetSelector.GetTarget(W.Range, TargetSelector.DamageType.Physical);
                if (t != null)
                    W.Cast(t);
            }
            //Combo
            if (ComboActive)
            {
                var target = TargetSelector.GetTarget(1200, TargetSelector.DamageType.Physical);
                if (target == null) return;

                if (!IsQActive && Config.Item("UseQC" + Id).GetValue<bool>())
                    Q.Cast();

                if (Config.Item("UseWC" + Id).GetValue<bool>() && W.IsReady())
                    W.Cast(target);

                if (Config.Item("UseRC" + Id).GetValue<bool>() && R.IsReady())
                {
                    var rTarget = TargetSelector.GetTarget(1500, TargetSelector.DamageType.Physical);

                    if (!rTarget.IsValidTarget() ||
                        !(ObjectManager.Player.GetSpellDamage(rTarget, SpellSlot.R) > rTarget.Health)) return;

                    R.Cast(rTarget);
                }
            }

            //Harass
            if (HarassActive)
            {
                var target = TargetSelector.GetTarget(1200, TargetSelector.DamageType.Physical);
                if (target == null) return;

                if (!IsQActive && Config.Item("UseQH" + Id).GetValue<bool>())
                    Q.Cast();

                if (Config.Item("UseWH" + Id).GetValue<bool>() && W.IsReady())
                    W.Cast(target);
            }

            //Manual cast R
            if (Config.Item("RManualCast" + Id).GetValue<KeyBind>().Active)
            {
                var rTarget = TargetSelector.GetTarget(2000, TargetSelector.DamageType.Physical);
                R.Cast(rTarget);
            }
        }

        public override bool ComboMenu(Menu config)
        {
            config.AddItem(new MenuItem("UseQC" + Id, "Use Q").SetValue(true));
            config.AddItem(new MenuItem("UseWC" + Id, "Use W").SetValue(true));
            config.AddItem(new MenuItem("UseRC" + Id, "Use R").SetValue(true));
            return true;
        }

        public override bool HarassMenu(Menu config)
        {
            config.AddItem(new MenuItem("UseQH" + Id, "Use Q").SetValue(true));
            config.AddItem(new MenuItem("UseWH" + Id, "Use W").SetValue(true));
            config.AddItem(
                new MenuItem("UseWTH" + Id, "Use W (Toggle)").SetValue(new KeyBind("H".ToCharArray()[0],
                    KeyBindType.Toggle)));
            return true;
        }

        public override bool LaneClearMenu(Menu config)
        {

            return true;
        }

        public override bool DrawingMenu(Menu config)
        {
            config.AddItem(
                new MenuItem("DrawW" + Id, "W range").SetValue(new Circle(true, Color.CornflowerBlue)));
            config.AddItem(
                new MenuItem("DrawE" + Id, "E range").SetValue(new Circle(true, Color.FromArgb(100, 255, 0, 255))));
            return true;
        }

        public override bool MiscMenu(Menu config)
        {
            config.AddItem(new MenuItem("RInterruptable" + Id, "Auto R Interruptable Spells").SetValue(true));
            config.AddItem(new MenuItem("EFlash" + Id, "Use E against Flashes").SetValue(true));
            config.AddItem(new MenuItem("RManualCast" + Id, "Cast R Manually(2000 range)"))
                .SetValue(new KeyBind('T', KeyBindType.Press));
            return true;
        }

        public bool IsQActive
        {
            get
            {
                return ObjectManager.Player.HasBuff("FrostShot");
            }
        }

        public override void Drawing_OnDraw(EventArgs args)
        {
            var drawW = Config.Item("DrawW" + Id).GetValue<Circle>();
            if (drawW.Active)
            {
                Render.Circle.DrawCircle(ObjectManager.Player.Position, W.Range, drawW.Color);
            }

            var drawE = Config.Item("DrawE" + Id).GetValue<Circle>();
            if (drawE.Active)
            {
                Render.Circle.DrawCircle(ObjectManager.Player.Position, E.Range, drawE.Color);
            }
        }
        public override bool ExtrasMenu(Menu config)
        {

            return true;
        }

    }
}
