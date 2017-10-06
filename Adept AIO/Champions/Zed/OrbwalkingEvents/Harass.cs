using Adept_AIO.Champions.Zed.Core;
using Adept_AIO.SDK.Generic;
using Adept_AIO.SDK.Unit_Extensions;
using Aimtec.SDK.Extensions;
using Aimtec.SDK.TargetSelector;

namespace Adept_AIO.Champions.Zed.OrbwalkingEvents
{
    internal class Harass
    {
        public static void OnUpdate()
        {
            var target = TargetSelector.GetTarget(SpellManager.WCastRange + Global.Player.AttackRange);
            if (target == null || Maths.GetEnergyPercent() < MenuConfig.Harass["Energy"].Value)
            {
                return;
            }

            if (SpellManager.W.Ready && MenuConfig.Harass["W"].Enabled)
            {
                if (ShadowManager.CanCastW1())
                {
                    SpellManager.W.Cast(target.ServerPosition);
                }
                else if (ShadowManager.CanSwitchToShadow() && MenuConfig.Harass["W2"].Enabled && Global.Player.HealthPercent() >= MenuConfig.Harass["Health"].Value)
                {
                    SpellManager.W.Cast();
                }
            }

            if (SpellManager.Q.Ready && MenuConfig.Harass["Q"].Enabled)
            {
                SpellManager.CastQ(target);
            }

            if (SpellManager.E.Ready && MenuConfig.Harass["E"].Enabled)
            {
                SpellManager.CastE(target);
            }
        }
    }
}
