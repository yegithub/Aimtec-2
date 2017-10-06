using Adept_AIO.Champions.Ezreal.OrbwalkingEvents;
using Adept_AIO.SDK.Unit_Extensions;
using Aimtec.SDK.Orbwalking;

namespace Adept_AIO.Champions.Ezreal.Miscellaneous
{
    internal class Manager
    {
        public static void OnUpdate()
        {
            if (Global.Player.IsDead || Global.Orbwalker.IsWindingUp)
            {
                return;
            }
       
            Misc.OnUpdate();

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
