using System;
using Adept_AIO.Champions.Riven.Core;
using Aimtec;
using Aimtec.SDK.Orbwalking;
using Aimtec.SDK.Util;

namespace Adept_AIO.Champions.Riven.Update.Miscellaneous
{
    internal class Animation
    {
        /// <summary>
        /// Manages Rivens animation
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="args"></param>
        public static void OnPlayAnimation(Obj_AI_Base sender, Obj_AI_BasePlayAnimationEventArgs args)
        {
            if (sender == null || !sender.IsMe)
            {
                return;
            }
            const int delay = 265;
            switch (args.Animation)
            {
                case "Spell1a":
                    Extensions.LastQTime = Environment.TickCount;
                    Extensions.CurrentQCount = 2;
                    DelayAction.Queue(GetDelay(delay), Reset);
                    break;
                case "Spell1b":
                    Extensions.LastQTime = Environment.TickCount;
                    Extensions.CurrentQCount = 3;
                    DelayAction.Queue(GetDelay(delay + 15), Reset);
                    break;
                case "Spell1c":
                    Extensions.LastQTime = Environment.TickCount;
                    Extensions.CurrentQCount = 1;
                    DelayAction.Queue(GetDelay(delay + 50), Reset);
                    break;
                case "Spell2":
                    Extensions.LastWTime = Environment.TickCount;
                    break;
                case "Spell3":
                    Extensions.LastETime = Environment.TickCount;
                    break;
            }
        }

        private static void Reset()
        {
            if (Orbwalker.Implementation.Mode == OrbwalkingMode.None)
            {
                return;
            }
            Orbwalker.Implementation.ResetAutoAttackTimer();
            ObjectManager.GetLocalPlayer().IssueOrder(OrderType.MoveTo, Game.CursorPos);
        }

        private static int GetDelay(int temp)
        {
            // Just broscienced this bullshit. Still needs a lot of work.
            return (int)(temp - ObjectManager.GetLocalPlayer().AttackSpeedMod * 9 + Game.Ping);
        }
    }
}
