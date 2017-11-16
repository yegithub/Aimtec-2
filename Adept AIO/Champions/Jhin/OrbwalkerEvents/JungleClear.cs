namespace Adept_AIO.Champions.Jhin.OrbwalkerEvents
{
    using System.Linq;
    using Aimtec;
    using Aimtec.SDK.Extensions;
    using Core;
    using SDK.Unit_Extensions;

    class JungleClear
    {
        public static void OnUpdate()
        {
            var mob = GameObjects.Jungle.OrderBy(x => x.Distance(Global.Player)).
                FirstOrDefault(x => x.GetJungleType() != GameObjects.JungleType.Small && x.IsValidTarget(Global.Player.AttackRange + 200));

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

            if (SpellManager.E.Ready && MenuConfig.JungleClear["E"].Enabled && Global.Player.GetSpell(SpellSlot.E).Ammo >= 2)
            {
                SpellManager.CastE(mob);
            }
        }
    }
}
