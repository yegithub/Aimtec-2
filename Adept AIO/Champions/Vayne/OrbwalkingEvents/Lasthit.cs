namespace Adept_AIO.Champions.Vayne.OrbwalkingEvents
{
    using System.Linq;
    using Aimtec;
    using Aimtec.SDK.Damage;
    using Aimtec.SDK.Extensions;
    using Aimtec.SDK.Orbwalking;
    using Core;
    using SDK.Unit_Extensions;

    class Lasthit
    {
        public static void PostAttack(object sender, PostAttackEventArgs args)
        {
            if (!SpellManager.Q.Ready || !MenuConfig.Lasthit["Q"].Enabled)
            {
                return;
            }

            var minion = GameObjects.EnemyMinions.FirstOrDefault(x => x.NetworkId != args.Target.NetworkId && x.Health < Global.Player.GetAutoAttackDamage(x) && x.Distance(Global.Player) <= SpellManager.Q.Range);
            if (minion == null)
            {
                return;
            }

            if (GameObjects.EnemyMinions.Count(x =>
                    x.Health < Global.Player.GetAutoAttackDamage(x) + Global.Player.GetSpellDamage(x, SpellSlot.Q) && x.Distance(Global.Player) <= Global.Player.AttackRange) >=
                2)
            {
                SpellManager.CastQ(minion);
            }
        }
    }
}