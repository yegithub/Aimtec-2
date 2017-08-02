using System.Linq;
using Adept_AIO.Champions.Jax.Core;
using Adept_AIO.SDK.Extensions;
using Adept_AIO.SDK.Usables;
using Aimtec.SDK.Extensions;
using GameObjects = Adept_AIO.SDK.Extensions.GameObjects;

namespace Adept_AIO.Champions.Jax.Update.OrbwalkingEvents
{
    internal class Clear
    {
        public static void OnPostAttack()
        {
            if (MenuConfig.Clear["Check"].Enabled && GlobalExtension.Player.CountEnemyHeroesInRange(1500) > 0)
            {
                return;
            }

            if (SpellConfig.W.Ready && MenuConfig.Clear["W"].Enabled)
            {
                SpellConfig.W.Cast();
                Items.CastTiamat();
                GlobalExtension.Orbwalker.ResetAutoAttackTimer();
            }

            if (SpellConfig.E.Ready && MenuConfig.Clear["E"].Enabled && GlobalExtension.Player.ManaPercent() >= 75 || GlobalExtension.Player.HealthPercent() <= 35)
            {
                SpellConfig.E.Cast();
            }
        }

        public static void OnUpdate()
        {
            if (MenuConfig.Clear["Check"].Enabled && GlobalExtension.Player.CountEnemyHeroesInRange(1500) > 0 || !MenuConfig.Clear["Q"].Enabled || !SpellConfig.Q.Ready)
            {
                return;
            }

            var mob = GameObjects.Jungle.FirstOrDefault(m => m.IsValidTarget(SpellConfig.Q.Range));

            if (mob == null)
            {
                return;
            }

            if (mob.Distance(GlobalExtension.Player) > GlobalExtension.Player.AttackRange)
            {
                SpellConfig.Q.CastOnUnit(mob);
            }
        }
    }
}
