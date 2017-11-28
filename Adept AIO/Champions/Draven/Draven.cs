namespace Adept_AIO.Champions.Draven
{
    using Aimtec;
    using Core;
    using Drawings;
    using Miscellaneous;
    using SDK.Delegates;

    class Draven
    {
        public Draven()
        {
            new MenuConfig();
            new SpellManager();

            new Killsteal();
            new Manager();

            Render.OnPresent += DrawManager.OnPresent;
            Render.OnRender += DrawManager.OnRender;

            Gapcloser.OnGapcloser += AntiGapcloser.OnGapcloser;
        }
    }
}