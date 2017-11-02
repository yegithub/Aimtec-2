namespace Adept_AIO.Champions.Jinx.OrbwalkingEvents
{
    using System.Linq;
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

        public void OnUpdate()
        {
            var minion = GameObjects.JungleLarge.FirstOrDefault(x => x.IsValidTarget(_spellConfig.W.Range));
            if (minion == null)
            {
                return;
            }

            var dist = Global.Player.Distance(minion);

            if (_spellConfig.W.Ready && _menuConfig.JungleClear["W"].Enabled && _menuConfig.JungleClear["W"].Value < Global.Player.ManaPercent() && dist <= 650 &&
                Global.Player.CountEnemyHeroesInRange(2000) == 0)
            {
                _spellConfig.W.Cast(minion);
            }

            if (_spellConfig.Q.Ready && _menuConfig.JungleClear["Q"].Enabled)
            {
                if (!_spellConfig.IsQ2 && dist > _spellConfig.DefaultAuotAttackRange || _spellConfig.IsQ2 && dist <= _spellConfig.DefaultAuotAttackRange)
                {
                    _spellConfig.Q.Cast();
                }
            }
        }
    }
}