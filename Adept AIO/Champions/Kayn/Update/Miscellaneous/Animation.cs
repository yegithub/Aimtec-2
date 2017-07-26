using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Adept_AIO.Champions.Kayn.Core;
using Aimtec;

namespace Adept_AIO.Champions.Kayn.Update.Miscellaneous
{
    class Animation
    {
        public static void OnPlayAnimation(Obj_AI_Base sender, Obj_AI_BasePlayAnimationEventArgs args)
        {
            if (sender == null || !sender.IsMe)
            {
                return;
            }

            if (args.Animation == "Spell1")
            {
                SpellConfig.LastQCast = Environment.TickCount;
            }
        }
    }
}
