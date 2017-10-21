namespace Adept_AIO.Champions.Twitch.OrbwalkingEvents
{
    using System.Linq;
    using Aimtec.SDK.Extensions;
    using Core;
    using SDK.Geometry_Related;
    using SDK.Unit_Extensions;

    class LaneClear
    {
        public static void OnUpdate()
        {
            if (MenuConfig.LaneClear["Check"].Enabled && Global.Player.CountEnemyHeroesInRange(1500) != 0 || Global.Player.ManaPercent() <= 30)
            {
                return;
            }

            if (SpellManager.E.Ready &&
                GameObjects.EnemyMinions.Count(x => x.IsValidTarget(SpellManager.E.Range) && Dmg.EDmg(x) > x.Health) >= MenuConfig.LaneClear["E"].Value &&
                MenuConfig.LaneClear["E"].Enabled)
            {
                SpellManager.E.Cast();
            }

            if (MenuConfig.LaneClear["W"].Enabled && SpellManager.W.Ready)
            {
                var minion = GameObjects.EnemyMinions.FirstOrDefault(x => x.IsValidTarget(SpellManager.W.Range));
                if(minion != null)
                SpellManager.W.Cast(minion, true, MenuConfig.LaneClear["W"].Value);
            }
        }
    }
}