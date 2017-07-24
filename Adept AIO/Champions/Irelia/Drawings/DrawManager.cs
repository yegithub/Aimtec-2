using System.Drawing;
using Adept_AIO.Champions.Irelia.Core;
using Aimtec;
using Aimtec.SDK.Orbwalking;

namespace Adept_AIO.Champions.Irelia.Drawings
{
    internal class DrawManager
    {
        public static void RenderManager()
        {
            if (ObjectManager.GetLocalPlayer().IsDead)
            {
                return;
            }

            if (MenuConfig.Drawings["Engage"].Enabled && Orbwalker.Implementation.Mode != OrbwalkingMode.None)
            {
                // Could turn into ? : statement as well.
                switch (MenuConfig.Combo["Mode"].Value)
                {
                    case 1:
                        Render.Circle(ObjectManager.GetLocalPlayer().Position, MenuConfig.Combo["Range"].Value, (uint)MenuConfig.Drawings["Segments"].Value, Color.White);
                        break;
                    case 0:
                        Render.Circle(Game.CursorPos, MenuConfig.Combo["Range"].Value, (uint)MenuConfig.Drawings["Segments"].Value, Color.White);
                        break;
                }
            }

            if (MenuConfig.Drawings["Q"].Enabled && SpellConfig.Q.Ready)
            {
                Render.Circle(ObjectManager.GetLocalPlayer().Position, SpellConfig.Q.Range, (uint)MenuConfig.Drawings["Segments"].Value, Color.Aqua);
            }

            if (MenuConfig.Drawings["R"].Enabled && SpellConfig.R.Ready)
            {
                Render.Circle(ObjectManager.GetLocalPlayer().Position, SpellConfig.R.Range, (uint)MenuConfig.Drawings["Segments"].Value, Color.IndianRed);
            }
        }
    }
}
