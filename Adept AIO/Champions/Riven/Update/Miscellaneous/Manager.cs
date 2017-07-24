﻿using System;
using Adept_AIO.Champions.Riven.Core;
using Adept_AIO.Champions.Riven.Update.OrbwalkingEvents;
using Aimtec;
using Aimtec.SDK.Extensions;
using Aimtec.SDK.Orbwalking;

namespace Adept_AIO.Champions.Riven.Update.Miscellaneous
{
    internal class Manager
    {
        public static void OnUpdate()
        {
            if (ObjectManager.GetLocalPlayer().IsDead)
            {
                return;
            }

            switch (Orbwalker.Implementation.Mode)
            {
                case OrbwalkingMode.Combo:
                   Combo.OnUpdate();
                    break;
                    case OrbwalkingMode.Mixed:
                    Harass.OnUpdate();
                        break;
                case OrbwalkingMode.Laneclear:
                    Lane.OnUpdate();
                    Jungle.OnUpdate();
                    break;
                case OrbwalkingMode.None:
                    Extensions.AllIn = false;
                    break;
            }

            if (SpellConfig.Q.Ready &&
                Extensions.CurrentQCount >= 2 &&
                MenuConfig.Miscellaneous["Active"].Enabled &&
               !ObjectManager.GetLocalPlayer().HasBuff("Recall") &&
                Environment.TickCount - Extensions.LastQTime >= 3580 + Game.Ping / 2 &&
                Environment.TickCount - Extensions.LastQTime <= 3700 + Game.Ping / 2) // Tries to prevents bugs.
            {
                SpellConfig.Q.Cast(Game.CursorPos);
            }
        }

        /// <summary>
        /// Handles whatever needs done when an autoattack has been processed
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="args"></param>
        public static void PostAttack(object sender, PostAttackEventArgs args)
        {
            if (Environment.TickCount - Extensions.LastQTime < 500)
            {
                return;
            }

            var target = args.Target as Obj_AI_Base;

            if (MenuConfig.BurstMode.Active)
            {
                Burst.OnPostAttack(target);
            }
            else
            {
                switch (Orbwalker.Implementation.Mode)
                {
                    case OrbwalkingMode.Combo:
                        Combo.OnPostAttack(target);
                        break;
                    case OrbwalkingMode.Mixed:
                        Harass.OnPostAttack(target);
                        break;
                    case OrbwalkingMode.Laneclear:
                        switch (args.Target.Type)
                        {
                            case GameObjectType.obj_AI_Minion:
                                Lane.OnPostAttack();
                                Jungle.OnPostAttack();
                                break;
                            case GameObjectType.obj_AI_Turret:
                            case GameObjectType.obj_HQ:
                            case GameObjectType.obj_BarracksDampener:
                            case GameObjectType.BasicLevelProp:
                            {
                                if (!SpellConfig.Q.Ready)
                                {
                                    return;
                                }
                                Extensions.AttackedStructure = true;
                                SpellConfig.Q.Cast(ObjectManager.GetLocalPlayer().ServerPosition.Extend(Game.CursorPos, 400));
                            }
                                break;
                        }
                        break;
                }
            }
        }
    }
}
