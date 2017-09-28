using System.Linq;
using Adept_AIO.Champions.Azir.Core;
using Adept_AIO.SDK.Geometry_Related;
using Adept_AIO.SDK.Unit_Extensions;
using Aimtec;
using Aimtec.SDK.Extensions;

namespace Adept_AIO.Champions.Azir.Update.OrbwalkingEvents
{
    internal class Combo
    {
        public static void OnUpdate()
        {
            var target = Global.TargetSelector.GetTarget(SpellConfig.Q.Range + 400);
            if (target == null)
            {
                return;
            }

            var dist = target.Distance(Global.Player);

            if (SpellConfig.E.Ready && MenuConfig.Combo["E"].Enabled)
            {
                foreach (var soldier in SoldierHelper.Soldiers)
                {
                    var rect = new Geometry.Rectangle(Vector3Extensions.To2D(Global.Player.ServerPosition), Vector3Extensions.To2D(soldier.ServerPosition), SpellConfig.E.Width);
                    var count = GameObjects.EnemyHeroes.Count(x => rect.IsInside(Vector3Extensions.To2D(x.ServerPosition)));

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
                if (SoldierHelper.Soldiers.All(soldier => soldier.Distance(target) <= 200))
                {
                    return;
                }

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
                AzirHelper.Rect = new Geometry.Rectangle(Vector3Extensions.To2D(target.ServerPosition), Vector3Extensions.To2D(Global.Player.ServerPosition.Extend(target.ServerPosition, -SpellConfig.R.Width / 2f)), SpellConfig.R.Width / 2f);
                if(AzirHelper.Rect.IsInside(Vector3Extensions.To2D(target.ServerPosition)))
                SpellConfig.R.Cast(target);
            }
        }
    }
}