namespace Adept_AIO.Champions.Kalista.OrbwalkerEvents
{
    using System;
    using System.Linq;
    using Aimtec.SDK.Extensions;
    using Core;
    using SDK.Unit_Extensions;

    class JungleClear
    {
        public static void OnUpdate()
        {
            if (SpellManager.E.Ready && GameObjects.Jungle.Count(x => Dmg.EDmg(x) > x.Health) >= 1 && MenuConfig.JungleClear["E"].Enabled)
            {
                SpellManager.E.Cast();
            }
          
            var mob = GameObjects.Jungle.FirstOrDefault(x => x.IsValidTarget(SpellManager.Q.Range));

            if (mob != null && MenuConfig.JungleClear["Q"].Enabled && SpellManager.Q.Ready)
            {
                SpellManager.CastQ(mob);
            }
        }
    }
}
