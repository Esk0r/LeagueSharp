#region
using System;
using System.Linq;
using LeagueSharp;
using LeagueSharp.Common;
using SharpDX;
using SharpDX.Direct3D9;
using Font = SharpDX.Direct3D9.Font;
#endregion

namespace Marksman
{
    internal class Jinx : Champion
    {
        public static Spell Q;
        public static Spell W;
        public static Spell E;
        public static Spell R;
        public readonly Font vText;

        public Jinx()
        {
            Q = new Spell(SpellSlot.Q);

            W = new Spell(SpellSlot.W, 1450f);
            W.SetSkillshot(0.7f, 60f, 3300f, true, SkillshotType.SkillshotLine);

            E = new Spell(SpellSlot.E, 900f);
            E.SetSkillshot(0.9f, 60f, 1700f, false, SkillshotType.SkillshotCircle);

            R = new Spell(SpellSlot.R, 1500f);
            R.SetSkillshot(0.6f, 140f, 1700f, false, SkillshotType.SkillshotLine);

            Obj_AI_Base.OnProcessSpellCast += Game_OnProcessSpell;

            Utils.PrintMessage("Jinx loaded.");
            vText = new Font(
                Drawing.Direct3DDevice,
                new FontDescription
                {
                    FaceName = "Courier new",
                    Height = 15,
                    OutputPrecision = FontPrecision.Default,
                    Quality = FontQuality.Default,
                });
        }


        public void Game_OnProcessSpell(Obj_AI_Base unit, GameObjectProcessSpellCastEventArgs spell)
        {
            return;
        }

        #region JinxData

        public class JinxData
        {
            public static bool CanUseFuckingPowPowStack = true;

            public class JinxSpells
            {

                public static bool CanCastQ
                {
                    get
                    {
                        var protectManaForUlt =
                            Program.Config.SubMenu("Misc").Item("ProtectManaForUlt").GetValue<bool>();

                        var xMana = ObjectManager.Player.Spellbook.GetSpell(SpellSlot.Q).ManaCost;
                        var xRMana = ObjectManager.Player.Spellbook.GetSpell(SpellSlot.R).ManaCost;

                        return !protectManaForUlt || (!R.IsReady() || ObjectManager.Player.Mana >= xRMana + xMana);
                    }
                }

                public static bool CanCastW
                {
                    get
                    {
                        var protectManaForUlt =
                            Program.Config.SubMenu("Misc").Item("ProtectManaForUlt").GetValue<bool>();

                        var xMana = ObjectManager.Player.Spellbook.GetSpell(SpellSlot.Q).ManaCost;
                        var xRMana = ObjectManager.Player.Spellbook.GetSpell(SpellSlot.R).ManaCost;

                        return !protectManaForUlt || (!R.IsReady() || ObjectManager.Player.Mana >= xRMana + xMana);
                    }
                }

                public static bool CanCastE
                {
                    get
                    {
                        var protectManaForUlt =
                            Program.Config.SubMenu("Misc").Item("ProtectManaForUlt").GetValue<bool>();

                        var xMana = ObjectManager.Player.Spellbook.GetSpell(SpellSlot.Q).ManaCost;
                        var xRMana = ObjectManager.Player.Spellbook.GetSpell(SpellSlot.R).ManaCost;

                        return !protectManaForUlt || (!R.IsReady() || ObjectManager.Player.Mana >= xRMana + xMana);
                    }
                }

            }

            internal enum GunType
            {
                Mini,
                Mega
            };

            public static float QMiniGunRange
            {
                get { return 600; }
            }

            public static float QMegaGunRange
            {
                get { return QMiniGunRange + 200 + 25 * Q.Level; }
            }

            public static bool EnemyHasBuffForCastE
            {
                get
                {
                    var t = TargetSelector.GetTarget(E.Range, TargetSelector.DamageType.Physical);
                    return t.HasBuffOfType(BuffType.Slow) || t.HasBuffOfType(BuffType.Stun) ||
                           t.HasBuffOfType(BuffType.Snare) || t.HasBuffOfType(BuffType.Charm) ||
                           t.HasBuffOfType(BuffType.Fear) || t.HasBuffOfType(BuffType.Taunt) ||
                           t.HasBuff("zhonyasringshield") || t.HasBuff("Recall") || t.HasBuff("teleport_target", true);
                }
            }

            public static int GetPowPowStacks
            {
                get
                {
                    return
                        ObjectManager.Player.Buffs.Where(buff => buff.DisplayName.ToLower() == "jinxqramp")
                            .Select(buff => buff.Count)
                            .FirstOrDefault();
                }
            }

            public static GunType QGunType
            {
                get { return ObjectManager.Player.HasBuff("JinxQ", true) ? GunType.Mega : GunType.Mini; }
            }

            public static int CountEnemies(float range)
            {
                return ObjectManager.Player.CountEnemiesInRange(range);
            }

            public static float GetRealPowPowRange(GameObject target)
            {
                return 525f + ObjectManager.Player.BoundingRadius + target.BoundingRadius;
            }

            public static float GetRealDistance(GameObject target)
            {
                return ObjectManager.Player.Position.Distance(target.Position);
            }

            public static float GetSlowEndTime(Obj_AI_Base target)
            {
                return
                    target.Buffs.OrderByDescending(buff => buff.EndTime - Game.Time)
                        .Where(buff => buff.Type == BuffType.Slow)
                        .Select(buff => buff.EndTime)
                        .FirstOrDefault();
            }

            public static HitChance GetWHitChance
            {
                get
                {
                    HitChance hitChance;
                    var wHitChance = Program.Config.Item("WHitChance").GetValue<StringList>().SelectedIndex;
                    switch (wHitChance)
                    {
                        case 0:
                        {
                            hitChance = HitChance.Low;
                            break;
                        }
                        case 1:
                        {
                            hitChance = HitChance.Medium;
                            break;
                        }
                        case 2:
                        {
                            hitChance = HitChance.High;
                            break;
                        }
                        case 3:
                        {
                            hitChance = HitChance.VeryHigh;
                            break;
                        }
                        case 4:
                        {
                            hitChance = HitChance.Dashing;
                            break;
                        }
                        default:
                        {
                            hitChance = HitChance.High;
                            break;
                        }
                    }
                    return hitChance;
                }
            }
        }

        #endregion

        public class JinxEvents
        {
            public static void ExecuteToggle()
            {
                if (JinxData.JinxSpells.CanCastQ)
                    HarassToggleQ();

                if (JinxData.JinxSpells.CanCastW)
                    HarassToggleW();
            }

            public static int GetEnemiesArround
            {
                get
                {
                    var t = TargetSelector.GetTarget(JinxData.QMegaGunRange, TargetSelector.DamageType.Physical);

                    var enemiesArround = 0;
                    enemiesArround +=
                        ObjectManager.Get<Obj_AI_Hero>()
                            .Count(
                                xEnemy =>
                                    xEnemy.IsEnemy && xEnemy.IsValidTarget(JinxData.QMegaGunRange) &&
                                    xEnemy.Distance(t) < 185 && xEnemy.ChampionName != t.ChampionName);
                    return enemiesArround + 1;

                }
            }

            public static void AlwaysChooseMiniGun()
            {
                if (Program.Config.SubMenu("Misc").Item("Swap2Mini").GetValue<bool>())
                {
                    if (JinxData.QGunType == JinxData.GunType.Mega && GetEnemiesArround < 2)
                    {
                        Q.Cast();
                    }
                }
            }

            private static void UsePowPowStack()
            {
                if (!Q.IsReady())
                    return;

                if (JinxData.QGunType == JinxData.GunType.Mini && JinxData.CanUseFuckingPowPowStack &&
                    JinxData.GetPowPowStacks == 3)
                {
                    var t = TargetSelector.GetTarget(JinxData.QMegaGunRange, TargetSelector.DamageType.Physical);
                    if (t.IsValidTarget())
                        Q.Cast();
                }
            }

            public static void ExecutePowPowStack()
            {
                var useQPow = Program.Config.SubMenu("Misc").Item("UseQPowPowStack").GetValue<bool>();
                if (useQPow)
                {
                    UsePowPowStack();
                }
            }

            public static void CastQ(bool checkPerMana = false)
            {
                if (!Q.IsReady())
                    return;

                if (checkPerMana &&
                    ObjectManager.Player.ManaPercentage() < Program.Config.Item("UseQTHM").GetValue<Slider>().Value)
                    return;

                var t = TargetSelector.GetTarget(JinxData.QMegaGunRange, TargetSelector.DamageType.Physical);
                if (!t.IsValidTarget())
                    return;

                var swapAoe = Program.Config.SubMenu("Misc").Item("SwapAOE").GetValue<Slider>().Value;
                if (swapAoe > 1 && JinxData.QGunType == JinxData.GunType.Mini && GetEnemiesArround > swapAoe)
                {
                    Q.Cast();
                    return;
                }

                if (!Program.Config.SubMenu("Misc").Item("SwapDistance").GetValue<bool>())
                    return;

                if (JinxData.QGunType == JinxData.GunType.Mega)
                {
                    if (JinxData.GetRealDistance(t) <= JinxData.QMiniGunRange)
                        Q.Cast();
                }
                else
                {
                    if (JinxData.GetRealDistance(t) > JinxData.QMiniGunRange)
                        Q.Cast();

                }
            }

            public static void CastW(bool checkPerMana = false)
            {
                if (!W.IsReady())
                    return;

                var existsManaPer = Program.Config.Item("UseQTHM").GetValue<Slider>().Value;

                if (checkPerMana && ObjectManager.Player.ManaPercentage() < existsManaPer)
                    return;

                var t = TargetSelector.GetTarget(W.Range, TargetSelector.DamageType.Physical);

                if (!t.IsValidTarget())
                    return;

                var minW = Program.Config.Item("MinWRange").GetValue<Slider>().Value;

                if (JinxData.GetRealDistance(t) >= minW ||
                    t.Health <= ObjectManager.Player.GetSpellDamage(t, SpellSlot.W))
                {
                    W.CastIfHitchanceEquals(t, JinxData.GetWHitChance);
                }
            }

            public static void CastE(HitChance hitChance)
            {
                if (!E.IsReady())
                    return;

                var t = TargetSelector.GetTarget(E.Range, TargetSelector.DamageType.Physical);

                if (!t.IsValidTarget())
                    return;

                if (t.GetWaypoints().Count == 1)
                    return;
                if (E.IsReady())
                {
                    E.CastIfHitchanceEquals(t, hitChance);
                }
            }

            private static void HarassToggleQ()
            {
                var toggleActive = Program.Config.Item("UseQTH").GetValue<KeyBind>().Active;
                if (toggleActive && !ObjectManager.Player.HasBuff("Recall"))
                {
                    CastQ(true);
                }
            }

            private static void HarassToggleW()
            {
                var toggleActive = Program.Config.Item("UseWTH").GetValue<KeyBind>().Active;

                if (toggleActive && !ObjectManager.Player.HasBuff("Recall"))
                    CastW(true);
            }
        }

        public override void Game_OnGameUpdate(EventArgs args)
        {
            JinxEvents.ExecuteToggle();
            JinxEvents.AlwaysChooseMiniGun();

            if (E.IsReady())
            {
                var t = TargetSelector.GetTarget(E.Range, TargetSelector.DamageType.Physical);
                if (GetValue<bool>("AutoEI"))
                {
                    JinxEvents.CastE(HitChance.Immobile);
                }

                if (GetValue<bool>("AutoED"))
                {
                    JinxEvents.CastE(HitChance.Dashing);
                }

                if (GetValue<bool>("AutoES"))
                {
                    if (JinxData.EnemyHasBuffForCastE)
                    {
                        JinxEvents.CastE(HitChance.High);
                    }
                }
            }


            if (GetValue<bool>("UseRC") && R.IsReady())
            {
                var maxRRange = GetValue<Slider>("MaxRRange").Value;
                var t = TargetSelector.GetTarget(maxRRange, TargetSelector.DamageType.Physical);

                var aaDamage = Orbwalking.InAutoAttackRange(t)
                    ? ObjectManager.Player.GetAutoAttackDamage(t, true) * JinxData.GetPowPowStacks
                    : 0;

                if (t.Health > aaDamage && t.Health <= ObjectManager.Player.GetSpellDamage(t, SpellSlot.R))
                {
                    if (t.IsValidTarget())
                    {
                        R.Cast(t);
                    }
                }
            }

            if ((!ComboActive && !HarassActive) || !Orbwalking.CanMove(100))
            {
                var useQ = GetValue<bool>("UseQ" + (ComboActive ? "C" : "H"));
                var useW = GetValue<bool>("UseW" + (ComboActive ? "C" : "H"));
                var useE = GetValue<bool>("UseE" + (ComboActive ? "C" : "H"));

                if (useQ && JinxData.JinxSpells.CanCastQ)
                    JinxEvents.CastQ();

                if (useW && JinxData.JinxSpells.CanCastW)
                {
                    JinxEvents.CastW();
                }

                if (useE && JinxData.JinxSpells.CanCastE)
                {
                    JinxEvents.CastE(HitChance.High);
                }
            }
        }

        public override void Orbwalking_AfterAttack(AttackableUnit unit, AttackableUnit target)
        {
            if ((ComboActive || HarassActive) && (unit.IsValid || unit.IsMe) && (target is Obj_AI_Hero))
            {
                var useW = GetValue<bool>("UseW" + (ComboActive ? "C" : "H"));

                if (useW && W.IsReady())
                    JinxEvents.CastW();
            }
        }

        public override void Drawing_OnDraw(EventArgs args)
        {

            var t = TargetSelector.GetTarget(JinxData.QMegaGunRange + 500, TargetSelector.DamageType.Physical);
            if (t != null)
            {
                var enemiesArround = 0;
                Render.Circle.DrawCircle(t.Position, 125f, System.Drawing.Color.Green);
                foreach (var xEnemy in ObjectManager.Get<Obj_AI_Hero>())
                {
                    if (xEnemy.IsEnemy && xEnemy.IsValidTarget(JinxData.QMegaGunRange + 500) &&
                        xEnemy.Distance(t) < 185f && xEnemy.ChampionName != t.ChampionName)
                    {
                        Render.Circle.DrawCircle(xEnemy.Position, 125f, System.Drawing.Color.Red);
                        enemiesArround++;
                    }
                }
            }

            Spell[] spellList = { W, E };
            var drawQbound = GetValue<Circle>("DrawQBound");

            if (Program.Config.Item("DrawToggleStatus").GetValue<bool>())
            {
                var xHarassStatus = "";
                if (Program.Config.Item("UseQTH").GetValue<KeyBind>().Active)
                    xHarassStatus += "Q + ";

                if (Program.Config.Item("UseWTH").GetValue<KeyBind>().Active)
                    xHarassStatus += "W + ";

                xHarassStatus = xHarassStatus.Length < 1 ? "Toggle: Off   " : "Toggle: " + xHarassStatus;

                var xText = xHarassStatus.Substring(0, xHarassStatus.Length - 3);

                var vText1 = vText;
                var vText2 = vText;

                Vector2 pos = Drawing.WorldToScreen(ObjectManager.Player.Position);
                Utils.DrawText(
                    vText1, xText, (int) ObjectManager.Player.HPBarPosition.X + 145,
                    (int) ObjectManager.Player.HPBarPosition.Y + 5, SharpDX.Color.White);


                t = TargetSelector.GetTarget(JinxData.QMegaGunRange + 200f, TargetSelector.DamageType.Physical);
                if (t != null)
                {
                    var xString = "Target:" + t.ChampionName;
                    Utils.DrawText(
                        vText2, xString, (int) ObjectManager.Player.HPBarPosition.X + 145,
                        (int) ObjectManager.Player.HPBarPosition.Y + 17, SharpDX.Color.White);
                }
            }

            foreach (var spell in spellList)
            {
                var menuItem = GetValue<Circle>("Draw" + spell.Slot);
                if (menuItem.Active)
                {
                    Render.Circle.DrawCircle(ObjectManager.Player.Position, spell.Range, menuItem.Color);
                }
            }

            if (drawQbound.Active)
            {
                Render.Circle.DrawCircle(
                    ObjectManager.Player.Position,
                    JinxData.QGunType == JinxData.GunType.Mini ? JinxData.QMegaGunRange : JinxData.QMiniGunRange,
                    drawQbound.Color);
            }
        }

        private static void ShowToggleStatus()
        {
            var xHarassStatus = "";
            if (Program.Config.Item("UseQTH").GetValue<KeyBind>().Active)
                xHarassStatus += "Q - ";

            if (Program.Config.Item("UseWTH").GetValue<KeyBind>().Active)
                xHarassStatus += "W - ";

            if (xHarassStatus.Length < 1)
            {
                xHarassStatus = "Toggle: Off   ";
            }
            else
            {
                xHarassStatus = "Toggle: " + xHarassStatus;
            }
            xHarassStatus = xHarassStatus.Substring(0, xHarassStatus.Length - 3);
            Drawing.DrawText(Drawing.Width * 0.44f, Drawing.Height * 0.82f, System.Drawing.Color.Wheat, xHarassStatus);
        }

        public override bool ComboMenu(Menu config)
        {
            config.AddItem(new MenuItem("UseQC" + Id, "Use Q").SetValue(true));
            config.AddItem(new MenuItem("UseWC" + Id, "Use W").SetValue(true));
            config.AddItem(new MenuItem("UseEC" + Id, "Use E").SetValue(true));
            config.AddItem(new MenuItem("UseRC" + Id, "Use R").SetValue(true));
            return true;
        }

        public override bool HarassMenu(Menu config)
        {
            config.AddItem(new MenuItem("UseQH" + Id, "Use Q").SetValue(true));
            config.AddItem(new MenuItem("UseWH" + Id, "Use W").SetValue(false));

            config.AddItem(
                new MenuItem("UseQTH", "Q (Toggle!)").SetValue(new KeyBind("T".ToCharArray()[0], KeyBindType.Toggle)));
            config.AddItem(new MenuItem("UseQTHM", "Q Mana Per.").SetValue(new Slider(50, 100, 0)));

            config.AddItem(
                new MenuItem("UseWTH", "W (Toggle!)").SetValue(new KeyBind("Y".ToCharArray()[0], KeyBindType.Toggle)));
            config.AddItem(new MenuItem("UseWTHM", "W Mana Per.").SetValue(new Slider(50, 100, 0)));
            return true;
        }

        public override bool LaneClearMenu(Menu config)
        {
            config.AddItem(new MenuItem("SwapQ" + Id, "Always swap to Minigun").SetValue(false));
            return true;
        }

        public override bool MiscMenu(Menu config)
        {
            var xQMenu = new Menu("Q Settings", "QSettings");
            {
                xQMenu.AddItem(
                    new MenuItem("SwapAOE", "Swap Q for AOE Damage If will hit enemies >=").SetValue(
                        new Slider(2, 0, 5)));
                xQMenu.AddItem(new MenuItem("SwapDistance", "Swap Q for Distance").SetValue(true));
                xQMenu.AddItem(new MenuItem("Swap2Mini", "Always Choose MiniGun If No Enemy").SetValue(true));
                config.AddSubMenu(xQMenu);
            }

            var xWMenu = new Menu("W Settings", "WSettings");
            {
                xWMenu.AddItem(
                    new MenuItem("WHitChance", "W HitChance").SetValue(
                        new StringList(new[] { "Low", "Medium", "High", "Very High", "Immobile" }, 2)));
                xWMenu.AddItem(new MenuItem("MinWRange", "Min. W range").SetValue(new Slider(525 + 65 * 2, 0, 1200)));
                config.AddSubMenu(xWMenu);
            }

            var xEMenu = new Menu("E Settings", "ESettings");
            {
                xEMenu.AddItem(new MenuItem("AutoEI" + Id, "Auto-E -> Immobile").SetValue(true));
                xEMenu.AddItem(new MenuItem("AutoES" + Id, "Auto-E -> Slowed/Stunned/Teleport/Snare").SetValue(true));
                xEMenu.AddItem(new MenuItem("AutoED" + Id, "Auto-E -> Dashing").SetValue(false));
                config.AddSubMenu(xEMenu);
            }

            var xRMenu = new Menu("R Settings", "RSettings");
            {
                xRMenu.AddItem(new MenuItem("MaxRRange" + Id, "Max R range").SetValue(new Slider(1700, 0, 4000)));
                xRMenu.AddItem(new MenuItem("ProtectManaForUlt", "Protect Mana for Ultimate").SetValue(true));
                config.AddSubMenu(xRMenu);
            }
            return true;
        }

        public override bool DrawingMenu(Menu config)
        {
            config.AddItem(
                new MenuItem("DrawQBound" + Id, "Draw Q bound").SetValue(new Circle(true, System.Drawing.Color.Azure)));
            config.AddItem(
                new MenuItem("DrawW" + Id, "W range").SetValue(new Circle(false, System.Drawing.Color.Azure)));
            config.AddItem(
                new MenuItem("DrawE" + Id, "E range").SetValue(new Circle(false, System.Drawing.Color.Azure)));

            config.AddItem(new MenuItem("DrawToggleStatus", "Show Toggle Status").SetValue(true));

            return true;
        }

        public override bool ExtrasMenu(Menu config)
        {
            return true;
        }
    }
}
