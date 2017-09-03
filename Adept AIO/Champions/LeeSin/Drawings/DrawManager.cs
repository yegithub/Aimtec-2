using System;
using System.Drawing;
using System.Linq;
using Adept_AIO.Champions.LeeSin.Core;
using Adept_AIO.Champions.LeeSin.Core.Insec_Manager;
using Adept_AIO.Champions.LeeSin.Core.Spells;
using Adept_AIO.SDK.Extensions;
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

            foreach (var target in GameObjects.EnemyHeroes.Where(x => !x.IsDead && x.IsFloatingHealthBarActive && x.IsVisible))
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
            Render.Text(new Vector2(bkToggleV2.X - 40, bkToggleV2.Y + 70), Temp.IsBubbaKush ? Color.White : Color.LightSlateGray, "Bubba Kush: " + Temp.IsBubbaKush);

            var selected = Global.TargetSelector.GetSelectedTarget();

            if (!PositionEnabled || selected == null)
            {
                return;
            }

            if (Temp.IsBubbaKush && !_insecManager.BkPosition(selected).IsZero)
            {
                var bkPos = _insecManager.BkPosition(selected);
                Render.WorldToScreen(bkPos, out var bkScreen);
                Render.Text(bkScreen, Color.Orange, "BK");

                var bkEndPos = selected.ServerPosition + (selected.ServerPosition - bkPos).Normalized() * 900;
                Render.WorldToScreen(bkEndPos, out var bkEndPosV2);
                Render.WorldToScreen(bkPos, out var bkPosV2);

                var pos1 = bkEndPosV2 + (bkPosV2 - bkEndPosV2).Normalized().Rotated(40 * (float)Math.PI / 180) * selected.BoundingRadius;
                var pos2 = bkEndPosV2 + (bkPosV2 - bkEndPosV2).Normalized().Rotated(-40 * (float)Math.PI / 180) * selected.BoundingRadius;

                Render.Line(bkEndPosV2, pos1, Color.White);
                Render.Line(bkEndPosV2, pos2, Color.White);
                Render.Line(bkPosV2, bkEndPosV2, Color.Orange);

                Render.Circle(bkPos, 65, (uint)SegmentsValue, Color.Orange);
            }
            else if(!_insecManager.InsecPosition(selected).IsZero)
            {
                var insecPos = _insecManager.InsecPosition(selected);
                var targetEndPos = selected.ServerPosition + (selected.ServerPosition - insecPos).Normalized() * 900;

                Render.WorldToScreen(targetEndPos, out var endPosV2);
                Render.WorldToScreen(insecPos, out var insecPosScreen);

                var pos1 = endPosV2 + (insecPosScreen - endPosV2).Normalized().Rotated( 40 * (float)Math.PI / 180) * selected.BoundingRadius;
                var pos2 = endPosV2 + (insecPosScreen - endPosV2).Normalized().Rotated(-40 * (float)Math.PI / 180) * selected.BoundingRadius;

                Render.Line(endPosV2, pos1, Color.White);
                Render.Line(endPosV2, pos2, Color.White);
                Render.Line(insecPosScreen, endPosV2, Color.Orange);

                Render.Circle(insecPos, 65, (uint)SegmentsValue, Color.White);
                Render.Text(insecPosScreen, Color.Orange, Temp.IsAlly ? "Ally" : "Turret");
            }
        }
    }
}
