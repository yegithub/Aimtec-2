using System.Linq;
using Adept_AIO.Champions.Irelia.Core;
using Adept_AIO.SDK.Extensions;
using Aimtec;
using Aimtec.SDK.Damage;
using Aimtec.SDK.Extensions;
using Aimtec.SDK.Orbwalking;
using GameObjects = Aimtec.SDK.Util.Cache.GameObjects;

namespace Adept_AIO.Champions.Irelia.Update.OrbwalkingEvents
{
    internal class Combo
    {
        public static void OnPreAttack(AttackableUnit target, PreAttackEventArgs preAttackEventArgs)
        {
            if (SpellConfig.E.Ready)
            {
                if (((Obj_AI_Base)target).HealthPercent() <= GlobalExtension.Player.HealthPercent() || Dmg.Damage((Obj_AI_Base)target) * 2 > target.Health)
                {
                    preAttackEventArgs.Cancel = true;
                    SpellConfig.E.CastOnUnit(target);
                }
            }
        }

        public static void OnPostAttack(AttackableUnit target)
        {
            if (target == null || !SpellConfig.W.Ready)
            {
                return;
            }

            SpellConfig.W.Cast();
            GlobalExtension.Orbwalker.ResetAutoAttackTimer();
        }

        public static void OnUpdate()
        {
            if (GlobalExtension.Orbwalker.IsWindingUp)
            {
                return;
            }

            var ganked = MenuConfig.Combo["Force"].Enabled &&
                         GameObjects.AllyHeroes.FirstOrDefault(x => x.SpellBook.GetSpell(SpellSlot.Summoner1).Name.ToLower().Contains("smite") ||
                                                                    x.SpellBook.GetSpell(SpellSlot.Summoner2).Name.ToLower().Contains("smite")) != null;

            if (SpellConfig.Q.Ready)
            {
                var killable = GlobalExtension.TargetSelector.GetTarget(SpellConfig.Q.Range);
                if (killable != null && ganked || Dmg.Damage(killable)*1.2 > killable?.Health)
                {
                    SpellConfig.Q.CastOnUnit(killable);
                }

                var longRangeTarget = GlobalExtension.TargetSelector.GetTarget(SpellConfig.Q.Range * 3);

                if (longRangeTarget == null || 
                    longRangeTarget.IsUnderEnemyTurret() && MenuConfig.Combo["Turret"].Enabled && longRangeTarget.Health > Dmg.Damage(longRangeTarget))
                {
                    return;
                }

                var minion = GameObjects.EnemyMinions.Where(x => x.Distance(GlobalExtension.Player) < SpellConfig.Q.Range &&
                                                                 x.Distance(longRangeTarget) < GlobalExtension.Player.Distance(longRangeTarget) &&
                                                                 x.Distance(longRangeTarget) < SpellConfig.Q.Range * 3)
                                                                 .OrderBy(x => x.Distance(longRangeTarget))
                                                                 .FirstOrDefault();

                if (minion == null || MenuConfig.Combo["Mode"].Value == 0 && minion.Distance(Game.CursorPos) > MenuConfig.Combo["Range"].Value)
                {
                    return;
                }

                if (minion.Health < GlobalExtension.Player.GetSpellDamage(minion, SpellSlot.Q))
                {
                    SpellConfig.Q.CastOnUnit(minion);
                }
                else if (SpellConfig.R.Ready && minion.Health >
                    GlobalExtension.Player.GetSpellDamage(minion, SpellSlot.Q) &&
                    minion.Health < GlobalExtension.Player.GetSpellDamage(minion, SpellSlot.R) +
                    GlobalExtension.Player.GetSpellDamage(minion, SpellSlot.Q) &&
                    (killable?.Health < Dmg.Damage(killable) || killable.HealthPercent() <= 40))
                {
                    SpellConfig.R.Cast(minion);
                }
            }

            var target = GlobalExtension.TargetSelector.GetTarget(SpellConfig.R.Range);
            if (target == null)
            {
                return;
            }


            if (MenuConfig.Combo["Killable"].Enabled && target.Distance(GlobalExtension.Player) < SpellConfig.Q.Range && target.Health < Dmg.Damage(target) && SpellConfig.Q.Ready)
            {
                SpellConfig.Q.CastOnUnit(target);
            }

            if (SpellConfig.E.Ready && target.Distance(GlobalExtension.Player) <= SpellConfig.E.Range)
            {
                if (ganked)
                {
                    if (SpellConfig.Q.Ready)
                    {
                        SpellConfig.Q.CastOnUnit(target);
                    }
                    SpellConfig.E.CastOnUnit(target);
                }

                if (target.HealthPercent() <= GlobalExtension.Player.HealthPercent() || Dmg.Damage(target) * 2 > target.Health)
                {
                    SpellConfig.E.CastOnUnit(target);
                }
            }

            if (SpellConfig.R.Ready && (target.Health < Dmg.Damage(target) || target.HealthPercent() <= 40 || SpellConfig.RCount < 4))
            {
                SpellConfig.R.Cast(target);
            }
        }
    }
}
