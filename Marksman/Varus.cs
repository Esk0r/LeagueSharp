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
           
            Q = new Spell(SpellSlot.Q, 1600f);
            W = new Spell(SpellSlot.W);
            E = new Spell(SpellSlot.E, 925f);
            R = new Spell(SpellSlot.R, 1200f);

            Q.SetSkillshot(.25f, 70f, 1650f, false, SkillshotType.SkillshotLine);
            E.SetSkillshot(.50f, 250f, 1400f, false, SkillshotType.SkillshotCircle);
            R.SetSkillshot(.25f, 100f, 1950f, false, SkillshotType.SkillshotLine);

            Q.SetCharged("VarusQ", "VarusQ", 250, 1600, 1.2f);
             
            Utility.HpBarDamageIndicator.DamageToUnit = GetComboDamage;
            Utility.HpBarDamageIndicator.Enabled = true;

            Obj_AI_Base.OnProcessSpellCast += Obj_AI_Hero_OnProcessSpellCast;
        }

        private float GetComboDamage(Obj_AI_Hero t)
        {
            float fComboDamage = 0f;

            if (Q.IsReady())
                fComboDamage += (float)ObjectManager.Player.GetSpellDamage(t, SpellSlot.Q); 
            //fComboDamage += CalcQDamage;

            if (W.Level >0)
                fComboDamage += (float)ObjectManager.Player.GetSpellDamage(t, SpellSlot.W);

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

        private void Obj_AI_Hero_OnProcessSpellCast(Obj_AI_Base sender, GameObjectProcessSpellCastEventArgs args)
        {
            if (!sender.IsMe || args.SData.Name.ToLower().Contains("attack"))
                return;

            LastSpellTick = Environment.TickCount;
        }

        public static int EnemyWStackCount(Obj_AI_Hero t)
        {
            return
                t.Buffs.Where(xBuff => xBuff.Name == "varuswdebuff" && t.IsValidTarget(Q.Range))
                    .Select(xBuff => xBuff.Count)
                    .FirstOrDefault();
        }

        public override void Drawing_OnDraw(EventArgs args)
        {
            var drawQ = GetValue<Circle>("DrawQ");
            var drawE = GetValue<Circle>("DrawE");
            var drawR = GetValue<Circle>("DrawR");
            var drawQc = GetValue<Circle>("DrawQC");
            var drawRs = GetValue<Circle>("DrawRS");

            if (drawQ.Active)
                Render.Circle.DrawCircle(ObjectManager.Player.Position, Q.Range, drawQ.Color);

            if (drawE.Active)
                Render.Circle.DrawCircle(ObjectManager.Player.Position, E.Range, drawE.Color);

            if (drawQc.Active)
                Render.Circle.DrawCircle(ObjectManager.Player.Position, GetValue<Slider>("QMinChargeC").Value,
                    drawQc.Color);

            if (drawR.Active)
                Render.Circle.DrawCircle(ObjectManager.Player.Position, R.Range, drawR.Color);
            
            if (GetValue<KeyBind>("CastR").Active && drawRs.Active)
            {
                Vector3 drawPosition;

                if (ObjectManager.Player.Distance(Game.CursorPos) < R.Range - 300f)
                    drawPosition = Game.CursorPos;
                else
                    drawPosition = ObjectManager.Player.Position +
                                   Vector3.Normalize(Game.CursorPos - ObjectManager.Player.Position)*(R.Range - 300f);

                Render.Circle.DrawCircle(drawPosition, 300f, drawRs.Color);
            }
        }

        static float CalcWDamage
        {
            get
            {
                var t = TargetSelector.GetTarget(Q.Range, TargetSelector.DamageType.Physical);
                var xEnemyWStackCount = EnemyWStackCount(t);
                var wExplodePerStack = ObjectManager.Player.GetSpellDamage(t, SpellSlot.W, 1) * xEnemyWStackCount > 0
                    ? xEnemyWStackCount
                    : 1;
                return (float) wExplodePerStack;
            }
        }

        static float CalcQDamage
        {
            get
            {
                var t = TargetSelector.GetTarget(Q.Range, TargetSelector.DamageType.Physical);
                
                if (!Q.IsReady())
                    return 0;

                /*
                var qDamageMaxPerLevel = new[] {15f, 70f, 125f, 180f, 235f};
                var fxQDamage2 = qDamageMaxPerLevel[Q.Level - 1] +
                                 1.6*
                                 (ObjectManager.Player.BaseAttackDamage + ObjectManager.Player.FlatPhysicalDamageMod);

                var xDis = ObjectManager.Player.Distance(t)/Q.ChargedMaxRange;
                return (float) fxQDamage2*xDis;
                */
                var fxQDamage2 = ObjectManager.Player.GetSpellDamage(t, SpellSlot.Q, 1);
                return (float) fxQDamage2;
            }
        }

        private static void CastSpellQ()
        {
            if (!Q.IsReady())
                return;

            var t = TargetSelector.GetTarget(Q.Range, TargetSelector.DamageType.Physical);
            if (!t.IsValidTarget(Q.Range)) 
                return;

            var qMinCharge = Program.Config.Item("QMinChargeC").GetValue<Slider>().Value;
            ObjectManager.Player.IssueOrder(GameObjectOrder.MoveTo, Game.CursorPos);
            
            if (Q.IsCharging)
            {
                if (Q.Range >= qMinCharge)
                Q.Cast(t, false, true);
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
                                Vector3.Normalize(Game.CursorPos - ObjectManager.Player.Position)*(R.Range - 300f);

                var rTarget =
                    ObjectManager.Get<Obj_AI_Hero>()
                        .Where(hero => hero.IsValidTarget(R.Range) && hero.Distance(searchPos) < 300f)
                        .OrderByDescending(TargetSelector.GetPriority)
                        .First();

                if (rTarget != null && R.IsReady())
                    R.Cast(rTarget);
            }

            if (GetValue<KeyBind>("UseQ2C").Active)
            {
                ObjectManager.Player.IssueOrder(GameObjectOrder.MoveTo, Game.CursorPos);
                CastSpellQ();
            }

            Obj_AI_Hero t;

            if (E.IsReady() && GetValue<KeyBind>("UseETH").Active)
            {
                if(ObjectManager.Player.HasBuff("Recall"))
                    return;
                t = TargetSelector.GetTarget(E.Range, TargetSelector.DamageType.Physical);
                if (t != null)
                    E.Cast(t, false, true);
            }           

            if (!ComboActive && !HarassActive) return;

            var useQ = GetValue<StringList>("UseQ" + (ComboActive ? "C" : "H"));
            var useE = GetValue<bool>("UseE" + (ComboActive ? "C" : "H"));
            var useR = GetValue<bool>("UseRC");
            
            t = TargetSelector.GetTarget(Q.Range, TargetSelector.DamageType.Physical);

            if (t.IsValidTarget(Q.Range) && t.Health <= CalcQDamage + CalcWDamage)
                CastSpellQ();

            switch (useQ.SelectedIndex)
            {
                case 1:
                    {
                        CastSpellQ();
                        break;
                    }
                case 2:
                    {
                        if (EnemyWStackCount(t) > 2 || W.Level == 0)
                            CastSpellQ();
                        break;
                    }
            }

            if (useE && E.IsReady())
            {
                t = TargetSelector.GetTarget(E.Range, TargetSelector.DamageType.Physical);
                if (t.IsValidTarget(E.Range))
                    E.Cast(t, false, true);
            }

            if (useR && R.IsReady())
            {
                t = TargetSelector.GetTarget(R.Range, TargetSelector.DamageType.Physical);
                if (t.IsValidTarget(R.Range) && t.Health <= ObjectManager.Player.GetSpellDamage(t, SpellSlot.R) - 30f) 
                    R.Cast(t);
            }
        }

        public override void Orbwalking_BeforeAttack(Orbwalking.BeforeAttackEventArgs args)
        {
            args.Process = !Q.IsCharging;
        }

        public override bool ComboMenu(Menu config)
        {
            config.AddItem(
                new MenuItem("UseQC" + Id, "Q Mode").SetValue(new StringList(new[] {"Off", "Use Allways", "Max W Stack = 3"}, 0)));
            config.AddItem(new MenuItem("UseEC" + Id, "Use E").SetValue(true));
            config.AddItem(new MenuItem("UseRC" + Id, "Use R").SetValue(true));

            config.AddItem(
                new MenuItem("QMinChargeC", "Min. Q Charge").SetValue(new Slider(1000, Q.ChargedMinRange,
                    Q.ChargedMaxRange)));
            config.AddItem(
                new MenuItem("UseQ2C" + Id, "Use Insta Q").SetValue(new KeyBind("J".ToCharArray()[0], KeyBindType.Press)));
            return true;
        }

        public override bool HarassMenu(Menu config)
        {
            config.AddItem(
                new MenuItem("UseQH" + Id, "Q").SetValue(new StringList(
                    new[] {"Off", "Use Allways", "Max W Stack = 3"}, 0)));
            config.AddItem(new MenuItem("UseEH" + Id, "E").SetValue(true));
            config.AddItem(
                new MenuItem("UseETH" + Id, "Use E (Toggle)").SetValue(new KeyBind("H".ToCharArray()[0],
                    KeyBindType.Toggle)));

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
            config.AddItem(new MenuItem("DrawQC" + Id, "Min. Q Charge").SetValue(new Circle(true, Color.White)));
            config.AddItem(new MenuItem("DrawRS" + Id, "R: Search Area").SetValue(new Circle(true, Color.White)));

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
