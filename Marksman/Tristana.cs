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

        public Obj_AI_Hero GetEChargeEnemy
        {
            get
            {
                return
                    ObjectManager.Get<Obj_AI_Hero>()
                        .Where(
                            enemy =>
                                !enemy.IsDead &&
                                enemy.IsValidTarget(Orbwalking.GetRealAutoAttackRange(ObjectManager.Player)))
                        .FirstOrDefault(
                            enemy => enemy.Buffs.Any(buff => buff.DisplayName == "TristanaEChargeSound"));
            }
        }

        public int GetECharge
        {
            get
            {
                foreach (
                    var buff in
                        ObjectManager.Get<Obj_AI_Hero>()
                            .Where(enemy => !enemy.IsDead && enemy.IsValidTarget(1500))
                            .SelectMany(enemy => enemy.Buffs.Where(buff => buff.DisplayName == "TristanaECharge")))
                {
                    return buff.Count;
                }
                return 0;
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

        public override void Drawing_OnDraw(EventArgs args)
        {
            var xTime = 0;
            foreach (var enemy in ObjectManager.Get<Obj_AI_Hero>().Where(enemy => !enemy.IsDead && enemy.IsValidTarget(1500)))
            {
                foreach (var buff in enemy.Buffs.Where(buff => buff.DisplayName.Contains("TristanaECharge")))
                {
                    if (buff.DisplayName == "TristanaEChargeSound")
                    {
                        if (LastTickTime < Environment.TickCount)
                            LastTickTime = Environment.TickCount + 5000;
                        xTime = LastTickTime - Environment.TickCount;
                    }

                    if (buff.DisplayName == "TristanaECharge")
                    {
                        xBuffCount = buff.Count;
                    }
    
                    var timer = string.Format("0:{0:D2}", xTime / 1000);
                    Utils.DrawText(
                        vText, timer + " : 4 / " + xBuffCount, (int)enemy.HPBarPosition.X + 145,
                        (int)enemy.HPBarPosition.Y + 5, SharpDX.Color.White);
                }
            }

            Spell[] spellList = { E};
            foreach (var spell in spellList)
            {
                var menuItem = GetValue<Circle>("Draw" + spell.Slot);
                if (menuItem.Active)
                    Render.Circle.DrawCircle(ObjectManager.Player.Position, spell.Range, menuItem.Color);
            }
        }

      
        public override void Game_OnGameUpdate(EventArgs args)
        {
            if (!Orbwalking.CanMove(100)) return;
            
            //Update Q range depending on level; 600 + 5 Ã— ( Tristana's level - 1)/* dont waste your Q for only 1 or 2 hits. */
            //Update E and R range depending on level; 630 + 9 Ã— ( Tristana's level - 1)
            Q.Range = 600 + 5 * (ObjectManager.Player.Level - 1);
            E.Range = 630 + 9 * (ObjectManager.Player.Level - 1);
            R.Range = 630 + 9 * (ObjectManager.Player.Level - 1);

            if (GetValue<KeyBind>("UseETH").Active)
            {
                 if(ObjectManager.Player.HasBuff("Recall"))
                    return;
                var eTarget = TargetSelector.GetTarget(E.Range, TargetSelector.DamageType.Physical);
                if (E.IsReady() && eTarget.IsValidTarget())
                    E.CastOnUnit(eTarget);
            }

            if (ComboActive || HarassActive)
            {
                Obj_AI_Hero t;
                t = TargetSelector.GetTarget(W.Range, TargetSelector.DamageType.Physical);

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
                    if (t.IsValidTarget() && W.IsReady() && GetECharge == 4 && !t.UnderTurret())
                    {
                        W.Cast(t);
                    }
                }
            }

            //Killsteal
            if (!ComboActive || !GetValue<bool>("UseRM") || !R.IsReady()) return;
            foreach (
                var hero in
                    ObjectManager.Get<Obj_AI_Hero>()
                        .Where(
                            hero =>
                                hero.IsValidTarget(R.Range) &&
                                ObjectManager.Player.GetSpellDamage(hero, SpellSlot.R) - 50 > hero.Health))
                R.CastOnUnit(hero);
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
                new MenuItem("UseETH" + Id, "Use E (Toggle)").SetValue(new KeyBind("H".ToCharArray()[0],
                    KeyBindType.Toggle)));
            return true;
        }

        public override bool DrawingMenu(Menu config)
        {
            config.AddItem(
                new MenuItem("DrawE" + Id, "E range").SetValue(new Circle(true, Color.CornflowerBlue)));
            var dmgAfterComboItem = new MenuItem("DamageAfterCombo", "Damage After Combo").SetValue(true);
            Config.AddItem(dmgAfterComboItem);

            return true;
        }

        public override bool MiscMenu(Menu config)
        {
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
