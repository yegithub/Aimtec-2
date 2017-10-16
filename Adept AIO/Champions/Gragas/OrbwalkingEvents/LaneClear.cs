namespace Adept_AIO.Champions.Gragas.OrbwalkingEvents
{
    using System.Linq;
    using Aimtec.SDK.Extensions;
    using Core;
    using SDK.Unit_Extensions;

    class LaneClear
    {
        public static void OnUpdate()
        {
            var minion = GameObjects.EnemyMinions.FirstOrDefault(x => x.IsValidTarget() && x.MaxHealth > 20);
            if (minion == null || MenuConfig.Lane["Check"].Enabled && Global.Player.CountEnemyHeroesInRange(2000) >= 1)
            {
                return;
            }

            if (SpellManager.Q.Ready && MenuConfig.Lane["Q"].Enabled && Global.Player.ManaPercent() > MenuConfig.Lane["QMana"].Value)
            {
                if (GameObjects.Minions.Count(x => x.Distance(minion) <= SpellManager.QRadius) >= MenuConfig.Lane["Q"].Value)
                {
                    SpellManager.CastQ(minion);
                }
            }

            if (SpellManager.W.Ready && MenuConfig.Combo["W"].Enabled)
            {
                if (GameObjects.Minions.Count(x => x.Distance(Global.Player) <= SpellManager.WHitboxRadius) >= 3)
                {
                    SpellManager.CastW(minion);
                }
            }

            if (SpellManager.E.Ready && MenuConfig.Combo["E"].Enabled)
            {
                if (GameObjects.Minions.Count(x => x.Distance(Global.Player) <= SpellManager.EHitboxRadius) >= MenuConfig.Lane["E"].Value)
                {
                    SpellManager.CastE(minion);
                }
            }
        }
    }
}