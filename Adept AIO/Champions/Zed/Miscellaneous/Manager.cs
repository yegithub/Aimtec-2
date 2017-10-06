using Adept_AIO.Champions.Zed.OrbwalkingEvents;
using Adept_AIO.SDK.Unit_Extensions;
using Aimtec.SDK.Orbwalking;

namespace Adept_AIO.Champions.Zed.Miscellaneous
{
    internal class Manager
    {
        public static void OnUpdate()
        {
            if (Global.Player.IsDead || Global.Orbwalker.IsWindingUp)
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
                case OrbwalkingMode.Lasthit:
                    Lasthit.OnUpdate();
                    break;
            }
        }
    }
}
