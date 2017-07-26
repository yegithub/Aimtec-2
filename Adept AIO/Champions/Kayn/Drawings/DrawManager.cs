using System.Drawing;
using Adept_AIO.Champions.Kayn.Core;
using Aimtec;

namespace Adept_AIO.Champions.Kayn.Drawings
{
    class DrawManager
    {
        public static void RenderManager()
        {
            if (ObjectManager.GetLocalPlayer().IsDead)
            {
                return;
            }

            if (MenuConfig.Drawings["W"].Enabled && SpellConfig.W.Ready)
            {
                Render.Circle(ObjectManager.GetLocalPlayer().Position, SpellConfig.W.Range, (uint)MenuConfig.Drawings["Segments"].Value, Color.IndianRed);
            }

            if (MenuConfig.Drawings["R"].Enabled && SpellConfig.R.Ready)
            {
                Render.Circle(ObjectManager.GetLocalPlayer().Position, SpellConfig.R.Range, (uint)MenuConfig.Drawings["Segments"].Value, Color.IndianRed);
            }
        }
    }
}
