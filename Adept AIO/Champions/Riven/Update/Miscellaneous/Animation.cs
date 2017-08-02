using System.Linq;
using Adept_AIO.Champions.Riven.Core;
using Adept_AIO.SDK.Extensions;
using Aimtec;
using Aimtec.SDK.Extensions;
using Aimtec.SDK.Orbwalking;

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
                GlobalExtension.Orbwalker.AttackingEnabled = true;
                return;
            }

            GlobalExtension.Orbwalker.ResetAutoAttackTimer();
            GlobalExtension.Orbwalker.AttackingEnabled = false;
            GlobalExtension.Orbwalker.Move(Game.CursorPos);

            lastReset = Game.TickCount;
            IAmSoTired = true;
        }

        public static int GetDelay()
        {
            var temp = Game.Ping / 2 + 25 + (Extensions.CurrentQCount == 1 ? 320 : 300);
            var delay = (int)(temp - GlobalExtension.Player.AttackSpeedMod * 17 - GlobalExtension.Player.GetSpell(SpellSlot.Q).Level);
            var target = GameObjects.Enemy.FirstOrDefault(x => x.Distance(GlobalExtension.Player) <= GlobalExtension.Player.AttackRange);

            if (target != null && target.IsMoving)
            {
                delay += 50; 
            }

            return delay;
        }
    }
}
