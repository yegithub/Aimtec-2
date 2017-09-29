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

            var distance = target.Distance(Global.Player);
            if (!target.HasBuff("YasuoDashWrapper") && distance <= SpellConfig.E.Range)
            {
                SpellConfig.E.CastOnUnit(target);

                DelayAction.Queue(Game.Ping / 2, () => SpellConfig.Q.Cast(), new CancellationToken(false));
                DelayAction.Queue(Game.Ping + 30, () => SpellConfig.R.Cast(), new CancellationToken(false));
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
                                DelayAction.Queue(Game.Ping / 2, () =>
                                {
                                    SpellConfig.Q.Cast();
                                    SummonerSpells.Flash.Cast(target.Position);
                                }, new CancellationToken(false));
                            }
                        }
                        break;
                    case Mode.Tornado:
                    case Mode.Normal:
                        if (target.IsValidTarget(SpellConfig.Q.Range) && Game.TickCount - SpellConfig.E.LastCastAttemptT >= 500 && Game.TickCount - SpellConfig.E.LastCastAttemptT <= 5000)
                        {
                            SpellConfig.Q.CastOnUnit(target);
                        }
                        break;
                }
            }

            if (SpellConfig.E.Ready)
            {
                if (distance <= 300)
                {
                    return;
                }

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
        }
    }
}
