namespace Adept_AIO.Champions.Twitch.Core
{
    using System.Collections.Generic;
    using Aimtec.SDK.Menu;
    using Aimtec.SDK.Menu.Components;
    using SDK.Menu_Extension;
    using SDK.Unit_Extensions;

    class MenuConfig
    {
        public static Menu Combo, Harass, LaneClear, JungleClear, Killsteal, Drawings;

        public MenuConfig()
        {
            var mainMenu = new Menu(string.Empty, $"Adept AIO - {Global.Player.ChampionName}", true);
            mainMenu.Attach();
            Global.Orbwalker.Attach(mainMenu);

            Combo = new Menu("Combo", "Combo")
            {
                new MenuBool("Q", "Use Q (For Attackspeed)"),
                new MenuBool("W", "Use W"),
                new MenuBool("W2", "Don't Use W When Ult Active"),
                new MenuBool("R", "Use R"),
                new MenuSlider("R2", "Use R If X Enemies Nearby", 3, 0, 5)
            };

            Harass = new Menu("Harass", "Harass") {new MenuBool("Q", "Use Q", false), new MenuBool("W", "Use W")};

            LaneClear = new Menu("LaneClear", "Lane")
            {
                new MenuBool("Check", "Dont' Clear When Enemies Nearby"),
                new MenuSliderBool("W", "Min. W Hit", true, 4, 1, 7),
                new MenuSliderBool("E", "Min. E Killed", true, 4, 1, 7)
            };

            JungleClear = new Menu("Jungle", "Jungle") {new MenuBool("E", "Use E"), new MenuBool("W", "Use W")};

            Killsteal = new Menu("Killsteal", "Killsteal") {new MenuBool("E", "Use E (AA)")};

            Drawings = new Menu("Drawings", "Drawings")
            {
                new MenuSlider("Segments", "Segments", 100, 10, 150).SetToolTip("Smoothness of the circles"),
                new MenuBool("Dmg", "Damage"),
                new MenuBool("World", "Draw Q Time (WORLD)"),
                new MenuBool("Debug", "Debug")
            };

            foreach (var menu in new List<Menu>
            {
                Combo,
                Harass,
                LaneClear,
                JungleClear,
                Killsteal,
                Drawings,
                MenuShortcut.Credits
            })
            {
                mainMenu.Add(menu);
            }
        }
    }
}