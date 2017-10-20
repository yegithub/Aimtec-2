namespace Adept_AIO.Champions.Vayne
{
    using Aimtec;
    using Core;
    using Drawings;
    using Miscellaneous;
    using SDK.Delegates;
    using SDK.Unit_Extensions;

    class Vayne
    {
        public Vayne()
        {
            MenuConfig.Attach();
            SpellManager.Load();

            Game.OnUpdate += Manager.OnUpdate;
            Game.OnUpdate += Killsteal.OnUpdate;

            Gapcloser.OnGapcloser += AntiGapcloser.OnGapcloser;
            Global.Orbwalker.PostAttack += Manager.PostAttack;
            Global.Orbwalker.PreAttack += Manager.PreAttack;
         
            Render.OnPresent += DrawManager.OnPresent;
            Render.OnRender += DrawManager.OnRender;
        }
    }
}