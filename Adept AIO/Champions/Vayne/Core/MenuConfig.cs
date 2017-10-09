using System.Collections.Generic;
using Adept_AIO.Champions.Vayne.OrbwalkingMode;
using Adept_AIO.SDK.Delegates;
using Adept_AIO.SDK.Menu_Extension;
using Adept_AIO.SDK.Unit_Extensions;
using Aimtec.SDK.Menu;
using Aimtec.SDK.Menu.Components;
using Aimtec.SDK.Orbwalking;
using Aimtec.SDK.Util;

namespace Adept_AIO.Champions.Vayne.Core
{
    internal class MenuConfig
    {
        public static OrbwalkerMode FleeOrbwalkerMode, CondemnFlashOrbwalkerMode;

        private static Menu _mainMenu;

        public static Menu Combo,
                           Harass,
                           LaneClear,
                           JungleClear,
                           Lasthit,
                           Killsteal,
                           Misc,
                           Drawings;

        public static void Attach()
        {
            _mainMenu = new Menu(string.Empty, $"Adept AIO - {Global.Player.ChampionName}", true);
            _mainMenu.Attach();
            Global.Orbwalker.Attach(_mainMenu);
            FleeOrbwalkerMode = new OrbwalkerMode("Flee", KeyCode.A, null, Flee.OnKeyPressed);
            Global.Orbwalker.AddMode(FleeOrbwalkerMode);

            CondemnFlashOrbwalkerMode = new OrbwalkerMode("Condemn Flash", KeyCode.T, null, CondemnFlash.OnKeyPressed);
            Global.Orbwalker.AddMode(CondemnFlashOrbwalkerMode);

            Combo = new Menu("Combo", "Combo")
            {
                new MenuList("Q", "Q Mode", new[] {"After Auto", "Engage"}, 0),
                new MenuList("Mode", "Q To:", new[] {"Cursor", "Side"}, 1),
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
                new MenuList("Q", "Q Mode", new[] {"After Auto", "Engage"}, 0),
                new MenuList("Mode", "Q To:", new[] {"Cursor", "Side"}, 1),
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
                new MenuList("Q", "Q Mode", new[] {"After Auto", "Engage"}, 0),
                new MenuList("QMode", "Q To:", new[] {"Cursor", "Side"}, 0),
            };

            JungleClear = new Menu("Jungle", "Jungle")
            {
                new MenuList("Q", "Q Mode", new[] {"After Auto", "Engage"}, 0),
                new MenuList("Mode", "Q To:", new[] {"Cursor", "Side"}, 0),
                new MenuBool("E", "Use E")
            };

            Lasthit = new Menu("Lasthit", "Lasthit")
            {
                new MenuBool("Q", "Use Q After AA")
            };

            Killsteal = new Menu("Killsteal", "Killsteal")
            {
                new MenuBool("Q", "Q -> AA"),
                new MenuBool("E", "Use E")
            };

            Misc = new Menu("Misc", "Miscellaneous")
            {
                new MenuBool("Q", "Anti Gapcloser | Q"),
                new MenuBool("E", "Anti Gapcloser | E")
            };

            Drawings = new Menu("Drawings", "Drawings")
            {
                new MenuSlider("Segments", "Segments", 60, 30, 300).SetToolTip("Smoothness of the circles"),
                new MenuBool("Dmg", "Damage"),
                new MenuBool("Q", "Draw Q Range"),
                new MenuBool("Pred", "Draw E Prediction")
            };

            Gapcloser.Attach(_mainMenu, "Anti Gapcloser");

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
            }) _mainMenu.Add(menu);
        }
    }
}