using System.Linq;
using Adept_AIO.Champions.Yasuo.Core;
using Adept_AIO.SDK.Unit_Extensions;
using Aimtec.SDK.Events;
using Aimtec.SDK.Extensions;

namespace Adept_AIO.Champions.Yasuo.Update.Miscellaneous
{
    class Stack
    {
        public static void OnUpdate()
        {
            if (Global.Player.CountEnemyHeroesInRange(600) >= 1 || Global.Player.CountEnemyHeroesInRange(2000) == 0 || Global.Orbwalker.IsWindingUp || Global.Player.IsDashing() || !MenuConfig.Misc["Stack"].Enabled || !SpellConfig.Q.Ready || Extension.CurrentMode == Mode.Tornado || Extension.CurrentMode == Mode.DashingTornado)
            {
                return;
            }

            var enemy = GameObjects.EnemyHeroes.FirstOrDefault(x => x.IsValidTarget() && x.Distance(Global.Player) <= 425);
            if (enemy != null)
            {
                SpellConfig.Q.Cast(enemy);
            }

            var mob = GameObjects.EnemyMinions.FirstOrDefault(x => x.IsValidTarget() && x.Distance(Global.Player) <= 425);
            if (mob == null)
            {
                return;
            }

            SpellConfig.Q.Cast(mob);
        }
    }
}
