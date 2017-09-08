using System.Collections.Generic;
using Adept_AIO.SDK.Delegates;
using Adept_AIO.SDK.Junk;
using Aimtec.SDK.Menu;
using Aimtec.SDK.Menu.Components;

namespace Adept_AIO.Champions.Ezreal.Core
{
    internal class MenuConfig
    {
        public static Menu Combo,
                           Harass,
                           Lane,
                           Jungle,
                           Killsteal,
                           Miscellaneous,
                           Drawings;

        public static void Attach()
        {
            var mainMenu = new Menu(string.Empty, "Adept AIO", true);
            mainMenu.Attach();

            Global.Orbwalker.Attach(mainMenu);
            Gapcloser.Attach(mainMenu, "Anti Gapcloser");

            Combo = new Menu("Combo", "Combo")
            {
                new MenuBool("Q", "Use Q"),
                new MenuBool("QM", "Q Minions For AttackSpeed"),
                new MenuSliderBool("W", "Use W At: Enemy (min. Mana%)", true, 30, 0, 100),
                new MenuBool("WA", "Use W At: Ally").SetToolTip("Wont W Allies if enemies nearby"),
                new MenuBool("E", "Use E At Killable Enemies")
            };

            Harass = new Menu("Harass", "Harass")
            {
                new MenuBool("Q", "Use Q"),
                new MenuSliderBool("W", "Use W (min Mana%)", true, 60, 0, 100),
                new MenuBool("WA", "W Allies", false),
                new MenuBool("E", "Use E", false)
            };

            Lane = new Menu("LaneClear", "LaneClear")
            {
                new MenuBool("Check", "Safe Clear").SetToolTip("Wont clear when enemies are nearby. Except for Q."),
                new MenuSliderBool("Q", "Use Q (min Mana%)", true, 80, 0, 100),
                new MenuSliderBool("W", "Use W (min Mana%)", true, 50, 0, 100)
            };

            Jungle = new Menu("JungleClear", "JungleClear")
            {
                new MenuBool("Q", "Use Q | Big Mobs"),
                new MenuBool("QS", "Use Q | Small Mobs"),
                new MenuSliderBool("W", "Use W if Mana % >=", true, 75, 0, 100)
            };

            Killsteal = new Menu("Killsteal", "Killsteal")
            {
                new MenuBool("Q", "Use Q"),
                new MenuBool("W", "Use W"),
                new MenuBool("E", "Use E", false),
                new MenuSliderBool("Range", "Use R if Distance <=", true, 1500, 500, 5000),
                new MenuBool("AA", "Autoattack")
            };

            Miscellaneous = new Menu("Misc", "Miscellaneous")
            {
                new MenuSliderBool("Stack", "Stack Tear (min. Mana%)", true, 80, 0, 100),
                new MenuBool("WT", "W When Attacking Turret").SetToolTip("Wont be activated when enemies are nearby")
            };

            Drawings = new Menu("DrawManager", "DrawManager")
            {
                new MenuSlider("Segments", "Segments", 100, 100, 200).SetToolTip("Smoothness of the circles"),
                new MenuBool("Dmg", "Damage"),
                new MenuBool("Q", "Draw Q Range"),
                new MenuBool("R", "Draw R Range")
            };

            foreach (var menu in new List<Menu>
            {
               Combo,
               Harass,
               Lane,
               Jungle,
               Killsteal,
               Drawings,
               Miscellaneous,
               MenuShortcut.Credits
            }) mainMenu.Add(menu);
        }
    }
}