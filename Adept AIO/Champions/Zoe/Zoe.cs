namespace Adept_AIO.Champions.Zoe
{
    using Aimtec;
    using Core;
    using Drawings;
    using Miscellaneous;
    using SDK.Delegates;

    class Zoe
    {
        public Zoe()
        {
            new MenuConfig();
            new SpellManager();

            Game.OnUpdate += Killsteal.OnUpdate;
            Game.OnUpdate += Manager.OnUpdate;

            Render.OnPresent += DrawManager.OnPresent;
            Render.OnRender += DrawManager.OnRender;

            Gapcloser.OnGapcloser += AntiGapcloser.OnGapcloser;

            Obj_AI_Base.OnProcessSpellCast += SpellManager.OnProcessSpellCast;
        }

    }
}