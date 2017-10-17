namespace Adept_AIO.Champions.Twitch.OrbwalkingEvents
{
    using System.Linq;
    using Aimtec.SDK.Extensions;
    using Core;
    using SDK.Unit_Extensions;

    class Harass
    {
        public static void OnUpdate()
        {
            var target = Global.TargetSelector.GetTarget(SpellManager.W.Range);
            if (target == null)
            {
                return;
            }

            if (SpellManager.Q.Ready && MenuConfig.Harass["Q"].Enabled)
            {
                SpellManager.Q.Cast();
            }

            if (SpellManager.W.Ready && MenuConfig.Harass["W"].Enabled && target.HasBuff("twitchdeadlyvenom"))
            {
                SpellManager.W.Cast(target);
            }
        }
    }
}