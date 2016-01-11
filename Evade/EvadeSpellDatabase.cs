// Copyright 2014 - 2014 Esk0r
// EvadeSpellDatabase.cs is part of Evade.
// 
// Evade is free software: you can redistribute it and/or modify
// it under the terms of the GNU General Public License as published by
// the Free Software Foundation, either version 3 of the License, or
// (at your option) any later version.
// 
// Evade is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE. See the
// GNU General Public License for more details.
// 
// You should have received a copy of the GNU General Public License
// along with Evade. If not, see <http://www.gnu.org/licenses/>.

#region

using System.Collections.Generic;
using System.Linq;
using LeagueSharp;
using LeagueSharp.Common;

#endregion

namespace Evade
{
    internal class EvadeSpellDatabase
    {
        public static List<EvadeSpellData> Spells = new List<EvadeSpellData>();

        static EvadeSpellDatabase()
        {
            //Add available evading spells to the database. SORTED BY PRIORITY.
            EvadeSpellData spell;

            #region Champion SpellShields

            #region Sivir

            if (ObjectManager.Player.ChampionName == "Sivir")
            {
                spell = new ShieldData("Sivir E", SpellSlot.E, 100, 1, true);
                Spells.Add(spell);
            }

            #endregion

            #region Nocturne

            if (ObjectManager.Player.ChampionName == "Nocturne")
            {
                spell = new ShieldData("Nocturne W", SpellSlot.W, 100, 1, true);
                Spells.Add(spell);
            }

            #endregion

            #endregion

            //Walking.
            spell = new EvadeSpellData("Walking", 1);
            Spells.Add(spell);

            #region Champion MoveSpeed buffs

            #region Blitzcrank

            if (ObjectManager.Player.ChampionName == "Blitzcrank")
            {
                spell = new MoveBuffData(
                    "Blitzcrank W", SpellSlot.W, 100, 3,
                    () =>
                        ObjectManager.Player.MoveSpeed *
                        (1 + 0.12f + 0.04f * ObjectManager.Player.Spellbook.GetSpell(SpellSlot.W).Level));
                Spells.Add(spell);
            }

            #endregion

            #region Draven

            if (ObjectManager.Player.ChampionName == "Draven")
            {
                spell = new MoveBuffData(
                    "Draven W", SpellSlot.W, 100, 3,
                    () =>
                        ObjectManager.Player.MoveSpeed *
                        (1 + 0.35f + 0.05f * ObjectManager.Player.Spellbook.GetSpell(SpellSlot.W).Level));
                Spells.Add(spell);
            }

            #endregion

            #region Evelynn

            if (ObjectManager.Player.ChampionName == "Evelynn")
            {
                spell = new MoveBuffData(
                    "Evelynn W", SpellSlot.W, 100, 3,
                    () =>
                        ObjectManager.Player.MoveSpeed *
                        (1 + 0.2f + 0.1f * ObjectManager.Player.Spellbook.GetSpell(SpellSlot.W).Level));
                Spells.Add(spell);
            }

            #endregion

            #region Garen

            if (ObjectManager.Player.ChampionName == "Garen")
            {
                spell = new MoveBuffData("Garen Q", SpellSlot.Q, 100, 3, () => ObjectManager.Player.MoveSpeed * (1.35f));
                Spells.Add(spell);
            }

            #endregion

            #region Katarina

            if (ObjectManager.Player.ChampionName == "Katarina")
            {
                spell = new MoveBuffData(
                    "Katarina W", SpellSlot.W, 100, 3,
                    () =>
                        ObjectManager.Get<Obj_AI_Hero>().Any(h => h.IsValidTarget(375))
                            ? ObjectManager.Player.MoveSpeed *
                              (1 + 0.10f + 0.05f * ObjectManager.Player.Spellbook.GetSpell(SpellSlot.W).Level)
                            : 0);
                Spells.Add(spell);
            }

            #endregion

            #region Karma 

            if (ObjectManager.Player.ChampionName == "Karma")
            {
                spell = new MoveBuffData(
                    "Karma E", SpellSlot.E, 100, 3,
                    () =>
                        ObjectManager.Player.MoveSpeed *
                        (1 + 0.35f + 0.05f * ObjectManager.Player.Spellbook.GetSpell(SpellSlot.E).Level));
                Spells.Add(spell);
            }

            #endregion

            #region Kennen

            if (ObjectManager.Player.ChampionName == "Kennen")
            {
                spell = new MoveBuffData("Kennen E", SpellSlot.E, 100, 3, () => 200 + ObjectManager.Player.MoveSpeed);
                //Actually it should be +335 but ingame you only gain +230, rito plz
                Spells.Add(spell);
            }

            #endregion

            #region Khazix

            if (ObjectManager.Player.ChampionName == "Khazix")
            {
                spell = new MoveBuffData("Khazix R", SpellSlot.R, 100, 5, () => ObjectManager.Player.MoveSpeed * 1.4f);
                Spells.Add(spell);
            }

            #endregion

            #region Lulu

            if (ObjectManager.Player.ChampionName == "Lulu")
            {
                spell = new MoveBuffData(
                    "Lulu W", SpellSlot.W, 100, 5,
                    () => ObjectManager.Player.MoveSpeed * (1.3f + ObjectManager.Player.FlatMagicDamageMod / 100 * 0.1f));
                Spells.Add(spell);
            }

            #endregion

            #region Nunu

            if (ObjectManager.Player.ChampionName == "Nunu")
            {
                spell = new MoveBuffData(
                    "Nunu W", SpellSlot.W, 100, 3,
                    () =>
                        ObjectManager.Player.MoveSpeed *
                        (1 + 0.1f + 0.01f * ObjectManager.Player.Spellbook.GetSpell(SpellSlot.W).Level));
                Spells.Add(spell);
            }

            #endregion

            #region Ryze

            if (ObjectManager.Player.ChampionName == "Ryze")
            {
                spell = new MoveBuffData("Ryze R", SpellSlot.R, 100, 5, () => 80 + ObjectManager.Player.MoveSpeed);
                Spells.Add(spell);
            }

            #endregion

            #region Shyvana

            if (ObjectManager.Player.ChampionName == "Sivir")
            {
                spell = new MoveBuffData("Sivir R", SpellSlot.R, 100, 5, () => ObjectManager.Player.MoveSpeed * (1.6f));
                Spells.Add(spell);
            }

            #endregion

            #region Shyvana

            if (ObjectManager.Player.ChampionName == "Shyvana")
            {
                spell = new MoveBuffData(
                    "Shyvana W", SpellSlot.W, 100, 4,
                    () =>
                        ObjectManager.Player.MoveSpeed *
                        (1 + 0.25f + 0.05f * ObjectManager.Player.Spellbook.GetSpell(SpellSlot.W).Level));
                spell.CheckSpellName = "ShyvanaImmolationAura";
                Spells.Add(spell);
            }

            #endregion

            #region Sona

            if (ObjectManager.Player.ChampionName == "Sona")
            {
                spell = new MoveBuffData(
                    "Sona E", SpellSlot.E, 100, 3,
                    () =>
                        ObjectManager.Player.MoveSpeed *
                        (1 + 0.12f + 0.01f * ObjectManager.Player.Spellbook.GetSpell(SpellSlot.E).Level +
                         ObjectManager.Player.FlatMagicDamageMod / 100 * 0.075f +
                         0.02f * ObjectManager.Player.Spellbook.GetSpell(SpellSlot.R).Level));
                Spells.Add(spell);
            }

            #endregion

            #region Teemo ^_^

            if (ObjectManager.Player.ChampionName == "Teemo")
            {
                spell = new MoveBuffData(
                    "Teemo W", SpellSlot.W, 100, 3,
                    () =>
                        ObjectManager.Player.MoveSpeed *
                        (1 + 0.06f + 0.04f * ObjectManager.Player.Spellbook.GetSpell(SpellSlot.W).Level));
                Spells.Add(spell);
            }

            #endregion

            #region Udyr

            if (ObjectManager.Player.ChampionName == "Udyr")
            {
                spell = new MoveBuffData(
                    "Udyr E", SpellSlot.E, 100, 3,
                    () =>
                        ObjectManager.Player.MoveSpeed *
                        (1 + 0.1f + 0.05f * ObjectManager.Player.Spellbook.GetSpell(SpellSlot.E).Level));
                Spells.Add(spell);
            }

            #endregion

            #region Zilean

            if (ObjectManager.Player.ChampionName == "Zilean")
            {
                spell = new MoveBuffData("Zilean E", SpellSlot.E, 100, 3, () => ObjectManager.Player.MoveSpeed * 1.55f);
                Spells.Add(spell);
            }

            #endregion

            #endregion

            #region Champion Dashes

            #region Aatrox

            if (ObjectManager.Player.ChampionName == "Aatrox")
            {
                spell = new DashData("Aatrox Q", SpellSlot.Q, 650, false, 400, 3000, 3);
                spell.Invert = true;
                Spells.Add(spell);
            }

            #endregion

            #region Akali

            if (ObjectManager.Player.ChampionName == "Akali")
            {
                spell = new DashData("Akali R", SpellSlot.R, 800, false, 100, 2461, 3);
                spell.ValidTargets = new[] { SpellValidTargets.EnemyChampions, SpellValidTargets.EnemyMinions };
                Spells.Add(spell);
            }

            #endregion

            #region Alistar

            if (ObjectManager.Player.ChampionName == "Alistar")
            {
                spell = new DashData("Alistar W", SpellSlot.W, 650, false, 100, 1900, 3);
                spell.ValidTargets = new[] { SpellValidTargets.EnemyChampions, SpellValidTargets.EnemyMinions };
                Spells.Add(spell);
            }

            #endregion

            #region Caitlyn

            if (ObjectManager.Player.ChampionName == "Caitlyn")
            {
                spell = new DashData("Caitlyn E", SpellSlot.E, 390, true, 250, 1000, 3);
                spell.Invert = true;
                Spells.Add(spell);
            }

            #endregion

            #region Corki

            if (ObjectManager.Player.ChampionName == "Corki")
            {
                spell = new DashData("Corki W", SpellSlot.W, 600, false, 250, 1044, 3);
                Spells.Add(spell);
            }

            #endregion

            #region Fizz

            if (ObjectManager.Player.ChampionName == "Fizz")
            {
                spell = new DashData("Fizz Q", SpellSlot.Q, 550, true, 100, 1400, 4);
                spell.ValidTargets = new[] { SpellValidTargets.EnemyMinions, SpellValidTargets.EnemyChampions };
                Spells.Add(spell);
            }

            #endregion

            #region Gragas

            if (ObjectManager.Player.ChampionName == "Gragas")
            {
                spell = new DashData("Gragas E", SpellSlot.E, 600, true, 250, 911, 3);
                Spells.Add(spell);
            }

            #endregion

            #region Gnar

            if (ObjectManager.Player.ChampionName == "Gnar")
            {
                spell = new DashData("Gnar E", SpellSlot.E, 50, false, 0, 900, 3);
                spell.CheckSpellName = "GnarE";
                Spells.Add(spell);
            }

            #endregion

            #region Graves

            if (ObjectManager.Player.ChampionName == "Graves")
            {
                spell = new DashData("Graves E", SpellSlot.E, 425, true, 100, 1223, 3);
                Spells.Add(spell);
            }

            #endregion

            #region Irelia

            if (ObjectManager.Player.ChampionName == "Irelia")
            {
                spell = new DashData("Irelia Q", SpellSlot.Q, 650, false, 100, 2200, 3);
                spell.ValidTargets = new[] { SpellValidTargets.EnemyChampions, SpellValidTargets.EnemyMinions };
                Spells.Add(spell);
            }

            #endregion

            #region Jax

            if (ObjectManager.Player.ChampionName == "Jax")
            {
                spell = new DashData("Jax Q", SpellSlot.Q, 700, false, 100, 1400, 3);
                spell.ValidTargets = new[]
                {
                    SpellValidTargets.EnemyWards, SpellValidTargets.AllyWards, SpellValidTargets.AllyMinions,
                    SpellValidTargets.AllyChampions, SpellValidTargets.EnemyChampions, SpellValidTargets.EnemyMinions
                };
                Spells.Add(spell);
            }

            #endregion

            #region Leblanc

            if (ObjectManager.Player.ChampionName == "Leblanc")
            {
                spell = new DashData("LeBlanc W1", SpellSlot.W, 600, false, 100, 1621, 3);
                spell.CheckSpellName = "LeblancSlide";
                Spells.Add(spell);
            }

            if (ObjectManager.Player.ChampionName == "Leblanc")
            {
                spell = new DashData("LeBlanc RW", SpellSlot.R, 600, false, 100, 1621, 3);
                spell.CheckSpellName = "LeblancSlideM";
                Spells.Add(spell);
            }

            #endregion

            #region LeeSin

            if (ObjectManager.Player.ChampionName == "LeeSin")
            {
                spell = new DashData("LeeSin W", SpellSlot.W, 700, false, 250, 2000, 3);
                spell.ValidTargets = new[]
                { SpellValidTargets.AllyChampions, SpellValidTargets.AllyMinions, SpellValidTargets.AllyWards };
                spell.CheckSpellName = "BlindMonkWOne";
                Spells.Add(spell);
            }

            #endregion

            #region Lucian

            if (ObjectManager.Player.ChampionName == "Lucian")
            {
                spell = new DashData("Lucian E", SpellSlot.E, 425, false, 100, 1350, 2);
                Spells.Add(spell);
            }

            #endregion

            #region Nidalee

            if (ObjectManager.Player.ChampionName == "Nidalee")
            {
                spell = new DashData("Nidalee W", SpellSlot.W, 375, true, 250, 943, 3);
                spell.CheckSpellName = "Pounce";
                Spells.Add(spell);
            }

            #endregion

            #region Pantheon

            if (ObjectManager.Player.ChampionName == "Pantheon")
            {
                spell = new DashData("Pantheon W", SpellSlot.W, 600, false, 100, 1000, 3);
                spell.ValidTargets = new[] { SpellValidTargets.EnemyChampions, SpellValidTargets.EnemyMinions };
                Spells.Add(spell);
            }

            #endregion

            #region Riven

            if (ObjectManager.Player.ChampionName == "Riven")
            {
                spell = new DashData("Riven Q", SpellSlot.Q, 222, true, 250, 560, 3);
                spell.RequiresPreMove = true;
                Spells.Add(spell);

                spell = new DashData("Riven E", SpellSlot.E, 250, false, 250, 1200, 3);
                Spells.Add(spell);
            }

            #endregion

            #region Tristana

            if (ObjectManager.Player.ChampionName == "Tristana")
            {
                spell = new DashData("Tristana W", SpellSlot.W, 900, true, 300, 800, 5);
                Spells.Add(spell);
            }

            #endregion

            #region Tryndamare

            if (ObjectManager.Player.ChampionName == "Tryndamere")
            {
                spell = new DashData("Tryndamere E", SpellSlot.E, 650, true, 250, 900, 3);
                Spells.Add(spell);
            }

            #endregion

            #region Vayne

            if (ObjectManager.Player.ChampionName == "Vayne")
            {
                spell = new DashData("Vayne Q", SpellSlot.Q, 300, true, 100, 910, 2);
                Spells.Add(spell);
            }

            #endregion

            #region Wukong

            if (ObjectManager.Player.ChampionName == "MonkeyKing")
            {
                spell = new DashData("Wukong E", SpellSlot.E, 650, false, 100, 1400, 3);
                spell.ValidTargets = new[] { SpellValidTargets.EnemyChampions, SpellValidTargets.EnemyMinions };
                Spells.Add(spell);
            }

            #endregion

            #endregion

            #region Champion Blinks

            #region Ezreal

            if (ObjectManager.Player.ChampionName == "Ezreal")
            {
                spell = new BlinkData("Ezreal E", SpellSlot.E, 450, 350, 3);
                Spells.Add(spell);
            }

            #endregion

            #region Kassadin

            if (ObjectManager.Player.ChampionName == "Kassadin")
            {
                spell = new BlinkData("Kassadin R", SpellSlot.R, 700, 200, 5);
                Spells.Add(spell);
            }

            #endregion

            #region Katarina

            if (ObjectManager.Player.ChampionName == "Katarina")
            {
                spell = new BlinkData("Katarina E", SpellSlot.E, 700, 200, 3);
                spell.ValidTargets = new[]
                {
                    SpellValidTargets.AllyChampions, SpellValidTargets.AllyMinions, SpellValidTargets.AllyWards,
                    SpellValidTargets.EnemyChampions, SpellValidTargets.EnemyMinions, SpellValidTargets.EnemyWards
                };
                Spells.Add(spell);
            }

            #endregion

            #region Shaco

            if (ObjectManager.Player.ChampionName == "Shaco")
            {
                spell = new BlinkData("Shaco Q", SpellSlot.Q, 400, 350, 3);
                Spells.Add(spell);
            }

            #endregion

            #region Talon

            if (ObjectManager.Player.ChampionName == "Talon")
            {
                spell = new BlinkData("Talon E", SpellSlot.E, 700, 100, 3);
                spell.ValidTargets = new[] { SpellValidTargets.EnemyChampions, SpellValidTargets.EnemyMinions };
                Spells.Add(spell);
            }

            #endregion

            #endregion

            #region Champion Invulnerabilities

            #region Elise

            if (ObjectManager.Player.ChampionName == "Elise")
            {
                spell = new InvulnerabilityData("Elise E", SpellSlot.E, 250, 3);
                spell.CheckSpellName = "EliseSpiderEInitial";
                spell.SelfCast = true;
                Spells.Add(spell);
            }

            #endregion

            #region Vladimir

            if (ObjectManager.Player.ChampionName == "Vladimir")
            {
                spell = new InvulnerabilityData("Vladimir W", SpellSlot.W, 250, 3);
                spell.SelfCast = true;
                Spells.Add(spell);
            }

            #endregion

            #region Fizz

            if (ObjectManager.Player.ChampionName == "Fizz")
            {
                spell = new InvulnerabilityData("Fizz E", SpellSlot.E, 250, 3);
                Spells.Add(spell);
            }

            #endregion

            #region MasterYi

            if (ObjectManager.Player.ChampionName == "MasterYi")
            {
                spell = new InvulnerabilityData("MasterYi Q", SpellSlot.Q, 250, 3);
                spell.MaxRange = 600;
                spell.ValidTargets = new[] { SpellValidTargets.EnemyChampions, SpellValidTargets.EnemyMinions };
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

            #region Tahm
            // add tahm eating here
            #endregion

            #region Annie

            #endregion

            #region Diana

            #endregion

            #region Galio

            #endregion

            #region Akali

            #endregion

            #region Akali

            #endregion

            #region Akali

            #endregion

            #region Akali

            #endregion

            #region Akali

            #endregion

            #region Akali

            #endregion

            #region Akali

            #endregion

            #region Akali

            #endregion

            #region Akali

            #endregion

            #region Karma

            if (ObjectManager.Player.ChampionName == "Karma")
            {
                spell = new ShieldData("Karma E", SpellSlot.E, 100, 2);
                spell.CanShieldAllies = true;
                spell.MaxRange = 800;
                Spells.Add(spell);
            }

            #endregion

            #region Janna

            if (ObjectManager.Player.ChampionName == "Janna")
            {
                spell = new ShieldData("Janna E", SpellSlot.E, 100, 1);
                spell.CanShieldAllies = true;
                spell.MaxRange = 800;
                Spells.Add(spell);
            }

            #endregion

            #region Morgana

            if (ObjectManager.Player.ChampionName == "Morgana")
            {
                spell = new ShieldData("Morgana E", SpellSlot.E, 100, 3);
                spell.CanShieldAllies = true;
                spell.MaxRange = 750;
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
                {
                    return evadeSpellData;
                }
            }

            return null;
        }
    }
}
