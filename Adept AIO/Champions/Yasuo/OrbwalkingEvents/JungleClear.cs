namespace Adept_AIO.Champions.Yasuo.OrbwalkingEvents
{
    using System.Linq;
    using Aimtec.SDK.Extensions;
    using Core;
    using SDK.Unit_Extensions;

    class JungleClear
    {
        public static void OnPostAttack()
        {
            if (SpellConfig.Q.Ready &&
                (Extension.CurrentMode != Mode.Tornado || MenuConfig.JungleClear["Q3"].Enabled) &&
                (Extension.CurrentMode != Mode.Normal || MenuConfig.JungleClear["Q"].Enabled))
            {
                var qminion = GameObjects.Jungle.FirstOrDefault(x => x.Distance(Global.Player) <= SpellConfig.Q.Range);

                if (qminion == null)
                {
                    return;
                }

                SpellConfig.Q.Cast(qminion.ServerPosition);
            }

            if (SpellConfig.E.Ready && MenuConfig.JungleClear["E"].Enabled)
            {
                var minion = GameObjects.Jungle.FirstOrDefault(x => x.Distance(Global.Player) <= SpellConfig.E.Range && !x.HasBuff("YasuoDashWrapper"));

                if (minion == null)
                {
                    return;
                }
                SpellConfig.E.CastOnUnit(minion);
            }
        }
    }
}