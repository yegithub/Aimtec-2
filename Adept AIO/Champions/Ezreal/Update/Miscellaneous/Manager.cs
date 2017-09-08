namespace Adept_AIO.Champions.Ezreal.Update.Miscellaneous
{
    using OrbwalkingEvents;
    using SDK.Junk;
    using Aimtec.SDK.Orbwalking;

    internal class Manager
    {
        public static void OnUpdate()
        {
            if (Global.Player.IsDead || Global.Orbwalker.IsWindingUp)
            {
                return;
            }
       
            Misc.OnUpdate();

            switch (Global.Orbwalker.Mode)
            {
                case OrbwalkingMode.Combo:
                    Combo.OnUpdate();
                    break;
                case OrbwalkingMode.Freeze:
                    Harass.OnUpdate();
                    break;
                case OrbwalkingMode.Laneclear:
                    LaneClear.OnUpdate();
                    JungleClear.OnUpdate();
                    break;
            }
        }
    }
}
