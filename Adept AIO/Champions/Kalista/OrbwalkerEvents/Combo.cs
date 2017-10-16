namespace Adept_AIO.Champions.Kalista.OrbwalkerEvents
{
    using System;
    using System.Linq;
    using Aimtec;
    using Aimtec.SDK.Extensions;
    using Core;
    using SDK.Unit_Extensions;

    class Combo
    {
        public static void OnUpdate()
        {
            var target = Global.TargetSelector.GetTarget(SpellManager.R.Range);
            if (target == null || !target.IsValidTarget())
            {
                return;
            }

            if (SpellManager.Q.Ready && MenuConfig.Combo["Q"].Enabled)
            {
                SpellManager.CastQ(target);
            }

            if (SpellManager.R.Ready && MenuConfig.Combo["R"].Enabled && target.CountEnemyHeroesInRange(1100) >= MenuConfig.Combo["R"].Value)
            {
                SpellManager.R.Cast();
            }
        }
    }
}
