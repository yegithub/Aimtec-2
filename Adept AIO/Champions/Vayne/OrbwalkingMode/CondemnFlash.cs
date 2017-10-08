using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Adept_AIO.Champions.Vayne.Core;
using Adept_AIO.SDK.Geometry_Related;
using Aimtec;
using Aimtec.SDK.Extensions;

namespace Adept_AIO.Champions.Vayne.OrbwalkingMode
{
    class CondemnFlash
    {
        public static void OnKeyPressed()
        {
            var target = MenuConfig.CondemnFlashOrbwalkerMode.GetTarget() as Obj_AI_Base;
            if (target == null || !target.IsValidTarget())
            {
                return;
            }

            var point = WallExtension.GeneratePoint(target.ServerPosition, Game.CursorPos);
            if (point.IsZero)
            {
                return;
            }


        }
    }
}
