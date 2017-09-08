using System.Collections.Generic;
using Adept_AIO.SDK.Junk;
using Aimtec.SDK.Menu;
using Aimtec.SDK.Menu.Components;

namespace Adept_AIO.Champions.Kayn.Core
{
    internal class MenuConfig
    {
        private static Menu _mainMenu;

        public static Menu Combo,
                           Whitelist,
                           Harass,
                           LaneClear,
                           JungleClear,
                           Killsteal,
                           Drawings;

        public static void Attach()
        {
            _mainMenu = new Menu(string.Empty, "Adept AIO", true);
            _mainMenu.Attach();

            Global.Orbwalker.Attach(_mainMenu);

            Whitelist = new Menu("Whitelist", "Whitelist");
            foreach (var hero in GameObjects.EnemyHeroes)
            {
                Whitelist.Add(new MenuBool(hero.ChampionName, "Use R Against: " + hero.ChampionName));
            }

            Combo = new Menu("Combo", "Combo")
            {
                new MenuBool("Beyblade", "Flash").SetToolTip("Q Flash AA R"),
                new MenuBool("Q", "Use Q"),
                new MenuBool("W", "Use W"),
                new MenuBool("E", "Use E", false),
                new MenuSliderBool("R", "Force R When Below (% HP)", true, 15, 1, 100)
            };

            Harass = new Menu("Harass", "Harass")
            {
                new MenuBool("Q", "Use Q"),
                new MenuBool("W", "Use W")
            };

            LaneClear = new Menu("LaneClear", "LaneClear")
            {
                new MenuBool("Check", "Dont' Clear When Enemies Nearby"),
                new MenuSliderBool("Q", "[Q] (Min. Mana %", true, 25, 1, 100),
                new MenuSliderBool("W", "[W] (Min. Mana %", true, 25, 1, 100)
            };

            JungleClear = new Menu("JungleClear", "JungleClear")
            {
                new MenuSliderBool("Q", "[Q] (Min. Mana %", true, 25, 1, 100),
                new MenuSliderBool("W", "[W] (Min. Mana %", true, 25, 1, 100)
            };

            Killsteal = new Menu("Killsteal", "Killsteal")
            {
                new MenuBool("Ignite", "Ignite"),
                new MenuBool("Q", "[Q]"),
                new MenuBool("W", "[W]"),
                new MenuBool("R", "[R]")
            };

            Drawings = new Menu("Drawings", "Drawings")
            {
                new MenuSlider("Segments", "Segments", 100, 100, 200).SetToolTip("Smoothness of the circles"),
                new MenuBool("Dmg", "Damage"),
                new MenuBool("W", "[W] Range"),
                new MenuBool("R", "[R] Range")
            };

            foreach (var menu in new List<Menu>
            {
                Whitelist,
                Combo,
                Harass,
                LaneClear,
                JungleClear,
                Killsteal,
                Drawings,
                MenuShortcut.Credits
            })  _mainMenu.Add(menu);
        }
    }
}
