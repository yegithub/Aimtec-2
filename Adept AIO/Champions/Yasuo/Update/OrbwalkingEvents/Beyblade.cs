using System.Linq;
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
            var target = GameObjects.EnemyHeroes.FirstOrDefault(x => x.Distance(Global.Player) <= Global.Player.AttackRange + 200);
            if (target == null)
            {
                return;
            }
        
            if (!target.HasBuff("YasuoDashWrapper"))
            {
                SpellConfig.E.Cast(target);

                DelayAction.Queue(Game.Ping / 2 + 40, () => SpellConfig.Q.Cast(), new CancellationToken(false));

                DelayAction.Queue(Game.Ping / 2 + 95, () => SpellConfig.R.Cast(), new CancellationToken(false));

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

            if (SpellConfig.Q.Ready)
            {
                switch (Extension.CurrentMode)
                {
                    case Mode.Dashing:
                        SpellConfig.Q.Cast();
                        break;
                    case Mode.DashingTornado:
                        if (minion != null && Global.Player.Distance(target) <= 525)
                        {
                            if (MenuConfig.Combo["Flash"].Enabled && SummonerSpells.IsValid(SummonerSpells.Flash))
                            {
                                DelayAction.Queue(Game.Ping / 2 + 10, () =>
                                {
                                    SpellConfig.Q.Cast();
                                    SummonerSpells.Flash.Cast(target.Position);
                                }, new CancellationToken(false)); ;
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

            //if (KnockUpHelper.IsItTimeToUlt(target, 860) && SpellConfig.R.Ready)
            //{
            //    SpellConfig.R.Cast();
            //}

            if (SpellConfig.E.Ready)
            {
                if (distance <= 320)
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
