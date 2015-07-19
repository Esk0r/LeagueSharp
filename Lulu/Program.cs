using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LeagueSharp;
using LeagueSharp.Common;

namespace Lulu
{
    class Program
    {
        private static string ChampionName = "Lulu";
        private static Obj_AI_Hero Player;
        public static Menu Config;
        public static Orbwalking.Orbwalker Orbwalker;

        public static Spell Q;
        public static Spell W;
        public static Spell E;
        public static Spell R;

        public static SpellSlot IgniteSlot;

        public static int nextJungleScan = 0;
        public static string[] jungleMobNames = new[] { "sru_blue", "sru_dragon", "sru_baron" };

        static void Main(string[] args)
        {
            if (Game.Mode == GameMode.Running)
            {
                GameOnOnStart(new EventArgs());
            }

            Game.OnStart += GameOnOnStart;
        }

        private static void GameOnOnStart(EventArgs args)
        {
            Player = ObjectManager.Player;
            if (Player.ChampionName != ChampionName)
                return;

            PixManager.DrawPix = true;
            
            Q = new Spell(SpellSlot.Q, 950);
            Q.SetSkillshot(0.25f, 60, 1450, false, SkillshotType.SkillshotLine);
            W = new Spell(SpellSlot.W, 650);
            E = new Spell(SpellSlot.E, 650);
            R = new Spell(SpellSlot.R, 900);

            IgniteSlot = Player.GetSpellSlot("SummonerDot");

            //Create menu
            Config = new Menu(ChampionName, ChampionName, true);
            TargetSelector.AddToMenu(Config.SubMenu("Target Selector"));
            Orbwalker = new Orbwalking.Orbwalker(Config.SubMenu("Orbwalking"));

            Config.SubMenu("Combo")
                .AddItem(
                    new MenuItem("ComboActive", "Combo!").SetValue(
                        new KeyBind(Config.Item("Orbwalk").GetValue<KeyBind>().Key, KeyBindType.Press)));

            Config.SubMenu("Farm").AddItem(new MenuItem("UseQFarm", "Use Q").SetValue(true));
            Config.SubMenu("Farm").AddItem(new MenuItem("UseEFarm", "Use E").SetValue(true));
            Config.SubMenu("Farm")
                .AddItem(
                    new MenuItem("FarmActive", "Farm!").SetValue(
                        new KeyBind(Config.Item("LaneClear").GetValue<KeyBind>().Key, KeyBindType.Press)));

            Config.SubMenu("JungleFarm").AddItem(new MenuItem("UseQJFarm", "Use Q").SetValue(true));
            Config.SubMenu("JungleFarm").AddItem(new MenuItem("UseEJFarm", "Use E").SetValue(true));
            Config.SubMenu("JungleFarm")
                .AddItem(
                    new MenuItem("JungleFarmActive", "JungleFarm!").SetValue(
                        new KeyBind(Config.Item("LaneClear").GetValue<KeyBind>().Key, KeyBindType.Press)));

            Config.SubMenu("W").AddItem(new MenuItem("InterruptSpellsW", "Interrupt spells using W").SetValue(true));

            Config.SubMenu("W").AddItem(new MenuItem("WKite", "Use W to kite").SetValue(true));
            Config.SubMenu("W").AddItem(new MenuItem("WKiteD", "W Kite distance").SetValue(new Slider(300, 0, 500)));

            Config.SubMenu("E").AddItem(new MenuItem("AutoE", "KS with E").SetValue(true));
            Config.SubMenu("E").AddItem(new MenuItem("AutoEMobs", "KS Blue / Drake / Baron with E").SetValue(true));

            Config.SubMenu("R").AddItem(new MenuItem("InterruptSpellsR", "Interrupt dangerous spells using R").SetValue(true));
            Config.SubMenu("R").AddItem(new MenuItem("AutoR", "Auto R AOE").SetValue(true));

            Config.AddToMainMenu();

            Game.OnUpdate += Game_OnUpdate;
            Interrupter2.OnInterruptableTarget += Interrupter2_OnInterruptableTarget;
        }

        static void Interrupter2_OnInterruptableTarget(Obj_AI_Hero sender, Interrupter2.InterruptableTargetEventArgs args)
        {
            if (Config.SubMenu("W").Item("InterruptSpellsW").GetValue<bool>())
            {
                if (W.IsReady() && sender.IsValidTarget(W.Range))
                {
                    W.Cast(sender);
                    return;
                }
            }

            if (Config.SubMenu("R").Item("InterruptSpellsR").GetValue<bool>() && args.DangerLevel > Interrupter2.DangerLevel.Medium)
            {
                if (R.IsReady())
                {
                    foreach (var ally in HeroManager.Allies)
                    {
                        if (ally.IsValidTarget(R.Range, false))
                        {
                            if (ally.Distance(sender, true) < 300 * 300)
                            {
                                R.Cast(ally);
                            }
                        }
                    }

                    if (Player.Distance(sender, true) < 300 * 300)
                    {
                        R.Cast(Player);
                    }
                }
                return;
            }
        }

        static void Game_OnUpdate(EventArgs args)
        {
            if (Config.Item("ComboActive").GetValue<KeyBind>().Active)
            {
                Combo();
            }
            else
            {
                if (Config.Item("JungleFarmActive").GetValue<KeyBind>().Active)
                {
                    JungleFarm();
                }
                if (Config.Item("FarmActive").GetValue<KeyBind>().Active)
                {
                    Farm();
                }
            }

            if (Config.SubMenu("E").Item("AutoE").GetValue<bool>()) 
            {
                ImABitch();
            }

            if (Config.SubMenu("E").Item("AutoEMobs").GetValue<bool>())
            {
                JungleKS();
            }

            if (Config.SubMenu("W").Item("WKite").GetValue<bool>() && W.IsReady())
            {
                var d = Config.SubMenu("W").Item("WKiteD").GetValue<Slider>();
                if (Player.CountEnemiesInRange(d.Value) >= 1)
                {
                    W.Cast(Player);
                }
            }

            if (Config.SubMenu("R").Item("AutoR").GetValue<bool>())
            {
                foreach (var ally in HeroManager.Allies)
                {
                    if (ally.IsValidTarget(R.Range, false))
                    {
                        var c = ally.CountEnemiesInRange(300);
                        if (c >= 1 + 1 + 1 || ally.HealthPercent <= 15 && c >= 1)
                        {
                            R.Cast(ally);
                        }
                    }
                }

                var ec = Player.CountEnemiesInRange(300);
                if (ec >= 1 + 1 + 1 || Player.HealthPercent <= 15 && ec >= 1)
                {
                    R.Cast(Player);
                }    
            }
        }

        static void ShootQ(bool useE = true)
        {
            if (!Q.IsReady())
            {
                return;
            }

            Obj_AI_Base pixTarget = null;
            if (PixManager.Pix != null)
            {
                pixTarget = TargetSelector.GetTarget(PixManager.Pix, Q.Range, TargetSelector.DamageType.Magical);
            }

            Obj_AI_Base luluTarget = TargetSelector.GetTarget(Q.Range, TargetSelector.DamageType.Magical);

            var pixTargetEffectiveHealth = pixTarget != null ? pixTarget.Health * (1 + pixTarget.SpellBlock / 100f)  :
            float.MaxValue;
            var luluTargetEffectiveHealth = luluTarget != null ? luluTarget.Health * (1 + luluTarget.SpellBlock / 100f) :
            float.MaxValue;

            var target = pixTargetEffectiveHealth * 1.2f > luluTargetEffectiveHealth ? luluTarget : pixTarget;
            var flag = false;
            Spell.CastStates qCastState = Spell.CastStates.OutOfRange;
            if (target != null)
            {
                var distanceToTargetFromPlayer = Player.Distance(target, true);
                var distanceToTargetFromPix = PixManager.Pix != null ? PixManager.Pix.Distance(target, true) : float.MaxValue;

                var source = PixManager.Pix == null ? Player : ( distanceToTargetFromPix < distanceToTargetFromPlayer ? PixManager.Pix : Player);
                Q.From = source.ServerPosition;
                Q.RangeCheckFrom = source.ServerPosition;
                if (!useE || !E.IsReady() || Q.From.Distance(target.ServerPosition) < Q.Range - 100)
                {
                    qCastState = Q.Cast(target);
                }
                flag = true;
            }

            if (qCastState == Spell.CastStates.OutOfRange)   //or outofrange
            {
                if (useE && E.IsReady())
                {
                    var eqTarget = TargetSelector.GetTarget(Q.Range + E.Range, TargetSelector.DamageType.Magical);
                    if (eqTarget != null)
                    {
                        var eTarget =
                            ObjectManager.Get<Obj_AI_Base>()
                                .Where(t => t.IsValidTarget(E.Range) && t.Distance(eqTarget, true) < Q.RangeSqr && !E.IsKillable(eqTarget)).MinOrDefault(t => t.Distance(eqTarget, true));
                        if (eTarget != null)
                        {
                            E.Cast(eTarget);
                            return;
                        }
                    }
                }

                if (flag)
                {
                    qCastState = Q.Cast(target);
                }
            }
        }                                                                

        static void Combo()
        {
            ShootQ();

            var eTarget = TargetSelector.GetTarget(E.Range, TargetSelector.DamageType.Magical);
            if (eTarget != null)
            {
                E.Cast(eTarget);
            }

            var comboDamage = GetComboDamage(eTarget);

            if (eTarget != null && Player.Distance(eTarget) < 600 && IgniteSlot != SpellSlot.Unknown &&
                Player.Spellbook.CanUseSpell(IgniteSlot) == SpellState.Ready)
            {
                if ( comboDamage > eTarget.Health)
                {
                    Player.Spellbook.CastSpell(IgniteSlot, eTarget);
                }
            }
        }

        private static void Farm()
        {
            var useQ = Config.Item("UseQFarm").GetValue<bool>();
            var useE = Config.Item("UseEFarm").GetValue<bool>();

            var allMinions = MinionManager.GetMinions(ObjectManager.Player.ServerPosition, Q.Range,
                MinionTypes.All);

            if (useQ)
            {
                Q.From = Player.ServerPosition;
                Q.RangeCheckFrom = Player.ServerPosition;

                var fl = Q.GetLineFarmLocation(allMinions);
                if (fl.MinionsHit >= 2)
                {
                    Q.Cast(fl.Position);
                }    
            }

            if (useE)
            {
                foreach (var minion in allMinions.Where(m => m.BaseSkinName.EndsWith("MinionSiege") && E.IsKillable(m)))
                {
                    E.Cast(minion);
                }
            }
        }


        static void JungleFarm()
        {
            var useQ = Config.Item("UseQJFarm").GetValue<bool>();
            var useE = Config.Item("UseEJFarm").GetValue<bool>();

            var mobs = MinionManager.GetMinions(Player.ServerPosition, E.Range, MinionTypes.All,
                MinionTeam.Neutral, MinionOrderTypes.MaxHealth);

            if (mobs.Count > 0)
            {
                var mob = mobs[0];
                if (useQ && Q.IsReady())
                {
                    Q.Cast(mob.Position);
                }
                else if (useE && E.IsReady())
                {
                    E.Cast(mob);
                }
            }    
        }

        static void JungleKS()
        {
            if (Utils.GameTimeTickCount - nextJungleScan <= 0)
            {
                return;
            }

            var mob =
                ObjectManager.Get<Obj_AI_Minion>()
                    .Where(m => m.Team == GameObjectTeam.Neutral && m.IsValidTarget(E.Range))
                    .MaxOrDefault(m => m.MaxHealth);

            if (mob != null && jungleMobNames.Contains(mob.BaseSkinName.ToLowerInvariant()) && E.IsKillable(mob))
            {
                E.Cast(mob);
            }

            if (mob == null)
            {
                nextJungleScan = Utils.GameTimeTickCount + 300;    
            }
        }

        static void ImABitch()
        {
            foreach (var enemy in HeroManager.Enemies.Where(e => e.IsValidTarget(E.Range) && E.IsKillable(e)))
            {
                E.Cast(enemy);
            }
        }

        public static float GetComboDamage(Obj_AI_Hero target)
        {
            var result = 0f;

            if (target == null)
            {
                return 0f;
            }

            if (Q.IsReady())
            {
                result += 2 * Q.GetDamage(target);
            }

            if (E.IsReady())
            {
                result +=  E.GetDamage(target);
            }

            if (IgniteSlot != SpellSlot.Unknown && Player.Spellbook.CanUseSpell(IgniteSlot) == SpellState.Ready)
                result += (float)ObjectManager.Player.GetSummonerSpellDamage(target, Damage.SummonerSpell.Ignite);

            result += 3 * (float)Player.GetAutoAttackDamage(target);

            return result;
        }

    }
}
