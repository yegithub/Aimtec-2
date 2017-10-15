namespace Adept_AIO.Champions.LeeSin.Miscellaneous
{
    using System;
    using Aimtec.SDK.Orbwalking;
    using OrbwalkingEvents.Combo;
    using OrbwalkingEvents.Harass;
    using OrbwalkingEvents.JungleClear;
    using OrbwalkingEvents.LaneClear;
    using OrbwalkingEvents.LastHit;
    using SDK.Unit_Extensions;

    class Manager
    {
        private readonly ICombo _combo;
        private readonly IHarass _harass;
        private readonly IJungleClear _jungleClear;
        private readonly ILaneClear _laneClear;
        private readonly ILasthit _lasthit;

        public Manager(ICombo combo, IHarass harass, IJungleClear jungleClear, ILaneClear laneClear, ILasthit lasthit)
        {
            _combo = combo;
            _harass = harass;
            _jungleClear = jungleClear;
            _laneClear = laneClear;
            _lasthit = lasthit;
        }

        public void PostAttack(object sender, PostAttackEventArgs args)
        {
            switch (Global.Orbwalker.Mode)
            {
                case OrbwalkingMode.Combo:
                    _combo.OnPostAttack(args.Target);
                    break;
                case OrbwalkingMode.Mixed:
                    _harass.OnPostAttack(args.Target);
                    break;
                case OrbwalkingMode.Laneclear:
                    _laneClear.OnPostAttack();
                    _jungleClear.OnPostAttack(args.Target);
                    break;
            }
        }

        public void OnUpdate()
        {
            try
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
                    case OrbwalkingMode.Lasthit:
                        _lasthit.OnUpdate();
                        break;
                }

                _jungleClear.SmiteMob();
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }
    }
}