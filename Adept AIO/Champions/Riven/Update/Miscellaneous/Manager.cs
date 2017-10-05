using System;
using System.Threading;
using Adept_AIO.Champions.Riven.Core;
using Adept_AIO.Champions.Riven.Update.OrbwalkingEvents;
using Adept_AIO.Champions.Riven.Update.OrbwalkingEvents.Combo;
using Adept_AIO.SDK.Unit_Extensions;
using Aimtec;
using Aimtec.SDK.Extensions;
using Aimtec.SDK.Orbwalking;
using Aimtec.SDK.Util;

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

                if (Animation.DidRecentlyCancel && Game.TickCount - Animation.LastReset >= Animation.GetDelay())
                {
                    Global.Orbwalker.Move(Game.CursorPos);

                    DelayAction.Queue(Global.Player.HasBuff("RivenFengShuiEngine") ? 80 : 0, () =>
                    {
                        Animation.DidRecentlyCancel = false;
                        Global.Orbwalker.AttackingEnabled = true;
                        Global.Orbwalker.ResetAutoAttackTimer();
                    }, new CancellationToken(false));
                }
                else
                {
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

                    if (!SpellConfig.Q.Ready || Extensions.CurrentQCount == 1 || !MenuConfig.Miscellaneous["Active"].Enabled 
                      || Global.Player.HasBuff("Recall")
                      || Global.Orbwalker.Mode == OrbwalkingMode.Laneclear || Global.Orbwalker.Mode == OrbwalkingMode.Lasthit
                      || Game.TickCount - Extensions.LastQCastAttempt < 3580 + Game.Ping / 2
                      || Game.TickCount - Extensions.LastQCastAttempt > 3700 + Game.Ping / 2)
                    {
                        return;
                    }

                    SpellConfig.Q.Cast();
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }

        public static void OnPostAttack(object sender, PostAttackEventArgs args)
        {
            if (Game.TickCount - Extensions.LastQCastAttempt <= 380 + Game.Ping / 2) 
            {
                return;
            }

            Extensions.DidJustAuto = true;

            if (MenuConfig.BurstMode.Active)
            {
                Burst.OnProcessAutoAttack();
            }
            else
            {
                switch (Global.Orbwalker.Mode)
                {
                    case OrbwalkingMode.Combo:
                        ComboManager.OnProcessAutoAttack();
                        break;
                    case OrbwalkingMode.Mixed:
                        Harass.OnProcessAutoAttack();
                        break;
                    case OrbwalkingMode.Laneclear:
                        if (args.Target.IsMinion)
                        {
                            Lane.OnProcessAutoAttack();
                            Jungle.OnProcessAutoAttack(args.Target as Obj_AI_Minion);
                        }
                        else if ((args.Target as Obj_AI_Base).IsBuilding() && SpellConfig.Q.Ready)
                        {
                            SpellConfig.Q.Cast(Global.Player.ServerPosition.Extend(args.Target.ServerPosition, 100));
                        }
                        break;
                }
            }
        }
    }
}
