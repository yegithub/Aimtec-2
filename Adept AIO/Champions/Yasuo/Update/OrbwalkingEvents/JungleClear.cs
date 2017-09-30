using System.Linq;
using Adept_AIO.Champions.Yasuo.Core;
using Adept_AIO.SDK.Unit_Extensions;
using Aimtec.SDK.Extensions;
using GameObjects = Adept_AIO.SDK.Unit_Extensions.GameObjects;

namespace Adept_AIO.Champions.Yasuo.Update.OrbwalkingEvents
{
    internal class JungleClear
    {
        public static void OnPostAttack()
        {
            var minion = GameObjects.Jungle.FirstOrDefault(x => x.IsValid && x.Distance(Global.Player) <= SpellConfig.Q.Range);

            if (minion == null)
            {
                return;
            }

            if (SpellConfig.E.Ready && MenuConfig.JungleClear["E"].Enabled && !minion.HasBuff("YasuoDashWrapper"))
            {
                SpellConfig.E.CastOnUnit(minion);
            }

            if (!SpellConfig.Q.Ready ||
                Extension.CurrentMode == Mode.Tornado && !MenuConfig.JungleClear["Q3"].Enabled ||
                Extension.CurrentMode == Mode.Normal && !MenuConfig.JungleClear["Q"].Enabled)
            {
                return;
            }

            SpellConfig.Q.Cast(minion);
        }
    }
}
