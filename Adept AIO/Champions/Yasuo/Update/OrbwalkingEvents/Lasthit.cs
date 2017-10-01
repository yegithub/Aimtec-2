using System.Linq;
using Adept_AIO.Champions.Yasuo.Core;
using Adept_AIO.SDK.Unit_Extensions;
using Aimtec;
using Aimtec.SDK.Damage;
using Aimtec.SDK.Extensions;

namespace Adept_AIO.Champions.Yasuo.Update.OrbwalkingEvents
{
    class Lasthit
    {
        public static void OnUpdate()
        {
            if (!SpellConfig.E.Ready || !MenuConfig.Misc["Lasthit"].Enabled)
            {
                return;
            }

            var minion = GameObjects.EnemyMinions.FirstOrDefault(x => !x.HasBuff("YasuoDashWrapper") && x.Health < Global.Player.GetSpellDamage(x, SpellSlot.E) && x.Distance(Global.Player) <= SpellConfig.E.Range);

            if (minion == null)
            {
                return;
            }

            SpellConfig.E.CastOnUnit(minion);
        }
    }
}
