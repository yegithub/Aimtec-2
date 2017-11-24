namespace Adept_AIO.Champions.Xerath.Core
{
    using System.Collections.Generic;
    using Aimtec.SDK.Menu;
    using Aimtec.SDK.Menu.Components;
    using SDK.Delegates;
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

            Combo = new Menu("XerathCombo", "Combo")
            {
                new MenuBool("Q", "Use Q"),
                new MenuBool("W", "Use W"),
                new MenuBool("E", "Use E")
            };

            Harass = new Menu("XerathHarass", "Harass")
            {
                new MenuSliderBool("Q", "Use Q | If Mana % >=", true, 50),
                new MenuSliderBool("W", "Use W | If Mana % >=", true, 50),
                new MenuSliderBool("E", "Use E | If Mana % >=", true, 50),
            };

            LaneClear = new Menu("XerathLane", "LaneClear")
            {
                new MenuBool("Check", "Don't Clear When Enemies Nearby"),
                new MenuSliderBool("Q", "Use Q | If Mana % >=", true, 50),
                new MenuSliderBool("W", "Use W | If Mana % >=", true, 50),
                new MenuSliderBool("E", "Use E | If Mana % >=", false, 80)
            };

            JungleClear = new Menu("XerathJungle", "JungleClear")
            {
                new MenuBool("Q", "Use Q"),
                new MenuBool("W", "Use W"),
                new MenuBool("E", "Use E"),
            };

            Killsteal = new Menu("XerathKillsteal", "Killsteal")
            {
                new MenuBool("Q", "Use Q"),
                new MenuBool("W", "Use W"),
                new MenuBool("E", "Use E"),
                new MenuBool("R", "Use R"),
            };

            Drawings = new Menu("XerathDrawManager", "DrawManager")
            {
                new MenuBool("Dmg", "Damage"),
                new MenuBool("Minimap", "Draw R (Minimap)"),
                new MenuBool("Q", "Draw Q Range"),
                new MenuBool("Pred", "Draw Q Prediction (Debug)", false)
            };

            Gapcloser.Attach(mainMenu, "Anti Gapcloser");

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