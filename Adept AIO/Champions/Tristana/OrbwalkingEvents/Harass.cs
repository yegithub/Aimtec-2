namespace Adept_AIO.Champions.Tristana.OrbwalkingEvents
{
    using Core;
    using SDK.Unit_Extensions;

    class Harass
    {
        private readonly MenuConfig _menuConfig;
        private readonly SpellConfig _spellConfig;

        public Harass(SpellConfig spellConfig, MenuConfig menuConfig)
        {
            _spellConfig = spellConfig;
            _menuConfig = menuConfig;
        }

        public void OnUpdate()
        {
            var target = Global.TargetSelector.GetTarget(_spellConfig.FullRange);

            if (target == null)
            {
                return;
            }

            if (_spellConfig.Q.Ready && _menuConfig.Harass["Q"].Enabled)
            {
                _spellConfig.Q.Cast();
            }

            if (_spellConfig.E.Ready && _menuConfig.Harass["E"].Enabled)
            {
                if (!_menuConfig.Harass[target.ChampionName].Enabled)
                {
                    return;
                }
                _spellConfig.E.CastOnUnit(target);
            }
        }
    }
}