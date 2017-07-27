using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Adept_AIO.Champions.LeeSin.Core;
using Aimtec;
using Aimtec.SDK.Extensions;

namespace Adept_AIO.Champions.LeeSin.Update.OrbwalkingEvents
{
    class WardJump
    {
        public static void OnKeyPressed()
        {
            if (SpellConfig.W.Ready && Extension.IsFirst(SpellConfig.W) && WardManager.IsWardReady)
            {
                WardManager.Jump(ObjectManager.GetLocalPlayer().ServerPosition.Extend(Game.CursorPos, 500));
            }
        }
    }
}
