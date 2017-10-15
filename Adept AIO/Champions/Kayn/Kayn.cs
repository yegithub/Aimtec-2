namespace Adept_AIO.Champions.Kayn
{
    using Aimtec;
    using Core;
    using Drawings;
    using Miscellaneous;
    using SDK.Unit_Extensions;

    class Kayn
    {
        public static void Init()
        {
            MenuConfig.Attach();
            SpellConfig.Load();

            Game.OnUpdate += Killsteal.OnUpdate;
            Game.OnUpdate += Manager.OnUpdate;
            Global.Orbwalker.PostAttack += Manager.PostAttack;

            Render.OnRender += DrawManager.OnRender;
            Render.OnPresent += DrawManager.RenderDamage;
        }
    }
}