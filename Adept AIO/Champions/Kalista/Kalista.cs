namespace Adept_AIO.Champions.Kalista
{
    using System;
    using Aimtec;
    using Aimtec.SDK.Orbwalking;
    using Core;
    using Drawings;
    using Miscellaneous;
    using SDK.Unit_Extensions;

    class Kalista
    {
        public Kalista()
        {
            new MenuConfig();
            new SpellManager();

            Global.Orbwalker.PreAttack += Automatic.PreAttack;

            Game.OnUpdate += Automatic.Test;
            Game.OnUpdate += Manager.OnUpdate;
            Game.OnUpdate += Killsteal.OnUpdate;
            Game.OnUpdate += Automatic.OnUpdate;

            Render.OnPresent += DrawManager.OnPresent;
            Render.OnRender += DrawManager.OnRender;
        }
    }
}