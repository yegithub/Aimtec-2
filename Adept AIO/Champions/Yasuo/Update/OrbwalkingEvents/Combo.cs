using System.Linq;
using Adept_AIO.Champions.Yasuo.Core;
using Adept_AIO.SDK.Extensions;
using Adept_AIO.SDK.Usables;
using Aimtec;
using Aimtec.SDK.Extensions;
using Aimtec.SDK.Util;

namespace Adept_AIO.Champions.Yasuo.Update.OrbwalkingEvents
{
    internal class Combo
    {
        public static void OnPostAttack()
        {
            var target = GlobalExtension.TargetSelector.GetTarget(SpellConfig.R.Range);
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
            var target = GameObjects.EnemyHeroes.FirstOrDefault(x => x.Distance(ObjectManager.GetLocalPlayer()) <= 3000);
            if (target == null)
            {
                return;
            }

            var distance = target.Distance(ObjectManager.GetLocalPlayer());
            var minion = Extension.GetDashableMinion(target);
            var dashDistance = Extension.DashDistance(minion, target);

            if (SpellConfig.Q.Ready)
            {
                switch (Extension.CurrentMode)
                {
                    case Mode.Dashing:
                        if (dashDistance <= 220)
                        {
                            SpellConfig.Q.Cast(target);
                        }
                        break;
                    case Mode.DashingTornado:
                        if (minion != null)
                        {
                            if (MenuConfig.Combo["Flash"].Enabled && dashDistance > 400 && target.IsValidTarget(425) && (Dmg.Damage(target) * 1.25 > target.Health || target.CountEnemyHeroesInRange(220) >= 2))
                            {
                                DelayAction.Queue(190, () =>
                                {
                                    SpellConfig.Q.Cast();
                                    SummonerSpells.Flash.Cast(target.Position);
                                });
                            }
                        }
                        else if (dashDistance <= 220)
                        {
                            SpellConfig.Q.Cast(target);
                        }
                        break;
                    case Mode.Tornado:
                        SpellConfig.Q.Cast(target);
                        break;
                    case Mode.Normal:
                        if (distance <= SpellConfig.Q.Range)
                        {
                            SpellConfig.Q.Cast(target);
                        }
                        else if (distance > 1200)
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

            if (SpellConfig.E.Ready)
            {
                if (!target.HasBuff("YasuoDashWrapper") && distance <= SpellConfig.E.Range)
                {
                    SpellConfig.E.CastOnUnit(target);
                }
                else if (minion != null)
                {
                    if (MenuConfig.Combo["Turret"].Enabled && minion.IsUnderEnemyTurret() || MenuConfig.Combo["Dash"].Value == 0 && minion.Distance(Game.CursorPos) > MenuConfig.Combo["Range"].Value)
                    {
                        return;
                    }

                    SpellConfig.E.CastOnUnit(minion);
                }
            }

            var airbourneTargets = GameObjects.EnemyHeroes.Where(x => Extension.KnockedUp(x) && x.Distance(ObjectManager.GetLocalPlayer()) <= SpellConfig.R.Range);
            var amount = airbourneTargets as Obj_AI_Hero[] ?? airbourneTargets.ToArray();

            if (SpellConfig.R.Ready && Extension.KnockedUp(target) && (amount.Length >= MenuConfig.Combo["Count"].Value || distance > 650 || distance > 350 && minion == null))
            {
                DelayAction.Queue(MenuConfig.Combo["Delay"].Enabled ? 375 + Game.Ping / 2 : 100 + Game.Ping / 2, () => SpellConfig.R.Cast());
            }
        }
    }
}
