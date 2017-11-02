namespace Adept_AIO.Champions.Kalista.Core
{
    using System.Collections.Generic;
    using Aimtec.SDK.Menu;
    using Aimtec.SDK.Menu.Components;
    using SDK.Menu_Extension;
    using SDK.Unit_Extensions;

    class MenuConfig
    {
        public static Menu Combo, Harass, LaneClear, JungleClear, Killsteal, Misc, Drawings;

        public MenuConfig()
        {
            var mainMenu = new Menu(string.Empty, $"Adept AIO - {Global.Player.ChampionName}", true);
            mainMenu.Attach();
            Global.Orbwalker.Attach(mainMenu);

            Combo = new Menu("KalistaCombo", "Combo")
            {
                new MenuBool("Minions", "Auto Attack Minions"),
                new MenuBool("Kite", "Automatic Kiting", false),
                new MenuBool("Q", "Use Q"),
                new MenuSliderBool("R", "Use R If X Enemies Nearby", true, 3, 0, 5)
            };

            Harass = new Menu("KalistaHarass", "Harass") {new MenuBool("Q", "Use Q")};

            LaneClear = new Menu("KalistaLaneClear", "Lane")
            {
                new MenuBool("Check", "Dont' Clear When Enemies Nearby"),
                new MenuSliderBool("Q", "Min. Q Hit", true, 3, 1, 7),
                new MenuBool("E", "Use E")
            };

            JungleClear = new Menu("KalistaJungle", "Jungle") {new MenuBool("Q", "Use Q"), new MenuBool("E", "Use E")};

            Killsteal = new Menu("KalistaKillsteal", "Killsteal") {new MenuBool("E", "Use E"), new MenuBool("Q", "Use Q")};

            Misc = new Menu("KalistaMisc", "Miscellaneous")
            {
                new MenuSliderBool("Soulbound", "Use R If Soulbound Health % <=", true, 10),
                new MenuBool("E", "Use E on minions when enemy has E stacks"),
                new MenuBool("W", "Use W Automatically")
            };

            Drawings = new Menu("KalistaDrawings", "Drawings")
            {
                new MenuSlider("Segments", "Segments", 100, 10, 150).SetToolTip("Smoothness of the circles"),
                new MenuBool("Dmg", "Damage"),
                new MenuBool("Debug", "Debug")
            };

            foreach (var menu in new List<Menu> {Combo, Harass, LaneClear, JungleClear, Killsteal, Misc, Drawings, MenuShortcut.Credits})
            {
                mainMenu.Add(menu);
            }
        }
    }
}