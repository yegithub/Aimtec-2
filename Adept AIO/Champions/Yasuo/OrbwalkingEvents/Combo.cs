namespace Adept_AIO.Champions.Yasuo.OrbwalkingEvents
{
    using System.Linq;
    using System.Threading;
    using Aimtec;
    using Aimtec.SDK.Events;
    using Aimtec.SDK.Extensions;
    using Aimtec.SDK.Util;
    using Core;
    using SDK.Unit_Extensions;
    using SDK.Usables;
    using Geometry = SDK.Geometry_Related.Geometry;

    class Combo
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
                var enemyHero = GameObjects.EnemyHeroes.FirstOrDefault(x => x.IsValidTarget(SpellConfig.Q.Range));
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
            if (minion != null && target.Distance(Global.Player) > Global.Player.AttackRange)
            {
                if (MenuConfig.Combo["Turret"].Enabled && minion.ServerPosition.PointUnderEnemyTurret() ||
                    MenuConfig.Combo["Dash"].Value == 0 && minion.Distance(Game.CursorPos) > MenuConfig.Combo["Range"].Value)
                {
                    return;
                }
                SpellConfig.E.CastOnUnit(minion);
            }
            else if (!target.HasBuff("YasuoDashWrapper") &&
                     target.Distance(Global.Player) <= SpellConfig.E.Range &&
                     target.Distance(Global.Player) > SpellConfig.E.Range - target.BoundingRadius)
            {
                SpellConfig.CastE(target);
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

            var positionBehindMinion = MinionHelper.WalkBehindMinion(target, minion);

            if (!positionBehindMinion.IsZero && positionBehindMinion.Distance(Global.Player) <= MenuConfig.Combo["MRange"].Value)
            {
                MinionHelper.ExtendedMinion = positionBehindMinion;
                MinionHelper.ExtendedTarget = target.ServerPosition;
            }

            var dashDistance = MinionHelper.DashDistance(minion, target);

            if (SpellConfig.E.Ready && targetDist > Global.Player.AttackRange)
            {
                if (MinionHelper.IsDashable(target) && (Extension.CurrentMode == Mode.Tornado || targetDist > SpellConfig.E.Range - target.BoundingRadius && GameObjects.EnemyMinions.Count(MinionHelper.IsDashable) == 0))
                {
                    SpellConfig.E.CastOnUnit(target);
                }

                if (MenuConfig.Combo["Walk"].Enabled &&
                    Global.Orbwalker.CanMove() &&
                    !(MenuConfig.Combo["Turret"].Enabled && target.IsUnderEnemyTurret()))
                {
                    SpellConfig.CastE(target, true, MenuConfig.Combo["MRange"].Value);
                }
                else if (minion != null)
                {
                    if (MenuConfig.Combo["Turret"].Enabled && minion.ServerPosition.PointUnderEnemyTurret() ||
                        MenuConfig.Combo["Dash"].Value == 0 && minion.Distance(Game.CursorPos) > MenuConfig.Combo["Range"].Value)
                    {
                        return;
                    }

                    SpellConfig.CastE(minion);
                }
            }

            if (SpellConfig.Q.Ready)
            {
                switch (Extension.CurrentMode)
                {
                    case Mode.Dashing:
                    case Mode.DashingTornado:
                        if (MenuConfig.Combo["Flash"].Enabled &&
                            dashDistance < 425 &&
                            dashDistance > 250 && targetDist > 220 &&
                            (Dmg.Damage(target) * 1.25 > target.Health || target.ServerPosition.CountEnemyHeroesInRange(220) >= 2))
                        {
                            SpellConfig.Q.Cast();
                            DelayAction.Queue(Game.Ping / 2 + 30, () => SummonerSpells.Flash.Cast(target.Position), new CancellationToken(false));
                        }
                        else
                        {
                            var circle = new Geometry.Circle(Global.Player.GetDashInfo().EndPos, 220);
                            var count = GameObjects.EnemyHeroes.Count(x => x.IsValidTarget() && x.Distance(circle.Center.To3D()) <= circle.Radius);

                            if (count != 0)
                            {
                                SpellConfig.Q.Cast(target);
                            }
                        }
                        break;
                    default: SpellConfig.CastQ(target); break;
                }
            }

            if (SpellConfig.R.Ready && KnockUpHelper.KnockedUp(target) && KnockUpHelper.IsItTimeToUlt(target))
            {
                var airbourneTargets = GameObjects.EnemyHeroes.Where(x => KnockUpHelper.KnockedUp(x) && x.Distance(Global.Player) <= SpellConfig.R.Range);
                var targetCount = (airbourneTargets as Obj_AI_Hero[] ?? airbourneTargets.ToArray()).Length;

                if (targetCount >= MenuConfig.Combo["Count"].Value || targetDist > 350)
                {
                    SpellConfig.R.Cast();
                }
            }
        }
    }
}