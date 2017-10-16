namespace Adept_AIO.Champions.Gragas.OrbwalkingEvents
{
    using System.Linq;
    using Aimtec.SDK.Extensions;
    using Core;
    using SDK.Unit_Extensions;

    class JungleClear
    {
        public static void OnUpdate()
        {
            var mob = GameObjects.Jungle.OrderByDescending(x => x.GetJungleType()).FirstOrDefault(x => x.Distance(Global.Player) <= 500);
            if (mob == null)
            {
                return;
            }

            if (SpellManager.Q.Ready && MenuConfig.Jungle["Q"].Enabled)
            {
                SpellManager.CastQ(mob);
            }

            if (SpellManager.W.Ready && MenuConfig.Jungle["W"].Enabled)
            {
                SpellManager.CastW(mob);
            }

            if (SpellManager.E.Ready && MenuConfig.Jungle["E"].Enabled)
            {
                SpellManager.CastE(mob);
            }
        }
    }
}