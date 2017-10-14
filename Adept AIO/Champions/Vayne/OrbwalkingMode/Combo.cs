using Adept_AIO.Champions.Vayne.Core;
using Adept_AIO.SDK.Unit_Extensions;
using Aimtec;
using Aimtec.SDK.Extensions;
using Aimtec.SDK.Orbwalking;

namespace Adept_AIO.Champions.Vayne.OrbwalkingMode
{
    internal class Combo
    {
        public static void PostAttack(object sender, PostAttackEventArgs args)
        {
            if (!SpellManager.Q.Ready || MenuConfig.Combo["Q1"].Value == 1)
            {
                return;
            }

            SpellManager.CastQ(args.Target as Obj_AI_Base, MenuConfig.Combo["Mode1"].Value, MenuConfig.Combo["ToE"].Enabled);
        }

        public static void OnUpdate()
        {
            var target = Global.TargetSelector.GetTarget(1000);
            if (!target.IsValidTarget())
            {
                return;
            }

            if (SpellManager.Q.Ready && MenuConfig.Combo["Q1"].Value == 1)
            {
                SpellManager.CastQ(target, MenuConfig.Combo["Mode1"].Value, MenuConfig.Combo["ToE"].Enabled);
            }

            else if (SpellManager.E.Ready && MenuConfig.Combo["E"].Enabled && MenuConfig.Combo[target.ChampionName].Enabled)
            {
                SpellManager.CastE(target);
            }

            else if (SpellManager.R.Ready && MenuConfig.Combo["R"].Enabled)
            {
                if (target.Health > Dmg.Damage(target) && MenuConfig.Combo["Killable"].Enabled)
                {
                    return;
                }

                if (Global.Player.CountEnemyHeroesInRange(1500) >= MenuConfig.Combo["Count"].Value && target.HealthPercent() >= 25)
                {
                    SpellManager.R.Cast();
                }
            }
        }
    }
}
