using System;
using Adept_AIO.Champions.Jax.Core;
using Adept_AIO.Champions.Jax.Update.Miscellaneous;
using Aimtec.SDK.Orbwalking;
using Aimtec.SDK.TargetSelector;

namespace Adept_AIO.Champions.Jax.Update.OrbwalkingEvents
{
    internal class Harass
    {
        public static void OnPostAttack()
        {
            if (!SpellConfig.W.Ready || !MenuConfig.Harass["W"].Enabled)
            {
                return;
            }

            SpellConfig.W.Cast();
            Orbwalker.Implementation.ResetAutoAttackTimer();
        }

        public static void OnUpdate()
        {
            var target = TargetSelector.GetTarget(SpellConfig.Q.Range);
            if (target == null)
            {
                return;
            }

            if (SpellConfig.E.Ready && MenuConfig.Harass["E"].Enabled)
            {
                SpellManager.CastE(target);
            }
            else if (SpellConfig.Q.Ready && MenuConfig.Harass["Q"].Enabled && Environment.TickCount - SpellConfig.CounterStrikeTime >= 1800 || SpellConfig.CounterStrikeTime <= 0)
            {
                SpellConfig.Q.CastOnUnit(target);
            }
        }
    }
}
