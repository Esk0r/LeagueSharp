#region

using System;
using System.Diagnostics.Eventing.Reader;
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
        
        public Vayne()
        {
            Utils.PrintMessage("Vayne loaded");
            
            Q = new Spell(SpellSlot.Q);
            E = new Spell(SpellSlot.E);

            Q.Range = 300f;
            E.SetTargetted(0.25f, 2200f);

            AntiGapcloser.OnEnemyGapcloser += AntiGapcloser_OnEnemyGapcloser;
            Interrupter.OnPossibleToInterrupt += Interrupter_OnPossibleToInterrupt;
        }

        public void AntiGapcloser_OnEnemyGapcloser(ActiveGapcloser gapcloser)
        {
            if (E.IsReady() && gapcloser.Sender.IsValidTarget(E.Range))
                E.CastOnUnit(gapcloser.Sender);
        }

        public void Interrupter_OnPossibleToInterrupt(Obj_AI_Base unit, InterruptableSpell spell)
        {
            if (GetValue<bool>("UseEInterrupt") && unit.IsValidTarget(550f))
                E.Cast(unit);
        }

        private static Obj_AI_Hero GetSilverBuffCountX(Obj_AI_Hero t)
        {

            foreach (var enemy in ObjectManager.Get<Obj_AI_Hero>().Where(enemy => enemy.IsEnemy && enemy.IsValidTarget(800)))
            {
                var buff = enemy.Buffs.Where(bx => bx.Name.Contains("vaynesilvereddebuf"));
                if (buff.Count()>1)
                {
                 return enemy;      
                }

            }
            
            return (from enemy in ObjectManager.Get<Obj_AI_Hero>().Where(enemy => enemy.IsEnemy && enemy.ChampionName == t.ChampionName)
                from buff in enemy.Buffs
                where buff.Name.Contains("vaynesilvereddebuf")
                select buff).Select(buff => buff.Count == 2 ? t : null).FirstOrDefault();
        }

        static int GetSilverBuffCount
        {
            get
            {
                var xBuffCount = 0;
                foreach (var buff in from enemy in ObjectManager.Get<Obj_AI_Hero>().Where(enemy => enemy.IsEnemy)
                    from buff in enemy.Buffs
                    where buff.Name.Contains("vaynesilvereddebuf")
                    select buff) 
                {
                    xBuffCount = buff.Count;
                }
                
                 return xBuffCount;
            }
        }
        public override void Game_OnGameUpdate(EventArgs args)
        {
            if ((!ComboActive && !HarassActive) || !Orbwalking.CanMove(100))
            {

                var useE = GetValue<bool>("UseE" + (ComboActive ? "C" : "H"));

                if (Q.IsReady() && GetValue<bool>("CompleteSilverBuff"))
                {
                    var t =
                        GetSilverBuffCountX(TargetSelector.GetTarget(Q.Range + ObjectManager.Player.AttackRange,
                            TargetSelector.DamageType.Physical));
                    if (t != null)
                    {
                        Q.Cast(Game.CursorPos);
                    }
                }

                if (E.IsReady() && useE)
                {
                    foreach (
                        var hero in
                            from hero in ObjectManager.Get<Obj_AI_Hero>().Where(hero => hero.IsValidTarget(550f))
                            let prediction = E.GetPrediction(hero)
                            where NavMesh.GetCollisionFlags(
                                prediction.UnitPosition.To2D()
                                    .Extend(ObjectManager.Player.ServerPosition.To2D(),
                                        -GetValue<Slider>("PushDistance").Value)
                                    .To3D())
                                .HasFlag(CollisionFlags.Wall) || NavMesh.GetCollisionFlags(
                                    prediction.UnitPosition.To2D()
                                        .Extend(ObjectManager.Player.ServerPosition.To2D(),
                                            -(GetValue<Slider>("PushDistance").Value/2))
                                        .To3D())
                                    .HasFlag(CollisionFlags.Wall)
                            select hero)
                    {
                        E.Cast(hero);
                    }
                }
            }

            if (LaneClearActive)
            {
                bool useQ = GetValue<bool>("UseQL");

                if (Q.IsReady() && useQ)
                {
                    var vMinions = MinionManager.GetMinions(ObjectManager.Player.Position, Q.Range);
                    foreach (
                        Obj_AI_Base minions in
                            vMinions.Where(
                                minions => minions.Health < ObjectManager.Player.GetSpellDamage(minions, SpellSlot.Q)))
                        Q.Cast(minions);
                }
            }
        }

        public override void Orbwalking_AfterAttack(AttackableUnit unit, AttackableUnit target)
        {
            var useQ =
                GetValue<bool>("UseQ" +
                               (ComboActive
                                   ? Orbwalker.ActiveMode == Orbwalking.OrbwalkingMode.Combo ? "C" : ""
                                   : Orbwalker.ActiveMode == Orbwalking.OrbwalkingMode.Mixed ? "H" : ""));
            //if (unit.IsMe && Orbwalker.ActiveMode == Orbwalking.OrbwalkingMode.Combo && GetValue<bool>("UseQC"))
            if (unit.IsMe && useQ)
                Q.Cast(Game.CursorPos);
 
        }

        public override bool ComboMenu(Menu config)
        {
            config.AddItem(new MenuItem("UseQC" + Id, "Use Q").SetValue(true));
            config.AddItem(new MenuItem("UseEC" + Id, "Use E").SetValue(true));
            return true;
        }

        public override bool HarassMenu(Menu config)
        {
            config.AddItem(new MenuItem("UseQH" + Id, "Use Q").SetValue(true));
            config.AddItem(new MenuItem("UseEH" + Id, "Use E").SetValue(true));
            return true;
        }
        public override bool MiscMenu(Menu config)
        {
            config.AddItem(
                new MenuItem("UseET" + Id, "Use E (Toggle)").SetValue(
                    new KeyBind("T".ToCharArray()[0], KeyBindType.Toggle)));
            config.AddItem(new MenuItem("UseEInterrupt" + Id, "Use E To Interrupt").SetValue(true));
            config.AddItem(
                new MenuItem("PushDistance" + Id, "E Push Distance").SetValue(new Slider(425, 475, 300)));
            config.AddItem(new MenuItem("CompleteSilverBuff" + Id, "Complete Silver Buff With Q").SetValue(true));
            return true;
        }

        public override bool ExtrasMenu(Menu config)
        {

            return true;
        }
        public override bool LaneClearMenu(Menu config)
        {
            config.AddItem(new MenuItem("UseQL" + Id, "Use Q").SetValue(true));
            return true;
        }

    }
}
