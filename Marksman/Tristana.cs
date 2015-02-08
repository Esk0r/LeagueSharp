#region
using LeagueSharp;
using LeagueSharp.Common;
using System;
using System.Drawing;
using System.Linq;
using SharpDX.Direct3D9;
using Font = SharpDX.Direct3D9.Font;
#endregion

namespace Marksman
{
    internal class Tristana : Champion
    {
        public static Obj_AI_Hero Player = ObjectManager.Player;
        public static Spell Q, W, E, R;
        public static Font vText;
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

        public class TristanaData
        {
            public static Obj_AI_Hero GetTarget(float vRange)
            {
                return TargetSelector.GetTarget(vRange, TargetSelector.DamageType.Physical);
            }

            public static double GetWDamage
            {
                get
                {
                    if (W.IsReady())
                    {
                        var wDamage = new double[] { 80, 105, 130, 155, 180 }[W.Level - 1] +
                                      0.5 * Player.FlatMagicDamageMod;
                        if (GetEMarkedCount > 0 && GetEMarkedCount < 4)
                        {
                            return wDamage + (wDamage * GetEMarkedCount * .20);
                        }
                        switch (GetEMarkedCount)
                        {
                            case 0:
                                return wDamage;
                            case 4:
                                return wDamage * 2;
                        }
                    }
                    return 0;
                }
            }

            public static float GetComboDamage
            {
                get
                {
                    var fComboDamage = 0d;
                    var t = GetTarget(W.Range * 2);
                    if (!t.IsValidTarget())
                        return 0;
                    /*
                    if (Q.IsReady())
                    {
                        var baseAttackSpeed = 0.656 + (0.656 / 100 * (Player.Level - 1) * 1.5);
                        var qExtraAttackSpeed = new double[] { 30, 50, 70, 90, 110 }[Q.Level - 1];
                        var attackDelay = (float) (baseAttackSpeed + (baseAttackSpeed / 100 * qExtraAttackSpeed));
                        attackDelay = (float) Math.Round(attackDelay, 2);

                        attackDelay *= 5;
                        attackDelay *= (float) Math.Floor(Player.TotalAttackDamage());
                        fComboDamage += attackDelay;
                    }
                    */
                    if (W.IsReady())
                    {
                        fComboDamage += GetWDamage;
                    }

                    if (E.IsReady())
                    {
                        fComboDamage += new double[] { 60, 70, 80, 90, 100 }[E.Level - 1] * 2 *
                                        Player.FlatMagicDamageMod;
                    }

                    if (R.IsReady())
                    {
                        fComboDamage += new double[] { 300, 400, 500 }[R.Level - 1] + Player.FlatMagicDamageMod;
                    }
                    return (float) fComboDamage;
                }
            }

            public static Obj_AI_Hero GetEMarkedEnemy
            {
                get
                {
                    return
                        ObjectManager.Get<Obj_AI_Hero>()
                            .Where(
                                enemy =>
                                    !enemy.IsDead &&
                                    enemy.IsValidTarget(W.Range + Orbwalking.GetRealAutoAttackRange(Player)))
                            .FirstOrDefault(
                                enemy => enemy.Buffs.Any(buff => buff.DisplayName == "TristanaEChargeSound"));
                }
            }

            public static int GetEMarkedCount
            {
                get
                {
                    if (GetEMarkedEnemy == null)
                        return 0;
                    return
                        GetEMarkedEnemy.Buffs.Where(buff => buff.DisplayName == "TristanaECharge")
                            .Select(xBuff => xBuff.Count)
                            .FirstOrDefault();
                }
            }
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

        public override void Orbwalking_AfterAttack(AttackableUnit unit, AttackableUnit target)
        {
            var t = target as Obj_AI_Hero;
            if (t != null && (ComboActive || HarassActive) && unit.IsMe)
            {
                var useQ = Q.IsReady() && GetValue<bool>("UseQ" + (ComboActive ? "C" : "H"));
                var useE = E.IsReady() && GetValue<bool>("UseE" + (ComboActive ? "C" : "H"));

                if (useQ)
                    Q.CastOnUnit(Player);

                if (useE && canUseE(t))
                    E.CastOnUnit(t);
            }
        }

        private static bool canUseE(Obj_AI_Hero t)
        {
            if (Player.CountEnemiesInRange(W.Range + (E.Range / 2)) == 1)
                return true;

            return (Program.Config.Item("DontUseE" + t.ChampionName) != null &&
                    Program.Config.Item("DontUseE" + t.ChampionName).GetValue<bool>() == false);
        }

        public override void Game_OnGameUpdate(EventArgs args)
        {
            if (!Orbwalking.CanMove(100))
                return;

            var getEMarkedEnemy = TristanaData.GetEMarkedEnemy;
            if (getEMarkedEnemy != null)
            {
                TargetSelector.SetTarget(getEMarkedEnemy);
            }
            else
            {
                var attackRange = Orbwalking.GetRealAutoAttackRange(Player);
                TargetSelector.SetTarget(TargetSelector.GetTarget(attackRange, TargetSelector.DamageType.Physical));
            }

            Q.Range = 600 + 5 * (Player.Level - 1);
            E.Range = 630 + 7 * (Player.Level - 1);
            R.Range = 630 + 7 * (Player.Level - 1);

            if (GetValue<KeyBind>("UseETH").Active)
            {
                if (Player.HasBuff("Recall"))
                    return;
                var t = TristanaData.GetTarget(E.Range);
                if (t.IsValidTarget() && E.IsReady() && canUseE(t))
                    E.CastOnUnit(t);
            }

            var useW = W.IsReady() && GetValue<bool>("UseWC");
            var useWc = W.IsReady() && GetValue<bool>("UseWCS");
            var useE = E.IsReady() && GetValue<bool>("UseE" + (ComboActive ? "C" : "H"));
            var useR = R.IsReady() && GetValue<bool>("UseRM") && R.IsReady();

            if (ComboActive || HarassActive)
            {
                Obj_AI_Hero t;
                if (TristanaData.GetEMarkedEnemy != null)
                {
                    t = TristanaData.GetEMarkedEnemy;
                    TargetSelector.SetTarget(TristanaData.GetEMarkedEnemy);
                }
                else
                {
                    t = TristanaData.GetTarget(W.Range);
                }

                if (useE && canUseE(t))
                {
                    if (E.IsReady() && t.IsValidTarget(E.Range))
                        E.CastOnUnit(t);
                }

                if (useW)
                {
                    t = TristanaData.GetTarget(W.Range);
                    if (t.IsValidTarget() && t.Health < TristanaData.GetWDamage)
                        W.Cast(t);
                }


                if (useWc)
                {
                    t = TristanaData.GetTarget(W.Range);
                    if (t.IsValidTarget() && TristanaData.GetEMarkedCount == 4)
                        W.Cast(t);
                }
            }

            if (ComboActive)
            {
                if (useR)
                {
                    var t = TristanaData.GetTarget(W.Range + R.Range - 30);

                    if (!t.IsValidTarget())
                        return;

                    if (Player.GetSpellDamage(t, SpellSlot.R) - 30 < t.Health ||
                        t.Health < Player.GetAutoAttackDamage(t, true))
                        return;

                    if (t.IsValidTarget() && !t.IsValidTarget(R.Range) && W.IsReady())
                        W.Cast(t);
                    R.CastOnUnit(t);
                }
            }
        }

        private static float GetComboDamage(Obj_AI_Hero t)
        {
            return TristanaData.GetComboDamage;
        }

        public override void Drawing_OnDraw(EventArgs args)
        {
            // Draw marked enemy status
            var drawEMarksStatus = Program.Config.SubMenu("Drawings").Item("DrawEMarkStatus").GetValue<bool>();
            var drawEMarkEnemy = Program.Config.SubMenu("Drawings").Item("DrawEMarkEnemy").GetValue<Circle>();
            if (drawEMarksStatus || drawEMarkEnemy.Active)
            {
                var vText1 = vText;
                var getEMarkedEnemy = TristanaData.GetEMarkedEnemy;
                if (getEMarkedEnemy != null)
                {
                    if (drawEMarksStatus)
                    {
                        if (LastTickTime < Environment.TickCount)
                            LastTickTime = Environment.TickCount + 5000;
                        var xTime = LastTickTime - Environment.TickCount;

                        var timer = string.Format("0:{0:D2}", xTime / 1000);
                        Utils.DrawText(
                            vText1, timer + " : 4 / " + TristanaData.GetEMarkedCount,
                            (int) getEMarkedEnemy.HPBarPosition.X + 145, (int) getEMarkedEnemy.HPBarPosition.Y + 5,
                            SharpDX.Color.White);
                    }

                    if (drawEMarkEnemy.Active)
                    {
                        Render.Circle.DrawCircle(TristanaData.GetEMarkedEnemy.Position, 140f, drawEMarkEnemy.Color, 1);
                    }
                }
            }

            Spell[] spellList = { W };
            foreach (var spell in spellList)
            {
                var menuItem = GetValue<Circle>("Draw" + spell.Slot);
                if (menuItem.Active)
                    Render.Circle.DrawCircle(Player.Position, spell.Range, menuItem.Color, 1);
            }

            var drawE = Program.Config.SubMenu("Drawings").Item("DrawE").GetValue<Circle>();
            if (drawE.Active)
            {
                Render.Circle.DrawCircle(Player.Position, E.Range, drawE.Color, 1);
            }
        }

        public override bool ComboMenu(Menu config)
        {
            config.AddItem(new MenuItem("UseQC" + Id, "Use Q").SetValue(true));
            config.AddItem(new MenuItem("UseWC" + Id, "Use W").SetValue(true));
            config.AddItem(new MenuItem("UseWCS" + Id, "Complete E stacks with W").SetValue(true));
            config.AddItem(new MenuItem("UseEC" + Id, "Use E").SetValue(true));

            config.AddSubMenu(new Menu("Don't Use E to", "DontUseE"));
            {
                foreach (var enemy in
                    ObjectManager.Get<Obj_AI_Hero>().Where(enemy => enemy.Team != Player.Team))
                {
                    config.SubMenu("DontUseE")
                        .AddItem(new MenuItem("DontUseE" + enemy.ChampionName, enemy.ChampionName).SetValue(false));
                }
            }
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
