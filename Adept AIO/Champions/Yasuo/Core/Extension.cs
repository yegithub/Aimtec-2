using System.Linq;
using Adept_AIO.SDK.Extensions;
using Aimtec;
using Aimtec.SDK.Extensions;
using Aimtec.SDK.Orbwalking;

namespace Adept_AIO.Champions.Yasuo.Core
{
    public enum Mode
    {
        Normal,
        Dashing,
        DashingTornado,
        Tornado
    }

    internal class Extension
    {
        public static Mode CurrentMode;
        public static OrbwalkerMode FleeMode, BeybladeMode;

        public static bool KnockedUp(Obj_AI_Base target)
        {
            if (!MenuConfig.Whitelist[((Obj_AI_Hero) target).ChampionName].Enabled)
            {
                return false;
            }

            return target.HasBuffOfType(BuffType.Knockback) || target.HasBuffOfType(BuffType.Knockup);
        }
    }

    public class KnockUpHelper
    {
        public static Obj_AI_Base Sender;
        public static float TimeLeftOnKnockup;
        public static float BuffStart;
        public static float BuffEnd;
        public static bool TimeToUlt = -(Game.TickCount - (BuffStart + BuffEnd)) > Game.Ping / 2f + 25;
    }

    public class MinionHelper
    {
        public static Vector3 ExtendedMinion;
        public static Vector3 ExtendedTarget;

        public static Obj_AI_Minion GetDashableMinion(Obj_AI_Base target)
        {
            return GameObjects.EnemyMinions.Where(x => !x.HasBuff("YasuoDashWrapper") &&
                                                       x.IsValid &&
                                                       x.MaxHealth > 7 &&
                                                       x.Distance(Global.Player) <= SpellConfig.E.Range)
                .LastOrDefault(x => DashDistance(x, target) > 0 &&
                                    x.Distance(target) < Global.Player.Distance(target));
        }


        public static Obj_AI_Minion GetDashableMinion()
        {
            return GameObjects.EnemyMinions.LastOrDefault(x => !x.HasBuff("YasuoDashWrapper") && x.Distance(Global.Player) <= SpellConfig.E.Range);
        }

        public static Obj_AI_Minion GetClosest(Obj_AI_Base target)
        {
            return GameObjects.EnemyMinions.Where(x => !x.HasBuff("YasuoDashWrapper") &&
                                                       x.IsValid &&
                                                       x.MaxHealth > 7 &&
                                                       x.Distance(Global.Player) <= SpellConfig.E.Range).OrderBy(x => x.Distance(Global.Player))
                .FirstOrDefault(x => DashDistance(x, target) > 0 && x.Distance(target) < Global.Player.Distance(target));
        }

        public static Vector3 WalkBehindMinion(Obj_AI_Base target)
        {
            var minion = GetClosest(target);

            if (target == null || minion == null || minion.IsDead)
            {
                return Vector3.Zero;
            }

            var opposite = minion.ServerPosition.Extend(minion.ServerPosition + (minion.ServerPosition - target.ServerPosition).Normalized(), 75 + minion.BoundingRadius);

            return opposite.Distance(ObjectManager.GetLocalPlayer()) > minion.BoundingRadius &&
                   opposite.Distance(ObjectManager.GetLocalPlayer()) < 600 ? opposite : Vector3.Zero;
        }

        public static float DashDistance(Obj_AI_Minion minion, Obj_AI_Base target, int overrideValue = 475)
        {
            if (minion == null || target == null)
            {
                return 0;
            }
            return Global.Player.ServerPosition.Extend(minion.ServerPosition, overrideValue).Distance(target.ServerPosition);
        }
    }
}
