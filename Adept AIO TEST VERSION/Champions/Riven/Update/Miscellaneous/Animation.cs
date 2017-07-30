using Adept_AIO.SDK.Extensions;
using Aimtec;
using Aimtec.SDK.Extensions;
using Aimtec.SDK.Orbwalking;

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
            GlobalExtension.Player.IssueOrder(OrderType.MoveTo, Game.CursorPos);
        }

        public static int GetDelay(int temp)
        {
            var delay = (int)(temp - GlobalExtension.Player.AttackSpeedMod * 12 - GlobalExtension.Player.GetSpell(SpellSlot.Q).Level);
           
            return delay;
        }
    }
}
