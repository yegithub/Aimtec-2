using Adept_AIO.Champions.Riven.Core;
using Adept_AIO.Champions.Riven.Update.Miscellaneous;
using Aimtec;

namespace Adept_AIO.Champions.Riven.Update.OrbwalkingEvents.Combo
{
    class MaximizeDmg
    {
        public static void OnPostAttack(Obj_AI_Base target)
        {
            if (SpellConfig.Q.Ready)
            {
                SpellManager.CastQ(target);
            }
            else if (SpellConfig.W.Ready && Extensions.CurrentQCount == 1 && !SpellConfig.Q.Ready)
            {
                SpellManager.CastW(target);
            }
        }

        public static void OnUpdate(Obj_AI_Base target)
        {
            if (target == null)
            {
                return;
            }

            if (SpellConfig.E.Ready)
            {
                SpellConfig.E.Cast(target.ServerPosition);
            }
            else if (ComboManager.CanCastR1(target))
            {
                SpellConfig.R.Cast();
            }
            else if (SpellConfig.R2.Ready && Enums.UltimateMode == UltimateMode.Second && !SpellConfig.W.Ready)
            {
                SpellManager.CastR2(target);
            }
        }
    }
}
