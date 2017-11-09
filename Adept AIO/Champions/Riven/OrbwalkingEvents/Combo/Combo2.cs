namespace Adept_AIO.Champions.Riven.OrbwalkingEvents.Combo
{
    using Aimtec.SDK.Events;
    using Aimtec.SDK.Extensions;
    using Aimtec.SDK.Orbwalking;
    using Core;
    using Miscellaneous;
    using SDK.Unit_Extensions;

    class Combo2
    {
        public static void OnPostAttack()
        {
            var target = Global.TargetSelector.GetTarget(Extensions.EngageRange);
            if (target == null)
            {
                return;
            }

            if (!SpellConfig.W.Ready && (!SpellConfig.Q.Ready || Extensions.CurrentQCount == 3) && SpellConfig.R2.Ready && Enums.UltimateMode == UltimateMode.Second && MenuConfig.Combo["R2"].Enabled && target.HealthPercent() <= 40)
            {
                SpellManager.CastR2(target);
            }

            if (SpellConfig.Q.Ready)
            {
                SpellManager.CastQ(target);
            }

        }

        public static void OnUpdate()
        {
            var target = Global.TargetSelector.GetTarget(Extensions.EngageRange);
            if (target == null)
            {
                return;
            }

            if (target.IsValidAutoRange() && SpellConfig.W.Ready)
            {
                SpellManager.CastW(target);
            }

            else if (SpellConfig.E.Ready && !target.IsValidAutoRange())
            {
                SpellConfig.E.Cast(target);
            }

            else if (SpellConfig.R.Ready && Enums.UltimateMode == UltimateMode.First && ComboManager.CanCastR1(target))
            {
                if (Global.Player.IsDashing() && Global.Player.GetDashInfo().EndPos.Distance(target) <= Global.Player.AttackRange + 50)
                {
                    return;
                }

                SpellConfig.R.Cast();
            }
        }
    }
}
