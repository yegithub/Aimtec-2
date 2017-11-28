namespace Adept_AIO.Champions.Draven.Drawings
{
    using System.Drawing;
    using System.Linq;
    using Aimtec;
    using Aimtec.SDK.Extensions;
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

            if (MenuConfig.Drawings["Catch"].Enabled)
            {
                Render.Circle(Global.Player.Position, MenuConfig.Misc["Range"].Value, 100, Color.Violet);
            }
          
            if (!MenuConfig.Drawings["Axe"].Enabled)
            {
                return;
            }

            foreach (var i in SpellManager.AxeList.Keys)
            {
                Render.Circle(i.ServerPosition, 120, 100, i.ServerPosition.Distance(Game.CursorPos) <= MenuConfig.Misc["Range"].Value ? Color.LimeGreen : Color.Crimson);
            }
        }
    }
}