using System.Drawing;
using System.Linq;
using Adept_AIO.Champions.LeeSin.Core.Insec_Manager;
using Adept_AIO.Champions.LeeSin.Core.Spells;
using Adept_AIO.SDK.Extensions;
using Aimtec;

namespace Adept_AIO.Champions.LeeSin.Drawings
{
    internal class DrawManager : IDrawManager
    {
        public bool QEnabled { get; set; }
        public bool PositionEnabled { get; set; }
        public int SegmentsValue { get; set; }

        private readonly ISpellConfig SpellConfig;
        private readonly IInsec_Manager _insecManager;
        private readonly IDmg Damage;

        public DrawManager(ISpellConfig spellConfig, IInsec_Manager insecManager, IDmg damage)
        {
            SpellConfig = spellConfig;
            _insecManager = insecManager;
            Damage = damage;
        }

        public void RenerDamage()
        {
            if (Global.Player.IsDead)
            {
                return;
            }

            foreach (var target in GameObjects.EnemyHeroes.Where(x => !x.IsDead && x.IsFloatingHealthBarActive && x.IsVisible))
            {
                var damage = Damage.Damage(target);

                Global.DamageIndicator.Unit = target;
                Global.DamageIndicator.DrawDmg((float)damage, Color.FromArgb(153, 12, 177, 28));
            }
        }

        public void RenderManager()
        {
            if (Global.Player.IsDead)
            {
                return;
            }

            if (QEnabled && SpellConfig.Q.Ready)
            {
                Render.Circle(Global.Player.Position, SpellConfig.Q.Range, (uint)SegmentsValue, Color.IndianRed);
            }

            var selected = Global.TargetSelector.GetSelectedTarget();

            if (PositionEnabled && selected != null && _insecManager.InsecPosition(selected) != Vector3.Zero)
            {
                Render.Circle(_insecManager.InsecPosition(selected), 65, (uint)SegmentsValue, Color.White);
            }
        }
    }
}
