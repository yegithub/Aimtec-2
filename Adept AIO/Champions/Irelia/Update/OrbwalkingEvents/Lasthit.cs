using System.Linq;
using Adept_AIO.Champions.Irelia.Core;
using Adept_AIO.SDK.Extensions;
using Aimtec;
using Aimtec.SDK.Damage;
using Aimtec.SDK.Extensions;

namespace Adept_AIO.Champions.Irelia.Update.OrbwalkingEvents
{
    class Lasthit
    {
        public static void OnUpdate()
        {
            if (!SpellConfig.Q.Ready || !MenuConfig.Clear["Lasthit"].Enabled || MenuConfig.Clear["Lasthit"].Value > ObjectManager.GetLocalPlayer().ManaPercent())
            {
                return;
            }

            foreach (var minion in GameObjects.EnemyMinions.Where(x => x.Health < ObjectManager.GetLocalPlayer().GetSpellDamage(x, SpellSlot.Q) && 
                                                                       x.Distance(ObjectManager.GetLocalPlayer()) < SpellConfig.Q.Range))
            {
                if (!minion.IsValid || minion.Distance(ObjectManager.GetLocalPlayer()) < ObjectManager.GetLocalPlayer().AttackRange || MenuConfig.Clear["Turret"].Enabled && minion.IsUnderEnemyTurret())
                {
                    continue;
                }

                SpellConfig.Q.CastOnUnit(minion);
            }
        }
    }
}
