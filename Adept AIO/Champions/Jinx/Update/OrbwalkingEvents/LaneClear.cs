using System.Linq;
using Adept_AIO.Champions.Jinx.Core;
using Adept_AIO.SDK.Extensions;
using Aimtec;
using Aimtec.SDK.Damage;
using Aimtec.SDK.Extensions;

namespace Adept_AIO.Champions.Jinx.Update.OrbwalkingEvents
{
    internal class LaneClear
    {
        private readonly MenuConfig MenuConfig;
        private readonly SpellConfig SpellConfig;

        public LaneClear(MenuConfig menuConfig, SpellConfig spellConfig)
        {
            MenuConfig = menuConfig;
            SpellConfig = spellConfig;
        }

        public void OnUpdate()
        {
            if (Global.Orbwalker.IsWindingUp)
            {
                return;
            }

            var minion = GameObjects.EnemyMinions.FirstOrDefault(x => x.IsValidTarget(SpellConfig.W.Range));
            if (minion == null)
            {
                return;
            }

            var dist = Global.Player.Distance(minion);

            if (SpellConfig.W.Ready && MenuConfig.LaneClear["W"].Enabled && Global.Player.CountEnemyHeroesInRange(2000) == 0)
            {
                if (dist > 800 && minion.Health < Global.Player.GetSpellDamage(minion, SpellSlot.W) && minion.UnitSkinName.ToLower().Contains("cannon"))
                {
                    SpellConfig.W.Cast(minion);
                }
            }

            if (SpellConfig.Q.Ready && MenuConfig.LaneClear["Q"].Enabled)
            {
                if (!SpellConfig.IsQ2 && dist > SpellConfig.DefaultAuotAttackRange && dist <= SpellConfig.Q2Range && GameObjects.EnemyMinions.Count(x => x.IsValidTarget(SpellConfig.Q2Range) && x.Health < Global.Player.GetAutoAttackDamage(x) * 2) >= 3 ||
                     SpellConfig.IsQ2 && dist <= SpellConfig.DefaultAuotAttackRange)
                {
                    SpellConfig.Q.Cast();
                }
            }
        }
    }
}
