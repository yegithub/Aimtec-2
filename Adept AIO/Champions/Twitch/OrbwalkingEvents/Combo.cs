namespace Adept_AIO.Champions.Twitch.OrbwalkingEvents
{
    using System.Linq;
    using Aimtec.SDK.Extensions;
    using Core;
    using SDK.Unit_Extensions;

    class Combo
    {
        public static void OnUpdate()
        {
            var target = GameObjects.EnemyHeroes.FirstOrDefault(x => x.IsValidTarget(SpellManager.R.Range));
            if (target == null)
            {
                return;
            }

            if (SpellManager.Q.Ready && MenuConfig.Combo["Q"].Enabled)
            {
                SpellManager.Q.Cast();
            }

            if (SpellManager.W.Ready && MenuConfig.Combo["W"].Enabled && !(MenuConfig.Combo["W2"].Enabled && SpellManager.HasUltBuff()))
            {
                SpellManager.W.Cast(target);
            }

            if (SpellManager.R.Ready && MenuConfig.Combo["R"].Enabled)
            {
                if (Global.Player.CountEnemyHeroesInRange(1500) < MenuConfig.Combo["R2"].Value)
                {
                    return;
                }

                SpellManager.CastR(target);
            }
        }
    }
}