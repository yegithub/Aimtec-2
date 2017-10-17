namespace Adept_AIO.Champions.Twitch.OrbwalkingEvents
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

            if (SpellManager.E.Ready &&
                GameObjects.EnemyMinions.Count(x => x.IsValidTarget() && Dmg.EDmg(x) > x.Health) >= MenuConfig.LaneClear["E"].Value &&
                MenuConfig.LaneClear["E"].Enabled)
            {
                SpellManager.E.Cast();
            }

            if (MenuConfig.LaneClear["W"].Enabled && SpellManager.W.Ready)
            {
                foreach (var minion in GameObjects.EnemyMinions.Where(x => x.IsValidTarget(SpellManager.W.Range)))
                {
                    var cirlce = new Geometry.Circle(minion.ServerPosition.To2D(), SpellManager.W.Range);
                    if (GameObjects.EnemyMinions.Count(x => x.Distance(cirlce.Center) <= cirlce.Radius) >= MenuConfig.LaneClear["W"].Value)
                    {
                        SpellManager.W.Cast(minion);
                    }
                }
            }
        }
    }
}