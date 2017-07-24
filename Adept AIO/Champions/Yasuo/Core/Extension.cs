using System.Linq;
using Aimtec;
using Aimtec.SDK.Extensions;
using Aimtec.SDK.Util.Cache;

namespace Adept_AIO.Champions.Yasuo.Core
{
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
            var distance = target.Distance(ObjectManager.GetLocalPlayer());
           
            return GameObjects.EnemyMinions
                .Where(x => !x.HasBuff("YasuoDashWrapper") &&
                             x.Distance(ObjectManager.GetLocalPlayer()) <= SpellConfig.E.Range &&
                            (x.Distance(target) < distance || x.Distance(target) < target.ServerPosition.Extend(x.ServerPosition, 475f).Distance(ObjectManager.GetLocalPlayer().ServerPosition)) &&
                             x.Distance(Game.CursorPos) < MenuConfig.Combo["Range"].Value)
                .OrderBy(x => x.Distance(target))
                .FirstOrDefault();
        }

        public static Obj_AI_Minion GetDashableMinion()
        {
            return GameObjects.EnemyMinions.FirstOrDefault(x => !x.HasBuff("YasuoDashWrapper") && x.Distance(ObjectManager.GetLocalPlayer()) <= SpellConfig.E.Range);
        }
    }
}
