using System.Threading;
using Adept_AIO.Champions.Yasuo.Core;
using Adept_AIO.SDK.Generic;
using Adept_AIO.SDK.Unit_Extensions;
using Adept_AIO.SDK.Usables;
using Aimtec;
using Aimtec.SDK.Events;
using Aimtec.SDK.Extensions;
using Aimtec.SDK.Util;

namespace Adept_AIO.Champions.Yasuo.Update.OrbwalkingEvents
{
    internal class Beyblade
    {
        public static void OnPostAttack()
        {
            var target = Global.TargetSelector.GetSelectedTarget();
            if (target == null)
            {
                return;
            }

            if (SpellConfig.Q.Ready)
            {
                SpellConfig.Q.Cast(target);
            }
        }

        public static void OnKeyPressed()
        {
            var target = Extension.BeybladeMode.GetTarget() as Obj_AI_Hero;
            if (target == null)
            {
                return;
            }

            var distance = target.Distance(Global.Player);
            var minion = MinionHelper.GetDashableMinion(target);

            var m2 = MinionHelper.GetClosest(target);
            var positionBehindMinion = MinionHelper.WalkBehindMinion(target);

            MinionHelper.ExtendedMinion = positionBehindMinion;
            MinionHelper.ExtendedTarget = target.ServerPosition;

            var dashDistance = MinionHelper.DashDistance(minion, target);

            //if (SpellConfig.R.Ready && Extension.KnockedUp(target))
            //{
            //    if (target.HasBuff("YasuoDashWrapper") ||
            //        Game.TickCount - KnockUpHelper.TimeLeftOnKnockup >= 1000 && Game.TickCount - KnockUpHelper.TimeLeftOnKnockup <= 3000 - Game.Ping / 2 &&
            //        Game.TickCount - SpellConfig.Q.LastCastAttemptT > 300 && Game.TickCount - SpellConfig.Q.LastCastAttemptT <= 1000)
            //    {
            //        SpellConfig.R.Cast();
            //    }
            //}

            DebugConsole.Print($"TIME LEFT: {KnockUpHelper.TimeLeftOnKnockup - Game.TickCount}");

            if (Game.TickCount - KnockUpHelper.TimeLeftOnKnockup >= 500 &&
                Game.TickCount - KnockUpHelper.TimeLeftOnKnockup <= 4000 - Game.Ping / 2)
            {
                if (!target.HasBuff("YasuoDashWrapper") && distance <= SpellConfig.E.Range)
                {
                    SpellConfig.E.CastOnUnit(target);
                }

                if (Global.Player.IsDashing())
                {
                    DelayAction.Queue(200, () => SpellConfig.Q.Cast(target), new CancellationToken(false));
                    DelayAction.Queue(400, () => SpellConfig.R.Cast(), new CancellationToken(false));
                }
               
            }

            if (SpellConfig.E.Ready)
            {
                if (!positionBehindMinion.IsZero && Global.Orbwalker.CanMove())
                {
                    Global.Orbwalker.Move(positionBehindMinion);
                    if (positionBehindMinion.Distance(Global.Player) <= 65)
                    {
                        SpellConfig.E.CastOnUnit(m2);
                    }
                }
                else if (minion != null)
                {
                    SpellConfig.E.CastOnUnit(minion);
                }
            }

            if (SpellConfig.Q.Ready)
            {
                switch (Extension.CurrentMode)
                {
                    case Mode.DashingTornado:
                    case Mode.Dashing:
                        if (minion != null && minion.Distance(target) <= 440)
                        {
                            if (MenuConfig.Combo["Flash"].Enabled && SummonerSpells.IsValid(SummonerSpells.Flash) && distance > 220 && dashDistance <= 350)
                            {
                                DelayAction.Queue(85, () =>
                                {
                                    SpellConfig.Q.Cast();
                                    SummonerSpells.Flash.Cast(target.Position);
                                }, new CancellationToken(false));
                            }
                        }
                        break;
                    case Mode.Normal:
                        if (target.IsValidTarget(SpellConfig.Q.Range))
                        {
                            SpellConfig.Q.CastOnUnit(target);
                        }
                        break;
                }
            }
        }
    }
}
