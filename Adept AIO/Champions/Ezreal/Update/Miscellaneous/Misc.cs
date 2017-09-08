using Aimtec;
using Aimtec.SDK.Orbwalking;

namespace Adept_AIO.Champions.Ezreal.Update.Miscellaneous
{
    using System.Linq;
    using Core;
    using SDK.Junk;
    using Aimtec.SDK.Extensions;

    internal class Misc
    {
        public static void OnUpdate()
        {
            if (SpellConfig.Q.Ready
             && Global.Orbwalker.Mode == OrbwalkingMode.None
             && Global.Player.IsMoving
             && Global.Player.CountEnemyHeroesInRange(2500) == 0
             && MenuConfig.Miscellaneous["Stack"].Enabled 
             && Global.Player.ManaPercent() >= MenuConfig.Miscellaneous["Stack"].Value  
             && Mixed.HasTear())
            {
                var objects = GameObjects.Enemy.FirstOrDefault(x => x.IsValidTarget(SpellConfig.Q.Range) && x.MaxHealth >= 10);
                if (objects != null)
                {
                    SpellConfig.Q.Cast(objects);
                }
            }

            if (SpellConfig.W.Ready
             && Global.Player.CountEnemyHeroesInRange(2500) == 0
             && MenuConfig.Miscellaneous["WT"].Enabled
             && Global.Player.ServerPosition.PointUnderEnemyTurret()
             && Global.Player.ManaPercent() >= 60)
            {
                var ally = GameObjects.AllyHeroes.FirstOrDefault(x => x.IsValidTarget(SpellConfig.W.Range - 100));
                if (ally != null)
                {
                    SpellConfig.W.Cast(ally);
                }
            }
        }
    }
}
