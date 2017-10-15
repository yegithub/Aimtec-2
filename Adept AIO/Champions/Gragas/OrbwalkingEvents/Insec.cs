namespace Adept_AIO.Champions.Gragas.OrbwalkingEvents
{
    using Core;
    using SDK.Unit_Extensions;
    using Aimtec.SDK.Events;

    class Insec
    {
        public static void OnKeyPressed()
        {
            var target = Global.TargetSelector.GetSelectedTarget();
            if (target == null)
            {
                return;
            }

            if (SpellManager.E.Ready && !target.IsDashing())
            {
                SpellManager.CastE(target, MenuConfig.InsecMenu["Flash"].Enabled);
            }

            if (SpellManager.R.Ready && !(MenuConfig.Combo["Q"].Enabled && SpellManager.Q.Ready && SpellManager.Barrel == null))
            {
                SpellManager.CastR(target);
            }

            if (SpellManager.Q.Ready)
            {
                SpellManager.CastQ(target, MenuConfig.Combo["Q"].Enabled);
            }

            else if (SpellManager.W.Ready)
            {
                SpellManager.CastW(target);
            }
        }
    }
}
