#region

using System;
using System.Linq;
using LeagueSharp;
using LeagueSharp.Common;
using SharpDX;

#endregion

namespace Marksman
{
    internal class Varus : Champion
    {
        public Spell Q, E, R;
        private float LastSpellTick; 

        public Varus()
        {
            Utils.PrintMessage("Varus loaded!");

            Q = new Spell(SpellSlot.Q, 1600f);
            E = new Spell(SpellSlot.E, 925f);
            R = new Spell(SpellSlot.R, 1200f);

            Q.SetSkillshot(0.25f, 70f, 1900f, false, SkillshotType.SkillshotLine);
            E.SetSkillshot(1f, 235f, 1500f, false, SkillshotType.SkillshotCircle);
            R.SetSkillshot(0.25f, 100f, 1950f, false, SkillshotType.SkillshotLine);

            Q.SetCharged("VarusQ", "VarusQ", 1100, 1600, 1.2f);

            Obj_AI_Hero.OnProcessSpellCast += Obj_AI_Hero_OnProcessSpellCast;
        }

        void Obj_AI_Hero_OnProcessSpellCast(Obj_AI_Base sender, GameObjectProcessSpellCastEventArgs args)
        {
            if (!sender.IsMe || args.SData.Name.ToLower().Contains("attack"))
                return;

            LastSpellTick = Environment.TickCount;
        }

        public override void Drawing_OnDraw(EventArgs args)
        {
            var drawQ = GetValue<Circle>("DrawQ");
            var drawE = GetValue<Circle>("DrawE");
            var drawR = GetValue<Circle>("DrawR");
            var drawRS = GetValue<Circle>("DrawRS");

            if (drawQ.Active)
                Utility.DrawCircle(ObjectManager.Player.Position, Q.Range, drawQ.Color);

            if (drawE.Active)
                Utility.DrawCircle(ObjectManager.Player.Position, E.Range, drawE.Color);

            if (drawR.Active)
                Utility.DrawCircle(ObjectManager.Player.Position, R.Range, drawR.Color);
            
            if (GetValue<KeyBind>("CastR").Active && drawRS.Active)
            {
                Vector3 DrawPosition;

                if (ObjectManager.Player.Distance(Game.CursorPos) < R.Range - 300f)
                    DrawPosition = Game.CursorPos;
                else
                    DrawPosition = ObjectManager.Player.Position + Vector3.Normalize(Game.CursorPos - ObjectManager.Player.Position) * (R.Range - 300f);

                Utility.DrawCircle(DrawPosition, 300f, drawRS.Color);
            }
        }

        public override void Game_OnGameUpdate(EventArgs args)
        {
            Console.Clear();
            foreach(var Enemy in ObjectManager.Get<Obj_AI_Hero>().First(hero => hero.IsValidTarget(1000f)).Buffs)
            {
                Console.WriteLine(Enemy.DisplayName);
            }
            if (GetValue<KeyBind>("CastR").Active)
            {
                Vector3 SearchPos;

                if (ObjectManager.Player.Distance(Game.CursorPos) < R.Range - 300f)
                    SearchPos = Game.CursorPos;
                else
                    SearchPos = ObjectManager.Player.Position + Vector3.Normalize(Game.CursorPos - ObjectManager.Player.Position) * (R.Range - 300f);

                var RTarget = ObjectManager.Get<Obj_AI_Hero>().Where(hero => hero.IsValidTarget(R.Range) && hero.Distance(SearchPos) < 300f).OrderByDescending(hero => SimpleTs.GetPriority(hero)).First();
                
                if (RTarget != null)
                    R.Cast(RTarget);
            }

            if (Environment.TickCount < LastSpellTick + GetValue<Slider>("spellDelay").Value)
                return;

            if (ComboActive || HarassActive || Q.IsCharging)
            {
                
                if (HarassActive && Orbwalker.GetTarget().IsMinion)
                    return;

                var useQ = GetValue<StringList>("UseQ" + (ComboActive ? "C" : "H"));
                var useW = GetValue<Slider>("UseW" + (ComboActive ? "C" : "H"));
                var useE = GetValue<bool>("UseE" + (ComboActive ? "C" : "H"));

                var Target = SimpleTs.GetTarget(Q.Range, SimpleTs.DamageType.Physical);
                var WBuff = Target.Buffs.FirstOrDefault(buff => buff.Name == "varuswdebuff");

                if (WBuff.Count < useW.Value && !Q.IsCharging)
                    return;

                if (Q.IsReady() && useQ.SelectedIndex > 0 && Target.IsValidTarget(useQ.SelectedIndex > 1 ? Q.Range : Q.ChargedMinRange))
                {
                    if ((useQ.SelectedIndex == 1) || (useQ.SelectedIndex == 2 && Q.Range == Q.ChargedMaxRange))
                        Q.Cast(Target);
                }
                else if (E.IsReady() && Target.IsValidTarget(E.Range + (E.Width / 2)))
                    E.Cast(Target);
            }
        }

        public override void Orbwalking_BeforeAttack(Orbwalking.BeforeAttackEventArgs args)
        {
            args.Process = !Q.IsCharging;
        }

        #region Menus
        public override bool ComboMenu(Menu config)
        {
            config.AddItem(new MenuItem("UseQC" + Id, "Q").SetValue(new StringList(new[] { "Off", "ASAP", "Max range" }, 1)));
            config.AddItem(new MenuItem("UseWC" + Id, "W").SetValue(new Slider(3, 0, 3)));
            config.AddItem(new MenuItem("UseEC" + Id, "E").SetValue(true));

            return true;
        }

        public override bool HarassMenu(Menu config)
        {
            config.AddItem(new MenuItem("UseQH" + Id, "Q").SetValue(new StringList(new[] { "Off", "ASAP", "Max range"}, 2)));
            config.AddItem(new MenuItem("UseWH" + Id, "W").SetValue(new Slider(2, 0, 3)));
            config.AddItem(new MenuItem("UseEH" + Id, "E").SetValue(true));

            return true;
        }

        public override bool MiscMenu(Menu config)
        {
            config.AddItem(new MenuItem("spellDelay" + Id, "Spell delay").SetValue(new Slider(500, 0, 3000)));
            config.AddItem(new MenuItem("CastR" + Id, "Cast R").SetValue(new KeyBind("T".ToCharArray()[0], KeyBindType.Press)));

            return true;
        }

        public override bool DrawingMenu(Menu config)
        {
            config.AddItem(new MenuItem("DrawQ" + Id, "Q").SetValue(new Circle(true, System.Drawing.Color.White)));
            config.AddItem(new MenuItem("DrawE" + Id, "E").SetValue(new Circle(true, System.Drawing.Color.White)));
            config.AddItem(new MenuItem("DrawR" + Id, "R").SetValue(new Circle(true, System.Drawing.Color.White)));
            config.AddItem(new MenuItem("DrawRS" + Id, "R: Search Area").SetValue(new Circle(true, System.Drawing.Color.White)));

            return true;
        }
        #endregion

    }
}
