using System.Threading;
using Adept_AIO.Champions.Azir.Core;
using Adept_AIO.SDK.Unit_Extensions;
using Aimtec;
using Aimtec.SDK.Extensions;
using Aimtec.SDK.Util;

namespace Adept_AIO.Champions.Azir.Update.OrbwalkingEvents
{
    internal class Flee
    {
        public static void OnKeyPressed()
        {
            Jump(Game.CursorPos);
        }

        public static void Jump(Vector3 pos)
        {
            if (SpellConfig.W.Ready || Game.TickCount - AzirHelper.LastW <= 1000)
            {
                var extend = Global.Player.ServerPosition.Extend(pos, SpellConfig.W.Range);
                if (SoldierHelper.Soldiers.Count == 0)
                {
                    SpellConfig.W.Cast(extend);
                }

                DelayAction.Queue(350, () => SpellConfig.Q.Cast(pos), new CancellationToken(false));
                DelayAction.Queue(150, () => SpellConfig.E.Cast(extend), new CancellationToken(false));
            }
            else if (SoldierHelper.GetSoldierNearestTo(pos).Distance(pos) <= 300)
            {
                var extend = SoldierHelper.GetSoldierNearestTo(pos).Extend(pos, SpellConfig.Q.Range);
                if (SoldierHelper.Soldiers.Count == 0)
                {
                    SpellConfig.W.Cast(extend);
                }
                DelayAction.Queue(350, () => SpellConfig.Q.Cast(pos), new CancellationToken(false));
                DelayAction.Queue(150, () => SpellConfig.E.Cast(extend), new CancellationToken(false));
            }
        }
    }
}
