namespace Adept_AIO.Champions.Jinx.OrbwalkingEvents
{
    using Aimtec.SDK.Extensions;
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
            var target = Global.TargetSelector.GetTarget(_spellConfig.W.Range);

            if (target == null)
            {
                return;
            }

            var dist = target.Distance(Global.Player);

            if (_spellConfig.Q.Ready && _menuConfig.Harass["Q"].Enabled)
            {
                if (!_spellConfig.IsQ2 && dist > _spellConfig.DefaultAuotAttackRange && dist <= _spellConfig.Q2Range ||
                    _spellConfig.IsQ2 && dist <= _spellConfig.DefaultAuotAttackRange)
                {
                    _spellConfig.Q.Cast();
                }
            }

            if (_spellConfig.W.Ready && _menuConfig.Harass["W"].Enabled && dist <= _menuConfig.Harass["W"].Value)
            {
                _spellConfig.W.Cast(target);
            }
        }
    }
}