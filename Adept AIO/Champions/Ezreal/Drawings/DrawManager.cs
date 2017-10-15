namespace Adept_AIO.Champions.Ezreal.Drawings
{
    using System.Drawing;
    using System.Linq;
    using Aimtec;
    using Core;
    using SDK.Unit_Extensions;

    class DrawManager
    {
        public static void OnPresent()
        {
            if (Global.Player.IsDead || !MenuConfig.Drawings["Dmg"].Enabled)
            {
                return;
            }

            foreach (var target in GameObjects.EnemyHeroes.Where(x =>
                !x.IsDead && x.IsFloatingHealthBarActive && x.IsVisible))
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

            if (MenuConfig.Drawings["Q"].Enabled)
            {
                Render.Circle(Global.Player.Position,
                    SpellConfig.Q.Range,
                    (uint) MenuConfig.Drawings["Segments"].Value,
                    Color.Yellow);
            }

            if (MenuConfig.Drawings["R"].Enabled)
            {
                Render.Circle(Global.Player.Position,
                    MenuConfig.Killsteal["Range"].Value,
                    (uint) MenuConfig.Drawings["Segments"].Value,
                    Color.CadetBlue);
            }
        }
    }
}