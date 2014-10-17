
#region
using System;
using System.Linq;
using LeagueSharp;
using LeagueSharp.Common;
using SharpDX;
using Color = System.Drawing.Color;
#endregion

namespace Marksman
{
    internal class Varus : Champion
    {
        public static Spell Q, W, E, R;
        private float LastSpellTick;
        
        public Varus()
        {
            Utils.PrintMessage("Varus loaded!");
        
            Q = new Spell(SpellSlot.Q, 1550f);
            W = new Spell(SpellSlot.W);
            E = new Spell(SpellSlot.E, 925f);
            R = new Spell(SpellSlot.R, 1200f);
            
            Q.SetSkillshot(.25f, 70f, float.MaxValue, false, SkillshotType.SkillshotLine);
            E.SetSkillshot(.50f, 250f, 1400f, false, SkillshotType.SkillshotCircle);
            R.SetSkillshot(.25f, 100f, 1950f, false, SkillshotType.SkillshotLine);
            
            Q.SetCharged("VarusQ", "VarusQ", 250, 1550, 1.2f);
            
            Obj_AI_Base.OnProcessSpellCast += Obj_AI_Hero_OnProcessSpellCast;
        }
        
        private void Obj_AI_Hero_OnProcessSpellCast(Obj_AI_Base sender, GameObjectProcessSpellCastEventArgs args)
        {
            if (!sender.IsMe || args.SData.Name.ToLower().Contains("attack"))
                return;
            
            LastSpellTick = Environment.TickCount;
        }
        
        static int EnemyWStackCount
        {
            get
            {
                var xBuffCount = 0;
                foreach (var buff in from enemy in ObjectManager.Get<Obj_AI_Hero>()
                                                                .Where(
                                                                     enemy =>
                                                                             enemy.IsEnemy && !enemy.IsDead && ObjectManager.Player.Distance(enemy) < Q.Range && W.Level > 0)
                                     from buff in enemy.Buffs
                                     where buff.Name.Contains("varuswdebuff")
                                     select buff) 
                {
                    xBuffCount = buff.Count;
                }
                return xBuffCount;
            }
        }
        
        public override void Drawing_OnDraw(EventArgs args)
        {
            var drawQ = GetValue<Circle>("DrawQ");
            var drawE = GetValue<Circle>("DrawE");
            var drawR = GetValue<Circle>("DrawR");
            var drawRS = GetValue<Circle>("DrawRS");
            
            if (drawQ.Active)
                Utility.DrawCircle(ObjectManager.Player.Position, Q.Range, drawQ.Color, 1, 5);
            
            if (drawE.Active)
                Utility.DrawCircle(ObjectManager.Player.Position, E.Range, drawE.Color, 1, 5);
            
            if (drawR.Active)
                Utility.DrawCircle(ObjectManager.Player.Position, R.Range, drawR.Color, 1, 5);
            
            if (GetValue<KeyBind>("CastR").Active && drawRS.Active)
            {
                Vector3 DrawPosition;
                
                if (ObjectManager.Player.Distance(Game.CursorPos) < R.Range - 300f)
                    DrawPosition = Game.CursorPos;
                else
                    DrawPosition = ObjectManager.Player.Position +
                                   Vector3.Normalize(Game.CursorPos - ObjectManager.Player.Position) * (R.Range - 300f);
                
                Utility.DrawCircle(DrawPosition, 300f, drawRS.Color, 1, 5);
            }
        }
        
        private static float CalcWExplodeDamage(Obj_AI_Hero vTarget)
        {
            var wExplodePerStack = new[] { 2f, 2.75f, 3.5f, 4.25f, 5f };
            var xWDamage = EnemyWStackCount * wExplodePerStack[W.Level - 1];
            
            var fxWDamage = vTarget.Health / 100 * xWDamage;
            return fxWDamage;
        }
        
        private static float CalcQDamage(Obj_AI_Hero vTarget)
        {
            var qDamageMinPerLevel = new[] { 10f, 47f, 83f, 120f, 157f };
            var qDamageMaxPerLevel = new[] { 15f, 70f, 125f, 180f, 235f };
            var xDistance = ObjectManager.Player.Distance(vTarget);
            
            var fxQDamage = xDistance < Q.ChargedMaxRange / 2
                            ? qDamageMinPerLevel[Q.Level - 1] + ObjectManager.Player.GetAutoAttackDamage(vTarget)
                            : qDamageMaxPerLevel[Q.Level - 1] + ObjectManager.Player.GetAutoAttackDamage(vTarget) * 1.6;
            
            return (float)fxQDamage;
        }
        
        private static void CastQEnemy(Obj_AI_Hero vTarget)
        {
            if (!Q.IsReady())
                return;
            
            if (Q.IsCharging)
            {
                Q.Cast(vTarget, false, true);
            }
            else
            { 
                Q.StartCharging();
            }
        }
        
        public override void Game_OnGameUpdate(EventArgs args)
        {
            if (GetValue<KeyBind>("CastR").Active)
            {
                Vector3 searchPos;
                
                if (ObjectManager.Player.Distance(Game.CursorPos) < R.Range - 300f)
                    searchPos = Game.CursorPos;
                else
                    searchPos = ObjectManager.Player.Position +
                                Vector3.Normalize(Game.CursorPos - ObjectManager.Player.Position) * (R.Range - 300f);
                
                var rTarget =
                    ObjectManager.Get<Obj_AI_Hero>()
                                 .Where(hero => hero.IsValidTarget(R.Range) && hero.Distance(searchPos) < 300f)
                                 .OrderByDescending(SimpleTs.GetPriority)
                                 .First();
                
                if (rTarget != null && R.IsReady())
                    R.Cast(rTarget);
            }
            if (ComboActive || HarassActive)
            {
                var useQ = GetValue<StringList>("UseQ" + (ComboActive ? "C" : "H"));
                var useW = GetValue<Slider>("UseW" + (ComboActive ? "C" : "H"));
                var useE = GetValue<bool>("UseE" + (ComboActive ? "C" : "H"));
                
                var qTarget = SimpleTs.GetTarget(Q.Range, SimpleTs.DamageType.Physical);
                var eTarget = SimpleTs.GetTarget(E.Range, SimpleTs.DamageType.Physical);
                
                // if (WBuff.Count < useW.Value && !Q.IsCharging)
                //     return;
                //var wBuff = qTarget.Buffs.Count(buff => buff.Name == "varuswdebuff");
                
                if (qTarget.Health < CalcQDamage(qTarget) + CalcWExplodeDamage(qTarget))
                    CastQEnemy(qTarget);
                else
                {
                    switch (useQ.SelectedIndex)
                    {
                        case 1: /* [ Use Q everytime ] */
                            {
                                CastQEnemy(qTarget);
                                break;
                            }
                        case 2: /* [ Use Q with W Stack Option Count ] */
                            {
                                if (EnemyWStackCount == useW.Value) 
                                    CastQEnemy(qTarget);
                                break;
                            }
                        
                        case 3: /* [ Use Q with W Max Stack ] */
                            {
                                if (EnemyWStackCount == 3)
                                    CastQEnemy(qTarget);
                                break;
                            }
                    }
                }
                
                if (useE && E.IsReady() && eTarget != null)
                    E.Cast(eTarget, false, true);
            }
        }
        
        public override void Orbwalking_BeforeAttack(Orbwalking.BeforeAttackEventArgs args)
        {
            args.Process = !Q.IsCharging;
        }
        
        #region Menus
        
        public override bool ComboMenu(Menu config)
        {
            config.AddItem(
                new MenuItem("UseQC" + Id, "Q").SetValue(
                new StringList(new[] { "Off", "Everytime", "W Stack Value", "Max W Stack" }, 3)));
            config.AddItem(new MenuItem("UseWC" + Id, "W").SetValue(new Slider(3, 1, 3)));
            config.AddItem(new MenuItem("UseEC" + Id, "E").SetValue(true));
            
            return true;
        }
        
        public override bool HarassMenu(Menu config)
        {
            config.AddItem(
                new MenuItem("UseQH" + Id, "Q").SetValue(
                new StringList(new[] { "Off", "Everytime", "W Stack Value", "Max W Stack" }, 3)));
            config.AddItem(new MenuItem("UseEH" + Id, "E").SetValue(true));
            
            return true;
        }
        
        public override bool MiscMenu(Menu config)
        {
            config.AddItem(new MenuItem("spellDelay" + Id, "Spell delay").SetValue(new Slider(500, 0, 3000)));
            config.AddItem(
                new MenuItem("CastR" + Id, "Cast R").SetValue(new KeyBind("T".ToCharArray()[0], KeyBindType.Press)));
            
            return true;
        }
        
        public override bool DrawingMenu(Menu config)
        {
            config.AddItem(new MenuItem("DrawQ" + Id, "Q").SetValue(new Circle(true, Color.DarkGray)));
            config.AddItem(new MenuItem("DrawE" + Id, "E").SetValue(new Circle(true, Color.DarkGray)));
            config.AddItem(new MenuItem("DrawR" + Id, "R").SetValue(new Circle(true, Color.DarkGray)));
            config.AddItem(new MenuItem("DrawRS" + Id, "R: Search Area").SetValue(new Circle(true, Color.White)));
            
            return true;
        }
    
        #endregion
    }
}
