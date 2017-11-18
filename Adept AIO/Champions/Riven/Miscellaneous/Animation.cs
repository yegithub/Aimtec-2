namespace Adept_AIO.Champions.Riven.Miscellaneous
{
    using System;
    using System.Threading;
    using Aimtec;
    using Aimtec.SDK.Extensions;
    using Aimtec.SDK.Util;
    using Core;
    using SDK.Unit_Extensions;

    class Animation
    {
        public static void Reset()
        {

            var qDelay = (Extensions.CurrentQCount == 1 ? 500 : 300) - 2 * Global.Player.Level;
            var moveDelay = (int)(qDelay * 0.8);
            var ping = Game.Ping   / 2;

            DelayAction.Queue(moveDelay,
                () =>
                {
                    Global.Orbwalker.Move(Game.CursorPos);
                });

            DelayAction.Queue(qDelay + ping,
                () =>
                {
                    Global.Orbwalker.ResetAutoAttackTimer();
                });
        }

        public static void OnPlayAnimation(Obj_AI_Base sender, Obj_AI_BasePlayAnimationEventArgs args)
        {
            if (sender == null || !sender.IsMe)
            {
                return;
            }
            Console.WriteLine("Animation Check");
            switch (args.Animation)
            {
                case "Spell1a":
                    Extensions.CurrentQCount = 2; // Q1
                    //  Reset();
                    break;
                case "Spell1b":
                    Extensions.CurrentQCount = 3; // Q2
                    //    Reset();
                    break;
                case "Spell1c":
                    Extensions.CurrentQCount = 1; // Q3 
                    //    Reset();
                    break;
            }
        }
    }
}