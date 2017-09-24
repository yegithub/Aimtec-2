using System;
using Adept_AIO.Champions.Riven.Core;
using Adept_AIO.SDK.Generic;
using Adept_AIO.SDK.Unit_Extensions;
using Aimtec;
using Aimtec.SDK.Extensions;
using Aimtec.SDK.Orbwalking;

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

        public static float GetDelay()
        {
            var level  =  Global.Player.Level;
            var target = Global.Orbwalker.GetOrbwalkingTarget();
            if (target == null)
            {
                return 0;
            }

            var shouldAddExtra = target.IsHero || target.IsBuilding();

            float delay  = Extensions.CurrentQCount == 1 ? 405 : 375; // Temp until API fixed (?)

            if (shouldAddExtra)
            {
                delay += 150;
            }

            delay -=  3.333f * level;
            //DebugConsole.Print($"Delay: {delay} | Q: {Extensions.CurrentQCount}", ConsoleColor.Red);
            return delay; 
        }

        public static void OnProcessAutoAttack(Obj_AI_Base sender, Obj_AI_BaseMissileClientDataEventArgs args)
        {
            if (!sender.IsMe)
            {
                return;
            }

            Extensions.DidJustAuto = true;
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
