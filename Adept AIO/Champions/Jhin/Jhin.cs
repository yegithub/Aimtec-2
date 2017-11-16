namespace Adept_AIO.Champions.Jhin
{
    using Aimtec;
    using Core;
    using Drawings;
    using Miscellaneous;
    using SDK.Delegates;

    class Jhin
    {
        public Jhin()
        {
            new MenuConfig();
            new SpellManager();

            new Automatic();
            new Manager();
            new Killsteal();

            new DrawManager();
            new AntiGapcloser();
        }
    }
}
