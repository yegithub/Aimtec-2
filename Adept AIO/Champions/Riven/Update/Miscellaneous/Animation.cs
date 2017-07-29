using System;
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
        public static void Reset()
        {
            if (GlobalExtension.Orbwalker.Mode == OrbwalkingMode.None)
            {
                return;
            }

            GlobalExtension.Orbwalker.ResetAutoAttackTimer();
            GlobalExtension.Orbwalker.Move(Game.CursorPos);
            ObjectManager.GetLocalPlayer().IssueOrder(OrderType.MoveTo, Game.CursorPos);
        }

        public static int GetDelay(int temp)
        {
            var delay = (int)(temp - ObjectManager.GetLocalPlayer().AttackSpeedMod * 12 - ObjectManager.GetLocalPlayer().GetSpell(SpellSlot.Q).Level);
           
            return delay;
        }
    }
}
