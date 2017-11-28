namespace Adept_AIO.Champions.Draven.OrbwalkingEvents
{
    using System.Linq;
    using Aimtec.SDK.Extensions;
    using Core;
    using SDK.Unit_Extensions;

    class JungleClear
    {
        public static void OnUpdate()
        {
            var creep = GameObjects.Jungle.OrderByDescending(x => x.MaxHealth).ThenBy(x => x.Distance(Global.Player)).FirstOrDefault(x => x.IsValidTarget(700) && x.MaxHealth > 15);
            if (creep == null )
            {
                return;
            }

            if (SpellManager.E.Ready && MenuConfig.JungleClear["E"].Enabled)
            {
                SpellManager.CastE(creep);
            }

            if (SpellManager.Q.Ready && MenuConfig.JungleClear["Q"].Enabled)
            {
                SpellManager.CastQ();
            }
        }
    }
}