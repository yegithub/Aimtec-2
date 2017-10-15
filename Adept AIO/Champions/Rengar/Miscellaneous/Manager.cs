namespace Adept_AIO.Champions.Rengar.Miscellaneous
{
    using Aimtec.SDK.Orbwalking;
    using OrbwalkingEvents;
    using SDK.Unit_Extensions;

    class Manager
    {
        public static void PostAttack(object sender, PostAttackEventArgs args)
        {
            switch (Global.Orbwalker.Mode)
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
            if (Global.Player.IsDead)
            {
                return;
            }

            switch (Global.Orbwalker.Mode)
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