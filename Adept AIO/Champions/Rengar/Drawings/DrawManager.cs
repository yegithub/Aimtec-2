using System.Drawing;
using Adept_AIO.Champions.Rengar.Core;
using Adept_AIO.SDK.Extensions;
using Aimtec;
using Aimtec.SDK.Util;

namespace Adept_AIO.Champions.Rengar.Drawings
{
    internal class DrawManager
    {
        public static void RenderManager()
        {
            if (GlobalExtension.Player.IsDead)
            {
                return;
            }

            if (MenuConfig.Drawings["Q"].Enabled && SpellConfig.Q.Ready)
            {
                Render.Circle(GlobalExtension.Player.Position, SpellConfig.Q.Range, (uint)MenuConfig.Drawings["Segments"].Value, Color.Cyan);
            }

            if (MenuConfig.Drawings["W"].Enabled && SpellConfig.W.Ready)
            {
                Render.Circle(GlobalExtension.Player.Position, SpellConfig.W.Range, (uint)MenuConfig.Drawings["Segments"].Value, Color.Cyan);
            }

            if (MenuConfig.Drawings["E"].Enabled && SpellConfig.E.Ready)
            {
                Render.Circle(GlobalExtension.Player.Position, SpellConfig.E.Range, (uint)MenuConfig.Drawings["Segments"].Value, Color.Cyan);
            }

            if (Extensions.AssassinTarget != null)
            {
                Vector2 screen;
                Render.WorldToScreen(GlobalExtension.Player.Position, out screen);
                Render.Text(new Vector2(screen.X - 55, screen.Y + 40), Color.White, "Target: " + Extensions.AssassinTarget.ChampionName);
                DelayAction.Queue(2500, () => Extensions.AssassinTarget = null);
            }
        }
    }
}
