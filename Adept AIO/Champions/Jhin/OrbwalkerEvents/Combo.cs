namespace Adept_AIO.Champions.Jhin.OrbwalkerEvents
{
    using Core;
    using SDK.Unit_Extensions;

    class Combo
    {
        public static void OnUpdate()
        {
            var target = Global.TargetSelector.GetTarget(Global.Player.AttackRange + 200);
            if (target == null)
            {
                return;
            }

            if (SpellManager.Q.Ready && MenuConfig.Combo["Q"].Enabled)
            {
                SpellManager.CastQ(target);
            }

            if (SpellManager.E.Ready && MenuConfig.Combo["E"].Enabled)
            {
                SpellManager.CastE(target);
            }

            if (SpellManager.R.Ready && MenuConfig.Combo["R"].Enabled)
            {
                SpellManager.CastR(target);
            }
        }
    }
}
