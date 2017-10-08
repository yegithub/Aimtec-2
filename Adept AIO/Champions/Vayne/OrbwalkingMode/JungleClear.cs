using System.Linq;
using Adept_AIO.Champions.Vayne.Core;
using Adept_AIO.SDK.Unit_Extensions;
using Aimtec.SDK.Extensions;
using Aimtec.SDK.Orbwalking;

namespace Adept_AIO.Champions.Vayne.OrbwalkingMode
{
    class JungleClear
    {
        public static void PostAttack(object sender, PostAttackEventArgs args)
        {
            if (!SpellManager.Q.Ready || MenuConfig.JungleClear["Q"].Value == 1)
            {
                return;
            }

            var mob = GameObjects.Jungle.FirstOrDefault(x => x.Distance(Global.Player) <= SpellManager.Q.Range);
            if (!mob.IsValidTarget())
            {
                return;
            }

            SpellManager.CastQ(mob, MenuConfig.JungleClear["Mode"].Value);
        }

        public static void OnUpdate()
        {
            var mob = GameObjects.Jungle.FirstOrDefault(x => x.Distance(Global.Player) <= SpellManager.Q.Range + Global.Player.AttackRange);
            if (!mob.IsValidTarget() || MenuConfig.JungleClear["Check"].Enabled && Global.Player.CountEnemyHeroesInRange(2000) >= 1)
            {
                return;
            }

            if (SpellManager.Q.Ready && MenuConfig.JungleClear["Q"].Value == 1)
            {
                SpellManager.CastQ(mob, MenuConfig.JungleClear["Mode"].Value);
            }

            if (SpellManager.E.Ready && MenuConfig.JungleClear["E"].Enabled)
            {
                SpellManager.CastE(mob);
            }
        }
    }
}
