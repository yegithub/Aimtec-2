namespace Adept_AIO.Champions.Jinx.OrbwalkingEvents
{
    using Aimtec.SDK.Extensions;
    using Core;
    using SDK.Unit_Extensions;

    class Combo
    {
        private readonly MenuConfig _menuConfig;
        private readonly SpellConfig _spellConfig;

        public Combo(SpellConfig spellConfig, MenuConfig menuConfig)
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

            if (_spellConfig.E.Ready &&
                _menuConfig.Combo["Close"].Enabled &&
                target.Distance(Global.Player) <= Global.Player.AttackRange - 250)
            {
                _spellConfig.E.Cast(target);
            }

            if (_spellConfig.Q.Ready && _menuConfig.Combo["Q"].Enabled)
            {
                if (!_spellConfig.IsQ2 && dist > _spellConfig.DefaultAuotAttackRange && dist <= _spellConfig.Q2Range ||
                    _spellConfig.IsQ2 && dist <= _spellConfig.DefaultAuotAttackRange)
                {
                    _spellConfig.Q.Cast();
                }
            }

            if (_spellConfig.W.Ready &&
                _menuConfig.Combo["W"].Enabled &&
                dist <= _menuConfig.Combo["W"].Value &&
                target.Distance(Global.Player) > Global.Player.AttackRange + 200)
            {
                _spellConfig.W.Cast(target);
            }
        }
    }
}