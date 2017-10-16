namespace Adept_AIO.Champions.Kalista.OrbwalkerEvents
{
    using System.Linq;
    using Aimtec.SDK.Extensions;
    using Core;
    using SDK.Unit_Extensions;

    class LaneClear
    {
        public static void OnUpdate()
        {
            if (MenuConfig.LaneClear["Check"].Enabled && Global.Player.CountEnemyHeroesInRange(1500) != 0)
            {
                return;
            }

            var minion = GameObjects.EnemyMinions.FirstOrDefault(x => x.IsValidTarget(SpellManager.Q.Range));
            if (minion != null && MenuConfig.LaneClear["Q"].Enabled && SpellManager.Q.Ready)
            {
                SpellManager.CastQ(minion, MenuConfig.LaneClear["Q"].Value);
            }
        }
    }
}