namespace Adept_AIO.Champions.Template
{
    using Core;
    using Drawings;
    using Miscellaneous;
    using SDK.Delegates;

    class Template
    {
        public Template()
        {
            new MenuConfig();
            new SpellManager();

            new Killsteal();
            new Manager();

            new DrawManager();

            Gapcloser.OnGapcloser += AntiGapcloser.OnGapcloser;
        }
    }
}