using Adept_AIO.Champions.Kayn.Core;
using Adept_AIO.Champions.Kayn.Drawings;
using Adept_AIO.Champions.Kayn.Update.Miscellaneous;
using Adept_AIO.SDK.Extensions;
using Aimtec;

namespace Adept_AIO.Champions.Kayn
{
    class Kayn
    {
        public static void Init()
        {
            MenuConfig.Attach();
            SpellConfig.Load();

            Game.OnUpdate += Killsteal.OnUpdate;
            Game.OnUpdate += Manager.OnUpdate;
            GlobalExtension.Orbwalker.PostAttack += Manager.PostAttack;

            Obj_AI_Base.OnPlayAnimation += Animation.OnPlayAnimation;

            Render.OnRender += DrawManager.RenderManager;
        }
    }
}
