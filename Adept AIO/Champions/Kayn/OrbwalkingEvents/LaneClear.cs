namespace Adept_AIO.Champions.Kayn.OrbwalkingEvents
{
    using System.Linq;
    using Aimtec.SDK.Extensions;
    using Core;
    using SDK.Unit_Extensions;

    class LaneClear
    {
        public static void OnUpdate()
        {
            if (MenuConfig.LaneClear["Check"].Enabled && Global.Player.CountEnemyHeroesInRange(2000) >= 1)
            {
                return;
            }

            if (SpellConfig.W.Ready &&
                MenuConfig.LaneClear["W"].Enabled &&
                MenuConfig.LaneClear["W"].Value <= Global.Player.ManaPercent())
            {
                var minion = GameObjects.EnemyMinions.FirstOrDefault(x => x.IsValidTarget(SpellConfig.W.Range));
                if (minion == null)
                {
                    return;
                }

                SpellConfig.W.Cast(minion);
            }

            if (SpellConfig.Q.Ready &&
                MenuConfig.LaneClear["Q"].Enabled &&
                MenuConfig.LaneClear["Q"].Value <= Global.Player.ManaPercent())
            {
                var minion = GameObjects.EnemyMinions.FirstOrDefault(x => x.IsValidTarget(SpellConfig.Q.Range));
                if (minion == null)
                {
                    return;
                }

                SpellConfig.Q.Cast(minion);
            }
        }
    }
}