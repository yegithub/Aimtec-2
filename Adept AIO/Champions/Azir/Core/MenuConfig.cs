using System.Collections.Generic;
using Adept_AIO.Champions.Azir.Update.OrbwalkingEvents;
using Adept_AIO.SDK.Delegates;
using Adept_AIO.SDK.Junk;
using Aimtec.SDK.Menu;
using Aimtec.SDK.Menu.Components;
using Aimtec.SDK.Orbwalking;
using Aimtec.SDK.Util;

namespace Adept_AIO.Champions.Azir.Core
{
    class MenuConfig
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
            var mainMenu = new Menu(string.Empty, $"Adept AIO - {Global.Player.ChampionName}", true);
            mainMenu.Attach();

            Global.Orbwalker.Attach(mainMenu);
            Gapcloser.Attach(mainMenu, "Anti Gapcloser");

            AzirHelper.JumpMode = new OrbwalkerMode("Jump (Cursor)", KeyCode.A, null, Flee.OnKeyPressed);
            AzirHelper.InsecMode = new OrbwalkerMode("Insec", KeyCode.T, () => Global.TargetSelector.GetSelectedTarget(), Insec.OnKeyPressed);

            Global.Orbwalker.AddMode(AzirHelper.JumpMode);
            Global.Orbwalker.AddMode(AzirHelper.InsecMode);

            Combo = new Menu("Combo", "Combo")
            {
                new MenuBool("Q", "Use Q"),
                new MenuBool("Extend", "Extended Q"),
                new MenuBool("W", "Use W"),
                new MenuBool("E", "Use E"),
                new MenuSlider("EDmg", "Force E When Health% <= ", 80),
                new MenuBool("R", "Use R")
            };

            Harass = new Menu("Harass", "Harass")
            {
                new MenuSliderBool("Q", "Use Q (min. Mana%)", true, 40),
                new MenuSliderBool("W", "Use W (min. Mana%)", true, 25),
                new MenuSliderBool("E", "Use E (min. Mana%)", false, 35),
            };

            Lane = new Menu("Lane", "Lane")
            {
                new MenuBool("Check", "Safe Clear"),
                new MenuSliderBool("Q", "Use Q (min Mana%)", true, 40),
                new MenuSliderBool("QHit", "Min Hit By Q", true, 3, 1, 7),
                new MenuSliderBool("W", "Use W (min Mana%)", true, 25),
                new MenuSliderBool("E", "Use E (min Mana%)", true, 70),
                new MenuSliderBool("EHit", "Min Hit By E", true, 4, 1, 7),
            };

            Jungle = new Menu("Jungle", "Jungle")
            {
                new MenuSliderBool("Q", "Use Q (min. Mana%)", true, 40),
                new MenuSliderBool("W", "Use W (min. Mana%)", true, 25),
            };

            Killsteal = new Menu("Killsteal", "Killsteal")
            {
                new MenuBool("Q", "Use Q"),
                new MenuBool("E", "Use E"),
            };

            Miscellaneous = new Menu("Misc", "Miscellaneous")
            {
                // Todo: Check if need
            };

            Drawings = new Menu("Drawings", "Drawings")
            {
                new MenuSlider("Segments", "Segments", 100, 100, 200).SetToolTip("Smoothness of the circles"),
                new MenuBool("Dmg", "Damage"),
                new MenuBool("Soldiers", "Draw Soldiers"),
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
