using System.Linq;
using Adept_AIO.Champions.LeeSin.Core;
using Adept_AIO.SDK.Extensions;
using Adept_AIO.SDK.Usables;
using Aimtec;
using Aimtec.SDK.Damage;
using Aimtec.SDK.Damage.JSON;
using Aimtec.SDK.Extensions;

namespace Adept_AIO.Champions.LeeSin.Update.Miscellaneous
{
    internal class Killsteal
    {
        public static void OnUpdate()
        {
            var target = GameObjects.EnemyHeroes.FirstOrDefault(x => x.Distance(GlobalExtension.Player) < SpellConfig.R.Range && x.HealthPercent() <= 40);

            if (target == null || !target.IsValidTarget())
            {
                return;
            }

            if (MenuConfig.Killsteal["Smite"].Enabled && SummonerSpells.Smite != null && SummonerSpells.Smite.Ready && target.Health < SummonerSpells.SmiteChampions())
            {
                SummonerSpells.Smite.CastOnUnit(target);
            }
            if (SpellConfig.Q.Ready && (Extension.IsQ2 ? target.Health < GlobalExtension.Player.GetSpellDamage(target, SpellSlot.Q, DamageStage.SecondCast) : target.Health < GlobalExtension.Player.GetSpellDamage(target, SpellSlot.Q)) &&
                target.IsValidTarget(SpellConfig.Q.Range) &&
                MenuConfig.Killsteal["Q"].Enabled)
            {
                SpellConfig.Q.Cast(target);
            }
            else if (SpellConfig.E.Ready && target.Health < GlobalExtension.Player.GetSpellDamage(target, SpellSlot.E) &&
                     target.IsValidTarget(SpellConfig.E.Range) &&
                     MenuConfig.Killsteal["E"].Enabled)
            {
                SpellConfig.E.Cast();
            }
            else if (SpellConfig.R.Ready && target.Health < GlobalExtension.Player.GetSpellDamage(target, SpellSlot.R) &&
                     target.IsValidTarget(SpellConfig.R.Range) &&
                     MenuConfig.Killsteal["R"].Enabled)
            {
                SpellConfig.R.CastOnUnit(target);
            }
            else if (MenuConfig.Killsteal["Ignite"].Enabled && SummonerSpells.Ignite != null && SummonerSpells.Ignite.Ready && target.Health < SummonerSpells.IgniteDamage(target))
            {
                SummonerSpells.Ignite.Cast(target);
            }
        }
    }
}
