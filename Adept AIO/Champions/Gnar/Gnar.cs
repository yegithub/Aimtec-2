namespace Adept_AIO.Champions.Gnar
{
    using Core;
    using Drawings;
    using Miscellaneous;

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