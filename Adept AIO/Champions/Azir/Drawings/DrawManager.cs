namespace Adept_AIO.Champions.Azir.Drawings
{
    using System.Drawing;
    using System.Linq;
    using Aimtec;
    using Core;
    using OrbwalkingEvents;
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
                    SpellConfig.R.Range,
                    (uint) MenuConfig.Drawings["Segments"].Value,
                    Color.Red);
            }

            if (AzirHelper.InsecMode.Active)
            {
                Render.Circle(Global.Player.ServerPosition,
                    Insec.InsecRange(),
                    (uint) MenuConfig.Drawings["Segments"].Value,
                    Color.LightGray);
            }

            if (AzirHelper.Rect != null)
            {
                AzirHelper.Rect.Draw(Color.SlateGray);
            }

            if (!SoldierManager.Soldiers.Any() || !MenuConfig.Drawings["Soldiers"].Enabled)
            {
                return;
            }

            foreach (var soldier in SoldierManager.Soldiers)
            {
                Render.Circle(soldier.ServerPosition,
                    325,
                    (uint) MenuConfig.Drawings["Segments"].Value,
                    Color.SlateBlue);
            }
        }
    }
}