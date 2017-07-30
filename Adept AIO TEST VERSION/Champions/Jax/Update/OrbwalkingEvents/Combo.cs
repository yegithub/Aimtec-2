using System;
using System.Linq;
using Adept_AIO.Champions.Jax.Core;
using Adept_AIO.Champions.Jax.Update.Miscellaneous;
using Adept_AIO.SDK.Extensions;
using Adept_AIO.SDK.Usables;
using Aimtec.SDK.Extensions;
using GameObjects = Aimtec.SDK.Util.Cache.GameObjects;

namespace Adept_AIO.Champions.Jax.Update.OrbwalkingEvents
{
    internal class Combo
    {
        public static void OnPostAttack()
        {
            if (SpellConfig.W.Ready)
            {
                SpellConfig.W.Cast();
                Items.CastTiamat();
                GlobalExtension.Orbwalker.ResetAutoAttackTimer();
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
            var target = GlobalExtension.TargetSelector.GetTarget(SpellConfig.Q.Range);
            if (target == null)
            {
                return;
            }

            if (SpellConfig.R.Ready && GlobalExtension.Player.CountEnemyHeroesInRange(SpellConfig.Q.Range) >= MenuConfig.Combo["R"].Value && MenuConfig.Combo["R"].Enabled)
            {
                SpellConfig.R.Cast();
            }

            if (SpellConfig.E.Ready && target.Distance(GlobalExtension.Player) <= MenuConfig.Combo["E"].Value && MenuConfig.Combo["E"].Enabled)
            {
                SpellManager.CastE(target);
            }

            if (MenuConfig.Combo["Jump"].Enabled && !(SpellConfig.E.Ready || Dmg.Damage(target) > target.Health * 0.75f))
            {
                return;
            }

            if (MenuConfig.Combo["Delay"].Enabled && (Environment.TickCount - SpellConfig.CounterStrikeTime < 1500 || SpellConfig.E.Ready && SpellConfig.CounterStrikeTime == 0f))
            {
                return; 
            }

            if (target.Distance(GlobalExtension.Player) > SpellConfig.Q.Range)
            {
                var minion = GameObjects.EnemyMinions.FirstOrDefault(x => x.IsValid &&
                                                                          x.Distance(target) < 300 &&
                                                                          x.Distance(target) < GlobalExtension.Player.Distance(target));
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
