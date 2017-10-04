using Adept_AIO.Champions.Yasuo.Core;
using Adept_AIO.SDK.Unit_Extensions;
using Aimtec.SDK.Extensions;

namespace Adept_AIO.Champions.Yasuo.Update.OrbwalkingEvents
{
    internal class Harass
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

                if (!target.HasBuff("YasuoDashWrapper") && target.IsValidTarget(SpellConfig.E.Range))
                {
                    SpellConfig.E.CastOnUnit(target);
                }
                else if (distance > SpellConfig.E.Range)
                {
                    if (minion != null || distance < Global.Player.AttackRange)
                    {
                        SpellConfig.E.CastOnUnit(minion);
                    }
                }
            }
        }
    }
}
