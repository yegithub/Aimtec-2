namespace Adept_AIO.Champions.Azir.OrbwalkingEvents
{
    using System.Linq;
    using Aimtec.SDK.Extensions;
    using Core;
    using SDK.Unit_Extensions;

    class JungleClear
    {
        public static void OnUpdate()
        {
            var mob = GameObjects.Jungle.FirstOrDefault(x =>
                x.Distance(Global.Player) <= SpellConfig.Q.Range / 2f + 100 && x.MaxHealth > 10);
            if (mob == null)
            {
                return;
            }

            if (SpellConfig.Q.Ready &&
                MenuConfig.Jungle["Q"].Enabled &&
                Global.Player.ManaPercent() > MenuConfig.Jungle["Q"].Value)
            {
                SpellConfig.CastQ(mob, false);
            }

            if (SpellConfig.W.Ready &&
                MenuConfig.Jungle["W"].Enabled &&
                Global.Player.ManaPercent() > MenuConfig.Jungle["W"].Value)
            {
                SpellConfig.W.Cast(Global.Player.ServerPosition.Extend(mob.ServerPosition, SpellConfig.W.Range));
            }
        }
    }
}