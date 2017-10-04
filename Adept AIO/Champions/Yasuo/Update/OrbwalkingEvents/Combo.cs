using System.Linq;
using System.Threading;
using Adept_AIO.Champions.Yasuo.Core;
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

            if (SpellConfig.R.Ready && KnockUpHelper.KnockedUp(target))
            {
                SpellConfig.R.Cast();
            }

            if (SpellConfig.Q.Ready)
            {
                var enemyHero = GameObjects.EnemyHeroes.OrderBy(x => x.Health).FirstOrDefault(x => x.IsValidTarget(SpellConfig.Q.Range));
                if (enemyHero != null)
                {
                    SpellConfig.Q.Cast(enemyHero);
                }
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
            if (target == null || !target.IsValidTarget())
            {
                return;
            }

            var targetDist = target.Distance(Global.Player);
            var minion = MinionHelper.GetDashableMinion(target);

            var m2 = MinionHelper.GetClosest(target);
            var positionBehindMinion = MinionHelper.WalkBehindMinion(target);

            if (!positionBehindMinion.IsZero && positionBehindMinion.Distance(Global.Player) <= MenuConfig.Combo["MRange"].Value)
            {
                MinionHelper.ExtendedMinion = positionBehindMinion;
                MinionHelper.ExtendedTarget = target.ServerPosition;
            }

            var dashDistance = MinionHelper.DashDistance(minion, target);

            var airbourneTargets = GameObjects.EnemyHeroes.Where(x => KnockUpHelper.KnockedUp(x) && x.Distance(Global.Player) <= SpellConfig.R.Range);
            var targetCount = (airbourneTargets as Obj_AI_Hero[] ?? airbourneTargets.ToArray()).Length;

            var circle = new Geometry.Circle(Global.Player.GetDashInfo().EndPos, 220);
            var circleCount = GameObjects.EnemyHeroes.Count(x => circle.Center.Distance(x.ServerPosition) <= circle.Radius);

            if (SpellConfig.E.Ready)
            {
                if (!positionBehindMinion.IsZero
                    && positionBehindMinion.Distance(Global.Player) <= MenuConfig.Combo["MRange"].Value
                    && MenuConfig.Combo["Walk"].Enabled && targetDist > Global.Player.AttackRange
                    && Global.Orbwalker.CanMove()
                    && !(MenuConfig.Combo["Turret"].Enabled && target.IsUnderEnemyTurret()))
                {
                    Global.Orbwalker.Move(positionBehindMinion);

                    if (positionBehindMinion.Distance(Global.Player) <= 65)
                    {
                        SpellConfig.E.CastOnUnit(m2);
                    }
                }
                else if (minion != null && (targetDist > Global.Player.AttackRange + 80 || SpellConfig.Q.Ready))
                {
              
                    if (MenuConfig.Combo["Turret"].Enabled && minion.ServerPosition.PointUnderEnemyTurret() ||
                        MenuConfig.Combo["Dash"].Value == 0 &&
                        minion.Distance(Game.CursorPos) > MenuConfig.Combo["Range"].Value)
                    {
                        return;
                    }

                    SpellConfig.E.CastOnUnit(minion);
                }
                else if (!target.HasBuff("YasuoDashWrapper") && targetDist <= SpellConfig.E.Range &&
                    targetDist > SpellConfig.E.Range - 50)
                {
                    SpellConfig.E.CastOnUnit(target);
                }
            }

            if (SpellConfig.Q.Ready)
            {
                if (Extension.CurrentMode == Mode.DashingTornado)
                {
                    if (minion != null && MenuConfig.Combo["Flash"].Enabled && dashDistance < 470 && dashDistance > 220 && (Dmg.Damage(target) * 1.25 > target.Health || target.ServerPosition.CountEnemyHeroesInRange(250) >= 2))
                    {
                        SpellConfig.Q.Cast();
                        DelayAction.Queue(Game.Ping / 2 + 30, () => SummonerSpells.Flash.Cast(target.Position), new CancellationToken(false));
                    }
                    else if(circleCount >= 1)
                    {
                        SpellConfig.Q.Cast(target);
                    }
                }
                else if(target.IsValidTarget(SpellConfig.Q.Range))
                {
                    var enemyHero = GameObjects.EnemyHeroes.OrderBy(x => x.Health).FirstOrDefault(x => x.IsValidTarget(SpellConfig.Q.Range));
                    if (enemyHero != null)
                    {
                        SpellConfig.Q.Cast(enemyHero);
                    }
                }
            }

            if (SpellConfig.R.Ready && KnockUpHelper.KnockedUp(target) && KnockUpHelper.IsItTimeToUlt(target))
            {
                if (targetCount >= MenuConfig.Combo["Count"].Value || targetDist > 350 && minion == null)
                {
                    SpellConfig.R.Cast();
                }
            }
        }
    }
}
