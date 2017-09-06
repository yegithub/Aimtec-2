using System.Collections.Generic;
using Adept_AIO.Champions.Riven.Update.OrbwalkingEvents;
using Adept_AIO.SDK.Delegates;
using Adept_AIO.SDK.Junk;
using Aimtec.SDK.Menu;
using Aimtec.SDK.Menu.Components;
using Aimtec.SDK.Orbwalking;
using Aimtec.SDK.Util;

namespace Adept_AIO.Champions.Riven.Core
{
    internal class MenuConfig
    {
        private static Menu _mainMenu;

        public static Menu Combo,
                           BurstMenu,
                           Harass,
                           Lane,
                           Jungle,
                           Killsteal,
                           Miscellaneous,
                           Drawings;

        public static OrbwalkerMode BurstMode, FleeMode;

        public static void Attach()
        {
            _mainMenu = new Menu(string.Empty, "Adept AIO", true);
            _mainMenu.Attach();

            FleeMode = new OrbwalkerMode("Flee", KeyCode.A, null, Flee.OnKeyPressed);
            BurstMode = new OrbwalkerMode("Burst", KeyCode.T, () => Global.TargetSelector.GetSelectedTarget(), Burst.OnUpdate);

            BurstMode.ModeBehaviour.Invoke();
            BurstMode.GetTargetImplementation.Invoke();
            
            Global.Orbwalker.AddMode(BurstMode);
            Global.Orbwalker.AddMode(FleeMode);

            Global.Orbwalker.Attach(_mainMenu);
            Gapcloser.Attach(_mainMenu, "Anti Gapcloser");

            Combo = new Menu("Combo", "Combo")
            {
                new MenuList("Mode", "Combo Mode: ", new []{"Automatic", "Max Damage", "Fast"}, 0),
                new MenuList("Chase", "Chase Mode", new []{"Disabled", "Q", "E", "E & Q"}, 0),
                new MenuBool("Flash", "Flash").SetToolTip("Will flash when an target is safely killable."),
                new MenuSliderBool("Check", "Don't Use R1 If X (% HP) <=", true, 20, 0, 100),
                new MenuList("R",  "R1 Mode: ",  new []{"Never", "Always", "Killable"}, 2),
                new MenuBool("R2", "Use R2")
            };

            BurstMenu = new Menu("Burst", "Burst")
            {
                new MenuSeperator("Note", "Select Target To Burst")
            };
            foreach (var hero in GameObjects.EnemyHeroes)
            {
                BurstMenu.Add(new MenuBool(hero.ChampionName, "Burst: " + hero.ChampionName));
            }

            Harass = new Menu("Harass", "Harass")
            {
                new MenuList("Mode", "Mode: ", new []{"Automatic", "Semi Combo", "Q3 To Safety", "Q3 To Target"}, 0),
                new MenuList("Dodge", "Dodge: ", new []{"Turret", "Cursor", "Away From Target"}, 0),
            };
            foreach (var hero in GameObjects.EnemyHeroes)
            {
                Harass.Add(new MenuBool(hero.ChampionName, "Harass: " + hero.ChampionName));
            }

            Lane = new Menu("Lane", "Lane")
            {
                new MenuBool("Check", "Safe Clear").SetToolTip("Wont clear when enemies are nearby"),
                new MenuBool("Q", "Q"),
                new MenuBool("W", "W"),
                new MenuBool("E", "E"),
            };

            Jungle = new Menu("Jungle", "Jungle")
            {
                new MenuBool("Check", "Safe Clear").SetToolTip("Wont clear when enemies are nearby"),
                new MenuBool("Q", "Q"),
                new MenuBool("W", "W"),
                new MenuBool("E", "E"),
            };

            Killsteal = new Menu("Killsteal", "Killsteal")
            {
                new MenuBool("Ignite", "Ignite"),
                new MenuBool("Items", "Items"),
                new MenuBool("Q", "Q"),
                new MenuBool("W", "W"),
                new MenuBool("R2", "R2"),
            };

            Miscellaneous = new Menu("Miscellaneous", "Miscellaneous")
            {
                new MenuBool("Walljump", "Walljump During Flee"),
                new MenuBool("Active", "Keep Q Active"),
                new MenuBool("Interrupt", "Dodge Certain Spells"),
            };

            Drawings = new Menu("Drawings", "Drawings")
            {
                new MenuSlider("Segments", "Segments", 200, 100, 300).SetToolTip("Smoothness of the circles. Less equals more FPS."),
                new MenuBool("Dmg", "Damage"),
                new MenuBool("Mouse", "Mouse Helper").SetToolTip("Shows where to put mouse to properly Q AA chase the target"),
                new MenuBool("Target", "Draw Line At Target"),
                new MenuBool("Engage", "Engage Range"),
                new MenuBool("R2", "R2 Range", false),
                new MenuBool("Harass", "Harass Pattern")
            };

            foreach (var menu in new List<Menu>
            {
                Combo,
                BurstMenu,
                Harass,
                Lane,
                Jungle,
                Killsteal,
                Drawings,
                Miscellaneous,
                MenuShortcut.Credits
            }) _mainMenu.Add(menu);
        }
    }
}
