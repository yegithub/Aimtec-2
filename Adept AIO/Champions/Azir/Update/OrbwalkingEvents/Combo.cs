using System.Linq;
using Adept_AIO.Champions.Azir.Core;
using Adept_AIO.SDK.Junk;
using Aimtec.SDK.Extensions;

namespace Adept_AIO.Champions.Azir.Update.OrbwalkingEvents
{
    class Combo
    {
        public static void OnUpdate()
        {
            var target = Global.TargetSelector.GetTarget(SpellConfig.Q.Range + 200);
            if (target == null)
            {
                return;
            }

            if (SpellConfig.Q.Ready && MenuConfig.Combo["Q"].Enabled)
            {
                SpellConfig.CastQ(target);
            }

            if (SpellConfig.W.Ready && MenuConfig.Combo["W"].Enabled)
            {
                SpellConfig.W.Cast(Global.Player.ServerPosition.Extend(target.ServerPosition, SpellConfig.W.Range));
            }

            if (SpellConfig.E.Ready && MenuConfig.Combo["E"].Enabled)
            {
                foreach (var soldier in SoldierHelper.Soldiers)
                {
                    var rect = new Geometry.Rectangle(Global.Player.ServerPosition.To2D(), soldier.ServerPosition.To2D(), SpellConfig.E.Width);

                    var count = GameObjects.EnemyHeroes.Count(x => rect.IsInside(x.ServerPosition.To2D()));
                    if (target.Health < Dmg.Damage(target) && count >= 1 || count >= 2)
                    {
                        SpellConfig.E.CastOnUnit(soldier);
                    }
                }
            }

            if (SpellConfig.R.Ready && MenuConfig.Combo["R"].Enabled && Dmg.Damage(target) > target.Health)
            {
                SpellConfig.R.Cast(target);
            }
        }
    }
}