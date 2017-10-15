namespace Adept_AIO.Champions.Gragas.OrbwalkingEvents
{
    using Aimtec.SDK.Extensions;
    using Core;
    using SDK.Unit_Extensions;

    class Harass
    {
        public static void OnUpdate()
        {
            var target = Global.TargetSelector.GetTarget(SpellManager.Q.Range);
            if (target == null || !target.IsValidTarget())
            {
                return;
            }

            if (SpellManager.Q.Ready && MenuConfig.Harass["Q"].Enabled)
            {
                SpellManager.CastQ(target);
            }

            if (SpellManager.W.Ready && MenuConfig.Harass["W"].Enabled)
            {
                SpellManager.CastW(target);
            }

            if (SpellManager.E.Ready && MenuConfig.Harass["E"].Enabled)
            {
                SpellManager.CastE(target);
            }
        }
    }
}