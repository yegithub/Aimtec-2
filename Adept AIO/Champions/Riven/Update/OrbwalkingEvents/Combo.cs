using System.Linq;
using Adept_AIO.Champions.Riven.Core;
using Adept_AIO.Champions.Riven.Update.Miscellaneous;
using Adept_AIO.SDK.Extensions;
using Adept_AIO.SDK.Usables;
using Aimtec;
using Aimtec.SDK.Damage;
using Aimtec.SDK.Extensions;

namespace Adept_AIO.Champions.Riven.Update.OrbwalkingEvents
{
    internal class Combo
    {
        public static void OnPostAttack(Obj_AI_Base target)
        {
            if (AutoBeforeR2(target) && (SpeedItUp(target) || Extensions.CurrentQCount == 1 && !SpellConfig.Q.Ready) &&
                target.Health < ObjectManager.GetLocalPlayer().GetAutoAttackDamage(target) + ObjectManager.GetLocalPlayer().GetSpellDamage(target, SpellSlot.R))
            {
                SpellManager.CastR2(target);
            }

            if (SpellConfig.Q.Ready)
            {
                SpellManager.CastQ(target);
            }

            if (MenuConfig.Combo["R"].Value != 0 && SpellConfig.R.Ready &&
                Extensions.UltimateMode == UltimateMode.First && !(MenuConfig.Combo["R"].Value == 2 && Dmg.Damage(target) < target.Health))
            {
                SpellConfig.R.Cast();
                SpellManager.CastW(target);
            }

            if (SpellConfig.W.Ready)
            {
                SpellManager.CastW(target);
            }
        }

        public static void OnUpdate()
        {
            var target = GlobalExtension.TargetSelector.GetTarget(SpellConfig.R2.Range);
            if (target != null && Extensions.UltimateMode == UltimateMode.Second && !AutoBeforeR2(target) && target.HealthPercent() <= 50)
            {
                SpellManager.CastR2(target);
            }
            else
            {
                ExecuteCombo();
                Flash();
            }
        }

        private static void ExecuteCombo()
        {
            var target = GlobalExtension.TargetSelector.GetTarget(Extensions.EngageRange());
            if (target == null || GlobalExtension.Orbwalker.IsWindingUp)
            {
                return;
            }

            if (SpellConfig.E.Ready)
            {
                ObjectManager.GetLocalPlayer().SpellBook.CastSpell(SpellSlot.E, target.ServerPosition);

                if (MenuConfig.Combo["R"].Value != 0 && SpellConfig.R.Ready &&
                    Extensions.UltimateMode == UltimateMode.First && Dmg.Damage(target) < target.Health)
                {
                    SpellConfig.R.Cast();
                }
            }

            if (SpellManager.InsideKiBurst(target) && SpellConfig.W.Ready)
            {
                SpellManager.CastW(target);
            }
        }

        private static void Flash()
        {
            var target = GlobalExtension.TargetSelector.GetTarget(1200);
            if (target == null || target.IsUnderEnemyTurret())
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

            if (SpellManager.InsideKiBurst(target))
            {
                SpellManager.CastW(target);
            }
        }

        private static bool AutoBeforeR2(GameObject target)
        {
            return target.Distance(ObjectManager.GetLocalPlayer()) < ObjectManager.GetLocalPlayer().AttackRange
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