using System.Linq;
using Adept_AIO.Champions.Zed.Core;
using Adept_AIO.SDK.Generic;
using Adept_AIO.SDK.Unit_Extensions;
using Aimtec.SDK.Extensions;

namespace Adept_AIO.Champions.Zed.OrbwalkingEvents
{
    internal class JungleClear
    {
        public static void OnUpdate()
        {
            var creep = GameObjects.Jungle.OrderBy(x => x.MaxHealth).FirstOrDefault(x => x.IsValidTarget(SpellManager.Q.Range) && x.MaxHealth > 15);
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
                SpellManager.CastE(creep);
            }
        }
    }
}
