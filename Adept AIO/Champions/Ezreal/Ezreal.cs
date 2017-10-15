namespace Adept_AIO.Champions.Ezreal
{
    using Aimtec;
    using Core;
    using Drawings;
    using Miscellaneous;
    using SDK.Delegates;

    class Ezreal
    {
        public static void Init()
        {
            MenuConfig.Attach();
            SpellConfig.Load();

            Game.OnUpdate += Manager.OnUpdate;
            Game.OnUpdate += Killsteal.OnUpdate;
            Render.OnRender += DrawManager.OnRender;
            Render.OnPresent += DrawManager.OnPresent;
            Gapcloser.OnGapcloser += AntiGapcloser.OnGapcloser;
        }
    }
}