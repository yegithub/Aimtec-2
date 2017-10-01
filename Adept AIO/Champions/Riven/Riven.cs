using Adept_AIO.Champions.Riven.Core;
using Adept_AIO.Champions.Riven.Drawings;
using Adept_AIO.Champions.Riven.Update.Miscellaneous;
using Adept_AIO.SDK.Delegates;
using Adept_AIO.SDK.Unit_Extensions;
using Aimtec;

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
            Gapcloser.OnGapcloser += AntiGapcloser.OnGapcloser;

            Obj_AI_Base.OnProcessSpellCast += SpellManager.OnProcessSpellCast;
            Obj_AI_Base.OnProcessSpellCast += SafetyMeasure.OnProcessSpellCast;
            Obj_AI_Base.OnProcessAutoAttack += Animation.OnProcessAutoAttack;
            Obj_AI_Base.OnPlayAnimation += Animation.OnPlayAnimation;

            Global.Orbwalker.PostAttack += Manager.PostAttack;
            Render.OnRender += DrawManager.RenderBasics;
            Render.OnPresent += DrawManager.OnPresent;   
        }
    }
}
