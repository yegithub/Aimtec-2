namespace Adept_AIO.Champions.Zoe.OrbwalkingEvents
{
    using Core;
    using SDK.Unit_Extensions;

    class Harass
    {
        public static void OnUpdate()
        {
            var target = Global.TargetSelector.GetTarget(2000);
            if (target == null)
            {
                return;
            }

            if (SpellManager.Q.Ready &&
                MenuConfig.Harass["Q"].Enabled)
            {
                SpellManager.CastQ(target);
            }

            if (SpellManager.E.Ready &&
                MenuConfig.Harass["E"].Enabled)
            {
                SpellManager.CastE(target);
            }
        }
    }
}