using System.Drawing;
using System.Linq;
using Adept_AIO.Champions.Tristana.Core;
using Adept_AIO.SDK.Extensions;
using Aimtec;

namespace Adept_AIO.Champions.Tristana.Drawings
{
    internal class DrawManager
    {
        private readonly SpellConfig SpellConfig;
        private readonly MenuConfig MenuConfig;
        private readonly Dmg Dmg;

        public DrawManager(MenuConfig menuConfig, Dmg dmg, SpellConfig spellConfig)
        {
            MenuConfig = menuConfig;
            Dmg = dmg;
            SpellConfig = spellConfig;
        }

        public void OnPresent()
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

        public void OnRender()
        {
            if (Global.Player.IsDead)
            {
                return;
            }

            if (MenuConfig.Drawings["W"].Enabled)
            {
                Render.Circle(Global.Player.Position, SpellConfig.W.Range, (uint)MenuConfig.Drawings["Segments"].Value, Color.Gray);
            }
        }
    }
}
