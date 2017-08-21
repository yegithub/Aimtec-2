using System.Linq;
using Adept_AIO.Champions.Jinx.Core;
using Adept_AIO.SDK.Extensions;
using Aimtec.SDK.Extensions;

namespace Adept_AIO.Champions.Jinx.Update.OrbwalkingEvents
{
    class JungleClear
    {
        private readonly MenuConfig MenuConfig;
        private readonly SpellConfig SpellConfig;

        public JungleClear(MenuConfig menuConfig, SpellConfig spellConfig)
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

            var minion = GameObjects.JungleLarge.FirstOrDefault(x => x.IsValidTarget(SpellConfig.W.Range));
            if (minion == null)
            {
                return;
            }

            var dist = Global.Player.Distance(minion);

            if (SpellConfig.W.Ready && MenuConfig.JungleClear["W"].Enabled && MenuConfig.JungleClear["W"].Value < Global.Player.ManaPercent() &&
                dist <= 650 && Global.Player.CountEnemyHeroesInRange(2000) == 0)
            {
                SpellConfig.W.Cast(minion);
            }

            if (SpellConfig.Q.Ready && MenuConfig.JungleClear["Q"].Enabled)
            {
                if (!SpellConfig.IsQ2 && dist > SpellConfig.DefaultAuotAttackRange ||
                     SpellConfig.IsQ2 && dist <= SpellConfig.DefaultAuotAttackRange)
                {
                    SpellConfig.Q.Cast();
                }
            }
        }
    }
}
