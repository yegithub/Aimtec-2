using Adept_AIO.Champions.Jax.Core;
using Adept_AIO.Champions.Jax.Drawings;
using Adept_AIO.Champions.Jax.Update.Miscellaneous;
using Aimtec;
using Aimtec.SDK.Orbwalking;

namespace Adept_AIO.Champions.Jax
{
    internal class Jax
    {
        public static void Init()
        {
            MenuConfig.Attach();
            SpellConfig.Load();

            Game.OnUpdate += Manager.OnUpdate;
            Game.OnUpdate += SpellManager.OnUpdate;
            Game.OnUpdate += Killsteal.OnUpdate;
            Orbwalker.Implementation.PostAttack += Manager.PostAttack;
            Obj_AI_Base.OnPlayAnimation += Animation.OnPlayAnimation;
            Obj_AI_Base.OnProcessSpellCast += SpellManager.OnProcessSpellCast;
            Render.OnRender += DrawManager.RenderManager;
        }
    }
}
