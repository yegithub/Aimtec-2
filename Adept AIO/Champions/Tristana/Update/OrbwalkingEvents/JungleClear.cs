
using System.Linq;
using Adept_AIO.Champions.Tristana.Core;
using Adept_AIO.SDK.Extensions;
using Aimtec;
using Aimtec.SDK.Extensions;


namespace Adept_AIO.Champions.Tristana.Update.OrbwalkingEvents
{
    class JungleClear
    {
        private readonly SpellConfig SpellConfig;
        private readonly MenuConfig MenuConfig;

        public JungleClear(MenuConfig menuConfig, SpellConfig spellConfig)
        {
            MenuConfig = menuConfig;
            SpellConfig = spellConfig;
        }

        public void OnPostAttack(AttackableUnit target)
        {
            if (!SpellConfig.Q.Ready || !MenuConfig.JungleClear["Q"].Enabled || target == null || MenuConfig.JungleClear["Avoid"].Enabled && Global.Player.Level == 1)
            {
                return;
            }

            SpellConfig.Q.Cast();
        }

        public void OnUpdate()
        {
            // todo: See if this works as intended. 
            var mob = GameObjects.Jungle.Where(x => x.IsValidTarget(SpellConfig.FullRange)).OrderBy(x => x.GetJungleType()).FirstOrDefault();

            if (mob == null || Global.Orbwalker.IsWindingUp || MenuConfig.JungleClear["Avoid"].Enabled && Global.Player.Level == 1)
            {
                return;
            }

            if (SpellConfig.E.Ready && MenuConfig.JungleClear["E"].Enabled)
            {
                SpellConfig.E.CastOnUnit(mob);
            }
        }
    }
}
