using System.Drawing;
using Adept_AIO.Champions.Yasuo.Core;
using Adept_AIO.SDK.Extensions;
using Aimtec;
using Aimtec.SDK.Orbwalking;

namespace Adept_AIO.Champions.Yasuo.Drawings
{
    internal class DrawManager
    {
        public static void RenderManager()
        {
            if (ObjectManager.GetLocalPlayer().IsDead)
            {
                return;
            }

            if (MenuConfig.Combo["Dash"].Value == 0 && GlobalExtension.Orbwalker.Mode != OrbwalkingMode.None)
            {
                Render.Circle(Game.CursorPos, MenuConfig.Combo["Range"].Value,
                    (uint)MenuConfig.Drawings["Segments"].Value, Color.White);
            }

            if (MenuConfig.Drawings["Debug"].Enabled)
            {
                Vector2 temp;
                Render.WorldToScreen(ObjectManager.GetLocalPlayer().Position, out temp);
                Render.Text(new Vector2(temp.X - 55, temp.Y + 40), Color.White, "Q Mode: " + Extension.CurrentMode + "- Range: " + SpellConfig.Q.Range);
            }

            if (SpellConfig.R.Ready)
            {
                if (MenuConfig.Drawings["R"].Enabled)
                {
                    Render.Circle(ObjectManager.GetLocalPlayer().Position, SpellConfig.R.Range, (uint)MenuConfig.Drawings["Segments"].Value, Color.Cyan);
                }

                //if (MenuConfig.Drawings["Timer"].Enabled && Extension.Airbourne > 0)
                //{
                //    Vector2 screen;
                //    Render.WorldToScreen(ObjectManager.GetLocalPlayer().Position, out screen);
                //    var time = Extension.GetAirbourneTime(GameObjects.EnemyHeroes.FirstOrDefault(x => x.HasBuffOfType(BuffType.Knockup) || x.HasBuffOfType(BuffType.Knockback)));
                //    Render.Text(new Vector2(screen.X - 55, screen.Y + 40), Color.White, "Airbourne: " + (int)(Environment.TickCount - Extension.Airbourne) + " / " + time);
                //}
            }
        }
    }
}
