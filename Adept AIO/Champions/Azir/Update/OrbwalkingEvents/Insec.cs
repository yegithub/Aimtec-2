using System;
using System.Linq;
using System.Threading;
using Adept_AIO.Champions.Azir.Core;
using Adept_AIO.SDK.Junk;
using Adept_AIO.SDK.Methods;
using Aimtec;
using Aimtec.SDK.Extensions;
using Aimtec.SDK.Util;
using Geometry = Adept_AIO.SDK.Junk.Geometry;

namespace Adept_AIO.Champions.Azir.Update.OrbwalkingEvents
{
    class Insec
    {
        public static void OnKeyPressed()
        {
            var target = Global.TargetSelector.GetSelectedTarget();
            if (target == null)
            {
                return;
            }

            var dist = Global.Player.Distance(target);
            var allyT = GameObjects.AllyTurrets.OrderBy(x => x.Distance(Global.Player)).FirstOrDefault(x => x.IsValid && !x.IsDead);

            var pos = GetPos(target);

            if (pos == Vector3.Zero || pos.Distance(target) > InsecRange() || dist > InsecRange())
            {
                return;
            }

            var soldierPos = SoldierHelper.GetSoldierNearestTo(target.ServerPosition);
            if (soldierPos != Vector3.Zero)
            {
                var spos = soldierPos.Extend(soldierPos + (soldierPos - target.ServerPosition).Normalized(), 150);
                if (spos.Distance(target) <= 235)
                {
                    SpellConfig.E.Cast(spos);
                }
            }
            else
            {
                SpellConfig.W.Cast(pos);
            }

            var rect = new Geometry.Rectangle(Global.Player.ServerPosition.To2D(), Global.Player.ServerPosition.Extend(target.ServerPosition, (float)SpellConfig.RSqrt / 2f).To2D(), SpellConfig.R.Width);

            if (rect.IsInside(target.ServerPosition.To2D()) && allyT != null && !allyT.ServerPosition.IsZero)
            {
                DelayAction.Queue(250, () =>
                {
                    SpellConfig.Q.Cast(allyT.ServerPosition);
                }, new CancellationToken(false));

                DelayAction.Queue(350, () =>
                {
                    SpellConfig.E.Cast(allyT.ServerPosition);
                }, new CancellationToken(false));
                SpellConfig.R.Cast(allyT.ServerPosition);
            }
            else
            {
                DelayAction.Queue(250, () =>
                {
                    SpellConfig.Q.Cast(pos);
                }, new CancellationToken(false));
                DelayAction.Queue(350, () =>
                {
                    SpellConfig.E.Cast(pos);
                }, new CancellationToken(false));
            }
        }


        private static Vector3 GetPos(Obj_AI_Base target)
        {
          

                return Global.Player.ServerPosition.Extend(target.ServerPosition, InsecRange());
           
        }

        private static float InsecRange()
        {
            var range = 0f;
            if (SpellConfig.E.Ready)
            {
                range += SpellConfig.E.Range;
            }

            if (SpellConfig.R.Ready)
            {
                range += (float)SpellConfig.RSqrt - 65;
            }
            return range;
        }
    }
}
