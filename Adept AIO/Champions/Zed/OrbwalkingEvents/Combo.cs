namespace Adept_AIO.Champions.Zed.OrbwalkingEvents
{
    using System.Linq;
    using Aimtec.SDK.Extensions;
    using Core;
    using SDK.Unit_Extensions;

    class Combo
    {
        public static void OnUpdate()
        {
            var target = Global.TargetSelector.GetTarget(SpellManager.WCastRange + SpellManager.Q.Range);
            if (target == null)
            {
                return;
            }

            if (SpellManager.R.Ready && target.IsValidTarget(SpellManager.R.Range) && !(MenuConfig.Combo["Killable"].Enabled && Dmg.Damage(target) < target.Health))
            {
                if (target.HealthPercent() <= 30)
                {
                    return;
                }

                if (!MenuConfig.Combo[target.ChampionName].Enabled)
                {
                    return;
                }

                SpellManager.CastR(target);
            }

            if (SpellManager.W.Ready && MenuConfig.Combo["W"].Enabled && target.IsValidTarget(SpellManager.WCastRange + SpellManager.R.Range))
            {
                if (ShadowManager.CanCastW1())
                {
                    if (!target.IsValidTarget(SpellManager.R.Range))
                    {
                        SpellManager.W.Cast(target.ServerPosition);
                    }
                    else if (MenuConfig.Combo["Extend"].Enabled)
                    {
                        foreach (var shadow in ShadowManager.Shadows)
                        {
                            SpellManager.W.Cast(target.ServerPosition.Extend(shadow.ServerPosition, -2000f));
                        }
                    }
                }
                else if (ShadowManager.CanSwitchToShadow() && ShadowManager.Shadows.FirstOrDefault().Distance(target) <= Global.Player.Distance(target) &&
                         target.Distance(Global.Player) > Global.Player.AttackRange + 65)
                {
                    SpellManager.W.Cast();
                }
            }
            else if (SpellManager.Q.Ready && MenuConfig.Combo["Q"].Enabled)
            {
                SpellManager.CastQ(target);
            }

            if (SpellManager.E.Ready && MenuConfig.Combo["E"].Enabled)
            {
                SpellManager.CastE(target);
            }
        }
    }
}