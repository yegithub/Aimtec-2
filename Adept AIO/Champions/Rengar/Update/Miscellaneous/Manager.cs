using Adept_AIO.Champions.Rengar.Update.OrbwalkingEvents;
using Adept_AIO.SDK.Extensions;
using Aimtec;
using Aimtec.SDK.Orbwalking;

namespace Adept_AIO.Champions.Rengar.Update.Miscellaneous
{
    internal class Manager
    {
        public static void PostAttack(object sender, PostAttackEventArgs args)
        {
            switch (GlobalExtension.Orbwalker.Mode)
            {
                case OrbwalkingMode.Combo:
                    Combo.OnPostAttack();
                    break;
                    case OrbwalkingMode.Laneclear:
                    LaneClear.OnPostAttack();
                    JungleClear.OnPostAttack();
                    break;
            }
        }

        public static void OnUpdate()
        {
            if (GlobalExtension.Player.IsDead)
            {
                return;
            }

            switch (GlobalExtension.Orbwalker.Mode)
            {
                case OrbwalkingMode.Combo:
                    Combo.OnUpdate();
                    break;
                case OrbwalkingMode.Laneclear:
                    LaneClear.OnUpdate();
                    JungleClear.OnUpdate();
                    break;
            }
        }
    }
}
