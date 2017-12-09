namespace Adept_AIO.Champions.MissFortune
{
    using Core;
    using Drawings;
    using Miscellaneous;
    using SDK.Delegates;

    class MissFortune
    {
        public MissFortune()
        {
            new MenuConfig();
            new SpellManager();

            new Automatic();
            new Manager();

            new DrawManager();

            new AntiGapcloser();
        }
    }
}