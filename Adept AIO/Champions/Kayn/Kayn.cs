using Adept_AIO.Champions.Kayn.Core;
using Adept_AIO.Champions.Kayn.Drawings;
using Adept_AIO.Champions.Kayn.Miscellaneous;
using Adept_AIO.SDK.Unit_Extensions;
using Aimtec;

namespace Adept_AIO.Champions.Kayn
{
    internal class Kayn
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
