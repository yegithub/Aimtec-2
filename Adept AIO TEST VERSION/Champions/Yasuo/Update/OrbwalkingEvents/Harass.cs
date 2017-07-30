﻿using Adept_AIO.Champions.Yasuo.Core;
using Adept_AIO.SDK.Extensions;
using Aimtec.SDK.Extensions;

namespace Adept_AIO.Champions.Yasuo.Update.OrbwalkingEvents
{
    internal class Harass
    {
        public static void OnPostAttack()
        {

        }

        public static void OnUpdate()
        {
            if (GlobalExtension.Orbwalker.IsWindingUp)
            {
                return;
            }

            var target = GlobalExtension.TargetSelector.GetTarget(1100);
            if (target == null || GlobalExtension.Orbwalker.IsWindingUp)
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
                var distance = target.Distance(GlobalExtension.Player);
                var minion = Extension.GetDashableMinion(target);

                if (!target.HasBuff("YasuoDashWrapper") && target.IsValidTarget(SpellConfig.E.Range))
                {
                    SpellConfig.E.CastOnUnit(target);
                }
                else if (distance > SpellConfig.E.Range)
                {
                    if (minion != null || distance < GlobalExtension.Player.AttackRange)
                    {
                        SpellConfig.E.CastOnUnit(minion);
                    }
                }
            }
        }
    }
}