namespace Adept_AIO.Champions.Xerath.OrbwalkingEvents
{
    using System.Linq;
    using Aimtec;
    using Aimtec.SDK.Damage;
    using Aimtec.SDK.Extensions;
    using Core;
    using SDK.Unit_Extensions;

    class LaneClear
    {
        public static void OnUpdate()
        {
            if (MenuConfig.LaneClear["Check"].Enabled && Global.Player.CountEnemyHeroesInRange(2000) > 0)
            {
                return;
            }

            var minion = GameObjects.EnemyMinions.OrderBy(x => x.Health).ThenBy(x => x.Distance(Global.Player)).LastOrDefault(x => x.IsValidTarget(1300));

            if (minion == null)
            {
                return;
            }

            if ((SpellManager.Q.Ready || SpellManager.Q.IsCharging) &&
                MenuConfig.LaneClear["Q"].Enabled &&
                Global.Player.ManaPercent() >= MenuConfig.LaneClear["Q"].Value)
            {
                if (SpellManager.Q.IsCharging && SpellManager.Q.ChargePercent > 60)
                {
                    SpellManager.Q.Cast(minion.ServerPosition);
                }
                else 
                {
                    var qRect = SpellManager.QRealRect(minion);

                    if (qRect != null && GameObjects.EnemyMinions.Count(x => qRect.IsInside(x.ServerPosition.To2D())) >= 3)
                    {
                        SpellManager.Q.Cast(minion);
                    }
                }
            }

            if (SpellManager.W.Ready && MenuConfig.LaneClear["W"].Enabled && Global.Player.ManaPercent() >= MenuConfig.LaneClear["W"].Value)
            {
                var wCircle = SpellManager.WCircle(minion);

                if (GameObjects.EnemyMinions.Count(x => wCircle.Center.Distance(x) < wCircle.Radius) >= 3)
                {
                    SpellManager.CastW(minion);
                }
            }

            else if (SpellManager.E.Ready &&
                     MenuConfig.LaneClear["E"].Enabled &&
                     Global.Player.ManaPercent() >= MenuConfig.LaneClear["E"].Value &&
                     minion.Health < Global.Player.GetSpellDamage(minion, SpellSlot.E))
            {
                SpellManager.CastE(minion);
            }
        }
    }
}