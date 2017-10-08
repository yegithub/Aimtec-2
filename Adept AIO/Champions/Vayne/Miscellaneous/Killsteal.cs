using System.Linq;
using Adept_AIO.Champions.Vayne.Core;
using Adept_AIO.SDK.Unit_Extensions;
using Aimtec;
using Aimtec.SDK.Damage;
using Aimtec.SDK.Extensions;

namespace Adept_AIO.Champions.Vayne.Miscellaneous
{
    internal class Killsteal
    {
        public static void OnUpdate()
        {
            var target = GameObjects.EnemyHeroes.FirstOrDefault(x => x.Distance(Global.Player) <= Global.Player.AttackRange + SpellManager.Q.Range && x.IsValid && !x.IsDead);

            if (target == null)
            {
                return;
            }

            if (SpellManager.Q.Ready && MenuConfig.Killsteal["Q"].Enabled)
            {
                if (target.Health <= Global.Player.GetSpellDamage(target, SpellSlot.Q) || target.Health <= Global.Player.GetSpellDamage(target, SpellSlot.Q) + Global.Player.GetAutoAttackDamage(target))
                {
                    SpellManager.CastQ(target);
                }
            }
            else if (SpellManager.E.Ready && MenuConfig.Killsteal["E"].Enabled)
            {
                if (target.Health <= Global.Player.GetSpellDamage(target, SpellSlot.E))
                {
                    SpellManager.CastE(target);
                }
            }
        }
    }
}
