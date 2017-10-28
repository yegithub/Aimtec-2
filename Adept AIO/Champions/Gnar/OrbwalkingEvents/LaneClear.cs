namespace Adept_AIO.Champions.Gnar.OrbwalkingEvents
{
    using System.Linq;
    using Aimtec.SDK.Extensions;
    using Core;
    using SDK.Unit_Extensions;

    class LaneClear
    {
        public static void OnUpdate()
        {
            var m = GameObjects.EnemyMinions.FirstOrDefault(x => x.IsValidTarget(SpellManager.Q.Range));
            if (m == null || MenuConfig.LaneClear["Check"].Enabled && Global.Player.CountEnemyHeroesInRange(2000) >= 1)
            {
                return;
            }

            if (SpellManager.Q.Ready && MenuConfig.LaneClear["Q"].Enabled)
            {
                SpellManager.CastQ(m, MenuConfig.LaneClear["Q"].Value);
            }

            if (SpellManager.W.Ready && MenuConfig.LaneClear["W"].Enabled)
            {
                SpellManager.CastW(m, MenuConfig.LaneClear["W"].Value);
            }
        }
    }
}