namespace Adept_AIO.Champions.Zed.OrbwalkingEvents
{
    using Aimtec;
    using Aimtec.SDK.Extensions;
    using Core;
    using SDK.Generic;
    using SDK.Unit_Extensions;

    class Harass
    {
        public static void OnUpdate()
        {
            var target = Global.TargetSelector.GetTarget(SpellManager.WCastRange + SpellManager.Q.Range);
            if (target == null || Maths.GetEnergyPercent() < MenuConfig.Harass["Energy"].Value)
            {
                return;
            }

            if (SpellManager.E.Ready && MenuConfig.Harass["E"].Enabled)
            {
                SpellManager.CastE(target);
            }

            if (SpellManager.W.Ready && target.IsValidTarget(SpellManager.WCastRange + SpellManager.E.Range))
            {
                if (ShadowManager.CanCastFirst(SpellSlot.W) && MenuConfig.Harass["W"].Enabled)
                {
                    SpellManager.CastW(target);
                }

                else if (ShadowManager.CanSwitchToShadow(SpellSlot.W) && MenuConfig.Harass["W2"].Enabled && Global.Player.HealthPercent() >= MenuConfig.Harass["Health"].Value &&
                         !target.IsUnderEnemyTurret())
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