namespace Adept_AIO.Champions.Vayne.OrbwalkingEvents
{
    using Aimtec;
    using Aimtec.SDK.Extensions;
    using Aimtec.SDK.Orbwalking;
    using Core;
    using SDK.Unit_Extensions;

    class Harass
    {
        public static void PostAttack(object sender, PostAttackEventArgs args)
        {
            if (!SpellManager.Q.Ready || MenuConfig.Harass["Q"].Value == 1)
            {
                return;
            }

            var t = args.Target as Obj_AI_Base;
            if (t == null || !t.IsValidAutoRange())
            {
                return;
            }

            SpellManager.CastQ(t, MenuConfig.Harass["Mode"].Value);
        }

        public static void OnUpdate()
        {
            var target = Global.TargetSelector.GetTarget(SpellManager.E.Range);
            if (target == null || !target.IsValidTarget())
            {
                return;
            }

            if (SpellManager.Q.Ready && MenuConfig.Harass["Q"].Value == 1)
            {
                SpellManager.CastQ(target, MenuConfig.Harass["Mode"].Value);
            }

            if (SpellManager.E.Ready && MenuConfig.Harass["E"].Enabled && MenuConfig.Harass[target.ChampionName].Enabled)
            {
                SpellManager.CastE(target);
            }
        }
    }
}