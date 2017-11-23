namespace Adept_AIO.Champions.Zoe.Drawings
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
            if (Global.Player.IsDead ||
                !MenuConfig.Drawings["Dmg"].Enabled)
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
            if (Global.Player.IsDead || !SpellManager.Q.Ready)
            {
                return;
            }

            if (MenuConfig.Drawings["Q"].Enabled)
            {
                Render.Circle(Global.Player.Position, SpellManager.Q.Range, (uint)MenuConfig.Drawings["Segments"].Value, Color.Cyan);
            }

            if (!MenuConfig.Drawings["Pred"].Enabled)
            {
                return;
            }

            var target = Global.TargetSelector.GetTarget(2500);
            if (target == null)
            {
                return;
            }

            var generated = SpellManager.GeneratePaddleStarPrediction(target, SpellManager.Q);
            if (generated.IsZero)
            {
                return;
            }

            Render.Circle(generated, 50, 100, Color.BlueViolet);

            if (!Render.WorldToScreen(generated, out var generatedV2) ||
                !Render.WorldToScreen(target.ServerPosition, out var targetV2) ||
                !Render.WorldToScreen(Global.Player.ServerPosition, out var playerV2))
            {
                return;
            }

            Render.Line(playerV2, generatedV2, 4, false, Color.Aqua);
            Render.Line(generatedV2, targetV2, 4, false, Color.Crimson);
        }
    }
}