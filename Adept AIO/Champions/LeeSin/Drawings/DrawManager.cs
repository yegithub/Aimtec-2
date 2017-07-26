using System.Drawing;
using Adept_AIO.Champions.LeeSin.Core;
using Aimtec;

namespace Adept_AIO.Champions.LeeSin.Drawings
{
    class DrawManager
    {
        public static void RenderManager()
        {
            if (ObjectManager.GetLocalPlayer().IsDead)
            {
                return;
            }

            if (MenuConfig.Drawings["Q"].Enabled && SpellConfig.Q.Ready)
            {
                Render.Circle(ObjectManager.GetLocalPlayer().Position, SpellConfig.Q.Range, (uint)MenuConfig.Drawings["Segments"].Value, Color.IndianRed);
            }

            if (MenuConfig.Drawings["Position"].Enabled)
            {
                // Todo: fix.
            }
        }
    }
}
