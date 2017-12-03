namespace Adept_AIO.Champions.Yorick.Drawings
{
    using System.Drawing;
    using System.Linq;
    using Aimtec;
    using Core;
    using SDK.Unit_Extensions;

    class DrawManager
    {
        public DrawManager()
        {
            Render.OnPresent += OnPresent;
            Render.OnRender += OnRender;
        }

        public static void OnPresent()
        {
            if (Global.Player.IsDead || !MenuConfig.Drawings["Dmg"].Enabled)
            {
                return;
            }

            foreach (var target in GameObjects.EnemyHeroes.Where(x => x.IsVisible && !x.IsDead))
            {
                var damage = Dmg.Damage(target);

                Global.DamageIndicator.Unit = target;
                Global.DamageIndicator.DrawDmg((float) damage, Color.FromArgb(153, 12, 177, 28));
            }
        }

        public static void OnRender()
        {
            if (Global.Player.IsDead)
            {
                return;
            }

            if (MenuConfig.Drawings["Shove"].Enabled && Render.WorldToScreen(Global.Player.Position, out var playerScreen))
            {
                var status = MenuConfig.LaneClear["Shove"].Enabled;
                Render.Text($"Shove Status: {status}", playerScreen, RenderTextFlags.Center, status ? Color.LimeGreen : Color.Crimson);
            }
        }
    }
}