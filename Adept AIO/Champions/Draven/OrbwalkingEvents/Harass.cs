namespace Adept_AIO.Champions.Draven.OrbwalkingEvents
{
    using Aimtec;
    using Aimtec.SDK.Extensions;
    using Core;
    using SDK.Unit_Extensions;

    class Harass
    {
        public static void OnUpdate()
        {
            var target = Global.TargetSelector.GetTarget(SpellManager.E.Range);
            if (target == null)
            {
                return;
            }

            if (SpellManager.E.Ready && MenuConfig.Harass["E"].Enabled && Global.Player.ManaPercent() >= MenuConfig.Harass["E"].Value)
            {
                SpellManager.CastE(target);
            }

            if ((SpellManager.Q.Ready || SpellManager.Q.IsCharging) && MenuConfig.Harass["Q"].Enabled && Global.Player.ManaPercent() >= MenuConfig.Harass["Q"].Value)
            {
                SpellManager.CastQ();
            }

            if (SpellManager.W.Ready &&
                MenuConfig.Harass["W"].Enabled &&
                Global.Player.ManaPercent() >= MenuConfig.Harass["W"].Value &&
                (!target.IsValidAutoRange() || Global.Player.HasBuffOfType(BuffType.Slow)))
            {
                SpellManager.CastW();
            }
        }
    }
}