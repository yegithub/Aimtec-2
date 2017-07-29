using System.Linq;
using Adept_AIO.Champions.LeeSin.Core;
using Adept_AIO.Champions.LeeSin.Update.Miscellaneous;
using Adept_AIO.SDK.Extensions;
using Aimtec;
using Aimtec.SDK.Extensions;

namespace Adept_AIO.Champions.LeeSin.Update.OrbwalkingEvents
{
    internal class Harass
    {
        public static void OnPostAttack(AttackableUnit target)
        {
            if (target == null || !target.IsHero)
            {
                return;
            }
            if (SpellConfig.E.Ready && MenuConfig.Harass["E2"].Enabled && !Extension.IsFirst(SpellConfig.E))
            {
                SpellConfig.E.Cast();
            }
            else if (SpellConfig.W.Ready && MenuConfig.Harass["Mode"].Value == 1)
            {
                SpellConfig.W.CastOnUnit(GlobalExtension.Player);
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
                    SpellConfig.CastE(target);
                }
            }

            if (SpellConfig.W.Ready && Extension.IsFirst(SpellConfig.W) && !SpellConfig.E.Ready && !SpellConfig.Q.Ready && MenuConfig.Harass["Mode"].Value == 0)
            {
                var pos = GlobalExtension.Player.ServerPosition + (GlobalExtension.Player.ServerPosition + target.ServerPosition) * 300;
                WardManager.WardJump(pos, true);
            }
        }
    }
}
