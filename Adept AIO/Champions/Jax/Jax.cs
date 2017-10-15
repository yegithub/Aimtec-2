namespace Adept_AIO.Champions.Jax
{
    using Aimtec;
    using Core;
    using Drawings;
    using Miscellaneous;
    using SDK.Unit_Extensions;

    class Jax
    {
        public static void Init()
        {
            MenuConfig.Attach();
            SpellConfig.Load();

            Game.OnUpdate += Manager.OnUpdate;
            Game.OnUpdate += SpellManager.OnUpdate;
            Game.OnUpdate += Killsteal.OnUpdate;
            Global.Orbwalker.PostAttack += Manager.PostAttack;
            Obj_AI_Base.OnPlayAnimation += Animation.OnPlayAnimation;
            Obj_AI_Base.OnProcessSpellCast += SpellManager.OnProcessSpellCast;
            Render.OnRender += DrawManager.OnRender;
            Render.OnPresent += DrawManager.OnPresent;
        }
    }
}