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
            if (SpellConfig.W.Ready && MenuConfig.Combo["W"].Enabled && MenuConfig.Combo["Ward"].Enabled)
            {
                var target = GlobalExtension.TargetSelector.GetTarget(SpellConfig.Q.Ready ? 1750 : 600);
                if (target == null || ObjectManager.GetLocalPlayer().Distance(target) < 500)
                {
                    return;
                }

                var objects = WardManager.JumpableObjects.FirstOrDefault();
                if (objects != null)
                {
                    SpellConfig.W.CastOnUnit(objects);
                }
                else if (WardManager.CanCastWard && WardManager.CanWardJump)
                {
                    WardManager.WardJump(target.Position);
                }
            }

            if (SpellConfig.Q.Ready && MenuConfig.Combo["Q"].Enabled)
            {
                var target = GlobalExtension.TargetSelector.GetTarget(1300);
                if (target == null)
                {
                    return;
                }

                if (Extension.IsQ2)
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

            if (SpellConfig.E.Ready && MenuConfig.Combo["E"].Enabled)
            {
                var target = GlobalExtension.TargetSelector.GetTarget(500);
                if (target == null)
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

            if (SpellConfig.R.Ready && MenuConfig.Combo["R"].Enabled)
            {
                var target = GlobalExtension.TargetSelector.GetTarget(SpellConfig.R.Range);
                if (target == null)
                {
                    return;
                }

                var count = SpellConfig.R2.GetPrediction(target).CollisionObjects.Count;
                if (count >= MenuConfig.Combo["Count"].Value)
                {
                    SpellConfig.R.CastOnUnit(target);
                }
            }
        }
    }
}
