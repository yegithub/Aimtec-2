namespace Adept_AIO.Champions.Zoe.OrbwalkingEvents
{
    using Aimtec.SDK.Extensions;
    using Core;
    using SDK.Unit_Extensions;

    class Combo
    {
        public static void OnUpdate()
        {
            var target = Global.TargetSelector.GetTarget(SpellManager.PaddleStar.IsZero ? 1000 : int.MaxValue);
            if (target == null)
            {
                return;
            }

            if (SpellManager.Q.Ready &&
                MenuConfig.Combo["Q"].Enabled)
            {
                if (SpellManager.R.Ready &&
                    MenuConfig.Combo["R"].Enabled && target.Distance(Global.Player) < Global.Player.AttackRange)
                {
                    SpellManager.CastR(target);
                }

                SpellManager.CastQ(target);
            }

            if (SpellManager.E.Ready &&
                MenuConfig.Combo["E"].Enabled)
            {
                SpellManager.CastE(target);
            }

            if (SpellManager.W.Ready &&
                MenuConfig.Combo["W"].Enabled)
            {
                SpellManager.CastW(target);
            }
        }
    }
}