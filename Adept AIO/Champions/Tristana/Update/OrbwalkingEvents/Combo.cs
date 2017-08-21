using Adept_AIO.Champions.Tristana.Core;
using Adept_AIO.SDK.Extensions;
using Aimtec.SDK.Extensions;

namespace Adept_AIO.Champions.Tristana.Update.OrbwalkingEvents
{
    class Combo
    {
        private readonly SpellConfig SpellConfig;
        private readonly MenuConfig MenuConfig;
        private readonly Dmg Dmg;

        public Combo(SpellConfig spellConfig, MenuConfig menuConfig, Dmg dmg)
        {
            SpellConfig = spellConfig;
            MenuConfig = menuConfig;
            Dmg = dmg;
        }

        public void OnUpdate()
        {
            var target = Global.TargetSelector.GetTarget(SpellConfig.FullRange);

            if (target == null || Global.Orbwalker.IsWindingUp)
            {
                return;
            }

            if (SpellConfig.Q.Ready && MenuConfig.Combo["Q"].Enabled)
            {
                SpellConfig.Q.Cast();
            }

            if (SpellConfig.W.Ready 
             && MenuConfig.Combo["W"].Enabled
             && target.Health < Dmg.Damage(target) 
             && target.Distance(Global.Player) > Global.Player.AttackRange + 100
             && Global.Player.CountEnemyHeroesInRange(2000) <= 2 
             && target.ServerPosition.CountAllyHeroesInRange(900) == 0)
            {
                SpellConfig.W.Cast(target);
            }

            if (SpellConfig.E.Ready && MenuConfig.Combo["E"].Enabled)
            {
                if (!MenuConfig.Combo[target.ChampionName].Enabled)
                {
                    return;
                }

                SpellConfig.E.CastOnUnit(target);
            }
        }
    }
}
