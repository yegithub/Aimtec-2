namespace Adept_AIO.Champions.Riven
{
    using Aimtec;
    using Core;
    using Drawings;
    using Miscellaneous;
    using SDK.Delegates;
    using SDK.Unit_Extensions;

    class Riven
    {
        public Riven()
        {
            new MenuConfig();
            new SpellConfig();

            Game.OnUpdate += Manager.OnUpdate;
            Game.OnUpdate += SpellManager.OnUpdate;
            Game.OnUpdate += Killsteal.OnUpdate;
            Gapcloser.OnGapcloser += AntiGapcloser.OnGapcloser;

            Obj_AI_Base.OnProcessSpellCast += SpellManager.OnProcessSpellCast;
            Obj_AI_Base.OnProcessSpellCast += DodgeSpell.OnProcessSpellCast;

            Obj_AI_Base.OnPlayAnimation += Animation.OnPlayAnimation;

            Global.Orbwalker.PostAttack += Manager.OnPostAttack;
            Render.OnRender += DrawManager.RenderBasics;
            Render.OnPresent += DrawManager.OnPresent;
        }
    }
}