using Adept_AIO.Champions.Gragas.Core;

namespace Adept_AIO.Champions.Gragas.Miscellaneous
{
    using OrbwalkingEvents;
    using SDK.Unit_Extensions;
    using Aimtec.SDK.Orbwalking;

    class Manager
    {
        public static void OnUpdate()
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
