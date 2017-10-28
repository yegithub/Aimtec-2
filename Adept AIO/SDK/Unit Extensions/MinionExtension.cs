namespace Adept_AIO.SDK.Unit_Extensions
{
    using System.Linq;
    using Aimtec;
    using Aimtec.SDK.Extensions;

    public static class MinionExtension
    {
        public static int CountMinionsInRange(this Vector3 position, float range)
        {
            return GameObjects.EnemyMinions.Count(x => x.IsValidTarget(range, false, false, position));
        }
    }
}