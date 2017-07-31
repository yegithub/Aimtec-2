using System.Drawing;
using Adept_AIO.Champions.Riven.Core;
using Adept_AIO.SDK.Extensions;
using Aimtec;
using Aimtec.SDK.Orbwalking;

namespace Adept_AIO.Champions.Riven.Drawings
{
    internal class DrawManager
    {
        public static void RenderManager()
        {
            if (GlobalExtension.Player.IsDead)
            {
                return;
            }

            if (MenuConfig.Drawings["Harass"].Enabled && GlobalExtension.Orbwalker.Mode == OrbwalkingMode.Mixed)
            {
                Vector2 screenPos;
                Render.WorldToScreen(GlobalExtension.Player.Position, out screenPos);
                Render.Text(new Vector2(screenPos.X - 65, screenPos.Y + 30), Color.Aqua, "PATTERN: " + Extensions.Current.ToString().ToUpper());
            }

            if (MenuConfig.Drawings["Engage"].Enabled)
            {
                if (Extensions.AllIn)
                {
                    Render.Circle(GlobalExtension.Player.Position, Extensions.FlashRange(),
                        (uint)MenuConfig.Drawings["Segments"].Value, Color.Yellow);
                }
                else
                {
                    Render.Circle(GlobalExtension.Player.Position, Extensions.EngageRange(),
                        (uint)MenuConfig.Drawings["Segments"].Value, Color.White);
                }
            }

            if (MenuConfig.Drawings["R2"].Enabled && SpellConfig.R2.Ready && Extensions.UltimateMode == UltimateMode.Second)
            {
                Render.Circle(GlobalExtension.Player.Position, SpellConfig.R2.Range, (uint)MenuConfig.Drawings["Segments"].Value, Color.OrangeRed);
            }
        }
    }
}
