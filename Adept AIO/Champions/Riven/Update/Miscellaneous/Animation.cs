using System;
using Adept_AIO.Champions.Riven.Core;
using Adept_AIO.SDK.Junk;
using Adept_AIO.SDK.Methods;
using Aimtec;
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

            Global.Orbwalker.Move(Game.CursorPos);
       
            Global.Orbwalker.ResetAutoAttackTimer();
          
            Global.Orbwalker.AttackingEnabled = false;

            LastReset = Game.TickCount;
            DidRecentlyCancel = true;
        }

        public static float GetDelay()
        {
            var level  =  Global.Player.Level;
            var delay  =  Extensions.CurrentQCount == 1 ? 440f : 420f;
                delay -= (Extensions.CurrentQCount != 1 ? 3.333f : 3.1f) * level;

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
