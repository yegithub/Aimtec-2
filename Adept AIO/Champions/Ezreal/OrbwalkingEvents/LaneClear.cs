namespace Adept_AIO.Champions.Ezreal.OrbwalkingEvents
{
    using System.Linq;
    using Aimtec;
    using Aimtec.SDK.Damage;
    using Aimtec.SDK.Extensions;
    using Core;
    using SDK.Unit_Extensions;

    class LaneClear
    {
        public static void OnUpdate()
        {
            if (SpellConfig.Q.Ready && MenuConfig.Lane["Q"].Enabled && Global.Player.ManaPercent() >= MenuConfig.Lane["Q"].Value)
            {
                var minion = GameObjects.EnemyMinions.FirstOrDefault(x =>
                    x.Health < Global.Player.GetSpellDamage(x, SpellSlot.Q) &&
                    x.IsValidTarget(SpellConfig.Q.Range) &&
                    x.Distance(Global.Player) > Global.Player.AttackRange);
                if (minion == null)
                {
                    return;
                }

                SpellConfig.Q.Cast(minion);
            }

            if (MenuConfig.Lane["Check"].Enabled && Global.Player.CountEnemyHeroesInRange(2000) >= 1 ||
                !SpellConfig.W.Ready ||
                !MenuConfig.Lane["W"].Enabled ||
                Global.Player.ManaPercent() < MenuConfig.Lane["W"].Value)
            {
                return;
            }

            var ally = GameObjects.AllyHeroes.FirstOrDefault(x => x.IsValidTarget(SpellConfig.W.Range - 100));
            if (ally != null)
            {
                SpellConfig.W.Cast(ally);
            }
        }
    }
}