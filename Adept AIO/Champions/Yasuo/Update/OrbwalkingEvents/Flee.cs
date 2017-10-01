using System;
using System.Linq;
using Adept_AIO.Champions.Yasuo.Core;
using Adept_AIO.SDK.Geometry_Related;
using Adept_AIO.SDK.Unit_Extensions;
using Aimtec;
using Aimtec.SDK.Extensions;

namespace Adept_AIO.Champions.Yasuo.Update.OrbwalkingEvents
{
    internal class Flee
    {
        public static void OnKeyPressed()
        {
            if (!SpellConfig.E.Ready)
            {
                return;
            }

            var mob = GameObjects.Jungle.OrderBy(x => x.Distance(Game.CursorPos)).FirstOrDefault(x => x.Distance(Global.Player) <= SpellConfig.E.Range + 200 && !x.HasBuff("YasuoDashWrapper"));
            if (mob != null)
            {
                var pos = mob.ServerPosition + (mob.ServerPosition -  Global.Player.ServerPosition).Normalized() * mob.BoundingRadius;

                var point = WallExtension.GeneratePoint(mob.ServerPosition, Global.Player.ServerPosition.Extend(mob.ServerPosition, 475 + mob.BoundingRadius));

                if (Global.Orbwalker.CanMove())
                {
                    Global.Orbwalker.Move(pos);
                }

                if (NavMesh.WorldToCell(point).Flags == NavCellFlags.Wall)
                {
                    return;
                }

                Console.WriteLine(pos.Distance(Global.Player));
                if (pos.Distance(Global.Player) <= mob.BoundingRadius + Global.Player.BoundingRadius + 35)
                    SpellConfig.E.CastOnUnit(mob);
            }
            else
            {

                var minion = GameObjects.EnemyMinions.Where(x => x.Distance(Global.Player) <= SpellConfig.E.Range && !x.HasBuff("YasuoDashWrapper"))
                    .OrderBy(x => x.Distance(Game.CursorPos)).FirstOrDefault();

                if (minion != null)
                {
                    SpellConfig.E.CastOnUnit(minion);
                }
            }
        }
    }
}
