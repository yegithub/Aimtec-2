using System;
using System.Drawing;
using Adept_AIO.Champions.Jax.Core;
using Aimtec;

namespace Adept_AIO.Champions.Jax.Drawings
{
    internal class DrawManager
    {
        public static void RenderManager()
        {
            if (ObjectManager.GetLocalPlayer().IsDead)
            {
                return;
            }

            if (MenuConfig.Drawings["E"].Enabled && SpellConfig.CounterStrikeTime > 0 && Environment.TickCount - SpellConfig.CounterStrikeTime < 2000)
            {
                Vector2 screen;
                Render.WorldToScreen(ObjectManager.GetLocalPlayer().Position, out screen);
                Render.Text(new Vector2(screen.X - 55, screen.Y + 40), Color.Cyan, "Time Until Q: " + (int)(Environment.TickCount - SpellConfig.CounterStrikeTime) + " / 2000");
            }

            if (MenuConfig.Drawings["Q"].Enabled && SpellConfig.Q.Ready)
            {
                Render.Circle(ObjectManager.GetLocalPlayer().Position, SpellConfig.Q.Range, (uint)MenuConfig.Drawings["Segments"].Value, Color.Cyan);
            }
        }
    }
}
