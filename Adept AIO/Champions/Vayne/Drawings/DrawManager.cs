using System.Drawing;
using System.Linq;
using Adept_AIO.Champions.Vayne.Core;
using Adept_AIO.SDK.Unit_Extensions;
using Adept_AIO.SDK.Usables;
using Aimtec;
using Aimtec.SDK.Extensions;

namespace Adept_AIO.Champions.Vayne.Drawings
{
    internal class DrawManager
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

            if (SpellManager.E.Ready && MenuConfig.Drawings["Pred"].Enabled)
            {
                var target = Global.Orbwalker.GetOrbwalkingTarget() as Obj_AI_Base;
                if (target != null && target.IsValidTarget() && target.IsHero && SpellManager.Rect(target) != null)
                {
                    SpellManager.Rect(target).Draw(Color.Crimson);
                }

                if (!SpellManager.DrawingPred.IsZero)
                {
                    if (Render.WorldToScreen(SpellManager.DrawingPred, out var screen) &&
                        Render.WorldToScreen(Global.Player.ServerPosition, out var pos))
                    {
                        Render.Line(pos, screen, Color.Yellow);
                    }
                   
                    Render.Circle(SpellManager.DrawingPred, 45, 100, Color.Yellow);
                    Render.Circle(SpellManager.DrawingPred, 65, 100, Color.Crimson);
                    Render.Circle(Global.Player.ServerPosition, 425, 100, Color.Orange);
                }
            }

            if (SpellManager.Q.Ready && MenuConfig.Drawings["Q"].Enabled)
            {
                Render.Circle(Global.Player.Position, SpellManager.Q.Range,
                    (uint) MenuConfig.Drawings["Segments"].Value, Color.Crimson);
            }
        }
    }
}