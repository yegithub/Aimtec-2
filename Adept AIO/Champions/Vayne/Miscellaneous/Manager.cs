using Adept_AIO.Champions.Vayne.OrbwalkingMode;
using Adept_AIO.SDK.Unit_Extensions;
using Aimtec.SDK.Orbwalking;

namespace Adept_AIO.Champions.Vayne.Miscellaneous
{
    internal class Manager
    {
        public static void PostAttack(object sender, PostAttackEventArgs args)
        {
            switch (Global.Orbwalker.Mode)
            {
                case Aimtec.SDK.Orbwalking.OrbwalkingMode.Combo:
                    Combo.PostAttack(sender, args);
                    break;
                case Aimtec.SDK.Orbwalking.OrbwalkingMode.Mixed:
                    Harass.PostAttack(sender, args);
                    break;
                case Aimtec.SDK.Orbwalking.OrbwalkingMode.Laneclear:
                    LaneClear.PostAttack(sender, args);
                    JungleClear.PostAttack(sender, args);
                    break;
                case Aimtec.SDK.Orbwalking.OrbwalkingMode.Lasthit:
                    Lasthit.PostAttack(sender, args);
                    break;

            }
        }

        public static void OnUpdate()
        {
            if (Global.Player.IsDead || Global.Orbwalker.IsWindingUp)
            {
                return;
            }

            switch (Global.Orbwalker.Mode)
            {
                case Aimtec.SDK.Orbwalking.OrbwalkingMode.Combo:
                    Combo.OnUpdate();
                    break;
                case Aimtec.SDK.Orbwalking.OrbwalkingMode.Mixed:
                    Harass.OnUpdate();
                    break;
                case Aimtec.SDK.Orbwalking.OrbwalkingMode.Laneclear:
                    LaneClear.OnUpdate();
                    JungleClear.OnUpdate();
                    break;
            }
        }
    }
}
