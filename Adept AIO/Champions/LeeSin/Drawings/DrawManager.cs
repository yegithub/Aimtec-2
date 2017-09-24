using System;
using System.Drawing;
using System.Linq;
using Adept_AIO.Champions.LeeSin.Core;
using Adept_AIO.Champions.LeeSin.Core.Damage;
using Adept_AIO.Champions.LeeSin.Core.Insec_Manager;
using Adept_AIO.Champions.LeeSin.Core.Spells;
using Adept_AIO.SDK.Unit_Extensions;
using Aimtec;
using Aimtec.SDK.Extensions;


namespace Adept_AIO.Champions.LeeSin.Drawings
{
    internal class DrawManager : IDrawManager
    {
        public bool QEnabled { get; set; }
        public bool PositionEnabled { get; set; }
        public int SegmentsValue { get; set; }

        private readonly ISpellConfig _spellConfig;
        private readonly IInsecManager _insecManager;
        private readonly IDmg _damage;

        public DrawManager(ISpellConfig spellConfig, IInsecManager insecManager, IDmg damage)
        {
            _spellConfig = spellConfig;
            _insecManager = insecManager;
            _damage = damage;
        }

        public void RenerDamage()
        {
            if (Global.Player.IsDead)
            {
                return;
            }

            foreach (var target in GameObjects.EnemyHeroes.Where(x =>  x.IsVisible))
            {
                var damage = _damage.Damage(target);

                Global.DamageIndicator.Unit = target;
                Global.DamageIndicator.DrawDmg((float)damage, Color.FromArgb(153, 12, 177, 28));
            }
        }

        public void OnRender()
        {
            if (Global.Player.IsDead)
            {
                return;
            }
            
            if (QEnabled && _spellConfig.Q.Ready)
            {
                Render.Circle(Global.Player.Position, _spellConfig.Q.Range, (uint)SegmentsValue, Color.IndianRed);
            }

            Render.WorldToScreen(Global.Player.ServerPosition, out var bkToggleV2);
            Render.Text("Bubba Kush: " + Temp.IsBubbaKush, new Vector2(bkToggleV2.X - 40, bkToggleV2.Y + 70), RenderTextFlags.Center, Temp.IsBubbaKush ? Color.White : Color.LightSlateGray);

            var target = Global.TargetSelector.GetSelectedTarget();

            if (!PositionEnabled || target == null)
            {
                return;
            }

            if (Temp.IsBubbaKush && _insecManager.BkPosition(target) != Vector3.Zero)
            {
                var bkPos = _insecManager.BkPosition(target);
                Render.WorldToScreen(bkPos, out var bkScreen);
                Render.Text("BK", bkScreen, RenderTextFlags.Center, Color.Orange);

                var bkEndPos = target.ServerPosition;
                Render.WorldToScreen(bkEndPos, out var bkEndPosV2);
                Render.WorldToScreen(bkPos, out var bkPosV2);

                var arrowLine1 = bkEndPosV2 + (bkPosV2 - bkEndPosV2).Normalized().Rotated( 40 * (float)Math.PI / 180) * target.BoundingRadius;
                var arrowLine2 = bkEndPosV2 + (bkPosV2 - bkEndPosV2).Normalized().Rotated(-40 * (float)Math.PI / 180) * target.BoundingRadius;

                Render.Line(bkEndPosV2, arrowLine1, Color.White);
                Render.Line(bkEndPosV2, arrowLine2, Color.White);
                Render.Line(bkPosV2, bkEndPosV2, Color.Orange);

                Render.Circle(bkPos, 65, (uint)SegmentsValue, Color.Orange);
            }
            else if(!_insecManager.InsecPosition(target).IsZero)
            {
                var insecPos = _insecManager.InsecPosition(target);
                var targetEndPos = target.ServerPosition + (target.ServerPosition - insecPos).Normalized() * 900;

                Render.WorldToScreen(targetEndPos, out var endPosV2);
                Render.WorldToScreen(insecPos, out var startPosV2);

                var arrowLine1 = endPosV2 + (startPosV2 - endPosV2).Normalized().Rotated( 40 * (float)Math.PI / 180) * target.BoundingRadius;
                var arrowLine2 = endPosV2 + (startPosV2 - endPosV2).Normalized().Rotated(-40 * (float)Math.PI / 180) * target.BoundingRadius;

                Render.Line(endPosV2, arrowLine1, Color.White);
                Render.Line(endPosV2, arrowLine2, Color.White);
                Render.Line(startPosV2, endPosV2, Color.Orange);

                Render.Circle(insecPos, 65, (uint)SegmentsValue, Color.White);
                Render.Text(Temp.IsAlly ? "Ally" : "Turret", startPosV2, RenderTextFlags.Center, Color.Orange);
            }
        }
    }
}
