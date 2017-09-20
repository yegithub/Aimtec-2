using System;
using System.Drawing;
using Adept_AIO.SDK.Unit_Extensions;
using Aimtec;
using Aimtec.SDK.Extensions;

namespace Adept_AIO.SDK.Draw_Extension
{
    internal class RenderHelper
    {
        public static void RenderArrowFromPlayer(GameObject target) 
        {
            if (target == null)
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

        public static void RenderArrowFromPlayer(Vector3 end)
        {
            if (end.IsZero)
            {
                return;
            }

            var extended = Global.Player.ServerPosition.Extend(end, end.Distance(Global.Player));
            Render.WorldToScreen(extended, out var extendedVector2);
            Render.WorldToScreen(Global.Player.Position, out var playerV2);

            var arrowLine1 = extendedVector2 + (playerV2 - extendedVector2).Normalized().Rotated(40 * (float)Math.PI / 180) * 65;
            var arrowLine2 = extendedVector2 + (playerV2 - extendedVector2).Normalized().Rotated(-40 * (float)Math.PI / 180) * 65;

            Render.Line(extendedVector2, arrowLine1, Color.White);
            Render.Line(extendedVector2, arrowLine2, Color.White);
            Render.Line(playerV2, extendedVector2, Color.Orange);
        }

        public static void RenderArrowFromPoint(Vector3 start, Vector3 end, int width = 1)
        {
            if (end.IsZero)
            {
                return;
            }

            var extended = start.Extend(end, end.Distance(start));
            Render.WorldToScreen(extended, out var extendedVector2);
            Render.WorldToScreen(start, out var startV2);

            var arrowLine1 = extendedVector2 + (startV2 - extendedVector2).Normalized().Rotated( 40 * (float)Math.PI / 180) * 65;
            var arrowLine2 = extendedVector2 + (startV2 - extendedVector2).Normalized().Rotated(-40 * (float)Math.PI / 180) * 65;

            Render.Line(extendedVector2, arrowLine1, width, false, Color.White);
            Render.Line(extendedVector2, arrowLine2, width, false, Color.White);
            Render.Line(startV2, extendedVector2, width, false, Color.Orange);
        }
    }
}
