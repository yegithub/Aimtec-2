namespace Adept_AIO.Champions.Gnar.OrbwalkingEvents
{
    using Aimtec.SDK.Extensions;
    using Core;
    using SDK.Unit_Extensions;

    class Combo
    {
        public static void OnUpdate()
        {
            var target = Global.TargetSelector.GetTarget(SpellManager.Q.Range);
            if (target == null)
            {
                return;
            }

            if (SpellManager.E.Ready && MenuConfig.Combo["E"].Enabled && Dmg.Damage(target) * 1.2f > target.Health &&
                !Global.Player.ServerPosition.Extend(target.ServerPosition, SpellManager.E.Range * 2).PointUnderEnemyTurret())
            {
                SpellManager.CastE(target);
            }

            if (SpellManager.Q.Ready && MenuConfig.Combo["Q"].Enabled)
            {
                SpellManager.CastQ(target);
            }

            if (SpellManager.R.Ready && MenuConfig.Combo["R"].Enabled && Dmg.Damage(target) * 1.2f > target.Health)
            {
                SpellManager.CastR(target);
            }

            if (SpellManager.W.Ready && MenuConfig.Combo["W"].Enabled)
            {
                SpellManager.CastW(target);
            }
        }
    }
}