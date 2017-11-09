namespace Adept_AIO.Champions.Riven.OrbwalkingEvents.Combo
{
    using System;
    using System.Linq;
    using System.Threading;
    using Aimtec;
    using Aimtec.SDK.Damage;
    using Aimtec.SDK.Extensions;
    using Aimtec.SDK.Orbwalking;
    using Aimtec.SDK.Util;
    using Core;
    using Miscellaneous;
    using SDK.Generic;
    using SDK.Unit_Extensions;
    using SDK.Usables;

    class ComboManager
    {
        public static void OnPostAttack()
        {
            Combo2.OnPostAttack();
            //switch (Enums.ComboPattern)
            //{
            //    case ComboPattern.MaximizeDmg:
            //        MaximizeDmg.OnPostAttack(sender, args);
            //        break;

            //    case ComboPattern.Normal: break;

            //    case ComboPattern.FastCombo:
            //        FastCombo.OnPostAttack(sender, args);
            //        break;

            //    default: throw new ArgumentOutOfRangeException();
            //}
        }

        public static void OnUpdate()
        {
            ChaseTarget();
            Flash();
            Manage();
        }

        private static void Manage()
        {
            //Enums.ComboPattern = Generate();

            var target = Global.TargetSelector.GetTarget(Extensions.EngageRange);
            if (target == null)
            {
                return;
            }
            Combo2.OnUpdate();
            //switch (Enums.ComboPattern)
            //{
            //    case ComboPattern.MaximizeDmg:
            //        MaximizeDmg.OnUpdate(target);
            //        break;

            //    case ComboPattern.Normal: break;

            //    case ComboPattern.FastCombo:
            //        FastCombo.OnUpdate(target);
            //        break;

            //    default: throw new ArgumentOutOfRangeException();
            //}
        }

        private static ComboPattern Generate()
        {
            var target = Global.TargetSelector.GetTarget(Extensions.EngageRange + 700);
            if (target == null)
            {
                return ComboPattern.MaximizeDmg;
            }

            switch (MenuConfig.Combo["Mode"].Value)
            {
                case 0:

                    if (Maths.Percent(target.Health, Dmg.Damage(target)) >= MenuConfig.Combo["Change"].Value)
                    {
                        return ComboPattern.FastCombo;
                    }
                    return ComboPattern.MaximizeDmg;

                case 1: return ComboPattern.MaximizeDmg;
                case 2: return ComboPattern.FastCombo;
            }
            return ComboPattern.MaximizeDmg;
        }

        private static void ChaseTarget()
        {
            var target = GameObjects.EnemyHeroes.FirstOrDefault(x => x.IsValidTarget(1500));
            if (target == null)
            {
                return;
            }

            if (MenuConfig.Combo["Chase"].Value == 0 || target.Distance(Global.Player) < Global.Player.AttackRange + 65)
            {
                return;
            }

            switch (MenuConfig.Combo["Chase"].Value)
            {
                case 1:
                    if (target.Distance(Global.Player) <= Global.Player.AttackRange + SpellConfig.Q.Range && Extensions.CurrentQCount == 1 &&
                        target.Distance(Global.Player) > Global.Player.AttackRange)
                    {
                        SpellManager.CastQ(target, true);
                    }
                    break;
                case 2:
                    if (target.Distance(Global.Player) <= Global.Player.AttackRange + SpellConfig.Q.Range + SpellConfig.E.Range &&
                        target.Distance(Global.Player) > Global.Player.AttackRange + SpellConfig.Q.Range && SpellConfig.E.Ready && SpellConfig.Q.Ready && Extensions.CurrentQCount == 1)
                    {
                        SpellConfig.E.Cast(target.ServerPosition);
                        DelayAction.Queue(190, () => SpellManager.CastQ(target, true), new CancellationToken(false));
                    }
                    break;
            }
        }

        private static void Flash()
        {
            var target = Global.TargetSelector.GetTarget(1200);
            if (target == null || target.IsUnderEnemyTurret())
            {
                return;
            }

            Extensions.AllIn = MenuConfig.Combo["Flash"].Enabled && SummonerSpells.Flash.Ready && CanFlashKill(target) && target.Distance(Global.Player) > 500 && target.Distance(Global.Player) < 720;

            if (!Extensions.AllIn)
            {
                return;
            }

            SummonerSpells.Flash.Cast(target);

            if (target.IsValidTarget(SpellConfig.W.Range))
            {
                SpellManager.CastW(target);
            }
        }

        private static bool CanFlashKill(Obj_AI_Base target)
        {
            return target.Health < Dmg.Damage(target) * .3 && Global.Player.HealthPercent() >= 65 ||
                   target.Health < Global.Player.GetAutoAttackDamage(target) && GameObjects.AllyHeroes.FirstOrDefault(x => x.Distance(target) < 300) == null ||
                   target.Health < Dmg.Damage(target) * .75f && target.HealthPercent() <= 25;
        }

        public static bool CanCastR1(Obj_AI_Base target)
        {
            return MenuConfig.Combo["R"].Value != 0 && SpellConfig.R.Ready && !(MenuConfig.Combo["Check"].Enabled && target.HealthPercent() < MenuConfig.Combo["Check"].Value) &&
                   Enums.UltimateMode == UltimateMode.First && !(MenuConfig.Combo["R"].Value == 2 && Dmg.Damage(target) < target.Health);
        }
    }
}