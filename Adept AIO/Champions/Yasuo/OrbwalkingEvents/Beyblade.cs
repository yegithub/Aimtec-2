﻿namespace Adept_AIO.Champions.Yasuo.OrbwalkingEvents
{
    using System.Linq;
    using System.Threading;
    using Aimtec;
    using Aimtec.SDK.Extensions;
    using Aimtec.SDK.Util;
    using Core;
    using SDK.Unit_Extensions;
    using SDK.Usables;

    class Beyblade
    {
        public static void OnPostAttack()
        {
            var target = GameObjects.EnemyHeroes.OrderBy(x => x.Distance(Global.Player)).FirstOrDefault(x => x.Distance(Global.Player) <= Global.Player.AttackRange + 200);
            if (target == null)
            {
                return;
            }

            if (!target.HasBuff("YasuoDashWrapper"))
            {
                SpellConfig.E.Cast(target);

                DelayAction.Queue(Game.Ping / 2, () => SpellConfig.Q.Cast(), new CancellationToken(false));

                DelayAction.Queue(Game.Ping / 2 + 30, () => SpellConfig.R.Cast(), new CancellationToken(false));
            }
            else if (SpellConfig.R.Ready)
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
            var positionBehindMinion = MinionHelper.WalkBehindMinion(target, m2);

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
                        if (minion != null && dashDistance <= 425 && dashDistance > 220)
                        {
                            if (MenuConfig.Combo["Flash"].Enabled && SummonerSpells.IsValid(SummonerSpells.Flash))
                            {
                                DelayAction.Queue(Game.Ping / 2,
                                    () =>
                                    {
                                        SpellConfig.Q.Cast();
                                    },
                                    new CancellationToken(false));

                                DelayAction.Queue(Game.Ping / 2 + 50,
                                    () =>
                                    {
                                        SummonerSpells.Flash.Cast(target.Position);
                                    },
                                    new CancellationToken(false));
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

            if (SpellConfig.E.Ready)
            {
                if (distance <= 320)
                {
                    return;
                }

                SpellConfig.CastE(target);
            }
        }
    }
}