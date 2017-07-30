using System;
using Adept_AIO.Champions.Jax.Core;
using Adept_AIO.Champions.Jax.Update.OrbwalkingEvents;
using Adept_AIO.SDK.Extensions;
using Aimtec.SDK.Orbwalking;

namespace Adept_AIO.Champions.Jax.Update.Miscellaneous
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
                    case OrbwalkingMode.Mixed:
                    Harass.OnPostAttack();
                    break;
                    case OrbwalkingMode.Laneclear:
                    Clear.OnPostAttack();
                    break;
            }
        }

        public static void OnUpdate()
        {
            if (GlobalExtension.Player.IsDead)
            {
                return;
            }

            if (Environment.TickCount - SpellConfig.CounterStrikeTime > 2100)
            {
                SpellConfig.CounterStrikeTime = 0;
            }
          
            switch (GlobalExtension.Orbwalker.Mode)
            {
                    case OrbwalkingMode.Combo:
                    Combo.OnUpdate();
                    break;
                case OrbwalkingMode.Mixed:
                    Harass.OnUpdate();
                    break;
                    case OrbwalkingMode.Laneclear:
                    Clear.OnUpdate();
                    break;
            }
        }
    }
}
