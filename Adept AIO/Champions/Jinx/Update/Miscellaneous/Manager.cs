using Adept_AIO.Champions.Jinx.Update.OrbwalkingEvents;
using Adept_AIO.SDK.Extensions;
using Aimtec.SDK.Orbwalking;

namespace Adept_AIO.Champions.Jinx.Update.Miscellaneous
{
    internal class Manager
    {
        private readonly Combo _combo;
        private readonly Harass _harass;
        private readonly LaneClear _laneClear;
        private readonly JungleClear _jungleClear;

        public Manager(Combo combo, Harass harass, LaneClear laneClear, JungleClear jungleClear)
        {
            _combo = combo;
            _harass = harass;
            _laneClear = laneClear;
            _jungleClear = jungleClear;
        }

        public void OnUpdate()
        {
            if (Global.Player.IsDead)
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
