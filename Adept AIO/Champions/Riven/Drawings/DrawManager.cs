using System.Drawing;
using System.Linq;
using Adept_AIO.Champions.Riven.Core;
using Adept_AIO.SDK.Draw_Extension;
using Adept_AIO.SDK.Geometry_Related;
using Adept_AIO.SDK.Unit_Extensions;
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
                Render.Circle(Extensions.FleePos, 50, (uint)MenuConfig.Drawings["Segments"].Value, Color.White);

                if (!WallExtension.EndPoint.IsZero)
                {
                    Render.WorldToScreen(Extensions.FleePos, out var startPointVector2);
                    Render.WorldToScreen(WallExtension.EndPoint, out var endPointVector2);
                    Render.Line(startPointVector2, endPointVector2, Color.Orange);
                    Render.Circle(WallExtension.EndPoint, 50, (uint)MenuConfig.Drawings["Segments"].Value, Color.White);
                }
            }

            if (MenuConfig.Drawings["Mouse"].Enabled && Global.Orbwalker.Mode != OrbwalkingMode.None)
            {
                var temp = Global.Orbwalker.GetOrbwalkingTarget();
                if (temp != null && temp.IsHero && temp.Distance(Global.Player) > Global.Player.AttackRange - 100)
                {
                    var pos = Global.Player.ServerPosition.Extend(temp.ServerPosition, 450);
                    Render.Circle(pos, 200, (uint)MenuConfig.Drawings["Segments"].Value, Color.Yellow);
                    Render.WorldToScreen(pos, out var posV2);
                    Render.Text("Put Mouse Here", new Vector2(posV2.X - 50, posV2.Y), RenderTextFlags.Center, Color.White);
                }
            }
         
            if (MenuConfig.Drawings["Pattern"].Enabled)
            {
                Render.WorldToScreen(Global.Player.Position, out var playerV2);

                if (MenuConfig.BurstMode.Active)
                {
                    RenderHelper.RenderArrowFromPlayer(Global.TargetSelector.GetSelectedTarget());
                    Render.Text("PATTERN: " + Enums.BurstPattern, new Vector2(playerV2.X - 65, playerV2.Y + 30), RenderTextFlags.Center, Color.Aqua);
                }

                switch (Global.Orbwalker.Mode)
                {
                    case OrbwalkingMode.Combo:
                        RenderHelper.RenderArrowFromPlayer(Global.TargetSelector.GetTarget(Extensions.EngageRange + 800));
                        Render.Text("PATTERN: " + Enums.ComboPattern, new Vector2(playerV2.X - 65, playerV2.Y + 30), RenderTextFlags.Center, Color.Aqua);
                        break;

                    case OrbwalkingMode.Mixed:
                        RenderHelper.RenderArrowFromPlayer(Global.TargetSelector.GetTarget(Extensions.EngageRange + 800));
                        Render.Text("PATTERN: " + Enums.Current, new Vector2(playerV2.X - 65, playerV2.Y + 30), RenderTextFlags.Center, Color.Aqua);
                        break;
                }
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
    }
}
