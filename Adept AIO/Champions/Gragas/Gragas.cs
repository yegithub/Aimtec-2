using System;

namespace Adept_AIO.Champions.Gragas
{
    using Core;
    using Drawings;
    using Miscellaneous;
    using Aimtec;
    using Aimtec.SDK.Orbwalking;

    class Gragas
    {
        public static OrbwalkerMode InsecOrbwalkerMode;

        public static void Init()
        {
            new SpellManager();
            new MenuConfig();

            Obj_AI_Base.OnProcessSpellCast += SpellManager.OnProcessSpellCast;
            GameObject.OnDestroy += SpellManager.OnDestroy;
           
            Game.OnUpdate += Manager.OnUpdate;
            Game.OnUpdate += Automatic.OnUpdate;

            Render.OnPresent += DrawManager.OnPresent;
            Render.OnPresent += DrawManager.OnRender;
        }
    }
}
