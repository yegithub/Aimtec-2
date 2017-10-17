namespace Adept_AIO.Champions.Yasuo.Core
{
    using System.Linq;
    using Aimtec;
    using Aimtec.SDK.Events;
    using Aimtec.SDK.Extensions;
    using Aimtec.SDK.Orbwalking;
    using SDK.Unit_Extensions;

    public enum Mode
    {
        Normal,
        Dashing,
        DashingTornado,
        Tornado
    }

    class Extension
    {
        public static Mode CurrentMode;
        public static OrbwalkerMode FleeMode, BeybladeMode;
    }

    public class KnockUpHelper
    {
        public static Obj_AI_Base Sender;
        public static int KnockedUpTick;
        public static int BuffStart;
        public static int BuffEnd;

        public static bool KnockedUp(Obj_AI_Base target)
        {
            if (!MenuConfig.Whitelist[((Obj_AI_Hero) target).ChampionName].Enabled)
            {
                return false;
            }

            return target.HasBuffOfType(BuffType.Knockback) || target.HasBuffOfType(BuffType.Knockup);
        }

        public static bool IsItTimeToUlt(Obj_AI_Base target, int timeUntilValid = 450)
        {
            var buff = target.Buffs.FirstOrDefault(i => i.Type == BuffType.Knockback || i.Type == BuffType.Knockup);
            if (buff == null)
            {
                return false;
            }

            var time = Game.TickCount - (buff.StartTime * 1000 - buff.Full);

            return time >= timeUntilValid - Game.Ping / 2 && time <= 1200;
        }
    }

    public class MinionHelper
    {
        public static Vector3 ExtendedMinion;
        public static Vector3 ExtendedTarget;

        public static bool IsDashable(Obj_AI_Base target) =>
            !target.HasBuff("YasuoDashWrapper") && target.Distance(Global.Player) < SpellConfig.E.Range && target.IsValidTarget();

        public static float DashDistance(Obj_AI_Minion minion, Obj_AI_Base target, int overrideValue = 475)
        {
            if (minion == null || target == null)
            {
                return 0;
            }
            return Global.Player.GetDashInfo().StartPos.Extend(minion.ServerPosition, overrideValue).Distance(target.ServerPosition);
        }

        public static Obj_AI_Minion GetDashableMinion(Obj_AI_Base target)
        {
            return GameObjects.EnemyMinions.
                Where(x => !x.HasBuff("YasuoDashWrapper") &&
                           x.IsValidTarget() &&
                           x.MaxHealth > 20 &&
                           x.Distance(Global.Player) <= SpellConfig.E.Range &&
                           DashDistance(x, target) > Global.Player.Distance(target)).
                OrderByDescending(x => x.Distance(target)).
                LastOrDefault();
        }

        public static Obj_AI_Minion GetDashableMinion()
        {
            return GameObjects.EnemyMinions.LastOrDefault(x => !x.HasBuff("YasuoDashWrapper") && x.Distance(Global.Player) <= SpellConfig.E.Range);
        }

        public static Obj_AI_Minion GetClosest(Obj_AI_Base target) { return GameObjects.EnemyMinions.OrderBy(x => x.Distance(Global.Player)).FirstOrDefault(IsDashable); }

        public static Vector3 WalkBehindMinion(Obj_AI_Base target)
        {
            var minion = GetClosest(target);

            if (target == null || minion == null || minion.IsDead)
            {
                return Vector3.Zero;
            }

            var position = minion.ServerPosition.Extend(minion.ServerPosition + (minion.ServerPosition - target.ServerPosition).Normalized(), 75 + minion.BoundingRadius);

            var isValid = position.Distance(ObjectManager.GetLocalPlayer()) > minion.BoundingRadius && position.Distance(ObjectManager.GetLocalPlayer()) < 300;

            return isValid ? position : Vector3.Zero;
        }
    }
}