using System.Linq;
using Adept_AIO.SDK.Unit_Extensions;
using Aimtec;
using Aimtec.SDK.Events;
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
        public static int KnockedUpTick;
        public static int BuffStart;
        public static int BuffEnd;
       
        public static bool IsItTimeToUlt(Obj_AI_Base target, int timeUntilValid = 600)
        {
            var buff = target.Buffs.FirstOrDefault(i => i.Type == BuffType.Knockback || i.Type == BuffType.Knockup);
            if (buff == null)
            {
                return false;
            }

            var time = Game.TickCount - (buff.StartTime * 1000 - buff.Full);
        
            return time >= timeUntilValid + Game.Ping / 2 && time <= 1200;
        }
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
            return GameObjects.EnemyMinions.LastOrDefault(x =>
                !x.HasBuff("YasuoDashWrapper") && x.Distance(Global.Player) <= SpellConfig.E.Range);
        }

        public static Obj_AI_Minion GetClosest(Obj_AI_Base target)
        {
            return GameObjects.EnemyMinions
                .OrderBy(x => x.Distance(Global.Player))
                .FirstOrDefault(x => !x.HasBuff("YasuoDashWrapper") &&
                                     x.IsValid &&
                                     x.MaxHealth > 10 &&
                                     x.Distance(Global.Player.ServerPosition.Extend(target.ServerPosition, 80)) <= SpellConfig.E.Range);
        }

        public static Vector3 WalkBehindMinion(Obj_AI_Base target)
        {
            var minion = GetClosest(target);

            if (target == null || minion == null || minion.IsDead)
            {
                return Vector3.Zero;
            }

            var position = minion.ServerPosition.Extend(minion.ServerPosition + (minion.ServerPosition - target.ServerPosition).Normalized(), 75 + minion.BoundingRadius);

            var isValid = position.Distance(ObjectManager.GetLocalPlayer()) > minion.BoundingRadius &&
                          position.Distance(ObjectManager.GetLocalPlayer()) < 300;

            return isValid ? position : Vector3.Zero;
        }

        public static float DashDistance(Obj_AI_Minion minion, Obj_AI_Base target, int overrideValue = 475)
        {
            if (minion == null || target == null)
            {
                return 0;
            }
            return Global.Player.GetDashInfo().StartPos.Extend(minion.ServerPosition, overrideValue).Distance(target.ServerPosition);
        }
    }
}