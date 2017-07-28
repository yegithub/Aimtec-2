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

        public static Obj_AI_Minion GetDashableMinion(Obj_AI_Base target)
        {
            return GameObjects.EnemyMinions.Where(x => !x.HasBuff("YasuoDashWrapper") &&
                                                   x.Distance(ObjectManager.GetLocalPlayer()) <= SpellConfig.E.Range)
                                                 .FirstOrDefault(x => DashDistance(x, target) > 0 && 
                                                  x.Distance(target) < ObjectManager.GetLocalPlayer().Distance(target));
        }

        public static Obj_AI_Minion GetDashableMinion()
        {
            return GameObjects.EnemyMinions.LastOrDefault(x => !x.HasBuff("YasuoDashWrapper") && x.Distance(ObjectManager.GetLocalPlayer()) <= SpellConfig.E.Range);
        }

        public static float DashDistance(Obj_AI_Minion minion, Obj_AI_Base target, int overrideValue = 475)
        {
            if (minion == null || target == null)
            {
                return 0;
            }
            return ObjectManager.GetLocalPlayer().ServerPosition.Extend(minion.ServerPosition, overrideValue).Distance(target.ServerPosition);
        }
    }
}
