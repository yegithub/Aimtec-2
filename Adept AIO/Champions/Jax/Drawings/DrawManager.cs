using System.Drawing;
using Adept_AIO.Champions.Jax.Core;
using Adept_AIO.SDK.Extensions;
using Aimtec;

namespace Adept_AIO.Champions.Jax.Drawings
{
    internal class DrawManager
    {
        public static void RenderManager()
        {
            if (GlobalExtension.Player.IsDead)
            {
                return;
            }

            if (MenuConfig.Drawings["E"].Enabled && SpellConfig.E.LastCastAttemptT > 0 && Game.TickCount - SpellConfig.E.LastCastAttemptT < 2000)
            {
                Vector2 screen;
                Render.WorldToScreen(GlobalExtension.Player.Position, out screen);
                Render.Text(new Vector2(screen.X - 55, screen.Y + 40), Color.Cyan, "Time Until Q: " + (int)(Game.TickCount - SpellConfig.E.LastCastAttemptT) + " / 2000");
            }

            if (MenuConfig.Drawings["Q"].Enabled && SpellConfig.Q.Ready)
            {
                Render.Circle(GlobalExtension.Player.Position, SpellConfig.Q.Range, (uint)MenuConfig.Drawings["Segments"].Value, Color.Cyan);
            }
        }
    }
}
