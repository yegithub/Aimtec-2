namespace Adept_AIO.Champions.Jhin.Drawings
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
                Global.DamageIndicator.DrawDmg((float)damage, Color.FromArgb(153, 12, 177, 28));
            }
        }

        private static void OnRender()
        {
            if (Global.Player.IsDead)
            {
                return;
            }

            if (SpellManager.R.Ready && MenuConfig.Drawings["R"].Enabled)
            {
                Render.Circle(Global.Player.Position, SpellManager.R.Range, (uint)MenuConfig.Drawings["Segments"].Value, Color.Crimson);
            }
        }
    }
}