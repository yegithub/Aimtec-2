using System.Linq;
using Adept_AIO.Champions.LeeSin.Core;
using Adept_AIO.SDK.Extensions;
using Aimtec;
using Aimtec.SDK.Extensions;

namespace Adept_AIO.Champions.LeeSin.Update.OrbwalkingEvents
{
    class Harass
    {
        public static void OnPostAttack()
        {
            if (SpellConfig.E.Ready && MenuConfig.Harass["E2"].Enabled && !Extension.IsFirst(SpellConfig.E))
            {
                SpellConfig.E.Cast();
            }
            else if (SpellConfig.W.Ready && MenuConfig.Harass["Mode"].Enabled && MenuConfig.Harass["W"].Value == 1)
            {
                SpellConfig.W.CastOnUnit(ObjectManager.GetLocalPlayer());
            }
        }

        public static void OnUpdate()
        {
            var target = GlobalExtension.TargetSelector.GetTarget(SpellConfig.Q.Range);
            if (target == null)
            {
                return;
            }

            if (SpellConfig.Q.Ready && MenuConfig.Harass["Q"].Enabled)
            {
                if (Extension.IsQ2 && MenuConfig.Harass["Q2"].Enabled || !Extension.IsQ2)
                {
                    SpellConfig.Q.Cast(target);
                }
            }

            if (SpellConfig.E.Ready)
            {
                if (Extension.IsFirst(SpellConfig.E) && MenuConfig.Harass["E"].Enabled && target.IsValidTarget(SpellConfig.E.Range))
                {
                    SpellConfig.E.Cast();
                }
            }

            if (SpellConfig.W.Ready && Extension.IsFirst(SpellConfig.W) && !SpellConfig.E.Ready && !SpellConfig.Q.Ready && MenuConfig.Harass["Mode"].Value == 0)
            {
               
            }
        }
    }
}
