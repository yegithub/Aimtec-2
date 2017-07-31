using System;
using System.Threading;
using Adept_AIO.Champions.Riven.Core;
using Adept_AIO.SDK.Extensions;
using Aimtec;
using Aimtec.SDK.Extensions;
using Aimtec.SDK.Orbwalking;
using Aimtec.SDK.Util;

namespace Adept_AIO.Champions.Riven.Update.Miscellaneous
{
    internal class Animation
    {
        public static float lastReset;
        public static bool IAmSoTired;

        public static void Reset()
        {
            if (GlobalExtension.Orbwalker.Mode == OrbwalkingMode.None)
            {
                return;
            }

            GlobalExtension.Orbwalker.ResetAutoAttackTimer();
            GlobalExtension.Orbwalker.AttackingEnabled = false;
            
            GlobalExtension.Player.IssueOrder(OrderType.MoveTo, Game.CursorPos);

            lastReset = Environment.TickCount;
            IAmSoTired = true;
        }

        public static int GetDelay(int temp)
        {
            var delay = (int)(temp - GlobalExtension.Player.AttackSpeedMod * 17 - GlobalExtension.Player.GetSpell(SpellSlot.Q).Level);
            Console.WriteLine(delay + " " + Extensions.CurrentQCount);
            return delay;
        }
    }
}
