namespace Adept_AIO.Champions.Yorick.Core
{
    using System.Collections.Generic;
    using Aimtec.SDK.Menu;
    using Aimtec.SDK.Menu.Components;
    using Aimtec.SDK.Util;
    using SDK.Delegates;
    using SDK.Menu_Extension;
    using SDK.Unit_Extensions;

    class MenuConfig
    {
        public static Menu Combo, Harass, LaneClear, JungleClear, Lasthit, Misc, Drawings;

        public MenuConfig()
        {
            var mainMenu = new Menu(string.Empty, $"Adept AIO - {Global.Player.ChampionName}", true);
            mainMenu.Attach();

            Global.Orbwalker.Attach(mainMenu);

            Combo = new Menu("YorickCombo", "Combo")
            {
                new MenuBool("Q", "Use Q"),
                new MenuBool("W", "Use W"),
                new MenuBool("E", "Use E"),
                new MenuSliderBool("R", "Use R | When Target Health % <=", true, 65),
            };

            Harass = new Menu("YorickHarass", "Harass")
            {
                new MenuSliderBool("Q", "Use Q | If Mana % >=", true, 15),
                new MenuSliderBool("W", "Use W | If Mana % >=", true, 30),
                new MenuSliderBool("E", "Use E | If Mana % >=", true, 50),
            };

            LaneClear = new Menu("YorickLane", "LaneClear")
            {
                new MenuBool("Check", "Don't Clear When Enemies Nearby"),
                new MenuSliderBool("Q", "Use Q | If Mana % >=", true, 25),
                new MenuSliderBool("E", "Use E | If Mana % >=", true, 20),
                new MenuKeyBind("Shove", "Shove Lane When Toggled | Ignores Spell Checks!", KeyCode.A, KeybindType.Toggle)
            };

            JungleClear = new Menu("YorickJungle", "JungleClear")
            {
                new MenuSliderBool("Q", "Use Q | If Mana % >=", true, 25),
                new MenuSliderBool("E", "Use E | If Mana % >=", true, 20),
            };

            Lasthit = new Menu("YorickLasthit", "Lasthit")
            {
                new MenuSliderBool("Q", "Use Q | If Mana % >=", true, 15),
                new MenuSliderBool("E", "Use E | If Mana % >=", false, 20),
            };

            Drawings = new Menu("YorickDrawManager", "DrawManager")
            {
                new MenuBool("Dmg", "Damage"),
                new MenuBool("Shove", "Shove Lane Status")
            };

            Gapcloser.Attach(mainMenu, "Anti Gapcloser");

            foreach (var menu in new List<Menu>
            {
                Combo,
                Harass,
                LaneClear,
                JungleClear,
                
                Drawings,
                MenuShortcut.Credits
            })
            {
                mainMenu.Add(menu);
            }
        }
    }
}