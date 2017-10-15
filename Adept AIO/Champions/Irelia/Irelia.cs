namespace Adept_AIO.Champions.Irelia
{
    using Aimtec;
    using Core;
    using Drawings;
    using Miscellaneous;
    using SDK.Unit_Extensions;

    class Irelia
    {
        public static void Init()
        {
            MenuConfig.Attach();
            SpellConfig.Load();

            Game.OnUpdate += Manager.OnUpdate;
            Game.OnUpdate += Killsteal.OnUpdate;
            Obj_AI_Base.OnProcessSpellCast += Manager.OnProcessSpellCast;
            Global.Orbwalker.PostAttack += Manager.PostAttack;
            Global.Orbwalker.PreAttack += Manager.OnPreAttack;
            Render.OnRender += DrawManager.OnRender;
            Render.OnPresent += DrawManager.RenderDamage;
        }
    }
}