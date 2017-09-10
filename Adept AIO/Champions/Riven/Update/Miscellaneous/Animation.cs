using System;
using System.Globalization;
using System.Linq;
using Adept_AIO.Champions.Riven.Core;
using Adept_AIO.SDK.Junk;
using Adept_AIO.SDK.Methods;
using Aimtec;
using Aimtec.SDK.Extensions;
using Aimtec.SDK.Orbwalking;

namespace Adept_AIO.Champions.Riven.Update.Miscellaneous
{
    internal class Animation
    {
        public static float LastReset;
        public static bool AmSoTired;
      
        public static void Reset()
        {
            if (Global.Orbwalker.Mode == OrbwalkingMode.None)
            {
                Global.Orbwalker.AttackingEnabled = true;
                return;
            }

            Global.Orbwalker.ResetAutoAttackTimer();
            Global.Orbwalker.AttackingEnabled = false;
            Global.Orbwalker.Move(Game.CursorPos);

            LastReset = Game.TickCount;
            AmSoTired = true;
        }

        public static float GetDelay()
        {
            var level = Global.Player.Level;
            var delay = Game.Ping / 2f + (Extensions.CurrentQCount == 1 ? 380 : 360);

            delay -= 3.333f * level;
            DebugConsole.Print($"Delay: {delay} | Q: {Extensions.CurrentQCount}", ConsoleColor.Red);
            return delay;
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
                    Extensions.CurrentQCount = 2;
                    break;
                case "Spell1b":
                    Extensions.CurrentQCount = 3;
                    break;
                case "Spell1c":
                    Extensions.CurrentQCount = 1;
                    break;
            }
        }
    }
}
