using System.Linq;
using Adept_AIO.Champions.Azir.Core;
using Adept_AIO.SDK.Geometry_Related;
using Adept_AIO.SDK.Unit_Extensions;
using Aimtec.SDK.Extensions;

namespace Adept_AIO.Champions.Azir.OrbwalkingEvents
{
    internal class Harass
    {
        public static void OnUpdate()
        {
            var target = Global.TargetSelector.GetTarget(SpellConfig.Q.Range + 400);
            if (target == null)
            {
                return;
            }

            var dist = target.Distance(Global.Player) - Global.Player.BoundingRadius - target.BoundingRadius;

            if (SpellConfig.Q.Ready 
             && MenuConfig.Harass["Q"].Enabled 
             && Global.Player.ManaPercent() > MenuConfig.Harass["Q"].Value
             && dist < SpellConfig.Q.Range + 200)
            {
                SpellConfig.CastQ(target);
            }

            if (SpellConfig.W.Ready
             && MenuConfig.Harass["W"].Enabled
             && Global.Player.ManaPercent() > MenuConfig.Harass["W"].Value)
            {
                SpellConfig.W.Cast(Global.Player.ServerPosition.Extend(target.ServerPosition, SpellConfig.W.Range));
            }

            if (SpellConfig.E.Ready && MenuConfig.Harass["E"].Enabled && Global.Player.ManaPercent() > MenuConfig.Harass["E"].Value)
            {
                foreach (var soldier in SoldierManager.Soldiers)
                {
                    var rect = new Geometry.Rectangle(Vector3Extensions.To2D(Global.Player.ServerPosition), Vector3Extensions.To2D(soldier.ServerPosition), SpellConfig.E.Width);
                    var count = GameObjects.EnemyHeroes.Count(x => rect.IsInside(Vector3Extensions.To2D(x.ServerPosition)));

                    if (count >= 1)
                    {
                        SpellConfig.E.Cast(soldier.ServerPosition);
                    }
                }
            }
        }
    }
}
