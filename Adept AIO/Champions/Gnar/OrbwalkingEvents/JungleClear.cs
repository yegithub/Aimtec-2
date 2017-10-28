namespace Adept_AIO.Champions.Gnar.OrbwalkingEvents
{
    using System.Linq;
    using Aimtec.SDK.Extensions;
    using Core;
    using SDK.Unit_Extensions;

    class JungleClear
    {
        public static void OnUpdate()
        {
            var mob = GameObjects.Jungle.FirstOrDefault(x => x.IsValidTarget(SpellManager.Q.Range));
            if (mob == null)
            {
                return;
            }

            if (SpellManager.Q.Ready && MenuConfig.JungleClear["Q"].Enabled)
            {
                SpellManager.CastQ(mob);
            }

            if (SpellManager.W.Ready && MenuConfig.JungleClear["W"].Enabled)
            {
                SpellManager.CastW(mob);
            }
        }
    }
}