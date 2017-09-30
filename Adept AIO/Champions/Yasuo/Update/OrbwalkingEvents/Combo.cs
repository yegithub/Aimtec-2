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
using Geometry = Adept_AIO.SDK.Geometry_Related.Geometry;

namespace Adept_AIO.Champions.Yasuo.Update.OrbwalkingEvents
{
    internal class Combo
    {
        public static void OnPostAttack()
        {
            var target = Global.TargetSelector.GetTarget(SpellConfig.R.Range);
            if (target == null)
            {
                return;
            }

            if (SpellConfig.R.Ready && Extension.KnockedUp(target))
            {
                SpellConfig.R.Cast();
            }

            if (SpellConfig.Q.Ready)
            {
                SpellConfig.Q.Cast(target);
            }

            if (!SpellConfig.E.Ready)
            {
                return;
            }

            var minion = MinionHelper.GetDashableMinion(target);
            var walkDashMinion = MinionHelper.WalkBehindMinion(target);

            if (!walkDashMinion.IsZero && MenuConfig.Combo["Walk"].Enabled && Global.Orbwalker.CanMove())
            {
                Global.Orbwalker.Move(walkDashMinion);
            }
            else if (minion != null)
            {
                if (MenuConfig.Combo["Turret"].Enabled && minion.ServerPosition.PointUnderEnemyTurret() || MenuConfig.Combo["Dash"].Value == 0 && minion.Distance(Game.CursorPos) > MenuConfig.Combo["Range"].Value)
                {
                    return;
                }
                SpellConfig.E.CastOnUnit(minion);
            }
            else if (!target.HasBuff("YasuoDashWrapper") && target.Distance(Global.Player) <= SpellConfig.E.Range && target.Distance(Global.Player) > SpellConfig.E.Range - target.BoundingRadius)
            {
                SpellConfig.E.CastOnUnit(target);
            }
        }

        public static void OnUpdate()
        {
            var target = Global.TargetSelector.GetTarget(2500);
            if (target == null)
            {
                return;
            }

            var targetDist = target.Distance(Global.Player);
            var minion = MinionHelper.GetDashableMinion(target);

            var m2 = MinionHelper.GetClosest(target);
            var positionBehindMinion = MinionHelper.WalkBehindMinion(target);
       
            MinionHelper.ExtendedMinion = positionBehindMinion;
            MinionHelper.ExtendedTarget = target.ServerPosition;

            var dashDistance = MinionHelper.DashDistance(minion, target);

            var airbourneTargets = GameObjects.EnemyHeroes.Where(x => Extension.KnockedUp(x) && x.Distance(Global.Player) <= SpellConfig.R.Range);
            var targetCount = (airbourneTargets as Obj_AI_Hero[] ?? airbourneTargets.ToArray()).Length;

            var circle = new Geometry.Circle(Global.Player.GetDashInfo().EndPos, 220);
            var circleCount = GameObjects.EnemyHeroes.Count(x => circle.Center.Distance(x.ServerPosition) <= circle.Radius);

            if (SpellConfig.Q.Ready && !(Global.Player.IsDashing() && circleCount <= 0))
            {
                switch (Extension.CurrentMode)
                {
                    case Mode.Dashing:
                        SpellConfig.Q.Cast(target);
                        break;
                    case Mode.DashingTornado:
                        if (minion != null)
                        {
                            if (MenuConfig.Combo["Flash"].Enabled && dashDistance > 400 && target.IsValidTarget(425) &&
                                (Dmg.Damage(target) * 1.25 > target.Health || target.CountEnemyHeroesInRange(220) >= 2))
                            {
                                SpellConfig.Q.Cast();

                                DelayAction.Queue(Game.Ping / 2 + 30, () =>
                                {
                                    SummonerSpells.Flash.Cast(target.Position);
                                }, new CancellationToken(false)); ;
                            }
                        }
                        else
                        {
                            SpellConfig.Q.Cast(target);
                        }
                        break;
                    case Mode.Tornado:
                        if (target.IsValidTarget(SpellConfig.Q.Range))
                        {
                            SpellConfig.Q.Cast(target);
                        }
                        break;
                    case Mode.Normal:
                        if (targetDist > 1200)
                        {
                            var stackableMinion = GameObjects.EnemyMinions.FirstOrDefault(x => x.IsEnemy && x.Distance(Global.Player) <= SpellConfig.Q.Range);
                            if (stackableMinion == null)
                            {
                                return;
                            }

                            SpellConfig.Q.Cast(stackableMinion);
                        }
                        else if (target.IsValidTarget(SpellConfig.Q.Range))
                        {
                            SpellConfig.Q.Cast(target);
                        }
                        break;
                }
            }

            if (SpellConfig.R.Ready && Extension.KnockedUp(target))
            {
                if (targetCount >= MenuConfig.Combo["Count"].Value || targetDist > 350 && minion == null)
                {
                    if (MenuConfig.Combo["Delay"].Enabled && KnockUpHelper.IsItTimeToUlt(target))
                    {
                        SpellConfig.R.Cast();
                    }
                    else
                    {
                        DelayAction.Queue(250, () => SpellConfig.R.Cast(), new CancellationToken(false));
                    }  
                }
            }

            if (!SpellConfig.E.Ready)
            {
                return;
            }

            if (!positionBehindMinion.IsZero && MenuConfig.Combo["Walk"].Enabled && targetDist > Global.Player.AttackRange 
                && Global.Orbwalker.CanMove() 
                && positionBehindMinion.Distance(Global.Player) <= MenuConfig.Combo["MRange"].Value
                && !(MenuConfig.Combo["Turret"].Enabled && minion != null && minion.IsUnderEnemyTurret() && target.IsUnderEnemyTurret()))
            {
                Global.Orbwalker.Move(positionBehindMinion);

                if (positionBehindMinion.Distance(Global.Player) <= 65)
                {
                    SpellConfig.E.CastOnUnit(m2);
                }
            }
            else if (minion != null && targetDist > Global.Player.AttackRange + 150)
            {
                if (MenuConfig.Combo["Turret"].Enabled && minion.ServerPosition.PointUnderEnemyTurret() || MenuConfig.Combo["Dash"].Value == 0 && minion.Distance(Game.CursorPos) > MenuConfig.Combo["Range"].Value)
                {
                    return;
                }

                SpellConfig.E.CastOnUnit(minion);
            }
            else if (!target.HasBuff("YasuoDashWrapper") && targetDist <= SpellConfig.E.Range && targetDist > SpellConfig.E.Range - target.BoundingRadius)
            {
                SpellConfig.E.CastOnUnit(target);
            }
        }
    }
}
