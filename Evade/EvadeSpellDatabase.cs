using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LeagueSharp;
using LeagueSharp.Common;

namespace Evade
{
    class EvadeSpellDatabase
    {
        public static List<EvadeSpellData> Spells = new List<EvadeSpellData>();

        static EvadeSpellDatabase()
        {
            //Add available evading spells to the database. SORTED BY PRIORITY.
            EvadeSpellData spell;

            #region Champion SpellShields
 
                #region Sivir
                if (ObjectManager.Player.BaseSkinName == "Sivir")
                {
                    spell = new ShieldData("Sivir E", SpellSlot.E, 100, 1, true);
                    Spells.Add(spell);
                }
                #endregion
                #region Nocturne
                if (ObjectManager.Player.BaseSkinName == "Nocturne")
                {
                    spell = new ShieldData("Nocturne E", SpellSlot.E, 100, 1, true);
                    Spells.Add(spell);
                }
                #endregion

            #endregion

            //Walking.
            spell = new EvadeSpellData("Walking", 1);
            Spells.Add(spell);

            #region Champion Dashes

                #region Caitlyn
                if (ObjectManager.Player.BaseSkinName == "Caitlyn")
                {
                    spell = new DashData("Caitlyn E", SpellSlot.E, 490, true, 250, 1000, 3);
                    spell.Invert = true;
                    Spells.Add(spell);
                }
                #endregion
                #region Gragas
                if (ObjectManager.Player.BaseSkinName == "Gragas")
                {
                    spell = new DashData("Gragas E", SpellSlot.E, 600, false, 250, 911, 3);
                    Spells.Add(spell);
                }
                #endregion
                #region Nidalee
                if (ObjectManager.Player.BaseSkinName == "Nidalee")
                {
                    spell = new DashData("Nidalee W", SpellSlot.W, 375, true, 250, 943, 3);
                    spell.CheckSpellName = "Pounce";
                    Spells.Add(spell);
                }
                #endregion
                #region Riven
                if (ObjectManager.Player.BaseSkinName == "Riven")
                {
                    spell = new DashData("Riven Q", SpellSlot.Q, 222, true, 250, 560, 3);
                    spell.RequiresPreMove = true;
                    Spells.Add(spell);

                    spell = new DashData("Riven E", SpellSlot.E, 250, false, 250, 1200, 3);
                    Spells.Add(spell);
                }
                #endregion
                #region Tristana
                if (ObjectManager.Player.BaseSkinName == "Tristana")
                {
                    spell = new DashData("Tristana W", SpellSlot.W, 900, true, 300, 800, 5);
                    Spells.Add(spell);
                }
                #endregion
                #region Tryndamare
                if (ObjectManager.Player.BaseSkinName == "Tryndamere")
                {
                    spell = new DashData("Tryndamere E", SpellSlot.E, 650, true, 250, 900, 3);
                    Spells.Add(spell);
                }
                #endregion
                #region Vayne
                if (ObjectManager.Player.BaseSkinName == "Vayne")
                { 
                    spell = new DashData("Vayne Q", SpellSlot.Q, 300, true, 250, 900, 2);
                    Spells.Add(spell);
                }
                #endregion

            #endregion

            #region Champion Blinks

                #region Ezreal
                if (ObjectManager.Player.BaseSkinName == "Ezreal")
                {
                    spell = new BlinkData("Ezreal E", SpellSlot.E, 450, 250, 3);
                    Spells.Add(spell);
                }
                #endregion
                #region Kassadin
                if (ObjectManager.Player.BaseSkinName == "Kassadin")
                {
                    spell = new BlinkData("Kassadin R", SpellSlot.R, 700, 200, 5);
                    Spells.Add(spell);
                }
                #endregion
                #region Katarina
                if (ObjectManager.Player.BaseSkinName == "Katarina")
                {
                    spell = new BlinkData("Katarina E", SpellSlot.E, 700, 100, 3);
                    spell.ValidTargets = new [] { SpellValidTargets.AllyChampions, SpellValidTargets.AllyMinions, SpellValidTargets.AllyWards, SpellValidTargets.EnemyChampions, SpellValidTargets.EnemyMinions, SpellValidTargets.EnemyWards, };
                    Spells.Add(spell);
                }
                #endregion
                #region Shaco
                if (ObjectManager.Player.BaseSkinName == "Shaco")
                {
                    spell = new BlinkData("Shaco Q", SpellSlot.Q, 400, 250, 3);
                    Spells.Add(spell);
                }
                #endregion

            #endregion

            #region Champion Invulnerabilities

                #region Elise
                if (ObjectManager.Player.BaseSkinName == "Elise")
                {
                    spell = new InvulnerabilityData("Elise E", SpellSlot.E, 250, 3);
                    spell.CheckSpellName = "EliseSpiderEInitial";
                    spell.SelfCast = true;
                    Spells.Add(spell);
                }
                #endregion
                #region Fizz
                if (ObjectManager.Player.BaseSkinName == "Fizz")
                {
                    spell = new InvulnerabilityData("Fizz E", SpellSlot.E, 250, 3);
                    Spells.Add(spell);
                }
                #endregion
                #region MasterYi
                if (ObjectManager.Player.BaseSkinName == "MasterYi")
                {
                    spell = new InvulnerabilityData("MasterYi Q", SpellSlot.Q, 250, 3);
                    spell.MaxRange = 600;
                    spell.ValidTargets = new[] {SpellValidTargets.EnemyChampions, SpellValidTargets.EnemyMinions};
                    Spells.Add(spell);
                }
                #endregion

            #endregion


            //Flash
            if (ObjectManager.Player.GetSpellSlot("SummonerFlash") != SpellSlot.Unknown)
            {
                spell = new BlinkData("Flash", ObjectManager.Player.GetSpellSlot("SummonerFlash"), 400, 100, 5, true);
                Spells.Add(spell);
            }

            //Zhonyas
            spell = new EvadeSpellData("Zhonyas", 5);
            Spells.Add(spell);

            #region Champion Shields
                #region Janna
                if (ObjectManager.Player.BaseSkinName == "Janna")
                {
                    spell = new ShieldData("Janna E", SpellSlot.E, 100, 1);
                    spell.CanShieldAllies = true;
                    spell.MaxRange = 800;
                    Spells.Add(spell);
                }
                #endregion
                #region Lulu
                if (ObjectManager.Player.BaseSkinName == "Lulu")
                {
                    spell = new ShieldData("Lulu E", SpellSlot.E, 100, 1);
                    spell.CanShieldAllies = true;
                    spell.MaxRange = ObjectManager.Player.Spellbook.GetSpell(SpellSlot.E).SData.CastRange[0];
                    Spells.Add(spell);
                }
                #endregion
                #region Karma
                if (ObjectManager.Player.BaseSkinName == "Karma")
                {
                    spell = new ShieldData("Karma E", SpellSlot.E, 100, 1);
                    spell.CanShieldAllies = true;
                    spell.MaxRange = ObjectManager.Player.Spellbook.GetSpell(SpellSlot.E).SData.CastRange[0];
                    Spells.Add(spell);
                }
                #endregion
                #region LeeSin
                if (ObjectManager.Player.BaseSkinName == "LeeSin")
                {
                    spell = new ShieldData("LeeSin W", SpellSlot.W, 100, 1);
                    spell.CanShieldAllies = true;
                    spell.MaxRange = ObjectManager.Player.Spellbook.GetSpell(SpellSlot.W).SData.CastRange[0];
                    Spells.Add(spell);
                }
                #endregion
                #region Orianna
                if (ObjectManager.Player.BaseSkinName == "Orianna")
                {
                    spell = new ShieldData("Orianna E", SpellSlot.E, 100, 1);
                    spell.CanShieldAllies = true;
                    spell.MaxRange = ObjectManager.Player.Spellbook.GetSpell(SpellSlot.E).SData.CastRange[0];
                    Spells.Add(spell);
                }
                #endregion
                #region Karma
                if (ObjectManager.Player.BaseSkinName == "Karma")
                {
                    spell = new ShieldData("Karma E", SpellSlot.E, 100, 1);
                    spell.CanShieldAllies = true;
                    spell.MaxRange = ObjectManager.Player.Spellbook.GetSpell(SpellSlot.E).SData.CastRange[0];
                    Spells.Add(spell);
                }
                #endregion
                #region Shen
                if (ObjectManager.Player.BaseSkinName == "Shen")
                {
                    spell = new ShieldData("Shen W", SpellSlot.W, 100, 1);
                    spell.CanShieldAllies = false;
                    spell.MaxRange = ObjectManager.Player.Spellbook.GetSpell(SpellSlot.W).SData.CastRange[0];
                    Spells.Add(spell);
                }
                {
                    spell = new ShieldData("Shen R", SpellSlot.R, 100, 1);
                    spell.CanShieldAllies = true;
                    spell.MaxRange = ObjectManager.Player.Spellbook.GetSpell(SpellSlot.R).SData.CastRange[0];
                    Spells.Add(spell);
                }
                #endregion
                #region Rumble
                if (ObjectManager.Player.BaseSkinName == "Rumble")
                {
                    spell = new ShieldData("Rumble W", SpellSlot.W, 100, 1);
                    spell.CanShieldAllies = false;
                    spell.MaxRange = ObjectManager.Player.Spellbook.GetSpell(SpellSlot.W).SData.CastRange[0];
                    Spells.Add(spell);
                }
                #endregion
                #region Nautilus
                if (ObjectManager.Player.BaseSkinName == "Nautilus")
                {
                    spell = new ShieldData("Nautilus W", SpellSlot.W, 100, 1);
                    spell.CanShieldAllies = false;
                    spell.MaxRange = ObjectManager.Player.Spellbook.GetSpell(SpellSlot.W).SData.CastRange[0];
                    Spells.Add(spell);
                }
                #endregion
                #region Sion
                if (ObjectManager.Player.BaseSkinName == "Sion")
                {
                    spell = new ShieldData("Sion W", SpellSlot.W, 100, 1);
                    spell.CanShieldAllies = false;
                    spell.MaxRange = ObjectManager.Player.Spellbook.GetSpell(SpellSlot.W).SData.CastRange[0];
                    Spells.Add(spell);
                }
                #endregion
                #region JarvanIV
                if (ObjectManager.Player.BaseSkinName == "JarvanIV")
                {
                    spell = new ShieldData("JarvanIV W", SpellSlot.W, 100, 1);
                    spell.CanShieldAllies = false;
                    spell.MaxRange = ObjectManager.Player.Spellbook.GetSpell(SpellSlot.W).SData.CastRange[0];
                    Spells.Add(spell);
                }
                #endregion
                #region Skarner
                if (ObjectManager.Player.BaseSkinName == "Skarner")
                {
                    spell = new ShieldData("Skarner W", SpellSlot.W, 100, 1);
                    spell.CanShieldAllies = false;
                    spell.MaxRange = ObjectManager.Player.Spellbook.GetSpell(SpellSlot.W).SData.CastRange[0];
                    Spells.Add(spell);
                }
                #endregion
                #region Urgot
                if (ObjectManager.Player.BaseSkinName == "Urgot")
                {
                    spell = new ShieldData("Urgot W", SpellSlot.W, 100, 1);
                    spell.CanShieldAllies = false;
                    spell.MaxRange = ObjectManager.Player.Spellbook.GetSpell(SpellSlot.W).SData.CastRange[0];
                    Spells.Add(spell);
                }
                #endregion
                #region Diana
                if (ObjectManager.Player.BaseSkinName == "Diana")
                {
                    spell = new ShieldData("Diana W", SpellSlot.W, 100, 1);
                    spell.CanShieldAllies = false;
                    spell.MaxRange = ObjectManager.Player.Spellbook.GetSpell(SpellSlot.W).SData.CastRange[0];
                    Spells.Add(spell);
                }
                #endregion
                #region Udyr
                if (ObjectManager.Player.BaseSkinName == "Udyr")
                {
                    spell = new ShieldData("Udyr W", SpellSlot.W, 100, 1);
                    spell.CanShieldAllies = false;
                    spell.MaxRange = ObjectManager.Player.Spellbook.GetSpell(SpellSlot.W).SData.CastRange[0];
                    Spells.Add(spell);
                }
                #endregion
                #region Kayle
                if (ObjectManager.Player.BaseSkinName == "Kayle")
                {
                    spell = new ShieldData("Kayle R", SpellSlot.R, 100, 1);
                    spell.CanShieldAllies = true;
                    spell.MaxRange = ObjectManager.Player.Spellbook.GetSpell(SpellSlot.R).SData.CastRange[0];
                    Spells.Add(spell);
                }
                #endregion
                #region Morgana
                if (ObjectManager.Player.BaseSkinName == "Morgana")
                {
                    spell = new ShieldData("Morgana E", SpellSlot.E, 100, 3);
                    spell.CanShieldAllies = true;
                    spell.MaxRange = 750;
                    Spells.Add(spell);
                }
                #endregion
                
                //  Wasn't sure how to handle these two special cases, but I think they will work as I have them set up.
                #region Zilean
                if (ObjectManager.Player.BaseSkinName == "Zilean")
                {
                    spell = new ShieldData("Zilean R", SpellSlot.R, 100, 5);
                    spell.CanShieldAllies = true;
                    spell.MaxRange = ObjectManager.Player.Spellbook.GetSpell(SpellSlot.R).SData.CastRange[0];
                    Spells.Add(spell);
                }
                #endregion
                #region Tryndamere
                if (ObjectManager.Player.BaseSkinName == "Tryndamere")
                {
                    spell = new ShieldData("Tryndamere W", SpellSlot.R, 100, 5);
                    spell.CanShieldAllies = false;
                    spell.MaxRange = ObjectManager.Player.Spellbook.GetSpell(SpellSlot.R).SData.CastRange[0];
                    Spells.Add(spell);
                }
                #endregion


            #endregion
        }

        public static EvadeSpellData GetByName(string Name)
        {
            Name = Name.ToLower();
            foreach (var evadeSpellData in Spells)
            {
                if (evadeSpellData.Name.ToLower() == Name)
                    return evadeSpellData;
            }

            return null;
        }
    }
}
