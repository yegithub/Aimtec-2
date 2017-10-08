using Adept_AIO.Champions.Vayne.Core;
using Adept_AIO.Champions.Vayne.Drawings;
using Adept_AIO.Champions.Vayne.Miscellaneous;
using Adept_AIO.SDK.Delegates;
using Adept_AIO.SDK.Unit_Extensions;
using Aimtec;

namespace Adept_AIO.Champions.Vayne
{
    class Vayne
    {
        public static void Init()
        {
            MenuConfig.Attach();
            SpellManager.Load();

            Game.OnUpdate += Manager.OnUpdate;
            Game.OnUpdate += Killsteal.OnUpdate;

            Gapcloser.OnGapcloser += AntiGapcloser.OnGapcloser;
            Global.Orbwalker.PostAttack += Manager.PostAttack;

            Render.OnPresent += DrawManager.OnPresent;
            Render.OnRender += DrawManager.OnRender;
        }
    }
}
