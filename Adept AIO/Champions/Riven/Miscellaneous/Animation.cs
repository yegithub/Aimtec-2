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

            var moveDelay = Global.Player.HasBuff("RivenFengShuiEngine") ? 100 : 0;
            DelayAction.Queue(moveDelay + (Extensions.CurrentQCount == 1 ? 70 : 30), () => Global.Orbwalker.Move(Game.CursorPos), new CancellationToken(false));

            DelayAction.Queue((int)GetDelay(),
                () =>
                {
                    Global.Orbwalker.AttackingEnabled = true;
                    Global.Orbwalker.ResetAutoAttackTimer();
                },
                new CancellationToken(false));
        }

        private static float GetDelay() => (Extensions.CurrentQCount == 1 ? 450 : 350) - 3.333f * Global.Player.Level;

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