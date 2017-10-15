﻿namespace Adept_AIO.Champions.Vayne.Core
{
    using System.Collections.Generic;
    using Aimtec.SDK.Menu;
    using Aimtec.SDK.Menu.Components;
    using Aimtec.SDK.Orbwalking;
    using Aimtec.SDK.Util;
    using OrbwalkingEvents;
    using SDK.Delegates;
    using SDK.Menu_Extension;
    using SDK.Unit_Extensions;

    class MenuConfig
    {
        public static OrbwalkerMode FleeOrbwalkerMode, CondemnFlashOrbwalkerMode;

        private static Menu _mainMenu;

        public static Menu Combo, Harass, LaneClear, JungleClear, Lasthit, Killsteal, Misc, Drawings;

        public static void Attach()
        {
            _mainMenu = new Menu(string.Empty, $"Adept AIO - {Global.Player.ChampionName}", true);
            _mainMenu.Attach();
            Global.Orbwalker.Attach(_mainMenu);
            FleeOrbwalkerMode = new OrbwalkerMode("Flee", KeyCode.A, null, Flee.OnKeyPressed);
            Global.Orbwalker.AddMode(FleeOrbwalkerMode);

            CondemnFlashOrbwalkerMode = new OrbwalkerMode("Condemn Flash", KeyCode.T, null, CondemnFlash.OnKeyPressed);
            Global.Orbwalker.AddMode(CondemnFlashOrbwalkerMode);

            Gapcloser.Attach(_mainMenu, "Anti Gapcloser");

            Combo = new Menu("Combo", "Combo")
            {
                new MenuList("Q1", "Q Mode", new[] {"After Auto", "Engage"}, 0),
                new MenuList("Mode1", "Q To:", new[] {"Cursor", "Side"}, 1),
                new MenuBool("ToE", "Force Q To E Pos"),
                new MenuBool("W", "Focus Targets With W Stacks"),
                new MenuBool("E", "Use E"),
                new MenuBool("R", "Use R"),
                new MenuBool("Killable", "Only Use R When Killable"),
                new MenuSlider("Count", "Use R If >= X Enemies Count", 2, 1, 5),
                new MenuSeperator("Whitelist")
            };

            foreach (var enemy in GameObjects.EnemyHeroes)
            {
                Combo.Add(new MenuBool(enemy.ChampionName, $"Use E On: {enemy.ChampionName}"));
            }

            Harass = new Menu("Harass", "Harass")
            {
                new MenuList("Q2", "Q Mode", new[] {"After Auto", "Engage"}, 0),
                new MenuList("Mode2", "Q To:", new[] {"Cursor", "Side"}, 1),
                new MenuBool("E", "Use E"),
                new MenuSeperator("Whitelist")
            };

            foreach (var enemy in GameObjects.EnemyHeroes)
            {
                Harass.Add(new MenuBool(enemy.ChampionName, $"Use E On: {enemy.ChampionName}"));
            }

            LaneClear = new Menu("LaneClear", "Lane")
            {
                new MenuBool("TurretFarm", "Smart Under Turret Farm"),
                new MenuList("Q3", "Q Mode", new[] {"After Auto", "Engage"}, 0),
                new MenuList("QMode3", "Q To:", new[] {"Cursor", "Side"}, 0)
            };

            JungleClear = new Menu("Jungle", "Jungle")
            {
                new MenuList("Q4", "Q Mode", new[] {"After Auto", "Engage"}, 0),
                new MenuList("Mode4", "Q To:", new[] {"Cursor", "Side"}, 0),
                new MenuBool("E", "Use E")
            };

            Lasthit = new Menu("Lasthit", "Lasthit") {new MenuBool("Q", "Use Q After AA")};

            Killsteal = new Menu("Killsteal", "Killsteal") {new MenuBool("Q", "Q -> AA"), new MenuBool("E", "Use E")};

            Misc = new Menu("Misc", "Miscellaneous")
            {
                new MenuBool("Q", "Anti Gapcloser | Q"),
                new MenuBool("E", "Anti Gapcloser | E")
            };

            Drawings = new Menu("DrawManager", "DrawManager")
            {
                new MenuSlider("Segments", "Segments", 100, 10, 150).SetToolTip("Smoothness of the circles"),
                new MenuBool("Dmg", "Damage"),
                new MenuBool("Q", "Draw Q Range"),
                new MenuBool("Pred", "Draw E Prediction")
            };

            foreach (var menu in new List<Menu>
            {
                Combo,
                Harass,
                LaneClear,
                JungleClear,
                Lasthit,
                Killsteal,
                Misc,
                Drawings,
                MenuShortcut.Credits
            })
            {
                _mainMenu.Add(menu);
            }
        }
    }
}