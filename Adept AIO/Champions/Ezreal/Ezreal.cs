using Adept_AIO.Champions.Ezreal.Miscellaneous;

namespace Adept_AIO.Champions.Ezreal
{
    using Core;
    using Drawings;
    using SDK.Delegates;
    using Aimtec;

    internal class Ezreal
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
