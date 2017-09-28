using System;
using Adept_AIO.Champions.Riven.Core;
using Adept_AIO.Champions.Riven.Update.OrbwalkingEvents;
using Adept_AIO.Champions.Riven.Update.OrbwalkingEvents.Combo;
using Adept_AIO.SDK.Generic;
using Adept_AIO.SDK.Unit_Extensions;
using Aimtec;
using Aimtec.SDK.Extensions;
using Aimtec.SDK.Orbwalking;

namespace Adept_AIO.Champions.Riven.Update.Miscellaneous
{
    internal class Manager
    {
        public static void OnUpdate()
        {
            try
            {
                if (Global.Player.IsDead || Global.Orbwalker.IsWindingUp)
                {
                    return;
                }

                if (Animation.DidRecentlyCancel)
                {
                    if (Game.TickCount - Extensions.LastQCastAttempt >= Animation.GetDelay())
                    {
                        Global.Orbwalker.ResetAutoAttackTimer();
                        Global.Orbwalker.AttackingEnabled = true;
                        Animation.DidRecentlyCancel = false;
                    }
                }

                switch (Global.Orbwalker.Mode)
                {
                    case OrbwalkingMode.Combo:
                        ComboManager.OnUpdate();
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
                    Extensions.CurrentQCount != 1 &&
                    MenuConfig.Miscellaneous["Active"].Enabled &&
                   !Global.Player.HasBuff("Recall") &&
                    Global.Orbwalker.Mode != OrbwalkingMode.Laneclear &&
                    Global.Orbwalker.Mode != OrbwalkingMode.Lasthit &&
                    Game.TickCount - Extensions.LastQCastAttempt >= 3580 + Game.Ping / 2 &&
                    Game.TickCount - Extensions.LastQCastAttempt <= 3700 + Game.Ping / 2)
                {
                    SpellConfig.Q.Cast();
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }

        public static void PostAttack(object sender, PostAttackEventArgs args)
        {
            if (Game.TickCount - Extensions.LastQCastAttempt < 400 + Game.Ping / 2f)
            {
                return;
            }

            var target = args.Target as Obj_AI_Base;

            if (MenuConfig.BurstMode.Active)
            {
                Burst.OnPostAttack();
            }
            else
            {
                switch (Global.Orbwalker.Mode)
                {
                    case OrbwalkingMode.Combo:
                        ComboManager.OnPostAttack();
                        break;
                    case OrbwalkingMode.Mixed:
                        Harass.OnPostAttack();
                        break;
                    case OrbwalkingMode.Laneclear:
                        if (args.Target.IsMinion)
                        {
                            Lane.OnPostAttack();
                            Jungle.OnPostAttack(args.Target as Obj_AI_Minion);
                        }
                        else if (args.Target.IsBuilding() && SpellConfig.Q.Ready)
                        {
                            SpellConfig.Q.Cast(Global.Player.ServerPosition.Extend(args.Target.ServerPosition, 100));
                        }
                        break;
                }
            }
        }
    }
}
