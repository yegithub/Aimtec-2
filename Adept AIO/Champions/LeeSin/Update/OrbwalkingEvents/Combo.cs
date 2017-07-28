using System;
using System.Linq;
using Adept_AIO.Champions.LeeSin.Core;
using Adept_AIO.SDK.Extensions;
using Adept_AIO.SDK.Usables;
using Aimtec;
using Aimtec.SDK.Extensions;

namespace Adept_AIO.Champions.LeeSin.Update.OrbwalkingEvents
{
    internal class Combo
    {
        private static float LastQTime;

        public static void OnPostAttack(AttackableUnit target)
        {
            if (target == null)
            {
                return;
            }

            if (SpellConfig.W.Ready && MenuConfig.Combo["W"].Enabled)
            {
                SpellConfig.W.Cast(ObjectManager.GetLocalPlayer());
            }
            else if (SpellConfig.E.Ready && MenuConfig.Combo["E"].Enabled)
            {
                if (!Extension.IsFirst(SpellConfig.E))
                {
                    SpellConfig.E.Cast();
                }
            }
        }

        public static void OnUpdate()
        {
            var target = GlobalExtension.TargetSelector.GetTarget(1600);
            if (target == null)
            {
                return;
            }

            var distance = target.Distance(ObjectManager.GetLocalPlayer());

            if (SpellConfig.Q.Ready && MenuConfig.Combo["Q"].Enabled)
            {
                if (distance > 1300)
                {
                    return;
                }

                if (Extension.HasQ2(target))
                {
                    if (MenuConfig.Combo["Turret"].Enabled && target.IsUnderEnemyTurret())
                    {
                        return;
                    }
                    LastQTime = Environment.TickCount;
                    SpellConfig.Q.Cast();
                }
                else
                {
                    Extension.QSmite(target);
                    LastQTime = Environment.TickCount;
                    SpellConfig.Q.Cast(target);
                }
            }
            else if (SpellConfig.W.Ready && Extension.IsFirst(SpellConfig.W) && MenuConfig.Combo["W"].Enabled && MenuConfig.Combo["Ward"].Enabled && distance > (SpellConfig.Q.Ready ? 1000 : 600))
            {
                if (Environment.TickCount - LastQTime <= 1000 && LastQTime > 0)
                {
                    return;
                }
                WardManager.WardJump(target.Position, true);
            }

            if (SpellConfig.E.Ready && MenuConfig.Combo["E"].Enabled && Extension.IsFirst(SpellConfig.E) && distance <= 350)
            {
                Items.CastTiamat();
                SpellConfig.E.Cast(target);
            }
        }
    }
}
