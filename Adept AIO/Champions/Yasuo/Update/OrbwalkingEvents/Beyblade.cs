using System.Threading;
using Adept_AIO.Champions.Yasuo.Core;
using Adept_AIO.SDK.Extensions;
using Adept_AIO.SDK.Usables;
using Aimtec;
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

            if (SpellConfig.R.Ready && Extension.KnockedUp(target))
            {
                SpellConfig.R.Cast();
            }
            else if (SpellConfig.Q.Ready)
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

            if (SpellConfig.R.Ready && distance > 300 && Extension.KnockedUp(target))
            {
                SpellConfig.R.Cast();
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
                else if (!target.HasBuff("YasuoDashWrapper") && distance <= SpellConfig.E.Range && !Extension.KnockedUp(target))
                {
                    SpellConfig.E.CastOnUnit(target);
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
                        if (minion != null)
                        {
                            if (MenuConfig.Combo["Flash"].Enabled && distance > 220 && dashDistance <= 350)
                            {
                                DelayAction.Queue(145, () =>
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
