namespace Adept_AIO.Champions.Xerath
{
    using Aimtec;
    using Core;
    using Drawings;
    using Miscellaneous;
    using SDK.Delegates;

    class Xerath
    {
        public Xerath()
        {
            new MenuConfig();
            new SpellManager();

            new Killsteal();
            new Manager();

            Render.OnPresent += DrawManager.OnPresent;
            Render.OnRender += DrawManager.OnRender;

            Gapcloser.OnGapcloser += AntiGapcloser.OnGapcloser;

            Obj_AI_Base.OnProcessSpellCast += SpellManager.OnProcessSpellCast;
        }
    }
}