using Adept_AIO.Champions.Rengar.Update.OrbwalkingEvents;
using Aimtec;
using Aimtec.SDK.Orbwalking;

namespace Adept_AIO.Champions.Rengar.Update.Miscellaneous
{
    class Manager
    {
        public static void PostAttack(object sender, PostAttackEventArgs args)
        {
            switch (Orbwalker.Implementation.Mode)
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
            if (ObjectManager.GetLocalPlayer().IsDead)
            {
                return;
            }

            switch (Orbwalker.Implementation.Mode)
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
