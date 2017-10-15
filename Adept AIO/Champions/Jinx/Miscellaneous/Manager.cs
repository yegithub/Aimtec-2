namespace Adept_AIO.Champions.Jinx.Miscellaneous
{
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