using System;
using System.Linq;
using Adept_AIO.Champions.LeeSin.Core;
using Adept_AIO.SDK.Extensions;
using Aimtec;
using Aimtec.SDK.Extensions;

namespace Adept_AIO.Champions.LeeSin.Update.OrbwalkingEvents
{
    class Combo
    {
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
            var target = GlobalExtension.TargetSelector.GetTarget(SpellConfig.Q.Ready ? 1750 : 850);
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
                    SpellConfig.Q.Cast();
                }
                else
                {
                    SpellConfig.Q.Cast(target);
                }
            }
            else if (SpellConfig.W.Ready && MenuConfig.Combo["W"].Enabled && MenuConfig.Combo["Ward"].Enabled && distance > (SpellConfig.Q.Ready ? 1750 : 600))
            {
                WardManager.Jump(target.ServerPosition);
            }

            if (SpellConfig.E.Ready && MenuConfig.Combo["E"].Enabled)
            {
                if (distance > 500)
                {
                    return;
                }

                if (Extension.IsFirst(SpellConfig.E))
                {
                    SpellConfig.E.Cast(target);
                }
                else 
                {
                    SpellConfig.E.Cast();
                }
            }
        }
    }
}
