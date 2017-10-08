using System.Linq;
using Adept_AIO.Champions.Vayne.Core;
using Adept_AIO.SDK.Unit_Extensions;
using Aimtec.SDK.Damage;
using Aimtec.SDK.Extensions;
using Aimtec.SDK.Orbwalking;

namespace Adept_AIO.Champions.Vayne.OrbwalkingMode
{
    class Lasthit
    {
        public static void PostAttack(object sender, PostAttackEventArgs args)
        {
            if (!SpellManager.Q.Ready || !MenuConfig.Lasthit["Q"].Enabled)
            {
                return;
            }

            var minion = GameObjects.EnemyMinions.FirstOrDefault(x => x.Health < Global.Player.GetAutoAttackDamage(x) && x.Distance(Global.Player) <= SpellManager.Q.Range);
            if (!minion.IsValidTarget())
            {
                return;
            }

            SpellManager.CastQ(minion);
        }
    }
}
