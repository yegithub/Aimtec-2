using System.Linq;
using Adept_AIO.Champions.Azir.Core;
using Adept_AIO.SDK.Junk;
using Aimtec.SDK.Extensions;

namespace Adept_AIO.Champions.Azir.Update.OrbwalkingEvents
{
    class LaneClear
    {
        public static void OnUpdate()
        {
            if (MenuConfig.Lane["Check"].Enabled && Global.Player.CountEnemyHeroesInRange(2000) > 0)
            {
                return;
            }

            if (SpellConfig.Q.Ready && MenuConfig.Lane["Q"].Enabled &&
                Global.Player.ManaPercent() > MenuConfig.Lane["Q"].Value)
            {
                foreach (var soldier in SoldierHelper.Soldiers)
                {
                    var rect = new Geometry.Rectangle(Global.Player.ServerPosition.To2D(), soldier.ServerPosition.To2D(), SpellConfig.Q.Width);

                    var count = GameObjects.EnemyMinions.Count(x => rect.IsInside(x.ServerPosition.To2D()));

                    if (count >= MenuConfig.Lane["QHit"].Value)
                    {
                    }
                }
            }
        }
    }
}