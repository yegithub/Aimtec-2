namespace Adept_AIO.Champions.Jax.OrbwalkingEvents
{
    using System.Linq;
    using Aimtec.SDK.Extensions;
    using Core;
    using SDK.Unit_Extensions;
    using SDK.Usables;

    class Clear
    {
        public static void OnPostAttack()
        {
            if (MenuConfig.Clear["Check"].Enabled && Global.Player.CountEnemyHeroesInRange(1500) > 0)
            {
                return;
            }

            if (SpellConfig.W.Ready && MenuConfig.Clear["W"].Enabled)
            {
                SpellConfig.W.Cast();
                Items.CastTiamat();
                Global.Orbwalker.ResetAutoAttackTimer();
            }

            if (SpellConfig.E.Ready && MenuConfig.Clear["E"].Enabled && Global.Player.ManaPercent() >= 75 ||
                Global.Player.HealthPercent() <= 35)
            {
                SpellConfig.E.Cast();
            }
        }

        public static void OnUpdate()
        {
            if (MenuConfig.Clear["Check"].Enabled && Global.Player.CountEnemyHeroesInRange(1500) > 0 ||
                !MenuConfig.Clear["Q"].Enabled ||
                !SpellConfig.Q.Ready)
            {
                return;
            }

            var mob = GameObjects.Jungle.FirstOrDefault(m => m.IsValidTarget(SpellConfig.Q.Range));

            if (mob == null)
            {
                return;
            }

            if (mob.Distance(Global.Player) > Global.Player.AttackRange)
            {
                SpellConfig.Q.CastOnUnit(mob);
            }
        }
    }
}