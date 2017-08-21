using Adept_AIO.Champions.Jinx.Update.OrbwalkingEvents;
using Adept_AIO.SDK.Extensions;
using Aimtec.SDK.Orbwalking;

namespace Adept_AIO.Champions.Jinx.Update.Miscellaneous
{
    internal class Manager
    {
        private readonly Combo Combo;
        private readonly Harass Harass;
        private readonly LaneClear LaneClear;
        private readonly JungleClear JungleClear;

        public Manager(Combo combo, Harass harass, LaneClear laneClear, JungleClear jungleClear)
        {
            Combo = combo;
            Harass = harass;
            LaneClear = laneClear;
            JungleClear = jungleClear;
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
                    Combo.OnUpdate();
                    break;
                case OrbwalkingMode.Mixed:
                    Harass.OnUpdate();
                    break;
                    case OrbwalkingMode.Laneclear:
                    LaneClear.OnUpdate();
                    JungleClear.OnUpdate();
                    break;
            }
        }
    }
}
