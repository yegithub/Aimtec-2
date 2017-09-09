using Aimtec.SDK.Damage.JSON;

namespace Adept_AIO.Champions.Ezreal.Update.Miscellaneous
{
    using System.Linq;
    using Core;
    using SDK.Junk;
    using Aimtec;
    using Aimtec.SDK.Damage;
    using Aimtec.SDK.Extensions;

    internal class Killsteal
    {
        public static void OnUpdate()
        {
            var target = GameObjects.EnemyHeroes.FirstOrDefault(x => x.IsValidTarget(SpellConfig.Q.Range));

            if (target == null)
            {
                return;
            }

            if (MenuConfig.Killsteal["AA"].Enabled && target.IsValidAutoRange() && target.Health < Global.Player.GetAutoAttackDamage(target))
            {
                Global.Orbwalker.ForceTarget(target);
            }

            if (Global.Orbwalker.IsWindingUp)
            {
                return;
            }
            
            if (SpellConfig.Q.Ready 
             && target.Health < Global.Player.GetSpellDamage(target, SpellSlot.Q)
             && MenuConfig.Killsteal["Q"].Enabled)
            {
                SpellConfig.Q.Cast(target);
            }
            else if(SpellConfig.W.Ready 
                && target.Health < Global.Player.GetSpellDamage(target, SpellSlot.W) 
                && target.IsValidTarget(SpellConfig.W.Range) 
                && MenuConfig.Killsteal["W"].Enabled)
            {
                SpellConfig.W.Cast(target);
            }
            else if(SpellConfig.E.Ready
                && target.Health < Global.Player.GetSpellDamage(target, SpellSlot.E)
                && target.IsValidTarget(SpellConfig.E.Range)
                && MenuConfig.Killsteal["E"].Enabled)
            {
                SpellConfig.E.Cast(target);
            }
            else if (SpellConfig.R.Ready
                     && (target.Health < Global.Player.GetSpellDamage(target, SpellSlot.R) || target.Health < Global.Player.GetSpellDamage(target, SpellSlot.R, DamageStage.AreaOfEffect))
                     && target.IsValidTarget(MenuConfig.Killsteal["Range"].Value)
                     && MenuConfig.Killsteal["Range"].Enabled
                     && target.Distance(Global.Player) > Global.Player.AttackRange + 250)
            {
                SpellConfig.R.Cast(target);
            }
        }
    }
}
