#region

using System;
using System.Collections.Generic;
using System.Linq.Expressions;
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
            #region Aatrox
            Spells.Add(new SpellData
            {
                BaseSkinName = "Aatrox",
                SpellName = "AatroxQ",
                Slot = SpellSlot.Q,
                Type = SkillShotType.SkillshotCircle,
                Delay = 600,
                Range = 650,
                Radius = 150,
                MissileSpeed = 2000,
                FixedRange = false,
                AddHitbox = true,
                DangerValue = 3,
                IsDangerous = true,
                MissileSpellName = "AatroxQ",

            });

            #endregion Aatrox
            #region Ahri
            Spells.Add(new SpellData
            {
                BaseSkinName = "Ahri",
                SpellName = "AhriOrbReturn",
                Slot = SpellSlot.Q,
                Type = SkillShotType.SkillshotMissileLine,
                Delay = 250,
                Range = 1000,
                Radius = 100,
                MissileSpeed = 1600,
                FixedRange = true,
                AddHitbox = true,
                DangerValue = 2,
                IsDangerous = false,
                MissileSpellName = "AhriOrbReturn",
                MissileFollowsUnit = true,

            });

            Spells.Add(new SpellData
            {
                BaseSkinName = "Ahri",
                SpellName = "AhriOrbofDeception",
                Slot = SpellSlot.Q,
                Type = SkillShotType.SkillshotMissileLine,
                Delay = 250,
                Range = 1000,
                Radius = 100,
                MissileSpeed = 1600,
                FixedRange = true,
                AddHitbox = true,
                DangerValue = 2,
                IsDangerous = false,
                MissileSpellName = "AhriOrbofDeception",

            });

            Spells.Add(new SpellData
            {
                BaseSkinName = "Ahri",
                SpellName = "AhriSeduce",
                Slot = SpellSlot.E,
                Type = SkillShotType.SkillshotMissileLine,
                Delay = 0,
                Range = 1000,
                Radius = 60,
                MissileSpeed = 1500,
                FixedRange = true,
                AddHitbox = true,
                DangerValue = 3,
                IsDangerous = true,
                MissileSpellName = "AhriSeduce",
                CanBeRemoved = true,

            });

            #endregion Ahri
            #region Amumu
            Spells.Add(new SpellData
            {
                BaseSkinName = "Amumu",
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
                MissileSpellName = "BandageToss",
                CanBeRemoved = true,

            });

            Spells.Add(new SpellData
            {
                BaseSkinName = "Amumu",
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
                MissileSpellName = "CurseoftheSadMummy",

            });

            #endregion Amumu
            #region Anivia
            Spells.Add(new SpellData
            {
                BaseSkinName = "Anivia",
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
                MissileSpellName = "FlashFrost",
                CanBeRemoved = true,

            });

            #endregion Anivia
            #region Annie
            Spells.Add(new SpellData
            {
                BaseSkinName = "Annie",
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
                MissileSpellName = "Incinerate",

            });

            Spells.Add(new SpellData
            {
                BaseSkinName = "Annie",
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
                MissileSpellName = "InfernalGuardian",

            });

            #endregion Annie
            #region Ashe
            Spells.Add(new SpellData
            {
                BaseSkinName = "Ashe",
                SpellName = "EnchantedCrystalArrow",
                Slot = SpellSlot.R,
                Type = SkillShotType.SkillshotMissileLine,
                Delay = 250,
                Range = 25000,
                Radius = 130,
                MissileSpeed = 1600,
                FixedRange = true,
                AddHitbox = true,
                DangerValue = 5,
                IsDangerous = true,
                MissileSpellName = "EnchantedCrystalArrow",
                CanBeRemoved = true,

            });

            #endregion Ashe
            #region Blitzcrank
            Spells.Add(new SpellData
            {
                BaseSkinName = "Blitzcrank",
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
                MissileSpellName = "RocketGrab",
                CanBeRemoved = true,

            });

            Spells.Add(new SpellData
            {
                BaseSkinName = "Blitzcrank",
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
                MissileSpellName = "StaticField",

            });

            #endregion Blitzcrank
            #region Brand
            Spells.Add(new SpellData
            {
                BaseSkinName = "Brand",
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
                MissileSpellName = "BrandBlaze",
                CanBeRemoved = true,

            });

            Spells.Add(new SpellData
            {
                BaseSkinName = "Brand",
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
                MissileSpellName = "BrandFissure",

            });

            #endregion Brand
            #region Braum
            Spells.Add(new SpellData
            {
                BaseSkinName = "Braum",
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
                MissileSpellName = "BraumQ",
                CanBeRemoved = true,

            });

            Spells.Add(new SpellData
            {
                BaseSkinName = "Braum",
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
                MissileSpellName = "BraumRWrapper",

            });

            #endregion Braum
            #region Caitlyn
            Spells.Add(new SpellData
            {
                BaseSkinName = "Caitlyn",
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

            });

            Spells.Add(new SpellData
            {
                BaseSkinName = "Caitlyn",
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
                MissileSpellName = "CaitlynEntrapment",
                CanBeRemoved = true,

            });

            #endregion Caitlyn
            #region Cassiopeia
            Spells.Add(new SpellData
            {
                BaseSkinName = "Cassiopeia",
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

            Spells.Add(new SpellData
            {
                BaseSkinName = "Cassiopeia",
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
            Spells.Add(new SpellData
            {
                BaseSkinName = "Chogath",
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
            Spells.Add(new SpellData
            {
                BaseSkinName = "Corki",
                SpellName = "PhosphorusBomb",
                Slot = SpellSlot.Q,
                Type = SkillShotType.SkillshotCircle,
                Delay = 500,
                Range = 825,
                Radius = 250,
                MissileSpeed = 1125,
                FixedRange = false,
                AddHitbox = true,
                DangerValue = 2,
                IsDangerous = false,
                MissileSpellName = "PhosphorusBomb",

            });

            Spells.Add(new SpellData
            {
                BaseSkinName = "Corki",
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
                MissileSpellName = "MissileBarrage",
                CanBeRemoved = true,

            });

            #endregion Corki
            #region Darius
            Spells.Add(new SpellData
            {
                BaseSkinName = "Darius",
                SpellName = "DariusAxeGrabCone",
                Slot = SpellSlot.E,
                Type = SkillShotType.SkillshotCone,
                Delay = 600,
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
            Spells.Add(new SpellData
            {
                BaseSkinName = "DrMundo",
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
                MissileSpellName = "InfectedCleaverMissileCast",
                CanBeRemoved = true,

            });

            #endregion DrMundo
            #region Draven
            Spells.Add(new SpellData
            {
                BaseSkinName = "Draven",
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
                MissileSpellName = "DravenDoubleShot",
                CanBeRemoved = true,

            });

            Spells.Add(new SpellData
            {
                BaseSkinName = "Draven",
                SpellName = "DravenRCast",
                Slot = SpellSlot.R,
                Type = SkillShotType.SkillshotMissileLine,
                Delay = 1000,
                Range = 20000,
                Radius = 160,
                MissileSpeed = 2000,
                FixedRange = true,
                AddHitbox = true,
                DangerValue = 5,
                IsDangerous = true,
                MissileSpellName = "DravenRCast",

            });

            #endregion Draven
            #region Elise
            Spells.Add(new SpellData
            {
                BaseSkinName = "Elise",
                SpellName = "EliseHumanE",
                Slot = SpellSlot.E,
                Type = SkillShotType.SkillshotMissileLine,
                Delay = 250,
                Range = 1100,
                Radius = 70,
                MissileSpeed = 1450,
                FixedRange = true,
                AddHitbox = true,
                DangerValue = 4,
                IsDangerous = true,
                MissileSpellName = "EliseHumanE",
                CanBeRemoved = true,

            });

            #endregion Elise
            #region Evelynn
            Spells.Add(new SpellData
            {
                BaseSkinName = "Evelynn",
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
            Spells.Add(new SpellData
            {
                BaseSkinName = "Ezreal",
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
                MissileSpellName = "EzrealMysticShot",
                CanBeRemoved = true,
                Id = 229,

            });

            Spells.Add(new SpellData
            {
                BaseSkinName = "Ezreal",
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
                MissileSpellName = "EzrealEssenceFlux",

            });

            Spells.Add(new SpellData
            {
                BaseSkinName = "Ezreal",
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
                Id = 245,

            });

            #endregion Ezreal
            #region Fizz
            Spells.Add(new SpellData
            {
                BaseSkinName = "Fizz",
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
                MissileSpellName = "FizzMarinerDoom",
                CanBeRemoved = true,

            });

            #endregion Fizz
            #region Galio
            Spells.Add(new SpellData
            {
                BaseSkinName = "Galio",
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

            Spells.Add(new SpellData
            {
                BaseSkinName = "Galio",
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
                MissileSpellName = "GalioIdolOfDurand",

            });

            #endregion Galio
            #region Gragas
            Spells.Add(new SpellData
            {
                BaseSkinName = "Gragas",
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
                MissileSpellName = "GragasQ",
                ExtraDuration = 4500,
                ToggleParticleName = "Gragas_",
                DonCross = true,

            });

            Spells.Add(new SpellData
            {
                BaseSkinName = "Gragas",
                SpellName = "GragasE",
                Slot = SpellSlot.E,
                Type = SkillShotType.SkillshotMissileLine,
                Delay = 0,
                Range = 950,
                Radius = 200,
                MissileSpeed = 1200,
                FixedRange = false,
                AddHitbox = true,
                DangerValue = 2,
                IsDangerous = false,
                MissileSpellName = "GragasE",
                CanBeRemoved = true,
                ExtraRange = 300,

            });

            Spells.Add(new SpellData
            {
                BaseSkinName = "Gragas",
                SpellName = "GragasR",
                Slot = SpellSlot.R,
                Type = SkillShotType.SkillshotCircle,
                Delay = 700,
                Range = 1050,
                Radius = 375,
                MissileSpeed = int.MaxValue,
                FixedRange = false,
                AddHitbox = true,
                DangerValue = 5,
                IsDangerous = true,
                MissileSpellName = "GragasR",

            });

            #endregion Gragas
            #region Graves
            Spells.Add(new SpellData
            {
                BaseSkinName = "Graves",
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
                MissileSpellName = "GravesClusterShot",

            });

            Spells.Add(new SpellData
            {
                BaseSkinName = "Graves",
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
                MissileSpellName = "GravesChargeShot",

            });

            #endregion Graves
            #region Heimerdinger
            Spells.Add(new SpellData
            {
                BaseSkinName = "Heimerdinger",
                SpellName = "Heimerdingerw",
                Slot = SpellSlot.W,
                Type = SkillShotType.SkillshotMissileLine,
                Delay = 250,
                Range = 1325,
                Radius = 1,
                MissileSpeed = 750,
                FixedRange = false,
                AddHitbox = true,
                DangerValue = 2,
                IsDangerous = false,
                MissileSpellName = "Heimerdingerw",

            });

            Spells.Add(new SpellData
            {
                BaseSkinName = "Heimerdinger",
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
                MissileSpellName = "HeimerdingerE",

            });

            #endregion Heimerdinger
            #region Irelia
            Spells.Add(new SpellData
            {
                BaseSkinName = "Irelia",
                SpellName = "IreliaTranscendentBlades",
                Slot = SpellSlot.R,
                Type = SkillShotType.SkillshotMissileLine,
                Delay = 0,
                Range = 1200,
                Radius = 0,
                MissileSpeed = 1600,
                FixedRange = true,
                AddHitbox = true,
                DangerValue = 2,
                IsDangerous = false,
                MissileSpellName = "IreliaTranscendentBlades",

            });

            #endregion Irelia
            #region Janna
            Spells.Add(new SpellData
            {
                BaseSkinName = "Janna",
                SpellName = "JannaQ",
                Slot = SpellSlot.Q,
                Type = SkillShotType.SkillshotMissileLine,
                Delay = 250,
                Range = 1300,
                Radius = 200,
                MissileSpeed = 900,
                FixedRange = true,
                AddHitbox = true,
                DangerValue = 2,
                IsDangerous = false,
                MissileSpellName = "JannaQ",

            });

            #endregion Janna
            #region JarvanIV
            Spells.Add(new SpellData
            {
                BaseSkinName = "JarvanIV",
                SpellName = "JarvanIVDragonStrike",
                Slot = SpellSlot.Q,
                Type = SkillShotType.SkillshotMissileLine,
                Delay = 250,
                Range = 1000,
                Radius = 70,
                MissileSpeed = 1450,
                FixedRange = true,
                AddHitbox = true,
                DangerValue = 3,
                IsDangerous = true,
                MissileSpellName = "JarvanIVDragonStrike",

            });

            Spells.Add(new SpellData
            {
                BaseSkinName = "JarvanIV",
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
            Spells.Add(new SpellData
            {
                BaseSkinName = "Jayce",
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
                MissileSpellName = "jayceshockblast",

            });

            Spells.Add(new SpellData
            {
                BaseSkinName = "Jayce",
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
                MissileSpellName = "JayceQAccel",
                CanBeRemoved = true,

            });

            #endregion Jayce
            #region Jinx
            Spells.Add(new SpellData
            {
                BaseSkinName = "Jinx",
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
                MissileSpellName = "JinxW",
                CanBeRemoved = true,

            });

            Spells.Add(new SpellData
            {
                BaseSkinName = "Jinx",
                SpellName = "JinxRWrapper",
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
                MissileSpellName = "JinxRWrapper",
                CanBeRemoved = true,

            });

            #endregion Jinx
            #region Karma
            Spells.Add(new SpellData
            {
                BaseSkinName = "Karma",
                SpellName = "KarmaQ",
                Slot = SpellSlot.Q,
                Type = SkillShotType.SkillshotMissileLine,
                Delay = 250,
                Range = 950,
                Radius = 60,
                MissileSpeed = 1700,
                FixedRange = true,
                AddHitbox = true,
                DangerValue = 2,
                IsDangerous = false,
                MissileSpellName = "KarmaQ",
                CanBeRemoved = true,

            });

            #endregion Karma
            #region Karthus
            Spells.Add(new SpellData
            {
                BaseSkinName = "Karthus",
                SpellName = "KarthusLayWasteA2",
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
                MissileSpellName = "KarthusLayWasteA2",

            });

            Spells.Add(new SpellData
            {
                BaseSkinName = "Karthus",
                SpellName = "KarthusLayWasteA1",
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
                MissileSpellName = "KarthusLayWasteA1",

            });

            Spells.Add(new SpellData
            {
                BaseSkinName = "Karthus",
                SpellName = "KarthusLayWasteA3",
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
                MissileSpellName = "KarthusLayWasteA3",

            });

            #endregion Karthus
            #region Kassadin
            Spells.Add(new SpellData
            {
                BaseSkinName = "Kassadin",
                SpellName = "RiftWalk",
                Slot = SpellSlot.R,
                Type = SkillShotType.SkillshotCircle,
                Delay = 250,
                Range = 700,
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
            Spells.Add(new SpellData
            {
                BaseSkinName = "Kennen",
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

            });

            #endregion Kennen
            #region Khazix
            Spells.Add(new SpellData
            {
                BaseSkinName = "Khazix",
                SpellName = "khazixwlong",
                Slot = SpellSlot.W,
                Type = SkillShotType.SkillshotMissileLine,
                Delay = 250,
                Range = 1025,
                Radius = 70,
                MissileSpeed = 1700,
                FixedRange = true,
                AddHitbox = true,
                DangerValue = 2,
                IsDangerous = false,
                MissileSpellName = "khazixwlong",
                CanBeRemoved = true,

            });

            Spells.Add(new SpellData
            {
                BaseSkinName = "Khazix",
                SpellName = "KhazixW",
                Slot = SpellSlot.W,
                Type = SkillShotType.SkillshotMissileLine,
                Delay = 250,
                Range = 1025,
                Radius = 70,
                MissileSpeed = 1700,
                FixedRange = true,
                AddHitbox = true,
                DangerValue = 2,
                IsDangerous = false,
                MissileSpellName = "KhazixW",
                CanBeRemoved = true,

            });

            Spells.Add(new SpellData
            {
                BaseSkinName = "Khazix",
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
            #region KogMaw
            Spells.Add(new SpellData
            {
                BaseSkinName = "KogMaw",
                SpellName = "KogMawQ",
                Slot = SpellSlot.Q,
                Type = SkillShotType.SkillshotMissileLine,
                Delay = 250,
                Range = 1000,
                Radius = 70,
                MissileSpeed = 1650,
                FixedRange = true,
                AddHitbox = true,
                DangerValue = 2,
                IsDangerous = false,
                MissileSpellName = "KogMawQ",
                CanBeRemoved = true,

            });

            Spells.Add(new SpellData
            {
                BaseSkinName = "KogMaw",
                SpellName = "KogMawVoidOoze",
                Slot = SpellSlot.E,
                Type = SkillShotType.SkillshotMissileLine,
                Delay = 250,
                Range = 1500,
                Radius = 120,
                MissileSpeed = 1400,
                FixedRange = true,
                AddHitbox = true,
                DangerValue = 2,
                IsDangerous = false,
                MissileSpellName = "KogMawVoidOoze",

            });

            Spells.Add(new SpellData
            {
                BaseSkinName = "KogMaw",
                SpellName = "KogMawLivingArtillery",
                Slot = SpellSlot.R,
                Type = SkillShotType.SkillshotCircle,
                Delay = 1000,
                Range = 1800,
                Radius = 225,
                MissileSpeed = int.MaxValue,
                FixedRange = false,
                AddHitbox = true,
                DangerValue = 2,
                IsDangerous = false,
                MissileSpellName = "KogMawLivingArtillery",

            });

            #endregion KogMaw
            #region Leblanc
            Spells.Add(new SpellData
            {
                BaseSkinName = "Leblanc",
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

            Spells.Add(new SpellData
            {
                BaseSkinName = "Leblanc",
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

            Spells.Add(new SpellData
            {
                BaseSkinName = "Leblanc",
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

            });

            Spells.Add(new SpellData
            {
                BaseSkinName = "Leblanc",
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

            });

            #endregion Leblanc
            #region LeeSin
            Spells.Add(new SpellData
            {
                BaseSkinName = "LeeSin",
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

            });

            #endregion LeeSin
            #region Leona
            Spells.Add(new SpellData
            {
                BaseSkinName = "Leona",
                SpellName = "LeonaZenithBlade",
                Slot = SpellSlot.E,
                Type = SkillShotType.SkillshotMissileLine,
                Delay = 250,
                Range = 900,
                Radius = 90,
                MissileSpeed = 2000,
                FixedRange = true,
                AddHitbox = true,
                DangerValue = 3,
                IsDangerous = true,
                MissileSpellName = "LeonaZenithBlade",

            });

            Spells.Add(new SpellData
            {
                BaseSkinName = "Leona",
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

            #endregion Leona+
            //fix
            #region Lucian
            Spells.Add(new SpellData
            {
                BaseSkinName = "Lucian",
                SpellName = "LucianQ",
                Slot = SpellSlot.Q,
                Type = SkillShotType.SkillshotLine,
                Delay = 600,
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
            Spells.Add(new SpellData
            {
                BaseSkinName = "Lulu",
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
                MissileSpellName = "LuluQ",

            });

            #endregion Lulu
            #region Lux
            Spells.Add(new SpellData
            {
                BaseSkinName = "Lux",
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
                MissileSpellName = "LuxLightBinding",
                CanBeRemoved = true,

            });

            Spells.Add(new SpellData
            {
                BaseSkinName = "Lux",
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
                DonCross = true,

            });

            Spells.Add(new SpellData
            {
                BaseSkinName = "Lux",
                SpellName = "LuxMaliceCannon",
                Slot = SpellSlot.R,
                Type = SkillShotType.SkillshotLine,
                Delay = 1150,
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
            Spells.Add(new SpellData
            {
                BaseSkinName = "Malphite",
                SpellName = "UFSlash",
                Slot = SpellSlot.R,
                Type = SkillShotType.SkillshotCircle,
                Delay = 250,
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
            //FIX
            #region Malzahar
            Spells.Add(new SpellData
            {
                BaseSkinName = "Malzahar",
                SpellName = "AlZaharCalloftheVoid",
                Slot = SpellSlot.Q,
                Type = SkillShotType.SkillshotLine,
                Delay = 1000,
                Range = 700,
                Radius = 85,
                MissileSpeed = int.MaxValue,
                FixedRange = false,
                AddHitbox = true,
                DangerValue = 2,
                IsDangerous = false,
                MissileSpellName = "AlZaharCalloftheVoid",

            });

            #endregion Malzahar
            #region Morgana
            Spells.Add(new SpellData
            {
                BaseSkinName = "Morgana",
                SpellName = "DarkBindingMissile",
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
                MissileSpellName = "DarkBindingMissile",
                CanBeRemoved = true,

            });

            #endregion Morgana
            #region Nami
            Spells.Add(new SpellData
            {
                BaseSkinName = "Nami",
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
                MissileSpellName = "NamiQ",

            });

            Spells.Add(new SpellData
            {
                BaseSkinName = "Nami",
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
                MissileSpellName = "NamiR",

            });

            #endregion Nami
            #region Nautilus
            Spells.Add(new SpellData
            {
                BaseSkinName = "Nautilus",
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
                MissileSpellName = "NautilusAnchorDrag",
                CanBeRemoved = true,

            });

            #endregion Nautilus
            #region Nidalee
            Spells.Add(new SpellData
            {
                BaseSkinName = "Nidalee",
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

            });

            #endregion Nidalee
            #region Olaf
            Spells.Add(new SpellData
            {
                BaseSkinName = "Olaf",
                SpellName = "OlafAxeThrowCast",
                Slot = SpellSlot.Q,
                Type = SkillShotType.SkillshotMissileLine,
                Delay = 250,
                Range = 1000,
                Radius = 90,
                MissileSpeed = 1600,
                FixedRange = false,
                AddHitbox = true,
                DangerValue = 2,
                IsDangerous = false,
                MissileSpellName = "OlafAxeThrowCast",
                CanBeRemoved = true,

            });

            #endregion Olaf
            #region Orianna
            Spells.Add(new SpellData
            {
                BaseSkinName = "Orianna",
                SpellName = "OriannaQs",
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
                MissileSpellName = "OriannaQs",

            });

            Spells.Add(new SpellData
            {
                BaseSkinName = "Orianna",
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

            Spells.Add(new SpellData
            {
                BaseSkinName = "Orianna",
                SpellName = "OriannaEM",
                Slot = SpellSlot.E,
                Type = SkillShotType.SkillshotMissileLine,
                Delay = 0,
                Range = 1500,
                Radius = 80,
                MissileSpeed = 1850,
                FixedRange = false,
                AddHitbox = true,
                DangerValue = 2,
                IsDangerous = false,
                MissileSpellName = "OriannaEM",

            });

            Spells.Add(new SpellData
            {
                BaseSkinName = "Orianna",
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
            Spells.Add(new SpellData
            {
                BaseSkinName = "Quinn",
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
                MissileSpellName = "QuinnQ",
                CanBeRemoved = true,

            });

            #endregion Quinn
            #region Rengar
            Spells.Add(new SpellData
            {
                BaseSkinName = "Rengar",
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
                MissileSpellName = "RengarE",
                CanBeRemoved = true,

            });

            #endregion Rengar
            //FIX
            #region Riven
            Spells.Add(new SpellData
            {
                BaseSkinName = "Riven",
                SpellName = "rivenizunablade",
                Slot = SpellSlot.R,
                Type = SkillShotType.SkillshotCone,
                Delay = 600,
                Range = 1100,
                Radius = 80,
                MissileSpeed = int.MaxValue,
                FixedRange = false,
                AddHitbox = false,
                DangerValue = 3,
                IsDangerous = true,
                MissileSpellName = "rivenizunablade",

            });

            #endregion Riven
            #region Rumble
            Spells.Add(new SpellData
            {
                BaseSkinName = "Rumble",
                SpellName = "RumbleGrenade",
                Slot = SpellSlot.E,
                Type = SkillShotType.SkillshotMissileLine,
                Delay = 0,
                Range = 950,
                Radius = 60,
                MissileSpeed = 2000,
                FixedRange = true,
                AddHitbox = true,
                DangerValue = 2,
                IsDangerous = false,
                MissileSpellName = "RumbleGrenade",
                CanBeRemoved = true,

            });

            #endregion Rumble
            #region Shen
            Spells.Add(new SpellData
            {
                BaseSkinName = "Shen",
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

            });

            #endregion Shen
            #region Shyvana
            Spells.Add(new SpellData
            {
                BaseSkinName = "Shyvana",
                SpellName = "ShyvanaFireball",
                Slot = SpellSlot.Q,
                Type = SkillShotType.SkillshotMissileLine,
                Delay = 250,
                Range = 950,
                Radius = 60,
                MissileSpeed = 1700,
                FixedRange = true,
                AddHitbox = true,
                DangerValue = 2,
                IsDangerous = false,
                MissileSpellName = "ShyvanaFireball",

            });

            Spells.Add(new SpellData
            {
                BaseSkinName = "Shyvana",
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
            Spells.Add(new SpellData
            {
                BaseSkinName = "Sivir",
                SpellName = "SivirQReturn",
                Slot = SpellSlot.Q,
                Type = SkillShotType.SkillshotMissileLine,
                Delay = 0,
                Range = 1175,
                Radius = 100,
                MissileSpeed = 1350,
                FixedRange = true,
                AddHitbox = true,
                DangerValue = 2,
                IsDangerous = false,
                MissileSpellName = "SivirQReturn",
                DisableFowDetection = true,
                MissileFollowsUnit = true,

            });

            Spells.Add(new SpellData
            {
                BaseSkinName = "Sivir",
                SpellName = "SivirQ",
                Slot = SpellSlot.Q,
                Type = SkillShotType.SkillshotMissileLine,
                Delay = 250,
                Range = 1175,
                Radius = 90,
                MissileSpeed = 1350,
                FixedRange = true,
                AddHitbox = true,
                DangerValue = 2,
                IsDangerous = false,
                MissileSpellName = "SivirQ",

            });

            #endregion Sivir
            #region Skarner
            Spells.Add(new SpellData
            {
                BaseSkinName = "Skarner",
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
                MissileSpellName = "SkarnerFracture",

            });

            #endregion Skarner
            #region Sona
            Spells.Add(new SpellData
            {
                BaseSkinName = "Sona",
                SpellName = "SonaCrescendo",
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
                MissileSpellName = "SonaCrescendo",

            });

            #endregion Sona
            #region Swain
            Spells.Add(new SpellData
            {
                BaseSkinName = "Swain",
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
            //FIX
            #region Syndra
            Spells.Add(new SpellData
            {
                BaseSkinName = "Syndra",
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

            Spells.Add(new SpellData
            {
                BaseSkinName = "Syndra",
                SpellName = "syndrawcast",
                Slot = SpellSlot.W,
                Type = SkillShotType.SkillshotCircle,
                Delay = 900,
                Range = 950,
                Radius = 210,
                MissileSpeed = int.MaxValue,
                FixedRange = false,
                AddHitbox = true,
                DangerValue = 2,
                IsDangerous = false,
                MissileSpellName = "syndrawcast",

            });

            Spells.Add(new SpellData
            {
                BaseSkinName = "Syndra",
                SpellName = "syndrae5",
                Slot = SpellSlot.E,
                Type = SkillShotType.SkillshotMissileLine,
                Delay = 500,
                Range = 950,
                Radius = 90,
                MissileSpeed = 1601,
                FixedRange = false,
                AddHitbox = true,
                DangerValue = 2,
                IsDangerous = false,
                MissileSpellName = "syndrae5",

            });

            Spells.Add(new SpellData
            {
                BaseSkinName = "Syndra",
                SpellName = "SyndraE",
                Slot = SpellSlot.E,
                Type = SkillShotType.SkillshotMissileLine,
                Delay = 500,
                Range = 950,
                Radius = 90,
                MissileSpeed = 1601,
                FixedRange = false,
                AddHitbox = true,
                DangerValue = 2,
                IsDangerous = false,
                MissileSpellName = "SyndraE",

            });

            #endregion Syndra
            //FIX
            #region Thresh
            Spells.Add(new SpellData
            {
                BaseSkinName = "Thresh",
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
                MissileSpellName = "ThreshQ",
                CanBeRemoved = true,

            });

            Spells.Add(new SpellData
            {
                BaseSkinName = "Thresh",
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
                MissileSpellName = "ThreshEFlay",

            });

            #endregion Thresh
            #region Tristana
            Spells.Add(new SpellData
            {
                BaseSkinName = "Tristana",
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
            Spells.Add(new SpellData
            {
                BaseSkinName = "Tryndamere",
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
            Spells.Add(new SpellData
            {
                BaseSkinName = "TwistedFate",
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
                MissileSpellName = "WildCards",
                MultipleNumber = 3,
                MultipleAngle = 28 * (float)Math.PI / 180,

            });

            #endregion TwistedFate
            #region Twitch
            Spells.Add(new SpellData
            {
                BaseSkinName = "Twitch",
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
                MissileSpellName = "TwitchVenomCask",

            });

            #endregion Twitch
            #region Urgot
            Spells.Add(new SpellData
            {
                BaseSkinName = "Urgot",
                SpellName = "UrgotPlasmaGrenade",
                Slot = SpellSlot.Q,
                Type = SkillShotType.SkillshotCircle,
                Delay = 250,
                Range = 1100,
                Radius = 210,
                MissileSpeed = 1500,
                FixedRange = false,
                AddHitbox = true,
                DangerValue = 2,
                IsDangerous = false,
                MissileSpellName = "UrgotPlasmaGrenade",

            });

            Spells.Add(new SpellData
            {
                BaseSkinName = "Urgot",
                SpellName = "UrgotHeatseekingLineMissile",
                Slot = SpellSlot.E,
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

            #endregion Urgot
            #region Varus
            Spells.Add(new SpellData
            {
                BaseSkinName = "Varus",
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
                MissileSpellName = "VarusQMissilee",

            });

            Spells.Add(new SpellData
            {
                BaseSkinName = "Varus",
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

            Spells.Add(new SpellData
            {
                BaseSkinName = "Varus",
                SpellName = "VarusR",
                Slot = SpellSlot.R,
                Type = SkillShotType.SkillshotMissileLine,
                Delay = 250,
                Range = 1200,
                Radius = 100,
                MissileSpeed = 1950,
                FixedRange = true,
                AddHitbox = true,
                DangerValue = 3,
                IsDangerous = true,
                MissileSpellName = "VarusR",
                CanBeRemoved = true,

            });

            #endregion Varus
            #region Veigar
            Spells.Add(new SpellData
            {
                BaseSkinName = "Veigar",
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
                MissileSpellName = "VeigarDarkMatter",

            });

            #endregion Veigar
            //FIX
            #region Velkoz
            Spells.Add(new SpellData
            {
                BaseSkinName = "Velkoz",
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
                MissileSpellName = "VelkozQ",
                CanBeRemoved = true,

            });

            Spells.Add(new SpellData
            {
                BaseSkinName = "Velkoz",
                SpellName = "VelkozQSplit",
                Slot = SpellSlot.Q,
                Type = SkillShotType.SkillshotMissileLine,
                Delay = 250,
                Range = 900,
                Radius = 45,
                MissileSpeed = 2100,
                FixedRange = true,
                AddHitbox = true,
                DangerValue = 2,
                IsDangerous = false,
                MissileSpellName = "VelkozQSplit",
                CanBeRemoved = true,

            });

            Spells.Add(new SpellData
            {
                BaseSkinName = "Velkoz",
                SpellName = "VelkozW",
                Slot = SpellSlot.W,
                Type = SkillShotType.SkillshotMissileLine,
                Delay = 250,
                Range = 1200,
                Radius = 65,
                MissileSpeed = 1700,
                FixedRange = true,
                AddHitbox = true,
                DangerValue = 2,
                IsDangerous = false,
                MissileSpellName = "VelkozW",

            });

            Spells.Add(new SpellData
            {
                BaseSkinName = "Velkoz",
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
                MissileSpellName = "VelkozE",

            });

            #endregion Velkoz
            //CHECK
            #region Vi
            Spells.Add(new SpellData
            {
                BaseSkinName = "Vi",
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
                MissileSpellName = "Viq",

            });

            #endregion Vi
            #region Viktor
            Spells.Add(new SpellData
            {
                BaseSkinName = "Viktor",
                SpellName = "ViktorDeathRayF",
                Slot = SpellSlot.Q,
                Type = SkillShotType.SkillshotMissileLine,
                Delay = 250,
                Range = 1500,
                Radius = 80,
                MissileSpeed = 780,
                FixedRange = false,
                AddHitbox = true,
                DangerValue = 2,
                IsDangerous = false,
                MissileSpellName = "ViktorDeathRayF",

            });

            #endregion Viktor
            #region Xerath
            Spells.Add(new SpellData
            {
                BaseSkinName = "Xerath",
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

            Spells.Add(new SpellData
            {
                BaseSkinName = "Xerath",
                SpellName = "XerathArcaneBarrage2",
                Slot = SpellSlot.W,
                Type = SkillShotType.SkillshotCircle,
                Delay = 900,
                Range = 1000,
                Radius = 200,
                MissileSpeed = int.MaxValue,
                FixedRange = false,
                AddHitbox = true,
                DangerValue = 2,
                IsDangerous = false,
                MissileSpellName = "XerathArcaneBarrage2",

            });

            Spells.Add(new SpellData
            {
                BaseSkinName = "Xerath",
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
                IsDangerous = false,
                MissileSpellName = "XerathMageSpear",
                CanBeRemoved = true,

            });

            Spells.Add(new SpellData
            {
                BaseSkinName = "Xerath",
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
            Spells.Add(new SpellData
            {
                BaseSkinName = "Yasuo",
                SpellName = "yasuoq2",
                Slot = SpellSlot.Q,
                Type = SkillShotType.SkillshotLine,
                Delay = 400,
                Range = 520,
                Radius = 20,
                MissileSpeed = int.MaxValue,
                FixedRange = true,
                AddHitbox = true,
                DangerValue = 2,
                IsDangerous = true,
                MissileSpellName = "yasuoq2",
                Invert = true,

            });

            Spells.Add(new SpellData
            {
                BaseSkinName = "Yasuo",
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

            });

            Spells.Add(new SpellData
            {
                BaseSkinName = "Yasuo",
                SpellName = "yasuoq",
                Slot = SpellSlot.Q,
                Type = SkillShotType.SkillshotLine,
                Delay = 400,
                Range = 520,
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
            Spells.Add(new SpellData
            {
                BaseSkinName = "Zac",
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
            Spells.Add(new SpellData
            {
                BaseSkinName = "Zed",
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
                MissileSpellName = "ZedShuriken",

            });

            #endregion Zed
            //FiX
            #region Ziggs
            Spells.Add(new SpellData
            {
                BaseSkinName = "Ziggs",
                SpellName = "ZiggsQ2",
                Slot = SpellSlot.Q,
                Type = SkillShotType.SkillshotCircle,
                Delay = 250,
                Range = 850,
                Radius = 240,
                MissileSpeed = 1600,
                FixedRange = false,
                AddHitbox = true,
                DangerValue = 2,
                IsDangerous = false,
                MissileSpellName = "ZiggsQ2",
                CanBeRemoved = true,
                DisableFowDetection = true,

            });

            Spells.Add(new SpellData
            {
                BaseSkinName = "Ziggs",
                SpellName = "ZiggsQ3",
                Slot = SpellSlot.Q,
                Type = SkillShotType.SkillshotCircle,
                Delay = 250,
                Range = 850,
                Radius = 240,
                MissileSpeed = 1400,
                FixedRange = false,
                AddHitbox = true,
                DangerValue = 2,
                IsDangerous = false,
                MissileSpellName = "ZiggsQ3",
                CanBeRemoved = true,
                DisableFowDetection = true,

            });

            Spells.Add(new SpellData
            {
                BaseSkinName = "Ziggs",
                SpellName = "ZiggsQ",
                Slot = SpellSlot.Q,
                Type = SkillShotType.SkillshotCircle,
                Delay = 250,
                Range = 850,
                Radius = 240,
                MissileSpeed = 1700,
                FixedRange = false,
                AddHitbox = true,
                DangerValue = 2,
                IsDangerous = false,
                MissileSpellName = "ZiggsQ",
                CanBeRemoved = true,
                DisableFowDetection = true,

            });

            Spells.Add(new SpellData
            {
                BaseSkinName = "Ziggs",
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

            });

            Spells.Add(new SpellData
            {
                BaseSkinName = "Ziggs",
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

            Spells.Add(new SpellData
            {
                BaseSkinName = "Ziggs",
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
            #region Zyra
            Spells.Add(new SpellData
            {
                BaseSkinName = "Zyra",
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

            Spells.Add(new SpellData
            {
                BaseSkinName = "Zyra",
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

            });

            Spells.Add(new SpellData
            {
                BaseSkinName = "Zyra",
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

            });

            #endregion Zyra

            Game.PrintChat("Added " + Spells.Count + " spells.");//138
        }

        public static SpellData GetByName(string spellName)
        {
            spellName = spellName.ToLower();
            foreach (var spellData in Spells)
            {
                if (spellData.SpellName.ToLower() == spellName)
                    return spellData;
            }

            return null;
        }

        public static SpellData GetBySpeed(string baseSkinName, int speed, int id = -1)
        {
            foreach (var spellData in Spells)
            {
                if (spellData.BaseSkinName == baseSkinName && spellData.MissileSpeed == speed &&
                    (spellData.Id == -1 || id == spellData.Id))
                    return spellData;
            }

            return null;
        }
    }
}