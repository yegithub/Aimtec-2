namespace Adept_AIO.Champions.Xerath.OrbwalkingEvents
{
    using System;
    using Aimtec.SDK.Extensions;
    using Core;
    using SDK.Unit_Extensions;

    class Combo
    {
        public static void OnUpdate()
        {
            var target = Global.TargetSelector.GetTarget(SpellManager.Q.ChargedMaxRange);
            if (target == null)
            {
                return;
            }

            if (SpellManager.E.Ready && MenuConfig.Combo["E"].Enabled)
            {
                SpellManager.CastE(target);
            }

            if (SpellManager.Q.Ready && MenuConfig.Combo["Q"].Enabled)
            {
              
                SpellManager.CastQ(target);
            }

            if (SpellManager.W.Ready && MenuConfig.Combo["W"].Enabled)
            {
                SpellManager.CastW(target);
            }
        }
    }
}