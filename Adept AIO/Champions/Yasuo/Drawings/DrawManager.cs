using System.Drawing;
using System.Globalization;
using System.Linq;
using Adept_AIO.Champions.Yasuo.Core;
using Adept_AIO.SDK.Extensions;
using Aimtec;
using Aimtec.SDK.Orbwalking;

namespace Adept_AIO.Champions.Yasuo.Drawings
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

            if (MenuConfig.Drawings["Range"].Enabled && MenuConfig.Combo["Dash"].Value == 0 && Global.Orbwalker.Mode != OrbwalkingMode.None)
            {
                Render.Circle(Game.CursorPos, MenuConfig.Combo["Range"].Value,
                    (uint)MenuConfig.Drawings["Segments"].Value, Color.White);
            }

            if (MenuConfig.Drawings["Debug"].Enabled)
            {
                if (KnockUpHelper.Sender != null)
                {
                    Render.Text(Geometry.To2D(KnockUpHelper.Sender.ServerPosition), Color.Yellow, (-(Game.TickCount - (KnockUpHelper.BuffStart + KnockUpHelper.BuffEnd))).ToString(CultureInfo.InvariantCulture));
                }

                Render.WorldToScreen(Global.Player.Position, out var temp);
                Render.Text(new Vector2(temp.X - 55, temp.Y + 40), Color.White, "Q Mode: " + Extension.CurrentMode + "- Range: " + SpellConfig.Q.Range);
            }

            if (SpellConfig.E.Ready)
            {
                if (MinionHelper.ExtendedMinion.IsZero || MinionHelper.ExtendedTarget.IsZero)
                {
                    return;
                }

                Render.WorldToScreen(MinionHelper.ExtendedTarget, out var targetV2);
                Render.WorldToScreen(MinionHelper.ExtendedMinion, out var minionV2);
                Render.WorldToScreen(Global.Player.ServerPosition, out var playerV2);

                Render.Line(playerV2, minionV2, Color.DeepSkyBlue);
                Render.Line(minionV2, targetV2, Color.DeepPink);

                Render.Circle(MinionHelper.ExtendedMinion, 50, 300, Color.White);
            }

            if (SpellConfig.R.Ready)
            {
                if (MenuConfig.Drawings["R"].Enabled)
                {
                    Render.Circle(Global.Player.Position, SpellConfig.R.Range, (uint)MenuConfig.Drawings["Segments"].Value, Color.Cyan);
                }
            }
        }
    }
}
