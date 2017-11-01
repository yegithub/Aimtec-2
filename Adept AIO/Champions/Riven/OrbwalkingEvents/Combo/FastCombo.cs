namespace Adept_AIO.Champions.Riven.OrbwalkingEvents.Combo
{
    using Aimtec;
    using Aimtec.SDK.Extensions;
    using Aimtec.SDK.Orbwalking;
    using Core;
    using Miscellaneous;
    using SDK.Unit_Extensions;

    class FastCombo
    {
        public static void OnPostAttack(object sender, PostAttackEventArgs args)
        {
            var target = Global.TargetSelector.GetTarget(Global.Player.AttackRange + 100);
            if (target == null)
            {
                return;
            }

            if (SpellConfig.R2.Ready && Enums.UltimateMode == UltimateMode.Second && MenuConfig.Combo["R2"].Enabled && target.HealthPercent() <= 40)
            {
                SpellManager.CastR2(target);
            }

            if (SpellConfig.Q.Ready && SpellConfig.W.Ready)
            {
                SpellManager.CastWq(target);
            }
            else
            {
                if (target.IsValidTarget(SpellConfig.W.Range) && SpellConfig.W.Ready)
                {
                    SpellConfig.W.Cast();
                }

                if (SpellConfig.Q.Ready)
                {
                    SpellManager.CastQ(target);
                }
            }
        }

        public static void OnUpdate(Obj_AI_Base target)
        {
            if (target == null)
            {
                return;
            }

            if (SpellConfig.Q.Ready && target.IsInRange(Global.Player.AttackRange))
            {
                Global.Orbwalker.Attack(target); // Prevents E WQ (1s delay) -> AA. (BUG)
            }

            if (SpellConfig.E.Ready)
            {
                SpellConfig.E.Cast(target.ServerPosition);
            }

            if (ComboManager.CanCastR1(target))
            {
                SpellConfig.R.Cast(target);
            }
        }
    }
}