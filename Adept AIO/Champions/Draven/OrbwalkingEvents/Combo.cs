namespace Adept_AIO.Champions.Draven.OrbwalkingEvents
{
    using Aimtec;
    using Aimtec.SDK.Extensions;
    using Core;
    using SDK.Unit_Extensions;

    class Combo
    {
        public static void OnUpdate()
        {
            var target = Global.TargetSelector.GetTarget(SpellManager.E.Range);
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
                SpellManager.CastQ();
            }

            if (SpellManager.W.Ready && MenuConfig.Combo["W"].Enabled && (!target.IsValidAutoRange() || Global.Player.HasBuffOfType(BuffType.Slow)))
            {
                SpellManager.CastW();
            }
        }
    }
}