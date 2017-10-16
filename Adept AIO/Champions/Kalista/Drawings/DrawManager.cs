namespace Adept_AIO.Champions.Kalista.Drawings
{
    using System;
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

            foreach (var target in GameObjects.EnemyHeroes.Where(x =>
                !x.IsDead && x.IsFloatingHealthBarActive && x.IsVisible))
            {
                var damage = Dmg.EDmg(target);

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

            if (MenuConfig.Drawings["Debug"].Enabled)
            {
                var target = Global.Orbwalker.GetOrbwalkingTarget() as Obj_AI_Base;

                if (target != null && target.IsValidTarget() && target.IsHero)
                {
                    SpellManager.GetRectangle(target)?.Draw(Color.LightCoral);
                }
            }
        }
    }
}