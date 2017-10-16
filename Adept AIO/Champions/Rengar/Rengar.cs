namespace Adept_AIO.Champions.Rengar
{
    using Aimtec;
    using Core;
    using Drawings;
    using Miscellaneous;
    using SDK.Unit_Extensions;

    class Rengar
    {
        public Rengar()
        {
            MenuConfig.Attach();
            SpellConfig.Load();

            Game.OnUpdate += Manager.OnUpdate;
            Global.Orbwalker.PostAttack += Manager.PostAttack;
            Render.OnRender += DrawManager.OnRender;
            Render.OnPresent += DrawManager.RenderDamage;
        }
    }
}