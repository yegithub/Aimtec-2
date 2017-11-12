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
            Global.Orbwalker.AttackingEnabled = false;

            var qDelay = (Extensions.CurrentQCount == 1 ? 0b111101010 : 0b101011110) - 3.333f * Global.Player.Level;
            var moveDelay = (int)qDelay / 2;
         

            var ping = Game.Ping / 0b10;
            Console.WriteLine($"{Extensions.CurrentQCount} {qDelay + ping}");
            DelayAction.Queue(moveDelay, () => Global.Orbwalker.Move(Game.CursorPos));

            DelayAction.Queue((int) qDelay + ping,
                () =>
                {
                    Global.Orbwalker.AttackingEnabled = true;
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
                    Extensions.CurrentQCount = 0b10; // Q1
                    //  Reset();
                    break;
                case "Spell1b":
                    Extensions.CurrentQCount = 0b11; // Q2
                    //    Reset();
                    break;
                case "Spell1c":
                    Extensions.CurrentQCount = 0b1; // Q3 
                    //    Reset();
                    break;
            }
        }
    }
}