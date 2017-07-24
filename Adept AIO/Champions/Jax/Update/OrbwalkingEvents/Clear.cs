using System.Linq;
using Adept_AIO.Champions.Jax.Core;
using Aimtec;
using Aimtec.SDK.Extensions;
using Aimtec.SDK.Orbwalking;
using GameObjects = Adept_AIO.SDK.Extensions.GameObjects;

namespace Adept_AIO.Champions.Jax.Update.OrbwalkingEvents
{
    internal class Clear
    {
        public static void OnPostAttack()
        {
            if (MenuConfig.Clear["Check"].Enabled && ObjectManager.GetLocalPlayer().CountEnemyHeroesInRange(1500) > 0)
            {
                return;
            }

            if (SpellConfig.W.Ready)
            {
                SpellConfig.W.Cast();
                Orbwalker.Implementation.ResetAutoAttackTimer();
            }

            if (SpellConfig.E.Ready && MenuConfig.Clear["E"].Enabled && ObjectManager.GetLocalPlayer().ManaPercent() >= 75 || ObjectManager.GetLocalPlayer().HealthPercent() <= 35)
            {
                SpellConfig.E.Cast();
            }
        }

        public static void OnUpdate()
        {
            if (MenuConfig.Clear["Check"].Enabled && ObjectManager.GetLocalPlayer().CountEnemyHeroesInRange(1500) > 0 || !MenuConfig.Clear["Q"].Enabled || !SpellConfig.Q.Ready)
            {
                return;
            }

            var mob = GameObjects.Jungle.FirstOrDefault(m => m.IsValidTarget(SpellConfig.Q.Range));

            if (mob == null)
            {
                return;
            }

            if (mob.Distance(ObjectManager.GetLocalPlayer()) > ObjectManager.GetLocalPlayer().AttackRange)
            {
                SpellConfig.Q.CastOnUnit(mob);
            }
        }
    }
}
