using System.Linq;
using Adept_AIO.Champions.Zed.Core;
using Adept_AIO.SDK.Generic;
using Adept_AIO.SDK.Unit_Extensions;
using Aimtec.SDK.Extensions;

namespace Adept_AIO.Champions.Zed.OrbwalkingEvents
{
    internal class LaneClear
    {
        public static void OnUpdate()
        {
            if (MenuConfig.LaneClear["Check"].Enabled && Global.Player.CountEnemyHeroesInRange(2000) > 0 || Maths.GetEnergyPercent() < MenuConfig.LaneClear["Energy"].Value)
            {
                return;
            }

            var minion = GameObjects.EnemyMinions.FirstOrDefault(x => x.IsValidTarget());
            if (minion == null)
            {
                return;
            }

            if (SpellManager.Q.Ready && MenuConfig.LaneClear["Q"].Enabled)
            {
                SpellManager.CastQ(minion, MenuConfig.LaneClear["Q"].Value);
            }
            
            if (SpellManager.W.Ready && MenuConfig.LaneClear["W"].Enabled)
            {
                if (GameObjects.EnemyMinions.Count(x => x.IsValidTarget(1300)) >= 4 && Global.Player.Level >= 10)
                {
                    if (ShadowManager.CanCastW1())
                    {
                        SpellManager.W.Cast(minion.ServerPosition);
                    }
                    else
                    {
                        SpellManager.W.Cast();
                    }
                }
            }

            if (SpellManager.E.Ready && MenuConfig.LaneClear["E"].Enabled)
            {
                SpellManager.CastE(minion, MenuConfig.LaneClear["E"].Value);
            }
        }
    }
}
