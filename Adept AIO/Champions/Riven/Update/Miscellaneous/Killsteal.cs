using System.Linq;
using Adept_AIO.Champions.Riven.Core;
using Adept_AIO.SDK.Junk;
using Adept_AIO.SDK.Usables;
using Aimtec;
using Aimtec.SDK.Damage;
using Aimtec.SDK.Extensions;

namespace Adept_AIO.Champions.Riven.Update.Miscellaneous
{
    internal class Killsteal
    {
        public static void OnUpdate()
        {
            var target = GameObjects.EnemyHeroes.FirstOrDefault(x => x.Distance(Global.Player) <= SpellConfig.R2.Range && x.IsValid && !x.IsDead);

            if (target == null)
            {
                return;
            }

            if (SpellConfig.R2.Ready
                && Enums.UltimateMode == UltimateMode.Second
                && MenuConfig.Killsteal["R2"].Enabled)
            {
                var killable = target.Health <= Global.Player.GetSpellDamage(target, SpellSlot.R) 
                            || target.Health <= Global.Player.GetSpellDamage(target, SpellSlot.R) +
                               Global.Player.GetAutoAttackDamage(target) && target.Distance(Global.Player) <= Global.Player.AttackRange;

                if (killable)
                {
                    SpellConfig.R2.Cast(target);
                }
            }
            else if (SpellConfig.W.Ready
                && MenuConfig.Killsteal["W"].Enabled
                && target.Health <= Global.Player.GetSpellDamage(target, SpellSlot.W)
                && SpellManager.InsideKiBurst(target.ServerPosition, target.BoundingRadius))
            {
                SpellManager.CastW(target);
            }
            else if (SpellConfig.Q.Ready
                && MenuConfig.Killsteal["Q"].Enabled
                && target.Health <= Global.Player.GetSpellDamage(target, SpellSlot.Q)
                && target.Distance(Global.Player) <= SpellConfig.Q.Range)
            {
                SpellManager.CastQ(target);
            }
         
            if (MenuConfig.Killsteal["Ignite"].Enabled && SummonerSpells.IsValid(SummonerSpells.Ignite) && target.Health < SummonerSpells.IgniteDamage(target))
            {
                SummonerSpells.Ignite.CastOnUnit(target);
            }
        }
    }
}
