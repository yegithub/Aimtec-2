namespace Adept_AIO.Champions.Twitch.Drawings
{
    using System;
    using System.Drawing;
    using System.Linq;
    using Aimtec;
    using Aimtec.SDK.Extensions;
    using Core;
    using SDK.Geometry_Related;
    using SDK.Unit_Extensions;

    class DrawManager
    {
        public static void OnPresent()
        {
            if (Global.Player.IsDead)
            {
                return;
            }

            if (!Global.Player.HasBuff("TwitchHideInShadows") && MenuConfig.Drawings["Map"].Enabled && SpellManager.Q.Ready)
            {
                Geometry.DrawCircleOnMinimap(Global.Player.ServerPosition,
                                             new[] {10, 11, 12, 13, 14}[Global.Player.SpellBook.GetSpell(SpellSlot.Q).Level - 1] * Global.Player.MoveSpeed,
                                             Color.DeepPink,
                                             5);
            }

            if (!MenuConfig.Drawings["Dmg"].Enabled)
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
            if (!target.HasBuff("twitchdeadlyvenom"))
            {
                return;
            }

            var percent = (int) (dmg * 100 / target.Health);
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

            if (MenuConfig.Drawings["Debug"].Enabled && SpellManager.HasUltBuff())
            {
                var target = Global.Orbwalker.GetOrbwalkingTarget() as Obj_AI_Base;

                if (target != null && target.IsValidTarget() && target.IsHero)
                {
                    var col = Global.Orbwalker.IsWindingUp ? Color.LightCoral : Color.Crimson;
                    SpellManager.GetRectangle(target)?.Draw(col);
                    Render.Circle(target.ServerPosition, 60, 100, col);
                }
            }

            if (Global.Player.HasBuff("TwitchHideInShadows") && MenuConfig.Drawings["World"].Enabled)
            {
                var time = Global.Player.MoveSpeed * (Math.Max(0, Global.Player.GetBuff("TwitchHideInShadows").EndTime) - Game.ClockTime);
                Render.Circle(Global.Player.Position, time, (uint) MenuConfig.Drawings["Segments"].Value, Color.Cyan);
            }
        }
    }
}