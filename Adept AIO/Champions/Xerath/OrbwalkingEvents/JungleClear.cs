namespace Adept_AIO.Champions.Xerath.OrbwalkingEvents
{
    using System.Linq;
    using Aimtec.SDK.Extensions;
    using Core;
    using SDK.Unit_Extensions;

    class JungleClear
    {
        public static void OnUpdate()
        {
            var creep = GameObjects.Jungle.OrderBy(x => x.MaxHealth).ThenBy(x => x.Distance(Global.Player)).FirstOrDefault(x => x.IsValidTarget(SpellManager.Q.ChargedMaxRange) && x.MaxHealth > 15);
            if (creep == null )
            {
                return;
            }

            if (SpellManager.E.Ready && MenuConfig.JungleClear["E"].Enabled)
            {
                SpellManager.CastE(creep);
            }

            if ((SpellManager.Q.Ready || SpellManager.Q.IsCharging) && MenuConfig.JungleClear["Q"].Enabled)
            {
                if (SpellManager.Q.IsCharging && SpellManager.Q.ChargePercent > 60)
                {
                    SpellManager.Q.Cast(creep);
                    return;
                }
                SpellManager.CastQ(creep);
            }

            if (SpellManager.W.Ready && MenuConfig.JungleClear["W"].Enabled)
            {
                SpellManager.CastW(creep);
            }
        }
    }
}