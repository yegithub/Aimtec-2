using System;
using Adept_AIO.Champions.LeeSin.Core;
using Adept_AIO.Champions.LeeSin.Update.OrbwalkingEvents;
using Adept_AIO.SDK.Extensions;
using Aimtec;
using Aimtec.SDK.Orbwalking;

namespace Adept_AIO.Champions.LeeSin.Update.Miscellaneous
{
    internal class Manager
    {
        public static void PostAttack(object sender, PostAttackEventArgs args)
        {
            switch (GlobalExtension.Orbwalker.Mode)
            {
                case OrbwalkingMode.Combo:
                    Combo.OnPostAttack(args.Target);
                    break;
                case OrbwalkingMode.Mixed:
                    Harass.OnPostAttack(args.Target);
                    break;
                case OrbwalkingMode.Laneclear:
                    LaneClear.OnPostAttack();
                    JungleClear.OnPostAttack(args.Target);
                    break;
            }
        }

        public static void OnUpdate()
        {
            if (GlobalExtension.Player.IsDead)
            {
                return;
            }

            Insec.OnKeyPressed();

            switch (GlobalExtension.Orbwalker.Mode)
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

            JungleClear.StealMobs();

        }
    }
}
