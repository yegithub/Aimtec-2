namespace Adept_AIO.Champions.Kayn.OrbwalkingEvents
{
    using System.Linq;
    using Aimtec.SDK.Extensions;
    using Core;
    using SDK.Unit_Extensions;

    class JungleClear
    {
        public static void OnPostAttack()
        {
            if (SpellConfig.W.Ready && MenuConfig.JungleClear["W"].Enabled && MenuConfig.JungleClear["W"].Value <= Global.Player.ManaPercent())
            {
                var mob = GameObjects.JungleLarge.FirstOrDefault(x => x.IsValidTarget(SpellConfig.W.Range));
                if (mob == null)
                {
                    return;
                }

                SpellConfig.W.Cast(mob);
            }
        }

        public static void OnUpdate()
        {
            if (!Global.Orbwalker.IsWindingUp && SpellConfig.Q.Ready && MenuConfig.JungleClear["Q"].Enabled && MenuConfig.JungleClear["Q"].Value <= Global.Player.ManaPercent())
            {
                var mob = GameObjects.Jungle.FirstOrDefault(x => x.IsValidTarget(SpellConfig.W.Range / 2));
                if (mob == null)
                {
                    return;
                }

                SpellConfig.Q.Cast(mob.ServerPosition);
            }
        }
    }
}