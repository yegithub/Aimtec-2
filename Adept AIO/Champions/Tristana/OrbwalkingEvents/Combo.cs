namespace Adept_AIO.Champions.Tristana.OrbwalkingEvents
{
    using Aimtec.SDK.Extensions;
    using Core;
    using SDK.Unit_Extensions;

    class Combo
    {
        private readonly Dmg _dmg;
        private readonly MenuConfig _menuConfig;
        private readonly SpellConfig _spellConfig;

        public Combo(SpellConfig spellConfig, MenuConfig menuConfig, Dmg dmg)
        {
            _spellConfig = spellConfig;
            _menuConfig = menuConfig;
            _dmg = dmg;
        }

        public void OnUpdate()
        {
            var target = Global.TargetSelector.GetTarget(_spellConfig.FullRange);

            if (target == null)
            {
                return;
            }

            if (_spellConfig.Q.Ready && _menuConfig.Combo["Q"].Enabled)
            {
                _spellConfig.Q.Cast();
            }

            if (_spellConfig.W.Ready &&
                _menuConfig.Combo["W"].Enabled &&
                target.Health < _dmg.Damage(target) &&
                target.Distance(Global.Player) > Global.Player.AttackRange + 100 &&
                Global.Player.CountEnemyHeroesInRange(2000) <= 2 &&
                target.ServerPosition.CountAllyHeroesInRange(900) == 0)
            {
                _spellConfig.W.Cast(target);
            }

            if (_spellConfig.E.Ready && _menuConfig.Combo["E"].Enabled)
            {
                if (!_menuConfig.Combo[target.ChampionName].Enabled)
                {
                    return;
                }

                _spellConfig.E.CastOnUnit(target);
            }
        }
    }
}