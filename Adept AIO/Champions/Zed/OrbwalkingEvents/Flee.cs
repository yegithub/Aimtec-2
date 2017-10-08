using Adept_AIO.Champions.Zed.Core;
using Adept_AIO.SDK.Unit_Extensions;
using Aimtec;

namespace Adept_AIO.Champions.Zed.OrbwalkingEvents
{
    internal class Flee
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
