using System.Collections.Generic;
using Adept_AIO.Champions.LeeSin.Update.OrbwalkingEvents;
using Adept_AIO.SDK.Extensions;
using Aimtec.SDK.Menu;
using Aimtec.SDK.Menu.Components;
using Aimtec.SDK.Orbwalking;
using Aimtec.SDK.Util;

namespace Adept_AIO.Champions.LeeSin.Core
{
    class MenuConfig
    {
        private static Menu MainMenu;

        public static Menu InsecMenu,
                           Combo,
                           Harass,
                           LaneClear,
                           JungleClear,
                           Killsteal,
                           Drawings,
                           Miscellaneous;

        public static void Attach()
        {
            MainMenu = new Menu(string.Empty, "Adept AIO", true);
            MainMenu.Attach();

            Extension.InsecMode= new OrbwalkerMode("Insec", KeyCode.T, null, Insec.OnKeyPressed);
            Extension.WardjumpMode = new OrbwalkerMode("Wardjump", KeyCode.G, null, WardJump.OnKeyPressed);
            Extension.KickFlashMode = new OrbwalkerMode("Kick Flash", KeyCode.A, null, Insec.Kick);

            GlobalExtension.Orbwalker.AddMode(Extension.InsecMode);
            GlobalExtension.Orbwalker.AddMode(Extension.WardjumpMode);
            GlobalExtension.Orbwalker.AddMode(Extension.KickFlashMode);
            GlobalExtension.Orbwalker.Attach(MainMenu);

            InsecMenu = new Menu("Insec", "Insec")
            {
                new MenuBool("Object", "[Q] - All Objects").SetToolTip("Uses Q to gapclose at every valid target"),
                new MenuList("Position", "Insec Position", new []{"Ally Turret", "Ally Hero"}, 0),
                new MenuList("Kick", "Kick Type: ", new []{"Flash R", "R Flash"}, 1),
            };

            foreach (var hero in GameObjects.EnemyHeroes)
            {
                InsecMenu.Add(new MenuBool(hero.ChampionName, "Insec: " + hero.ChampionName));
            }

            Combo = new Menu("Combo", "Combo")
            {
                new MenuBool("Turret", "Don't Q2 Into Turret"),
                new MenuBool("Q", "Use Q"),
                new MenuBool("W", "Use W"),
                new MenuBool("Ward", "Use Wards"),
                new MenuBool("E", "Use E"),
                new MenuBool("R", "Use R"),
                new MenuBool("Star", "Star Combo On Killable")
            };

            Harass = new Menu("Harass", "Harass")
            {
                new MenuBool("Q", "Use Q"),
                new MenuBool("Q2", "Use Q2"),
                new MenuList("Mode", "W Mode: ", new []{"Away", "W Self"}, 0),
                new MenuBool("E", "Use E"),
                new MenuBool("E2", "Use E2")
            };

            JungleClear = new Menu("Jungle", "JungleClear")
            {
                new MenuBool("Q", "Q"),
                new MenuBool("W", "W"),
                new MenuBool("E", "E"),
            };

            LaneClear = new Menu("Lane", "LaneClear")
            {
                new MenuBool("Check", "Don't Clear When Enemies Nearby"),
                new MenuBool("Q", "Q"),
                new MenuBool("W", "W"),
                new MenuBool("E", "E"),
            };

            Killsteal = new Menu("Killsteal", "Killsteal")
            {
                new MenuBool("Ignite", "Ignite"),
                new MenuBool("Smite", "Smite"),
                new MenuBool("Q", "Q"),
                new MenuBool("E", "E"),
                new MenuBool("R", "R")
            };

            Drawings = new Menu("Drawings", "Drawings")
            {
                new MenuSlider("Segments", "Segments", 200, 100, 300).SetToolTip("Smoothness of the circles. Less equals more FPS."),
                new MenuBool("Position", "Insec Position"),
                new MenuBool("Q", "Q Range"),

            };

            Miscellaneous = new Menu("Miscellaneous", "Miscellaneous")
            {
                new MenuBool("Steal", "Steal Legendary").SetToolTip("Will Q2 -> Smite -> W"),
                new MenuBool("Interrupt", "Interrupt Spells"),
                new MenuBool("Stealth", "Anti-Stealth"),
                new MenuSliderBool("Count", "R If X Hit", true, 2, 1, 5),
            };

            foreach (var menu in new List<Menu>
            {
                InsecMenu,
                Combo,
                Harass,
                LaneClear,
                JungleClear,
                Killsteal,
                Drawings,
                Miscellaneous,
                MenuShortcut.Credits
            })
                MainMenu.Add(menu);
        }
    }
}
