using Adept_AIO.Champions.Riven.Core;
using Adept_AIO.Champions.Riven.Drawings;
using Adept_AIO.Champions.Riven.Update.Miscellaneous;
using Aimtec;
using Aimtec.SDK.Orbwalking;

namespace Adept_AIO.Champions.Riven
{
    internal class Riven
    {
        public static void Init()
        {
            MenuConfig.Attach();
            SpellConfig.Load();

            Game.OnUpdate += Manager.OnUpdate;
            Game.OnUpdate += SpellManager.OnUpdate;
            Game.OnUpdate += Killsteal.OnUpdate;

            Obj_AI_Base.OnPlayAnimation    += Animation.OnPlayAnimation;
            Obj_AI_Base.OnProcessSpellCast += SpellManager.OnProcessSpellCast;
            Obj_AI_Base.OnProcessSpellCast += SafetyMeasure.OnProcessSpellCast;

            Orbwalker.Implementation.PostAttack += Manager.PostAttack;
            Render.OnRender += DrawManager.RenderManager;
        }
    }
}
