using System;
using Adept_AIO.Champions.Kayn.Core;
using Adept_AIO.SDK.Extensions;
using Adept_AIO.SDK.Usables;
using Aimtec;
using Aimtec.SDK.Extensions;

namespace Adept_AIO.Champions.Kayn.Update.OrbwalkingEvents
{
    internal class Harass
    {
        public static void OnUpdate()
        {
            if (SpellConfig.W.Ready && MenuConfig.Harass["W"].Enabled && MenuConfig.Harass["W"].Value <= ObjectManager.GetLocalPlayer().ManaPercent())
            {
                var target = GlobalExtension.TargetSelector.GetTarget(SpellConfig.W.Range);
                if (target != null)
                {
                    SpellConfig.W.Cast(target);
                }
            }

            if (SpellConfig.Q.Ready && MenuConfig.Harass["Q"].Enabled && MenuConfig.Harass["Q"].Value <= ObjectManager.GetLocalPlayer().ManaPercent())
            {
                var target = GlobalExtension.TargetSelector.GetTarget(SpellConfig.Q.Range);
                if (target != null)
                {
                    SpellConfig.Q.Cast(target);
                }
            }
        }
    }
}
