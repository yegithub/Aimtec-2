namespace Adept_AIO.Champions.Zoe.OrbwalkingEvents
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

            var minion = GameObjects.EnemyMinions.FirstOrDefault(x => x.IsValidTarget(1100) && x.Health < Global.Player.GetSpellDamage(x, SpellSlot.Q));
            if (minion == null)
            {
                return;
            }

            if (SpellManager.Q.Ready && MenuConfig.LaneClear["Q"].Enabled)
            {
                SpellManager.CastQ(minion);
            }
        }
    }
}