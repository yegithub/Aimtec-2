namespace Adept_AIO.Champions.Jhin.OrbwalkerEvents
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

            var minion = GameObjects.EnemyMinions.OrderBy(x => x.Distance(Global.Player)).FirstOrDefault(x => x.IsValidTarget(SpellManager.Q.Range));
            if (minion == null)
            {
                return;
            }

            if(MenuConfig.LaneClear["Q"].Enabled && SpellManager.Q.Ready)
            {
                SpellManager.Q.Cast(minion, true, MenuConfig.LaneClear["Q"].Value); // todo: Check if this works, else tryhard a bit. ZzzZz...
            }

            if (MenuConfig.LaneClear["E"].Enabled && SpellManager.E.Ready)
            {
                SpellManager.E.Cast(minion, true, MenuConfig.LaneClear["E"].Value);
            }
        }
    }
}
