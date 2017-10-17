namespace Adept_AIO.Champions.Yasuo.OrbwalkingEvents
{
    using Aimtec.SDK.Extensions;
    using Core;
    using SDK.Unit_Extensions;

    class Harass
    {
        public static void OnUpdate()
        {
            var target = Global.TargetSelector.GetTarget(1100);
            if (target == null || Global.Orbwalker.IsWindingUp)
            {
                return;
            }

            if (SpellConfig.Q.Ready && target.IsValidTarget(SpellConfig.Q.Range))
            {
                if (Extension.CurrentMode == Mode.Tornado && !MenuConfig.Harass["Q"].Enabled)
                {
                    return;
                }

                SpellConfig.Q.Cast(target);
            }

            if (SpellConfig.E.Ready && MenuConfig.Harass["E"].Enabled && !target.IsUnderEnemyTurret())
            {
                var distance = target.Distance(Global.Player);
                var minion = MinionHelper.GetDashableMinion(target);

                if (MinionHelper.IsDashable(target))
                {
                    SpellConfig.E.CastOnUnit(target);
                }

                if (minion != null)
                {
                    SpellConfig.E.CastOnUnit(minion);
                }
            }
        }
    }
}