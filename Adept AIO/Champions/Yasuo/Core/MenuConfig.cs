using System.Collections.Generic;
using Adept_AIO.Champions.Yasuo.Update.OrbwalkingEvents;
using Adept_AIO.SDK.Delegates;
using Adept_AIO.SDK.Menu_Extension;
using Adept_AIO.SDK.Unit_Extensions;
using Aimtec.SDK.Menu;
using Aimtec.SDK.Menu.Components;
using Aimtec.SDK.Orbwalking;
using Aimtec.SDK.Util;
using GameObjects = Aimtec.SDK.Util.Cache.GameObjects;

namespace Adept_AIO.Champions.Yasuo.Core
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
            _mainMenu = new Menu(string.Empty, $"Adept AIO - {Global.Player.ChampionName}", true);
            _mainMenu.Attach();

            Extension.FleeMode     = new OrbwalkerMode("Flee", KeyCode.A, null, Flee.OnKeyPressed);
            Extension.BeybladeMode = new OrbwalkerMode("Beyblade", KeyCode.T, () => Global.TargetSelector.GetSelectedTarget(), Beyblade.OnKeyPressed);

            Extension.BeybladeMode.ModeBehaviour.Invoke();
            Extension.BeybladeMode.GetTargetImplementation.Invoke();

            Global.Orbwalker.AddMode(Extension.FleeMode);
            Global.Orbwalker.AddMode(Extension.BeybladeMode);
         
            Global.Orbwalker.Attach(_mainMenu);

            Whitelist= new Menu("Whitelist", "Whitelist");
            foreach (var hero in GameObjects.EnemyHeroes)
            {
                Whitelist.Add(new MenuBool(hero.ChampionName, "Use R Against: " + hero.ChampionName));
            }

            Combo = new Menu("Combo", "Combo")
            {
                new MenuBool("Walk", "Walk Behind Minion To Dash"),
                new MenuBool("Dodge", "Windwall Targetted Spells"),
                new MenuSlider("Count", "Use R If X Airbourne", 2, 0, 5),
                new MenuBool("Delay", "Delay R").SetToolTip("Tries to Knockup -> AA -> R"),
                new MenuBool("Flash", "Use Flash (Beyblade)").SetToolTip("Will try to E-Q -> Flash. Known as Beyblade"),
                new MenuBool("Turret", "Avoid Using E Under Turret"),
                new MenuBool("Stack", "Safely Stack Q"),
                new MenuList("Dash", "Dash Mode: ", new []{"Cursor", "From Player"}, 0),
                new MenuSlider("Range", "Mouse Dash Range: ", 650, 1, 1000)
            };
            
            // Todo: Add Check and go: EQ AA -> E Out 
            Harass = new Menu("Harass", "Harass")
            {
                new MenuBool("Q", "Use Q3"),
                new MenuBool("E", "Use E", false)
            };

            LaneClear = new Menu("LaneClear", "LaneClear")
            {
                new MenuBool("Check", "Don't Clear When Enemies Nearby"),
                new MenuBool("Turret", "Don't Clear Under Turret"),
                new MenuBool("Q3", "Use Q3"),
                new MenuBool("EAA", "Only E After AA (Fast Clear)"),
                new MenuList("Mode", "E Mode: ", new []{"Disabled", "Lasthit", "Fast Clear"}, 1)
            };

            JungleClear = new Menu("JungleClear", "JungleClear")
            {
                new MenuBool("Q3", "Allow Q3 Usage"),
                new MenuBool("Q",  "Allow Q1 Usage"),
                new MenuBool("E",  "Allow E  Usage")
            };

            Killsteal = new Menu("Killsteal", "Killsteal")
            {
                new MenuBool("Ignite", "Ignite"),
                new MenuBool("Q", "Use Q"),
                new MenuBool("Q3", "Use Q3"),
                new MenuBool("E", "Use E")
            };

            Drawings = new Menu("Drawings", "Drawings")
            {
                new MenuSlider("Segments", "Segments", 100, 100, 200).SetToolTip("Smoothness of the circles"),
                new MenuBool("Dmg", "Damage"),
                new MenuBool("R", "Draw R Range"),
                new MenuBool("Range", "Draw Minion Search Range"),
                new MenuBool("Debug", "Debug")
            };

            Gapcloser.Attach(_mainMenu, "Anti Gapcloser");

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
            })
            _mainMenu.Add(menu);
        }
    }
}
