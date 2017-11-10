namespace Adept_AIO.SDK.Geometry_Related
{
    using Aimtec;
    using Aimtec.SDK.Extensions;
    using Generic;
    using Unit_Extensions;

    class DashManager
    {
        public static Vector3 DashKite(Obj_AI_Base target, float range, int enemyRange = 410)
        {
            var pos = Vector3.Zero;

            for (var i = 140; i < 360; i += 20)
            {
                var dir = Global.Player.Orientation.To2D();
                var angleRad = Maths.DegreeToRadian(i);
                var rot = (Global.Player.ServerPosition.To2D() + range * dir.Rotated((float) angleRad)).To3D();
                if (rot.CountEnemyHeroesInRange(enemyRange) != 0 || rot.PointUnderEnemyTurret())
                {
                    continue;
                }
                pos = rot;
            }
            return pos;
        }
    }
}