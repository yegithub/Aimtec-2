namespace Adept_AIO.Champions.Lucian
{
    using Aimtec;
    using Core;
    using Drawings;
    using Miscellaneous;
    using SDK.Delegates;
    using SDK.Unit_Extensions;

    class Lucian
    {
        public Lucian()
        {
            new MenuConfig();
            new SpellManager();

            Game.OnUpdate += Manager.OnUpdate;
            Game.OnUpdate += Killsteal.OnUpdate;

            Render.OnPresent += DrawManager.OnPresent;
            Render.OnRender += DrawManager.OnRender;

            Gapcloser.OnGapcloser += AntiGapcloser.OnGapcloser;
            Global.Orbwalker.PostAttack += Manager.PostAttack;
        }
    }
}