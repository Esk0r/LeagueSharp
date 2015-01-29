#region
using System;
using System.Drawing;
using System.Linq;
using LeagueSharp;
using SharpDX.Direct3D9;
using Font = SharpDX.Direct3D9.Font;
using LeagueSharp.Common;
#endregion

namespace Marksman
{

    internal class Tristana : Champion
    {
        public static Spell Q, W, E, R;
        public static Font vText;
        public static int xBuffCount;
        public static int LastTickTime;

        public Tristana()
        {
            Q = new Spell(SpellSlot.Q, 703);

            W = new Spell(SpellSlot.W, 900);
            W.SetSkillshot(.50f, 250f, 1400f, false, SkillshotType.SkillshotCircle);

            E = new Spell(SpellSlot.E, 703);
            R = new Spell(SpellSlot.R, 703);

            Utility.HpBarDamageIndicator.DamageToUnit = GetComboDamage;
            Utility.HpBarDamageIndicator.Enabled = true;

            AntiGapcloser.OnEnemyGapcloser += AntiGapcloser_OnEnemyGapcloser;
            Interrupter.OnPossibleToInterrupt += Interrupter_OnPossibleToInterrupt;
            vText = new Font(
                Drawing.Direct3DDevice,
                new FontDescription
                {
                    FaceName = "Courier new",
                    Height = 15,
                    OutputPrecision = FontPrecision.Default,
                    Quality = FontQuality.Default,
                });

            Utils.PrintMessage("Tristana loaded.");
        }

        public void AntiGapcloser_OnEnemyGapcloser(ActiveGapcloser gapcloser)
        {
            if (R.IsReady() && gapcloser.Sender.IsValidTarget(R.Range) && GetValue<bool>("UseRMG"))
                R.CastOnUnit(gapcloser.Sender);
        }

        public void Interrupter_OnPossibleToInterrupt(Obj_AI_Base unit, InterruptableSpell spell)
        {
            if (R.IsReady() && unit.IsValidTarget(R.Range) && GetValue<bool>("UseRMI"))
                R.CastOnUnit(unit);
        }

        public Obj_AI_Hero GetEMarkedEnemy
        {
            get
            {
                return
                    ObjectManager.Get<Obj_AI_Hero>()
                        .Where(
                            enemy =>
                                !enemy.IsDead &&
                                enemy.IsValidTarget(W.Range + Orbwalking.GetRealAutoAttackRange(ObjectManager.Player)))
                        .FirstOrDefault(enemy => enemy.Buffs.Any(buff => buff.DisplayName == "TristanaEChargeSound"));
            }
        }

        public int GetEMarkedCount
        {
            get
            {
                return
                    GetEMarkedEnemy.Buffs.Where(buff => buff.DisplayName == "TristanaECharge")
                        .Select(xBuff => xBuff.Count)
                        .FirstOrDefault();
            }
        }

        public override void Orbwalking_AfterAttack(AttackableUnit unit, AttackableUnit target)
        {
            var t = target as Obj_AI_Hero;
            if (t != null && (ComboActive || HarassActive) && unit.IsMe)
            {
                var useQ = GetValue<bool>("UseQ" + (ComboActive ? "C" : "H"));
                var useE = GetValue<bool>("UseE" + (ComboActive ? "C" : "H"));

                if (useQ && Q.IsReady())
                    Q.CastOnUnit(ObjectManager.Player);

                if (useE && E.IsReady())
                    E.CastOnUnit(t);
            }
        }


        public override void Game_OnGameUpdate(EventArgs args)
        {
            if (!Orbwalking.CanMove(100))
                return;
            var getEMarkedEnemy = GetEMarkedEnemy;
            if (getEMarkedEnemy != null)
            {
                TargetSelector.SetTarget(getEMarkedEnemy);
            }
            else
            {
                var attackRange = Orbwalking.GetRealAutoAttackRange(ObjectManager.Player);
                TargetSelector.SetTarget(TargetSelector.GetTarget(attackRange, TargetSelector.DamageType.Physical));
            }

            //Update Q range depending on level; 600 + 5 Ã— ( Tristana's level - 1)/* dont waste your Q for only 1 or 2 hits. */
            //Update E and R range depending on level; 630 + 9 Ã— ( Tristana's level - 1)
            Q.Range = 600 + 5 * (ObjectManager.Player.Level - 1);
            E.Range = 630 + 9 * (ObjectManager.Player.Level - 1);
            R.Range = 630 + 9 * (ObjectManager.Player.Level - 1);

            if (GetValue<KeyBind>("UseETH").Active)
            {
                if (ObjectManager.Player.HasBuff("Recall"))
                    return;
                var eTarget = TargetSelector.GetTarget(E.Range, TargetSelector.DamageType.Physical);
                if (E.IsReady() && eTarget.IsValidTarget())
                    E.CastOnUnit(eTarget);
            }

            if (ComboActive || HarassActive)
            {
                Obj_AI_Hero t;
                if (GetEMarkedEnemy != null)
                {
                    t = GetEMarkedEnemy;
                    TargetSelector.SetTarget(GetEMarkedEnemy);
                }
                else
                {
                    t = TargetSelector.GetTarget(W.Range, TargetSelector.DamageType.Physical);
                }

                var useE = GetValue<bool>("UseE" + (ComboActive ? "C" : "H"));
                if (useE)
                {
                    if (E.IsReady() && t.IsValidTarget(E.Range))
                        E.CastOnUnit(t);
                }

                var useW = GetValue<bool>("UseW" + (ComboActive ? "C" : "H"));
                if (useW)
                {
                    t = TargetSelector.GetTarget(W.Range, TargetSelector.DamageType.Physical);
                    if (t.IsValidTarget() && W.IsReady() && GetEMarkedCount == 4 && !t.UnderTurret() &&
                        ObjectManager.Player.Distance(t) > Orbwalking.GetRealAutoAttackRange(t))
                    {
                        W.Cast(t);
                    }
                }
            }

            //Killsteal
            if (!ComboActive)
                return;

            foreach (var hero in
                ObjectManager.Get<Obj_AI_Hero>()
                    .Where(hero => hero.IsValidTarget(R.Range) || hero.IsValidTarget(W.Range)))
            {
                if (GetValue<bool>("UseWM") && W.IsReady() &&
                    ObjectManager.Player.GetSpellDamage(hero, SpellSlot.W) - 10 > hero.Health)
                    W.Cast(hero);
                else if (GetValue<bool>("UseRM") && R.IsReady() &&
                         ObjectManager.Player.GetSpellDamage(hero, SpellSlot.R) - 30 > hero.Health)
                    R.CastOnUnit(hero);
            }
        }

        private static float GetComboDamage(Obj_AI_Hero t)
        {
            var fComboDamage = 0f;

            if (E.IsReady())
                fComboDamage += (float) ObjectManager.Player.GetSpellDamage(t, SpellSlot.E);

            if (R.IsReady())
                fComboDamage += (float) ObjectManager.Player.GetSpellDamage(t, SpellSlot.R);

            if (ObjectManager.Player.GetSpellSlot("summonerdot") != SpellSlot.Unknown &&
                ObjectManager.Player.Spellbook.CanUseSpell(ObjectManager.Player.GetSpellSlot("summonerdot")) ==
                SpellState.Ready && ObjectManager.Player.Distance(t) < 550)
                fComboDamage += (float) ObjectManager.Player.GetSummonerSpellDamage(t, Damage.SummonerSpell.Ignite);

            if (Items.CanUseItem(3144) && ObjectManager.Player.Distance(t) < 550)
                fComboDamage += (float) ObjectManager.Player.GetItemDamage(t, Damage.DamageItems.Bilgewater);

            if (Items.CanUseItem(3153) && ObjectManager.Player.Distance(t) < 550)
                fComboDamage += (float) ObjectManager.Player.GetItemDamage(t, Damage.DamageItems.Botrk);

            return fComboDamage;
        }

        public override void Drawing_OnDraw(EventArgs args)
        {
            // Draw marked enemy status
            var drawEMarksStatus = Program.Config.SubMenu("Drawings").Item("DrawEMarkStatus").GetValue<bool>();
            var drawEMarkEnemy = Program.Config.SubMenu("Drawings").Item("DrawEMarkEnemy").GetValue<Circle>();
            if (drawEMarksStatus || drawEMarkEnemy.Active)
            {
                var getEMarkedEnemy = GetEMarkedEnemy;
                if (getEMarkedEnemy != null)
                {
                    if (drawEMarksStatus)
                    {
                        if (LastTickTime < Environment.TickCount)
                            LastTickTime = Environment.TickCount + 5000;
                        var xTime = LastTickTime - Environment.TickCount;

                        xBuffCount = GetEMarkedCount;
                        var timer = string.Format("0:{0:D2}", xTime / 1000);
                        Utils.DrawText(
                            vText, timer + " : 4 / " + xBuffCount, (int) getEMarkedEnemy.HPBarPosition.X + 145,
                            (int) getEMarkedEnemy.HPBarPosition.Y + 5, SharpDX.Color.White);
                    }

                    if (drawEMarkEnemy.Active)
                    {
                        Render.Circle.DrawCircle(GetEMarkedEnemy.Position, 140f, drawEMarkEnemy.Color, 1);
                    }
                }
            }

            // draw W spell
            Spell[] spellList = { W };
            foreach (var spell in spellList)
            {
                var menuItem = GetValue<Circle>("Draw" + spell.Slot);
                if (menuItem.Active)
                    Render.Circle.DrawCircle(ObjectManager.Player.Position, spell.Range, menuItem.Color, 1);
            }

            // draw E spell
            var drawE = Program.Config.SubMenu("Drawings").Item("DrawE").GetValue<Circle>();
            if (drawE.Active)
            {
                Render.Circle.DrawCircle(ObjectManager.Player.Position, E.Range, drawE.Color, 1);
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
            config.AddItem(new MenuItem("UseQH" + Id, "Use Q").SetValue(false));
            config.AddItem(new MenuItem("UseWH" + Id, "Use W").SetValue(true));
            config.AddItem(new MenuItem("UseEH" + Id, "Use E").SetValue(true));
            config.AddItem(
                new MenuItem("UseETH" + Id, "Use E (Toggle)").SetValue(
                    new KeyBind("H".ToCharArray()[0], KeyBindType.Toggle)));
            return true;
        }

        public override bool DrawingMenu(Menu config)
        {
            config.AddItem(new MenuItem("DrawW" + Id, "W range").SetValue(new Circle(true, Color.Beige)));

            var drawE = new Menu("Draw E", "DrawE");
            {
                drawE.AddItem(new MenuItem("DrawE", "E range").SetValue(new Circle(true, Color.Beige)));
                drawE.AddItem(
                    new MenuItem("DrawEMarkEnemy", "E Marked Enemy").SetValue(new Circle(true, Color.GreenYellow)));
                drawE.AddItem(new MenuItem("DrawEMarkStatus", "E Marked Status").SetValue(true));
                config.AddSubMenu(drawE);
            }

            var dmgAfterComboItem = new MenuItem("DamageAfterCombo", "Damage After Combo").SetValue(true);
            Config.AddItem(dmgAfterComboItem);

            return true;
        }

        public override bool MiscMenu(Menu config)
        {
            config.AddItem(new MenuItem("UseWM" + Id, "Use W KillSteal").SetValue(true));
            config.AddItem(new MenuItem("UseRM" + Id, "Use R KillSteal").SetValue(true));
            config.AddItem(new MenuItem("UseRMG" + Id, "Use R Gapclosers").SetValue(true));
            config.AddItem(new MenuItem("UseRMI" + Id, "Use R Interrupt").SetValue(true));
            return true;
        }

        public override bool ExtrasMenu(Menu config)
        {

            return true;
        }

        public override bool LaneClearMenu(Menu config)
        {
            return true;
        }
    }
}
