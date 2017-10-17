using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Adept_AIO.SDK.Geometry_Related
{
    using Aimtec;
    using Aimtec.SDK.Extensions;
    using Generic;
    using Unit_Extensions;

    class DashManager
    {
        public static Vector3 DashKite(Obj_AI_Base target, float range, int enemyRange = 400)
        {
            var pos = Vector3.Zero;
           
            for (var i = 140; i < 360; i += 20)
            {
                var dir = Global.Player.Orientation.To2D();
                var angleRad = Maths.DegreeToRadian(i);
                var rot = (Global.Player.ServerPosition.To2D() + range * dir.Rotated((float)angleRad)).To3D();
                if (rot.CountEnemyHeroesInRange(enemyRange) != 0 || rot.PointUnderEnemyTurret())
                {
                    continue;
                }
                DebugConsole.Write("[DASH] KITE", ConsoleColor.Yellow);
                pos = rot;
            }
            return pos;
        }
    }
}
