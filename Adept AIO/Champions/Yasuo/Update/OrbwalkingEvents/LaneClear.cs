using System.Linq;
using Adept_AIO.Champions.Yasuo.Core;
using Adept_AIO.SDK.Geometry_Related;
using Adept_AIO.SDK.Unit_Extensions;
using Aimtec;
using Aimtec.SDK.Damage;
using Aimtec.SDK.Events;
using Aimtec.SDK.Extensions;
using GameObjects = Adept_AIO.SDK.Unit_Extensions.GameObjects;

namespace Adept_AIO.Champions.Yasuo.Update.OrbwalkingEvents
{
    internal class LaneClear
    {
    
        public static void OnPostAttack()
        {
            if (MenuConfig.LaneClear["Check"].Enabled && Global.Player.CountEnemyHeroesInRange(2000) != 0)
            {
                return;
            }

            if (SpellConfig.E.Ready && MenuConfig.LaneClear["EAA"].Enabled)
            {
                var minion = GameObjects.EnemyMinions.FirstOrDefault(x => x.IsValidTarget() && x.Distance(Global.Player) <= SpellConfig.E.Range && !x.HasBuff("YasuoDashWrapper"));

                if (!SpellConfig.E.Ready || minion == null || MenuConfig.LaneClear["Turret"].Enabled && minion.IsUnderEnemyTurret() || MenuConfig.LaneClear["Check"].Enabled && Global.Player.CountEnemyHeroesInRange(2000) != 0)
                {
                    return;
                }

                switch (MenuConfig.LaneClear["Mode"].Value)
                {
                    case 1:

                        if (minion.Health > Global.Player.GetSpellDamage(minion, SpellSlot.E))
                        {
                            return;
                        }

                        SpellConfig.E.CastOnUnit(minion);
                        break;
                    case 2:
                        SpellConfig.E.CastOnUnit(minion);
                        break;
                }
            }

            if (SpellConfig.Q.Ready)
            {
                var minion = GameObjects.EnemyMinions.FirstOrDefault(x => x.Distance(Global.Player) <= SpellConfig.Q.Range && x.IsValidTarget());
                if (minion == null)
                {
                    return;
                }

                switch (Extension.CurrentMode)
                {
                    case Mode.Normal:
                        SpellConfig.Q.Cast(minion);
                        break;
                }
            }
        }

        public static void OnUpdate()
        {
            if (SpellConfig.E.Ready && !MenuConfig.LaneClear["EAA"].Enabled)
            {
                var minion = GameObjects.EnemyMinions.FirstOrDefault(x => x.IsValidTarget() && x.Distance(Global.Player) <= SpellConfig.E.Range && !x.HasBuff("YasuoDashWrapper"));

                if (!SpellConfig.E.Ready || minion == null || MenuConfig.LaneClear["Turret"].Enabled && minion.IsUnderEnemyTurret() || MenuConfig.LaneClear["Check"].Enabled && Global.Player.CountEnemyHeroesInRange(2000) != 0)
                {
                    return;
                }

                switch (MenuConfig.LaneClear["Mode"].Value)
                {
                    case 1:

                        if (minion.Health > Global.Player.GetSpellDamage(minion, SpellSlot.E))
                        {
                            return;
                        }

                        SpellConfig.E.CastOnUnit(minion);
                        break;
                    case 2:
                        SpellConfig.E.CastOnUnit(minion);
                        break;
                }
            }
            
            if (SpellConfig.Q.Ready)
            {
                switch (Extension.CurrentMode)
                {
                    case Mode.Tornado:

                        var m = GameObjects.EnemyMinions.LastOrDefault(x => x.IsValidSpellTarget(SpellConfig.Q.Range));
                        if (m == null)
                        {
                            return;
                        }

                        var rect = new Geometry.Rectangle(Geometry.To2D(Global.Player.ServerPosition), Geometry.To2D(m.ServerPosition), SpellConfig.Q.Width);
                        var count = GameObjects.EnemyMinions.Count(x => rect.IsInside(Geometry.To2D(x.ServerPosition)));

                        if (MenuConfig.LaneClear["Q3"].Enabled && count >= 2)
                        {
                            SpellConfig.Q.Cast(m);
                        }
                        break;
                    case Mode.Normal:
                        var nM = GameObjects.EnemyMinions.FirstOrDefault(x => x.IsValidSpellTarget(SpellConfig.Q.Range - 100));
                        if (nM == null)
                        {
                            return;
                        }
                        SpellConfig.Q.Cast(nM);
                        break;
                    case Mode.DashingTornado:
                    case Mode.Dashing:
                        var dashM = GameObjects.EnemyMinions.FirstOrDefault(x => x.IsValidSpellTarget(SpellConfig.Q.Range));
                        if (dashM == null || !dashM.IsValidTarget())
                        {
                            return;
                        }

                        var circle = new Geometry.Circle(Global.Player.GetDashInfo().EndPos, 220);
                        var circleCount = GameObjects.EnemyMinions.Count(x => circle.Center.Distance(x.ServerPosition) <= circle.Radius);
                       
                        if (circleCount >= 1)
                        {
                            SpellConfig.Q.Cast(dashM);
                        }
                        break;
                }
            }
        }
    }
}
