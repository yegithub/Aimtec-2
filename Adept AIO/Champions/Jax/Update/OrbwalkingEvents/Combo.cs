using System;
using System.Linq;
using Adept_AIO.Champions.Jax.Core;
using Adept_AIO.Champions.Jax.Update.Miscellaneous;
using Aimtec;
using Aimtec.SDK.Extensions;
using Aimtec.SDK.Orbwalking;
using Aimtec.SDK.TargetSelector;
using Aimtec.SDK.Util.Cache;

namespace Adept_AIO.Champions.Jax.Update.OrbwalkingEvents
{
    internal class Combo
    {
        public static void OnPostAttack()
        {
            if (SpellConfig.W.Ready)
            {
                SpellConfig.W.Cast();
                Orbwalker.Implementation.ResetAutoAttackTimer();
            }
            var target = GameObjects.EnemyHeroes.FirstOrDefault(x => x.IsValidAutoRange());
            if (target == null)
            {
                return;
            }
            if (SpellConfig.R.Ready && Dmg.Damage(target) * 2 > target.Health || target.HealthPercent() <= 40)
            {
                SpellConfig.R.Cast();
            }
        }

        public static void OnUpdate()
        {
            var target = TargetSelector.GetTarget(SpellConfig.Q.Range + 50);
            if (target == null)
            {
                return;
            }

            if (SpellConfig.R.Ready && ObjectManager.GetLocalPlayer().CountEnemyHeroesInRange(SpellConfig.Q.Range) >= MenuConfig.Combo["R"].Value)
            {
                SpellConfig.R.Cast();
            }

            if (SpellConfig.E.Ready)
            {
                SpellManager.CastE(target);
            }

            if (MenuConfig.Combo["Jump"].Enabled && !(SpellConfig.E.Ready || Dmg.Damage(target) > target.Health * 0.75f))
            {
                return;
            }

            if (MenuConfig.Combo["Delay"].Enabled && (Environment.TickCount - SpellConfig.CounterStrikeTime < 1500 || SpellConfig.E.Ready && SpellConfig.CounterStrikeTime == 0))
            {
                return; 
            }

            if (target.Distance(ObjectManager.GetLocalPlayer()) > SpellConfig.Q.Range)
            {
                var minion = GameObjects.EnemyMinions.FirstOrDefault(x => x.IsValid &&
                                                                          x.Distance(target) < 300 &&
                                                                          x.Distance(target) < ObjectManager.GetLocalPlayer().Distance(target));
                if (minion != null)
                {
                    SpellConfig.Q.CastOnUnit(minion);
                }
            }
            else
            {
                SpellConfig.Q.CastOnUnit(target);
            }
        }
    }
}
