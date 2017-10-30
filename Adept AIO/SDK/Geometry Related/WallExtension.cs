namespace Adept_AIO.SDK.Geometry_Related
{
    using Aimtec;
    using Aimtec.SDK.Extensions;
    using Generic;

    class WallExtension
    {
        public static Vector3 EndPoint = Vector3.Zero;

        public static bool IsWallAt(Vector3 pos)
        {
            return NavMesh.WorldToCell(pos).Flags.HasFlag(NavCellFlags.Wall) || NavMesh.WorldToCell(pos).Flags.HasFlag(NavCellFlags.Building);
        }

        public static Vector3 GeneratePoint(Vector3 start, Vector3 end)
        {
            for (var i = 0; i < start.Distance(end); i++)
            {
                var newPoint = start.Extend(end, i);

                if (IsWallAt(newPoint))
                {
                    EndPoint = end.Extend(start, i);
                    return newPoint;
                }
            }
            return Vector3.Zero;
        }

        public static bool IsWall(Vector3 start, Vector3 end)
        {
            for (var i = 0; i < start.Distance(end); i++)
            {
                var newPoint = start.Extend(end, i);

                if (IsWallAt(newPoint))
                {
                    EndPoint = end.Extend(start, i);
                    return true;
                }
            }
            return false;
        }

        public static Vector3 NearestWall(Obj_AI_Base target, float range)
        {
            for (var i = 0; i < 360; i += 40)
            {
                var dir = target.Orientation.To2D();
                var angleRad = Maths.DegreeToRadian(i);
                var rot = (target.ServerPosition.To2D() + range * dir.Rotated((float) angleRad)).To3D();

                if (!IsWallAt(rot))
                {
                    continue;
                }

                return rot;
            }

            return Vector3.Zero;
        }

        public static Vector3 FurthestWall(Obj_AI_Base target, int range) // useful for Camille
        {
            for (var i = range; i >= 360; i -= 15)
            {
                var dir = target.Orientation.To2D();
                var angleRad = Maths.DegreeToRadian(i);
                var rot = (target.ServerPosition.To2D() + range * dir.Rotated((float) angleRad)).To3D();

                if (!IsWallAt(rot))
                {
                    continue;
                }

                return rot;
            }

            return Vector3.Zero;
        }

        public static float GetWallWidth(Vector3 start, Vector3 direction, float maxWallWidth = 275)
        {
            var thickness = 0f;

            for (var i = 0; i < maxWallWidth; i++)
            {
                if (IsWallAt(start.Extend(direction, i)))
                {
                    thickness += i;
                }
                else
                {
                    return thickness;
                }
            }
            return thickness;
        }
    }
}