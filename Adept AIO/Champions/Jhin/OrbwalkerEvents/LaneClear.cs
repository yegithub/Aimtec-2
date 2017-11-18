namespace Adept_AIO.Champions.Jhin.OrbwalkerEvents
{
    using System.Linq;
    using Aimtec.SDK.Extensions;
    using Core;
    using SDK.Geometry_Related;
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
                var range = SpellManager.Q.Range * 2;
                var circle = new Geometry.Circle(minion.ServerPosition.To2D(), range);
                var possible = GameObjects.EnemyMinions.Where(x => x.Distance(circle.Center) < range).OrderBy(x => x.Distance(minion));

                if (possible.Count() >= MenuConfig.LaneClear["Q"].Value)
                {
                    SpellManager.Q.Cast(minion); 
                }
            }

            if (MenuConfig.LaneClear["E"].Enabled && SpellManager.E.Ready)
            {
                SpellManager.E.Cast(minion, true, MenuConfig.LaneClear["E"].Value);
            }
        }
    }
}
