namespace Adept_AIO.Champions.Gnar.OrbwalkingEvents
{
    using Aimtec.SDK.Extensions;
    using Core;
    using SDK.Unit_Extensions;

    class Harass
    {
        public static void OnUpdate()
        {
            var target = Global.TargetSelector.GetTarget(SpellManager.Q.Range);
            if (target == null)
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

            if (SpellManager.E.Ready &&
                MenuConfig.Harass["E"].Enabled &&
                !Global.Player.ServerPosition.Extend(target.ServerPosition, SpellManager.E.Range * 2).PointUnderEnemyTurret())
            {
                SpellManager.CastE(target);
            }
        }
    }
}