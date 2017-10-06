using System.Linq;
using Adept_AIO.Champions.Zed.Core;
using Adept_AIO.SDK.Generic;
using Adept_AIO.SDK.Unit_Extensions;
using Aimtec;
using Aimtec.SDK.Damage;
using Aimtec.SDK.Extensions;

namespace Adept_AIO.Champions.Zed.OrbwalkingEvents
{
    internal class Lasthit
    {
        public static void OnUpdate()
        {
            var creep = GameObjects.Jungle.OrderByDescending(x => x.Health).FirstOrDefault(x => x.IsValidTarget(SpellManager.Q.Range));
            if (creep == null || Maths.GetEnergyPercent() < MenuConfig.Lasthit["Energy"].Value)
            {
                return;
            }

            if (SpellManager.Q.Ready && MenuConfig.Lasthit["Q"].Enabled && creep.Health < Global.Player.GetSpellDamage(creep, SpellSlot.Q))
            {
                SpellManager.CastQ(creep);
            }

            if (SpellManager.E.Ready && MenuConfig.Lasthit["E"].Enabled && creep.Health < Global.Player.GetSpellDamage(creep, SpellSlot.E))
            {
                SpellManager.CastE(creep);
            }
        }
    }
}
