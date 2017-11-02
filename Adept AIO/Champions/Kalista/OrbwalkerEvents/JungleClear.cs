namespace Adept_AIO.Champions.Kalista.OrbwalkerEvents
{
    using System.Linq;
    using Aimtec.SDK.Extensions;
    using Core;
    using SDK.Unit_Extensions;

    class JungleClear
    {
        public static void OnUpdate()
        {
            var mob = GameObjects.Jungle.OrderBy(x => x.Distance(Global.Player)).FirstOrDefault(x => x.GetJungleType() != GameObjects.JungleType.Small && x.IsValidTarget(SpellManager.Q.Range));

            if (mob != null && MenuConfig.JungleClear["Q"].Enabled && SpellManager.Q.Ready)
            {
                SpellManager.CastQ(mob);
            }
        }
    }
}