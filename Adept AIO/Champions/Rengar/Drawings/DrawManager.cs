using System.Drawing;
using Adept_AIO.Champions.Rengar.Core;
using Aimtec;
using Aimtec.SDK.Util;

namespace Adept_AIO.Champions.Rengar.Drawings
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
                Render.Circle(ObjectManager.GetLocalPlayer().Position, SpellConfig.Q.Range, (uint)MenuConfig.Drawings["Segments"].Value, Color.Cyan);
            }

            if (MenuConfig.Drawings["W"].Enabled && SpellConfig.W.Ready)
            {
                Render.Circle(ObjectManager.GetLocalPlayer().Position, SpellConfig.W.Range, (uint)MenuConfig.Drawings["Segments"].Value, Color.Cyan);
            }

            if (MenuConfig.Drawings["E"].Enabled && SpellConfig.E.Ready)
            {
                Render.Circle(ObjectManager.GetLocalPlayer().Position, SpellConfig.E.Range, (uint)MenuConfig.Drawings["Segments"].Value, Color.Cyan);
            }

            if (Extensions.AssassinTarget != null)
            {
                Vector2 screen;
                Render.WorldToScreen(ObjectManager.GetLocalPlayer().Position, out screen);
                Render.Text(new Vector2(screen.X - 55, screen.Y + 40), Color.White, "Target: " + Extensions.AssassinTarget.ChampionName);
                DelayAction.Queue(2500, () => Extensions.AssassinTarget = null);
            }
        }
    }
}
