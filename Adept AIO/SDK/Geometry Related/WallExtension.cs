using Aimtec;
using Aimtec.SDK.Extensions;

namespace Adept_AIO.SDK.Geometry_Related
{
    internal class WallExtension
    {
        private static bool IsWallAt(Vector3 pos)
        {
            return NavMesh.WorldToCell(pos).Flags.HasFlag(NavCellFlags.Wall) || NavMesh.WorldToCell(pos).Flags.HasFlag(NavCellFlags.Building);
        }

        public static Vector3 EndPoint = Vector3.Zero;

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

        public static float GetWallWidth(Vector3 start, Vector3 direction, int maxWallWidth = 275)
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
