namespace Adept_AIO.Champions.Kayn.OrbwalkingEvents
{
    using Aimtec.SDK.Extensions;
    using Core;
    using SDK.Unit_Extensions;

    class Harass
    {
        public static void OnUpdate()
        {
            if (SpellConfig.W.Ready &&
                MenuConfig.Harass["W"].Enabled &&
                MenuConfig.Harass["W"].Value <= Global.Player.ManaPercent())
            {
                var target = Global.TargetSelector.GetTarget(SpellConfig.W.Range);
                if (target != null)
                {
                    SpellConfig.W.Cast(target);
                }
            }

            if (SpellConfig.Q.Ready &&
                MenuConfig.Harass["Q"].Enabled &&
                MenuConfig.Harass["Q"].Value <= Global.Player.ManaPercent())
            {
                var target = Global.TargetSelector.GetTarget(SpellConfig.Q.Range);
                if (target != null)
                {
                    SpellConfig.Q.Cast(target);
                }
            }
        }
    }
}