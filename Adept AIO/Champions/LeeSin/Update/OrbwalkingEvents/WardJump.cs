using Adept_AIO.Champions.LeeSin.Core;
using Aimtec;

namespace Adept_AIO.Champions.LeeSin.Update.OrbwalkingEvents
{
    internal class WardJump
    {
        public static void OnKeyPressed()
        {
            if (SpellConfig.W.Ready && Extension.IsFirst(SpellConfig.W) && WardManager.IsWardReady)
            {
                WardManager.WardJump(Game.CursorPos, true);
            }
        }
    }
}
