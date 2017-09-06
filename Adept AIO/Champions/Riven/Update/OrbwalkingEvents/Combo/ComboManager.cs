using System;
using System.Linq;
using Adept_AIO.Champions.Riven.Core;
using Adept_AIO.Champions.Riven.Update.Miscellaneous;
using Adept_AIO.SDK.Junk;
using Adept_AIO.SDK.Methods;
using Adept_AIO.SDK.Usables;
using Aimtec;
using Aimtec.SDK.Damage;
using Aimtec.SDK.Extensions;

namespace Adept_AIO.Champions.Riven.Update.OrbwalkingEvents.Combo
{
    internal class ComboManager
    {
        public static void OnPostAttack(Obj_AI_Base target)
        {
            switch (Enums.ComboPattern)
            {
                case ComboPattern.MaximizeDmg:
                    MaximizeDmg.OnPostAttack(target);
                    break;

                case ComboPattern.Normal:
                    break;

                case ComboPattern.FastCombo:
                    FastCombo.OnPostAttack(target);
                    break;

                default: throw new ArgumentOutOfRangeException();
            }
        }

        public static void OnUpdate()
        {
            ChaseTarget();

            var target = Global.TargetSelector.GetTarget(Extensions.EngageRange);
            if (target == null)
            {
                return;
            }

            Enums.ComboPattern = Generate(target);
           
            Flash();

            switch (Enums.ComboPattern)
            {
                case ComboPattern.MaximizeDmg:
                    MaximizeDmg.OnUpdate(target);
                    break;

                case ComboPattern.Normal:

                    break;

                case ComboPattern.FastCombo:
                    FastCombo.OnUpdate(target);
                    break;

                default: throw new ArgumentOutOfRangeException();
            }
        }

        private static ComboPattern Generate(Obj_AI_Base target)
        {
            switch (MenuConfig.Combo["Mode"].Value)
            {
                case 0:
                    if (Mixed.PercentDmg(target, Dmg.Damage(target)) >= 90)
                    {
                        return ComboPattern.FastCombo;
                    }
                    return ComboPattern.MaximizeDmg;

                case 1: return ComboPattern.MaximizeDmg;
                case 2: return ComboPattern.FastCombo;
            }

            return ComboPattern.MaximizeDmg; //Dmg.Damage(target) / target.Health >= 70 ? ComboPattern.Normal : ComboPattern.MaximizeDmg;
        }

        private static void ChaseTarget()
        {
            var target = GameObjects.EnemyHeroes.FirstOrDefault(x => x.IsValidTarget(1500));
            if (target == null)
            {
                return;
            }

            if (MenuConfig.Combo["Chase"].Value != 0 && target.Distance(Global.Player) > Global.Player.AttackRange)
            {
                switch (MenuConfig.Combo["Chase"].Value)
                {
                    case 1:
                        if (target.Distance(Global.Player) <= Global.Player.AttackRange + SpellConfig.Q.Range && Extensions.CurrentQCount == 1)
                        {
                            SpellManager.CastQ(target); // Bug: Will only cast Q when inside of Q range. CBA fix now
                                                        // Todo: Add Q casting towards a Vector3
                        }
                        break;
                    case 2:
                        if (target.Distance(Global.Player) <= Global.Player.AttackRange + SpellConfig.E.Range)
                        {
                            SpellConfig.E.Cast(target.ServerPosition);
                        }
                        break;
                    case 3:
                        if (target.Distance(Global.Player) <= Global.Player.AttackRange + SpellConfig.Q.Range + SpellConfig.E.Range
                         && target.Distance(Global.Player) > Global.Player.AttackRange + SpellConfig.Q.Range)
                        {
                            SpellConfig.E.Cast(target.ServerPosition);
                            SpellManager.CastQ(target);
                        }
                        break;
                }
            }
        }

        private static void Flash()
        {
            var target = Global.TargetSelector.GetTarget(1200);
            if (target == null || target.IsUnderEnemyTurret())
            {
                return;
            }

            Extensions.AllIn = MenuConfig.Combo["Flash"].Enabled &&
                               SummonerSpells.Flash.Ready &&
                               SafetyCheck(target) &&
                               target.Distance(Global.Player) > 500 &&
                               target.Distance(Global.Player) < 720;

            if (!Extensions.AllIn)
            {
                return;
            }

            SummonerSpells.Flash.Cast(target);

            if (SpellManager.InsideKiBurst(target))
            {
                SpellManager.CastW(target);
            }
        }

        private static bool SafetyCheck(Obj_AI_Base target)
        {
            return target.Health < Dmg.Damage(target) * .3 && Global.Player.HealthPercent() >= 65 ||
                   target.Health < Global.Player.GetAutoAttackDamage(target) && GameObjects.AllyHeroes.FirstOrDefault(x => x.Distance(target) < 300) == null ||
                   target.Health < Dmg.Damage(target) * .75f && target.HealthPercent() <= 25;
        }

        public static bool CanCastR1(Obj_AI_Base target)
        {
            return MenuConfig.Combo["R"].Value != 0
                   && SpellConfig.R.Ready
                   && !(MenuConfig.Combo["Check"].Enabled && target.HealthPercent() < MenuConfig.Combo["Check"].Value)
                   && Enums.UltimateMode == UltimateMode.First
                   && !(MenuConfig.Combo["R"].Value == 2 && Dmg.Damage(target) < target.Health);
        }
    }
}