using Adept_AIO.Champions.Tristana.Core;
using Adept_AIO.SDK.Extensions;

namespace Adept_AIO.Champions.Tristana.Update.OrbwalkingEvents
{
    class Harass
    {
        private readonly SpellConfig SpellConfig;
        private readonly MenuConfig MenuConfig;

        public Harass(SpellConfig spellConfig, MenuConfig menuConfig)
        {
            SpellConfig = spellConfig;
            MenuConfig = menuConfig;
        }

        public void OnUpdate()
        {
            var target = Global.TargetSelector.GetTarget(SpellConfig.FullRange);

            if (target == null || Global.Orbwalker.IsWindingUp)
            {
                return;
            }

            if (SpellConfig.Q.Ready && MenuConfig.Harass["Q"].Enabled)
            {
                SpellConfig.Q.Cast();
            }

            if (SpellConfig.E.Ready && MenuConfig.Harass["E"].Enabled)
            {
                if (!MenuConfig.Harass[target.ChampionName].Enabled)
                {
                    return;
                }
                SpellConfig.E.CastOnUnit(target);
            }
        }
    }
}
