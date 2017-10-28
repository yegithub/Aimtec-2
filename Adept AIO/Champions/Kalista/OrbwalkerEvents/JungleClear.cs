namespace Adept_AIO.Champions.Kalista.OrbwalkerEvents
{
    using System.Linq;
    using Core;
    using SDK.Unit_Extensions;

    class JungleClear
    {
        public static void OnUpdate()
        {
            var mob = GameObjects.GetMobsInRange(SpellManager.Q.Range).FirstOrDefault();

            if (mob != null && MenuConfig.JungleClear["Q"].Enabled && SpellManager.Q.Ready)
            {
                SpellManager.CastQ(mob);
            }
        }
    }
}