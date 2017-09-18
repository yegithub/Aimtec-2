﻿using System.Linq;
using Adept_AIO.Champions.Azir.Core;
using Adept_AIO.SDK.Junk;
using Aimtec;
using Aimtec.SDK.Extensions;

namespace Adept_AIO.Champions.Azir.Update.OrbwalkingEvents
{
    class LaneClear
    {
        public static void OnUpdate()
        {
            if (MenuConfig.Lane["Check"].Enabled && Global.Player.CountEnemyHeroesInRange(2000) > 0)
            {
                return;
            }

            if (SpellConfig.Q.Ready && MenuConfig.Lane["Q"].Enabled &&
                Global.Player.ManaPercent() > MenuConfig.Lane["Q"].Value)
            {
                var lastMinion = GameObjects.EnemyMinions.LastOrDefault(x => x.IsValid);
                if (lastMinion == null)
                {
                    return;
                }

                var rect = new Geometry.Rectangle(Global.Player.ServerPosition.To2D(), lastMinion.ServerPosition.To2D(), SpellConfig.Q.Width);

                var count = GameObjects.EnemyMinions.Count(x => rect.IsInside(x.ServerPosition.To2D()));

                if (count >= MenuConfig.Lane["QHit"].Value)
                {
                    SpellConfig.Q.Cast(lastMinion);
                }
            }

            if (SpellConfig.W.Ready && MenuConfig.Lane["W"].Enabled &&
                Global.Player.ManaPercent() > MenuConfig.Lane["W"].Value)
            {
                var minion = GameObjects.EnemyMinions.FirstOrDefault(x => x.Distance(Global.Player) <= SpellConfig.W.Range && x.MaxHealth > 10 && x.IsValid && !x.IsDead);
                if (minion == null)
                {
                    return;
                }

                var circle = new Geometry.Circle(minion.ServerPosition.To2D(), 250);

                if (GameObjects.EnemyMinions.Count(x => circle.Center.Distance(x.ServerPosition) <= circle.Radius) >= 3 && Global.Player.GetSpell(SpellSlot.W).Ammo > 1)
                {
                    SpellConfig.W.Cast(Global.Player.ServerPosition.Extend(minion.ServerPosition, SpellConfig.W.Range + 100));
                }
            }

            if (SpellConfig.E.Ready && MenuConfig.Lane["E"].Enabled && Global.Player.ManaPercent() > MenuConfig.Lane["E"].Value)
            {
                foreach (var soldier in SoldierHelper.Soldiers)
                {
                    var rect = new Geometry.Rectangle(Global.Player.ServerPosition.To2D(), soldier.ServerPosition.To2D(), SpellConfig.E.Width);

                    var count = GameObjects.EnemyMinions.Count(x => rect.IsInside(x.ServerPosition.To2D()));
                    if (count >= MenuConfig.Lane["EHit"].Value)
                    {
                        SpellConfig.E.Cast(soldier.ServerPosition);
                    }
                }
            }
        }
    }
}