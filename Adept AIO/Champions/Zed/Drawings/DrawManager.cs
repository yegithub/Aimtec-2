using System.Drawing;
using System.Linq;
using Adept_AIO.Champions.Zed.Core;
using Adept_AIO.SDK.Geometry_Related;
using Adept_AIO.SDK.Unit_Extensions;
using Aimtec;
using Aimtec.SDK.Extensions;
using Aimtec.SDK.Orbwalking;

namespace Adept_AIO.Champions.Zed.Drawings
{
    internal class DrawManager
    {
        public static void OnPresent()
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

        public static void OnRender()
        {
            if (Global.Player.IsDead)
            {
                return;
            }

            if (MenuConfig.Drawings["Pred"].Enabled)
            {
                foreach (var shadow in ShadowManager.Shadows.Where(ShadowManager.IsShadow))
                {
                    var enemy = GameObjects.Enemy.FirstOrDefault(x => x.Distance(shadow) <= SpellManager.Q.Range);
                    if (enemy == null)
                    {
                        continue;
                    }

                    var pred = SpellManager.Q.GetPrediction(enemy, shadow.ServerPosition, shadow.ServerPosition);
                    var rect = new Geometry.Rectangle(shadow.ServerPosition.To2D(), shadow.ServerPosition.Extend(pred.CastPosition.To2D(), SpellManager.Q.Range).To2D(), SpellManager.Q.Width);
                    rect.Draw(Color.Crimson);
                }
            }

            if (MenuConfig.Drawings["Range"].Enabled)
            {
                Render.Circle(Global.Player.Position, SpellManager.WCastRange + (Global.Orbwalker.Mode == OrbwalkingMode.Combo ? SpellManager.R.Range : Global.Player.AttackRange), (uint)MenuConfig.Drawings["Segments"].Value, Color.Crimson);
            }

            if (SpellManager.Q.Ready && MenuConfig.Drawings["Q"].Enabled)
            {
                Render.Circle(Global.Player.Position, SpellManager.Q.Range, (uint)MenuConfig.Drawings["Segments"].Value, Color.Cyan);
            } 
        }
    }
}
