namespace Adept_AIO.Champions.Twitch.OrbwalkingEvents
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

            if (SpellManager.E.Ready &&
                GameObjects.EnemyMinions.Count(x => x.IsValidTarget() && Dmg.EDmg(x) > x.Health) >= MenuConfig.LaneClear["E"].Value &&
                MenuConfig.LaneClear["E"].Enabled)
            {
                SpellManager.E.Cast();
            }

            var minion = GameObjects.EnemyMinions.FirstOrDefault(x => x.IsValidTarget(SpellManager.W.Range));
            if (minion != null && MenuConfig.LaneClear["W"].Enabled && SpellManager.W.Ready)
            {
                if (GameObjects.EnemyMinions.Count(x => x.Distance(minion) <= SpellManager.W.Range) >= MenuConfig.LaneClear["W"].Value)
                {
                    SpellManager.W.Cast(minion);
                }
            }
        }
    }
}