using System.Linq;
using Adept_AIO.Champions.Ezreal.Core;
using Adept_AIO.SDK.Geometry_Related;
using Adept_AIO.SDK.Unit_Extensions;
using Aimtec;
using Aimtec.SDK.Damage;
using Aimtec.SDK.Damage.JSON;
using Aimtec.SDK.Extensions;

namespace Adept_AIO.Champions.Ezreal.Miscellaneous
{
    internal class Killsteal
    {
        public static void OnUpdate()
        {
            var target = GameObjects.EnemyHeroes.FirstOrDefault(x => x.IsValidTarget(5000));

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
             && target.IsValidTarget(SpellConfig.Q.Range)
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
                     && target.Health < Global.Player.GetSpellDamage(target, SpellSlot.R)
                     && target.IsValidTarget(MenuConfig.Killsteal["Range"].Value)
                     && MenuConfig.Killsteal["Range"].Enabled
                     && target.Distance(Global.Player) > Global.Player.AttackRange + 275)
            {
                var rectangle = new Geometry.Rectangle(Global.Player.ServerPosition.To2D(), target.ServerPosition.To2D(), SpellConfig.R.Width);
                if (GameObjects.EnemyHeroes.Count(x => rectangle.IsInside(x.ServerPosition.To2D())) >= 2 &&
                    target.Health > Global.Player.GetSpellDamage(target, SpellSlot.R, DamageStage.AreaOfEffect))
                {
                    return;
                }

                SpellConfig.R.Cast(target);
            }
        }
    }
}
