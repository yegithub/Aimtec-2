using System;
using Adept_AIO.Champions.LeeSin.Core;
using Adept_AIO.Champions.LeeSin.Drawings;
using Adept_AIO.Champions.LeeSin.Update.Miscellaneous;
using Adept_AIO.Champions.LeeSin.Update.OrbwalkingEvents;
using Adept_AIO.SDK.Extensions;
using Aimtec;

namespace Adept_AIO.Champions.LeeSin
{
    internal class LeeSin
    {
        public static void Init()
        {
            MenuConfig.Attach();
            SpellConfig.Load();

            Game.OnUpdate += Manager.OnUpdate;
            Game.OnUpdate += Killsteal.OnUpdate;
            Game.OnUpdate += WardManager.OnUpdate;

            GlobalExtension.Orbwalker.PostAttack += Manager.PostAttack;

            Render.OnRender += DrawManager.RenderManager;

            Obj_AI_Base.OnProcessSpellCast += Insec.OnProcessSpellCast;
        }
    }
}
