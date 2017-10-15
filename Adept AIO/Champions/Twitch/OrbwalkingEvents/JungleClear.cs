namespace Adept_AIO.Champions.Twitch.OrbwalkingEvents
{
    using System.Linq;
    using Aimtec.SDK.Extensions;
    using Core;
    using SDK.Unit_Extensions;

    class JungleClear
    {
        public static void OnUpdate()
        {
            if (SpellManager.E.Ready && MenuConfig.JungleClear["E"].Enabled)
            {
                var mob = GameObjects.Jungle.FirstOrDefault(x => x.HasBuff("twitchdeadlyvenom") && x.IsValidTarget(SpellManager.E.Range));
                if (mob != null)
                {
                    SpellManager.CastE(mob);
                }
            }

            var minion = GameObjects.Jungle.FirstOrDefault(x => x.IsValidTarget(SpellManager.W.Range));
            if (minion != null && MenuConfig.JungleClear["W"].Enabled && SpellManager.W.Ready)
            {
                SpellManager.W.Cast(minion);
            }
        }
    }
}