namespace Adept_AIO.Champions.Twitch.Drawings
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
                Global.DamageIndicator.DrawDmg((float) damage, Color.FromArgb(153, 12, 177, 28));
            }
        }

        public static void OnRender()
        {
            if (Global.Player.IsDead)
            {
                return;
            }

            if (MenuConfig.Drawings["Debug"].Enabled && SpellManager.HasUltBuff())
            {
                var target = Global.Orbwalker.GetOrbwalkingTarget() as Obj_AI_Base;

                if (target != null && target.IsValidTarget() && target.IsHero)
                {
                    var rRectangle = SpellManager.GetRectangle(target);
                    rRectangle?.Draw(Global.Orbwalker.IsWindingUp ? Color.LightCoral : Color.Crimson);
                    Render.Circle(target.ServerPosition, 60, 100, Color.Cyan);
                }
            }

            if (Global.Player.HasBuff("TwitchHideInShadows"))
            {
                if (MenuConfig.Drawings["World"].Enabled)
                {
                    var time = Global.Player.MoveSpeed *
                               (Math.Max(0, Global.Player.GetBuff("TwitchHideInShadows").EndTime) - Game.ClockTime);
                    Render.Circle(Global.Player.Position,
                        time,
                        (uint) MenuConfig.Drawings["Segments"].Value,
                        Color.Cyan);
                }
            }
        }
    }
}