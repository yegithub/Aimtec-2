namespace Adept_AIO.Champions.Kayn.Miscellaneous
{
    using System.Linq;
    using Aimtec;
    using Aimtec.SDK.Damage;
    using Aimtec.SDK.Extensions;
    using Core;
    using SDK.Unit_Extensions;
    using SDK.Usables;

    class Killsteal
    {
        public static void OnUpdate()
        {
            var target = GameObjects.EnemyHeroes.FirstOrDefault(x => x.Distance(Global.Player) < SpellConfig.R.Range && x.HealthPercent() <= 40);

            if (target == null || !target.IsValidTarget())
            {
                return;
            }

            if (SpellConfig.Q.Ready && target.Health < Global.Player.GetSpellDamage(target, SpellSlot.Q) && target.IsValidTarget(SpellConfig.Q.Range) && MenuConfig.Killsteal["Q"].Enabled)
            {
                SpellConfig.Q.Cast(target);
            }
            else if (SpellConfig.W.Ready && target.Health < Global.Player.GetSpellDamage(target, SpellSlot.W) && target.IsValidTarget(SpellConfig.W.Range) && MenuConfig.Killsteal["W"].Enabled)
            {
                SpellConfig.W.Cast(target);
            }
            else if (SpellConfig.R.Ready && target.Health < Global.Player.GetSpellDamage(target, SpellSlot.R) + Global.Player.GetAutoAttackDamage(target) &&
                     target.IsValidTarget(SpellConfig.R.Range) && MenuConfig.Killsteal["R"].Enabled)
            {
                SpellConfig.R.CastOnUnit(target);
            }
            else if (MenuConfig.Killsteal["Ignite"].Enabled && SummonerSpells.IsValid(SummonerSpells.Ignite) && target.Health < SummonerSpells.IgniteDamage(target))
            {
                SummonerSpells.Ignite.Cast(target);
            }
        }
    }
}