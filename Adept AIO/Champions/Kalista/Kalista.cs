namespace Adept_AIO.Champions.Kalista
{
    using System.Collections.Generic;
    using Aimtec;
    using Core;
    using Drawings;
    using Miscellaneous;

    class Kalista
    {
        public Kalista()
        {
            new MenuConfig();
            new SpellManager();

            Game.OnUpdate += Manager.OnUpdate;
            Game.OnUpdate += Killsteal.OnUpdate;
            Game.OnUpdate += Automatic.OnUpdate;

            Render.OnPresent += DrawManager.OnPresent;
            Render.OnRender += DrawManager.OnRender;
        }
    }
}
