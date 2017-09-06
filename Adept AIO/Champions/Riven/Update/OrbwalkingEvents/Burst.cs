using System.Threading;
using Adept_AIO.Champions.Riven.Core;
using Adept_AIO.Champions.Riven.Update.Miscellaneous;
using Adept_AIO.SDK.Junk;
using Adept_AIO.SDK.Methods;
using Adept_AIO.SDK.Usables;
using Aimtec;
using Aimtec.SDK.Extensions;
using Aimtec.SDK.Util;

namespace Adept_AIO.Champions.Riven.Update.OrbwalkingEvents
{
    internal class Burst
    {
        public static void OnPostAttack(Obj_AI_Base target)
        {
            switch (Enums.BurstPattern)
            {
                case BurstPattern.TheShy:

                    if (SpellConfig.R2.Ready)
                    {
                        SpellConfig.R2.CastOnUnit(target);
                    }
                    else if (SpellConfig.Q.Ready)
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

            var distance = target.Distance(Global.Player);

            Extensions.AllIn = SummonerSpells.IsValid(SummonerSpells.Flash);

            if (SpellConfig.Q.Ready)
            {
                Global.Orbwalker.Attack(target); // Prevents E WQ (1s delay) -> AA. (BUG)
            }

            switch (Enums.BurstPattern)
            {
                case BurstPattern.TheShy:

                    if (Extensions.AllIn)
                    {
                        if (SpellConfig.W.Ready && SpellManager.InsideKiBurst(target))
                        {
                            SpellManager.CastW(target);
                        }

                        if (SpellConfig.R.Ready
                         && Enums.UltimateMode == UltimateMode.First
                         && SpellConfig.E.Ready 
                         && distance < Extensions.FlashRange())
                        {
                            SpellConfig.E.Cast(target.ServerPosition);
                            SpellConfig.R.Cast();
                        }

                        if (distance < Extensions.FlashRange() && SpellConfig.W.Ready && SpellConfig.R.Ready && SummonerSpells.IsValid(SummonerSpells.Flash))
                        {
                            Global.Player.SpellBook.CastSpell(SpellSlot.W);
                            SummonerSpells.Flash.Cast(target.ServerPosition.Extend(Global.Player.ServerPosition, target.BoundingRadius));
                        }
                        else if (SpellConfig.E.Ready)
                        {
                            SpellConfig.E.Cast(target);
                        }
                    }
                    else if (target.IsValidTarget(SpellConfig.E.Range + Global.Player.AttackRange))
                    {
                        if (SpellConfig.E.Ready)
                        {
                            SpellConfig.E.Cast(target.ServerPosition);

                            if (SpellConfig.R.Ready && Enums.UltimateMode == UltimateMode.First)
                            {
                                SpellConfig.R.Cast();
                            }
                        }

                        if (SpellConfig.W.Ready)
                        {
                            SpellManager.CastW(target);
                        }
                    }

                    break;

                case BurstPattern.Execution:

                    if (distance > 1200)
                    {
                        return;
                    }

                    if (SpellConfig.R.Ready && Enums.UltimateMode == UltimateMode.First)
                    {
                        SpellConfig.R.Cast();
                    }
                  
                    if (distance > Extensions.FlashRange())
                    {
                        return;
                    }

                    if (SpellConfig.E.Ready && Enums.UltimateMode == UltimateMode.Second && Game.TickCount - SpellConfig.R.LastCastAttemptT >= 1100)
                    {
                        SpellConfig.E.Cast(target.ServerPosition);
                        SpellConfig.R2.Cast(target.ServerPosition);

                        DelayAction.Queue(100, () =>
                        {
                            SummonerSpells.Flash.Cast(target.ServerPosition);
                        }, new CancellationToken(false));

                        DelayAction.Queue(500, () =>
                        {
                            SpellConfig.W.Cast();
                            SpellManager.CastQ(target);
                        }, new CancellationToken(false));
                    }
                 
                    break;
            }  
        }

        private static BurstPattern GeneratePattern(Obj_AI_Base target)
        {
            return Mixed.PercentDmg(target, Dmg.Damage(target)) > 135 ? BurstPattern.Execution : BurstPattern.TheShy;
        }
    }
}
