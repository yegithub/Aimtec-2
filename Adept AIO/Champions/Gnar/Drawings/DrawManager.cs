namespace Adept_AIO.Champions.Gnar.Drawings
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

        private static void OnPresent()
        {
            if (Global.Player.IsDead || !MenuConfig.Drawings["Dmg"].Enabled)
            {
                return;
            }

            foreach (var target in GameObjects.EnemyHeroes.Where(x => !x.IsDead && x.IsFloatingHealthBarActive && x.IsVisible))
            {
                var damage = Dmg.Damage(target);

                Global.DamageIndicator.Unit = target;
                Global.DamageIndicator.DrawDmg((float) damage, Color.FromArgb(153, 12, 177, 28));
            }
        }

        private static void OnRender()
        {
            if (Global.Player.IsDead)
            {
                return;
            }

            if (MenuConfig.Drawings["Q"].Enabled)
            {
                Render.Circle(Global.Player.Position, SpellManager.Q.Range, (uint) MenuConfig.Drawings["Segments"].Value, Color.Crimson);
            }

            if (MenuConfig.Drawings["Debug"].Enabled)
            {
                if (Render.WorldToScreen(Global.Player.Position, out var temp))
                {
                    Render.Text($"STATE: {SpellManager.GnarState}", new Vector2(temp.X - 55, temp.Y + 40), RenderTextFlags.Center, Color.Cyan);
                }
            }
        }
    }
}