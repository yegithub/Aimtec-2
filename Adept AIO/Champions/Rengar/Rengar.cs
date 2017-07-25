using Adept_AIO.Champions.Rengar.Core;
using Adept_AIO.Champions.Rengar.Drawings;
using Adept_AIO.Champions.Rengar.Update.Miscellaneous;
using Adept_AIO.SDK.Extensions;
using Aimtec;

namespace Adept_AIO.Champions.Rengar
{
    class Rengar
    {
        public static void Init()
        {
            MenuConfig.Attach();
            SpellConfig.Load();

            Game.OnUpdate += Manager.OnUpdate;
            GlobalExtension.Orbwalker.PostAttack += Manager.PostAttack;
            Render.OnRender += DrawManager.RenderManager;
        }
    }
}
