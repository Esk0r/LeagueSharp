using System;
using System.Collections.Generic;
using System.Linq;
using LeagueSharp;
using LeagueSharp.Common;
using SharpDX;
using Color = System.Drawing.Color;

/*Credits:
 * andreluis034
 * 
 * 
 */
/*TODO:
 * Mana manager
 */

namespace Marksman
{
    internal class Reticles
    {
        public GameObject Object { get; set; }
        public float NetworkId { get; set; }
        public Vector3 ReticlePos { get; set; }
        public double ExpireTime { get; set; }
    }

    internal class Draven : Champion
    {
        private static readonly List<Reticles> ExistingReticles = new List<Reticles>();
        public static Spell Q, W, E, R;
        public int QStacks = 0;

        public Draven()
        {
            Q = new Spell(SpellSlot.Q);
            W = new Spell(SpellSlot.W);
            E = new Spell(SpellSlot.E, 1100);
            R = new Spell(SpellSlot.R, 20000);
            E.SetSkillshot(250f, 130f, 1400f, false, SkillshotType.SkillshotLine);
            R.SetSkillshot(400f, 160f, 2000f, false, SkillshotType.SkillshotLine);
            GameObject.OnCreate += OnCreateObject;
            GameObject.OnDelete += OnDeleteObject;
            AntiGapcloser.OnEnemyGapcloser += OnEnemyGapcloser;
            Interrupter.OnPosibleToInterrupt += OnPosibleToInterrupt;
            Utils.PrintMessage("Draven loaded.");
        }

        public void OnEnemyGapcloser(ActiveGapcloser gapcloser)
        {
            if (E.IsReady() && Config.Item("EGapCloser").GetValue<bool>() && gapcloser.Sender.IsValidTarget(E.Range))
            {
                E.Cast(gapcloser.Sender);
            }
        }

        public void OnPosibleToInterrupt(Obj_AI_Base unit, InterruptableSpell spell)
        {
            if (E.IsReady() && Config.Item("EInterruptable").GetValue<bool>() && unit.IsValidTarget(E.Range))
            {
                E.Cast(unit);
            }
        }

        private static void OnDeleteObject(GameObject sender, EventArgs args)
        {
            if (!(sender.Name.Contains("Q_reticle_self")))
            {
                return;
            }
            for (var i = 0; i < ExistingReticles.Count; i++)
            {
                if (ExistingReticles[i].NetworkId == sender.NetworkId)
                {
                    ExistingReticles.RemoveAt(i);
                    return;
                }
            }
            //ExistingReticles.RemoveAll(reticle => reticle.NetworkId == sender.NetworkId);
            //Packet.S2C.Ping.Encoded(new Packet.S2C.Ping.Struct(sender.Position.X, sender.Position.Y)).Process();
        }

        private static void OnCreateObject(GameObject sender, EventArgs args)
        {
            if (!(sender.Name.Contains("Q_reticle_self")))
            {
                return;
            }
            //Console.WriteLine(sender.Name + sender.NetworkId + sender.Type);
            //Packet.S2C.Ping.Encoded(new Packet.S2C.Ping.Struct(sender.Position.X, sender.Position.Y)).Process();

            ExistingReticles.Add(
                new Reticles
                {
                    Object = sender,
                    NetworkId = sender.NetworkId,
                    ReticlePos = sender.Position,
                    ExpireTime = Game.Time + 1.20
                });
        }

        public override void Drawing_OnDraw(EventArgs args)
        {
            var drawOrbwalk = Config.Item("DrawOrbwalk").GetValue<Circle>();
            var drawReticles = Config.Item("DrawReticles").GetValue<Circle>();
            if (drawOrbwalk.Active)
            {
                Utility.DrawCircle(GetOrbwalkPos(), 100, drawOrbwalk.Color);
            }
            if (drawReticles.Active)
            {
                foreach (var existingReticle in ExistingReticles)
                {
                    Utility.DrawCircle(existingReticle.ReticlePos, 100, drawReticles.Color);
                }
            }

            if (GetOrbwalkPos() != Game.CursorPos &&
                (ComboActive || LaneClearActive || Orbwalker.ActiveMode == Orbwalking.OrbwalkingMode.LastHit))
            {
                Utility.DrawCircle(Game.CursorPos, Config.Item("CatchRadius").GetValue<Slider>().Value, Color.Red);
            }
            else
            {
                Utility.DrawCircle(
                    Game.CursorPos, Config.Item("CatchRadius").GetValue<Slider>().Value, Color.CornflowerBlue);
            }

            var drawE = Config.Item("DrawE").GetValue<Circle>();
            if (drawE.Active)
            {
                Utility.DrawCircle(ObjectManager.Player.Position, E.Range, drawE.Color);
            }
            var drawR = Config.Item("DrawR").GetValue<Circle>();
            if (drawR.Active)
            {
                Utility.DrawCircle(ObjectManager.Player.Position, 2000, drawR.Color);
            }
        }

        public override void Game_OnGameUpdate(EventArgs args)
        {
            var orbwalkPos = GetOrbwalkPos();
            var cursor = Game.CursorPos;
            if (orbwalkPos != cursor &&
                (ComboActive || LaneClearActive || Orbwalker.ActiveMode == Orbwalking.OrbwalkingMode.LastHit))
            {
                Orbwalker.SetOrbwalkingPoint(orbwalkPos);
            }
            else
            {
                Orbwalker.SetOrbwalkingPoint(cursor);
            }

            //Combo
            var rtarget = SimpleTs.GetTarget(2000, SimpleTs.DamageType.Physical);
            if (ComboActive)
            {
                var target = SimpleTs.GetTarget(550, SimpleTs.DamageType.Physical);
                if (target == null)
                {
                    return;
                }
                if (W.IsReady() && Config.Item("UseWC").GetValue<bool>() &&
                    ObjectManager.Player.Buffs.FirstOrDefault(
                        buff => buff.Name == "dravenfurybuff" || buff.Name == "DravenFury") == null)
                {
                    W.Cast();
                }
                if (IsFleeing(target) && Config.Item("UseEC").GetValue<bool>())
                {
                    E.Cast(target);
                }

                try
                {
                    if (Config.Item("UseRC").GetValue<bool>() && R.GetHealthPrediction(target) <= 0)
                    {
                        R.Cast(target);
                    }
                }
                catch (Exception e)
                {
                    Console.WriteLine(e);
                    throw;
                }
            }
            //Manual cast R
            if (Config.Item("RManualCast").GetValue<KeyBind>().Active)
            {
                R.Cast(rtarget);
            }


            //Peel from melees
            if (Config.Item("EPeel").GetValue<bool>()) //Taken from ziggs(by pq/esk0r)
            {
                foreach (var pos in from enemy in ObjectManager.Get<Obj_AI_Hero>()
                    where
                        enemy.IsValidTarget() &&
                        enemy.Distance(ObjectManager.Player) <=
                        enemy.BoundingRadius + enemy.AttackRange + ObjectManager.Player.BoundingRadius &&
                        enemy.IsMelee()
                    let direction =
                        (enemy.ServerPosition.To2D() - ObjectManager.Player.ServerPosition.To2D()).Normalized()
                    let pos = ObjectManager.Player.ServerPosition.To2D()
                    select pos + Math.Min(200, Math.Max(50, enemy.Distance(ObjectManager.Player) / 2)) * direction)
                {
                    E.Cast(pos.To3D());
                }
            }
        }

        public override void Orbwalking_AfterAttack(Obj_AI_Base unit, Obj_AI_Base target)
        {
            if (!unit.IsMe)
            {
                return;
            }
            Console.WriteLine("Hai");
            Console.WriteLine(Config.Item("maxqamount").GetValue<Slider>().Value);
            var qOnHero = QBuffCount();
            if (unit.IsMe &&
                ((ComboActive && Config.Item("UseQC").GetValue<bool>()) ||
                 (HarassActive && Config.Item("UseQC").GetValue<bool>())) && qOnHero < 2 &&
                qOnHero + ExistingReticles.Count < Config.Item("maxqamount").GetValue<Slider>().Value)
            {
                Q.Cast();
                Console.WriteLine("Casted Q");
            }
        }

        public override void ComboMenu(Menu config)
        {
            config.AddItem(new MenuItem("UseQC", "Use Q").SetValue(true));
            config.AddItem(new MenuItem("UseWC", "Use W").SetValue(true));
            config.AddItem(new MenuItem("UseEC", "Use E").SetValue(true));
            config.AddItem(new MenuItem("UseRC", "Use R").SetValue(true));
        }

        public override void HarassMenu(Menu config)
        {
            config.AddItem(new MenuItem("UseQH", "Use Q").SetValue(true));
        }

        public override void DrawingMenu(Menu config)
        {
            config.AddItem(
                new MenuItem("DrawE", "E range").SetValue(new Circle(true, Color.FromArgb(100, 255, 0, 255))));
            config.AddItem(
                new MenuItem("DrawR", "R range(2000 units)").SetValue(
                    new Circle(true, Color.FromArgb(100, 255, 0, 255))));
            config.AddItem(
                new MenuItem("DrawOrbwalk", "Draw orbwalk position").SetValue(new Circle(true, Color.Yellow)));
            config.AddItem(new MenuItem("DrawReticles", "Draw on reticles").SetValue(new Circle(true, Color.Green)));
        }

        public override void MiscMenu(Menu config)
        {
            config.AddItem(new MenuItem("maxqamount", "Max Qs to use simultaneous").SetValue(new Slider(2, 4, 1)));
            config.AddItem(new MenuItem("EGapCloser", "Auto E Gap closers").SetValue(true));
            config.AddItem(new MenuItem("EInterruptable", "Auto E interruptable spells").SetValue(true));
            config.AddItem(new MenuItem("RManualCast", "Cast R Manually(2000 range)"))
                .SetValue(new KeyBind("T".ToCharArray()[0], KeyBindType.Press));
            config.AddItem(new MenuItem("Epeel", "Peel self with E").SetValue(true));
            config.AddItem(new MenuItem("CatchRadius", "Axe catch radius").SetValue(new Slider(600, 200, 1000)));
        }

        public static int QBuffCount()
        {
            var buff =
                ObjectManager.Player.Buffs.FirstOrDefault(buff1 => buff1.Name.Equals("dravenspinningattack"));
            return buff != null ? buff.Count : 0;
        }

        public Vector3 GetOrbwalkPos()
        {
            if (ExistingReticles.Count <= 0)
            {
                return Game.CursorPos;
            }
            var myHero = ObjectManager.Player;
            var cursor = Game.CursorPos;
            var reticles =
                ExistingReticles.OrderBy(reticle => reticle.ExpireTime)
                    .FirstOrDefault(
                        reticle =>
                            reticle.ReticlePos.Distance(cursor) <= Config.Item("CatchRadius").GetValue<Slider>().Value &&
                            reticle.Object.IsValid &&
                            myHero.GetPath(reticle.ReticlePos).ToList().To2D().PathLength() / myHero.MoveSpeed + Game.Time <
                            reticle.ExpireTime);

            return reticles != null && myHero.Distance(reticles.ReticlePos) >= 100 ? reticles.ReticlePos : cursor;
        }

        public static bool IsFleeing(Obj_AI_Hero hero)
        {
            var position = E.GetPrediction(hero);
            return position != null &&
                   Vector3.DistanceSquared(ObjectManager.Player.Position, position.CastPosition) >
                   Vector3.DistanceSquared(hero.Position, position.CastPosition);
        }
    }
}