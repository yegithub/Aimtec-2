namespace Adept_AIO.Champions.Zed.OrbwalkingEvents
{
    using System.Linq;
    using Aimtec;
    using Aimtec.SDK.Damage;
    using Aimtec.SDK.Extensions;
    using Core;
    using SDK.Generic;
    using SDK.Unit_Extensions;

    class Lasthit
    {
        public static void OnUpdate()
        {
            var creep = GameObjects.Jungle.FirstOrDefault(x => x.IsValidTarget(SpellManager.Q.Range));
            if (creep == null || Maths.GetEnergyPercent() < MenuConfig.Lasthit["Energy"].Value)
            {
                return;
            }

            if (SpellManager.Q.Ready &&
                MenuConfig.Lasthit["Q"].Enabled &&
                creep.Health < Global.Player.GetSpellDamage(creep, SpellSlot.Q))
            {
                SpellManager.CastQ(creep);
            }

            if (SpellManager.E.Ready &&
                MenuConfig.Lasthit["E"].Enabled &&
                creep.Health < Global.Player.GetSpellDamage(creep, SpellSlot.E))
            {
                SpellManager.CastE(creep);
            }
        }
    }
}