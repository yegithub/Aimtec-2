using System.Collections.Generic;
using Adept_AIO.SDK.Junk;
using Aimtec.SDK.Menu;
using Aimtec.SDK.Menu.Components;

namespace Adept_AIO.Champions.Jax.Core
{
    internal class MenuConfig
    {
        private static Menu _mainMenu;

        public static Menu Combo,
                           Harass,
                           Clear,
                           Killsteal,
                           Drawings;

        public static void Attach()
        {
            _mainMenu = new Menu(string.Empty, $"Adept AIO - {Global.Player.ChampionName}", true);
            _mainMenu.Attach();

            Global.Orbwalker.Attach(_mainMenu);

            Combo = new Menu("Combo", "Combo")
            {
                new MenuSliderBool("E", "Start E If Distance <", true, 700, 350, 800),
                new MenuBool("Jump", "Only Q If E Is Up OR Killable"),
                new MenuBool("Delay", "Delay Jump When E Is Active"),
                new MenuSliderBool("R", "Use R If 'x' Enemies Nearby", true, 2, 1, 5),
                new MenuBool("Check", "Check For Enemy Turrets")
            };

            Harass = new Menu("Harass", "Harass")
            {
                new MenuBool("Q", "Use Q"),
                new MenuBool("W", "Use W"),
                new MenuBool("E", "E", false)
            };

            Clear = new Menu("Clear", "Clear")
            {
                new MenuBool("Check", "Don't Clear If Nearby Enemies"),
                new MenuBool("Q", "(Q) Jump to Big Mobs"),
                new MenuBool("W", "(W) After Auto"),
                new MenuBool("E", "(E) After Auto")
            };

            Killsteal = new Menu("Killsteal", "Killsteal")
            {
                new MenuBool("Q", "(Q)"),
            };

            Drawings = new Menu("Drawings", "Drawings")
            {
                new MenuSlider("Segments", "Segments", 100, 100, 200).SetToolTip("Smoothness of the circles"),
                new MenuBool("Q", "Draw Q Range"),
                new MenuBool("E", "Draw E-Q Time"),
                new MenuBool("Dmg", "Draw Damage"),
            };

            foreach (var menu in new List<Menu>
            {
                Combo,
                Harass,
                Clear,
                Killsteal,
                Drawings,
                MenuShortcut.Credits
            })
            _mainMenu.Add(menu);
        }
    }
}
