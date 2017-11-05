namespace Adept_AIO.Champions.Tristana.Miscellaneous
{
    using System.Linq;
    using Aimtec.SDK.Damage;
    using Aimtec.SDK.Extensions;
    using Aimtec.SDK.Orbwalking;
    using OrbwalkingEvents;
    using SDK.Unit_Extensions;

    class Manager
    {
        private readonly Combo _combo;
        private readonly Harass _harass;
        private readonly JungleClear _jungleClear;
        private readonly LaneClear _laneClear;

        public Manager(Combo combo, Harass harass, LaneClear laneClear, JungleClear jungleClear)
        {
            _combo = combo;
            _harass = harass;
            _laneClear = laneClear;
            _jungleClear = jungleClear;
        }

        public void OnPostAttack(object sender, PostAttackEventArgs args)
        {
            switch (Global.Orbwalker.Mode)
            {
                case OrbwalkingMode.Laneclear:
                    _jungleClear.OnPostAttack(args.Target);
                    _laneClear.OnPostAttack();
                    break;
            }
        }

        public void OnPreAttack(object sender, PreAttackEventArgs args)
        {
            switch (Global.Orbwalker.Mode)
            {
                case OrbwalkingMode.Combo:
                case OrbwalkingMode.Mixed:
                    var enemy = GameObjects.EnemyHeroes.FirstOrDefault(x => x.IsValidTarget() && x.HasBuff("TristanaECharge"));
                    if (enemy != null && enemy.IsValidAutoRange())
                    {
                        args.Target = enemy;
                    }
                    break;
                case OrbwalkingMode.Laneclear:
                case OrbwalkingMode.Lasthit:
                    var minion = GameObjects.EnemyMinions.OrderBy(x => x.Health).FirstOrDefault(x => x.Health < Global.Player.GetAutoAttackDamage(x));
                    if (minion != null && minion.NetworkId != args.Target.NetworkId)
                    {
                        args.Target = minion;
                    }
                    break;
            }
        }

        public void OnUpdate()
        {
            if (Global.Player.IsDead || Global.Orbwalker.IsWindingUp)
            {
                return;
            }

            switch (Global.Orbwalker.Mode)
            {
                case OrbwalkingMode.Combo:
                    _combo.OnUpdate();
                    break;
                case OrbwalkingMode.Mixed:
                    _harass.OnUpdate();
                    break;
                case OrbwalkingMode.Laneclear:
                    _laneClear.OnUpdate();
                    _jungleClear.OnUpdate();
                    break;
            }
        }
    }
}