using Adept_AIO.Champions.Rengar.Core;
using Adept_AIO.Champions.Rengar.Drawings;
using Adept_AIO.Champions.Rengar.Update.Miscellaneous;
using Aimtec;
using Aimtec.SDK.Orbwalking;

namespace Adept_AIO.Champions.Rengar
{
    class Rengar
    {
        public static void Init()
        {
            MenuConfig.Attach();
            SpellConfig.Load();

            Game.OnUpdate += Manager.OnUpdate;
            Orbwalker.Implementation.PostAttack += Manager.PostAttack;
            Render.OnRender += DrawManager.RenderManager;
        }
    }
}
