using System.Linq;
using Adept_AIO.Champions.Yasuo.Core;
using Adept_AIO.SDK.Usables;
using Aimtec;
using Aimtec.SDK.Extensions;
using Aimtec.SDK.TargetSelector;
using Aimtec.SDK.Util;
using Aimtec.SDK.Util.Cache;

namespace Adept_AIO.Champions.Yasuo.Update.OrbwalkingEvents
{
    class Combo
    {
        public static void OnPostAttack()
        {
            var target = TargetSelector.GetTarget(SpellConfig.R.Range);
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

        public static void OnUpdate()
        {
            var target = TargetSelector.GetTarget(1800);
            if (target == null)
            {
                return;
            }

            var distance = target.Distance(ObjectManager.GetLocalPlayer());
            var dashDistance = ObjectManager.GetLocalPlayer().ServerPosition.Extend(target.ServerPosition, 475f).Distance(target.ServerPosition);

            var minion = Extension.GetDashableMinion(target);

            if (SpellConfig.E.Ready)
            {
                if (MenuConfig.Combo["Turret"].Enabled && target.IsUnderEnemyTurret())
                {
                    return;
                }

                if (!target.HasBuff("YasuoDashWrapper") && distance < SpellConfig.E.Range)
                {
                    SpellConfig.E.CastOnUnit(target);
                }
                else if (distance > SpellConfig.E.Range)
                {
                    if (minion != null || distance < ObjectManager.GetLocalPlayer().AttackRange)
                    {
                        SpellConfig.E.CastOnUnit(minion);
                    }
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
                            if (MenuConfig.Combo["Flash"].Enabled && target.IsValidTarget(425) && (Dmg.Damage(target) * 1.25 > target.Health || target.CountEnemyHeroesInRange(220) >= 2))
                            {
                                DelayAction.Queue(190, () =>
                                {
                                    SpellConfig.Q.Cast();
                                    SummonerSpells.Flash.Cast(target.Position);
                                });
                            }
                        }
                        else if (dashDistance <= 65)
                        {
                            SpellConfig.Q.Cast(target);
                        }
                        break;
                    case Mode.Tornado:
                        if (target.IsValidTarget(SpellConfig.Q.Range))
                        {
                            if (minion != null && ObjectManager.GetLocalPlayer().IsFacing(minion) && distance > SpellConfig.Q.Range / 2)
                            {
                                return;
                            }

                            SpellConfig.Q.Cast(target);
                        }
                        break;
                    case Mode.Normal:
                        if (target.IsValidTarget(SpellConfig.Q.Range))
                        {
                            SpellConfig.Q.CastOnUnit(target);
                        }
                        else if(distance > 800)
                        {
                            var stackableMinion = GameObjects.EnemyMinions.FirstOrDefault(x => x.IsEnemy && x.Distance(ObjectManager.GetLocalPlayer()) <= SpellConfig.Q.Range);
                            if (stackableMinion == null)
                            {
                                return;
                            }

                            SpellConfig.Q.Cast(stackableMinion);
                        }
                        break;
                }
            }

            if (SpellConfig.R.Ready && Extension.KnockedUp(target) && (distance > 800 || distance > 500 && minion == null))
            {
                DelayAction.Queue(MenuConfig.Combo["Delay"].Enabled ? 375 + Game.Ping / 2 : 100 + Game.Ping / 2, () => SpellConfig.R.Cast());
            }
        }
    }
}
