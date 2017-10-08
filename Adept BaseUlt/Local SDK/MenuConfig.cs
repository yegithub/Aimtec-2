using Aimtec.SDK.Menu;
using Aimtec.SDK.Menu.Components;
using Aimtec.SDK.Util.Cache;

namespace Adept_BaseUlt.Local_SDK
{
    internal class MenuConfig
    {
        public Menu Menu;

        public void AttatchMenu()
        {
            Menu = new Menu("hello", "Adept - BaseUlt", true);
            Menu.Attach();

            Menu.Add(new MenuBool("RandomUlt", "Use RandomUlt").SetToolTip(
                "Will GUESS the enemy position and ult there"));

            Menu.Add(new MenuSeperator("yes", "Whitelist"));

            foreach (var hero in GameObjects.EnemyHeroes)
            {
                Menu.Add(new MenuBool(hero.ChampionName, "ULT: " + hero.ChampionName));
            }

            Menu.Add(new MenuSeperator("no"));
            Menu.Add(new MenuSlider("Distance", "Max Distance | RandomUlt", 2000, 500, 4000));
        }
    }
}
