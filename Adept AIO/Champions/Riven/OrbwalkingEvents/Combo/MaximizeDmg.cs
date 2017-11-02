namespace Adept_AIO.Champions.Riven.OrbwalkingEvents.Combo
{
    using Aimtec;
    using Aimtec.SDK.Extensions;
    using Aimtec.SDK.Orbwalking;
    using Core;
    using Miscellaneous;
    using SDK.Unit_Extensions;

    class MaximizeDmg
    {
        public static void OnPostAttack(object sender, PostAttackEventArgs args)
        {
            var target = Global.TargetSelector.GetTarget(Global.Player.AttackRange + 100);
            if (target == null)
            {
                return;
            }

            if (SpellConfig.Q.Ready)
            {
                SpellManager.CastQ(target);
            }

            if (SpellConfig.W.Ready && !SpellConfig.Q.Ready)
            {
                SpellConfig.W.Cast();
            }
        }

        public static void OnUpdate(Obj_AI_Base target)
        {
            if (target == null)
            {
                return;
            }

            if (SpellConfig.E.Ready)
            {
                SpellConfig.E.Cast(target.ServerPosition);
            }
            else if (ComboManager.CanCastR1(target))
            {
                SpellConfig.R.Cast(target);
            }
            else if (SpellConfig.R2.Ready && Enums.UltimateMode == UltimateMode.Second && !SpellConfig.W.Ready && target.HealthPercent() <= 30)
            {
                SpellManager.CastR2(target);
            }
        }
    }
}