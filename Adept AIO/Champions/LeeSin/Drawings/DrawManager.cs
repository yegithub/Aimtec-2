using System.Drawing;
using System.Linq;
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

            var selected = Global.TargetSelector.GetSelectedTarget();

            if (PositionEnabled && selected != null && !selected.UnitSkinName.ToLower().Contains("dummy"))
            {
                if (!_insecManager.InsecQPosition.IsZero)
                {
                    Render.Line(Geometry.To2D(Global.Player.ServerPosition), Geometry.To2D(_insecManager.InsecQPosition), 5, false, Color.Gray);
                }

                if (!_insecManager.InsecWPosition.IsZero)
                {
                    Render.Line(Geometry.To2D(Global.Player.ServerPosition), Geometry.To2D(_insecManager.InsecWPosition), 5, false, Color.Yellow);
                }

                if (!_insecManager.BKPosition(selected).IsZero)
                {
                    Render.WorldToScreen(_insecManager.BKPosition(selected), out var screen);
                    Render.Text(screen, Color.Orange, "BK");

                    Render.Circle(_insecManager.BKPosition(selected), 65, (uint)SegmentsValue, Color.Orange);
                }

                if (!_insecManager.InsecPosition(selected).IsZero)
                {
                    Render.Circle(_insecManager.InsecPosition(selected), 65, (uint)SegmentsValue, Color.White);
                }
            }
        }
    }
}
