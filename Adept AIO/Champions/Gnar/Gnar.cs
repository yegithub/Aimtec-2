namespace Adept_AIO.Champions.Gnar
{
    using Aimtec;
    using Aimtec.SDK.Extensions;
    using Core;
    using Drawings;
    using Miscellaneous;
    using SDK.Unit_Extensions;

    class Gnar
    {
        public Gnar()
        {
            new MenuConfig();
            new SpellManager();
            new DrawManager();
            new Manager();
            new Killsteal();
            new AntiGapcloser();
        }
    }
}