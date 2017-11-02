namespace Adept_AIO.Champions.Yasuo.Miscellaneous
{
    using System.Linq;
    using Aimtec.SDK.Events;
    using Aimtec.SDK.Extensions;
    using Core;
    using SDK.Unit_Extensions;

    class Stack
    {
        public static void OnUpdate()
        {
            if (Global.Player.CountEnemyHeroesInRange(600) >= 1 || Global.Player.CountEnemyHeroesInRange(2000) == 0 || Global.Orbwalker.IsWindingUp || Global.Player.IsDashing() ||
                !MenuConfig.Misc["Stack"].Enabled || !SpellConfig.Q.Ready || Extension.CurrentMode != Mode.Normal)
            {
                return;
            }

            var enemy = GameObjects.EnemyHeroes.FirstOrDefault(x => x.IsValidTarget() && x.Distance(Global.Player) <= 425 && !x.Name.ToLower().Contains("ward"));
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