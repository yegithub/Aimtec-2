namespace Adept_AIO.Champions.Riven.Miscellaneous
{
    using System;
    using System.Diagnostics;
    using Aimtec;
    using Aimtec.SDK.Extensions;
    using Core;
    using Orbwalker;
    using OrbwalkingEvents;
    using SDK.Generic;
    using SDK.Unit_Extensions;
    using OrbwalkingMode = Aimtec.SDK.Orbwalking.OrbwalkingMode;

    class Manager
    {
        public static void OnUpdate()
        {
            try
            {
                if (Global.Player.IsDead || Global.Orbwalker.IsWindingUp)
                {
                    return;
                }
                DebugConsole.WriteLine($"Q STATE: {Global.Player.GetSpell(SpellSlot.Q).State} | If random numbers, CORE IS OUT OF DATE.", MessageState.Debug);
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

                if (!SpellConfig.Q.Ready || Extensions.CurrentQCount == 1 || !MenuConfig.Miscellaneous["Active"].Enabled || Global.Player.HasBuff("Recall") ||
                    Global.Orbwalker.Mode == OrbwalkingMode.Laneclear || Global.Orbwalker.Mode == OrbwalkingMode.Lasthit ||
                    Environment.TickCount - Extensions.LastQCastAttempt < 3580 + Game.Ping / 2 ||
                    Environment.TickCount - Extensions.LastQCastAttempt > 3700 + Game.Ping / 2)
                {
                    return;
                }

                SpellConfig.Q.Cast();
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }

        public static void OnPostAttack(object sender, PostAttackEventArgs args)
        {
            if (Environment.TickCount - Extensions.LastQCastAttempt <= 400)
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
                        ComboManager.OnPostAttack();
                        break;
                    case OrbwalkingMode.Mixed:
                        Harass.OnProcessAutoAttack();
                        break;
                    case OrbwalkingMode.Laneclear:
                        if (args.Target.IsMinion)
                        {
                            Lane.OnProcessAutoAttack();
                            Jungle.OnProcessAutoAttack();
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