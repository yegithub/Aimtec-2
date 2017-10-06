using Adept_AIO.Champions.Riven.Core;
using Adept_AIO.SDK.Unit_Extensions;
using Aimtec;
using Aimtec.SDK.Extensions;
using Aimtec.SDK.Util;

namespace Adept_AIO.Champions.Riven.Miscellaneous
{
    internal class Animation
    {
        public static void Reset()
        {
            Global.Orbwalker.Move(Game.CursorPos);
            ResetAutoAttack();
        }

        private static void ResetAutoAttack()
        {
            Global.Orbwalker.AttackingEnabled = false;

            DelayAction.Queue((int)GetDelay(), () =>
            {
                Global.Orbwalker.AttackingEnabled = true;
                Global.Orbwalker.ResetAutoAttackTimer();
            });
        }

        private static float GetDelay()
        {
            return (Extensions.CurrentQCount == 1 ? 400 : 340) - 3.333f * Global.Player.Level +
                   (Global.Player.HasBuff("RivenFengShuiEngine") ? 120 : 0); 
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
