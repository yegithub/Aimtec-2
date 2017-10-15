namespace Adept_AIO.Champions.Riven.Miscellaneous
{
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

            DelayAction.Queue((int) GetDelay(),
                () =>
                {
                    Global.Orbwalker.Move(Game.CursorPos);
                    Global.Orbwalker.AttackingEnabled = true;
                    Global.Orbwalker.ResetAutoAttackTimer();
                });
        }

        private static float GetDelay() => (Extensions.CurrentQCount == 1 ? 480 : 380) -
                                           3.333f * Global.Player.Level +
                                           (Global.Player.HasBuff("RivenFengShuiEngine") ? 80 : 0);

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