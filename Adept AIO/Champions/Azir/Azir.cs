using Adept_AIO.Champions.Azir.Core;
using Adept_AIO.Champions.Azir.Drawings;
using Adept_AIO.Champions.Azir.Update.Miscellaneous;
using Aimtec;

namespace Adept_AIO.Champions.Azir
{
    class Azir
    {
        public static void Init()
        {
            MenuConfig.Attach();
            SpellConfig.Load();

            Game.OnUpdate += Manager.OnUpdate;
            Game.OnUpdate += Killsteal.OnUpdate;

            Render.OnPresent += DrawManager.OnPresent;
            Render.OnRender += DrawManager.OnRender;

            GameObject.OnCreate += SoldierHelper.OnCreate;
            GameObject.OnDestroy += SoldierHelper.OnDestroy;
        }
    }
}
