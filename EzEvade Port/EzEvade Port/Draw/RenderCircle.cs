﻿namespace EzEvade_Port.Draw
{
    using System.Drawing;
    using Aimtec;
    using Aimtec.SDK.Extensions;
    using Utils;

    class RenderCircle : RenderObject
    {
        public Color Color = Color.White;

        public int Radius;
        public Vector2 RenderPosition;
        public Vector2 RenderPositionDx;
        public int Width;

        public RenderCircle(Vector2 renderPosition, float renderTime, int radius = 65, int width = 5)
        {
            StartTime = EvadeUtils.TickCount;
            EndTime = StartTime + renderTime;
            RenderPosition = renderPosition;

            Radius = radius;
            Width = width;
        }

        public RenderCircle(Vector2 renderPosition, float renderTime, Color color, int radius = 65, int width = 5)
        {
            StartTime = EvadeUtils.TickCount;
            EndTime = StartTime + renderTime;
            RenderPosition = renderPosition;

            Color = color;

            Radius = radius;
            Width = width;
        }

        public override void Draw()
        {
            if (RenderPosition.IsOnScreen())
            {
                Render.Circle(RenderPosition.To3D(), Radius, 50, Color);
            }
        }
    }
}