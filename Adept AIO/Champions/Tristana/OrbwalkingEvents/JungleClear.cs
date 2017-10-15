namespace Adept_AIO.Champions.Tristana.OrbwalkingEvents
{
    using System.Linq;
    using Aimtec;
    using Aimtec.SDK.Extensions;
    using Core;
    using SDK.Unit_Extensions;

    class JungleClear
    {
        private readonly MenuConfig _menuConfig;
        private readonly SpellConfig _spellConfig;

        public JungleClear(MenuConfig menuConfig, SpellConfig spellConfig)
        {
            _menuConfig = menuConfig;
            _spellConfig = spellConfig;
        }

        public void OnPostAttack(AttackableUnit target)
        {
            if (!_spellConfig.Q.Ready ||
                !_menuConfig.JungleClear["Q"].Enabled ||
                target == null ||
                _menuConfig.JungleClear["Avoid"].Enabled && Global.Player.Level == 1)
            {
                return;
            }

            _spellConfig.Q.Cast();
        }

        public void OnUpdate()
        {
            var mob = GameObjects.Jungle.Where(x => x.IsValidTarget(_spellConfig.FullRange)).
                OrderByDescending(x => x.GetJungleType()).
                FirstOrDefault();

            if (mob == null || _menuConfig.JungleClear["Avoid"].Enabled && Global.Player.Level == 1)
            {
                return;
            }

            if (_spellConfig.E.Ready && _menuConfig.JungleClear["E"].Enabled)
            {
                _spellConfig.E.CastOnUnit(mob);
            }
        }
    }
}