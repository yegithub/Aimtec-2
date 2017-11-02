namespace Adept_AIO.Champions.Gnar.Core
{
    using System.Collections.Generic;
    using Aimtec.SDK.Menu;
    using Aimtec.SDK.Menu.Components;
    using SDK.Delegates;
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

            Gapcloser.Attach(mainMenu, "Anti Gapcloser");

            Combo = new Menu("GnarCombo", "Combo")
            {
                new MenuBool("Q", "Use Q"),
                new MenuBool("W", "Use W"),
                new MenuBool("E", "Use E (Killable)"),
                new MenuBool("R", "Use R (Killable)"),
                new MenuBool("Flash", "Flash")
            };

            Harass = new Menu("GnarHarass", "Harass") {new MenuBool("Q", "Use Q"), new MenuBool("W", "Use W"), new MenuBool("E", "Use E", false)};

            LaneClear = new Menu("GnarLaneClear", "LaneClear")
            {
                new MenuBool("Check", "Don't Clear When Enemies Nearby"),
                new MenuSliderBool("Q", "Use Q if hit count => ", true, 3, 1, 6),
                new MenuSliderBool("W", "Use W if hit count => ", true, 3, 1, 6)
            };

            JungleClear = new Menu("GnarJngl", " JungleClear") {new MenuBool("Q", "Use Q"), new MenuBool("W", "Use W")};

            Killsteal = new Menu("GnarKS", "Killsteal") {new MenuBool("Q", "Use Q"), new MenuBool("W", "Use W")};

            Misc = new Menu("GnarMisc", "Miscellaneous") {new MenuSliderBool("Auto", "Auto R if hit count => ", true, 3, 1, 5)};

            Drawings = new Menu("YDrawManager", "Drawings")
            {
                new MenuSlider("Segments", "Segments", 100, 10, 150).SetToolTip("Smoothness of the circles"),
                new MenuBool("Dmg", "Damage"),
                new MenuBool("Q", "Draw Q Range"),
                new MenuBool("Debug", "Debug")
            };

            foreach (var menu in new List<Menu> {Combo, Harass, LaneClear, JungleClear, Killsteal, Misc, Drawings, MenuShortcut.Credits})
            {
                mainMenu.Add(menu);
            }
        }
    }
}