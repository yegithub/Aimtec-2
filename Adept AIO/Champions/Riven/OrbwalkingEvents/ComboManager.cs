namespace Adept_AIO.Champions.Riven.OrbwalkingEvents
{
    using System.Linq;
    using System.Threading;
    using Aimtec;
    using Aimtec.SDK.Damage;
    using Aimtec.SDK.Events;
    using Aimtec.SDK.Extensions;
    using Aimtec.SDK.Util;
    using Core;
    using Miscellaneous;
    using SDK.Unit_Extensions;
    using SDK.Usables;

    class ComboManager
    {
        public static void OnPostAttack()
        {
            var target = Global.TargetSelector.GetTarget(Extensions.EngageRange);
            if (target == null)
            {
                return;
            }

            if (!SpellConfig.W.Ready && (!SpellConfig.Q.Ready || Extensions.CurrentQCount == 3) && SpellConfig.R2.Ready && Enums.UltimateMode == UltimateMode.Second &&
                MenuConfig.Combo["R2"].Enabled && target.HealthPercent() <= 40)
            {
                SpellManager.CastR2(target);
            }

            if (SpellConfig.Q.Ready)
            {
                SpellManager.CastQ(target);
            }
        }

        public static void OnUpdate()
        {
            ChaseTarget();
            Flash();
            Manage();
        }

        private static void Manage()
        {
            var target = Global.TargetSelector.GetTarget(Extensions.EngageRange);
            if (target == null)
            {
                return;
            }

            if (target.IsValidAutoRange() && SpellConfig.W.Ready)
            {
                SpellManager.CastW(target);
            }

            if (SpellConfig.E.Ready && !target.IsValidAutoRange())
            {
                SpellConfig.E.Cast(target.ServerPosition);
            }

            else if (SpellConfig.R.Ready && Enums.UltimateMode == UltimateMode.First && CanCastR1(target))
            {
                if (Global.Player.IsDashing() && Global.Player.GetDashInfo().EndPos.Distance(target) <= Global.Player.AttackRange + 50)
                {
                    return;
                }

                SpellConfig.R.Cast();
            }
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

            Extensions.AllIn = MenuConfig.Combo["Flash"].Enabled && SummonerSpells.Flash.Ready && CanFlashKill(target) && target.Distance(Global.Player) > 500 &&
                               target.Distance(Global.Player) < 720;

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