namespace Adept_AIO.Champions.Zoe.OrbwalkingEvents
{
    using System.Linq;
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

            var minion = GameObjects.EnemyMinions
                .OrderBy(x => x.Health)
                .ThenBy(x => x.Distance(Global.Player))
                .FirstOrDefault(x => x.IsValidTarget(SpellManager.Q.Range) && x.Health > Global.Player.GetAutoAttackDamage(x)); //&& x.Health < Global.Player.GetSpellDamage(x, SpellSlot.Q));
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