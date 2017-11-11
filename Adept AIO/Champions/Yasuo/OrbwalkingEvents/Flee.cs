namespace Adept_AIO.Champions.Yasuo.OrbwalkingEvents
{
    using System.Linq;
    using Aimtec;
    using Aimtec.SDK.Extensions;
    using Core;
    using SDK.Geometry_Related;
    using SDK.Unit_Extensions;

    class Flee
    {
        public static void OnKeyPressed()
        {
            if (Extension.CurrentMode == Mode.Dashing && SpellConfig.Q.Ready)
            {
                SpellConfig.Q.Cast();
            }

            if (!SpellConfig.E.Ready)
            {
                return;
            }

            var mob = GameObjects.Jungle.OrderBy(x => x.Distance(Game.CursorPos)).
                FirstOrDefault(x => x.Distance(Global.Player) <= SpellConfig.E.Range + 200 && !x.HasBuff("YasuoDashWrapper"));

            if (mob != null)
            {
                var pos = mob.ServerPosition + (mob.ServerPosition - Global.Player.ServerPosition).Normalized() * mob.BoundingRadius;

                var point = WallExtension.GeneratePoint(mob.ServerPosition, Global.Player.ServerPosition.Extend(mob.ServerPosition, 475 + mob.BoundingRadius));

                if (Global.Orbwalker.CanMove())
                {
                    Global.Orbwalker.Move(pos);
                }

                if (NavMesh.WorldToCell(point).Flags == NavCellFlags.Wall)
                {
                    return;
                }

                if (pos.Distance(Global.Player) <= mob.BoundingRadius + Global.Player.BoundingRadius + 43)
                {
                    SpellConfig.E.CastOnUnit(mob);
                }
            }
            else
            {
                var minion = GameObjects.EnemyMinions.Where(MinionHelper.IsDashable).OrderBy(x => x.Distance(Game.CursorPos)).FirstOrDefault();

                if (minion != null)
                {
                    SpellConfig.E.CastOnUnit(minion);
                }
            }
        }
    }
}