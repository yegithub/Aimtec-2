namespace Adept_AIO.Champions.Jhin
{
    using Core;
    using Drawings;
    using Miscellaneous;

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
