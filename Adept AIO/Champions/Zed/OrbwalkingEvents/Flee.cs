namespace Adept_AIO.Champions.Zed.OrbwalkingEvents
{
    using Aimtec;
    using Core;
    using SDK.Unit_Extensions;

    class Flee
    {
        public static void OnKeyPressed()
        {
            Global.Orbwalker.Move(Game.CursorPos);

            if (ShadowManager.CanCastW1())
            {
                SpellManager.W.Cast(Game.CursorPos);
            }
            else
            {
                SpellManager.W.Cast();
            }
        }
    }
}