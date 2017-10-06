using System.Collections.Generic;
using Adept_AIO.Champions.Zed.Core;
using Adept_AIO.Champions.Zed.Drawings;
using Adept_AIO.Champions.Zed.Miscellaneous;
using Adept_AIO.SDK.Delegates;
using Aimtec;

namespace Adept_AIO.Champions.Zed
{
    internal class Zed
    {
        public static void Init()
        {
            MenuConfig.Attach();
            SpellManager.Load();

            
            Game.OnUpdate += Manager.OnUpdate;

            Render.OnPresent += DrawManager.OnPresent;
            Render.OnRender += DrawManager.OnRender;

            Gapcloser.OnGapcloser += AntiGapcloser.OnGapcloser;

            Obj_AI_Base.OnProcessSpellCast += SpellManager.Cast;

            ShadowManager.Shadows = new List<Obj_AI_Minion>();
            GameObject.OnCreate += ShadowManager.OnCreate;
            GameObject.OnDestroy += ShadowManager.OnDelete;
        }
    }
}
