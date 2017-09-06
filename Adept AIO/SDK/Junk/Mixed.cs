using System;
using System.Drawing;
using Aimtec;
using Aimtec.SDK.Extensions;

namespace Adept_AIO.SDK.Junk
{
    internal class Mixed
    {
        public static Vector3 GetFountainPos(GameObject target)
        {
            switch (Game.MapId)
            {
                case GameMapId.SummonersRift:
                    return target.Team == GameObjectTeam.Order
                        ? new Vector3(396, 185.1325f, 462)
                        : new Vector3(14340, 171.9777f, 14390);

                case GameMapId.TwistedTreeline:
                    return target.Team == GameObjectTeam.Order
                        ? new Vector3(1058, 150.8638f, 7297)
                        : new Vector3(14320, 151.9291f, 7235);
            }
            return Vector3.Zero;
        }

        public static int PercentDmg(Obj_AI_Base target, double dmg)
        {
            return (int)(dmg / target.Health * 100);
        }

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

        public static void RenderArrowFromPoint(Vector3 start, Vector3 end)
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

            Render.Line(extendedVector2, arrowLine1, Color.White);
            Render.Line(extendedVector2, arrowLine2, Color.White);
            Render.Line(startV2, extendedVector2, Color.Orange);
        }
    }
}
