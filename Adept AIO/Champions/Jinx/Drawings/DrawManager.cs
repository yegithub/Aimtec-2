namespace Adept_AIO.Champions.Jinx.Drawings
{
    using System.Drawing;
    using System.Linq;
    using Aimtec;
    using Aimtec.SDK.Damage;
    using Core;
    using SDK.Unit_Extensions;

    class DrawManager
    {
        private readonly Dmg _dmg;
        private readonly MenuConfig _menuConfig;
        private readonly SpellConfig _spellConfig;

        public DrawManager(MenuConfig menuConfig, Dmg dmg, SpellConfig spellConfig)
        {
            _menuConfig = menuConfig;
            _dmg = dmg;
            _spellConfig = spellConfig;
        }

        public void OnPresent()
        {
            if (Global.Player.IsDead || !_menuConfig.Drawings["Dmg"].Enabled)
            {
                return;
            }

            foreach (var target in GameObjects.EnemyHeroes.Where(x =>
                !x.IsDead && x.IsFloatingHealthBarActive && x.IsVisible))
            {
                var damage = Global.Player.GetSpellDamage(target, SpellSlot.R);

                Global.DamageIndicator.Unit = target;
                Global.DamageIndicator.DrawDmg((float) damage, Color.FromArgb(153, 12, 177, 28));
            }
        }

        public void OnRender()
        {
            if (Global.Player.IsDead)
            {
                return;
            }

            if (_menuConfig.Drawings["R"].Enabled)
            {
                Render.Circle(Global.Player.Position,
                    _menuConfig.Killsteal["Range"].Value,
                    (uint) _menuConfig.Drawings["Segments"].Value,
                    Color.CadetBlue);
            }

            if (_menuConfig.Drawings["W"].Enabled)
            {
                Render.Circle(Global.Player.Position,
                    _spellConfig.W.Range,
                    (uint) _menuConfig.Drawings["Segments"].Value,
                    Color.Gray);
            }
        }
    }
}