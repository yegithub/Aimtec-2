namespace Adept_AIO.Champions.Riven.OrbwalkingEvents
{
    using System;
    using System.Threading;
    using Aimtec;
    using Aimtec.SDK.Extensions;
    using Aimtec.SDK.Util;
    using Core;
    using Miscellaneous;
    using SDK.Generic;
    using SDK.Unit_Extensions;
    using SDK.Usables;

    class Burst
    {
        public static void OnProcessAutoAttack()
        {
            var target = Global.Orbwalker.GetOrbwalkingTarget() as Obj_AI_Base;
            if (target == null)
            {
                return;
            }
            switch (Enums.BurstPattern)
            {
                case BurstPattern.TheShy:

                    if (SpellConfig.Q.Ready)
                    {
                        SpellManager.CastQ(target);
                    }
                    break;

                case BurstPattern.Execution:
                    if (SpellConfig.Q.Ready)
                    {
                        SpellManager.CastQ(target);
                    }
                    break;
            }
        }

        public static void OnUpdate()
        {
            var target = MenuConfig.BurstMode.GetTarget() as Obj_AI_Hero;

            if (target == null || !MenuConfig.BurstMenu[target.ChampionName].Enabled)
            {
                return;
            }

            Enums.BurstPattern = GeneratePattern(target);

            Extensions.AllIn = SummonerSpells.IsValid(SummonerSpells.Flash);

            if (!target.IsValidTarget(Extensions.FlashRange()))
            {
                return;
            }

            if (SpellConfig.R2.Ready && Enums.UltimateMode == UltimateMode.Second)
            {
                SpellManager.CastR2(target);
            }

            if (target.IsValidSpellTarget(SpellConfig.W.Range) && SpellConfig.W.Ready)
            {
                Global.Player.SpellBook.CastSpell(SpellSlot.W);
            }

            if (SpellConfig.R.Ready && Enums.UltimateMode == UltimateMode.First && SpellConfig.E.Ready)
            {
                SpellConfig.E.Cast(target.ServerPosition);
                SpellConfig.R.Cast();
            }
            else if (SpellConfig.E.Ready)
            {
                SpellConfig.E.Cast(target.ServerPosition);
            }

            switch (Enums.BurstPattern)
            {
                case BurstPattern.TheShy:

                    if (Extensions.AllIn && target.Distance(Global.Player) > SpellConfig.E.Range + Global.Player.AttackRange && SummonerSpells.IsValid(SummonerSpells.Flash))
                    {
                        DelayAction.Queue(Game.Ping / 2 + 50,
                                          delegate
                                          {
                                              Global.Player.SpellBook.CastSpell(SpellSlot.W);
                                          },
                                          new CancellationToken(false));

                        DelayAction.Queue(150,
                                          delegate
                                          {
                                              SummonerSpells.Flash.Cast(target.ServerPosition);
                                          },
                                          new CancellationToken(false));
                    }
                    break;

                case BurstPattern.Execution:

                    if (SpellConfig.R.Ready && Enums.UltimateMode == UltimateMode.First)
                    {
                        SpellConfig.R.Cast();
                    }
                    else if (SpellConfig.E.Ready && Enums.UltimateMode == UltimateMode.Second && Environment.TickCount - SpellConfig.R.LastCastAttemptT >= 1100)
                    {
                        SpellConfig.E.Cast(target.ServerPosition);
                        SpellConfig.R2.Cast(target.ServerPosition);

                        DelayAction.Queue(100,
                                          () =>
                                          {
                                              SummonerSpells.Flash.Cast(target.ServerPosition);
                                          },
                                          new CancellationToken(false));

                        DelayAction.Queue(500,
                                          () =>
                                          {
                                              SpellConfig.W.Cast();
                                              SpellManager.CastQ(target);
                                          },
                                          new CancellationToken(false));
                    }

                    break;
            }
        }

        private static BurstPattern GeneratePattern(Obj_AI_Base target)
        {
            switch (MenuConfig.BurstMenu["Mode"].Value)
            {
                case 0: return Maths.Percent(target.Health, Dmg.Damage(target)) > 135 ? BurstPattern.Execution : BurstPattern.TheShy;
                case 1: return BurstPattern.TheShy;
                case 2: return BurstPattern.Execution;
                default: throw new ArgumentOutOfRangeException();
            }
        }
    }
}