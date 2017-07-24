using System;
using System.Linq;
using Adept_AIO.Champions.Riven.Core;
using Adept_AIO.Champions.Riven.Update.Miscellaneous;
using Adept_AIO.SDK.Usables;
using Aimtec;
using Aimtec.SDK.Damage;
using Aimtec.SDK.Extensions;
using Aimtec.SDK.Orbwalking;
using Aimtec.SDK.TargetSelector;
using Aimtec.SDK.Util.Cache;

namespace Adept_AIO.Champions.Riven.Update.OrbwalkingEvents
{
    internal class Combo
    {
        public static void OnPostAttack()
        {
            var target = TargetSelector.GetTarget(Extensions.GetRange() + 65);

            if (target == null)
            {
                return;
            }

            if (SpellConfig.Q.Ready && MenuConfig.Combo["Q"].Enabled)
            {
                SpellManager.CastQ(target);
            }

            if(MenuConfig.Combo["W"].Enabled && SpellManager.InsideKiBurst(target))
            {
                if (SpellConfig.R.Ready && Extensions.UltimateMode == UltimateMode.First)
                {
                    RW(target, false);
                }

                SpellManager.CastW(target, Environment.TickCount - Extensions.LastETime > 400);
            }

            if (AutoBeforeR2(target) && (SpeedItUp(target) || Extensions.CurrentQCount == 1) &&
               (!SpellConfig.Q.Ready || target.Health < ObjectManager.GetLocalPlayer().GetAutoAttackDamage(target) +
               ObjectManager.GetLocalPlayer().GetSpellDamage(target, SpellSlot.R)))
            {
                SpellManager.CastR2(target);
            }
        }

        public static void OnUpdate()
        {
            var ultTarget = TargetSelector.GetTarget(SpellConfig.R2.Range);
            if (ultTarget != null && Extensions.UltimateMode == UltimateMode.Second && !AutoBeforeR2(ultTarget) && ultTarget.HealthPercent() <= 50)
            {
                SpellManager.CastR2(ultTarget);
            }
            else
            {
                ExecuteCombo();
                Flash();
            }
        }

        private static void ExecuteCombo()
        {
            var target = TargetSelector.GetTarget(Extensions.GetRange());
            if (target == null)
            {
                return;
            }

            if (SpellConfig.E.Ready && MenuConfig.Combo["E"].Enabled)
            {
                SpellConfig.E.CastOnUnit(target);
            }

            if (Environment.TickCount - Extensions.LastETime > 300 + Game.Ping / 2f &&
                !Orbwalker.Implementation.CanAttack() && 
                SpellConfig.R.Ready &&
                target.Distance(ObjectManager.GetLocalPlayer()) > ObjectManager.GetLocalPlayer().AttackRange)
            {
                RW(target, true);
            }
        }

        private static void RW(Obj_AI_Base target, bool doublecast)
        {
            if (SpellConfig.R.Ready && Extensions.UltimateMode == UltimateMode.First &&
                MenuConfig.Combo["R"].Value != 0 && (MenuConfig.Combo["R"].Value != 2 || SpeedItUp(target)))
            {
                SpellConfig.R.Cast();
            }

            if (MenuConfig.Combo["W"].Enabled && SpellManager.InsideKiBurst(target))
            {
                SpellManager.CastW(target, doublecast);
            }
        }

        private static void Flash()
        {
            var target = TargetSelector.GetTarget(1200);
            if (target == null)
            {
                return;
            }

            Extensions.AllIn = MenuConfig.Combo["Flash"].Enabled &&
                               SummonerSpells.Flash.Ready &&
                               SpeedItUp(target) &&
                               target.Distance(ObjectManager.GetLocalPlayer()) > 500 &&
                               target.Distance(ObjectManager.GetLocalPlayer()) < 720;

            if (!Extensions.AllIn)
            {
                return;
            }

            SummonerSpells.Flash.Cast(target);

            if (MenuConfig.Combo["W"].Enabled && SpellManager.InsideKiBurst(target))
            {
                SpellManager.CastW(target);
            }
        }

        private static bool AutoBeforeR2(GameObject target)
        {
            return target.Distance(ObjectManager.GetLocalPlayer()) < ObjectManager.GetLocalPlayer().AttackRange + 100
                   && SpellConfig.R2.Ready
                   && Extensions.UltimateMode == UltimateMode.Second
                   && MenuConfig.Combo["R2"].Value == 1;
        }

        private static bool SpeedItUp(Obj_AI_Base target)
        {
            return target.Health < Dmg.Damage(target) * .35 && ObjectManager.GetLocalPlayer().HealthPercent() >= 65 || 
                   target.Health < ObjectManager.GetLocalPlayer().GetAutoAttackDamage(target) && GameObjects.AllyHeroes.FirstOrDefault(x => x.Distance(target) < 300) == null ||
                   target.Health < Dmg.Damage(target) * .75f && target.HealthPercent() <= 40;
        }
    }
}
