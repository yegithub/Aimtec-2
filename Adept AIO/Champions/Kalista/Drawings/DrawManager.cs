namespace Adept_AIO.Champions.Kalista.Drawings
{
    using System.Drawing;
    using System.Linq;
    using Aimtec;
    using Aimtec.SDK.Extensions;
    using Core;
    using SDK.Generic;
    using SDK.Unit_Extensions;

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
                var damage = Dmg.EDmg(target);

                Global.DamageIndicator.Unit = target;
                Global.DamageIndicator.DrawDmg((float) damage, Color.FromArgb(153, 12, 177, 28));
                RenderEDamage(target, damage);
            }

            foreach (var mob in GameObjects.Jungle.Where(x => !x.IsDead && x.IsVisible && x.GetJungleType() != GameObjects.JungleType.Small))
            {
                RenderEDamage(mob, Dmg.EDmg(mob));
            }
        }

        private static void RenderEDamage(Obj_AI_Base target, double dmg)
        {
            if (!target.HasBuff("kalistaexpungemarker"))
            {
                return;
            }
        
            var percent = (int)(dmg * 100 / target.Health);
            var pos = target.FloatingHealthBarPosition;
            var offset = new Vector2(75, 37);

            Render.Text($"E DMG: {percent}%", new Vector2(pos.X + offset.X, pos.Y + offset.Y), RenderTextFlags.Center, Color.White);
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