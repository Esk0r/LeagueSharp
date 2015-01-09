#region

using System;
using LeagueSharp;
using LeagueSharp.Common;
using SharpDX;
using Color = System.Drawing.Color;

#endregion

namespace Marksman
{
    internal class Caitlyn : Champion // Base done by xQx, Drawings and improvements added by Dibes.
    {
        public Spell Q;
        public Spell W;
        public Spell E;
        public Spell R;

        public bool ShowUlt;
        public string UltTarget;

        public Caitlyn()
        {
            Utils.PrintMessage("Caitlyn loaded.");

            Q = new Spell(SpellSlot.Q, 1240);
            W = new Spell(SpellSlot.W, 820);
            E = new Spell(SpellSlot.E, 800);
            R = new Spell(SpellSlot.R, 2000);

            Q.SetSkillshot(0.25f, 60f, 2000f, false, SkillshotType.SkillshotLine);
            E.SetSkillshot(0.25f, 80f, 1600f, true, SkillshotType.SkillshotLine);

            AntiGapcloser.OnEnemyGapcloser += AntiGapcloser_OnEnemyGapcloser;
        }

        public void AntiGapcloser_OnEnemyGapcloser(ActiveGapcloser gapcloser)
        {
            if (E.IsReady() && gapcloser.Sender.IsValidTarget(E.Range))
                E.CastOnUnit(gapcloser.Sender);
        }

        public override void Drawing_OnDraw(EventArgs args)
        {
            Spell[] spellList = { Q, E, R };
            foreach (var spell in spellList)
            {
                var menuItem = GetValue<Circle>("Draw" + spell.Slot);
                if (menuItem.Active)
                    Render.Circle.DrawCircle(ObjectManager.Player.Position, spell.Range, menuItem.Color);
            }

            var drawUlt = GetValue<Circle>("DrawUlt");

            if (drawUlt.Active && ShowUlt)
            {
                //var playerPos = Drawing.WorldToScreen(ObjectManager.Player.Position);
                //Drawing.DrawText(playerPos.X - 65, playerPos.Y + 20, drawUlt.Color, "Hit R To kill " + UltTarget + "!");
            }
        }

        public override void Game_OnGameUpdate(EventArgs args)
        {
            R.Range = 500 * R.Level + 1500;

            Obj_AI_Hero t;
            
            if (W.IsReady() && GetValue<bool>("AutoWI"))
            {
                t = TargetSelector.GetTarget(W.Range, TargetSelector.DamageType.Physical);
                if (t.IsValidTarget(W.Range) &&
                    (t.HasBuffOfType(BuffType.Stun) || t.HasBuffOfType(BuffType.Snare) ||
                    t.HasBuffOfType(BuffType.Taunt) || t.HasBuff("zhonyasringshield") ||
                    t.HasBuff("Recall")))
                {
                    W.Cast(t.Position);
                }                
            }

            if (Q.IsReady() && GetValue<bool>("AutoQI"))
            {
                t = TargetSelector.GetTarget(Q.Range, TargetSelector.DamageType.Physical);
                if (t.IsValidTarget(Q.Range) &&
                    (t.HasBuffOfType(BuffType.Stun) || t.HasBuffOfType(BuffType.Snare) ||
                     t.HasBuffOfType(BuffType.Taunt) || t.HasBuffOfType(BuffType.Slow))) 
                {
                    Q.Cast(t, false, true);
                }
            }

            if (R.IsReady())
            {
                t = TargetSelector.GetTarget(R.Range, TargetSelector.DamageType.Physical);
                if (t.IsValidTarget(R.Range) && t.Health <= R.GetDamage(t))
                {
                    if (GetValue<KeyBind>("UltHelp").Active)
                        R.Cast(t);

                    UltTarget = t.ChampionName;
                    ShowUlt = true;
                }
                else
                {
                    ShowUlt = false;
                }
            }
            else
            {
                ShowUlt = false;
            }

            if (GetValue<KeyBind>("Dash").Active && E.IsReady())
            {
                var pos = ObjectManager.Player.ServerPosition.To2D().Extend(Game.CursorPos.To2D(), - 300).To3D();
                E.Cast(pos, true);
            }

            if (GetValue<KeyBind>("UseEQC").Active && E.IsReady() && Q.IsReady())
            {
                t = TargetSelector.GetTarget(E.Range, TargetSelector.DamageType.Physical);
                if (t.IsValidTarget(E.Range))
                {
                    E.Cast(t);
                    Q.Cast(t, false, true);
                }
            }
            // PQ you broke it D:
            if ((!ComboActive && !HarassActive) || !Orbwalking.CanMove(100)) return;

            var useQ = GetValue<bool>("UseQ" + (ComboActive ? "C" : "H"));
            var useE = GetValue<bool>("UseEC");
            var useR = GetValue<bool>("UseRC");

            if (Q.IsReady() && useQ)
            {
                t = TargetSelector.GetTarget(Q.Range, TargetSelector.DamageType.Physical);
                if (t != null)
                    Q.Cast(t, false, true);
            }
            else if (E.IsReady() && useE)
            {
                t = TargetSelector.GetTarget(E.Range, TargetSelector.DamageType.Physical);
                if (t != null && t.Health <= E.GetDamage(t))
                    E.Cast(t);
            }

            if (R.IsReady() && useR)
            {
                t = TargetSelector.GetTarget(R.Range, TargetSelector.DamageType.Physical);
                if (t != null && t.Health <= R.GetDamage(t) &&
                    !Orbwalking.InAutoAttackRange(t))
                {
                    R.CastOnUnit(t);
                }
            }
        }

        public override void Orbwalking_AfterAttack(AttackableUnit unit, AttackableUnit target)
        {
            var t = target as Obj_AI_Hero;
            if (t != null || (!ComboActive && !HarassActive) || unit.IsMe) 
                return;

            var useQ = GetValue<bool>("UseQ" + (ComboActive ? "C" : "H"));
            if (useQ)
                Q.Cast(t, false, true);
        }

        public override bool ComboMenu(Menu config)
        {
            config.AddItem(new MenuItem("UseQC" + Id, "Use Q").SetValue(true));
            config.AddItem(new MenuItem("UseEC" + Id, "Use E").SetValue(true));
            config.AddItem(new MenuItem("UseRC" + Id, "Use R").SetValue(true));
            return true;
        }

        public override bool HarassMenu(Menu config)
        {
            config.AddItem(new MenuItem("UseQH" + Id, "Use Q").SetValue(true));
            return true;
        }

        public override bool DrawingMenu(Menu config)
        {
            config.AddItem(
                new MenuItem("DrawQ" + Id, "Q range").SetValue(new Circle(true, Color.FromArgb(100, 255, 0, 255))));
            config.AddItem(
                new MenuItem("DrawE" + Id, "E range").SetValue(new Circle(false, Color.FromArgb(100, 255, 255, 255))));
            config.AddItem(
                new MenuItem("DrawR" + Id, "R range").SetValue(new Circle(false, Color.FromArgb(100, 255, 255, 255))));
            config.AddItem(
                new MenuItem("DrawUlt" + Id, "Ult Text").SetValue(new Circle(true, Color.FromArgb(255, 255, 255, 255))));
            return true;
        }

        public override bool MiscMenu(Menu config)
        {
            config.AddItem(
                new MenuItem("UltHelp" + Id, "Ult Target on R").SetValue(new KeyBind("R".ToCharArray()[0],
                    KeyBindType.Press)));
            config.AddItem(
                new MenuItem("UseEQC" + Id, "Use E-Q Combo").SetValue(new KeyBind("T".ToCharArray()[0],
                    KeyBindType.Press)));
            config.AddItem(
                new MenuItem("Dash" + Id, "Dash to Mouse").SetValue(new KeyBind("Z".ToCharArray()[0], KeyBindType.Press)));
            return true;
        }

        public override bool ExtrasMenu(Menu config)
        {
            config.AddItem(new MenuItem("AutoQI" + Id, "Auto Q (Stun/Snare/Taunt/Slow)").SetValue(true));
            config.AddItem(new MenuItem("AutoWI" + Id, "Auto W (Stun/Snare/Taunt)").SetValue(true));
            return true;
        }
        public override bool LaneClearMenu(Menu config)
        {

             return true;
        }
    }
}
