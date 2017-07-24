using System;
using Adept_AIO.Champions.Irelia.Core;
using Adept_AIO.Champions.Irelia.Drawings;
using Adept_AIO.Champions.Irelia.Update.Miscellaneous;
using Aimtec;
using Aimtec.SDK.Orbwalking;

namespace Adept_AIO.Champions.Irelia
{
    internal class Irelia
    {
        public static void Init()
        {
            MenuConfig.Attach();
            SpellConfig.Load();

            Game.OnUpdate += Manager.OnUpdate;
            Game.OnUpdate += Killsteal.OnUpdate;
            Obj_AI_Base.OnProcessSpellCast += Manager.OnProcessSpellCast;
            Orbwalker.Implementation.PostAttack += Manager.PostAttack;
            Orbwalker.Implementation.PreAttack  += Manager.OnPreAttack;
            Render.OnRender += DrawManager.RenderManager;
        }
    }
}
