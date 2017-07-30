using System.Drawing;
using Adept_AIO.Champions.LeeSin.Core.Spells;
using Adept_AIO.SDK.Extensions;
using Aimtec;

namespace Adept_AIO.Champions.LeeSin.Drawings
{
    internal interface IDrawManager
    {
        void RenderManager();
    }

    internal class DrawManager : IDrawManager
    {
        public bool QEnabled { get; set; }
        public bool PositionEnabled { get; set; }
        public int SegmentsValue { get; set; }

        private readonly ISpellConfig SpellConfig;

        public DrawManager(ISpellConfig spellConfig)
        {
            SpellConfig = spellConfig;
        }
           
        public void RenderManager()
        {
            if (GlobalExtension.Player.IsDead)
            {
                return;
            }

            if (QEnabled && SpellConfig.Q.Ready)
            {
                Render.Circle(GlobalExtension.Player.Position, SpellConfig.Q.Range, (uint)SegmentsValue, Color.IndianRed);
            }

            if (PositionEnabled && SpellConfig.InsecPosition != Vector3.Zero)
            {
                Render.Circle(SpellConfig.InsecPosition, 65, (uint)SegmentsValue, Color.White);
            }
        }
    }
}
