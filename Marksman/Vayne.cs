#region

using System;
using System.Linq;
using LeagueSharp;
using LeagueSharp.Common;

#endregion

namespace Marksman
{
    internal class Vayne : Champion
    {
        public Spell E;
        public Spell Q;
        public string[] interrupt;
        public string[] notarget;
        public string[] gapcloser;

        public Vayne()
        {
            Utils.PrintMessage("Vayne loaded");

            Q = new Spell(SpellSlot.Q, 0f);
            E = new Spell(SpellSlot.E, float.MaxValue);

            E.SetTargetted(0.25f, 2200f);
            Obj_AI_Base.OnProcessSpellCast += Game_ProcessSpell;
        }

        public override void Game_OnGameUpdate(EventArgs args)
        {
            if ((!E.IsReady()) ||
                ((Orbwalker.ActiveMode != Orbwalking.OrbwalkingMode.Combo || !GetValue<bool>("UseEC")) &&
                 !GetValue<KeyBind>("UseET").Active)) return;

            foreach (var hero in from hero in ObjectManager.Get<Obj_AI_Hero>().Where(hero => hero.IsValidTarget(550f))
                let prediction = E.GetPrediction(hero)
                where NavMesh.GetCollisionFlags(
                    prediction.UnitPosition.To2D()
                        .Extend(ObjectManager.Player.ServerPosition.To2D(), -GetValue<Slider>("PushDistance").Value)
                        .To3D())
                    .HasFlag(CollisionFlags.Wall) || NavMesh.GetCollisionFlags(
                        prediction.UnitPosition.To2D()
                            .Extend(ObjectManager.Player.ServerPosition.To2D(),
                                -(GetValue<Slider>("PushDistance").Value / 2))
                            .To3D())
                        .HasFlag(CollisionFlags.Wall)
                select hero)
            {
                E.Cast(hero);
            }
        }

        public override void Orbwalking_AfterAttack(Obj_AI_Base unit, Obj_AI_Base target)
        {
            if (unit.IsMe && Orbwalker.ActiveMode == Orbwalking.OrbwalkingMode.Combo && GetValue<bool>("UseQC"))
                Q.Cast(Game.CursorPos);
        }

        public override void ComboMenu(Menu config)
        {
            config.AddItem(new MenuItem("UseQC" + Id, "Use Q").SetValue(true));
            config.AddItem(new MenuItem("UseEC" + Id, "Use E").SetValue(true));
        }

        public override void MiscMenu(Menu config)
        {
            
            config.AddItem(
                new MenuItem("UseET" + Id, "Use E (Toggle)").SetValue(
                    new KeyBind("T".ToCharArray()[0], KeyBindType.Toggle)));
            config.AddItem(new MenuItem("UseEInterrupt" + Id, "Use E To Interrupt").SetValue(true));
            config.AddItem(
                new MenuItem("PushDistance" + Id, "E Push Distance").SetValue(new Slider(425, 475, 300)));
            config.AddItem(new MenuItem("UseEaa", "Use E after auto").SetValue(new KeyBind("G".ToCharArray()[0], KeyBindType.Press)));
            config.AddSubMenu(new Menu("Gapcloser List", "gap"));
            config.AddSubMenu(new Menu("Gapcloser List 2", "gap2"));
            config.AddSubMenu(new Menu("Interrupt List", "int"));
            gapcloser = new[]
            {
                "AkaliShadowDance", "Headbutt", "DianaTeleport", "IreliaGatotsu", "JaxLeapStrike", "JayceToTheSkies",
                "MaokaiUnstableGrowth", "MonkeyKingNimbus", "Pantheon_LeapBash", "PoppyHeroicCharge", "QuinnE",
                "XenZhaoSweep", "blindmonkqtwo", "FizzPiercingStrike", "RengarLeap"
            };
            notarget = new[]
            {
                "AatroxQ", "GragasE", "GravesMove", "HecarimUlt", "JarvanIVDragonStrike", "JarvanIVCataclysm", "KhazixE",
                "khazixelong", "LeblancSlide", "LeblancSlideM", "LeonaZenithBlade", "UFSlash", "RenektonSliceAndDice",
                "SejuaniArcticAssault", "ShenShadowDash", "RocketJump", "slashCast"
            };
            interrupt = new[]
            {
                "KatarinaR", "GalioIdolOfDurand", "Crowstorm", "Drain", "AbsoluteZero", "ShenStandUnited", "UrgotSwap2",
                "AlZaharNetherGrasp", "FallenOne", "Pantheon_GrandSkyfall_Jump", "VarusQ", "CaitlynAceintheHole",
                "MissFortuneBulletTime", "InfiniteDuress", "LucianR"
            };
            for (int i=0; i<gapcloser.Length; i++)
            {
                menu.SubMenu("gap").AddItem(new MenuItem(gapcloser[i], gapcloser[i])).SetValue(true);
            }
            for (int i = 0; i < notarget.Length; i++)
            {
                menu.SubMenu("gap2").AddItem(new MenuItem(notarget[i], notarget[i])).SetValue(true);
            }
            for (int i = 0; i < interrupt.Length; i++)
            {
                menu.SubMenu("int").AddItem(new MenuItem(interrupt[i], interrupt[i])).SetValue(true);
            }
        }
        
        public static void Game_ProcessSpell(Obj_AI_Base hero, GameObjectProcessSpellCastEventArgs args)
        {
            var t = SimpleTs.GetTarget(550, SimpleTs.DamageType.Physical);
            if (hero.IsMe)
            {
                if (args.SData.Name.ToLower().Contains("attack"))
                {
                    if (menu.Item("UseEaa").GetValue<KeyBind>().Active)
                    {
                        E.Cast(t);
                    }

                    if (orbwalker.ActiveMode.ToString() == "Combo" && menu.Item("UseQC").GetValue<bool>())
                    {
                        var after = ObjectManager.Player.Position +
                            Normalize(Game.CursorPos - ObjectManager.Player.Position)*300;
                        var disafter = Vector3.DistanceSquared(after, t.Position);
                        if (disafter < 630*630 && disafter > 175*175)
                        {
                            Q.Cast(Game.CursorPos);
                        }
                        if (Vector3.DistanceSquared(t.Position, ObjectManager.Player.Position) > 630*630 &&
                            disafter < 630*630)
                        {
                            Q.Cast(Game.CursorPos);
                        }
                    }

                }
                

                if (args.SData.Name == "VayneCondemnMissile")
                {
                    Orbwalking.ResetAutoAttackTimer();
                }
            }
            
            if (menu.Item("UseEInterrupt").GetValue<bool>() && hero.IsValidTarget(550f) && menu.Item(args.SData.Name).GetValue<bool>())
            {
                if (interrupt.Any(str => str.Contains(args.SData.Name)))
                {
                    E.Cast(hero);
                }
            }

            if (gapcloser.Any(str => str.Contains(args.SData.Name)) && args.Target == ObjectManager.Player && hero.IsValidTarget(550f) && menu.Item(args.SData.Name).GetValue<bool>())
            {
                E.Cast(hero);
            }

            if (notarget.Any(str => str.Contains(args.SData.Name)) && Vector3.Distance(args.End, ObjectManager.Player.Position) <= 300 && hero.IsValidTarget(550f) && menu.Item(args.SData.Name).GetValue<bool>())
            {
                E.Cast(hero);
            }
        }
        
        public static Vector3 Normalize(Vector3 A)
        {
            double distance = Math.Sqrt(A.X * A.X + A.Y * A.Y);
            return new Vector3(new Vector2((float) (A.X / distance)), (float) (A.Y / distance));
        }
    }
}
