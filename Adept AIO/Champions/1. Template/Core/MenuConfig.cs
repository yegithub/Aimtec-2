namespace Adept_AIO.Champions._1._Template.Core
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

            Combo = new Menu("TemplateCombo", "Combo")
            {
                new MenuBool("Q", "Use Q"),
                new MenuBool("W", "Use W"),
                new MenuBool("E", "Use E"),
                new MenuBool("Q", "Use Q"),
            };

            Harass = new Menu("TemplateHarass", "Harass")
            {
                new MenuSliderBool("Q", "Use Q | If Mana % >=", true, 15),
                new MenuSliderBool("W", "Use W | If Mana % >=", true, 30),
                new MenuSliderBool("E", "Use E | If Mana % >=", true, 50),
            };

            LaneClear = new Menu("TemplateLane", "LaneClear")
            {
                new MenuBool("Check", "Don't Clear When Enemies Nearby"),
                new MenuSliderBool("Q", "Use Q | If Mana % >=", true, 25),
                new MenuSliderBool("E", "Use E | If Mana % >=", true, 20)
            };

            JungleClear = new Menu("TemplateJungle", "JungleClear")
            {
                new MenuSliderBool("Q", "Use Q | If Mana % >=", true, 25),
                new MenuSliderBool("W", "Use W | If Mana % >=", true, 35),
                new MenuSliderBool("E", "Use E | If Mana % >=", true, 20),
            };

            Killsteal = new Menu("TemplateKillsteal", "Killsteal")
            {
                new MenuBool("Q", "Use Q"),
                new MenuBool("R", "Use R"),
            };

            Drawings = new Menu("TemplateDrawManager", "DrawManager")
            {
                new MenuBool("Dmg", "Damage"),
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