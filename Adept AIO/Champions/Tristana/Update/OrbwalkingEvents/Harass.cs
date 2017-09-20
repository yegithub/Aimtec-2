using Adept_AIO.Champions.Tristana.Core;
using Adept_AIO.SDK.Unit_Extensions;

namespace Adept_AIO.Champions.Tristana.Update.OrbwalkingEvents
{
    internal class Harass
    {
        private readonly SpellConfig _spellConfig;
        private readonly MenuConfig _menuConfig;

        public Harass(SpellConfig spellConfig, MenuConfig menuConfig)
        {
            _spellConfig = spellConfig;
            _menuConfig = menuConfig;
        }

        public void OnUpdate()
        {
            var target = Global.TargetSelector.GetTarget(_spellConfig.FullRange);

            if (target == null || Global.Orbwalker.IsWindingUp)
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
