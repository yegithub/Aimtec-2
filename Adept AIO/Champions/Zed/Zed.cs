namespace Adept_AIO.Champions.Zed
{
    using System.Collections.Generic;
    using Aimtec;
    using Core;
    using Drawings;
    using Miscellaneous;
    using OrbwalkingEvents;
    using SDK.Delegates;

    class Zed
    {
        public static void Init()
        {
            MenuConfig.Attach();
            SpellManager.Load();

            Game.OnUpdate += Killsteal.OnUpdate;
            Game.OnUpdate += Manager.OnUpdate;

            Render.OnPresent += DrawManager.OnPresent;
            Render.OnRender += DrawManager.OnRender;

            Gapcloser.OnGapcloser += AntiGapcloser.OnGapcloser;

            Obj_AI_Base.OnProcessSpellCast += SpellManager.OnProcessSpellCast;
            Obj_AI_Base.OnProcessSpellCast += LaneClear.OnProcessSpellCast;

            ShadowManager.Shadows = new List<Obj_AI_Minion>();
            GameObject.OnCreate += ShadowManager.OnCreate;
            GameObject.OnDestroy += ShadowManager.OnDelete;
        }
    }
}