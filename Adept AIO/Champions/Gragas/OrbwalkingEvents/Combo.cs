using Adept_AIO.Champions.Gragas.Core;
using Adept_AIO.SDK.Unit_Extensions;
using Aimtec.SDK.Extensions;

namespace Adept_AIO.Champions.Gragas.OrbwalkingEvents
{
    class Combo
    {
        public static void OnUpdate()
        {
            var target = Global.TargetSelector.GetTarget(SpellManager.Q.Range);
            if (target == null || !target.IsValidTarget())
            {
                return;
            }

            if (SpellManager.Q.Ready && MenuConfig.Combo["Q"].Enabled)
            {
                SpellManager.CastQ(target);
            }

            if (SpellManager.W.Ready && MenuConfig.Combo["W"].Enabled)
            {
                SpellManager.CastW(target);
            }

            if (SpellManager.E.Ready && MenuConfig.Combo["E"].Enabled)
            {
                SpellManager.CastE(target);
            }

            if (!SpellManager.R.Ready || !MenuConfig.Combo["R"].Enabled || MenuConfig.Combo["Killable"].Enabled && Dmg.Damage(target) < target.Health && target.IsValidTarget(SpellManager.R.Range))
            {
                return;
            }
        
            SpellManager.CastR(target);
        }
    }
}