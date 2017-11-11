namespace Adept_AIO.Champions.Gragas.OrbwalkingEvents
{
    using Aimtec.SDK.Extensions;
    using Core;
    using SDK.Unit_Extensions;

    class Combo
    {
        public static void OnUpdate()
        {
            var target = Global.TargetSelector.GetTarget(2000);
            if (target == null || !target.IsValidTarget())
            {
                return;
            }

            if (SpellManager.R.Ready && MenuConfig.Combo["R"].Enabled &&
                (!MenuConfig.Combo["Killable"].Enabled || Dmg.Damage(target) > target.Health || !target.IsValidTarget(SpellManager.R.Range)))
            {
                SpellManager.CastR(target);
            }

            else if (SpellManager.Q.Ready && MenuConfig.Combo["Q"].Enabled)
            {
                SpellManager.CastQ(target);
            }

            else if (SpellManager.W.Ready && MenuConfig.Combo["W"].Enabled)
            {
                SpellManager.CastW(target);
            }

            else if (SpellManager.E.Ready && MenuConfig.Combo["E"].Enabled)
            {
                SpellManager.CastE(target, MenuConfig.Combo["Flash"].Enabled);
            }
        }
    }
}