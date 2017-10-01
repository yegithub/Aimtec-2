using System.Threading;
using Adept_AIO.Champions.Riven.Core;
using Adept_AIO.SDK.Unit_Extensions;
using Aimtec;
using Aimtec.SDK.Orbwalking;
using Aimtec.SDK.Util;

namespace Adept_AIO.Champions.Riven.Update.Miscellaneous
{
    internal class Animation
    {
        public static float LastReset;
        public static bool DidRecentlyCancel;

        public static void Reset()
        {
            if (Global.Orbwalker.Mode == OrbwalkingMode.None)
            {
                Global.Orbwalker.AttackingEnabled = true;
                return;
            }

            Global.Orbwalker.AttackingEnabled = false;
            Global.Orbwalker.Move(Game.CursorPos);

            LastReset = Game.TickCount;
            DidRecentlyCancel = true;
        }

        private static int MoveDelay()
        {
            var baseDelay = 0;
            if (Game.TickCount - SpellConfig.W.LastCastAttemptT <= 900)
            {
                baseDelay += 600;
            }

            if (Game.TickCount - SpellManager.LastR <= 1600)
            {
                baseDelay *= 2;
            }
            return baseDelay;
        }

        public static float GetDelay()
        {
            return MoveDelay() + (Extensions.CurrentQCount == 1 ? 450 : 350) - 3.333f * Global.Player.Level; 
        }

        public static void OnProcessAutoAttack(Obj_AI_Base sender, Obj_AI_BaseMissileClientDataEventArgs args)
        {
            if (!sender.IsMe)
            {
                return;
            }

            DelayAction.Queue(Game.Ping / 2 + 100, ()=> Extensions.DidJustAuto = true, new CancellationToken(false));
        }

        public static void OnPlayAnimation(Obj_AI_Base sender, Obj_AI_BasePlayAnimationEventArgs args)
        {
            if (sender == null || !sender.IsMe)
            {
                return;
            }

            switch (args.Animation)
            {
                case "Spell1a":
                    Extensions.CurrentQCount = 2; // Q1
                    break;
                case "Spell1b":
                    Extensions.CurrentQCount = 3; // Q2
                    break;
                case "Spell1c":
                    Extensions.CurrentQCount = 1; // Q3 
                    break;
            }
        }
    }
}
