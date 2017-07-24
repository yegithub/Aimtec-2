using Adept_AIO.Champions.Yasuo.Core;
using Adept_AIO.Champions.Yasuo.Drawings;
using Adept_AIO.Champions.Yasuo.Update.Miscellaneous;
using Aimtec;
using Aimtec.SDK.Orbwalking;

namespace Adept_AIO.Champions.Yasuo
{
    class Yasuo
    {
        public static void Init()
        {
            MenuConfig.Attach();
            SpellConfig.Load();

            Game.OnUpdate += Manager.OnUpdate;
            Obj_AI_Base.OnPlayAnimation += Manager.OnPlayAnimation;
            Orbwalker.Implementation.PostAttack += Manager.PostAttack;
            Render.OnRender += DrawManager.RenderManager;
            BuffManager.OnAddBuff += Manager.BuffManagerOnOnAddBuff;
            BuffManager.OnRemoveBuff += Manager.BuffManagerOnOnRemoveBuff;
        }
    }
}
