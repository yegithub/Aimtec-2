namespace Adept_AIO.Champions.Yorick
{
    using Core;
    using Drawings;
    using Miscellaneous;
    using SDK.Delegates;

    class Yorick
    {
        public Yorick()
        {
            new MenuConfig();
            new SpellManager();

            new Manager();

            new DrawManager();

            Gapcloser.OnGapcloser += AntiGapcloser.OnGapcloser;
        }
    }
}