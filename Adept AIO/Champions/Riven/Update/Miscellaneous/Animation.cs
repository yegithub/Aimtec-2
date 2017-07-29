using System;
using Adept_AIO.Champions.Riven.Core;
using Adept_AIO.SDK.Extensions;
using Aimtec;
using Aimtec.SDK.Orbwalking;
using Aimtec.SDK.Util;

namespace Adept_AIO.Champions.Riven.Update.Miscellaneous
{
    internal class Animation
    {
        public static void OnProcessSpellCast(Obj_AI_Base sender, Obj_AI_BaseMissileClientDataEventArgs args)
        {
            if (sender == null || !sender.IsMe)
            {
                return;
            }

            

            switch (args.SpellSlot)
            {
                    case SpellSlot.Q:
                    Console.WriteLine(args.SpellData.Name);
                    break;
            }
        }

        public static void OnPlayAnimation(Obj_AI_Base sender, Obj_AI_BasePlayAnimationEventArgs args)
        {
            if (sender == null || !sender.IsMe)
            {
                return;
            }

            //switch (args.Animation)
            //{
            //    case "Spell1a":
            //        Extensions.LastQTime = Environment.TickCount;
            //        Extensions.CurrentQCount = 2;
            //        DelayAction.Queue(GetDelay(MenuConfig.Animation["Q1"].Value), Reset);

            //        break;
            //    case "Spell1b":
            //        Extensions.LastQTime = Environment.TickCount;
            //        Extensions.CurrentQCount = 3;
            //        DelayAction.Queue(GetDelay(MenuConfig.Animation["Q2"].Value), Reset);

            //        break;
            //    case "Spell1c":
            //        Extensions.LastQTime = Environment.TickCount;
            //        Extensions.CurrentQCount = 1;
            //        DelayAction.Queue(GetDelay(MenuConfig.Animation["Q3"].Value), Reset);
            //        break;
            //    case "Spell2":
            //        Extensions.LastWTime = Environment.TickCount;
            //        break;
            //    case "Spell3":
            //        Extensions.LastETime = Environment.TickCount;
            //        break;
            //}
        }

        public static void Reset()
        {
            if (GlobalExtension.Orbwalker.Mode == OrbwalkingMode.None)
            {
                return;
            }

            GlobalExtension.Orbwalker.ResetAutoAttackTimer();
            ObjectManager.GetLocalPlayer().IssueOrder(OrderType.MoveTo, Game.CursorPos);
        }

        public static int GetDelay(int temp)
        {
            var delay = (int)(temp - ObjectManager.GetLocalPlayer().AttackSpeedMod * 5);
        
            return delay;
        }
    }
}
