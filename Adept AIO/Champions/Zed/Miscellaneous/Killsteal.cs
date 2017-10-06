using System.Linq;
using Adept_AIO.Champions.Zed.Core;
using Adept_AIO.SDK.Unit_Extensions;
using Aimtec;
using Aimtec.SDK.Damage;
using Aimtec.SDK.Extensions;

namespace Adept_AIO.Champions.Zed.Miscellaneous
{
    internal class Killsteal
    {
        public static void OnUpdate()
        {
            var target = GameObjects.EnemyHeroes.OrderBy(x => x.Health).FirstOrDefault(x => x.Distance(Global.Player) < 1300);

            if (target == null || !target.IsValidTarget())
            {
                return;
            }

            if (SpellManager.Q.Ready && target.Health < Global.Player.GetSpellDamage(target, SpellSlot.Q) && MenuConfig.Killsteal["Q"].Enabled)
            {
                SpellManager.CastQ(target);
            }
            else if (SpellManager.E.Ready && target.Health < Global.Player.GetSpellDamage(target, SpellSlot.E) && MenuConfig.Killsteal["E"].Enabled)
            {
                SpellManager.CastE(target);
            }
        }
    }
}
