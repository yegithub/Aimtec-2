using System.Drawing;
using System.Linq;
using Adept_AIO.Champions.Vayne.Core;
using Adept_AIO.SDK.Unit_Extensions;
using Aimtec;
using Aimtec.SDK.Extensions;

namespace Adept_AIO.Champions.Vayne.Drawings
{
    class DrawManager
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

            if (SpellManager.E.Ready && MenuConfig.Drawings["Pred"].Enabled)
            {
                var target = Global.Orbwalker.GetOrbwalkingTarget() as Obj_AI_Base;
                if (target != null && target.IsValidTarget() && target.IsHero && SpellManager.Rect(target) != null)
                {
                    SpellManager.Rect(target).Draw(Color.Crimson);
                }
            }

            if (SpellManager.Q.Ready && MenuConfig.Drawings["Q"].Enabled)
            {
                Render.Circle(Global.Player.Position, SpellManager.Q.Range, (uint)MenuConfig.Drawings["Segments"].Value, Color.Crimson);
            }
        }
    }
}
