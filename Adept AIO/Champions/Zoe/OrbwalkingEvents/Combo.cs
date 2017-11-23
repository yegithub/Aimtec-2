namespace Adept_AIO.Champions.Zoe.OrbwalkingEvents
{
    using Aimtec.SDK.Extensions;
    using Core;
    using SDK.Unit_Extensions;

    class Combo
    {
        public static void OnUpdate()
        {
            var target = Global.TargetSelector.GetTarget(SpellManager.PaddleStar.IsZero ? SpellManager.Q.Range + 400 : 3000);
            if (target == null)
            {
                return;
            }
        
            if (SpellManager.W.Ready)
            {
                var defensive = SpellManager.GetDefensiveWSpell();
                var aggressive = SpellManager.GetAggressiveWSpell();

                if (aggressive != null)
                {
                    if (target.HealthPercent() <= MenuConfig.Combo["W"].Value && MenuConfig.Combo["W"].Enabled)
                    {
                        aggressive.CastOnUnit(target);
                    }
                }

                if(defensive != null)
                {
                    if (Global.Player.HealthPercent() <= MenuConfig.Combo["WP"].Value && MenuConfig.Combo["WP"].Enabled)
                    {
                        defensive.CastOnUnit(Global.Player);
                    }
                }
            }

            if (SpellManager.E.Ready &&
                MenuConfig.Combo["E"].Enabled)
            {
                SpellManager.CastE(target);
            }

            if (!SpellManager.Q.Ready ||
                !MenuConfig.Combo["Q"].Enabled)
            {
                return;
            }

            if (MenuConfig.Combo["QHit"].Enabled && !target.IsHardCc())
            {
               return;
            }

            if (SpellManager.R.Ready &&
                MenuConfig.Combo["R"].Enabled && 
                SpellManager.PaddleStar.IsZero && target.Distance(Global.Player) <= Global.Player.AttackRange)
            {
                SpellManager.CastR(target, MenuConfig.Combo["Flash"].Enabled);
            }

            SpellManager.CastQ(target);
        }
    }
}