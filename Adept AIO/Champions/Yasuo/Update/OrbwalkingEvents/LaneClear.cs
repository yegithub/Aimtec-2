using System.Linq;
using Adept_AIO.Champions.Yasuo.Core;
using Aimtec;
using Aimtec.SDK.Damage;
using Aimtec.SDK.Extensions;
using Aimtec.SDK.Orbwalking;
using GameObjects = Adept_AIO.SDK.Extensions.GameObjects;

namespace Adept_AIO.Champions.Yasuo.Update.OrbwalkingEvents
{
    class LaneClear
    {
    
        public static void OnPostAttack()
        {
            if (MenuConfig.LaneClear["Check"].Enabled && ObjectManager.GetLocalPlayer().CountEnemyHeroesInRange(2000) != 0)
            {
                return;
            }

            if (SpellConfig.Q.Ready)
            {
                var stackableMinion = GameObjects.EnemyMinions.FirstOrDefault(x => x.IsEnemy && x.IsValidTarget(SpellConfig.Q.Range));
                if (stackableMinion == null)
                {
                    return;
                }

                switch (Extension.CurrentMode)
                {
                        case Mode.Normal:
                            SpellConfig.Q.Cast(stackableMinion);
                        break;
                }
            }
        }

        public static void OnUpdate()
        {
            if (SpellConfig.Q.Ready)
            {
                switch (Extension.CurrentMode)
                {
                    case Mode.Tornado:
                        foreach (var m in GameObjects.EnemyMinions.Where(x => x.IsEnemy && x.IsMinion && x.IsValidTarget(SpellConfig.Q.Range)))
                        {
                            if (SpellConfig.Q.GetPrediction(m).AoeTargetsHitCount >= MenuConfig.LaneClear["Q3"].Value && MenuConfig.LaneClear["Q3"].Enabled)
                            {
                                SpellConfig.Q.Cast(m);
                            }
                        }
                        break;
                    case Mode.DashingTornado:
                    case Mode.Dashing:
                        var dashM = GameObjects.EnemyMinions.Where(x => x.IsEnemy && x.IsMinion && x.Distance(ObjectManager.GetLocalPlayer()) <= 220);

                        var minions = dashM as Obj_AI_Minion[] ?? dashM.ToArray();
                        if (minions.Length > 2)
                        {
                            SpellConfig.Q.Cast(minions.FirstOrDefault());
                        }
                        break;
                }
            }

            var minion = GameObjects.EnemyMinions.FirstOrDefault(x => x.Distance(ObjectManager.GetLocalPlayer()) <= SpellConfig.E.Range &&
                                                                      x.Distance(Game.CursorPos) < MenuConfig.Combo["Range"].Value &&
                                                                     !x.HasBuff("YasuoDashWrapper"));

            if (minion == null || minion.IsUnderEnemyTurret() || MenuConfig.LaneClear["Check"].Enabled &&
                ObjectManager.GetLocalPlayer().CountEnemyHeroesInRange(2000) != 0)
            {
                return;
            }
           
            if (SpellConfig.E.Ready || Orbwalker.Implementation.IsWindingUp)
            {
                switch (MenuConfig.LaneClear["Mode"].Value)
                {
                    case 1:
                        if (minion.Health > ObjectManager.GetLocalPlayer().GetSpellDamage(minion, SpellSlot.E))
                        {
                            return;
                        }
                        SpellConfig.E.CastOnUnit(minion);
                        break;
                    case 2:
                        if (minion.Health < ObjectManager.GetLocalPlayer().GetAutoAttackDamage(minion) * 1.5f)
                        {
                            return;
                        }
                        SpellConfig.E.CastOnUnit(minion);
                        break;
                }
            }
        }
    }
}
