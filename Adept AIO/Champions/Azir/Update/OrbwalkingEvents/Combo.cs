using System.Linq;
using Adept_AIO.Champions.Azir.Core;
using Adept_AIO.SDK.Geometry_Related;
using Adept_AIO.SDK.Unit_Extensions;
using Aimtec;
using Aimtec.SDK.Extensions;

namespace Adept_AIO.Champions.Azir.Update.OrbwalkingEvents
{
    class Combo
    {
        public static void OnUpdate()
        {
            var target = Global.TargetSelector.GetTarget(SpellConfig.Q.Range + 400);
            if (target == null)
            {
                return;
            }

            var dist = target.Distance(Global.Player) - Global.Player.BoundingRadius - target.BoundingRadius;

            if (SpellConfig.E.Ready && MenuConfig.Combo["E"].Enabled)
            {
                foreach (var soldier in SoldierHelper.Soldiers)
                {
                    var rect = new Geometry.Rectangle(Global.Player.ServerPosition.To2D(), soldier.ServerPosition.To2D(), SpellConfig.E.Width);
                    var count = GameObjects.EnemyHeroes.Count(x => rect.IsInside(x.ServerPosition.To2D()));

                    if (count >= 2)
                    {
                        SpellConfig.E.Cast(soldier.ServerPosition);
                    }
                }

                if (target.HealthPercent() <= MenuConfig.Combo["EDmg"].Value)
                {
                    var soldier = SoldierHelper.Soldiers.FirstOrDefault(x => x.Distance(target) <= 500 && !x.IsMoving);
                    if (soldier != null && soldier.ServerPosition != Vector3.Zero)
                    {
                        SpellConfig.E.Cast(soldier);
                    }
                }
            }

            if (SpellConfig.Q.Ready && MenuConfig.Combo["Q"].Enabled && dist < SpellConfig.Q.Range + 200)
            {
                if (SoldierHelper.Soldiers.Count >= MenuConfig.Combo["QCount"].Value)
                {
                    SpellConfig.CastQ(target, MenuConfig.Combo["Extend"].Enabled);
                }
            }

            if (SpellConfig.W.Ready && MenuConfig.Combo["W"].Enabled)
            {
                if (SpellConfig.Q.Ready && MenuConfig.Combo["Q"].Enabled)
                {
                    SpellConfig.W.Cast(Global.Player.ServerPosition.Extend(target.ServerPosition, SpellConfig.W.Range));
                }
                else if(dist < SpellConfig.W.Range)
                {
                    SpellConfig.W.Cast(target);
                }
            }

            if (SpellConfig.R.Ready && MenuConfig.Combo["R"].Enabled && target.HealthPercent() <= 40 && dist < SpellConfig.R.Range)
            {
                AzirHelper.Rect = new Geometry.Rectangle(target.ServerPosition.To2D(), Global.Player.ServerPosition.Extend(target.ServerPosition, -SpellConfig.R.Width / 2f).To2D(), SpellConfig.R.Width / 2f);
                if(AzirHelper.Rect.IsInside(target.ServerPosition.To2D()))
                SpellConfig.R.Cast(target);
            }
        }
    }
}