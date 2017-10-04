using System.Linq;
using Adept_AIO.Champions.Riven.Core;
using Adept_AIO.Champions.Riven.Update.Miscellaneous;
using Adept_AIO.SDK.Unit_Extensions;
using Aimtec;
using Aimtec.SDK.Extensions;

namespace Adept_AIO.Champions.Riven.Update.OrbwalkingEvents.Combo
{
    internal class MaximizeDmg
    {
        public static void OnPostAttack()
        {
            var target = GameObjects.EnemyHeroes.FirstOrDefault(x => x.Distance(Global.Player) <= 600);
            if (target == null)
            {
                return;
            }

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
                SpellConfig.R.Cast(target);
            }
            else if (SpellConfig.R2.Ready && Enums.UltimateMode == UltimateMode.Second && !SpellConfig.W.Ready && target.HealthPercent() <= 30)
            {
                SpellManager.CastR2(target);
            }
        }
    }
}
