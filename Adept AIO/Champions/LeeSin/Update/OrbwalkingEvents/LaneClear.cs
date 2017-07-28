using System.Linq;
using Adept_AIO.Champions.LeeSin.Core;
using Adept_AIO.SDK.Extensions;
using Adept_AIO.SDK.Usables;
using Aimtec;
using Aimtec.SDK.Damage;
using Aimtec.SDK.Extensions;

namespace Adept_AIO.Champions.LeeSin.Update.OrbwalkingEvents
{
    internal class LaneClear
    {
        public static void OnPostAttack()
        {
            var minion = GameObjects.EnemyMinions.FirstOrDefault(x => x.Distance(ObjectManager.GetLocalPlayer()) < ObjectManager.GetLocalPlayer().AttackRange + x.BoundingRadius &&
                                                                      x.Health > ObjectManager.GetLocalPlayer().GetAutoAttackDamage(x));

            if (minion == null || !MenuConfig.LaneClear["Check"].Enabled && ObjectManager.GetLocalPlayer().CountEnemyHeroesInRange(2000) >= 1)
            {
                return;
            }

            if (SpellConfig.E.Ready && MenuConfig.JungleClear["E"].Enabled && minion.Health < ObjectManager.GetLocalPlayer().GetSpellDamage(minion, SpellSlot.E))
            {
                Items.CastTiamat();
                SpellConfig.E.Cast();
            }
            else if (SpellConfig.W.Ready && MenuConfig.JungleClear["W"].Enabled)
            {
                SpellConfig.W.CastOnUnit(ObjectManager.GetLocalPlayer());
            }
        }

        public static void OnUpdate()
        {
            var minion = GameObjects.EnemyMinions.FirstOrDefault(x => x.Distance(ObjectManager.GetLocalPlayer()) < SpellConfig.Q.Range + x.BoundingRadius);

            if (minion == null || !MenuConfig.LaneClear["Check"].Enabled && ObjectManager.GetLocalPlayer().CountEnemyHeroesInRange(2000) >= 1)
            {
                return;
            }

            if (SpellConfig.Q.Ready && MenuConfig.LaneClear["Q"].Enabled)
            {
                SpellConfig.Q.Cast(minion);
            }
        }
    }
}
