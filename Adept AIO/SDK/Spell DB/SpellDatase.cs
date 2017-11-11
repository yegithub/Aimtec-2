﻿namespace Adept_AIO.SDK.Spell_DB
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Aimtec;
    using Aimtec.SDK.Prediction.Collision;
    using Aimtec.SDK.Prediction.Skillshots;

    #region

    #endregion

    public static class SpellDatabase
    {
        public static List<SpellData> Spells = new List<SpellData>();

        static SpellDatabase()
        {
            //Add spells to the database 

            #region ball

            Spells.Add(new SpellData
            {
                ChampionName = "AllChampions",
                SpellName = "SummonerSnowball",
                Slot = SpellSlot.Summoner1,
                Type = SkillshotType.Line,
                Delay = 0,
                Range = 1600,
                Radius = 50,
                MissileSpeed = 1200,
                FixedRange = true,
                AddHitbox = true,
                DangerValue = 3,
                IsDangerous = false,
                CollisionObjects = new[]
                {
                    CollisionableObjects.Heroes,
                    CollisionableObjects.Minions,
                    CollisionableObjects.YasuoWall
                }
            });

            Spells.Add(new SpellData
            {
                ChampionName = "AllChampions",
                SpellName = "SummonerPoroThrow",
                Slot = SpellSlot.Summoner1,
                Type = SkillshotType.Line,
                Delay = 0,
                Range = 1200,
                Radius = 50,
                MissileSpeed = 1200,
                FixedRange = true,
                AddHitbox = true,
                DangerValue = 3,
                IsDangerous = false,
                CollisionObjects = new[]
                {
                    CollisionableObjects.Heroes,
                    CollisionableObjects.Minions,
                    CollisionableObjects.YasuoWall
                }
            });

            #endregion

            #region Aatrox

            Spells.Add(new SpellData
            {
                ChampionName = "Aatrox",
                SpellName = "AatroxQ",
                Slot = SpellSlot.Q,
                Type = SkillshotType.Circle,
                Delay = 600,
                Range = 650,
                Radius = 280,
                MissileSpeed = 3000,
                FixedRange = false,
                AddHitbox = true,
                DangerValue = 3,
                IsDangerous = true,
                MissileSpellName = ""
            });

            Spells.Add(new SpellData
            {
                ChampionName = "Aatrox",
                SpellName = "AatroxE",
                Slot = SpellSlot.E,
                Type = SkillshotType.Line,
                Delay = 250,
                Range = 1075,
                Radius = 60,
                MissileSpeed = 1250,
                FixedRange = true,
                AddHitbox = true,
                DangerValue = 3,
                IsDangerous = false,
                MissileSpellName = "AatroxEConeMissile"
            });

            #endregion Aatrox

            #region Ahri

            Spells.Add(new SpellData
            {
                ChampionName = "Ahri",
                SpellName = "AhriOrbofDeception",
                Slot = SpellSlot.Q,
                Type = SkillshotType.Line,
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
                CollisionObjects = new[]
                {
                    CollisionableObjects.YasuoWall
                }
            });

            Spells.Add(new SpellData
            {
                ChampionName = "Ahri",
                SpellName = "AhriOrbReturn",
                Slot = SpellSlot.Q,
                Type = SkillshotType.Line,
                Delay = 200,
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
                CollisionObjects = new[]
                {
                    CollisionableObjects.YasuoWall
                }
            });

            Spells.Add(new SpellData
            {
                ChampionName = "Ahri",
                SpellName = "AhriSeduce",
                Slot = SpellSlot.E,
                Type = SkillshotType.Line,
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
                CollisionObjects = new[]
                {
                    CollisionableObjects.Heroes,
                    CollisionableObjects.Minions,
                    CollisionableObjects.YasuoWall
                }
            });

            #endregion Ahri

            #region Akali

            Spells.Add(new SpellData
            {
                ChampionName = "Akali",
                SpellName = "AkaliShadowSwipe",
                Slot = SpellSlot.E,
                Type = SkillshotType.Circle,
                Delay = 250,
                Range = 10, //Fix Range, maybe 0 better~
                Radius = 325,
                MissileSpeed = int.MaxValue,
                FixedRange = false,
                AddHitbox = false,
                DangerValue = 2,
                IsDangerous = false,
                MissileSpellName = ""
            });

            #endregion Akali

            #region Alistar

            Spells.Add(new SpellData
            {
                ChampionName = "Alistar",
                SpellName = "Pulverize",
                Slot = SpellSlot.Q,
                Type = SkillshotType.Circle,
                Delay = 250,
                Range = 365,
                Radius = 365,
                MissileSpeed = int.MaxValue,
                FixedRange = true,
                AddHitbox = true,
                DangerValue = 3,
                IsDangerous = true
            });

            #endregion

            #region Amumu

            Spells.Add(new SpellData
            {
                ChampionName = "Amumu",
                SpellName = "BandageToss",
                Slot = SpellSlot.Q,
                Type = SkillshotType.Line,
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
                CollisionObjects = new[]
                {
                    CollisionableObjects.Heroes,
                    CollisionableObjects.Minions,
                    CollisionableObjects.YasuoWall
                }
            });

            Spells.Add(new SpellData
            {
                ChampionName = "Amumu",
                SpellName = "CurseoftheSadMummy",
                Slot = SpellSlot.R,
                Type = SkillshotType.Circle,
                Delay = 250,
                Range = 0,
                Radius = 550,
                MissileSpeed = int.MaxValue,
                FixedRange = true,
                AddHitbox = false,
                DangerValue = 5,
                IsDangerous = true,
                MissileSpellName = ""
            });

            #endregion Amumu

            #region Anivia

            Spells.Add(new SpellData
            {
                ChampionName = "Anivia",
                SpellName = "FlashFrost",
                Slot = SpellSlot.Q,
                Type = SkillshotType.Line,
                Delay = 250,
                Range = 1100,
                Radius = 110,
                MissileSpeed = 850,
                FixedRange = true,
                AddHitbox = true,
                DangerValue = 3,
                IsDangerous = true,
                MissileSpellName = "FlashFrostSpell",
                CanBeRemoved = true,
                CollisionObjects = new[]
                {
                    CollisionableObjects.YasuoWall
                }
            });

            #endregion Anivia

            #region Annie

            Spells.Add(new SpellData
            {
                ChampionName = "Annie",
                SpellName = "Incinerate",
                Slot = SpellSlot.W,
                Type = SkillshotType.Cone,
                Delay = 250,
                Range = 825,
                Radius = 80,
                MissileSpeed = int.MaxValue,
                FixedRange = false,
                AddHitbox = false,
                DangerValue = 2,
                IsDangerous = false,
                MissileSpellName = ""
            });

            Spells.Add(new SpellData
            {
                ChampionName = "Annie",
                SpellName = "InfernalGuardian",
                Slot = SpellSlot.R,
                Type = SkillshotType.Circle,
                Delay = 250,
                Range = 600,
                Radius = 250,
                MissileSpeed = int.MaxValue,
                FixedRange = false,
                AddHitbox = true,
                DangerValue = 5,
                IsDangerous = true,
                MissileSpellName = ""
            });

            #endregion Annie

            #region Ashe

            Spells.Add(new SpellData
            {
                ChampionName = "Ashe",
                SpellName = "Volley",
                Slot = SpellSlot.W,
                Type = SkillshotType.Line,
                Delay = 250,
                Range = 1200,
                Radius = 60,
                MissileSpeed = 2000,
                FixedRange = true,
                AddHitbox = true,
                DangerValue = 2,
                IsDangerous = false,
                MissileSpellName = "VolleyAttack",
                MultipleNumber = 9,
                MultipleAngle = 4.62f * (float) Math.PI / 180,
                CanBeRemoved = true,
                CollisionObjects = new[]
                {
                    CollisionableObjects.Heroes,
                    CollisionableObjects.YasuoWall,
                    CollisionableObjects.Minions
                }
            });

            Spells.Add(new SpellData
            {
                ChampionName = "Ashe",
                SpellName = "EnchantedCrystalArrow",
                Slot = SpellSlot.R,
                Type = SkillshotType.Line,
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
                CollisionObjects = new[]
                {
                    CollisionableObjects.Heroes,
                    CollisionableObjects.YasuoWall
                }
            });

            #endregion Ashe

            #region Aurelion Sol

            Spells.Add(new SpellData
            {
                ChampionName = "AurelionSol",
                SpellName = "AurelionSolQ",
                Slot = SpellSlot.Q,
                Type = SkillshotType.Line,
                Delay = 250,
                Range = 1500,
                Radius = 180,
                MissileSpeed = 850,
                FixedRange = true,
                AddHitbox = true,
                DangerValue = 2,
                IsDangerous = false,
                MissileSpellName = "AurelionSolQMissile",
                CanBeRemoved = true,
                CollisionObjects = new[]
                {
                    CollisionableObjects.YasuoWall
                }
            });

            Spells.Add(new SpellData
            {
                ChampionName = "AurelionSol",
                SpellName = "AurelionSolR",
                Slot = SpellSlot.R,
                Type = SkillshotType.Line,
                Delay = 300,
                Range = 1420,
                Radius = 120,
                MissileSpeed = 4500,
                FixedRange = true,
                AddHitbox = true,
                DangerValue = 3,
                IsDangerous = true,
                MissileSpellName = "AurelionSolRBeamMissile",
                CanBeRemoved = true,
                CollisionObjects = new[]
                {
                    CollisionableObjects.YasuoWall
                }
            });

            #endregion Aurelion Sol

            #region Azir

            Spells.Add(new SpellData
            {
                ChampionName = "Azir",
                SpellName = "AzirQSoldier",
                ExtraSpellNames = new[]
                {
                    "AzirQWrapper"
                },
                Slot = SpellSlot.Q,
                Type = SkillshotType.Line,
                Delay = 250,
                Range = 1000,
                Radius = 80,
                MissileSpeed = 2550,
                FixedRange = false,
                AddHitbox = true,
                DangerValue = 3,
                IsDangerous = false,
                MissileSpellName = "",
                FromObjects = new[]
                {
                    "AzirSoldier"
                }
            });

            Spells.Add(new SpellData
            {
                ChampionName = "Azir",
                SpellName = "AzirR",
                Slot = SpellSlot.R,
                Type = SkillshotType.Line,
                Delay = 250,
                Range = 700,
                Radius = 450,
                MissileSpeed = 1400,
                FixedRange = true,
                AddHitbox = true,
                DangerValue = 5,
                IsDangerous = true,
                MissileSpellName = "AzirSoldierRMissile"
            });

            #endregion

            #region Bard

            Spells.Add(new SpellData
            {
                ChampionName = "Bard",
                SpellName = "BardQ",
                Slot = SpellSlot.Q,
                Type = SkillshotType.Line,
                Delay = 250,
                Range = 950,
                Radius = 60,
                MissileSpeed = 1500,
                FixedRange = true,
                AddHitbox = true,
                DangerValue = 3,
                IsDangerous = true,
                MissileSpellName = "BardQMissile",
                CanBeRemoved = true,
                CollisionObjects = new[]
                {
                    CollisionableObjects.Heroes,
                    CollisionableObjects.YasuoWall
                }
            });

            Spells.Add(new SpellData
            {
                ChampionName = "Bard",
                SpellName = "BardR",
                Slot = SpellSlot.R,
                Type = SkillshotType.Circle,
                Delay = 500,
                Range = 3400,
                Radius = 350,
                MissileSpeed = 2100,
                FixedRange = false,
                AddHitbox = true,
                DangerValue = 2,
                IsDangerous = false,
                MissileSpellName = "BardR"
            });

            #endregion

            #region Blatzcrank

            Spells.Add(new SpellData
            {
                ChampionName = "Blitzcrank",
                SpellName = "RocketGrab",
                Slot = SpellSlot.Q,
                Type = SkillshotType.Line,
                Delay = 250,
                Range = 1050,
                Radius = 80,
                MissileSpeed = 1800,
                FixedRange = true,
                AddHitbox = true,
                DangerValue = 4,
                IsDangerous = true,
                MissileSpellName = "RocketGrabMissile",
                CanBeRemoved = true,
                CollisionObjects = new[]
                {
                    CollisionableObjects.Heroes,
                    CollisionableObjects.Minions,
                    CollisionableObjects.YasuoWall
                }
            });

            Spells.Add(new SpellData
            {
                ChampionName = "Blitzcrank",
                SpellName = "StaticField",
                Slot = SpellSlot.R,
                Type = SkillshotType.Circle,
                Delay = 250,
                Range = 0,
                Radius = 600,
                MissileSpeed = int.MaxValue,
                FixedRange = true,
                AddHitbox = false,
                DangerValue = 2,
                IsDangerous = false,
                MissileSpellName = ""
            });

            #endregion Blatzcrink

            #region Brand

            Spells.Add(new SpellData
            {
                ChampionName = "Brand",
                SpellName = "BrandQ",
                Slot = SpellSlot.Q,
                Type = SkillshotType.Line,
                Delay = 250,
                Range = 1100,
                Radius = 60,
                MissileSpeed = 1550,
                FixedRange = true,
                AddHitbox = true,
                DangerValue = 3,
                IsDangerous = true,
                MissileSpellName = "BrandQMissile",
                CanBeRemoved = true,
                CollisionObjects = new[]
                {
                    CollisionableObjects.Heroes,
                    CollisionableObjects.Minions,
                    CollisionableObjects.YasuoWall
                }
            });

            Spells.Add(new SpellData
            {
                ChampionName = "Brand",
                SpellName = "BrandW",
                Slot = SpellSlot.W,
                Type = SkillshotType.Circle,
                Delay = 850,
                Range = 900,
                Radius = 260,
                MissileSpeed = int.MaxValue,
                FixedRange = false,
                AddHitbox = false,
                DangerValue = 2,
                IsDangerous = false,
                MissileSpellName = ""
            });

            #endregion Brand

            #region Braum

            Spells.Add(new SpellData
            {
                ChampionName = "Braum",
                SpellName = "BraumQ",
                Slot = SpellSlot.Q,
                Type = SkillshotType.Line,
                Delay = 250,
                Range = 1000,
                Radius = 60,
                MissileSpeed = 1700,
                FixedRange = true,
                AddHitbox = true,
                DangerValue = 3,
                IsDangerous = true,
                MissileSpellName = "BraumQMissile",
                CanBeRemoved = true,
                CollisionObjects = new[]
                {
                    CollisionableObjects.Heroes,
                    CollisionableObjects.Minions,
                    CollisionableObjects.YasuoWall
                }
            });

            Spells.Add(new SpellData
            {
                ChampionName = "Braum",
                SpellName = "BraumRWrapper",
                Slot = SpellSlot.R,
                Type = SkillshotType.Line,
                Delay = 500,
                Range = 1250,
                Radius = 115,
                MissileSpeed = 1400,
                FixedRange = true,
                AddHitbox = true,
                DangerValue = 4,
                IsDangerous = true,
                MissileSpellName = "braumrmissile",
                CollisionObjects = new[]
                {
                    CollisionableObjects.YasuoWall
                }
            });

            #endregion Braum

            #region Caitlyn

            Spells.Add(new SpellData
            {
                ChampionName = "Caitlyn",
                SpellName = "CaitlynPiltoverPeacemaker",
                Slot = SpellSlot.Q,
                Type = SkillshotType.Line,
                Delay = 625,
                Range = 1250,
                Radius = 90,
                MissileSpeed = 2200,
                FixedRange = true,
                AddHitbox = true,
                DangerValue = 2,
                IsDangerous = false,
                MissileSpellName = "CaitlynPiltoverPeacemaker",
                CollisionObjects = new[]
                {
                    CollisionableObjects.YasuoWall
                }
            });

            Spells.Add(new SpellData
            {
                ChampionName = "Caitlyn",
                SpellName = "CaitlynEntrapment",
                Slot = SpellSlot.E,
                Type = SkillshotType.Line,
                Delay = 125,
                Range = 950,
                Radius = 70,
                MissileSpeed = 1600,
                FixedRange = true,
                AddHitbox = true,
                DangerValue = 1,
                IsDangerous = false,
                MissileSpellName = "CaitlynEntrapmentMissile",
                CanBeRemoved = true,
                CollisionObjects = new[]
                {
                    CollisionableObjects.Heroes,
                    CollisionableObjects.Minions,
                    CollisionableObjects.YasuoWall
                }
            });

            #endregion Caitlyn

            #region Camile

            Spells.Add(new SpellData
            {
                ChampionName = "Camille",
                SpellName = "CamilleE",
                Slot = SpellSlot.E,
                Type = SkillshotType.Line,
                Delay = 250,
                Range = 1100,
                Radius = 80,
                MissileSpeed = 2500,
                FixedRange = true,
                AddHitbox = true,
                DangerValue = 2,
                IsDangerous = false,
                MissileSpellName = "CamilleEMissile",
                CanBeRemoved = true
            });

            Spells.Add(new SpellData
            {
                ChampionName = "Camille",
                SpellName = "CamilleEDash2",
                Slot = SpellSlot.E,
                Type = SkillshotType.Line,
                Delay = 250,
                Range = 800,
                Radius = 80,
                MissileSpeed = 2500,
                FixedRange = true,
                AddHitbox = true,
                DangerValue = 2,
                IsDangerous = false,
                MissileSpellName = "CamilleEDash2"
            });

            #endregion Camile

            #region Cassiopeia

            Spells.Add(new SpellData
            {
                ChampionName = "Cassiopeia",
                SpellName = "CassiopeiaQ",
                Slot = SpellSlot.Q,
                Type = SkillshotType.Circle,
                Delay = 750,
                Range = 850,
                Radius = 150,
                MissileSpeed = int.MaxValue,
                FixedRange = false,
                AddHitbox = true,
                DangerValue = 2,
                IsDangerous = false,
                MissileSpellName = "CassiopeiaQ"
            });

            Spells.Add(new SpellData
            {
                ChampionName = "Cassiopeia",
                SpellName = "CassiopeiaW",
                Slot = SpellSlot.W,
                Type = SkillshotType.Circle,
                Delay = 0,
                Range = 850,
                Radius = 180,
                MissileSpeed = 3000,
                FixedRange = false,
                AddHitbox = true,
                DangerValue = 3,
                IsDangerous = false,
                MissileSpellName = "CassiopeiaWMissile",
                ExtraDuration = 5000,
                DontCross = true,
                CanBeRemoved = false,
                CollisionObjects = new[]
                {
                    CollisionableObjects.YasuoWall
                }
            });

            Spells.Add(new SpellData
            {
                ChampionName = "Cassiopeia",
                SpellName = "CassiopeiaR",
                Slot = SpellSlot.R,
                Type = SkillshotType.Cone,
                Delay = 600,
                Range = 825,
                Radius = 80,
                MissileSpeed = int.MaxValue,
                FixedRange = false,
                AddHitbox = false,
                DangerValue = 5,
                IsDangerous = true,
                MissileSpellName = "CassiopeiaR"
            });

            #endregion Cassiopeia

            #region Chogath

            Spells.Add(new SpellData
            {
                ChampionName = "Chogath",
                SpellName = "Rupture",
                Slot = SpellSlot.Q,
                Type = SkillshotType.Circle,
                Delay = 1200,
                Range = 950,
                Radius = 250,
                MissileSpeed = int.MaxValue,
                FixedRange = false,
                AddHitbox = true,
                DangerValue = 3,
                IsDangerous = false,
                MissileSpellName = "Rupture"
            });

            Spells.Add(new SpellData
            {
                ChampionName = "Chogath",
                SpellName = "FeralScream",
                Slot = SpellSlot.W,
                Type = SkillshotType.Cone,
                Delay = 250,
                Range = 585,
                Radius = 60,
                MissileSpeed = int.MaxValue,
                FixedRange = true,
                AddHitbox = true,
                DangerValue = 2,
                IsDangerous = false
            });

            #endregion Chogath

            #region Corki

            Spells.Add(new SpellData
            {
                ChampionName = "Corki",
                SpellName = "PhosphorusBomb",
                Slot = SpellSlot.Q,
                Type = SkillshotType.Circle,
                Delay = 300,
                Range = 825,
                Radius = 250,
                MissileSpeed = 1000,
                FixedRange = false,
                AddHitbox = true,
                DangerValue = 2,
                IsDangerous = false,
                MissileSpellName = "PhosphorusBombMissile",
                CollisionObjects = new[]
                {
                    CollisionableObjects.YasuoWall
                }
            });

            Spells.Add(new SpellData
            {
                ChampionName = "Corki",
                SpellName = "MissileBarrage",
                Slot = SpellSlot.R,
                Type = SkillshotType.Line,
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
                CollisionObjects = new[]
                {
                    CollisionableObjects.Heroes,
                    CollisionableObjects.Minions,
                    CollisionableObjects.YasuoWall
                }
            });

            Spells.Add(new SpellData
            {
                ChampionName = "Corki",
                SpellName = "MissileBarrage2",
                Slot = SpellSlot.R,
                Type = SkillshotType.Line,
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
                CollisionObjects = new[]
                {
                    CollisionableObjects.Heroes,
                    CollisionableObjects.Minions,
                    CollisionableObjects.YasuoWall
                }
            });

            #endregion Corki

            #region Darius

            Spells.Add(new SpellData
            {
                ChampionName = "Darius",
                SpellName = "DariusCleave",
                Slot = SpellSlot.Q,
                Type = SkillshotType.Circle,
                Delay = 750,
                Range = 0,
                Radius = 375,
                MissileSpeed = int.MaxValue,
                FixedRange = true,
                AddHitbox = true,
                DangerValue = 3,
                IsDangerous = false,
                MissileSpellName = "DariusCleave",
                FollowCaster = true,
                DisabledByDefault = true
            });

            Spells.Add(new SpellData
            {
                ChampionName = "Darius",
                SpellName = "DariusAxeGrabCone",
                Slot = SpellSlot.E,
                Type = SkillshotType.Cone,
                Delay = 250,
                Range = 550,
                Radius = 80,
                MissileSpeed = int.MaxValue,
                FixedRange = true,
                AddHitbox = false,
                DangerValue = 3,
                IsDangerous = true,
                MissileSpellName = "DariusAxeGrabCone"
            });

            #endregion Darius

            #region Diana

            Spells.Add(new SpellData
            {
                ChampionName = "Diana",
                SpellName = "DianaArc",
                Slot = SpellSlot.Q,
                Type = SkillshotType.Circle,
                Delay = 250,
                Range = 895,
                Radius = 195,
                MissileSpeed = 1400,
                FixedRange = false,
                AddHitbox = true,
                DangerValue = 3,
                IsDangerous = true,
                MissileSpellName = "DianaArcArc",
                CollisionObjects = new[]
                {
                    CollisionableObjects.YasuoWall
                }
            });

            Spells.Add(new SpellData
            {
                ChampionName = "Diana",
                SpellName = "DianaArcArc",
                Slot = SpellSlot.Q,
                Type = SkillshotType.Line,
                Delay = 250,
                Range = 895,
                Radius = 195,
                DontCross = true,
                MissileSpeed = 1400,
                FixedRange = false,
                AddHitbox = true,
                DangerValue = 3,
                IsDangerous = true,
                MissileSpellName = "DianaArcArc",
                TakeClosestPath = true,
                CollisionObjects = new[]
                {
                    CollisionableObjects.YasuoWall
                }
            });

            #endregion Diana

            #region DrMundo

            Spells.Add(new SpellData
            {
                ChampionName = "DrMundo",
                SpellName = "InfectedCleaverMissileCast",
                Slot = SpellSlot.Q,
                Type = SkillshotType.Line,
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
                CollisionObjects = new[]
                {
                    CollisionableObjects.Heroes,
                    CollisionableObjects.Minions,
                    CollisionableObjects.YasuoWall
                }
            });

            #endregion DrMundo

            #region Draven

            Spells.Add(new SpellData
            {
                ChampionName = "Draven",
                SpellName = "DravenDoubleShot",
                Slot = SpellSlot.E,
                Type = SkillshotType.Line,
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
                CollisionObjects = new[]
                {
                    CollisionableObjects.YasuoWall
                }
            });

            Spells.Add(new SpellData
            {
                ChampionName = "Draven",
                SpellName = "DravenRCast",
                Slot = SpellSlot.R,
                Type = SkillshotType.Line,
                Delay = 400,
                Range = 20000,
                Radius = 160,
                MissileSpeed = 2000,
                FixedRange = true,
                AddHitbox = true,
                DangerValue = 5,
                IsDangerous = true,
                MissileSpellName = "DravenR",
                CollisionObjects = new[]
                {
                    CollisionableObjects.YasuoWall
                }
            });

            #endregion Draven

            #region Ekko

            Spells.Add(new SpellData
            {
                ChampionName = "Ekko",
                SpellName = "EkkoQ",
                Slot = SpellSlot.Q,
                Type = SkillshotType.Line,
                Delay = 250,
                Range = 950,
                Radius = 60,
                MissileSpeed = 1650,
                FixedRange = true,
                AddHitbox = true,
                DangerValue = 4,
                IsDangerous = true,
                MissileSpellName = "ekkoqmis",
                CanBeRemoved = true,
                CollisionObjects = new[]
                {
                    CollisionableObjects.YasuoWall
                }
            });

            Spells.Add(new SpellData
            {
                ChampionName = "Ekko",
                SpellName = "EkkoQReturn",
                Slot = SpellSlot.Q,
                Type = SkillshotType.Line,
                Delay = 250,
                Range = 20000,
                Radius = 100,
                MissileSpeed = 2300,
                FixedRange = true,
                AddHitbox = true,
                DangerValue = 4,
                IsDangerous = true,
                MissileSpellName = "EkkoQReturn",
                CanBeRemoved = true,
                MissileFollowsUnit = true,
                CollisionObjects = new[]
                {
                    CollisionableObjects.YasuoWall
                }
            });

            Spells.Add(new SpellData
            {
                ChampionName = "Ekko",
                SpellName = "EkkoW",
                Slot = SpellSlot.W,
                Type = SkillshotType.Circle,
                Delay = 3350,
                Range = 1600,
                Radius = 375,
                MissileSpeed = 1650,
                FixedRange = false,
                DisabledByDefault = true,
                AddHitbox = false,
                DangerValue = 3,
                IsDangerous = false,
                MissileSpellName = "EkkoW",
                CanBeRemoved = true
            });

            Spells.Add(new SpellData
            {
                ChampionName = "Ekko",
                SpellName = "EkkoR",
                Slot = SpellSlot.R,
                Type = SkillshotType.Circle,
                Delay = 250,
                Range = 1600,
                Radius = 375,
                MissileSpeed = 1650,
                FixedRange = true,
                AddHitbox = true,
                DangerValue = 3,
                IsDangerous = false,
                MissileSpellName = "EkkoR",
                CanBeRemoved = true,
                FromObjects = new[]
                {
                    "Ekko_Base_R_TrailEnd.troy"
                }
            });

            #endregion Ekko

            #region Elise

            Spells.Add(new SpellData
            {
                ChampionName = "Elise",
                SpellName = "EliseHumanE",
                Slot = SpellSlot.E,
                Type = SkillshotType.Line,
                Delay = 250,
                Range = 1100,
                Radius = 55,
                MissileSpeed = 1600,
                FixedRange = true,
                AddHitbox = true,
                DangerValue = 4,
                IsDangerous = true,
                MissileSpellName = "EliseHumanE",
                CanBeRemoved = true,
                CollisionObjects = new[]
                {
                    CollisionableObjects.Heroes,
                    CollisionableObjects.Minions,
                    CollisionableObjects.YasuoWall
                }
            });

            #endregion Elise

            #region Evelynn

            Spells.Add(new SpellData
            {
                ChampionName = "Evelynn",
                SpellName = "EvelynnR",
                Slot = SpellSlot.R,
                Type = SkillshotType.Circle,
                Delay = 250,
                Range = 650,
                Radius = 350,
                MissileSpeed = int.MaxValue,
                FixedRange = false,
                AddHitbox = true,
                DangerValue = 5,
                IsDangerous = true,
                MissileSpellName = "EvelynnR"
            });

            #endregion Evelynn

            #region Ezreal

            Spells.Add(new SpellData
            {
                ChampionName = "Ezreal",
                SpellName = "EzrealMysticShot",
                Slot = SpellSlot.Q,
                Type = SkillshotType.Line,
                Delay = 250,
                Range = 1200,
                Radius = 60,
                MissileSpeed = 2000,
                FixedRange = true,
                AddHitbox = true,
                DangerValue = 2,
                IsDangerous = false,
                MissileSpellName = "EzrealMysticShotMissile",
                ExtraMissileNames = new[]
                {
                    "EzrealMysticShotPulseMissile"
                },
                CanBeRemoved = true,
                CollisionObjects = new[]
                {
                    CollisionableObjects.Heroes,
                    CollisionableObjects.Minions,
                    CollisionableObjects.YasuoWall
                },
                Id = 229
            });

            Spells.Add(new SpellData
            {
                ChampionName = "Ezreal",
                SpellName = "EzrealEssenceFlux",
                Slot = SpellSlot.W,
                Type = SkillshotType.Line,
                Delay = 250,
                Range = 1050,
                Radius = 80,
                MissileSpeed = 1600,
                FixedRange = true,
                AddHitbox = true,
                DangerValue = 2,
                IsDangerous = false,
                MissileSpellName = "EzrealEssenceFluxMissile",
                CollisionObjects = new[]
                {
                    CollisionableObjects.YasuoWall
                }
            });

            Spells.Add(new SpellData
            {
                ChampionName = "Ezreal",
                SpellName = "EzrealTrueshotBarrage",
                Slot = SpellSlot.R,
                Type = SkillshotType.Line,
                Delay = 1500,
                Range = 20000,
                Radius = 160,
                MissileSpeed = 2000,
                FixedRange = true,
                AddHitbox = true,
                DangerValue = 3,
                IsDangerous = true,
                MissileSpellName = "EzrealTrueshotBarrage",
                CollisionObjects = new[]
                {
                    CollisionableObjects.YasuoWall
                },
                Id = 245
            });

            #endregion Ezreal

            #region Fiora

            Spells.Add(new SpellData
            {
                ChampionName = "Fiora",
                SpellName = "FioraW",
                Slot = SpellSlot.W,
                Type = SkillshotType.Line,
                Delay = 500,
                Range = 800,
                Radius = 70,
                MissileSpeed = 3200,
                FixedRange = true,
                AddHitbox = true,
                DangerValue = 2,
                IsDangerous = false,
                MissileSpellName = "FioraWMissile",
                CollisionObjects = new[]
                {
                    CollisionableObjects.YasuoWall
                }
            });

            #endregion Fiora

            #region Fizz

            Spells.Add(new SpellData
            {
                ChampionName = "Fizz",
                SpellName = "FizzQ",
                Slot = SpellSlot.Q,
                Type = SkillshotType.Line,
                Delay = 250,
                Range = 550,
                Radius = 60,
                MissileSpeed = 3000,
                FixedRange = true,
                AddHitbox = true,
                DangerValue = 2,
                IsDangerous = true,
                MissileSpellName = ""
            });

            Spells.Add(new SpellData
            {
                ChampionName = "Fizz",
                SpellName = "FizzJump",
                Slot = SpellSlot.E,
                Type = SkillshotType.Circle,
                Delay = 560,
                Range = 400,
                Radius = 360,
                MissileSpeed = int.MaxValue,
                FixedRange = false,
                AddHitbox = true,
                DangerValue = 3,
                IsDangerous = true,
                MissileSpellName = "FizzJumpBuffer"
            });

            Spells.Add(new SpellData
            {
                ChampionName = "Fizz",
                SpellName = "FizzR",
                Slot = SpellSlot.R,
                Type = SkillshotType.Line,
                Delay = 250,
                Range = 1300,
                Radius = 120,
                MissileSpeed = 1350,
                FixedRange = false,
                AddHitbox = true,
                DangerValue = 5,
                IsDangerous = true,
                MissileSpellName = "FizzRMissile",
                ToggleParticleName = "Fizz_.+_R_OrbitFish",
                ExtraDuration = 2300,
                CollisionObjects = new[]
                {
                    CollisionableObjects.Heroes,
                    CollisionableObjects.YasuoWall
                },
                CanBeRemoved = true
            });

            #endregion Fizz

            #region Galio

            Spells.Add(new SpellData
            {
                ChampionName = "Galio",
                SpellName = "GalioQ",
                Slot = SpellSlot.Q,
                Type = SkillshotType.Circle,
                Delay = 250,
                Range = 825,
                Radius = 200,
                MissileSpeed = 1400,
                FixedRange = false,
                AddHitbox = false,
                DangerValue = 2,
                IsDangerous = false,
                MissileSpellName = "GalioQMissile",
                ExtraMissileNames = new[]
                {
                    "GalioArcArc"
                },
                ExtraDuration = 1500,
                DontCross = true
            });

            Spells.Add(new SpellData
            {
                ChampionName = "Galio",
                SpellName = "GalioW",
                Slot = SpellSlot.W,
                Type = SkillshotType.Circle,
                Delay = 0,
                Range = 500,
                Radius = 500,
                MissileSpeed = int.MaxValue,
                FixedRange = false,
                AddHitbox = true,
                DangerValue = 3,
                IsDangerous = true,
                MissileFollowsUnit = true,
                TakeClosestPath = true,
                ExtraDuration = 2000,
                DontCross = true,
                MissileSpellName = ""
            });

            Spells.Add(new SpellData
            {
                ChampionName = "Galio",
                SpellName = "GalioE",
                Slot = SpellSlot.E,
                Type = SkillshotType.Line,
                Delay = 250,
                Range = 850,
                Radius = 250,
                MissileSpeed = 1400,
                FixedRange = true,
                AddHitbox = true,
                DangerValue = 2,
                IsDangerous = false,
                MissileSpellName = "GalioE",
                CollisionObjects = new[]
                {
                    CollisionableObjects.YasuoWall
                }
            });

            Spells.Add(new SpellData
            {
                ChampionName = "Galio",
                SpellName = "GalioE2",
                Slot = SpellSlot.E,
                Type = SkillshotType.Line,
                Delay = 250,
                Range = 850,
                Radius = 250,
                MissileSpeed = 2000,
                FixedRange = true,
                AddHitbox = true,
                DangerValue = 2,
                IsDangerous = false,
                MissileSpellName = "GalioE2"
            });

            #endregion Galio

            #region Gnar

            Spells.Add(new SpellData
            {
                ChampionName = "Gnar",
                SpellName = "GnarQ",
                Slot = SpellSlot.Q,
                Type = SkillshotType.Line,
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
                CollisionObjects = new[]
                {
                    CollisionableObjects.YasuoWall
                }
            });

            Spells.Add(new SpellData
            {
                ChampionName = "Gnar",
                SpellName = "GnarQReturn",
                Slot = SpellSlot.Q,
                Type = SkillshotType.Line,
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
                DisabledByDefault = true,
                CollisionObjects = new[]
                {
                    CollisionableObjects.YasuoWall
                }
            });

            Spells.Add(new SpellData
            {
                ChampionName = "Gnar",
                SpellName = "GnarBigQ",
                Slot = SpellSlot.Q,
                Type = SkillshotType.Line,
                Delay = 500,
                Range = 1150,
                Radius = 90,
                MissileSpeed = 2100,
                FixedRange = true,
                AddHitbox = true,
                DangerValue = 2,
                IsDangerous = false,
                MissileSpellName = "GnarBigQMissile",
                CollisionObjects = new[]
                {
                    CollisionableObjects.YasuoWall
                }
            });

            Spells.Add(new SpellData
            {
                ChampionName = "Gnar",
                SpellName = "GnarBigW",
                Slot = SpellSlot.W,
                Type = SkillshotType.Line,
                Delay = 600,
                Range = 600,
                Radius = 80,
                MissileSpeed = int.MaxValue,
                FixedRange = true,
                AddHitbox = true,
                DangerValue = 2,
                IsDangerous = false,
                MissileSpellName = "GnarBigW"
            });

            Spells.Add(new SpellData
            {
                ChampionName = "Gnar",
                SpellName = "GnarE",
                Slot = SpellSlot.E,
                Type = SkillshotType.Circle,
                Delay = 0,
                Range = 473,
                Radius = 150,
                MissileSpeed = 903,
                FixedRange = false,
                AddHitbox = true,
                DangerValue = 2,
                IsDangerous = false,
                MissileSpellName = "GnarE"
            });

            Spells.Add(new SpellData
            {
                ChampionName = "Gnar",
                SpellName = "GnarBigE",
                Slot = SpellSlot.E,
                Type = SkillshotType.Circle,
                Delay = 250,
                Range = 475,
                Radius = 200,
                MissileSpeed = 1000,
                FixedRange = false,
                AddHitbox = true,
                DangerValue = 2,
                IsDangerous = false,
                MissileSpellName = "GnarBigE"
            });

            Spells.Add(new SpellData
            {
                ChampionName = "Gnar",
                SpellName = "GnarR",
                Slot = SpellSlot.R,
                Type = SkillshotType.Circle,
                Delay = 250,
                Range = 0,
                Radius = 500,
                MissileSpeed = int.MaxValue,
                FixedRange = true,
                AddHitbox = false,
                DangerValue = 5,
                IsDangerous = true,
                MissileSpellName = ""
            });

            #endregion

            #region Gragas

            Spells.Add(new SpellData
            {
                ChampionName = "Gragas",
                SpellName = "GragasQ",
                Slot = SpellSlot.Q,
                Type = SkillshotType.Circle,
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
                ToggleParticleName = "Gragas_.+_Q_(Enemy|Ally)",
                DontCross = true
            });

            Spells.Add(new SpellData
            {
                ChampionName = "Gragas",
                SpellName = "GragasE",
                Slot = SpellSlot.E,
                Type = SkillshotType.Line,
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
                CollisionObjects = new[]
                {
                    CollisionableObjects.Heroes,
                    CollisionableObjects.Minions
                }
            });

            Spells.Add(new SpellData
            {
                ChampionName = "Gragas",
                SpellName = "GragasR",
                Slot = SpellSlot.R,
                Type = SkillshotType.Circle,
                Delay = 250,
                Range = 1050,
                Radius = 375,
                MissileSpeed = 1800,
                FixedRange = false,
                AddHitbox = true,
                DangerValue = 5,
                IsDangerous = true,
                MissileSpellName = "GragasRBoom",
                CollisionObjects = new[]
                {
                    CollisionableObjects.YasuoWall
                }
            });

            #endregion Gragas

            #region Graves

            Spells.Add(new SpellData
            {
                ChampionName = "Graves",
                SpellName = "GravesQLineSpell",
                Slot = SpellSlot.Q,
                Type = SkillshotType.Line,
                Delay = 250,
                Range = 950,
                Radius = 40,
                MissileSpeed = 3000,
                FixedRange = true,
                AddHitbox = true,
                DangerValue = 3,
                IsDangerous = false,
                MissileSpellName = "GravesQLineMis",
                ExtraMissileNames = new[]
                {
                    "GravesQReturn"
                },
                CollisionObjects = new[]
                {
                    CollisionableObjects.YasuoWall
                }
            });

            Spells.Add(new SpellData
            {
                ChampionName = "Graves",
                SpellName = "GravesQLineSpell",
                Slot = SpellSlot.Q,
                Type = SkillshotType.Line,
                Delay = 250,
                Range = 900,
                Radius = 100,
                MissileSpeed = 1600,
                FixedRange = true,
                AddHitbox = true,
                DangerValue = 2,
                IsDangerous = false,
                MissileSpellName = "GravesQReturn"
            });

            Spells.Add(new SpellData
            {
                ChampionName = "Graves",
                SpellName = "GravesSmokeGrenade",
                Slot = SpellSlot.W,
                Type = SkillshotType.Circle,
                Delay = 250,
                Range = 950,
                Radius = 250,
                MissileSpeed = 1000,
                FixedRange = false,
                AddHitbox = true,
                DangerValue = 2,
                IsDangerous = true,
                MissileSpellName = "",
                ExtraDuration = 4000
            });

            Spells.Add(new SpellData
            {
                ChampionName = "Graves",
                SpellName = "GravesChargeShot",
                Slot = SpellSlot.R,
                Type = SkillshotType.Line,
                Delay = 250,
                Range = 1100,
                Radius = 100,
                MissileSpeed = 2100,
                FixedRange = true,
                AddHitbox = true,
                DangerValue = 5,
                IsDangerous = true,
                MissileSpellName = "GravesChargeShotShot",
                CollisionObjects = new[]
                {
                    CollisionableObjects.Heroes,
                    CollisionableObjects.Minions,
                    CollisionableObjects.YasuoWall
                }
            });

            #endregion Graves

            #region Hecarim

            Spells.Add(new SpellData
            {
                ChampionName = "Hecarim",
                SpellName = "HecarimUlt",
                Slot = SpellSlot.R,
                Type = SkillshotType.Line,
                Delay = 250,
                Range = 1000,
                Radius = 400,
                MissileSpeed = 1000,
                FixedRange = false,
                AddHitbox = true,
                DangerValue = 4,
                IsDangerous = true,
                MissileSpellName = "hecarimultmissile",
                ExtraMissileNames = new[]
                {
                    "hecarimultmissileskn4r1",
                    "hecarimultmissileskn4r2",
                    "hecarimultmissileskn411",
                    "hecarimultmissileskn412"
                }
            });

            #endregion

            #region Heimerdinger

            Spells.Add(new SpellData
            {
                ChampionName = "Heimerdinger",
                SpellName = "HeimerdingerTurretEnergyBlast",
                Slot = SpellSlot.Q,
                Type = SkillshotType.Line,
                Delay = 435,
                Range = 1000,
                Radius = 50,
                MissileSpeed = 1650,
                FixedRange = true,
                AddHitbox = true,
                DangerValue = 2,
                IsDangerous = false,
                MissileSpellName = "",
                CollisionObjects = new[]
                {
                    CollisionableObjects.YasuoWall
                }
            });

            Spells.Add(new SpellData
            {
                ChampionName = "Heimerdinger",
                SpellName = "HeimerdingerTurretBigEnergyBlast",
                Slot = SpellSlot.Q,
                Type = SkillshotType.Line,
                Delay = 350,
                Range = 1000,
                Radius = 75,
                MissileSpeed = 1650,
                FixedRange = true,
                AddHitbox = true,
                DangerValue = 3,
                IsDangerous = true,
                MissileSpellName = "",
                CollisionObjects = new[]
                {
                    CollisionableObjects.YasuoWall
                }
            });

            Spells.Add(new SpellData
            {
                ChampionName = "Heimerdinger",
                SpellName = "Heimerdingerwm",
                Slot = SpellSlot.W,
                Type = SkillshotType.Line,
                Delay = 250,
                Range = 1500,
                Radius = 70,
                MissileSpeed = 1800,
                FixedRange = true,
                AddHitbox = true,
                DangerValue = 2,
                IsDangerous = false,
                MissileSpellName = "HeimerdingerWAttack2",
                CollisionObjects = new[]
                {
                    CollisionableObjects.YasuoWall
                }
            });

            Spells.Add(new SpellData
            {
                ChampionName = "Heimerdinger",
                SpellName = "HeimerdingerE",
                Slot = SpellSlot.E,
                Type = SkillshotType.Circle,
                Delay = 250,
                Range = 925,
                Radius = 180,
                MissileSpeed = 1200,
                FixedRange = false,
                AddHitbox = true,
                DangerValue = 2,
                IsDangerous = false,
                MissileSpellName = "heimerdingerespell",
                ExtraMissileNames = new[]
                {
                    "heimerdingerespell_ult",
                    "heimerdingerespell_ult2",
                    "heimerdingerespell_ult3"
                },
                CollisionObjects = new[]
                {
                    CollisionableObjects.YasuoWall
                }
            });

            #endregion

            #region Illaoi

            Spells.Add(new SpellData
            {
                ChampionName = "Illaoi",
                SpellName = "IllaoiQ",
                Slot = SpellSlot.Q,
                Type = SkillshotType.Line,
                Delay = 750,
                Range = 850,
                Radius = 100,
                MissileSpeed = int.MaxValue,
                FixedRange = true,
                AddHitbox = true,
                DangerValue = 3,
                IsDangerous = true,
                MissileSpellName = "illaoiemis",
                CollisionObjects = new[]
                {
                    CollisionableObjects.YasuoWall
                }
            });

            Spells.Add(new SpellData
            {
                ChampionName = "Illaoi",
                SpellName = "IllaoiE",
                Slot = SpellSlot.E,
                Type = SkillshotType.Line,
                Delay = 250,
                Range = 950,
                Radius = 50,
                MissileSpeed = 1900,
                FixedRange = true,
                AddHitbox = true,
                DangerValue = 3,
                IsDangerous = true,
                MissileSpellName = "illaoiemis",
                CollisionObjects = new[]
                {
                    CollisionableObjects.YasuoWall
                }
            });

            Spells.Add(new SpellData
            {
                ChampionName = "Illaoi",
                SpellName = "IllaoiR",
                Slot = SpellSlot.R,
                Type = SkillshotType.Circle,
                Delay = 500,
                Range = 0,
                Radius = 450,
                MissileSpeed = int.MaxValue,
                FixedRange = true,
                AddHitbox = false,
                DangerValue = 3,
                IsDangerous = true,
                MissileSpellName = ""
            });

            #endregion Illaoi

            #region Irelia

            Spells.Add(new SpellData
            {
                ChampionName = "Irelia",
                SpellName = "IreliaTranscendentBlades",
                Slot = SpellSlot.R,
                Type = SkillshotType.Line,
                Delay = 0,
                Range = 1200,
                Radius = 65,
                MissileSpeed = 1600,
                FixedRange = true,
                AddHitbox = true,
                DangerValue = 2,
                IsDangerous = false,
                MissileSpellName = "IreliaTranscendentBlades",
                CollisionObjects = new[]
                {
                    CollisionableObjects.YasuoWall
                }
            });

            #endregion Irelia

            #region Ivern

            Spells.Add(new SpellData
            {
                ChampionName = "Ivern",
                SpellName = "IvernQ",
                Slot = SpellSlot.Q,
                Type = SkillshotType.Line,
                Delay = 250,
                Range = 1100,
                Radius = 65,
                MissileSpeed = 1300,
                FixedRange = true,
                AddHitbox = true,
                DangerValue = 3,
                IsDangerous = true,
                MissileSpellName = "IvernQ",
                CollisionObjects = new[]
                {
                    CollisionableObjects.YasuoWall,
                    CollisionableObjects.Minions,
                    CollisionableObjects.Heroes
                }
            });

            #endregion Ivern

            #region Janna

            Spells.Add(new SpellData
            {
                ChampionName = "Janna",
                SpellName = "JannaQ",
                Slot = SpellSlot.Q,
                Type = SkillshotType.Line,
                Delay = 250,
                Range = 1700,
                Radius = 120,
                MissileSpeed = 900,
                FixedRange = false,
                AddHitbox = true,
                DangerValue = 2,
                IsDangerous = false,
                MissileSpellName = "HowlingGaleSpell",
                CollisionObjects = new[]
                {
                    CollisionableObjects.YasuoWall
                }
            });

            #endregion Janna

            #region JarvanIV

            Spells.Add(new SpellData
            {
                ChampionName = "JarvanIV",
                SpellName = "JarvanIVDragonStrike",
                Slot = SpellSlot.Q,
                Type = SkillshotType.Line,
                Delay = 600,
                Range = 770,
                Radius = 70,
                MissileSpeed = int.MaxValue,
                FixedRange = true,
                AddHitbox = true,
                DangerValue = 3,
                IsDangerous = false
            });

            Spells.Add(new SpellData
            {
                ChampionName = "JarvanIV",
                SpellName = "JarvanIVEQ",
                Slot = SpellSlot.Q,
                Type = SkillshotType.Line,
                Delay = 250,
                Range = 880,
                Radius = 70,
                MissileSpeed = 1450,
                FixedRange = true,
                AddHitbox = true,
                DangerValue = 3,
                IsDangerous = true
            });

            Spells.Add(new SpellData
            {
                ChampionName = "JarvanIV",
                SpellName = "JarvanIVDemacianStandard",
                Slot = SpellSlot.E,
                Type = SkillshotType.Circle,
                Delay = 500,
                Range = 860,
                Radius = 175,
                MissileSpeed = int.MaxValue,
                FixedRange = false,
                AddHitbox = true,
                DangerValue = 2,
                IsDangerous = false,
                MissileSpellName = "JarvanIVDemacianStandard"
            });

            #endregion JarvanIV

            #region Jayce

            Spells.Add(new SpellData
            {
                ChampionName = "Jayce",
                SpellName = "jayceshockblast",
                Slot = SpellSlot.Q,
                Type = SkillshotType.Line,
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
                CollisionObjects = new[]
                {
                    CollisionableObjects.Heroes,
                    CollisionableObjects.Minions,
                    CollisionableObjects.YasuoWall
                }
            });

            Spells.Add(new SpellData
            {
                ChampionName = "Jayce",
                SpellName = "JayceQAccel",
                Slot = SpellSlot.Q,
                Type = SkillshotType.Line,
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
                CollisionObjects = new[]
                {
                    CollisionableObjects.Heroes,
                    CollisionableObjects.Minions,
                    CollisionableObjects.YasuoWall
                }
            });

            #endregion Jayce

            #region Jhin

            Spells.Add(new SpellData
            {
                ChampionName = "Jhin",
                SpellName = "JhinW",
                Slot = SpellSlot.W,
                Type = SkillshotType.Line,
                Delay = 750,
                Range = 2550,
                Radius = 40,
                MissileSpeed = 5000,
                FixedRange = true,
                AddHitbox = true,
                DangerValue = 3,
                IsDangerous = true,
                MissileSpellName = "JhinWMissile",
                CanBeRemoved = true,
                CollisionObjects = new[]
                {
                    CollisionableObjects.Heroes,
                    CollisionableObjects.YasuoWall
                }
            });

            Spells.Add(new SpellData
            {
                ChampionName = "Jhin",
                SpellName = "JhinRShot",
                Slot = SpellSlot.R,
                Type = SkillshotType.Line,
                Delay = 250,
                Range = 3500,
                Radius = 80,
                MissileSpeed = 5000,
                FixedRange = true,
                AddHitbox = true,
                DangerValue = 3,
                IsDangerous = true,
                MissileSpellName = "JhinRShotMis",
                ExtraMissileNames = new[]
                {
                    "JhinRShotMis4"
                },
                CanBeRemoved = true,
                CollisionObjects = new[]
                {
                    CollisionableObjects.Heroes,
                    CollisionableObjects.YasuoWall
                }
            });

            #endregion Jhin

            #region Jinx

            Spells.Add(new SpellData
            {
                ChampionName = "Jinx",
                SpellName = "JinxW",
                Slot = SpellSlot.W,
                Type = SkillshotType.Line,
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
                CollisionObjects = new[]
                {
                    CollisionableObjects.Heroes,
                    CollisionableObjects.Minions,
                    CollisionableObjects.YasuoWall
                }
            });

            Spells.Add(new SpellData
            {
                ChampionName = "Jinx",
                SpellName = "JinxE",
                Slot = SpellSlot.E,
                Type = SkillshotType.Line,
                Delay = 350,
                Range = 850,
                Radius = 65,
                MissileSpeed = int.MaxValue,
                FixedRange = false,
                AddHitbox = true,
                DangerValue = 3,
                IsDangerous = false,
                MissileSpellName = "JinxEMissile",
                ExtraDuration = 5300,
                DontCross = true
            });

            Spells.Add(new SpellData
            {
                ChampionName = "Jinx",
                SpellName = "JinxR",
                Slot = SpellSlot.R,
                Type = SkillshotType.Line,
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
                CollisionObjects = new[]
                {
                    CollisionableObjects.Heroes,
                    CollisionableObjects.YasuoWall
                }
            });

            #endregion Jinx

            #region Kalista

            Spells.Add(new SpellData
            {
                ChampionName = "Kalista",
                SpellName = "KalistaMysticShot",
                Slot = SpellSlot.Q,
                Type = SkillshotType.Line,
                Delay = 250,
                Range = 1200,
                Radius = 40,
                MissileSpeed = 1700,
                FixedRange = true,
                AddHitbox = true,
                DangerValue = 2,
                IsDangerous = false,
                MissileSpellName = "kalistamysticshotmis",
                ExtraMissileNames = new[]
                {
                    "kalistamysticshotmistrue"
                },
                CanBeRemoved = true,
                CollisionObjects = new[]
                {
                    CollisionableObjects.Heroes,
                    CollisionableObjects.Minions,
                    CollisionableObjects.YasuoWall
                }
            });

            #endregion Kalista

            #region Karma

            Spells.Add(new SpellData
            {
                ChampionName = "Karma",
                SpellName = "KarmaQ",
                Slot = SpellSlot.Q,
                Type = SkillshotType.Line,
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
                CollisionObjects = new[]
                {
                    CollisionableObjects.Heroes,
                    CollisionableObjects.Minions,
                    CollisionableObjects.YasuoWall
                }
            });

            Spells.Add(new SpellData
            {
                ChampionName = "Karma",
                SpellName = "KarmaQMantra",
                Slot = SpellSlot.Q,
                Type = SkillshotType.Line,
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
                CollisionObjects = new[]
                {
                    CollisionableObjects.Heroes,
                    CollisionableObjects.Minions,
                    CollisionableObjects.YasuoWall
                }
            });

            #endregion Karma

            #region Karthus

            Spells.Add(new SpellData
            {
                ChampionName = "Karthus",
                SpellName = "KarthusLayWasteA2",
                ExtraSpellNames = new[]
                {
                    "karthuslaywastea3",
                    "karthuslaywastea1",
                    "karthuslaywastedeada1",
                    "karthuslaywastedeada2",
                    "karthuslaywastedeada3"
                },
                Slot = SpellSlot.Q,
                Type = SkillshotType.Circle,
                Delay = 625,
                Range = 875,
                Radius = 160,
                MissileSpeed = int.MaxValue,
                FixedRange = false,
                AddHitbox = true,
                DangerValue = 2,
                IsDangerous = false,
                MissileSpellName = ""
            });

            #endregion Karthus

            #region Kassadin

            Spells.Add(new SpellData
            {
                ChampionName = "Kassadin",
                SpellName = "ForcePulse",
                Slot = SpellSlot.E,
                Type = SkillshotType.Cone,
                Delay = 400,
                Range = 700,
                Radius = 80,
                MissileSpeed = int.MaxValue,
                FixedRange = true,
                AddHitbox = true,
                DangerValue = 2,
                IsDangerous = false,
                MissileSpellName = ""
            });

            Spells.Add(new SpellData
            {
                ChampionName = "Kassadin",
                SpellName = "RiftWalk",
                Slot = SpellSlot.R,
                Type = SkillshotType.Circle,
                Delay = 250,
                Range = 450,
                Radius = 270,
                MissileSpeed = int.MaxValue,
                FixedRange = false,
                AddHitbox = true,
                DangerValue = 2,
                IsDangerous = false,
                MissileSpellName = "RiftWalk"
            });

            #endregion Kassadin

            #region Kennen

            Spells.Add(new SpellData
            {
                ChampionName = "Kennen",
                SpellName = "KennenShurikenHurlMissile1",
                Slot = SpellSlot.Q,
                Type = SkillshotType.Line,
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
                CollisionObjects = new[]
                {
                    CollisionableObjects.Heroes,
                    CollisionableObjects.Minions,
                    CollisionableObjects.YasuoWall
                }
            });

            #endregion Kennen

            #region Khazix

            Spells.Add(new SpellData
            {
                ChampionName = "Khazix",
                SpellName = "KhazixW",
                ExtraSpellNames = new[]
                {
                    "khazixwlong"
                },
                Slot = SpellSlot.W,
                Type = SkillshotType.Line,
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
                CollisionObjects = new[]
                {
                    CollisionableObjects.Heroes,
                    CollisionableObjects.Minions,
                    CollisionableObjects.YasuoWall
                }
            });

            Spells.Add(new SpellData
            {
                ChampionName = "Khazix",
                SpellName = "KhazixE",
                Slot = SpellSlot.E,
                Type = SkillshotType.Circle,
                Delay = 250,
                Range = 600,
                Radius = 300,
                MissileSpeed = 1500,
                FixedRange = false,
                AddHitbox = true,
                DangerValue = 2,
                IsDangerous = false,
                MissileSpellName = "KhazixE"
            });

            #endregion Khazix

            #region Kled

            Spells.Add(new SpellData
            {
                ChampionName = "Kled",
                SpellName = "KledQ",
                Slot = SpellSlot.Q,
                Type = SkillshotType.Line,
                Delay = 250,
                Range = 800,
                Radius = 45,
                MissileSpeed = 1600,
                FixedRange = true,
                AddHitbox = true,
                DangerValue = 2,
                IsDangerous = true,
                MissileSpellName = "KledQMissile",
                CanBeRemoved = true,
                CollisionObjects = new[]
                {
                    CollisionableObjects.Heroes,
                    CollisionableObjects.YasuoWall
                }
            });

            Spells.Add(new SpellData
            {
                ChampionName = "Kled",
                SpellName = "KledE",
                Slot = SpellSlot.E,
                Type = SkillshotType.Line,
                Delay = 0,
                Range = 750,
                Radius = 125,
                MissileSpeed = 945,
                FixedRange = true,
                AddHitbox = true,
                DangerValue = 2,
                IsDangerous = true,
                MissileSpellName = "",
                CanBeRemoved = true,
                CollisionObjects = new[]
                {
                    CollisionableObjects.Heroes,
                    CollisionableObjects.YasuoWall
                }
            });

            Spells.Add(new SpellData
            {
                ChampionName = "Kled",
                SpellName = "KledRiderQ",
                Slot = SpellSlot.Q,
                Type = SkillshotType.Line,
                Delay = 250,
                Range = 700,
                Radius = 40,
                MissileSpeed = 3000,
                FixedRange = true,
                AddHitbox = true,
                DangerValue = 2,
                IsDangerous = false,
                MissileSpellName = "KledRiderQMissile",
                MultipleNumber = 5,
                MultipleAngle = 5 * (float) Math.PI / 180,
                CanBeRemoved = true,
                CollisionObjects = new[]
                {
                    CollisionableObjects.YasuoWall
                }
            });

            #endregion Kled

            #region Kogmaw

            Spells.Add(new SpellData
            {
                ChampionName = "Kogmaw",
                SpellName = "KogMawQ",
                Slot = SpellSlot.Q,
                Type = SkillshotType.Line,
                Delay = 250,
                Range = 1200,
                Radius = 70,
                MissileSpeed = 1650,
                FixedRange = true,
                AddHitbox = true,
                DangerValue = 2,
                IsDangerous = false,
                MissileSpellName = "KogMawQ",
                CanBeRemoved = true,
                CollisionObjects = new[]
                {
                    CollisionableObjects.Heroes,
                    CollisionableObjects.Minions,
                    CollisionableObjects.YasuoWall
                }
            });

            Spells.Add(new SpellData
            {
                ChampionName = "Kogmaw",
                SpellName = "KogMawVoidOoze",
                Slot = SpellSlot.E,
                Type = SkillshotType.Line,
                Delay = 250,
                Range = 1360,
                Radius = 120,
                MissileSpeed = 1400,
                FixedRange = true,
                AddHitbox = true,
                DangerValue = 2,
                IsDangerous = false,
                MissileSpellName = "KogMawVoidOozeMissile",
                CollisionObjects = new[]
                {
                    CollisionableObjects.YasuoWall
                }
            });

            Spells.Add(new SpellData
            {
                ChampionName = "Kogmaw",
                SpellName = "KogMawLivingArtillery",
                Slot = SpellSlot.R,
                Type = SkillshotType.Circle,
                Delay = 1200,
                Range = 1800,
                Radius = 225,
                MissileSpeed = int.MaxValue,
                FixedRange = false,
                AddHitbox = true,
                DangerValue = 2,
                IsDangerous = false,
                MissileSpellName = "KogMawLivingArtillery"
            });

            #endregion Kogmaw

            #region Leblanc

            Spells.Add(new SpellData
            {
                ChampionName = "Leblanc",
                SpellName = "LeblancW",
                Slot = SpellSlot.W,
                Type = SkillshotType.Circle,
                Delay = 0,
                Range = 600,
                Radius = 220,
                MissileSpeed = 1450,
                FixedRange = false,
                AddHitbox = true,
                DangerValue = 2,
                IsDangerous = false,
                MissileSpellName = "LeblancW"
            });

            Spells.Add(new SpellData
            {
                ChampionName = "Leblanc",
                SpellName = "LeblancSlideM",
                Slot = SpellSlot.R,
                Type = SkillshotType.Circle,
                Delay = 0,
                Range = 600,
                Radius = 220,
                MissileSpeed = 1450,
                FixedRange = false,
                AddHitbox = true,
                DangerValue = 2,
                IsDangerous = false,
                MissileSpellName = "LeblancSlideM"
            });

            Spells.Add(new SpellData
            {
                ChampionName = "Leblanc",
                SpellName = "LeblancE",
                Slot = SpellSlot.E,
                Type = SkillshotType.Line,
                Delay = 250,
                Range = 950,
                Radius = 70,
                MissileSpeed = 1600,
                FixedRange = true,
                AddHitbox = true,
                DangerValue = 3,
                IsDangerous = true,
                MissileSpellName = "LeblancE",
                CanBeRemoved = true,
                CollisionObjects = new[]
                {
                    CollisionableObjects.Heroes,
                    CollisionableObjects.Minions,
                    CollisionableObjects.YasuoWall
                }
            });

            Spells.Add(new SpellData
            {
                ChampionName = "Leblanc",
                SpellName = "LeblancEM",
                Slot = SpellSlot.R,
                Type = SkillshotType.Line,
                Delay = 250,
                Range = 950,
                Radius = 70,
                MissileSpeed = 1600,
                FixedRange = true,
                AddHitbox = true,
                DangerValue = 3,
                IsDangerous = true,
                MissileSpellName = "LeblancEM",
                CanBeRemoved = true,
                CollisionObjects = new[]
                {
                    CollisionableObjects.Heroes,
                    CollisionableObjects.Minions,
                    CollisionableObjects.YasuoWall
                }
            });

            #endregion Leblanc

            #region LeeSin

            Spells.Add(new SpellData
            {
                ChampionName = "LeeSin",
                SpellName = "BlindMonkQOne",
                Slot = SpellSlot.Q,
                Type = SkillshotType.Line,
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
                CollisionObjects = new[]
                {
                    CollisionableObjects.Heroes,
                    CollisionableObjects.Minions,
                    CollisionableObjects.YasuoWall
                }
            });

            #endregion LeeSin

            #region Leona

            Spells.Add(new SpellData
            {
                ChampionName = "Leona",
                SpellName = "LeonaZenithBlade",
                Slot = SpellSlot.E,
                Type = SkillshotType.Line,
                Delay = 250,
                Range = 905,
                Radius = 70,
                MissileSpeed = 2000,
                FixedRange = true,
                AddHitbox = true,
                DangerValue = 3,
                IsDangerous = true,
                TakeClosestPath = true,
                MissileSpellName = "LeonaZenithBladeMissile",
                CollisionObjects = new[]
                {
                    CollisionableObjects.YasuoWall
                }
            });

            Spells.Add(new SpellData
            {
                ChampionName = "Leona",
                SpellName = "LeonaSolarFlare",
                Slot = SpellSlot.R,
                Type = SkillshotType.Circle,
                Delay = 1000,
                Range = 1200,
                Radius = 300,
                MissileSpeed = int.MaxValue,
                FixedRange = false,
                AddHitbox = true,
                DangerValue = 5,
                IsDangerous = true,
                MissileSpellName = "LeonaSolarFlare"
            });

            #endregion Leona

            #region Lissandra

            Spells.Add(new SpellData
            {
                ChampionName = "Lissandra",
                SpellName = "LissandraQ",
                Slot = SpellSlot.Q,
                Type = SkillshotType.Line,
                Delay = 250,
                Range = 825,
                Radius = 75,
                MissileSpeed = 2200,
                FixedRange = true,
                AddHitbox = true,
                DangerValue = 2,
                IsDangerous = false,
                MissileSpellName = "LissandraQMissile",
                CollisionObjects = new[]
                {
                    CollisionableObjects.YasuoWall
                }
            });

            Spells.Add(new SpellData
            {
                ChampionName = "Lissandra",
                SpellName = "LissandraQShards",
                Slot = SpellSlot.Q,
                Type = SkillshotType.Line,
                Delay = 250,
                Range = 825,
                Radius = 90,
                MissileSpeed = 2200,
                FixedRange = true,
                AddHitbox = true,
                DangerValue = 2,
                IsDangerous = false,
                MissileSpellName = "lissandraqshards",
                CollisionObjects = new[]
                {
                    CollisionableObjects.YasuoWall
                }
            });

            Spells.Add(new SpellData
            {
                ChampionName = "Lissandra",
                SpellName = "LissandraW",
                Slot = SpellSlot.W,
                Type = SkillshotType.Circle,
                Delay = 250,
                Range = 725,
                Radius = 450,
                MissileSpeed = int.MaxValue,
                FixedRange = true,
                AddHitbox = true,
                DangerValue = 3,
                IsDangerous = true
            });

            Spells.Add(new SpellData
            {
                ChampionName = "Lissandra",
                SpellName = "LissandraE",
                Slot = SpellSlot.E,
                Type = SkillshotType.Line,
                Delay = 250,
                Range = 1025,
                Radius = 125,
                MissileSpeed = 850,
                FixedRange = true,
                AddHitbox = true,
                DangerValue = 2,
                IsDangerous = false,
                MissileSpellName = "LissandraEMissile",
                CollisionObjects = new[]
                {
                    CollisionableObjects.YasuoWall
                }
            });

            #endregion Lulu

            #region Lucian

            Spells.Add(new SpellData
            {
                ChampionName = "Lucian",
                SpellName = "LucianQ",
                Slot = SpellSlot.Q,
                Type = SkillshotType.Line,
                Delay = 500,
                Range = 1300,
                Radius = 65,
                MissileSpeed = int.MaxValue,
                FixedRange = true,
                AddHitbox = true,
                DangerValue = 2,
                IsDangerous = false,
                MissileSpellName = "LucianQ"
            });

            Spells.Add(new SpellData
            {
                ChampionName = "Lucian",
                SpellName = "LucianW",
                Slot = SpellSlot.W,
                Type = SkillshotType.Line,
                Delay = 250,
                Range = 1000,
                Radius = 55,
                MissileSpeed = 1600,
                FixedRange = true,
                AddHitbox = true,
                DangerValue = 2,
                IsDangerous = false,
                MissileSpellName = "lucianwmissile"
            });

            Spells.Add(new SpellData
            {
                ChampionName = "Lucian",
                SpellName = "LucianRMis",
                Slot = SpellSlot.R,
                Type = SkillshotType.Line,
                Delay = 500,
                Range = 1400,
                Radius = 110,
                MissileSpeed = 2800,
                FixedRange = true,
                AddHitbox = true,
                DangerValue = 2,
                IsDangerous = false,
                MissileSpellName = "lucianrmissileoffhand",
                ExtraMissileNames = new[]
                {
                    "lucianrmissile"
                },
                DontCheckForDuplicates = true,
                DisabledByDefault = true
            });

            #endregion Lucian

            #region Lulu

            Spells.Add(new SpellData
            {
                ChampionName = "Lulu",
                SpellName = "LuluQ",
                Slot = SpellSlot.Q,
                Type = SkillshotType.Line,
                Delay = 250,
                Range = 950,
                Radius = 60,
                MissileSpeed = 1450,
                FixedRange = true,
                AddHitbox = true,
                DangerValue = 2,
                IsDangerous = false,
                MissileSpellName = "LuluQMissile",
                CollisionObjects = new[]
                {
                    CollisionableObjects.YasuoWall
                }
            });

            Spells.Add(new SpellData
            {
                ChampionName = "Lulu",
                SpellName = "LuluQPix",
                Slot = SpellSlot.Q,
                Type = SkillshotType.Line,
                Delay = 250,
                Range = 950,
                Radius = 60,
                MissileSpeed = 1450,
                FixedRange = true,
                AddHitbox = true,
                DangerValue = 2,
                IsDangerous = false,
                MissileSpellName = "LuluQMissileTwo",
                CollisionObjects = new[]
                {
                    CollisionableObjects.YasuoWall
                }
            });

            #endregion Lulu

            #region Lux

            Spells.Add(new SpellData
            {
                ChampionName = "Lux",
                SpellName = "LuxLightBinding",
                Slot = SpellSlot.Q,
                Type = SkillshotType.Line,
                Delay = 250,
                Range = 1300,
                Radius = 70,
                MissileSpeed = 1200,
                FixedRange = true,
                AddHitbox = true,
                DangerValue = 3,
                IsDangerous = true,
                MissileSpellName = "LuxLightBindingMis"
            });

            Spells.Add(new SpellData
            {
                ChampionName = "Lux",
                SpellName = "LuxLightStrikeKugel",
                Slot = SpellSlot.E,
                Type = SkillshotType.Circle,
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
                ToggleParticleName = "Lux_.+_E_tar_aoe_",
                DontCross = true,
                CanBeRemoved = true,
                DisabledByDefault = true,
                CollisionObjects = new[]
                {
                    CollisionableObjects.YasuoWall
                }
            });

            Spells.Add(new SpellData
            {
                ChampionName = "Lux",
                SpellName = "LuxMaliceCannon",
                Slot = SpellSlot.R,
                Type = SkillshotType.Line,
                Delay = 1000,
                Range = 3500,
                Radius = 190,
                MissileSpeed = int.MaxValue,
                FixedRange = true,
                AddHitbox = true,
                DangerValue = 5,
                IsDangerous = true,
                MissileSpellName = "LuxMaliceCannon"
            });

            #endregion Lux

            #region Malphite

            Spells.Add(new SpellData
            {
                ChampionName = "Malphite",
                SpellName = "UFSlash",
                Slot = SpellSlot.R,
                Type = SkillshotType.Circle,
                Delay = 0,
                Range = 1000,
                Radius = 270,
                MissileSpeed = 1500,
                FixedRange = false,
                AddHitbox = true,
                DangerValue = 5,
                IsDangerous = true,
                MissileSpellName = "UFSlash"
            });

            #endregion Malphite

            #region Malzahar

            Spells.Add(new SpellData
            {
                ChampionName = "Malzahar",
                SpellName = "MalzaharQ",
                Slot = SpellSlot.Q,
                Type = SkillshotType.Line,
                Delay = 750,
                Range = 900,
                Radius = 85,
                MissileSpeed = int.MaxValue,
                FixedRange = false,
                AddHitbox = true,
                DangerValue = 2,
                IsDangerous = false,
                DontCross = true,
                MissileSpellName = "MalzaharQ"
            });

            #endregion Malzahar

            #region Maokai

            Spells.Add(new SpellData
            {
                ChampionName = "Maokai",
                SpellName = "MaokaiTrunkLine",
                Slot = SpellSlot.Q,
                Type = SkillshotType.Line,
                Delay = 400,
                Range = 650,
                Radius = 110 + 360,
                MissileSpeed = 1800,
                FixedRange = true,
                AddHitbox = true,
                DangerValue = 3,
                IsDangerous = true,
                MissileSpellName = "MaokaiTrunkLineMissile",
                CanBeRemoved = true
            });

            #endregion Maokai

            #region Morgana

            Spells.Add(new SpellData
            {
                ChampionName = "Morgana",
                SpellName = "DarkBindingMissile",
                Slot = SpellSlot.Q,
                Type = SkillshotType.Line,
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
                CollisionObjects = new[]
                {
                    CollisionableObjects.Heroes,
                    CollisionableObjects.Minions,
                    CollisionableObjects.YasuoWall
                }
            });

            #endregion Morgana

            #region Nami

            Spells.Add(new SpellData
            {
                ChampionName = "Nami",
                SpellName = "NamiQ",
                Slot = SpellSlot.Q,
                Type = SkillshotType.Circle,
                Delay = 950,
                Range = 1625,
                Radius = 150,
                MissileSpeed = int.MaxValue,
                FixedRange = false,
                AddHitbox = true,
                DangerValue = 3,
                IsDangerous = true,
                MissileSpellName = "namiqmissile"
            });

            Spells.Add(new SpellData
            {
                ChampionName = "Nami",
                SpellName = "NamiR",
                Slot = SpellSlot.R,
                Type = SkillshotType.Line,
                Delay = 500,
                Range = 2750,
                Radius = 260,
                MissileSpeed = 850,
                FixedRange = true,
                AddHitbox = true,
                DangerValue = 2,
                IsDangerous = false,
                MissileSpellName = "NamiRMissile",
                CollisionObjects = new[]
                {
                    CollisionableObjects.YasuoWall
                }
            });

            #endregion Nami

            #region Nautilus

            Spells.Add(new SpellData
            {
                ChampionName = "Nautilus",
                SpellName = "NautilusAnchorDrag",
                Slot = SpellSlot.Q,
                Type = SkillshotType.Line,
                Delay = 250,
                Range = 1250,
                Radius = 90,
                MissileSpeed = 2000,
                FixedRange = true,
                AddHitbox = true,
                DangerValue = 3,
                IsDangerous = true,
                MissileSpellName = "NautilusAnchorDragMissile",
                CanBeRemoved = true,
                CollisionObjects = new[]
                {
                    CollisionableObjects.Heroes,
                    CollisionableObjects.Minions,
                    CollisionableObjects.YasuoWall
                }
            });

            #endregion Nautilus

            #region Nocturne

            Spells.Add(new SpellData
            {
                ChampionName = "Nocturne",
                SpellName = "NocturneDuskbringer",
                Slot = SpellSlot.Q,
                Type = SkillshotType.Line,
                Delay = 250,
                Range = 1125,
                Radius = 60,
                MissileSpeed = 1400,
                DangerValue = 2,
                IsDangerous = false,
                MissileSpellName = "NocturneDuskbringer",
                CollisionObjects = new[]
                {
                    CollisionableObjects.YasuoWall
                }
            });

            #endregion Nocturne

            #region Nidalee

            Spells.Add(new SpellData
            {
                ChampionName = "Nidalee",
                SpellName = "JavelinToss",
                Slot = SpellSlot.Q,
                Type = SkillshotType.Line,
                Delay = 250,
                Range = 1500,
                Radius = 40,
                MissileSpeed = 1300,
                FixedRange = true,
                AddHitbox = true,
                DangerValue = 3,
                IsDangerous = true,
                MissileSpellName = "JavelinToss",
                CanBeRemoved = true,
                CollisionObjects = new[]
                {
                    CollisionableObjects.Heroes,
                    CollisionableObjects.Minions,
                    CollisionableObjects.YasuoWall
                }
            });

            #endregion Nidalee

            #region Olaf

            Spells.Add(new SpellData
            {
                ChampionName = "Olaf",
                SpellName = "OlafAxeThrowCast",
                Slot = SpellSlot.Q,
                Type = SkillshotType.Line,
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
                CollisionObjects = new[]
                {
                    CollisionableObjects.YasuoWall
                }
            });

            #endregion Olaf

            #region Orianna

            Spells.Add(new SpellData
            {
                ChampionName = "Orianna",
                SpellName = "OriannasQ",
                Slot = SpellSlot.Q,
                Type = SkillshotType.Line,
                Delay = 0,
                Range = 1500,
                Radius = 80,
                MissileSpeed = 1200,
                FixedRange = false,
                AddHitbox = true,
                DangerValue = 2,
                IsDangerous = false,
                MissileSpellName = "orianaizuna",
                CollisionObjects = new[]
                {
                    CollisionableObjects.YasuoWall
                }
            });

            Spells.Add(new SpellData
            {
                ChampionName = "Orianna",
                SpellName = "OriannaQend",
                Slot = SpellSlot.Q,
                Type = SkillshotType.Circle,
                Delay = 0,
                Range = 1500,
                Radius = 90,
                MissileSpeed = 1200,
                FixedRange = false,
                AddHitbox = true,
                DangerValue = 2,
                IsDangerous = false,
                MissileSpellName = "",
                CollisionObjects = new[]
                {
                    CollisionableObjects.YasuoWall
                }
            });

            Spells.Add(new SpellData
            {
                ChampionName = "Orianna",
                SpellName = "OrianaDissonanceCommand-",
                Slot = SpellSlot.W,
                Type = SkillshotType.Circle,
                Delay = 250,
                Range = 0,
                Radius = 255,
                MissileSpeed = int.MaxValue,
                FixedRange = true,
                AddHitbox = true,
                DangerValue = 2,
                IsDangerous = false,
                MissileSpellName = "OrianaDissonanceCommand-",
                FromObject = "yomu_ring_",
                SourceObjectName = "w_dissonance_ball" //Orianna_Base_W_Dissonance_ball_green.troy & Orianna_Base_W_Dissonance_cas_green.troy
            });

            Spells.Add(new SpellData
            {
                ChampionName = "Orianna",
                SpellName = "OriannasE",
                Slot = SpellSlot.E,
                Type = SkillshotType.Line,
                Delay = 0,
                Range = 1500,
                Radius = 85,
                MissileSpeed = 1850,
                FixedRange = false,
                AddHitbox = true,
                DangerValue = 2,
                IsDangerous = false,
                MissileSpellName = "orianaredact",
                CollisionObjects = new[]
                {
                    CollisionableObjects.YasuoWall
                }
            });

            Spells.Add(new SpellData
            {
                ChampionName = "Orianna",
                SpellName = "OrianaDetonateCommand-",
                Slot = SpellSlot.R,
                Type = SkillshotType.Circle,
                Delay = 700,
                Range = 0,
                Radius = 410,
                MissileSpeed = int.MaxValue,
                FixedRange = true,
                AddHitbox = true,
                DangerValue = 5,
                IsDangerous = true,
                MissileSpellName = "OrianaDetonateCommand-",
                FromObject = "yomu_ring_",
                SourceObjectName = "r_vacuumindicator" //Orianna_Base_R_VacuumIndicator.troy
            });

            #endregion Orianna

            #region Quinn

            Spells.Add(new SpellData
            {
                ChampionName = "Quinn",
                SpellName = "QuinnQ",
                Slot = SpellSlot.Q,
                Type = SkillshotType.Line,
                Delay = 313,
                Range = 1050,
                Radius = 60,
                MissileSpeed = 1550,
                FixedRange = true,
                AddHitbox = true,
                DangerValue = 2,
                IsDangerous = false,
                MissileSpellName = "QuinnQ",
                CanBeRemoved = true,
                CollisionObjects = new[]
                {
                    CollisionableObjects.Heroes,
                    CollisionableObjects.Minions,
                    CollisionableObjects.YasuoWall
                }
            });

            #endregion Quinn

            #region Pantheon

            Spells.Add(new SpellData
            {
                ChampionName = "Pantheon",
                SpellName = "PantheonE",
                Slot = SpellSlot.E,
                Type = SkillshotType.Cone,
                Delay = 250,
                Range = 600,
                Radius = 50,
                MissileSpeed = 1400,
                FixedRange = true,
                AddHitbox = true,
                DangerValue = 2,
                IsDangerous = false,
                ExtraDuration = 750
            });

            #endregion

            #region Poppy

            Spells.Add(new SpellData
            {
                ChampionName = "Poppy",
                SpellName = "PoppyQ",
                Slot = SpellSlot.Q,
                Type = SkillshotType.Line,
                Delay = 500,
                Range = 430,
                Radius = 100,
                MissileSpeed = int.MaxValue,
                FixedRange = true,
                AddHitbox = true,
                DangerValue = 2,
                IsDangerous = false,
                MissileSpellName = "PoppyQ"
            });

            Spells.Add(new SpellData
            {
                ChampionName = "Poppy",
                SpellName = "PoppyRSpell",
                Slot = SpellSlot.R,
                Type = SkillshotType.Line,
                Delay = 300,
                Range = 1200,
                Radius = 100,
                MissileSpeed = 1600,
                FixedRange = true,
                AddHitbox = true,
                DangerValue = 3,
                IsDangerous = true,
                MissileSpellName = "PoppyRMissile",
                CanBeRemoved = true,
                CollisionObjects = new[]
                {
                    CollisionableObjects.Heroes,
                    CollisionableObjects.Minions,
                    CollisionableObjects.YasuoWall
                }
            });

            #endregion Poppy

            #region Rengar

            Spells.Add(new SpellData
            {
                ChampionName = "Rengar",
                SpellName = "RengarE",
                Slot = SpellSlot.E,
                Type = SkillshotType.Line,
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
                CollisionObjects = new[]
                {
                    CollisionableObjects.Heroes,
                    CollisionableObjects.Minions,
                    CollisionableObjects.YasuoWall
                }
            });

            #endregion Rengar

            #region RekSai

            Spells.Add(new SpellData
            {
                ChampionName = "RekSai",
                SpellName = "reksaiqburrowed",
                Slot = SpellSlot.Q,
                Type = SkillshotType.Line,
                Delay = 500,
                Range = 1625,
                Radius = 60,
                MissileSpeed = 1950,
                FixedRange = true,
                AddHitbox = true,
                DangerValue = 3,
                IsDangerous = false,
                MissileSpellName = "RekSaiQBurrowedMis",
                CollisionObjects = new[]
                {
                    CollisionableObjects.Heroes,
                    CollisionableObjects.Minions,
                    CollisionableObjects.YasuoWall
                }
            });

            #endregion RekSai

            #region Riven

            Spells.Add(new SpellData
            {
                ChampionName = "Riven",
                SpellName = "rivenizunablade",
                Slot = SpellSlot.R,
                Type = SkillshotType.Line,
                Delay = 250,
                Range = 1100,
                Radius = 125,
                MissileSpeed = 1600,
                FixedRange = false,
                AddHitbox = false,
                DangerValue = 5,
                IsDangerous = true,
                MultipleNumber = 3,
                MultipleAngle = 15 * (float) Math.PI / 180,
                MissileSpellName = "RivenLightsaberMissile",
                ExtraMissileNames = new[]
                {
                    "RivenLightsaberMissileSide"
                }
            });

            #endregion Riven

            #region Rumble

            Spells.Add(new SpellData
            {
                ChampionName = "Rumble",
                SpellName = "RumbleGrenade",
                Slot = SpellSlot.E,
                Type = SkillshotType.Line,
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
                CollisionObjects = new[]
                {
                    CollisionableObjects.Heroes,
                    CollisionableObjects.Minions,
                    CollisionableObjects.YasuoWall
                }
            });

            Spells.Add(new SpellData
            {
                ChampionName = "Rumble",
                SpellName = "RumbleCarpetBombM",
                Slot = SpellSlot.R,
                Type = SkillshotType.Line,
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
                CanBeRemoved = false
            });

            #endregion Rumble

            #region Ryze

            Spells.Add(new SpellData
            {
                ChampionName = "Ryze",
                SpellName = "RyzeQ",
                Slot = SpellSlot.Q,
                Type = SkillshotType.Line,
                Delay = 250,
                Range = 1000,
                Radius = 55,
                MissileSpeed = 1700,
                FixedRange = true,
                AddHitbox = true,
                DangerValue = 2,
                IsDangerous = false,
                MissileSpellName = "RyzeQ",
                CanBeRemoved = true,
                CollisionObjects = new[]
                {
                    CollisionableObjects.Heroes,
                    CollisionableObjects.Minions,
                    CollisionableObjects.YasuoWall
                }
            });

            #endregion

            #region Sejuani

            Spells.Add(new SpellData
            {
                ChampionName = "Sejuani",
                SpellName = "SejuaniArcticAssault",
                Slot = SpellSlot.Q,
                Type = SkillshotType.Line,
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
                CollisionObjects = new[]
                {
                    CollisionableObjects.Heroes,
                    CollisionableObjects.Minions,
                    CollisionableObjects.YasuoWall
                }
            });

            Spells.Add(new SpellData
            {
                ChampionName = "Sejuani",
                SpellName = "SejuaniGlacialPrisonStart",
                Slot = SpellSlot.R,
                Type = SkillshotType.Line,
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
                CollisionObjects = new[]
                {
                    CollisionableObjects.Heroes,
                    CollisionableObjects.YasuoWall
                }
            });

            #endregion Sejuani

            #region Sion

            Spells.Add(new SpellData
            {
                ChampionName = "Sion",
                SpellName = "SionE",
                Slot = SpellSlot.E,
                Type = SkillshotType.Line,
                Delay = 250,
                Range = 800,
                Radius = 80,
                MissileSpeed = 1800,
                FixedRange = true,
                AddHitbox = true,
                DangerValue = 3,
                IsDangerous = true,
                MissileSpellName = "SionEMissile",
                CollisionObjects = new[]
                {
                    CollisionableObjects.Heroes,
                    CollisionableObjects.YasuoWall
                }
            });

            Spells.Add(new SpellData
            {
                ChampionName = "Sion",
                SpellName = "SionR",
                Slot = SpellSlot.R,
                Type = SkillshotType.Line,
                Delay = 500,
                Range = 800,
                Radius = 120,
                MissileSpeed = 1000,
                FixedRange = true,
                AddHitbox = true,
                DangerValue = 3,
                IsDangerous = true,
                CollisionObjects = new[]
                {
                    CollisionableObjects.Heroes
                }
            });

            #endregion Sion

            #region Soraka

            Spells.Add(new SpellData
            {
                ChampionName = "Soraka",
                SpellName = "SorakaQ",
                Slot = SpellSlot.Q,
                Type = SkillshotType.Circle,
                Delay = 500,
                Range = 950,
                Radius = 300,
                MissileSpeed = 1750,
                FixedRange = false,
                AddHitbox = true,
                DangerValue = 2,
                IsDangerous = false,
                MissileSpellName = "",
                CollisionObjects = new[]
                {
                    CollisionableObjects.YasuoWall
                }
            });

            #endregion Soraka

            #region Shen

            Spells.Add(new SpellData
            {
                ChampionName = "Shen",
                SpellName = "ShenE",
                Slot = SpellSlot.E,
                Type = SkillshotType.Line,
                Delay = 0,
                Range = 650,
                Radius = 50,
                MissileSpeed = 1600,
                FixedRange = false,
                AddHitbox = true,
                DangerValue = 3,
                IsDangerous = true,
                MissileSpellName = "ShenE",
                ExtraRange = 200,
                CollisionObjects = new[]
                {
                    CollisionableObjects.Heroes,
                    CollisionableObjects.Minions,
                    CollisionableObjects.YasuoWall
                }
            });

            #endregion Shen

            #region Shyvana

            Spells.Add(new SpellData
            {
                ChampionName = "Shyvana",
                SpellName = "ShyvanaFireball",
                Slot = SpellSlot.E,
                Type = SkillshotType.Line,
                Delay = 250,
                Range = 950,
                Radius = 60,
                MissileSpeed = 1700,
                FixedRange = true,
                AddHitbox = true,
                DangerValue = 2,
                IsDangerous = false,
                MissileSpellName = "ShyvanaFireballMissile",
                CollisionObjects = new[]
                {
                    CollisionableObjects.Heroes,
                    CollisionableObjects.Minions,
                    CollisionableObjects.YasuoWall
                }
            });

            Spells.Add(new SpellData
            {
                ChampionName = "Shyvana",
                SpellName = "ShyvanaTransformCast",
                Slot = SpellSlot.R,
                Type = SkillshotType.Line,
                Delay = 250,
                Range = 1000,
                Radius = 150,
                MissileSpeed = 1500,
                FixedRange = true,
                AddHitbox = true,
                DangerValue = 3,
                IsDangerous = true,
                MissileSpellName = "ShyvanaTransformCast",
                ExtraRange = 200
            });

            Spells.Add(new SpellData
            {
                ChampionName = "Shyvana",
                SpellName = "shyvanafireballdragon2",
                Slot = SpellSlot.R,
                Type = SkillshotType.Line,
                Delay = 250,
                Range = 850,
                Radius = 70,
                MissileSpeed = 2000,
                FixedRange = true,
                AddHitbox = true,
                DangerValue = 3,
                IsDangerous = false,
                MissileSpellName = "ShyvanaFireballDragonFxMissile",
                ExtraRange = 200,
                MultipleNumber = 5,
                MultipleAngle = 10 * (float) Math.PI / 180
            });

            #endregion Shyvana

            #region Sivir

            Spells.Add(new SpellData
            {
                ChampionName = "Sivir",
                SpellName = "SivirQReturn",
                Slot = SpellSlot.Q,
                Type = SkillshotType.Line,
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
                CollisionObjects = new[]
                {
                    CollisionableObjects.YasuoWall
                }
            });

            Spells.Add(new SpellData
            {
                ChampionName = "Sivir",
                SpellName = "SivirQ",
                Slot = SpellSlot.Q,
                Type = SkillshotType.Line,
                Delay = 250,
                Range = 1250,
                Radius = 90,
                MissileSpeed = 1350,
                FixedRange = true,
                AddHitbox = true,
                DangerValue = 2,
                IsDangerous = false,
                MissileSpellName = "SivirQMissile",
                CollisionObjects = new[]
                {
                    CollisionableObjects.YasuoWall
                }
            });

            #endregion Sivir

            #region Skarner

            Spells.Add(new SpellData
            {
                ChampionName = "Skarner",
                SpellName = "SkarnerFracture",
                Slot = SpellSlot.E,
                Type = SkillshotType.Line,
                Delay = 250,
                Range = 1000,
                Radius = 70,
                MissileSpeed = 1500,
                FixedRange = true,
                AddHitbox = true,
                DangerValue = 2,
                IsDangerous = false,
                MissileSpellName = "SkarnerFractureMissile",
                CollisionObjects = new[]
                {
                    CollisionableObjects.YasuoWall
                }
            });

            #endregion Skarner

            #region Sona

            Spells.Add(new SpellData
            {
                ChampionName = "Sona",
                SpellName = "SonaR",
                Slot = SpellSlot.R,
                Type = SkillshotType.Line,
                Delay = 250,
                Range = 1000,
                Radius = 140,
                MissileSpeed = 2400,
                FixedRange = true,
                AddHitbox = true,
                DangerValue = 5,
                IsDangerous = true,
                MissileSpellName = "SonaR",
                CollisionObjects = new[]
                {
                    CollisionableObjects.YasuoWall
                }
            });

            #endregion Sona

            #region Swain

            Spells.Add(new SpellData
            {
                ChampionName = "Swain",
                SpellName = "SwainShadowGrasp",
                Slot = SpellSlot.W,
                Type = SkillshotType.Circle,
                Delay = 1100,
                Range = 900,
                Radius = 180,
                MissileSpeed = int.MaxValue,
                FixedRange = false,
                AddHitbox = true,
                DangerValue = 3,
                IsDangerous = true,
                MissileSpellName = "SwainShadowGrasp"
            });

            #endregion Swain

            #region Syndra

            Spells.Add(new SpellData
            {
                ChampionName = "Syndra",
                SpellName = "SyndraQ",
                Slot = SpellSlot.Q,
                Type = SkillshotType.Circle,
                Delay = 600,
                Range = 800,
                Radius = 150,
                MissileSpeed = int.MaxValue,
                FixedRange = false,
                AddHitbox = true,
                DangerValue = 2,
                IsDangerous = false,
                MissileSpellName = "SyndraQ"
            });

            Spells.Add(new SpellData
            {
                ChampionName = "Syndra",
                SpellName = "syndrawcast",
                Slot = SpellSlot.W,
                Type = SkillshotType.Circle,
                Delay = 250,
                Range = 950,
                Radius = 210,
                MissileSpeed = 1450,
                FixedRange = false,
                AddHitbox = true,
                DangerValue = 2,
                IsDangerous = false,
                MissileSpellName = "syndrawcast"
            });

            Spells.Add(new SpellData
            {
                ChampionName = "Syndra",
                SpellName = "syndrae5",
                Slot = SpellSlot.E,
                Type = SkillshotType.Line,
                Delay = 0,
                Range = 950,
                Radius = 100,
                MissileSpeed = 2000,
                FixedRange = false,
                AddHitbox = true,
                DangerValue = 2,
                IsDangerous = false,
                MissileSpellName = "syndrae5",
                DisableFowDetection = true
            });

            Spells.Add(new SpellData
            {
                ChampionName = "Syndra",
                SpellName = "SyndraE",
                Slot = SpellSlot.E,
                Type = SkillshotType.Line,
                Delay = 0,
                Range = 950,
                Radius = 100,
                MissileSpeed = 2000,
                FixedRange = false,
                AddHitbox = true,
                DangerValue = 2,
                IsDangerous = false,
                DisableFowDetection = true,
                MissileSpellName = "SyndraE"
            });

            #endregion Syndra

            #region Taliyah

            Spells.Add(new SpellData
            {
                ChampionName = "Taliyah",
                SpellName = "TaliyahQ",
                Slot = SpellSlot.Q,
                Type = SkillshotType.Line,
                Delay = 250,
                Range = 1000,
                Radius = 100,
                MissileSpeed = 3600,
                FixedRange = true,
                AddHitbox = true,
                DangerValue = 2,
                IsDangerous = false,
                MissileSpellName = "TaliyahQMis",
                CollisionObjects = new[]
                {
                    CollisionableObjects.Heroes,
                    CollisionableObjects.Minions,
                    CollisionableObjects.YasuoWall
                },
                DisabledByDefault = true
            });

            Spells.Add(new SpellData
            {
                ChampionName = "Taliyah",
                SpellName = "TaliyahW",
                Slot = SpellSlot.W,
                Type = SkillshotType.Circle,
                Delay = 600,
                Range = 900,
                Radius = 200,
                MissileSpeed = int.MaxValue,
                FixedRange = false,
                AddHitbox = true,
                DangerValue = 2,
                IsDangerous = true,
                MissileSpellName = "TaliyahW"
            });

            #endregion Taliyah

            #region Talon

            Spells.Add(new SpellData
            {
                ChampionName = "Talon",
                SpellName = "TalonRake",
                Slot = SpellSlot.W,
                Type = SkillshotType.Line,
                Delay = 250,
                Range = 950,
                Radius = 80,
                MissileSpeed = 2300,
                FixedRange = true,
                AddHitbox = true,
                DangerValue = 2,
                IsDangerous = true,
                MultipleNumber = 3,
                MultipleAngle = 20 * (float) Math.PI / 180,
                MissileSpellName = "talonrakemissileone"
            });

            Spells.Add(new SpellData
            {
                ChampionName = "Talon",
                SpellName = "TalonRakeReturn",
                Slot = SpellSlot.W,
                Type = SkillshotType.Line,
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
                MissileSpellName = "talonrakemissiletwo"
            });

            #endregion Talon

            #region Tahm Kench

            Spells.Add(new SpellData
            {
                ChampionName = "TahmKench",
                SpellName = "TahmKenchQ",
                Slot = SpellSlot.Q,
                Type = SkillshotType.Line,
                Delay = 250,
                Range = 951,
                Radius = 90,
                MissileSpeed = 2800,
                FixedRange = true,
                AddHitbox = true,
                DangerValue = 3,
                IsDangerous = true,
                MissileSpellName = "tahmkenchqmissile",
                CanBeRemoved = true,
                CollisionObjects = new[]
                {
                    CollisionableObjects.Heroes,
                    CollisionableObjects.Minions,
                    CollisionableObjects.YasuoWall
                }
            });

            #endregion Tahm Kench

            #region Taric

            Spells.Add(new SpellData
            {
                ChampionName = "Taric",
                SpellName = "TaricE",
                Slot = SpellSlot.E,
                Type = SkillshotType.Line,
                Delay = 1000,
                Range = 750,
                Radius = 100,
                MissileSpeed = int.MaxValue,
                FixedRange = true,
                AddHitbox = true,
                DangerValue = 3,
                IsDangerous = true,
                MissileSpellName = "TaricE"
            });

            #endregion Taric

            #region Thresh

            Spells.Add(new SpellData
            {
                ChampionName = "Thresh",
                SpellName = "ThreshQ",
                Slot = SpellSlot.Q,
                Type = SkillshotType.Line,
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
                CollisionObjects = new[]
                {
                    CollisionableObjects.Heroes,
                    CollisionableObjects.Minions,
                    CollisionableObjects.YasuoWall
                }
            });

            Spells.Add(new SpellData
            {
                ChampionName = "Thresh",
                SpellName = "ThreshEFlay",
                Slot = SpellSlot.E,
                Type = SkillshotType.Line,
                Delay = 125,
                Range = 1075,
                Radius = 110,
                MissileSpeed = 2000,
                FixedRange = true,
                AddHitbox = true,
                DangerValue = 3,
                IsDangerous = true,
                Centered = true,
                MissileSpellName = "ThreshEMissile1"
            });

            #endregion Thresh

            #region Tristana

            Spells.Add(new SpellData
            {
                ChampionName = "Tristana",
                SpellName = "RocketJump",
                Slot = SpellSlot.W,
                Type = SkillshotType.Circle,
                Delay = 500,
                Range = 900,
                Radius = 270,
                MissileSpeed = 1500,
                FixedRange = false,
                AddHitbox = true,
                DangerValue = 2,
                IsDangerous = false,
                MissileSpellName = "RocketJump"
            });

            #endregion Tristana

            #region Tryndamere

            Spells.Add(new SpellData
            {
                ChampionName = "Tryndamere",
                SpellName = "slashCast",
                Slot = SpellSlot.E,
                Type = SkillshotType.Line,
                Delay = 0,
                Range = 660,
                Radius = 93,
                MissileSpeed = 1300,
                FixedRange = false,
                AddHitbox = true,
                DangerValue = 2,
                IsDangerous = false,
                MissileSpellName = "slashCast"
            });

            #endregion Tryndamere

            #region TwistedFate

            Spells.Add(new SpellData
            {
                ChampionName = "TwistedFate",
                SpellName = "WildCards",
                Slot = SpellSlot.Q,
                Type = SkillshotType.Line,
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
                CollisionObjects = new[]
                {
                    CollisionableObjects.YasuoWall
                }
            });

            #endregion TwistedFate

            #region Twitch

            Spells.Add(new SpellData
            {
                ChampionName = "Twitch",
                SpellName = "TwitchVenomCask",
                Slot = SpellSlot.W,
                Type = SkillshotType.Circle,
                Delay = 250,
                Range = 950,
                Radius = 275,
                MissileSpeed = 1400,
                FixedRange = false,
                AddHitbox = true,
                DangerValue = 2,
                IsDangerous = false,
                MissileSpellName = "TwitchVenomCaskMissile",
                ExtraDuration = 3000,
                CollisionObjects = new[]
                {
                    CollisionableObjects.YasuoWall
                }
            });

            Spells.Add(new SpellData
            {
                ChampionName = "Twitch",
                SpellName = "TwitchSprayandPrayAttack",
                ExtraSpellNames = new[]
                {
                    "TwitchFullAutomatic"
                },
                Slot = SpellSlot.R,
                Type = SkillshotType.Line,
                Delay = 250,
                Range = 1100,
                Radius = 60,
                MissileSpeed = 4000,
                FixedRange = true,
                AddHitbox = true,
                DangerValue = 2,
                IsDangerous = false,
                MissileSpellName = "",
                CollisionObjects = new[]
                {
                    CollisionableObjects.YasuoWall
                }
            });

            #endregion Twitch

            #region Urgot

            //TODO Rework

            #endregion

            #region Varus

            Spells.Add(new SpellData
            {
                ChampionName = "Varus",
                SpellName = "VarusQMissilee",
                Slot = SpellSlot.Q,
                Type = SkillshotType.Line,
                Delay = 250,
                Range = 1800,
                Radius = 70,
                MissileSpeed = 1900,
                FixedRange = true,
                AddHitbox = true,
                DangerValue = 2,
                IsDangerous = false,
                MissileSpellName = "VarusQMissile",
                CollisionObjects = new[]
                {
                    CollisionableObjects.YasuoWall
                }
            });

            Spells.Add(new SpellData
            {
                ChampionName = "Varus",
                SpellName = "VarusE",
                Slot = SpellSlot.E,
                Type = SkillshotType.Circle,
                Delay = 1000,
                Range = 925,
                Radius = 235,
                MissileSpeed = 1500,
                FixedRange = false,
                AddHitbox = true,
                DangerValue = 2,
                IsDangerous = false,
                MissileSpellName = "VarusE"
            });

            Spells.Add(new SpellData
            {
                ChampionName = "Varus",
                SpellName = "VarusR",
                Slot = SpellSlot.R,
                Type = SkillshotType.Line,
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
                CollisionObjects = new[]
                {
                    CollisionableObjects.Heroes,
                    CollisionableObjects.YasuoWall
                }
            });

            #endregion Varus

            #region Veigar

            Spells.Add(new SpellData
            {
                ChampionName = "Veigar",
                SpellName = "VeigarBalefulStrike",
                Slot = SpellSlot.Q,
                Type = SkillshotType.Line,
                Delay = 250,
                Range = 950,
                Radius = 70,
                MissileSpeed = 2200,
                FixedRange = true,
                AddHitbox = true,
                DangerValue = 2,
                IsDangerous = false,
                MissileSpellName = "VeigarBalefulStrikeMis",
                CollisionObjects = new[]
                {
                    CollisionableObjects.YasuoWall
                }
            });

            Spells.Add(new SpellData
            {
                ChampionName = "Veigar",
                SpellName = "VeigarDarkMatter",
                Slot = SpellSlot.W,
                Type = SkillshotType.Circle,
                Delay = 1350,
                Range = 900,
                Radius = 225,
                MissileSpeed = int.MaxValue,
                FixedRange = false,
                AddHitbox = true,
                DangerValue = 2,
                IsDangerous = false,
                MissileSpellName = ""
            });

            Spells.Add(new SpellData
            {
                ChampionName = "Veigar",
                SpellName = "VeigarEventHorizon",
                Slot = SpellSlot.E,
                Type = SkillshotType.Circle,
                Delay = 500,
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
                MissileSpellName = ""
            });

            #endregion Veigar

            #region Velkoz

            Spells.Add(new SpellData
            {
                ChampionName = "Velkoz",
                SpellName = "VelkozQ",
                Slot = SpellSlot.Q,
                Type = SkillshotType.Line,
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
                CollisionObjects = new[]
                {
                    CollisionableObjects.Heroes,
                    CollisionableObjects.Minions,
                    CollisionableObjects.YasuoWall
                }
            });

            Spells.Add(new SpellData
            {
                ChampionName = "Velkoz",
                SpellName = "VelkozQSplit",
                Slot = SpellSlot.Q,
                Type = SkillshotType.Line,
                Delay = 250,
                Range = 1100,
                Radius = 55,
                MissileSpeed = 2100,
                FixedRange = true,
                AddHitbox = true,
                DangerValue = 2,
                IsDangerous = false,
                MissileSpellName = "VelkozQMissileSplit",
                CanBeRemoved = true,
                CollisionObjects = new[]
                {
                    CollisionableObjects.Heroes,
                    CollisionableObjects.Minions,
                    CollisionableObjects.YasuoWall
                }
            });

            Spells.Add(new SpellData
            {
                ChampionName = "Velkoz",
                SpellName = "VelkozW",
                Slot = SpellSlot.W,
                Type = SkillshotType.Line,
                Delay = 250,
                Range = 1200,
                Radius = 88,
                MissileSpeed = 1700,
                FixedRange = true,
                AddHitbox = true,
                DangerValue = 2,
                IsDangerous = false,
                MissileSpellName = "VelkozWMissile"
            });

            Spells.Add(new SpellData
            {
                ChampionName = "Velkoz",
                SpellName = "VelkozE",
                Slot = SpellSlot.E,
                Type = SkillshotType.Circle,
                Delay = 500,
                Range = 800,
                Radius = 225,
                MissileSpeed = 1500,
                FixedRange = false,
                AddHitbox = false,
                DangerValue = 2,
                IsDangerous = false,
                MissileSpellName = "VelkozEMissile"
            });

            #endregion Velkoz

            #region Vi

            Spells.Add(new SpellData
            {
                ChampionName = "Vi",
                SpellName = "Vi-q",
                Slot = SpellSlot.Q,
                Type = SkillshotType.Line,
                Delay = 250,
                Range = 1000,
                Radius = 90,
                MissileSpeed = 1500,
                FixedRange = true,
                AddHitbox = true,
                DangerValue = 3,
                IsDangerous = true,
                MissileSpellName = "ViQMissile"
            });

            #endregion Vi

            #region Viktor

            Spells.Add(new SpellData
            {
                ChampionName = "Viktor",
                SpellName = "ViktorGravitonField",
                Slot = SpellSlot.W,
                Type = SkillshotType.Circle,
                Delay = 1600,
                Range = 700,
                Radius = 300,
                MissileSpeed = int.MaxValue,
                FixedRange = false,
                AddHitbox = true,
                DangerValue = 2,
                IsDangerous = false
            });

            Spells.Add(new SpellData
            {
                ChampionName = "Viktor",
                SpellName = "Laser",
                Slot = SpellSlot.E,
                Type = SkillshotType.Line,
                Delay = 250,
                Range = 1500,
                Radius = 80,
                MissileSpeed = 1350,
                FixedRange = false,
                AddHitbox = true,
                DangerValue = 3,
                IsDangerous = true,
                MissileSpellName = "ViktorDeathRayMissile",
                ExtraMissileNames = new[]
                {
                    "viktoreaugmissile"
                },
                ExtraDuration = 1000,
                CollisionObjects = new[]
                {
                    CollisionableObjects.YasuoWall
                }
            });

            #endregion Viktor

            #region Rakan

            Spells.Add(new SpellData
            {
                ChampionName = "Rakan",
                SpellName = "RakanQ",
                Slot = SpellSlot.Q,
                Type = SkillshotType.Line,
                Delay = 250,
                Range = 900,
                Radius = 200,
                MissileSpeed = 1850,
                FixedRange = true,
                AddHitbox = true,
                DangerValue = 1,
                IsDangerous = false,
                MissileSpellName = "RakanQMis",
                CollisionObjects = new[]
                {
                    CollisionableObjects.YasuoWall,
                    CollisionableObjects.Heroes,
                    CollisionableObjects.Minions
                }
            });

            Spells.Add(new SpellData
            {
                ChampionName = "Rakan",
                SpellName = "RakanW",
                Slot = SpellSlot.W,
                Type = SkillshotType.Circle,
                Delay = 250,
                Range = 600,
                Radius = 250,
                MissileSpeed = 1800,
                FixedRange = true,
                AddHitbox = true,
                DangerValue = 2,
                IsDangerous = false,
                MissileSpellName = "RakanWCast"
            });

            #endregion

            #region Kayn

            Spells.Add(new SpellData
            {
                ChampionName = "Kayn",
                SpellName = "KaynQ",
                Slot = SpellSlot.Q,
                Type = SkillshotType.Circle,
                Delay = 250,
                Range = 350,
                Radius = 600,
                MissileSpeed = 2400,
                FixedRange = true,
                AddHitbox = true,
                DangerValue = 2,
                IsDangerous = false,
                MissileSpellName = "KaynQ" //?
            });

            Spells.Add(new SpellData
            {
                ChampionName = "Kayn",
                SpellName = "KaynW",
                Slot = SpellSlot.W,
                Type = SkillshotType.Line,
                Delay = 500,
                Range = 750,
                Radius = 120,
                MissileSpeed = 1600,
                FixedRange = true,
                AddHitbox = true,
                DangerValue = 2,
                IsDangerous = false,
                MissileSpellName = "KaynW"
            });

            #endregion

            #region Ornn

            Spells.Add(new SpellData
            {
                ChampionName = "Ornn",
                SpellName = "OrnnQ",
                Slot = SpellSlot.Q,
                Type = SkillshotType.Line,
                Delay = 250,
                Range = 800,
                Radius = 80,
                MissileSpeed = 1800,
                FixedRange = true,
                AddHitbox = true,
                DangerValue = 1,
                IsDangerous = false,
                MissileSpellName = "OrnnQ",
                CollisionObjects = new[]
                {
                    CollisionableObjects.Heroes,
                    CollisionableObjects.Minions
                }
            });

            Spells.Add(new SpellData
            {
                ChampionName = "Ornn",
                SpellName = "OrnnE",
                Slot = SpellSlot.E,
                Type = SkillshotType.Line,
                Delay = 250,
                Range = 800,
                Radius = 140,
                MissileSpeed = 1600,
                FixedRange = true,
                AddHitbox = true,
                DangerValue = 2,
                IsDangerous = false,
                MissileSpellName = "OrnnE",
                CollisionObjects = new[]
                {
                    CollisionableObjects.Minions
                }
            });

            #endregion

            #region Xayah

            Spells.Add(new SpellData
            {
                ChampionName = "Xayah",
                SpellName = "XayahQ",
                Slot = SpellSlot.Q,
                Type = SkillshotType.Line,
                Delay = 250,
                Range = 1100,
                Radius = 50,
                MissileSpeed = 1400,
                FixedRange = true,
                AddHitbox = true,
                DangerValue = 2,
                IsDangerous = false,
                MissileSpellName = "XayahQMissile",
                ExtraMissileNames = new[]
                {
                    "XayahQMissile2"
                },
                ToggleParticleName = "Xayah_.+_Passive_Dagger_(Enemy|Ally)"
            });

            Spells.Add(new SpellData
            {
                ChampionName = "Xayah",
                SpellName = "XayahE",
                Slot = SpellSlot.E,
                Type = SkillshotType.Line,
                Delay = 250,
                Range = 1100,
                Radius = 60,
                MissileSpeed = 1200,
                Invert = true,
                MissileFollowsUnit = true,
                FixedRange = true,
                AddHitbox = true,
                DangerValue = 2,
                IsDangerous = true,
                MissileSpellName = "XayahEMissileSFX",
                ToggleParticleName = "Xayah_.+_Passive_Dagger_(Enemy|Ally)"
            });

            Spells.Add(new SpellData
            {
                ChampionName = "Xayah",
                SpellName = "XayahR",
                Slot = SpellSlot.R,
                Type = SkillshotType.Cone,
                Delay = 1000,
                Range = 1100,
                Radius = 200,
                MissileSpeed = 2000,
                FixedRange = true,
                AddHitbox = true,
                DangerValue = 5,
                IsDangerous = true,
                MissileSpellName = "XayahR",
                MissileFollowsUnit = true,
                ToggleParticleName = "Xayah_.+_Passive_Dagger_(Enemy|Ally)"
            });

            #endregion Xayah

            #region Xerath

            Spells.Add(new SpellData
            {
                ChampionName = "Xerath",
                SpellName = "xeratharcanopulse2",
                Slot = SpellSlot.Q,
                Type = SkillshotType.Line,
                Delay = 600,
                Range = 1600,
                Radius = 95,
                MissileSpeed = int.MaxValue,
                FixedRange = true,
                AddHitbox = true,
                DangerValue = 2,
                IsDangerous = false,
                MissileSpellName = "xeratharcanopulse2"
            });

            Spells.Add(new SpellData
            {
                ChampionName = "Xerath",
                SpellName = "XerathArcaneBarrage2",
                Slot = SpellSlot.W,
                Type = SkillshotType.Circle,
                Delay = 700,
                Range = 1000,
                Radius = 200,
                MissileSpeed = int.MaxValue,
                FixedRange = false,
                AddHitbox = true,
                DangerValue = 2,
                IsDangerous = false,
                MissileSpellName = "XerathArcaneBarrage2"
            });

            Spells.Add(new SpellData
            {
                ChampionName = "Xerath",
                SpellName = "XerathMageSpear",
                Slot = SpellSlot.E,
                Type = SkillshotType.Line,
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
                CollisionObjects = new[]
                {
                    CollisionableObjects.Heroes,
                    CollisionableObjects.Minions,
                    CollisionableObjects.YasuoWall
                }
            });

            Spells.Add(new SpellData
            {
                ChampionName = "Xerath",
                SpellName = "xerathrmissilewrapper",
                Slot = SpellSlot.R,
                Type = SkillshotType.Circle,
                Delay = 700,
                Range = 5600,
                Radius = 130,
                MissileSpeed = int.MaxValue,
                FixedRange = false,
                AddHitbox = true,
                DangerValue = 3,
                IsDangerous = true,
                MissileSpellName = "xerathrmissilewrapper"
            });

            #endregion Xerath

            #region Warwick

            Spells.Add(new SpellData
            {
                ChampionName = "Warwick",
                SpellName = "WarwickR",
                Slot = SpellSlot.R,
                Type = SkillshotType.Line,
                Delay = 250,
                Range = 1300 + 500,
                Radius = 90,
                MissileSpeed = 2175,
                FixedRange = true,
                AddHitbox = true,
                DangerValue = 5,
                IsDangerous = true,
                MissileSpellName = "WarwickRMissile",
                CollisionObjects = new[]
                {
                    CollisionableObjects.YasuoWall,
                    CollisionableObjects.Heroes
                }
            });

            #endregion Warwick

            #region Yasuo 

            Spells.Add(new SpellData
            {
                ChampionName = "Yasuo",
                SpellName = "yasuoq2",
                Slot = SpellSlot.Q,
                Type = SkillshotType.Line,
                Delay = 400,
                Range = 550,
                Radius = 20,
                MissileSpeed = int.MaxValue,
                FixedRange = true,
                AddHitbox = true,
                DangerValue = 2,
                IsDangerous = true,
                MissileSpellName = "yasuoq2",
                Invert = true
            });

            Spells.Add(new SpellData
            {
                ChampionName = "Yasuo",
                SpellName = "yasuoq3w",
                Slot = SpellSlot.Q,
                Type = SkillshotType.Line,
                Delay = 500,
                Range = 1150,
                Radius = 90,
                MissileSpeed = 1500,
                FixedRange = true,
                AddHitbox = true,
                DangerValue = 3,
                IsDangerous = true,
                MissileSpellName = "yasuoq3w",
                CollisionObjects = new[]
                {
                    CollisionableObjects.YasuoWall
                }
            });

            Spells.Add(new SpellData
            {
                ChampionName = "Yasuo",
                SpellName = "yasuoq",
                Slot = SpellSlot.Q,
                Type = SkillshotType.Line,
                Delay = 400,
                Range = 550,
                Radius = 20,
                MissileSpeed = int.MaxValue,
                FixedRange = true,
                AddHitbox = true,
                DangerValue = 2,
                IsDangerous = true,
                MissileSpellName = "yasuoq",
                Invert = true
            });

            #endregion Yasuo

            #region Zac

            Spells.Add(new SpellData
            {
                ChampionName = "Zac",
                SpellName = "ZacQ",
                Slot = SpellSlot.Q,
                Type = SkillshotType.Line,
                Delay = 500,
                Range = 550,
                Radius = 120,
                MissileSpeed = int.MaxValue,
                FixedRange = true,
                AddHitbox = true,
                DangerValue = 2,
                IsDangerous = false,
                MissileSpellName = "ZacQ"
            });

            Spells.Add(new SpellData
            {
                ChampionName = "Zac",
                SpellName = "ZacE2",
                Slot = SpellSlot.E,
                Type = SkillshotType.Circle,
                Delay = 0,
                Range = 1800,
                Radius = 300,
                MissileSpeed = 1300,
                FixedRange = false,
                AddHitbox = true,
                DangerValue = 4,
                IsDangerous = true,
                MissileSpellName = "ZacE2"
            });

            #endregion Zac

            #region Zed

            Spells.Add(new SpellData
            {
                ChampionName = "Zed",
                SpellName = "ZedQ",
                Slot = SpellSlot.Q,
                Type = SkillshotType.Line,
                Delay = 250,
                Range = 925,
                Radius = 50,
                MissileSpeed = 1700,
                FixedRange = true,
                AddHitbox = true,
                DangerValue = 2,
                IsDangerous = false,
                MissileSpellName = "ZedQMissile",
                //FromObjects = new[] { "Zed_Clone_idle.troy", "Zed_Clone_Idle.troy" },
                FromObjects = new[]
                {
                    "Zed_Base_W_tar.troy",
                    "Zed_Base_W_cloneswap_buf.troy"
                },
                CollisionObjects = new[]
                {
                    CollisionableObjects.YasuoWall
                }
            });

            #endregion Zed

            #region Ziggs

            Spells.Add(new SpellData
            {
                ChampionName = "Ziggs",
                SpellName = "ZiggsQ",
                Slot = SpellSlot.Q,
                Type = SkillshotType.Circle,
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
                DisableFowDetection = true
            });

            Spells.Add(new SpellData
            {
                ChampionName = "Ziggs",
                SpellName = "ZiggsQBounce1",
                Slot = SpellSlot.Q,
                Type = SkillshotType.Circle,
                Delay = 250,
                Range = 850,
                Radius = 140,
                MissileSpeed = 1700,
                FixedRange = false,
                AddHitbox = true,
                DangerValue = 2,
                IsDangerous = false,
                MissileSpellName = "ZiggsQSpell2",
                ExtraMissileNames = new[]
                {
                    "ZiggsQSpell2"
                },
                CanBeRemoved = false,
                DisableFowDetection = true
            });

            Spells.Add(new SpellData
            {
                ChampionName = "Ziggs",
                SpellName = "ZiggsQBounce2",
                Slot = SpellSlot.Q,
                Type = SkillshotType.Circle,
                Delay = 250,
                Range = 850,
                Radius = 160,
                MissileSpeed = 1700,
                FixedRange = false,
                AddHitbox = true,
                DangerValue = 2,
                IsDangerous = false,
                MissileSpellName = "ZiggsQSpell3",
                ExtraMissileNames = new[]
                {
                    "ZiggsQSpell3"
                },
                CanBeRemoved = false,
                DisableFowDetection = true
            });

            Spells.Add(new SpellData
            {
                ChampionName = "Ziggs",
                SpellName = "ZiggsW",
                Slot = SpellSlot.W,
                Type = SkillshotType.Circle,
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
                CollisionObjects = new[]
                {
                    CollisionableObjects.YasuoWall
                }
            });

            Spells.Add(new SpellData
            {
                ChampionName = "Ziggs",
                SpellName = "ZiggsE",
                Slot = SpellSlot.E,
                Type = SkillshotType.Circle,
                Delay = 500,
                Range = 900,
                Radius = 235,
                MissileSpeed = 1750,
                FixedRange = false,
                AddHitbox = true,
                DangerValue = 2,
                IsDangerous = false,
                MissileSpellName = "ZiggsE",
                DisableFowDetection = true
            });

            Spells.Add(new SpellData
            {
                ChampionName = "Ziggs",
                SpellName = "ZiggsR",
                Slot = SpellSlot.R,
                Type = SkillshotType.Circle,
                Delay = 0,
                Range = 5300,
                Radius = 500,
                MissileSpeed = int.MaxValue,
                FixedRange = false,
                AddHitbox = true,
                DangerValue = 2,
                IsDangerous = false,
                MissileSpellName = "ZiggsR",
                DisableFowDetection = true
            });

            #endregion Ziggs

            #region Zilean

            Spells.Add(new SpellData
            {
                ChampionName = "Zilean",
                SpellName = "ZileanQ",
                Slot = SpellSlot.Q,
                Type = SkillshotType.Circle,
                Delay = 250 + 450,
                ExtraDuration = 400,
                Range = 900,
                Radius = 140,
                MissileSpeed = int.MaxValue,
                FixedRange = false,
                AddHitbox = true,
                DangerValue = 2,
                IsDangerous = false,
                MissileSpellName = "ZileanQMissile",
                CollisionObjects = new[]
                {
                    CollisionableObjects.YasuoWall
                }
            });

            #endregion Zilean

            #region Zyra

            Spells.Add(new SpellData
            {
                ChampionName = "Zyra",
                SpellName = "ZyraQ",
                Slot = SpellSlot.Q,
                Type = SkillshotType.Line,
                Delay = 850,
                Range = 800,
                Radius = 140,
                MissileSpeed = int.MaxValue,
                FixedRange = false,
                AddHitbox = true,
                DangerValue = 2,
                IsDangerous = false,
                MissileSpellName = "ZyraQ"
            });

            Spells.Add(new SpellData
            {
                ChampionName = "Zyra",
                SpellName = "ZyraE",
                Slot = SpellSlot.E,
                Type = SkillshotType.Line,
                Delay = 250,
                Range = 1150,
                Radius = 70,
                MissileSpeed = 1150,
                FixedRange = true,
                AddHitbox = true,
                DangerValue = 3,
                IsDangerous = true,
                MissileSpellName = "ZyraE",
                CollisionObjects = new[]
                {
                    CollisionableObjects.YasuoWall
                }
            });

            Spells.Add(new SpellData
            {
                ChampionName = "Zyra",
                SpellName = "ZyraR",
                Slot = SpellSlot.R,
                Type = SkillshotType.Circle,
                Delay = 2100,
                Range = 700,
                Radius = 550,
                MissileSpeed = int.MaxValue,
                FixedRange = false,
                AddHitbox = true,
                DangerValue = 4,
                IsDangerous = true
            });

            #endregion Zyra
        }

        public static SpellData GetBySourceObjectName(string objectName)
        {
            objectName = objectName.ToLowerInvariant();

            return Spells.Where(spellData => spellData.SourceObjectName.Length != 0).FirstOrDefault(spellData => objectName.Contains(spellData.SourceObjectName));
        }

        public static SpellData GetByName(string spellName)
        {
            spellName = spellName.ToLower();

            return Spells.FirstOrDefault(spellData => spellData.SpellName.ToLower() == spellName || spellData.ExtraSpellNames.Contains(spellName));
        }

        public static SpellData GetByMissileName(string missileSpellName)
        {
            missileSpellName = missileSpellName.ToLower();

            return Spells.FirstOrDefault(spellData => spellData.MissileSpellName != null && spellData.MissileSpellName.ToLower() == missileSpellName ||
                                                      spellData.ExtraMissileNames.Contains(missileSpellName));
        }

        public static SpellData GetBySpeed(string championName, int speed, int id = -1)
        {
            return Spells.FirstOrDefault(spellData => spellData.ChampionName == championName && spellData.MissileSpeed == speed && (spellData.Id == -1 || id == spellData.Id));
        }
    }
}