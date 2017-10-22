namespace Adept_AIO.Champions.Irelia.OrbwalkingEvents
{
    using System.Linq;
    using Aimtec;
    using Aimtec.SDK.Damage;
    using Aimtec.SDK.Extensions;
    using Core;
    using SDK.Unit_Extensions;

    class Lasthit
    {
        public static void OnUpdate()
        {
            if (!SpellConfig.Q.Ready || !MenuConfig.Clear["Lasthit"].Enabled || MenuConfig.Clear["Lasthit"].Value > Global.Player.ManaPercent())
            {
                return;
            }

            foreach (var minion in GameObjects.EnemyMinions.Where(x =>
                x.Health < Global.Player.GetSpellDamage(x, SpellSlot.Q) && x.Distance(Global.Player) < SpellConfig.Q.Range))
            {
                if (!minion.IsValid || minion.Distance(Global.Player) < Global.Player.AttackRange || MenuConfig.Clear["Turret"].Enabled && minion.IsUnderEnemyTurret())
                {
                    continue;
                }

                SpellConfig.Q.CastOnUnit(minion);
            }
        }
    }
}