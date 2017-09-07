using System;
using System.Drawing;
using System.Linq;
using Adept_AIO.Champions.Riven.Core;
using Adept_AIO.SDK.Junk;
using Aimtec;
using Aimtec.SDK.Extensions;
using Aimtec.SDK.Orbwalking;

namespace Adept_AIO.Champions.Riven.Drawings
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

        public static void RenderBasics()
        {
            if (Global.Player.IsDead)
            {
                return;
            }

            if (MenuConfig.FleeMode.Active && !Extensions.FleePos.IsZero)
            {
                Mixed.RenderArrowFromPlayer(Extensions.FleePos);

                Render.Circle(Extensions.FleePos, 50, (uint)MenuConfig.Drawings["Segments"].Value, Color.White);
            }

            if (MenuConfig.Drawings["Mouse"].Enabled && Global.Orbwalker.Mode != OrbwalkingMode.None)
            {
                var temp = Global.Orbwalker.GetOrbwalkingTarget();
                if (temp != null && temp.IsHero && temp.Distance(Global.Player) > Global.Player.AttackRange - 100)
                {
                    var pos = Global.Player.ServerPosition.Extend(temp.ServerPosition, 450);
                    Render.Circle(pos, 200, (uint)MenuConfig.Drawings["Segments"].Value, Color.Yellow);
                    Render.WorldToScreen(pos, out var posV2);
                    Render.Text(new Vector2(posV2.X - 50, posV2.Y), Color.White, "Put Mouse Here");
                }
            }
         
            if (MenuConfig.Drawings["Harass"].Enabled && Global.Orbwalker.Mode == OrbwalkingMode.Mixed)
            {
                Mixed.RenderArrowFromPlayer(Global.TargetSelector.GetTarget(Extensions.EngageRange + 800));

                Render.WorldToScreen(Global.Player.Position, out var playerV2);
                Render.Text(new Vector2(playerV2.X - 65, playerV2.Y + 30), Color.Aqua, "PATTERN: " + Enums.Current);
            }

            if (Global.Orbwalker.Mode == OrbwalkingMode.Combo)
            {
                Mixed.RenderArrowFromPlayer(Global.TargetSelector.GetTarget(Extensions.EngageRange + 800));

                Render.WorldToScreen(Global.Player.Position, out var playerV2);             
                Render.Text(new Vector2(playerV2.X - 65, playerV2.Y + 30), Color.Aqua, "PATTERN: " + Enums.ComboPattern);
            }

            if (MenuConfig.BurstMode.Active)
            {
                Mixed.RenderArrowFromPlayer(Global.TargetSelector.GetSelectedTarget());

                Render.WorldToScreen(Global.Player.Position, out var playerV2);
                Render.Text(new Vector2(playerV2.X - 65, playerV2.Y + 30), Color.Aqua, "PATTERN: " + Enums.BurstPattern);
            }

            if (MenuConfig.Drawings["Engage"].Enabled)
            {
                if (Extensions.AllIn)
                {
                    Render.Circle(Global.Player.Position, Extensions.FlashRange(),
                        (uint)MenuConfig.Drawings["Segments"].Value, Color.Yellow);
                }
                else
                {
                    Render.Circle(Global.Player.Position, Extensions.EngageRange,
                        (uint)MenuConfig.Drawings["Segments"].Value, Color.White);
                }
            }

            if (MenuConfig.Drawings["R2"].Enabled && SpellConfig.R2.Ready && Enums.UltimateMode == UltimateMode.Second)
            {
                Render.Circle(Global.Player.Position, SpellConfig.R2.Range, (uint)MenuConfig.Drawings["Segments"].Value, Color.OrangeRed);
            }
        }

        private static void RenderArrow(GameObject target)
        {
            if (!MenuConfig.Drawings["Target"].Enabled || target == null)
            {
                return;
            }

            var extended = Global.Player.ServerPosition.Extend(target.ServerPosition, target.Distance(Global.Player));
            Render.WorldToScreen(extended, out var extendedVector2);
            Render.WorldToScreen(Global.Player.Position, out var playerV2);

            var arrowLine1 = extendedVector2 + (playerV2 - extendedVector2).Normalized().Rotated(40 * (float)Math.PI / 180) * 65;
            var arrowLine2 = extendedVector2 + (playerV2 - extendedVector2).Normalized().Rotated(-40 * (float)Math.PI / 180) * 65;

            Render.Line(extendedVector2, arrowLine1, Color.White);
            Render.Line(extendedVector2, arrowLine2, Color.White);
            Render.Line(playerV2, extendedVector2, Color.Orange);
        }
    }
}
