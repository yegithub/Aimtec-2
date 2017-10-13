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
            var target = TargetSelector.GetTarget(SpellManager.WCastRange + SpellManager.Q.Range);
            if (target == null || Maths.GetEnergyPercent() < MenuConfig.Harass["Energy"].Value)
            {
                return;
            }

            if (SpellManager.E.Ready && MenuConfig.Harass["E"].Enabled)
            {
                SpellManager.CastE(target);
            }

            if (SpellManager.W.Ready && target.IsValidTarget(SpellManager.WCastRange))
            {
                if (ShadowManager.CanCastW1() && MenuConfig.Harass["W"].Enabled)
                {
                    SpellManager.CastW(target);
                }

                else if (ShadowManager.CanSwitchToShadow() && MenuConfig.Harass["W2"].Enabled && Global.Player.HealthPercent() >= MenuConfig.Harass["Health"].Value && !target.IsUnderEnemyTurret())
                {
                    SpellManager.CastW(target, true);
                }
            }

            if (SpellManager.Q.Ready && MenuConfig.Harass["Q"].Enabled)
            {
                SpellManager.CastQ(target);
            }
        }
    }
}
