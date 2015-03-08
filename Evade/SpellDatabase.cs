// Copyright 2014 - 2014 Esk0r
// SpellDatabase.cs is part of Evade.
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

using System;
using System.Collections.Generic;
using System.Linq;
using LeagueSharp;

#endregion

namespace Evade
{
    public static class SpellDatabase
    {
        public static List<SpellData> Spells = new List<SpellData>();

        static SpellDatabase()
        {
            //Add spells to the database 

            #region Test

            if (Config.TestOnAllies)
            {
                Spells.Add(
                    new SpellData
                    {
                        ChampionName = ObjectManager.Player.ChampionName,
                        SpellName = "TestSkillShot",
                        Slot = SpellSlot.R,
                        Type = SkillShotType.SkillshotCircle,
                        Delay = 600,
                        Range = 650,
                        Radius = 350,
                        MissileSpeed = int.MaxValue,
                        FixedRange = false,
                        AddHitbox = true,
                        DangerValue = 5,
                        IsDangerous = true,
                        MissileSpellName = "TestSkillShot",
                    });
            }

            #endregion Test

            #region Aatrox

            Spells.Add(
                new SpellData
                {
                    ChampionName = "Aatrox",
                    SpellName = "AatroxQ",
                    Slot = SpellSlot.Q,
                    Type = SkillShotType.SkillshotCircle,
                    Delay = 600,
                    Range = 650,
                    Radius = 250,
                    MissileSpeed = 2000,
                    FixedRange = false,
                    AddHitbox = true,
                    DangerValue = 3,
                    IsDangerous = true,
                    MissileSpellName = "",
                });

            Spells.Add(
                new SpellData
                {
                    ChampionName = "Aatrox",
                    SpellName = "AatroxE",
                    Slot = SpellSlot.E,
                    Type = SkillShotType.SkillshotMissileLine,
                    Delay = 250,
                    Range = 1075,
                    Radius = 35,
                    MissileSpeed = 1250,
                    FixedRange = true,
                    AddHitbox = true,
                    DangerValue = 3,
                    IsDangerous = false,
                    MissileSpellName = "AatroxEConeMissile",
                });

            #endregion Aatrox

            #region Ahri

            Spells.Add(
                new SpellData
                {
                    ChampionName = "Ahri",
                    SpellName = "AhriOrbofDeception",
                    Slot = SpellSlot.Q,
                    Type = SkillShotType.SkillshotMissileLine,
                    Delay = 250,
                    Range = 1000,
                    Radius = 100,
                    MissileSpeed = 2500,
                    MissileAccel = -3200,
                    MissileMaxSpeed = 2500,
                    MissileMinSpeed = 400,
                    FixedRange = true,
                    AddHitbox = true,
                    DangerValue = 2,
                    IsDangerous = false,
                    MissileSpellName = "AhriOrbMissile",
                    CanBeRemoved = true,
                    ForceRemove = true,
                    CollisionObjects = new[] { CollisionObjectTypes.YasuoWall }
                });

            Spells.Add(
                new SpellData
                {
                    ChampionName = "Ahri",
                    SpellName = "AhriOrbReturn",
                    Slot = SpellSlot.Q,
                    Type = SkillShotType.SkillshotMissileLine,
                    Delay = 250,
                    Range = 1000,
                    Radius = 100,
                    MissileSpeed = 60,
                    MissileAccel = 1900,
                    MissileMinSpeed = 60,
                    MissileMaxSpeed = 2600,
                    FixedRange = true,
                    AddHitbox = true,
                    DangerValue = 2,
                    IsDangerous = false,
                    MissileFollowsUnit = true,
                    CanBeRemoved = true,
                    ForceRemove = true,
                    MissileSpellName = "AhriOrbReturn",
                    CollisionObjects = new[] { CollisionObjectTypes.YasuoWall }
                });

            Spells.Add(
                new SpellData
                {
                    ChampionName = "Ahri",
                    SpellName = "AhriSeduce",
                    Slot = SpellSlot.E,
                    Type = SkillShotType.SkillshotMissileLine,
                    Delay = 250,
                    Range = 1000,
                    Radius = 60,
                    MissileSpeed = 1550,
                    FixedRange = true,
                    AddHitbox = true,
                    DangerValue = 3,
                    IsDangerous = true,
                    MissileSpellName = "AhriSeduceMissile",
                    CanBeRemoved = true,
                    CollisionObjects =
                        new[]
                        { CollisionObjectTypes.Champions, CollisionObjectTypes.Minion, CollisionObjectTypes.YasuoWall }
                });

            #endregion Ahri

            #region Amumu

            Spells.Add(
                new SpellData
                {
                    ChampionName = "Amumu",
                    SpellName = "BandageToss",
                    Slot = SpellSlot.Q,
                    Type = SkillShotType.SkillshotMissileLine,
                    Delay = 250,
                    Range = 1100,
                    Radius = 90,
                    MissileSpeed = 2000,
                    FixedRange = true,
                    AddHitbox = true,
                    DangerValue = 3,
                    IsDangerous = true,
                    MissileSpellName = "SadMummyBandageToss",
                    CanBeRemoved = true,
                    CollisionObjects =
                        new[]
                        { CollisionObjectTypes.Champions, CollisionObjectTypes.Minion, CollisionObjectTypes.YasuoWall }
                });

            Spells.Add(
                new SpellData
                {
                    ChampionName = "Amumu",
                    SpellName = "CurseoftheSadMummy",
                    Slot = SpellSlot.R,
                    Type = SkillShotType.SkillshotCircle,
                    Delay = 250,
                    Range = 0,
                    Radius = 550,
                    MissileSpeed = int.MaxValue,
                    FixedRange = true,
                    AddHitbox = false,
                    DangerValue = 5,
                    IsDangerous = true,
                    MissileSpellName = "",
                });

            #endregion Amumu

            #region Anivia

            Spells.Add(
                new SpellData
                {
                    ChampionName = "Anivia",
                    SpellName = "FlashFrost",
                    Slot = SpellSlot.Q,
                    Type = SkillShotType.SkillshotMissileLine,
                    Delay = 250,
                    Range = 1100,
                    Radius = 110,
                    MissileSpeed = 850,
                    FixedRange = false,
                    AddHitbox = true,
                    DangerValue = 3,
                    IsDangerous = true,
                    MissileSpellName = "FlashFrostSpell",
                    CanBeRemoved = true,
                    CollisionObjects = new[] { CollisionObjectTypes.YasuoWall }
                });

            #endregion Anivia

            #region Annie

            Spells.Add(
                new SpellData
                {
                    ChampionName = "Annie",
                    SpellName = "Incinerate",
                    Slot = SpellSlot.W,
                    Type = SkillShotType.SkillshotCone,
                    Delay = 250,
                    Range = 825,
                    Radius = 80,
                    MissileSpeed = int.MaxValue,
                    FixedRange = false,
                    AddHitbox = false,
                    DangerValue = 2,
                    IsDangerous = false,
                    MissileSpellName = "",
                });

            Spells.Add(
                new SpellData
                {
                    ChampionName = "Annie",
                    SpellName = "InfernalGuardian",
                    Slot = SpellSlot.R,
                    Type = SkillShotType.SkillshotCircle,
                    Delay = 250,
                    Range = 600,
                    Radius = 251,
                    MissileSpeed = int.MaxValue,
                    FixedRange = false,
                    AddHitbox = true,
                    DangerValue = 5,
                    IsDangerous = true,
                    MissileSpellName = "",
                });

            #endregion Annie

            #region Ashe

            Spells.Add(
                new SpellData
                {
                    ChampionName = "Ashe",
                    SpellName = "VolleyAttack",
                    Slot = SpellSlot.W,
                    Type = SkillShotType.SkillshotMissileLine,
                    Delay = 250,
                    Range = 1200,
                    Radius = 60,
                    MissileSpeed = 1500,
                    FixedRange = true,
                    AddHitbox = true,
                    DangerValue = 2,
                    IsDangerous = false,
                    MissileSpellName = "VolleyAttack",
                    MultipleNumber = 7,
                    MultipleAngle = 9.58f * (float) Math.PI / 180,
                    CanBeRemoved = false,
                });

            Spells.Add(
                new SpellData
                {
                    ChampionName = "Ashe",
                    SpellName = "EnchantedCrystalArrow",
                    Slot = SpellSlot.R,
                    Type = SkillShotType.SkillshotMissileLine,
                    Delay = 250,
                    Range = 20000,
                    Radius = 130,
                    MissileSpeed = 1600,
                    FixedRange = true,
                    AddHitbox = true,
                    DangerValue = 5,
                    IsDangerous = true,
                    MissileSpellName = "EnchantedCrystalArrow",
                    CanBeRemoved = true,
                    CollisionObjects = new[] { CollisionObjectTypes.Champions, CollisionObjectTypes.YasuoWall }
                });

            #endregion Ashe

            #region Blitzcrank

            Spells.Add(
                new SpellData
                {
                    ChampionName = "Blitzcrank",
                    SpellName = "RocketGrab",
                    Slot = SpellSlot.Q,
                    Type = SkillShotType.SkillshotMissileLine,
                    Delay = 250,
                    Range = 1050,
                    Radius = 70,
                    MissileSpeed = 1800,
                    FixedRange = true,
                    AddHitbox = true,
                    DangerValue = 4,
                    IsDangerous = true,
                    MissileSpellName = "RocketGrabMissile",
                    CanBeRemoved = true,
                    CollisionObjects =
                        new[]
                        { CollisionObjectTypes.Champions, CollisionObjectTypes.Minion, CollisionObjectTypes.YasuoWall }
                });

            Spells.Add(
                new SpellData
                {
                    ChampionName = "Blitzcrank",
                    SpellName = "StaticField",
                    Slot = SpellSlot.R,
                    Type = SkillShotType.SkillshotCircle,
                    Delay = 250,
                    Range = 0,
                    Radius = 600,
                    MissileSpeed = int.MaxValue,
                    FixedRange = true,
                    AddHitbox = false,
                    DangerValue = 2,
                    IsDangerous = false,
                    MissileSpellName = "",
                });

            #endregion Blitzcrank

            #region Brand

            Spells.Add(
                new SpellData
                {
                    ChampionName = "Brand",
                    SpellName = "BrandBlaze",
                    Slot = SpellSlot.Q,
                    Type = SkillShotType.SkillshotMissileLine,
                    Delay = 250,
                    Range = 1100,
                    Radius = 60,
                    MissileSpeed = 1600,
                    FixedRange = true,
                    AddHitbox = true,
                    DangerValue = 3,
                    IsDangerous = true,
                    MissileSpellName = "BrandBlazeMissile",
                    CanBeRemoved = true,
                    CollisionObjects =
                        new[]
                        { CollisionObjectTypes.Champions, CollisionObjectTypes.Minion, CollisionObjectTypes.YasuoWall }
                });

            Spells.Add(
                new SpellData
                {
                    ChampionName = "Brand",
                    SpellName = "BrandFissure",
                    Slot = SpellSlot.W,
                    Type = SkillShotType.SkillshotCircle,
                    Delay = 850,
                    Range = 900,
                    Radius = 240,
                    MissileSpeed = int.MaxValue,
                    FixedRange = false,
                    AddHitbox = true,
                    DangerValue = 2,
                    IsDangerous = false,
                    MissileSpellName = "",
                });

            #endregion Brand

            #region Braum

            Spells.Add(
                new SpellData
                {
                    ChampionName = "Braum",
                    SpellName = "BraumQ",
                    Slot = SpellSlot.Q,
                    Type = SkillShotType.SkillshotMissileLine,
                    Delay = 250,
                    Range = 1050,
                    Radius = 60,
                    MissileSpeed = 1700,
                    FixedRange = true,
                    AddHitbox = true,
                    DangerValue = 3,
                    IsDangerous = true,
                    MissileSpellName = "BraumQMissile",
                    CanBeRemoved = true,
                    CollisionObjects =
                        new[]
                        { CollisionObjectTypes.Champions, CollisionObjectTypes.Minion, CollisionObjectTypes.YasuoWall }
                });

            Spells.Add(
                new SpellData
                {
                    ChampionName = "Braum",
                    SpellName = "BraumRWrapper",
                    Slot = SpellSlot.R,
                    Type = SkillShotType.SkillshotMissileLine,
                    Delay = 500,
                    Range = 1200,
                    Radius = 115,
                    MissileSpeed = 1400,
                    FixedRange = true,
                    AddHitbox = true,
                    DangerValue = 4,
                    IsDangerous = true,
                    MissileSpellName = "braumrmissile",
                    CollisionObjects = new[] { CollisionObjectTypes.YasuoWall }
                });

            #endregion Braum

            #region Caitlyn

            Spells.Add(
                new SpellData
                {
                    ChampionName = "Caitlyn",
                    SpellName = "CaitlynPiltoverPeacemaker",
                    Slot = SpellSlot.Q,
                    Type = SkillShotType.SkillshotMissileLine,
                    Delay = 625,
                    Range = 1300,
                    Radius = 90,
                    MissileSpeed = 2200,
                    FixedRange = true,
                    AddHitbox = true,
                    DangerValue = 2,
                    IsDangerous = false,
                    MissileSpellName = "CaitlynPiltoverPeacemaker",
                    CollisionObjects = new[] { CollisionObjectTypes.YasuoWall }
                });

            Spells.Add(
                new SpellData
                {
                    ChampionName = "Caitlyn",
                    SpellName = "CaitlynEntrapment",
                    Slot = SpellSlot.E,
                    Type = SkillShotType.SkillshotMissileLine,
                    Delay = 125,
                    Range = 1000,
                    Radius = 80,
                    MissileSpeed = 2000,
                    FixedRange = true,
                    AddHitbox = true,
                    DangerValue = 1,
                    IsDangerous = false,
                    MissileSpellName = "CaitlynEntrapmentMissile",
                    CanBeRemoved = true,
                    CollisionObjects =
                        new[]
                        { CollisionObjectTypes.Champions, CollisionObjectTypes.Minion, CollisionObjectTypes.YasuoWall }
                });

            #endregion Caitlyn

            #region Cassiopeia

            Spells.Add(
                new SpellData
                {
                    ChampionName = "Cassiopeia",
                    SpellName = "CassiopeiaNoxiousBlast",
                    Slot = SpellSlot.Q,
                    Type = SkillShotType.SkillshotCircle,
                    Delay = 600,
                    Range = 850,
                    Radius = 150,
                    MissileSpeed = int.MaxValue,
                    FixedRange = false,
                    AddHitbox = true,
                    DangerValue = 2,
                    IsDangerous = false,
                    MissileSpellName = "CassiopeiaNoxiousBlast",
                });

            Spells.Add(
                new SpellData
                {
                    ChampionName = "Cassiopeia",
                    SpellName = "CassiopeiaPetrifyingGaze",
                    Slot = SpellSlot.R,
                    Type = SkillShotType.SkillshotCone,
                    Delay = 600,
                    Range = 825,
                    Radius = 80,
                    MissileSpeed = int.MaxValue,
                    FixedRange = false,
                    AddHitbox = false,
                    DangerValue = 5,
                    IsDangerous = true,
                    MissileSpellName = "CassiopeiaPetrifyingGaze",
                });

            #endregion Cassiopeia

            #region Chogath

            Spells.Add(
                new SpellData
                {
                    ChampionName = "Chogath",
                    SpellName = "Rupture",
                    Slot = SpellSlot.Q,
                    Type = SkillShotType.SkillshotCircle,
                    Delay = 1200,
                    Range = 950,
                    Radius = 250,
                    MissileSpeed = int.MaxValue,
                    FixedRange = false,
                    AddHitbox = true,
                    DangerValue = 3,
                    IsDangerous = false,
                    MissileSpellName = "Rupture",
                });

            #endregion Chogath

            #region Corki

            Spells.Add(
                new SpellData
                {
                    ChampionName = "Corki",
                    SpellName = "PhosphorusBomb",
                    Slot = SpellSlot.Q,
                    Type = SkillShotType.SkillshotCircle,
                    Delay = 300,
                    Range = 825,
                    Radius = 250,
                    MissileSpeed = 1000,
                    FixedRange = false,
                    AddHitbox = true,
                    DangerValue = 2,
                    IsDangerous = false,
                    MissileSpellName = "PhosphorusBombMissile",
                    CollisionObjects = new[] { CollisionObjectTypes.YasuoWall }
                });

            Spells.Add(
                new SpellData
                {
                    ChampionName = "Corki",
                    SpellName = "MissileBarrage",
                    Slot = SpellSlot.R,
                    Type = SkillShotType.SkillshotMissileLine,
                    Delay = 200,
                    Range = 1300,
                    Radius = 40,
                    MissileSpeed = 2000,
                    FixedRange = true,
                    AddHitbox = true,
                    DangerValue = 2,
                    IsDangerous = false,
                    MissileSpellName = "MissileBarrageMissile",
                    CanBeRemoved = true,
                    CollisionObjects =
                        new[]
                        { CollisionObjectTypes.Champions, CollisionObjectTypes.Minion, CollisionObjectTypes.YasuoWall }
                });

            Spells.Add(
                new SpellData
                {
                    ChampionName = "Corki",
                    SpellName = "MissileBarrage2",
                    Slot = SpellSlot.R,
                    Type = SkillShotType.SkillshotMissileLine,
                    Delay = 200,
                    Range = 1500,
                    Radius = 40,
                    MissileSpeed = 2000,
                    FixedRange = true,
                    AddHitbox = true,
                    DangerValue = 2,
                    IsDangerous = false,
                    MissileSpellName = "MissileBarrageMissile2",
                    CanBeRemoved = true,
                    CollisionObjects =
                        new[]
                        { CollisionObjectTypes.Champions, CollisionObjectTypes.Minion, CollisionObjectTypes.YasuoWall }
                });

            #endregion Corki

            #region Darius

            Spells.Add(
                new SpellData
                {
                    ChampionName = "Darius",
                    SpellName = "DariusAxeGrabCone",
                    Slot = SpellSlot.E,
                    Type = SkillShotType.SkillshotCone,
                    Delay = 300,
                    Range = 550,
                    Radius = 80,
                    MissileSpeed = int.MaxValue,
                    FixedRange = true,
                    AddHitbox = false,
                    DangerValue = 3,
                    IsDangerous = true,
                    MissileSpellName = "DariusAxeGrabCone",
                });

            #endregion Darius

            #region DrMundo

            Spells.Add(
                new SpellData
                {
                    ChampionName = "DrMundo",
                    SpellName = "InfectedCleaverMissileCast",
                    Slot = SpellSlot.Q,
                    Type = SkillShotType.SkillshotMissileLine,
                    Delay = 250,
                    Range = 1050,
                    Radius = 60,
                    MissileSpeed = 2000,
                    FixedRange = true,
                    AddHitbox = true,
                    DangerValue = 3,
                    IsDangerous = false,
                    MissileSpellName = "InfectedCleaverMissile",
                    CanBeRemoved = true,
                    CollisionObjects =
                        new[]
                        { CollisionObjectTypes.Champions, CollisionObjectTypes.Minion, CollisionObjectTypes.YasuoWall }
                });

            #endregion DrMundo

            #region Draven

            Spells.Add(
                new SpellData
                {
                    ChampionName = "Draven",
                    SpellName = "DravenDoubleShot",
                    Slot = SpellSlot.E,
                    Type = SkillShotType.SkillshotMissileLine,
                    Delay = 250,
                    Range = 1100,
                    Radius = 130,
                    MissileSpeed = 1400,
                    FixedRange = true,
                    AddHitbox = true,
                    DangerValue = 3,
                    IsDangerous = true,
                    MissileSpellName = "DravenDoubleShotMissile",
                    CanBeRemoved = true,
                    CollisionObjects = new[] { CollisionObjectTypes.YasuoWall }
                });

            Spells.Add(
                new SpellData
                {
                    ChampionName = "Draven",
                    SpellName = "DravenRCast",
                    Slot = SpellSlot.R,
                    Type = SkillShotType.SkillshotMissileLine,
                    Delay = 400,
                    Range = 20000,
                    Radius = 160,
                    MissileSpeed = 2000,
                    FixedRange = true,
                    AddHitbox = true,
                    DangerValue = 5,
                    IsDangerous = true,
                    MissileSpellName = "DravenR",
                    CollisionObjects = new[] { CollisionObjectTypes.YasuoWall }
                });

            #endregion Draven

            #region Elise

            Spells.Add(
                new SpellData
                {
                    ChampionName = "Elise",
                    SpellName = "EliseHumanE",
                    Slot = SpellSlot.E,
                    Type = SkillShotType.SkillshotMissileLine,
                    Delay = 250,
                    Range = 1100,
                    Radius = 55,
                    MissileSpeed = 1450,
                    FixedRange = true,
                    AddHitbox = true,
                    DangerValue = 4,
                    IsDangerous = true,
                    MissileSpellName = "EliseHumanE",
                    CanBeRemoved = true,
                    CollisionObjects =
                        new[]
                        { CollisionObjectTypes.Champions, CollisionObjectTypes.Minion, CollisionObjectTypes.YasuoWall }
                });

            #endregion Elise

            #region Evelynn

            Spells.Add(
                new SpellData
                {
                    ChampionName = "Evelynn",
                    SpellName = "EvelynnR",
                    Slot = SpellSlot.R,
                    Type = SkillShotType.SkillshotCircle,
                    Delay = 250,
                    Range = 650,
                    Radius = 350,
                    MissileSpeed = int.MaxValue,
                    FixedRange = false,
                    AddHitbox = true,
                    DangerValue = 5,
                    IsDangerous = true,
                    MissileSpellName = "EvelynnR",
                });

            #endregion Evelynn

            #region Ezreal

            Spells.Add(
                new SpellData
                {
                    ChampionName = "Ezreal",
                    SpellName = "EzrealMysticShot",
                    Slot = SpellSlot.Q,
                    Type = SkillShotType.SkillshotMissileLine,
                    Delay = 250,
                    Range = 1200,
                    Radius = 60,
                    MissileSpeed = 2000,
                    FixedRange = true,
                    AddHitbox = true,
                    DangerValue = 2,
                    IsDangerous = false,
                    MissileSpellName = "EzrealMysticShotMissile",
                    ExtraMissileNames = new[] { "EzrealMysticShotPulseMissile" },
                    CanBeRemoved = true,
                    CollisionObjects =
                        new[]
                        { CollisionObjectTypes.Champions, CollisionObjectTypes.Minion, CollisionObjectTypes.YasuoWall },
                    Id = 229,
                });

            Spells.Add(
                new SpellData
                {
                    ChampionName = "Ezreal",
                    SpellName = "EzrealEssenceFlux",
                    Slot = SpellSlot.W,
                    Type = SkillShotType.SkillshotMissileLine,
                    Delay = 250,
                    Range = 1050,
                    Radius = 80,
                    MissileSpeed = 1600,
                    FixedRange = true,
                    AddHitbox = true,
                    DangerValue = 2,
                    IsDangerous = false,
                    MissileSpellName = "EzrealEssenceFluxMissile",
                    CollisionObjects = new[] { CollisionObjectTypes.YasuoWall },
                });

            Spells.Add(
                new SpellData
                {
                    ChampionName = "Ezreal",
                    SpellName = "EzrealTrueshotBarrage",
                    Slot = SpellSlot.R,
                    Type = SkillShotType.SkillshotMissileLine,
                    Delay = 1000,
                    Range = 20000,
                    Radius = 160,
                    MissileSpeed = 2000,
                    FixedRange = true,
                    AddHitbox = true,
                    DangerValue = 3,
                    IsDangerous = true,
                    MissileSpellName = "EzrealTrueshotBarrage",
                    CollisionObjects = new[] { CollisionObjectTypes.YasuoWall },
                    Id = 245,
                });

            #endregion Ezreal

            #region Fizz

            Spells.Add(
                new SpellData
                {
                    ChampionName = "Fizz",
                    SpellName = "FizzMarinerDoom",
                    Slot = SpellSlot.R,
                    Type = SkillShotType.SkillshotMissileLine,
                    Delay = 250,
                    Range = 1300,
                    Radius = 120,
                    MissileSpeed = 1350,
                    FixedRange = false,
                    AddHitbox = true,
                    DangerValue = 5,
                    IsDangerous = true,
                    MissileSpellName = "FizzMarinerDoomMissile",
                    CollisionObjects = new[] { CollisionObjectTypes.Champions, CollisionObjectTypes.YasuoWall },
                    CanBeRemoved = true,
                });

            #endregion Fizz

            #region Galio

            Spells.Add(
                new SpellData
                {
                    ChampionName = "Galio",
                    SpellName = "GalioResoluteSmite",
                    Slot = SpellSlot.Q,
                    Type = SkillShotType.SkillshotCircle,
                    Delay = 250,
                    Range = 900,
                    Radius = 200,
                    MissileSpeed = 1300,
                    FixedRange = false,
                    AddHitbox = true,
                    DangerValue = 2,
                    IsDangerous = false,
                    MissileSpellName = "GalioResoluteSmite",
                });

            Spells.Add(
                new SpellData
                {
                    ChampionName = "Galio",
                    SpellName = "GalioRighteousGust",
                    Slot = SpellSlot.E,
                    Type = SkillShotType.SkillshotMissileLine,
                    Delay = 250,
                    Range = 1200,
                    Radius = 120,
                    MissileSpeed = 1200,
                    FixedRange = true,
                    AddHitbox = true,
                    DangerValue = 2,
                    IsDangerous = false,
                    MissileSpellName = "GalioRighteousGust",
                    CollisionObjects = new[] { CollisionObjectTypes.YasuoWall },
                });

            Spells.Add(
                new SpellData
                {
                    ChampionName = "Galio",
                    SpellName = "GalioIdolOfDurand",
                    Slot = SpellSlot.R,
                    Type = SkillShotType.SkillshotCircle,
                    Delay = 250,
                    Range = 0,
                    Radius = 550,
                    MissileSpeed = int.MaxValue,
                    FixedRange = true,
                    AddHitbox = false,
                    DangerValue = 5,
                    IsDangerous = true,
                    MissileSpellName = "",
                });

            #endregion Galio

            #region Gnar

            Spells.Add(
                new SpellData
                {
                    ChampionName = "Gnar",
                    SpellName = "GnarQ",
                    Slot = SpellSlot.Q,
                    Type = SkillShotType.SkillshotMissileLine,
                    Delay = 250,
                    Range = 1125,
                    Radius = 60,
                    MissileSpeed = 2500,
                    MissileAccel = -3000,
                    MissileMaxSpeed = 2500,
                    MissileMinSpeed = 1400,
                    FixedRange = true,
                    AddHitbox = true,
                    DangerValue = 2,
                    IsDangerous = false,
                    CanBeRemoved = true,
                    ForceRemove = true,
                    MissileSpellName = "gnarqmissile",
                    CollisionObjects = new[] { CollisionObjectTypes.YasuoWall },
                });

            Spells.Add(
                new SpellData
                {
                    ChampionName = "Gnar",
                    SpellName = "GnarQReturn",
                    Slot = SpellSlot.Q,
                    Type = SkillShotType.SkillshotMissileLine,
                    Delay = 0,
                    Range = 2500,
                    Radius = 75,
                    MissileSpeed = 60,
                    MissileAccel = 800,
                    MissileMaxSpeed = 2600,
                    MissileMinSpeed = 60,
                    FixedRange = true,
                    AddHitbox = true,
                    DangerValue = 2,
                    IsDangerous = false,
                    CanBeRemoved = true,
                    ForceRemove = true,
                    MissileSpellName = "GnarQMissileReturn",
                    DisableFowDetection = false,
                    CollisionObjects = new[] { CollisionObjectTypes.YasuoWall },
                });

            Spells.Add(
                new SpellData
                {
                    ChampionName = "Gnar",
                    SpellName = "GnarBigQ",
                    Slot = SpellSlot.Q,
                    Type = SkillShotType.SkillshotMissileLine,
                    Delay = 500,
                    Range = 1150,
                    Radius = 90,
                    MissileSpeed = 2100,
                    FixedRange = true,
                    AddHitbox = true,
                    DangerValue = 2,
                    IsDangerous = false,
                    MissileSpellName = "GnarBigQMissile",
                    CollisionObjects = new[] { CollisionObjectTypes.YasuoWall },
                });

            Spells.Add(
                new SpellData
                {
                    ChampionName = "Gnar",
                    SpellName = "GnarBigW",
                    Slot = SpellSlot.W,
                    Type = SkillShotType.SkillshotLine,
                    Delay = 600,
                    Range = 600,
                    Radius = 80,
                    MissileSpeed = int.MaxValue,
                    FixedRange = true,
                    AddHitbox = true,
                    DangerValue = 2,
                    IsDangerous = false,
                    MissileSpellName = "GnarBigW",
                });

            Spells.Add(
                new SpellData
                {
                    ChampionName = "Gnar",
                    SpellName = "GnarE",
                    Slot = SpellSlot.E,
                    Type = SkillShotType.SkillshotCircle,
                    Delay = 0,
                    Range = 473,
                    Radius = 150,
                    MissileSpeed = 903,
                    FixedRange = false,
                    AddHitbox = true,
                    DangerValue = 2,
                    IsDangerous = false,
                    MissileSpellName = "GnarE",
                });

            Spells.Add(
                new SpellData
                {
                    ChampionName = "Gnar",
                    SpellName = "GnarBigE",
                    Slot = SpellSlot.E,
                    Type = SkillShotType.SkillshotCircle,
                    Delay = 250,
                    Range = 475,
                    Radius = 200,
                    MissileSpeed = 1000,
                    FixedRange = false,
                    AddHitbox = true,
                    DangerValue = 2,
                    IsDangerous = false,
                    MissileSpellName = "GnarBigE",
                });

            Spells.Add(
                new SpellData
                {
                    ChampionName = "Gnar",
                    SpellName = "GnarR",
                    Slot = SpellSlot.R,
                    Type = SkillShotType.SkillshotCircle,
                    Delay = 250,
                    Range = 0,
                    Radius = 500,
                    MissileSpeed = int.MaxValue,
                    FixedRange = true,
                    AddHitbox = false,
                    DangerValue = 5,
                    IsDangerous = true,
                    MissileSpellName = "",
                });

            #endregion

            #region Gragas

            Spells.Add(
                new SpellData
                {
                    ChampionName = "Gragas",
                    SpellName = "GragasQ",
                    Slot = SpellSlot.Q,
                    Type = SkillShotType.SkillshotCircle,
                    Delay = 250,
                    Range = 1100,
                    Radius = 275,
                    MissileSpeed = 1300,
                    FixedRange = false,
                    AddHitbox = true,
                    DangerValue = 2,
                    IsDangerous = false,
                    MissileSpellName = "GragasQMissile",
                    ExtraDuration = 4500,
                    ToggleParticleName = "Gragas_",
                    DontCross = true,
                });

            Spells.Add(
                new SpellData
                {
                    ChampionName = "Gragas",
                    SpellName = "GragasE",
                    Slot = SpellSlot.E,
                    Type = SkillShotType.SkillshotMissileLine,
                    Delay = 0,
                    Range = 950,
                    Radius = 200,
                    MissileSpeed = 1200,
                    FixedRange = true,
                    AddHitbox = true,
                    DangerValue = 2,
                    IsDangerous = false,
                    MissileSpellName = "GragasE",
                    CanBeRemoved = true,
                    ExtraRange = 300,
                    CollisionObjects = new[] { CollisionObjectTypes.Champions, CollisionObjectTypes.Minion },
                });

            Spells.Add(
                new SpellData
                {
                    ChampionName = "Gragas",
                    SpellName = "GragasR",
                    Slot = SpellSlot.R,
                    Type = SkillShotType.SkillshotCircle,
                    Delay = 250,
                    Range = 1050,
                    Radius = 375,
                    MissileSpeed = 1800,
                    FixedRange = false,
                    AddHitbox = true,
                    DangerValue = 5,
                    IsDangerous = true,
                    MissileSpellName = "GragasRBoom",
                    CollisionObjects = new[] { CollisionObjectTypes.YasuoWall },
                });

            #endregion Gragas

            #region Graves

            Spells.Add(
                new SpellData
                {
                    ChampionName = "Graves",
                    SpellName = "GravesClusterShot",
                    Slot = SpellSlot.Q,
                    Type = SkillShotType.SkillshotMissileLine,
                    Delay = 250,
                    Range = 1000,
                    Radius = 50,
                    MissileSpeed = 2000,
                    FixedRange = true,
                    AddHitbox = true,
                    DangerValue = 2,
                    IsDangerous = false,
                    MissileSpellName = "GravesClusterShotAttack",
                    CollisionObjects = new[] { CollisionObjectTypes.YasuoWall },
                    MultipleNumber = 3,
                    MultipleAngle = 15 * (float) Math.PI / 180,
                });

            Spells.Add(
                new SpellData
                {
                    ChampionName = "Graves",
                    SpellName = "GravesChargeShot",
                    Slot = SpellSlot.R,
                    Type = SkillShotType.SkillshotMissileLine,
                    Delay = 250,
                    Range = 1100,
                    Radius = 100,
                    MissileSpeed = 2100,
                    FixedRange = true,
                    AddHitbox = true,
                    DangerValue = 5,
                    IsDangerous = true,
                    MissileSpellName = "GravesChargeShotShot",
                    CollisionObjects =
                        new[]
                        { CollisionObjectTypes.Champions, CollisionObjectTypes.Minion, CollisionObjectTypes.YasuoWall },
                });

            #endregion Graves

            #region Heimerdinger

            Spells.Add(
                new SpellData
                {
                    ChampionName = "Heimerdinger",
                    SpellName = "Heimerdingerwm",
                    Slot = SpellSlot.W,
                    Type = SkillShotType.SkillshotMissileLine,
                    Delay = 250,
                    Range = 1500,
                    Radius = 70,
                    MissileSpeed = 1800,
                    FixedRange = true,
                    AddHitbox = true,
                    DangerValue = 2,
                    IsDangerous = false,
                    MissileSpellName = "HeimerdingerWAttack2",
                    CollisionObjects = new[] { CollisionObjectTypes.YasuoWall },
                });

            Spells.Add(
                new SpellData
                {
                    ChampionName = "Heimerdinger",
                    SpellName = "HeimerdingerE",
                    Slot = SpellSlot.E,
                    Type = SkillShotType.SkillshotCircle,
                    Delay = 250,
                    Range = 925,
                    Radius = 100,
                    MissileSpeed = 1200,
                    FixedRange = false,
                    AddHitbox = true,
                    DangerValue = 2,
                    IsDangerous = false,
                    MissileSpellName = "heimerdingerespell",
                    CollisionObjects = new[] { CollisionObjectTypes.YasuoWall },
                });

            #endregion Heimerdinger

            #region Irelia

            Spells.Add(
                new SpellData
                {
                    ChampionName = "Irelia",
                    SpellName = "IreliaTranscendentBlades",
                    Slot = SpellSlot.R,
                    Type = SkillShotType.SkillshotMissileLine,
                    Delay = 0,
                    Range = 1200,
                    Radius = 65,
                    MissileSpeed = 1600,
                    FixedRange = true,
                    AddHitbox = true,
                    DangerValue = 2,
                    IsDangerous = false,
                    MissileSpellName = "IreliaTranscendentBlades",
                    CollisionObjects = new[] { CollisionObjectTypes.YasuoWall },
                });

            #endregion Irelia

            #region Janna

            Spells.Add(
                new SpellData
                {
                    ChampionName = "Janna",
                    SpellName = "JannaQ",
                    Slot = SpellSlot.Q,
                    Type = SkillShotType.SkillshotMissileLine,
                    Delay = 250,
                    Range = 1700,
                    Radius = 120,
                    MissileSpeed = 900,
                    FixedRange = false,
                    AddHitbox = true,
                    DangerValue = 2,
                    IsDangerous = false,
                    MissileSpellName = "HowlingGaleSpell",
                    CollisionObjects = new[] { CollisionObjectTypes.YasuoWall },
                });

            #endregion Janna

            #region JarvanIV

            Spells.Add(
                new SpellData
                {
                    ChampionName = "JarvanIV",
                    SpellName = "JarvanIVDragonStrike",
                    Slot = SpellSlot.Q,
                    Type = SkillShotType.SkillshotMissileLine,
                    Delay = 250,
                    Range = 880,
                    Radius = 70,
                    MissileSpeed = 1450,
                    FixedRange = true,
                    AddHitbox = true,
                    DangerValue = 3,
                    IsDangerous = true,
                    MissileSpellName = "JarvanIVDragonStrike",
                });

            Spells.Add(
                new SpellData
                {
                    ChampionName = "JarvanIV",
                    SpellName = "JarvanIVDemacianStandard",
                    Slot = SpellSlot.E,
                    Type = SkillShotType.SkillshotCircle,
                    Delay = 500,
                    Range = 860,
                    Radius = 175,
                    MissileSpeed = int.MaxValue,
                    FixedRange = false,
                    AddHitbox = true,
                    DangerValue = 2,
                    IsDangerous = false,
                    MissileSpellName = "JarvanIVDemacianStandard",
                });

            #endregion JarvanIV

            #region Jayce

            Spells.Add(
                new SpellData
                {
                    ChampionName = "Jayce",
                    SpellName = "jayceshockblast",
                    Slot = SpellSlot.Q,
                    Type = SkillShotType.SkillshotMissileLine,
                    Delay = 250,
                    Range = 1300,
                    Radius = 70,
                    MissileSpeed = 1450,
                    FixedRange = true,
                    AddHitbox = true,
                    DangerValue = 2,
                    IsDangerous = false,
                    MissileSpellName = "JayceShockBlastMis",
                    CanBeRemoved = true,
                    CollisionObjects =
                        new[]
                        { CollisionObjectTypes.Champions, CollisionObjectTypes.Minion, CollisionObjectTypes.YasuoWall },
                });

            Spells.Add(
                new SpellData
                {
                    ChampionName = "Jayce",
                    SpellName = "JayceQAccel",
                    Slot = SpellSlot.Q,
                    Type = SkillShotType.SkillshotMissileLine,
                    Delay = 250,
                    Range = 1300,
                    Radius = 70,
                    MissileSpeed = 2350,
                    FixedRange = true,
                    AddHitbox = true,
                    DangerValue = 2,
                    IsDangerous = false,
                    MissileSpellName = "JayceShockBlastWallMis",
                    CanBeRemoved = true,
                    CollisionObjects =
                        new[]
                        { CollisionObjectTypes.Champions, CollisionObjectTypes.Minion, CollisionObjectTypes.YasuoWall },
                });

            #endregion Jayce

            #region Jinx

            //TODO: Detect the animation from fow instead of the missile.
            Spells.Add(
                new SpellData
                {
                    ChampionName = "Jinx",
                    SpellName = "JinxW",
                    Slot = SpellSlot.W,
                    Type = SkillShotType.SkillshotMissileLine,
                    Delay = 600,
                    Range = 1500,
                    Radius = 60,
                    MissileSpeed = 3300,
                    FixedRange = true,
                    AddHitbox = true,
                    DangerValue = 3,
                    IsDangerous = true,
                    MissileSpellName = "JinxWMissile",
                    CanBeRemoved = true,
                    CollisionObjects =
                        new[]
                        { CollisionObjectTypes.Champions, CollisionObjectTypes.Minion, CollisionObjectTypes.YasuoWall },
                });

            Spells.Add(
                new SpellData
                {
                    ChampionName = "Jinx",
                    SpellName = "JinxR",
                    Slot = SpellSlot.R,
                    Type = SkillShotType.SkillshotMissileLine,
                    Delay = 600,
                    Range = 20000,
                    Radius = 140,
                    MissileSpeed = 1700,
                    FixedRange = true,
                    AddHitbox = true,
                    DangerValue = 5,
                    IsDangerous = true,
                    MissileSpellName = "JinxR",
                    CanBeRemoved = true,
                    CollisionObjects = new[] { CollisionObjectTypes.Champions, CollisionObjectTypes.YasuoWall },
                });

            #endregion Jinx

            #region Kalista

            Spells.Add(
                new SpellData
                {
                    ChampionName = "Kalista",
                    SpellName = "KalistaMysticShot",
                    Slot = SpellSlot.Q,
                    Type = SkillShotType.SkillshotMissileLine,
                    Delay = 250,
                    Range = 1200,
                    Radius = 40,
                    MissileSpeed = 1700,
                    FixedRange = true,
                    AddHitbox = true,
                    DangerValue = 2,
                    IsDangerous = false,
                    MissileSpellName = "kalistamysticshotmis",
                    ExtraMissileNames = new[] { "kalistamysticshotmistrue" },
                    CanBeRemoved = true,
                    CollisionObjects =
                        new[] { CollisionObjectTypes.Champions, CollisionObjectTypes.Minion, CollisionObjectTypes.YasuoWall },
                });

            #endregion Kalista

            #region Karma

            Spells.Add(
                new SpellData
                {
                    ChampionName = "Karma",
                    SpellName = "KarmaQ",
                    Slot = SpellSlot.Q,
                    Type = SkillShotType.SkillshotMissileLine,
                    Delay = 250,
                    Range = 1050,
                    Radius = 60,
                    MissileSpeed = 1700,
                    FixedRange = true,
                    AddHitbox = true,
                    DangerValue = 2,
                    IsDangerous = false,
                    MissileSpellName = "KarmaQMissile",
                    CanBeRemoved = true,
                    CollisionObjects =
                        new[]
                        { CollisionObjectTypes.Champions, CollisionObjectTypes.Minion, CollisionObjectTypes.YasuoWall },
                });

            //TODO: add the circle at the end.
            Spells.Add(
                new SpellData
                {
                    ChampionName = "Karma",
                    SpellName = "KarmaQMantra",
                    Slot = SpellSlot.Q,
                    Type = SkillShotType.SkillshotMissileLine,
                    Delay = 250,
                    Range = 950,
                    Radius = 80,
                    MissileSpeed = 1700,
                    FixedRange = true,
                    AddHitbox = true,
                    DangerValue = 2,
                    IsDangerous = false,
                    MissileSpellName = "KarmaQMissileMantra",
                    CanBeRemoved = true,
                    CollisionObjects =
                        new[]
                        { CollisionObjectTypes.Champions, CollisionObjectTypes.Minion, CollisionObjectTypes.YasuoWall },
                });

            #endregion Karma

            #region Karthus

            Spells.Add(
                new SpellData
                {
                    ChampionName = "Karthus",
                    SpellName = "KarthusLayWasteA2",
                    ExtraSpellNames =
                        new[]
                        {
                            "karthuslaywastea3", "karthuslaywastea1", "karthuslaywastedeada1", "karthuslaywastedeada2",
                            "karthuslaywastedeada3"
                        },
                    Slot = SpellSlot.Q,
                    Type = SkillShotType.SkillshotCircle,
                    Delay = 625,
                    Range = 875,
                    Radius = 160,
                    MissileSpeed = int.MaxValue,
                    FixedRange = false,
                    AddHitbox = true,
                    DangerValue = 2,
                    IsDangerous = false,
                    MissileSpellName = "",
                });

            #endregion Karthus

            #region Kassadin

            Spells.Add(
                new SpellData
                {
                    ChampionName = "Kassadin",
                    SpellName = "RiftWalk",
                    Slot = SpellSlot.R,
                    Type = SkillShotType.SkillshotCircle,
                    Delay = 250,
                    Range = 450,
                    Radius = 270,
                    MissileSpeed = int.MaxValue,
                    FixedRange = false,
                    AddHitbox = true,
                    DangerValue = 2,
                    IsDangerous = false,
                    MissileSpellName = "RiftWalk",
                });

            #endregion Kassadin

            #region Kennen

            Spells.Add(
                new SpellData
                {
                    ChampionName = "Kennen",
                    SpellName = "KennenShurikenHurlMissile1",
                    Slot = SpellSlot.Q,
                    Type = SkillShotType.SkillshotMissileLine,
                    Delay = 125,
                    Range = 1050,
                    Radius = 50,
                    MissileSpeed = 1700,
                    FixedRange = true,
                    AddHitbox = true,
                    DangerValue = 2,
                    IsDangerous = false,
                    MissileSpellName = "KennenShurikenHurlMissile1",
                    CanBeRemoved = true,
                    CollisionObjects =
                        new[]
                        { CollisionObjectTypes.Champions, CollisionObjectTypes.Minion, CollisionObjectTypes.YasuoWall },
                });

            #endregion Kennen

            #region Khazix

            Spells.Add(
                new SpellData
                {
                    ChampionName = "Khazix",
                    SpellName = "KhazixW",
                    ExtraSpellNames = new[] { "khazixwlong" },
                    Slot = SpellSlot.W,
                    Type = SkillShotType.SkillshotMissileLine,
                    Delay = 250,
                    Range = 1025,
                    Radius = 73,
                    MissileSpeed = 1700,
                    FixedRange = true,
                    AddHitbox = true,
                    DangerValue = 2,
                    IsDangerous = false,
                    MissileSpellName = "KhazixWMissile",
                    CanBeRemoved = true,
                    MultipleNumber = 3,
                    MultipleAngle = 22f * (float) Math.PI / 180,
                    CollisionObjects =
                        new[]
                        { CollisionObjectTypes.Champions, CollisionObjectTypes.Minion, CollisionObjectTypes.YasuoWall },
                });

            Spells.Add(
                new SpellData
                {
                    ChampionName = "Khazix",
                    SpellName = "KhazixE",
                    Slot = SpellSlot.E,
                    Type = SkillShotType.SkillshotCircle,
                    Delay = 250,
                    Range = 600,
                    Radius = 300,
                    MissileSpeed = 1500,
                    FixedRange = false,
                    AddHitbox = true,
                    DangerValue = 2,
                    IsDangerous = false,
                    MissileSpellName = "KhazixE",
                });

            #endregion Khazix

            #region Kogmaw

            Spells.Add(
                new SpellData
                {
                    ChampionName = "Kogmaw",
                    SpellName = "KogMawQ",
                    Slot = SpellSlot.Q,
                    Type = SkillShotType.SkillshotMissileLine,
                    Delay = 250,
                    Range = 1200,
                    Radius = 70,
                    MissileSpeed = 1650,
                    FixedRange = true,
                    AddHitbox = true,
                    DangerValue = 2,
                    IsDangerous = false,
                    MissileSpellName = "KogMawQMis",
                    CanBeRemoved = true,
                    CollisionObjects =
                        new[]
                        { CollisionObjectTypes.Champions, CollisionObjectTypes.Minion, CollisionObjectTypes.YasuoWall },
                });

            Spells.Add(
                new SpellData
                {
                    ChampionName = "Kogmaw",
                    SpellName = "KogMawVoidOoze",
                    Slot = SpellSlot.E,
                    Type = SkillShotType.SkillshotMissileLine,
                    Delay = 250,
                    Range = 1360,
                    Radius = 120,
                    MissileSpeed = 1400,
                    FixedRange = true,
                    AddHitbox = true,
                    DangerValue = 2,
                    IsDangerous = false,
                    MissileSpellName = "KogMawVoidOozeMissile",
                    CollisionObjects = new[] { CollisionObjectTypes.YasuoWall },
                });

            Spells.Add(
                new SpellData
                {
                    ChampionName = "Kogmaw",
                    SpellName = "KogMawLivingArtillery",
                    Slot = SpellSlot.R,
                    Type = SkillShotType.SkillshotCircle,
                    Delay = 1200,
                    Range = 1800,
                    Radius = 150,
                    MissileSpeed = int.MaxValue,
                    FixedRange = false,
                    AddHitbox = true,
                    DangerValue = 2,
                    IsDangerous = false,
                    MissileSpellName = "KogMawLivingArtillery",
                });

            #endregion Kogmaw

            #region Leblanc

            Spells.Add(
                new SpellData
                {
                    ChampionName = "Leblanc",
                    SpellName = "LeblancSlide",
                    Slot = SpellSlot.W,
                    Type = SkillShotType.SkillshotCircle,
                    Delay = 0,
                    Range = 600,
                    Radius = 220,
                    MissileSpeed = 1500,
                    FixedRange = false,
                    AddHitbox = true,
                    DangerValue = 2,
                    IsDangerous = false,
                    MissileSpellName = "LeblancSlide",
                });

            Spells.Add(
                new SpellData
                {
                    ChampionName = "Leblanc",
                    SpellName = "LeblancSlideM",
                    Slot = SpellSlot.W,
                    Type = SkillShotType.SkillshotCircle,
                    Delay = 0,
                    Range = 600,
                    Radius = 220,
                    MissileSpeed = 1500,
                    FixedRange = false,
                    AddHitbox = true,
                    DangerValue = 2,
                    IsDangerous = false,
                    MissileSpellName = "LeblancSlideM",
                });

            Spells.Add(
                new SpellData
                {
                    ChampionName = "Leblanc",
                    SpellName = "LeblancSoulShackle",
                    Slot = SpellSlot.E,
                    Type = SkillShotType.SkillshotMissileLine,
                    Delay = 250,
                    Range = 950,
                    Radius = 70,
                    MissileSpeed = 1600,
                    FixedRange = true,
                    AddHitbox = true,
                    DangerValue = 3,
                    IsDangerous = true,
                    MissileSpellName = "LeblancSoulShackle",
                    CanBeRemoved = true,
                    CollisionObjects =
                        new[]
                        { CollisionObjectTypes.Champions, CollisionObjectTypes.Minion, CollisionObjectTypes.YasuoWall },
                });

            Spells.Add(
                new SpellData
                {
                    ChampionName = "Leblanc",
                    SpellName = "LeblancSoulShackleM",
                    Slot = SpellSlot.E,
                    Type = SkillShotType.SkillshotMissileLine,
                    Delay = 250,
                    Range = 950,
                    Radius = 70,
                    MissileSpeed = 1600,
                    FixedRange = true,
                    AddHitbox = true,
                    DangerValue = 3,
                    IsDangerous = true,
                    MissileSpellName = "LeblancSoulShackleM",
                    CanBeRemoved = true,
                    CollisionObjects =
                        new[]
                        { CollisionObjectTypes.Champions, CollisionObjectTypes.Minion, CollisionObjectTypes.YasuoWall },
                });

            #endregion Leblanc

            #region LeeSin

            Spells.Add(
                new SpellData
                {
                    ChampionName = "LeeSin",
                    SpellName = "BlindMonkQOne",
                    Slot = SpellSlot.Q,
                    Type = SkillShotType.SkillshotMissileLine,
                    Delay = 250,
                    Range = 1100,
                    Radius = 65,
                    MissileSpeed = 1800,
                    FixedRange = true,
                    AddHitbox = true,
                    DangerValue = 3,
                    IsDangerous = true,
                    MissileSpellName = "BlindMonkQOne",
                    CanBeRemoved = true,
                    CollisionObjects =
                        new[]
                        { CollisionObjectTypes.Champions, CollisionObjectTypes.Minion, CollisionObjectTypes.YasuoWall },
                });

            #endregion LeeSin

            #region Leona

            Spells.Add(
                new SpellData
                {
                    ChampionName = "Leona",
                    SpellName = "LeonaZenithBlade",
                    Slot = SpellSlot.E,
                    Type = SkillShotType.SkillshotMissileLine,
                    Delay = 250,
                    Range = 905,
                    Radius = 120,
                    MissileSpeed = 2000,
                    FixedRange = true,
                    AddHitbox = true,
                    DangerValue = 3,
                    IsDangerous = true,
                    MissileSpellName = "LeonaZenithBladeMissile",
                    CollisionObjects = new[] { CollisionObjectTypes.YasuoWall },
                });

            Spells.Add(
                new SpellData
                {
                    ChampionName = "Leona",
                    SpellName = "LeonaSolarFlare",
                    Slot = SpellSlot.R,
                    Type = SkillShotType.SkillshotCircle,
                    Delay = 1000,
                    Range = 1200,
                    Radius = 300,
                    MissileSpeed = int.MaxValue,
                    FixedRange = false,
                    AddHitbox = true,
                    DangerValue = 5,
                    IsDangerous = true,
                    MissileSpellName = "LeonaSolarFlare",
                });

            #endregion Leona

            #region Lissandra

            Spells.Add(
                new SpellData
                {
                    ChampionName = "Lissandra",
                    SpellName = "LissandraQ",
                    Slot = SpellSlot.Q,
                    Type = SkillShotType.SkillshotMissileLine,
                    Delay = 250,
                    Range = 700,
                    Radius = 75,
                    MissileSpeed = 2200,
                    FixedRange = true,
                    AddHitbox = true,
                    DangerValue = 2,
                    IsDangerous = false,
                    MissileSpellName = "LissandraQMissile",
                    CollisionObjects = new[] { CollisionObjectTypes.YasuoWall },
                });

            Spells.Add(
                new SpellData
                {
                    ChampionName = "Lissandra",
                    SpellName = "LissandraQShards",
                    Slot = SpellSlot.Q,
                    Type = SkillShotType.SkillshotMissileLine,
                    Delay = 250,
                    Range = 700,
                    Radius = 90,
                    MissileSpeed = 2200,
                    FixedRange = true,
                    AddHitbox = true,
                    DangerValue = 2,
                    IsDangerous = false,
                    MissileSpellName = "lissandraqshards",
                    CollisionObjects = new[] { CollisionObjectTypes.YasuoWall },
                });

            Spells.Add(
                new SpellData
                {
                    ChampionName = "Lissandra",
                    SpellName = "LissandraE",
                    Slot = SpellSlot.E,
                    Type = SkillShotType.SkillshotMissileLine,
                    Delay = 250,
                    Range = 1025,
                    Radius = 125,
                    MissileSpeed = 850,
                    FixedRange = true,
                    AddHitbox = true,
                    DangerValue = 2,
                    IsDangerous = false,
                    MissileSpellName = "LissandraEMissile",
                    CollisionObjects = new[] { CollisionObjectTypes.YasuoWall },
                });
            #endregion Lulu

            #region Lucian

            Spells.Add(
                new SpellData
                {
                    ChampionName = "Lucian",
                    SpellName = "LucianQ",
                    Slot = SpellSlot.Q,
                    Type = SkillShotType.SkillshotLine,
                    Delay = 500,
                    Range = 1300,
                    Radius = 65,
                    MissileSpeed = int.MaxValue,
                    FixedRange = true,
                    AddHitbox = true,
                    DangerValue = 2,
                    IsDangerous = false,
                    MissileSpellName = "LucianQ",
                });

            #endregion Lucian

            #region Lulu

            Spells.Add(
                new SpellData
                {
                    ChampionName = "Lulu",
                    SpellName = "LuluQ",
                    Slot = SpellSlot.Q,
                    Type = SkillShotType.SkillshotMissileLine,
                    Delay = 250,
                    Range = 950,
                    Radius = 60,
                    MissileSpeed = 1450,
                    FixedRange = true,
                    AddHitbox = true,
                    DangerValue = 2,
                    IsDangerous = false,
                    MissileSpellName = "LuluQMissile",
                    ExtraMissileNames = new[] { "LuluQMissileTwo" },
                    CollisionObjects = new[] { CollisionObjectTypes.YasuoWall },
                });

            #endregion Lulu

            #region Lux

            Spells.Add(
                new SpellData
                {
                    ChampionName = "Lux",
                    SpellName = "LuxLightBinding",
                    Slot = SpellSlot.Q,
                    Type = SkillShotType.SkillshotMissileLine,
                    Delay = 250,
                    Range = 1300,
                    Radius = 70,
                    MissileSpeed = 1200,
                    FixedRange = true,
                    AddHitbox = true,
                    DangerValue = 3,
                    IsDangerous = true,
                    MissileSpellName = "LuxLightBindingMis",
                    //CanBeRemoved = true,
                    //CollisionObjects = new[] { CollisionObjectTypes.Champions, CollisionObjectTypes.Minion, CollisionObjectTypes.YasuoWall, },
                });

            Spells.Add(
                new SpellData
                {
                    ChampionName = "Lux",
                    SpellName = "LuxLightStrikeKugel",
                    Slot = SpellSlot.E,
                    Type = SkillShotType.SkillshotCircle,
                    Delay = 250,
                    Range = 1100,
                    Radius = 275,
                    MissileSpeed = 1300,
                    FixedRange = false,
                    AddHitbox = true,
                    DangerValue = 2,
                    IsDangerous = false,
                    MissileSpellName = "LuxLightStrikeKugel",
                    ExtraDuration = 5500,
                    ToggleParticleName = "LuxLightstrike_tar",
                    DontCross = true,
                    CanBeRemoved = true,
                    CollisionObjects = new[] { CollisionObjectTypes.YasuoWall },
                });

            Spells.Add(
                new SpellData
                {
                    ChampionName = "Lux",
                    SpellName = "LuxMaliceCannon",
                    Slot = SpellSlot.R,
                    Type = SkillShotType.SkillshotLine,
                    Delay = 1000,
                    Range = 3500,
                    Radius = 190,
                    MissileSpeed = int.MaxValue,
                    FixedRange = true,
                    AddHitbox = true,
                    DangerValue = 5,
                    IsDangerous = true,
                    MissileSpellName = "LuxMaliceCannon",
                });

            #endregion Lux

            #region Malphite

            Spells.Add(
                new SpellData
                {
                    ChampionName = "Malphite",
                    SpellName = "UFSlash",
                    Slot = SpellSlot.R,
                    Type = SkillShotType.SkillshotCircle,
                    Delay = 0,
                    Range = 1000,
                    Radius = 270,
                    MissileSpeed = 1500,
                    FixedRange = false,
                    AddHitbox = true,
                    DangerValue = 5,
                    IsDangerous = true,
                    MissileSpellName = "UFSlash",
                });

            #endregion Malphite

            #region Malzahar

            Spells.Add(
                new SpellData
                {
                    ChampionName = "Malzahar",
                    SpellName = "AlZaharCalloftheVoid",
                    Slot = SpellSlot.Q,
                    Type = SkillShotType.SkillshotLine,
                    Delay = 1000,
                    Range = 900,
                    Radius = 85,
                    MissileSpeed = int.MaxValue,
                    FixedRange = false,
                    AddHitbox = true,
                    DangerValue = 2,
                    IsDangerous = false,
                    DontCross = true,
                    MissileSpellName = "AlZaharCalloftheVoid",
                });

            #endregion Malzahar

            #region Morgana

            Spells.Add(
                new SpellData
                {
                    ChampionName = "Morgana",
                    SpellName = "DarkBindingMissile",
                    Slot = SpellSlot.Q,
                    Type = SkillShotType.SkillshotMissileLine,
                    Delay = 250,
                    Range = 1300,
                    Radius = 80,
                    MissileSpeed = 1200,
                    FixedRange = true,
                    AddHitbox = true,
                    DangerValue = 3,
                    IsDangerous = true,
                    MissileSpellName = "DarkBindingMissile",
                    CanBeRemoved = true,
                    CollisionObjects =
                        new[]
                        { CollisionObjectTypes.Champions, CollisionObjectTypes.Minion, CollisionObjectTypes.YasuoWall },
                });

            #endregion Morgana

            #region Nami

            Spells.Add(
                new SpellData
                {
                    ChampionName = "Nami",
                    SpellName = "NamiQ",
                    Slot = SpellSlot.Q,
                    Type = SkillShotType.SkillshotCircle,
                    Delay = 1000,
                    Range = 1625,
                    Radius = 150,
                    MissileSpeed = int.MaxValue,
                    FixedRange = false,
                    AddHitbox = true,
                    DangerValue = 3,
                    IsDangerous = true,
                    MissileSpellName = "namiqmissile",
                });

            Spells.Add(
                new SpellData
                {
                    ChampionName = "Nami",
                    SpellName = "NamiR",
                    Slot = SpellSlot.R,
                    Type = SkillShotType.SkillshotMissileLine,
                    Delay = 500,
                    Range = 2750,
                    Radius = 260,
                    MissileSpeed = 850,
                    FixedRange = true,
                    AddHitbox = true,
                    DangerValue = 2,
                    IsDangerous = false,
                    MissileSpellName = "NamiRMissile",
                    CollisionObjects = new[] { CollisionObjectTypes.YasuoWall },
                });

            #endregion Nami

            #region Nautilus

            Spells.Add(
                new SpellData
                {
                    ChampionName = "Nautilus",
                    SpellName = "NautilusAnchorDrag",
                    Slot = SpellSlot.Q,
                    Type = SkillShotType.SkillshotMissileLine,
                    Delay = 250,
                    Range = 1100,
                    Radius = 90,
                    MissileSpeed = 2000,
                    FixedRange = true,
                    AddHitbox = true,
                    DangerValue = 3,
                    IsDangerous = true,
                    MissileSpellName = "NautilusAnchorDragMissile",
                    CanBeRemoved = true,
                    CollisionObjects =
                        new[]
                        { CollisionObjectTypes.Champions, CollisionObjectTypes.Minion, CollisionObjectTypes.YasuoWall },
                    //walls?
                });

            #endregion Nautilus

            #region Nidalee

            Spells.Add(
                new SpellData
                {
                    ChampionName = "Nidalee",
                    SpellName = "JavelinToss",
                    Slot = SpellSlot.Q,
                    Type = SkillShotType.SkillshotMissileLine,
                    Delay = 125,
                    Range = 1500,
                    Radius = 40,
                    MissileSpeed = 1300,
                    FixedRange = true,
                    AddHitbox = true,
                    DangerValue = 3,
                    IsDangerous = true,
                    MissileSpellName = "JavelinToss",
                    CanBeRemoved = true,
                    CollisionObjects =
                        new[]
                        { CollisionObjectTypes.Champions, CollisionObjectTypes.Minion, CollisionObjectTypes.YasuoWall },
                });

            #endregion Nidalee

            #region Olaf

            Spells.Add(
                new SpellData
                {
                    ChampionName = "Olaf",
                    SpellName = "OlafAxeThrowCast",
                    Slot = SpellSlot.Q,
                    Type = SkillShotType.SkillshotMissileLine,
                    Delay = 250,
                    Range = 1000,
                    ExtraRange = 150,
                    Radius = 105,
                    MissileSpeed = 1600,
                    FixedRange = false,
                    AddHitbox = true,
                    DangerValue = 2,
                    IsDangerous = false,
                    MissileSpellName = "olafaxethrow",
                    CanBeRemoved = true,
                    CollisionObjects =
                        new[]
                        { CollisionObjectTypes.Champions, CollisionObjectTypes.Minion, CollisionObjectTypes.YasuoWall },
                });

            #endregion Olaf

            #region Orianna

            Spells.Add(
                new SpellData
                {
                    ChampionName = "Orianna",
                    SpellName = "OriannasQ",
                    Slot = SpellSlot.Q,
                    Type = SkillShotType.SkillshotMissileLine,
                    Delay = 0,
                    Range = 1500,
                    Radius = 80,
                    MissileSpeed = 1200,
                    FixedRange = false,
                    AddHitbox = true,
                    DangerValue = 2,
                    IsDangerous = false,
                    MissileSpellName = "orianaizuna",
                    CollisionObjects = new[] { CollisionObjectTypes.YasuoWall },
                });

            Spells.Add(
                new SpellData
                {
                    ChampionName = "Orianna",
                    SpellName = "OriannaQend",
                    Slot = SpellSlot.Q,
                    Type = SkillShotType.SkillshotCircle,
                    Delay = 0,
                    Range = 1500,
                    Radius = 90,
                    MissileSpeed = 1200,
                    FixedRange = false,
                    AddHitbox = true,
                    DangerValue = 2,
                    IsDangerous = false,
                    MissileSpellName = "",
                    CollisionObjects = new[] { CollisionObjectTypes.YasuoWall },
                });

            Spells.Add(
                new SpellData
                {
                    ChampionName = "Orianna",
                    SpellName = "OrianaDissonanceCommand",
                    Slot = SpellSlot.W,
                    Type = SkillShotType.SkillshotCircle,
                    Delay = 250,
                    Range = 0,
                    Radius = 255,
                    MissileSpeed = int.MaxValue,
                    FixedRange = true,
                    AddHitbox = true,
                    DangerValue = 2,
                    IsDangerous = false,
                    MissileSpellName = "OrianaDissonanceCommand",
                    FromObject = "yomu_ring_",
                });

            Spells.Add(
                new SpellData
                {
                    ChampionName = "Orianna",
                    SpellName = "OriannasE",
                    Slot = SpellSlot.E,
                    Type = SkillShotType.SkillshotMissileLine,
                    Delay = 0,
                    Range = 1500,
                    Radius = 85,
                    MissileSpeed = 1850,
                    FixedRange = false,
                    AddHitbox = true,
                    DangerValue = 2,
                    IsDangerous = false,
                    MissileSpellName = "orianaredact",
                    CollisionObjects = new[] { CollisionObjectTypes.YasuoWall },
                });

            Spells.Add(
                new SpellData
                {
                    ChampionName = "Orianna",
                    SpellName = "OrianaDetonateCommand",
                    Slot = SpellSlot.R,
                    Type = SkillShotType.SkillshotCircle,
                    Delay = 700,
                    Range = 0,
                    Radius = 410,
                    MissileSpeed = int.MaxValue,
                    FixedRange = true,
                    AddHitbox = true,
                    DangerValue = 5,
                    IsDangerous = true,
                    MissileSpellName = "OrianaDetonateCommand",
                    FromObject = "yomu_ring_",
                });

            #endregion Orianna

            #region Quinn

            Spells.Add(
                new SpellData
                {
                    ChampionName = "Quinn",
                    SpellName = "QuinnQ",
                    Slot = SpellSlot.Q,
                    Type = SkillShotType.SkillshotMissileLine,
                    Delay = 250,
                    Range = 1050,
                    Radius = 80,
                    MissileSpeed = 1550,
                    FixedRange = true,
                    AddHitbox = true,
                    DangerValue = 2,
                    IsDangerous = false,
                    MissileSpellName = "QuinnQMissile",
                    CanBeRemoved = true,
                    CollisionObjects =
                        new[]
                        { CollisionObjectTypes.Champions, CollisionObjectTypes.Minion, CollisionObjectTypes.YasuoWall },
                });

            #endregion Quinn

            #region Rengar

            Spells.Add(
                new SpellData
                {
                    ChampionName = "Rengar",
                    SpellName = "RengarE",
                    Slot = SpellSlot.E,
                    Type = SkillShotType.SkillshotMissileLine,
                    Delay = 250,
                    Range = 1000,
                    Radius = 70,
                    MissileSpeed = 1500,
                    FixedRange = true,
                    AddHitbox = true,
                    DangerValue = 3,
                    IsDangerous = true,
                    MissileSpellName = "RengarEFinal",
                    CanBeRemoved = true,
                    CollisionObjects =
                        new[]
                        { CollisionObjectTypes.Champions, CollisionObjectTypes.Minion, CollisionObjectTypes.YasuoWall },
                });

            #endregion Rengar
            
            #region RekSai

            Spells.Add(
                new SpellData
                {
                    ChampionName = "RekSai",
                    SpellName = "reksaiqburrowed",
                    Slot = SpellSlot.Q,
                    Type = SkillShotType.SkillshotMissileLine,
                    Delay = 500,
                    Range = 1625,
                    Radius = 60,
                    MissileSpeed = 1950,
                    FixedRange = true,
                    AddHitbox = true,
                    DangerValue = 3,
                    IsDangerous = false,
                    MissileSpellName = "RekSaiQBurrowedMis",
                    CollisionObjects =
                        new[]
                        { CollisionObjectTypes.Champions, CollisionObjectTypes.Minion, CollisionObjectTypes.YasuoWall },
                });

            #endregion RekSai

            #region Riven

            Spells.Add(
                new SpellData
                {
                    ChampionName = "Riven",
                    SpellName = "rivenizunablade",
                    Slot = SpellSlot.R,
                    Type = SkillShotType.SkillshotMissileLine,
                    Delay = 250,
                    Range = 1100,
                    Radius = 125,
                    MissileSpeed = 2200,
                    FixedRange = false,
                    AddHitbox = false,
                    DangerValue = 5,
                    IsDangerous = true,
                    MultipleNumber = 3,
                    MultipleAngle = 15 * (float) Math.PI / 180,
                    MissileSpellName = "RivenLightsaberMissile",
                    ExtraMissileNames = new[] { "RivenLightsaberMissileSide" }
                });

            #endregion Riven

            #region Rumble

            Spells.Add(
                new SpellData
                {
                    ChampionName = "Rumble",
                    SpellName = "RumbleGrenade",
                    Slot = SpellSlot.E,
                    Type = SkillShotType.SkillshotMissileLine,
                    Delay = 250,
                    Range = 950,
                    Radius = 60,
                    MissileSpeed = 2000,
                    FixedRange = true,
                    AddHitbox = true,
                    DangerValue = 2,
                    IsDangerous = false,
                    MissileSpellName = "RumbleGrenade",
                    CanBeRemoved = true,
                    CollisionObjects =
                        new[]
                        { CollisionObjectTypes.Champions, CollisionObjectTypes.Minion, CollisionObjectTypes.YasuoWall },
                });

            Spells.Add(
                new SpellData
                {
                    ChampionName = "Rumble",
                    SpellName = "RumbleCarpetBombM",
                    Slot = SpellSlot.R,
                    Type = SkillShotType.SkillshotMissileLine,
                    Delay = 400,
                    MissileDelayed = true,
                    Range = 1200,
                    Radius = 200,
                    MissileSpeed = 1600,
                    FixedRange = true,
                    AddHitbox = true,
                    DangerValue = 4,
                    IsDangerous = false,
                    MissileSpellName = "RumbleCarpetBombMissile",
                    CanBeRemoved = false,
                    CollisionObjects = new CollisionObjectTypes[] { },
                });

            #endregion Rumble

            #region Sejuani

            Spells.Add(
                new SpellData
                {
                    ChampionName = "Sejuani",
                    SpellName = "SejuaniArcticAssault",
                    Slot = SpellSlot.Q,
                    Type = SkillShotType.SkillshotMissileLine,
                    Delay = 0,
                    Range = 900,
                    Radius = 70,
                    MissileSpeed = 1600,
                    FixedRange = false,
                    AddHitbox = true,
                    DangerValue = 3,
                    IsDangerous = true,
                    MissileSpellName = "",
                    ExtraRange = 200,
                    CollisionObjects =
                        new[]
                        { CollisionObjectTypes.Champions, CollisionObjectTypes.Minion, CollisionObjectTypes.YasuoWall },
                });
            //TODO: fix?
            Spells.Add(
                new SpellData
                {
                    ChampionName = "Sejuani",
                    SpellName = "SejuaniGlacialPrisonStart",
                    Slot = SpellSlot.R,
                    Type = SkillShotType.SkillshotMissileLine,
                    Delay = 250,
                    Range = 1100,
                    Radius = 110,
                    MissileSpeed = 1600,
                    FixedRange = true,
                    AddHitbox = true,
                    DangerValue = 3,
                    IsDangerous = true,
                    MissileSpellName = "sejuaniglacialprison",
                    CanBeRemoved = true,
                    CollisionObjects = new[] { CollisionObjectTypes.Champions, CollisionObjectTypes.YasuoWall },
                });

            #endregion Sejuani

            #region Sion

            Spells.Add(
                new SpellData
                {
                    ChampionName = "Sion",
                    SpellName = "SionE",
                    Slot = SpellSlot.E,
                    Type = SkillShotType.SkillshotMissileLine,
                    Delay = 250,
                    Range = 800,
                    Radius = 80,
                    MissileSpeed = 1800,
                    FixedRange = true,
                    AddHitbox = true,
                    DangerValue = 3,
                    IsDangerous = true,
                    MissileSpellName = "SionEMissile",
                    CollisionObjects =
                        new[] { CollisionObjectTypes.Champions, CollisionObjectTypes.Minion, CollisionObjectTypes.YasuoWall },
                });

            #endregion Sion

            #region Soraka

            Spells.Add(
                new SpellData
                {
                    ChampionName = "Soraka",
                    SpellName = "SorakaQ",
                    Slot = SpellSlot.Q,
                    Type = SkillShotType.SkillshotCircle,
                    Delay = 500,
                    Range = 950,
                    Radius = 300,
                    MissileSpeed = 1750,
                    FixedRange = false,
                    AddHitbox = true,
                    DangerValue = 2,
                    IsDangerous = false,
                    MissileSpellName = "",
                    CollisionObjects = new[] { CollisionObjectTypes.YasuoWall },
                });

            #endregion Soraka

            #region Shen

            Spells.Add(
                new SpellData
                {
                    ChampionName = "Shen",
                    SpellName = "ShenShadowDash",
                    Slot = SpellSlot.E,
                    Type = SkillShotType.SkillshotMissileLine,
                    Delay = 0,
                    Range = 650,
                    Radius = 50,
                    MissileSpeed = 1600,
                    FixedRange = false,
                    AddHitbox = true,
                    DangerValue = 3,
                    IsDangerous = true,
                    MissileSpellName = "ShenShadowDash",
                    ExtraRange = 200,
                    CollisionObjects =
                        new[]
                        { CollisionObjectTypes.Minion, CollisionObjectTypes.Champions, CollisionObjectTypes.YasuoWall },
                });

            #endregion Shen

            #region Shyvana

            Spells.Add(
                new SpellData
                {
                    ChampionName = "Shyvana",
                    SpellName = "ShyvanaFireball",
                    Slot = SpellSlot.E,
                    Type = SkillShotType.SkillshotMissileLine,
                    Delay = 250,
                    Range = 950,
                    Radius = 60,
                    MissileSpeed = 1700,
                    FixedRange = true,
                    AddHitbox = true,
                    DangerValue = 2,
                    IsDangerous = false,
                    MissileSpellName = "ShyvanaFireballMissile",
                    CollisionObjects =
                        new[]
                        { CollisionObjectTypes.Minion, CollisionObjectTypes.Champions, CollisionObjectTypes.YasuoWall },
                });

            Spells.Add(
                new SpellData
                {
                    ChampionName = "Shyvana",
                    SpellName = "ShyvanaTransformCast",
                    Slot = SpellSlot.R,
                    Type = SkillShotType.SkillshotMissileLine,
                    Delay = 250,
                    Range = 1000,
                    Radius = 150,
                    MissileSpeed = 1500,
                    FixedRange = true,
                    AddHitbox = true,
                    DangerValue = 3,
                    IsDangerous = true,
                    MissileSpellName = "ShyvanaTransformCast",
                    ExtraRange = 200,
                });

            #endregion Shyvana

            #region Sivir

            Spells.Add(
                new SpellData
                {
                    ChampionName = "Sivir",
                    SpellName = "SivirQReturn",
                    Slot = SpellSlot.Q,
                    Type = SkillShotType.SkillshotMissileLine,
                    Delay = 0,
                    Range = 1250,
                    Radius = 100,
                    MissileSpeed = 1350,
                    FixedRange = true,
                    AddHitbox = true,
                    DangerValue = 2,
                    IsDangerous = false,
                    MissileSpellName = "SivirQMissileReturn",
                    DisableFowDetection = false,
                    MissileFollowsUnit = true,
                    CollisionObjects = new[] { CollisionObjectTypes.YasuoWall },
                });

            Spells.Add(
                new SpellData
                {
                    ChampionName = "Sivir",
                    SpellName = "SivirQ",
                    Slot = SpellSlot.Q,
                    Type = SkillShotType.SkillshotMissileLine,
                    Delay = 250,
                    Range = 1250,
                    Radius = 90,
                    MissileSpeed = 1350,
                    FixedRange = true,
                    AddHitbox = true,
                    DangerValue = 2,
                    IsDangerous = false,
                    MissileSpellName = "SivirQMissile",
                    CollisionObjects = new[] { CollisionObjectTypes.YasuoWall },
                });

            #endregion Sivir

            #region Skarner

            Spells.Add(
                new SpellData
                {
                    ChampionName = "Skarner",
                    SpellName = "SkarnerFracture",
                    Slot = SpellSlot.E,
                    Type = SkillShotType.SkillshotMissileLine,
                    Delay = 250,
                    Range = 1000,
                    Radius = 70,
                    MissileSpeed = 1500,
                    FixedRange = true,
                    AddHitbox = true,
                    DangerValue = 2,
                    IsDangerous = false,
                    MissileSpellName = "SkarnerFractureMissile",
                    CollisionObjects = new[] { CollisionObjectTypes.YasuoWall },
                });

            #endregion Skarner

            #region Sona

            Spells.Add(
                new SpellData
                {
                    ChampionName = "Sona",
                    SpellName = "SonaR",
                    Slot = SpellSlot.R,
                    Type = SkillShotType.SkillshotMissileLine,
                    Delay = 250,
                    Range = 1000,
                    Radius = 140,
                    MissileSpeed = 2400,
                    FixedRange = true,
                    AddHitbox = true,
                    DangerValue = 5,
                    IsDangerous = true,
                    MissileSpellName = "SonaR",
                    CollisionObjects = new[] { CollisionObjectTypes.YasuoWall },
                });

            #endregion Sona

            #region Swain

            Spells.Add(
                new SpellData
                {
                    ChampionName = "Swain",
                    SpellName = "SwainShadowGrasp",
                    Slot = SpellSlot.W,
                    Type = SkillShotType.SkillshotCircle,
                    Delay = 1100,
                    Range = 900,
                    Radius = 180,
                    MissileSpeed = int.MaxValue,
                    FixedRange = false,
                    AddHitbox = true,
                    DangerValue = 3,
                    IsDangerous = true,
                    MissileSpellName = "SwainShadowGrasp",
                });

            #endregion Swain

            #region Syndra

            Spells.Add(
                new SpellData
                {
                    ChampionName = "Syndra",
                    SpellName = "SyndraQ",
                    Slot = SpellSlot.Q,
                    Type = SkillShotType.SkillshotCircle,
                    Delay = 600,
                    Range = 800,
                    Radius = 150,
                    MissileSpeed = int.MaxValue,
                    FixedRange = false,
                    AddHitbox = true,
                    DangerValue = 2,
                    IsDangerous = false,
                    MissileSpellName = "SyndraQ",
                });

            Spells.Add(
                new SpellData
                {
                    ChampionName = "Syndra",
                    SpellName = "syndrawcast",
                    Slot = SpellSlot.W,
                    Type = SkillShotType.SkillshotCircle,
                    Delay = 250,
                    Range = 950,
                    Radius = 210,
                    MissileSpeed = 1450,
                    FixedRange = false,
                    AddHitbox = true,
                    DangerValue = 2,
                    IsDangerous = false,
                    MissileSpellName = "syndrawcast",
                });

            Spells.Add(
                new SpellData
                {
                    ChampionName = "Syndra",
                    SpellName = "syndrae5",
                    Slot = SpellSlot.E,
                    Type = SkillShotType.SkillshotMissileLine,
                    Delay = 300,
                    Range = 950,
                    Radius = 90,
                    MissileSpeed = 1601,
                    FixedRange = false,
                    AddHitbox = true,
                    DangerValue = 2,
                    IsDangerous = false,
                    MissileSpellName = "syndrae5",
                    DisableFowDetection = true,
                });

            Spells.Add(
                new SpellData
                {
                    ChampionName = "Syndra",
                    SpellName = "SyndraE",
                    Slot = SpellSlot.E,
                    Type = SkillShotType.SkillshotMissileLine,
                    Delay = 300,
                    Range = 950,
                    Radius = 90,
                    MissileSpeed = 1601,
                    FixedRange = false,
                    AddHitbox = true,
                    DangerValue = 2,
                    IsDangerous = false,
                    DisableFowDetection = true,
                    MissileSpellName = "SyndraE",
                });

            #endregion Syndra

            #region Talon

            Spells.Add(
                new SpellData
                {
                    ChampionName = "Talon",
                    SpellName = "TalonRake",
                    Slot = SpellSlot.W,
                    Type = SkillShotType.SkillshotMissileLine,
                    Delay = 250,
                    Range = 800,
                    Radius = 80,
                    MissileSpeed = 2300,
                    FixedRange = true,
                    AddHitbox = true,
                    DangerValue = 2,
                    IsDangerous = true,
                    MultipleNumber = 3,
                    MultipleAngle = 20 * (float) Math.PI / 180,
                    MissileSpellName = "talonrakemissileone",
                });

            Spells.Add(
                new SpellData
                {
                    ChampionName = "Talon",
                    SpellName = "TalonRakeReturn",
                    Slot = SpellSlot.W,
                    Type = SkillShotType.SkillshotMissileLine,
                    Delay = 250,
                    Range = 800,
                    Radius = 80,
                    MissileSpeed = 1850,
                    FixedRange = true,
                    AddHitbox = true,
                    DangerValue = 2,
                    IsDangerous = true,
                    MultipleNumber = 3,
                    MultipleAngle = 20 * (float) Math.PI / 180,
                    MissileSpellName = "talonrakemissiletwo",
                });

            #endregion Riven

            #region Thresh

            Spells.Add(
                new SpellData
                {
                    ChampionName = "Thresh",
                    SpellName = "ThreshQ",
                    Slot = SpellSlot.Q,
                    Type = SkillShotType.SkillshotMissileLine,
                    Delay = 500,
                    Range = 1100,
                    Radius = 70,
                    MissileSpeed = 1900,
                    FixedRange = true,
                    AddHitbox = true,
                    DangerValue = 3,
                    IsDangerous = true,
                    MissileSpellName = "ThreshQMissile",
                    CanBeRemoved = true,
                    CollisionObjects =
                        new[]
                        { CollisionObjectTypes.Minion, CollisionObjectTypes.Champions, CollisionObjectTypes.YasuoWall },
                });

            Spells.Add(
                new SpellData
                {
                    ChampionName = "Thresh",
                    SpellName = "ThreshEFlay",
                    Slot = SpellSlot.E,
                    Type = SkillShotType.SkillshotMissileLine,
                    Delay = 125,
                    Range = 1075,
                    Radius = 110,
                    MissileSpeed = 2000,
                    FixedRange = true,
                    AddHitbox = true,
                    DangerValue = 3,
                    IsDangerous = true,
                    Centered = true,
                    MissileSpellName = "ThreshEMissile1",
                });

            #endregion Thresh

            #region Tristana

            Spells.Add(
                new SpellData
                {
                    ChampionName = "Tristana",
                    SpellName = "RocketJump",
                    Slot = SpellSlot.W,
                    Type = SkillShotType.SkillshotCircle,
                    Delay = 500,
                    Range = 900,
                    Radius = 270,
                    MissileSpeed = 1500,
                    FixedRange = false,
                    AddHitbox = true,
                    DangerValue = 2,
                    IsDangerous = false,
                    MissileSpellName = "RocketJump",
                });

            #endregion Tristana

            #region Tryndamere

            Spells.Add(
                new SpellData
                {
                    ChampionName = "Tryndamere",
                    SpellName = "slashCast",
                    Slot = SpellSlot.E,
                    Type = SkillShotType.SkillshotMissileLine,
                    Delay = 0,
                    Range = 660,
                    Radius = 93,
                    MissileSpeed = 1300,
                    FixedRange = false,
                    AddHitbox = true,
                    DangerValue = 2,
                    IsDangerous = false,
                    MissileSpellName = "slashCast",
                });

            #endregion Tryndamere

            #region TwistedFate

            Spells.Add(
                new SpellData
                {
                    ChampionName = "TwistedFate",
                    SpellName = "WildCards",
                    Slot = SpellSlot.Q,
                    Type = SkillShotType.SkillshotMissileLine,
                    Delay = 250,
                    Range = 1450,
                    Radius = 40,
                    MissileSpeed = 1000,
                    FixedRange = true,
                    AddHitbox = true,
                    DangerValue = 2,
                    IsDangerous = false,
                    MissileSpellName = "SealFateMissile",
                    MultipleNumber = 3,
                    MultipleAngle = 28 * (float) Math.PI / 180,
                    CollisionObjects = new[] { CollisionObjectTypes.YasuoWall },
                });

            #endregion TwistedFate

            #region Twitch

            Spells.Add(
                new SpellData
                {
                    ChampionName = "Twitch",
                    SpellName = "TwitchVenomCask",
                    Slot = SpellSlot.W,
                    Type = SkillShotType.SkillshotCircle,
                    Delay = 250,
                    Range = 900,
                    Radius = 275,
                    MissileSpeed = 1400,
                    FixedRange = false,
                    AddHitbox = true,
                    DangerValue = 2,
                    IsDangerous = false,
                    MissileSpellName = "TwitchVenomCaskMissile",
                    CollisionObjects = new[] { CollisionObjectTypes.YasuoWall },
                });

            #endregion Twitch

            #region Urgot

            Spells.Add(
                new SpellData
                {
                    ChampionName = "Urgot",
                    SpellName = "UrgotHeatseekingLineMissile",
                    Slot = SpellSlot.Q,
                    Type = SkillShotType.SkillshotMissileLine,
                    Delay = 125,
                    Range = 1000,
                    Radius = 60,
                    MissileSpeed = 1600,
                    FixedRange = true,
                    AddHitbox = true,
                    DangerValue = 2,
                    IsDangerous = false,
                    MissileSpellName = "UrgotHeatseekingLineMissile",
                    CanBeRemoved = true,
                });

            Spells.Add(
                new SpellData
                {
                    ChampionName = "Urgot",
                    SpellName = "UrgotPlasmaGrenade",
                    Slot = SpellSlot.E,
                    Type = SkillShotType.SkillshotCircle,
                    Delay = 250,
                    Range = 1100,
                    Radius = 210,
                    MissileSpeed = 1500,
                    FixedRange = false,
                    AddHitbox = true,
                    DangerValue = 2,
                    IsDangerous = false,
                    MissileSpellName = "UrgotPlasmaGrenadeBoom",
                });

            #endregion Urgot

            #region Varus

            Spells.Add(
                new SpellData
                {
                    ChampionName = "Varus",
                    SpellName = "VarusQMissilee",
                    Slot = SpellSlot.Q,
                    Type = SkillShotType.SkillshotMissileLine,
                    Delay = 250,
                    Range = 1800,
                    Radius = 70,
                    MissileSpeed = 1900,
                    FixedRange = true,
                    AddHitbox = true,
                    DangerValue = 2,
                    IsDangerous = false,
                    MissileSpellName = "VarusQMissile",
                    CollisionObjects = new[] { CollisionObjectTypes.YasuoWall },
                });

            Spells.Add(
                new SpellData
                {
                    ChampionName = "Varus",
                    SpellName = "VarusE",
                    Slot = SpellSlot.E,
                    Type = SkillShotType.SkillshotCircle,
                    Delay = 1000,
                    Range = 925,
                    Radius = 235,
                    MissileSpeed = 1500,
                    FixedRange = false,
                    AddHitbox = true,
                    DangerValue = 2,
                    IsDangerous = false,
                    MissileSpellName = "VarusE",
                });

            Spells.Add(
                new SpellData
                {
                    ChampionName = "Varus",
                    SpellName = "VarusR",
                    Slot = SpellSlot.R,
                    Type = SkillShotType.SkillshotMissileLine,
                    Delay = 250,
                    Range = 1200,
                    Radius = 120,
                    MissileSpeed = 1950,
                    FixedRange = true,
                    AddHitbox = true,
                    DangerValue = 3,
                    IsDangerous = true,
                    MissileSpellName = "VarusRMissile",
                    CanBeRemoved = true,
                    CollisionObjects = new[] { CollisionObjectTypes.Champions, CollisionObjectTypes.YasuoWall },
                });

            #endregion Varus

            #region Veigar

            Spells.Add(
                new SpellData
                {
                    ChampionName = "Veigar",
                    SpellName = "VeigarBalefulStrike",
                    Slot = SpellSlot.Q,
                    Type = SkillShotType.SkillshotMissileLine,
                    Delay = 250,
                    Range = 850,
                    Radius = 70,
                    MissileSpeed = 1750,
                    FixedRange = true,
                    AddHitbox = true,
                    DangerValue = 2,
                    IsDangerous = false,
                    MissileSpellName = "VeigarBalefulStrikeMis",
                    CollisionObjects = new[] { CollisionObjectTypes.YasuoWall },
                });

            Spells.Add(
                new SpellData
                {
                    ChampionName = "Veigar",
                    SpellName = "VeigarDarkMatter",
                    Slot = SpellSlot.W,
                    Type = SkillShotType.SkillshotCircle,
                    Delay = 1350,
                    Range = 900,
                    Radius = 225,
                    MissileSpeed = int.MaxValue,
                    FixedRange = false,
                    AddHitbox = true,
                    DangerValue = 2,
                    IsDangerous = false,
                    MissileSpellName = "",
                });

            Spells.Add(
                new SpellData
                {
                    ChampionName = "Veigar",
                    SpellName = "VeigarEventHorizon",
                    Slot = SpellSlot.E,
                    Type = SkillShotType.SkillshotRing,
                    Delay = 750,
                    Range = 700,
                    Radius = 80,
                    MissileSpeed = int.MaxValue,
                    FixedRange = false,
                    AddHitbox = false,
                    DangerValue = 3,
                    IsDangerous = true,
                    DontAddExtraDuration = true,
                    RingRadius = 350,
                    ExtraDuration = 3300,
                    DontCross = true,
                    MissileSpellName = "",
                });

            #endregion Veigar

            #region Velkoz

            Spells.Add(
                new SpellData
                {
                    ChampionName = "Velkoz",
                    SpellName = "VelkozQ",
                    Slot = SpellSlot.Q,
                    Type = SkillShotType.SkillshotMissileLine,
                    Delay = 250,
                    Range = 1100,
                    Radius = 50,
                    MissileSpeed = 1300,
                    FixedRange = true,
                    AddHitbox = true,
                    DangerValue = 2,
                    IsDangerous = false,
                    MissileSpellName = "VelkozQMissile",
                    CanBeRemoved = true,
                    CollisionObjects =
                        new[]
                        { CollisionObjectTypes.Minion, CollisionObjectTypes.Champions, CollisionObjectTypes.YasuoWall },
                });

            Spells.Add(
                new SpellData
                {
                    ChampionName = "Velkoz",
                    SpellName = "VelkozQSplit",
                    Slot = SpellSlot.Q,
                    Type = SkillShotType.SkillshotMissileLine,
                    Delay = 250,
                    Range = 900,
                    Radius = 55,
                    MissileSpeed = 2100,
                    FixedRange = true,
                    AddHitbox = true,
                    DangerValue = 2,
                    IsDangerous = false,
                    MissileSpellName = "VelkozQMissileSplit",
                    CanBeRemoved = true,
                    CollisionObjects =
                        new[]
                        { CollisionObjectTypes.Minion, CollisionObjectTypes.Champions, CollisionObjectTypes.YasuoWall },
                });

            Spells.Add(
                new SpellData
                {
                    ChampionName = "Velkoz",
                    SpellName = "VelkozW",
                    Slot = SpellSlot.W,
                    Type = SkillShotType.SkillshotMissileLine,
                    Delay = 250,
                    Range = 1200,
                    Radius = 88,
                    MissileSpeed = 1700,
                    FixedRange = true,
                    AddHitbox = true,
                    DangerValue = 2,
                    IsDangerous = false,
                    MissileSpellName = "VelkozWMissile",
                });

            Spells.Add(
                new SpellData
                {
                    ChampionName = "Velkoz",
                    SpellName = "VelkozE",
                    Slot = SpellSlot.E,
                    Type = SkillShotType.SkillshotCircle,
                    Delay = 500,
                    Range = 800,
                    Radius = 225,
                    MissileSpeed = 1500,
                    FixedRange = false,
                    AddHitbox = false,
                    DangerValue = 2,
                    IsDangerous = false,
                    MissileSpellName = "VelkozEMissile",
                });

            #endregion Velkoz

            #region Vi

            Spells.Add(
                new SpellData
                {
                    ChampionName = "Vi",
                    SpellName = "Vi-q",
                    Slot = SpellSlot.Q,
                    Type = SkillShotType.SkillshotMissileLine,
                    Delay = 250,
                    Range = 1000,
                    Radius = 90,
                    MissileSpeed = 1500,
                    FixedRange = true,
                    AddHitbox = true,
                    DangerValue = 3,
                    IsDangerous = true,
                    MissileSpellName = "ViQMissile",
                });

            #endregion Vi

            #region Viktor

            Spells.Add(
                new SpellData
                {
                    ChampionName = "Viktor",
                    SpellName = "Laser",
                    Slot = SpellSlot.E,
                    Type = SkillShotType.SkillshotMissileLine,
                    Delay = 250,
                    Range = 1500,
                    Radius = 80,
                    MissileSpeed = 780,
                    FixedRange = false,
                    AddHitbox = true,
                    DangerValue = 2,
                    IsDangerous = false,
                    MissileSpellName = "ViktorDeathRayFixMissile",
                    CollisionObjects = new[] { CollisionObjectTypes.YasuoWall },
                });

            #endregion Viktor

            #region Xerath

            Spells.Add(
                new SpellData
                {
                    ChampionName = "Xerath",
                    SpellName = "xeratharcanopulse2",
                    Slot = SpellSlot.Q,
                    Type = SkillShotType.SkillshotLine,
                    Delay = 600,
                    Range = 1600,
                    Radius = 100,
                    MissileSpeed = int.MaxValue,
                    FixedRange = true,
                    AddHitbox = true,
                    DangerValue = 2,
                    IsDangerous = false,
                    MissileSpellName = "xeratharcanopulse2",
                });

            Spells.Add(
                new SpellData
                {
                    ChampionName = "Xerath",
                    SpellName = "XerathArcaneBarrage2",
                    Slot = SpellSlot.W,
                    Type = SkillShotType.SkillshotCircle,
                    Delay = 700,
                    Range = 1000,
                    Radius = 200,
                    MissileSpeed = int.MaxValue,
                    FixedRange = false,
                    AddHitbox = true,
                    DangerValue = 2,
                    IsDangerous = false,
                    MissileSpellName = "XerathArcaneBarrage2",
                });

            Spells.Add(
                new SpellData
                {
                    ChampionName = "Xerath",
                    SpellName = "XerathMageSpear",
                    Slot = SpellSlot.E,
                    Type = SkillShotType.SkillshotMissileLine,
                    Delay = 200,
                    Range = 1150,
                    Radius = 60,
                    MissileSpeed = 1400,
                    FixedRange = true,
                    AddHitbox = true,
                    DangerValue = 2,
                    IsDangerous = true,
                    MissileSpellName = "XerathMageSpearMissile",
                    CanBeRemoved = true,
                    CollisionObjects =
                        new[]
                        { CollisionObjectTypes.Minion, CollisionObjectTypes.Champions, CollisionObjectTypes.YasuoWall },
                });

            Spells.Add(
                new SpellData
                {
                    ChampionName = "Xerath",
                    SpellName = "xerathrmissilewrapper",
                    Slot = SpellSlot.R,
                    Type = SkillShotType.SkillshotCircle,
                    Delay = 700,
                    Range = 5600,
                    Radius = 120,
                    MissileSpeed = int.MaxValue,
                    FixedRange = false,
                    AddHitbox = true,
                    DangerValue = 3,
                    IsDangerous = true,
                    MissileSpellName = "xerathrmissilewrapper",
                });

            #endregion Xerath

            #region Yasuo 

            Spells.Add(
                new SpellData
                {
                    ChampionName = "Yasuo",
                    SpellName = "yasuoq2",
                    Slot = SpellSlot.Q,
                    Type = SkillShotType.SkillshotLine,
                    Delay = 400,
                    Range = 550,
                    Radius = 20,
                    MissileSpeed = int.MaxValue,
                    FixedRange = true,
                    AddHitbox = true,
                    DangerValue = 2,
                    IsDangerous = true,
                    MissileSpellName = "yasuoq2",
                    Invert = true,
                });

            Spells.Add(
                new SpellData
                {
                    ChampionName = "Yasuo",
                    SpellName = "yasuoq3w",
                    Slot = SpellSlot.Q,
                    Type = SkillShotType.SkillshotMissileLine,
                    Delay = 500,
                    Range = 1150,
                    Radius = 90,
                    MissileSpeed = 1500,
                    FixedRange = true,
                    AddHitbox = true,
                    DangerValue = 3,
                    IsDangerous = true,
                    MissileSpellName = "yasuoq3w",
                    CollisionObjects = new[] { CollisionObjectTypes.YasuoWall }
                });

            Spells.Add(
                new SpellData
                {
                    ChampionName = "Yasuo",
                    SpellName = "yasuoq",
                    Slot = SpellSlot.Q,
                    Type = SkillShotType.SkillshotLine,
                    Delay = 400,
                    Range = 550,
                    Radius = 20,
                    MissileSpeed = int.MaxValue,
                    FixedRange = true,
                    AddHitbox = true,
                    DangerValue = 2,
                    IsDangerous = true,
                    MissileSpellName = "yasuoq",
                    Invert = true,
                });

            #endregion Yasuo

            #region Zac

            Spells.Add(
                new SpellData
                {
                    ChampionName = "Zac",
                    SpellName = "ZacQ",
                    Slot = SpellSlot.Q,
                    Type = SkillShotType.SkillshotLine,
                    Delay = 500,
                    Range = 550,
                    Radius = 120,
                    MissileSpeed = int.MaxValue,
                    FixedRange = true,
                    AddHitbox = true,
                    DangerValue = 2,
                    IsDangerous = false,
                    MissileSpellName = "ZacQ",
                });

            #endregion Zac

            #region Zed

            Spells.Add(
                new SpellData
                {
                    ChampionName = "Zed",
                    SpellName = "ZedShuriken",
                    Slot = SpellSlot.Q,
                    Type = SkillShotType.SkillshotMissileLine,
                    Delay = 250,
                    Range = 925,
                    Radius = 50,
                    MissileSpeed = 1700,
                    FixedRange = true,
                    AddHitbox = true,
                    DangerValue = 2,
                    IsDangerous = false,
                    MissileSpellName = "zedshurikenmisone",
                    FromObjects = new[] { "Zed_Clone_idle.troy", "Zed_Clone_Idle.troy" },
                    ExtraMissileNames = new[] { "zedshurikenmistwo", "zedshurikenmisthree" },
                    CollisionObjects = new[] { CollisionObjectTypes.YasuoWall },
                });

            #endregion Zed

            #region Ziggs

            Spells.Add(
                new SpellData
                {
                    ChampionName = "Ziggs",
                    SpellName = "ZiggsQ",
                    Slot = SpellSlot.Q,
                    Type = SkillShotType.SkillshotCircle,
                    Delay = 250,
                    Range = 850,
                    Radius = 140,
                    MissileSpeed = 1700,
                    FixedRange = false,
                    AddHitbox = true,
                    DangerValue = 2,
                    IsDangerous = false,
                    MissileSpellName = "ZiggsQSpell",
                    CanBeRemoved = false,
                    DisableFowDetection = true,
                });

            Spells.Add(
                new SpellData
                {
                    ChampionName = "Ziggs",
                    SpellName = "ZiggsQBounce1",
                    Slot = SpellSlot.Q,
                    Type = SkillShotType.SkillshotCircle,
                    Delay = 250,
                    Range = 850,
                    Radius = 140,
                    MissileSpeed = 1700,
                    FixedRange = false,
                    AddHitbox = true,
                    DangerValue = 2,
                    IsDangerous = false,
                    MissileSpellName = "ZiggsQSpell2",
                    ExtraMissileNames = new[] { "ZiggsQSpell2" },
                    CanBeRemoved = false,
                    DisableFowDetection = true,
                });

            Spells.Add(
                new SpellData
                {
                    ChampionName = "Ziggs",
                    SpellName = "ZiggsQBounce2",
                    Slot = SpellSlot.Q,
                    Type = SkillShotType.SkillshotCircle,
                    Delay = 250,
                    Range = 850,
                    Radius = 160,
                    MissileSpeed = 1700,
                    FixedRange = false,
                    AddHitbox = true,
                    DangerValue = 2,
                    IsDangerous = false,
                    MissileSpellName = "ZiggsQSpell3",
                    ExtraMissileNames = new[] { "ZiggsQSpell3" },
                    CanBeRemoved = false,
                    DisableFowDetection = true,
                });

            Spells.Add(
                new SpellData
                {
                    ChampionName = "Ziggs",
                    SpellName = "ZiggsW",
                    Slot = SpellSlot.W,
                    Type = SkillShotType.SkillshotCircle,
                    Delay = 250,
                    Range = 1000,
                    Radius = 275,
                    MissileSpeed = 1750,
                    FixedRange = false,
                    AddHitbox = true,
                    DangerValue = 2,
                    IsDangerous = false,
                    MissileSpellName = "ZiggsW",
                    DisableFowDetection = true,
                    CollisionObjects = new[] { CollisionObjectTypes.YasuoWall },
                });

            Spells.Add(
                new SpellData
                {
                    ChampionName = "Ziggs",
                    SpellName = "ZiggsE",
                    Slot = SpellSlot.E,
                    Type = SkillShotType.SkillshotCircle,
                    Delay = 500,
                    Range = 900,
                    Radius = 235,
                    MissileSpeed = 1750,
                    FixedRange = false,
                    AddHitbox = true,
                    DangerValue = 2,
                    IsDangerous = false,
                    MissileSpellName = "ZiggsE",
                    DisableFowDetection = true,
                });

            Spells.Add(
                new SpellData
                {
                    ChampionName = "Ziggs",
                    SpellName = "ZiggsR",
                    Slot = SpellSlot.R,
                    Type = SkillShotType.SkillshotCircle,
                    Delay = 0,
                    Range = 5300,
                    Radius = 500,
                    MissileSpeed = int.MaxValue,
                    FixedRange = false,
                    AddHitbox = true,
                    DangerValue = 2,
                    IsDangerous = false,
                    MissileSpellName = "ZiggsR",
                    DisableFowDetection = true,
                });

            #endregion Ziggs

            #region Zilean

            Spells.Add(
                new SpellData
                {
                    ChampionName = "Zilean",
                    SpellName = "ZileanQ",
                    Slot = SpellSlot.Q,
                    Type = SkillShotType.SkillshotCircle,
                    Delay = 300,
                    Range = 900,
                    Radius = 210,
                    MissileSpeed = 2000,
                    FixedRange = false,
                    AddHitbox = true,
                    DangerValue = 2,
                    IsDangerous = false,
                    MissileSpellName = "ZileanQMissile",
                    CollisionObjects = new[] { CollisionObjectTypes.YasuoWall }
                });

            #endregion Zilean

            #region Zyra

            Spells.Add(
                new SpellData
                {
                    ChampionName = "Zyra",
                    SpellName = "ZyraQFissure",
                    Slot = SpellSlot.Q,
                    Type = SkillShotType.SkillshotCircle,
                    Delay = 600,
                    Range = 800,
                    Radius = 220,
                    MissileSpeed = int.MaxValue,
                    FixedRange = false,
                    AddHitbox = true,
                    DangerValue = 2,
                    IsDangerous = false,
                    MissileSpellName = "ZyraQFissure",
                });

            Spells.Add(
                new SpellData
                {
                    ChampionName = "Zyra",
                    SpellName = "ZyraGraspingRoots",
                    Slot = SpellSlot.E,
                    Type = SkillShotType.SkillshotMissileLine,
                    Delay = 250,
                    Range = 1150,
                    Radius = 70,
                    MissileSpeed = 1150,
                    FixedRange = true,
                    AddHitbox = true,
                    DangerValue = 3,
                    IsDangerous = true,
                    MissileSpellName = "ZyraGraspingRoots",
                    CollisionObjects = new[] { CollisionObjectTypes.YasuoWall },
                });

            Spells.Add(
                new SpellData
                {
                    ChampionName = "Zyra",
                    SpellName = "zyrapassivedeathmanager",
                    Slot = SpellSlot.E,
                    Type = SkillShotType.SkillshotMissileLine,
                    Delay = 700,
                    Range = 1474,
                    Radius = 70,
                    MissileSpeed = 2000,
                    FixedRange = true,
                    AddHitbox = true,
                    DangerValue = 3,
                    IsDangerous = true,
                    MissileSpellName = "zyrapassivedeathmanager",
                    CollisionObjects = new[] { CollisionObjectTypes.YasuoWall },
                });

            #endregion Zyra

            //Game.PrintChat("Added " + Spells.Count + " spells.");
        }

        public static SpellData GetByName(string spellName)
        {
            spellName = spellName.ToLower();
            foreach (var spellData in Spells)
            {
                if (spellData.SpellName.ToLower() == spellName || spellData.ExtraSpellNames.Contains(spellName))
                {
                    return spellData;
                }
            }

            return null;
        }

        public static SpellData GetByMissileName(string missileSpellName)
        {
            missileSpellName = missileSpellName.ToLower();
            foreach (var spellData in Spells)
            {
                if (spellData.MissileSpellName != null && spellData.MissileSpellName.ToLower() == missileSpellName ||
                    spellData.ExtraMissileNames.Contains(missileSpellName))
                {
                    return spellData;
                }
            }

            return null;
        }

        public static SpellData GetBySpeed(string ChampionName, int speed, int id = -1)
        {
            foreach (var spellData in Spells)
            {
                if (spellData.ChampionName == ChampionName && spellData.MissileSpeed == speed &&
                    (spellData.Id == -1 || id == spellData.Id))
                {
                    return spellData;
                }
            }

            return null;
        }
    }
}
