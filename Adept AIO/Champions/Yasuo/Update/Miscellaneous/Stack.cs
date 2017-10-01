using System.Linq;
using Adept_AIO.Champions.Yasuo.Core;
using Adept_AIO.SDK.Unit_Extensions;
using Aimtec.SDK.Extensions;

namespace Adept_AIO.Champions.Yasuo.Update.Miscellaneous
{
    class Stack
    {
        public static void OnUpdate()
        {
            if (Global.Player.CountEnemyHeroesInRange(900) >= 1 || !MenuConfig.Misc["Stack"].Enabled || !SpellConfig.Q.Ready || Extension.CurrentMode == Mode.Tornado || Extension.CurrentMode == Mode.DashingTornado)
            {
                return;
            }

            var mob = GameObjects.EnemyMinions.FirstOrDefault(x => x.IsValidTarget() && x.Distance(Global.Player) <= SpellConfig.Q.Range - 100);
            if (mob == null)
            {
                return;
            }

            SpellConfig.Q.Cast(mob);
        }
    }
}
