using System.Drawing;
using Adept_AIO.Champions.Irelia.Core;
using Adept_AIO.SDK.Extensions;
using Aimtec;
using Aimtec.SDK.Orbwalking;

namespace Adept_AIO.Champions.Irelia.Drawings
{
    internal class DrawManager
    {
        public static void RenderManager()
        {
            if (GlobalExtension.Player.IsDead)
            {
                return;
            }

            if (MenuConfig.Drawings["Engage"].Enabled && GlobalExtension.Orbwalker.Mode != OrbwalkingMode.None)
            {
                // Could turn into ? : statement as well.
                switch (MenuConfig.Combo["Mode"].Value)
                {
                    case 1:
                        Render.Circle(GlobalExtension.Player.Position, MenuConfig.Combo["Range"].Value, (uint)MenuConfig.Drawings["Segments"].Value, Color.White);
                        break;
                    case 0:
                        Render.Circle(Game.CursorPos, MenuConfig.Combo["Range"].Value, (uint)MenuConfig.Drawings["Segments"].Value, Color.White);
                        break;
                }
            }

            if (MenuConfig.Drawings["Q"].Enabled && SpellConfig.Q.Ready)
            {
                Render.Circle(GlobalExtension.Player.Position, SpellConfig.Q.Range, (uint)MenuConfig.Drawings["Segments"].Value, Color.Aqua);
            }

            if (MenuConfig.Drawings["R"].Enabled && SpellConfig.R.Ready)
            {
                Render.Circle(GlobalExtension.Player.Position, SpellConfig.R.Range, (uint)MenuConfig.Drawings["Segments"].Value, Color.IndianRed);
            }
        }
    }
}
