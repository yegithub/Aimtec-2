namespace Adept_AIO.Champions.Zed.OrbwalkingEvents
{
    using System.Linq;
    using Aimtec.SDK.Extensions;
    using Core;
    using SDK.Generic;
    using SDK.Unit_Extensions;

    class JungleClear
    {
        public static void OnUpdate()
        {
            var creep = GameObjects.Jungle.OrderBy(x => x.MaxHealth).
                FirstOrDefault(x => x.IsValidTarget(SpellManager.Q.Range) && x.MaxHealth > 15);
            if (creep == null || Maths.GetEnergyPercent() < MenuConfig.JungleClear["Energy"].Value)
            {
                return;
            }

            if (SpellManager.Q.Ready && MenuConfig.JungleClear["Q"].Enabled)
            {
                SpellManager.CastQ(creep);
            }

            if (SpellManager.E.Ready && MenuConfig.JungleClear["E"].Enabled)
            {
                SpellManager.CastE(creep, 1, true);
            }
        }
    }
}