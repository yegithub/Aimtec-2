namespace Adept_AIO.Champions.Gragas.OrbwalkingEvents
{
    using Core;
    using SDK.Unit_Extensions;
    using Aimtec.SDK.Events;
    using Aimtec.SDK.Extensions;

    class Insec
    {
        public static void OnKeyPressed()
        {
            var target = Global.TargetSelector.GetSelectedTarget();
            if (target == null || !target.IsValidTarget(1500))
            {
                return;
            }

            if (SpellManager.R.Ready && !(MenuConfig.Combo["Q"].Enabled && SpellManager.Q.Ready))
            {
                SpellManager.CastR(target);
            }

            if (SpellManager.Q.Ready)
            {
                SpellManager.CastQ(target, MenuConfig.Combo["Q"].Enabled);
            }

            else if (SpellManager.E.Ready && !target.IsDashing())
            {
                SpellManager.CastE(target);
            }

            else if (SpellManager.W.Ready)
            {
                SpellManager.CastW(target);
            }
        }
    }
}
