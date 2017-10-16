namespace Adept_AIO.SDK.Geometry_Related
{
    using Aimtec;
    using Aimtec.SDK.Extensions;
    using Unit_Extensions;

    class WallExtension
    {
        public static Vector3 EndPoint = Vector3.Zero;

        private static bool IsWallAt(Vector3 pos) => NavMesh.WorldToCell(pos).Flags.HasFlag(NavCellFlags.Wall) ||
                                                     NavMesh.WorldToCell(pos).Flags.HasFlag(NavCellFlags.Building);

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

        public static Vector3 NearestWall(Vector3 position, int range = 600)
        {
            for (var i = -range / 2; i < range / 2; i += 10)
            {
                var next = position + new Vector3(i, Global.Player.ServerPosition.Y, i);
                if (IsWallAt(next))
                {
                    return next; 
                }
            }

            for (var i = -range; i < range; i += 10)
            {
                var next = position + new Vector3(i, Global.Player.ServerPosition.Y, i);
                if (IsWallAt(next))
                {
                    return next;
                }
            }

            // -- 

            for (var i = -range /2; i < range / 2; i += 10)
            {
                var next = position + new Vector3(i, Global.Player.ServerPosition.Y, -i);
                if (IsWallAt(next))
                {
                    return next;
                }
            }

            for (var i = -range / 2; i < range / 2; i += 10)
            {
                var next = position + new Vector3(-i, Global.Player.ServerPosition.Y, i);
                if (IsWallAt(next))
                {
                    return next;
                }
            }

            for (var i = -range; i < range; i += 10)
            {
                var next = position + new Vector3(i, Global.Player.ServerPosition.Y, -i);
                if (IsWallAt(next))
                {
                    return next;
                }
            }

            for (var i = -range; i < range; i += 10)
            {
                var next = position + new Vector3(-i, Global.Player.ServerPosition.Y, i);
                if (IsWallAt(next))
                {
                    return next;
                }
            }

            return Vector3.Zero;
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