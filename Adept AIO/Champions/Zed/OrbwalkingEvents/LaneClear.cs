namespace Adept_AIO.Champions.Zed.OrbwalkingEvents
{
    using System.Linq;
    using Aimtec;
    using Aimtec.SDK.Damage;
    using Aimtec.SDK.Extensions;
    using Core;
    using SDK.Generic;
    using SDK.Unit_Extensions;

    class LaneClear
    {
        private static Obj_AI_Minion _turretTarget;
        private static Obj_AI_Base _turret;

        public static void OnUpdate()
        {
            if (MenuConfig.LaneClear["Check"].Enabled && Global.Player.CountEnemyHeroesInRange(2000) > 0 || Maths.GetEnergyPercent() < MenuConfig.LaneClear["Energy"].Value)
            {
                return;
            }

            if (_turretTarget != null && _turret != null && MenuConfig.LaneClear["TurretFarm"].Enabled)
            {
                if (_turretTarget.IsDead)
                {
                    _turret = null;
                    _turretTarget = null;
                }
                else
                {
                    var turretDamage = _turret.GetAutoAttackDamage(_turretTarget);
                    var playerDamage = Global.Player.GetAutoAttackDamage(_turretTarget);
                    var inAaRange = _turretTarget.Distance(Global.Player) <= Global.Player.AttackRange + 65;

                    if (!inAaRange)
                    {
                        return;
                    }

                    if (_turretTarget.Health < playerDamage * 2 + turretDamage && _turretTarget.Health > turretDamage + playerDamage && Global.Orbwalker.CanAttack())
                    {
                        Global.Orbwalker.Attack(_turretTarget);
                    }

                    else if (SpellManager.E.Ready && _turretTarget.Health < Global.Player.GetSpellDamage(_turretTarget, SpellSlot.E) + playerDamage)
                    {
                        SpellManager.CastE(_turretTarget);
                    }
                    else if (SpellManager.Q.Ready && _turretTarget.Health < Global.Player.GetSpellDamage(_turretTarget, SpellSlot.Q) + playerDamage)
                    {
                        SpellManager.CastQ(_turretTarget);
                    }
                }
            }
            else
            {
                var minion = GameObjects.EnemyMinions.FirstOrDefault(x => x.IsValidTarget(SpellManager.Q.Range));
                if (minion == null)
                {
                    return;
                }

                if (SpellManager.Q.Ready && MenuConfig.LaneClear["Q"].Enabled)
                {
                    SpellManager.CastQ(minion, MenuConfig.LaneClear["Q"].Value);
                }

                if (SpellManager.W.Ready && MenuConfig.LaneClear["W"].Enabled)
                {
                    if (GameObjects.EnemyMinions.Count(x => x.IsValidTarget(1300)) >= 6 && Global.Player.Level >= 12 && Maths.GetEnergyPercent() >= 70)
                    {
                        if (ShadowManager.CanCastW1())
                        {
                            SpellManager.W.Cast(minion.ServerPosition);
                        }
                        else
                        {
                            SpellManager.W.Cast();
                        }
                    }
                }

                if (SpellManager.E.Ready && MenuConfig.LaneClear["E"].Enabled)
                {
                    SpellManager.CastE(minion, MenuConfig.LaneClear["E"].Value, true);
                }
            }
        }

        public static void OnProcessSpellCast(Obj_AI_Base sender, Obj_AI_BaseMissileClientDataEventArgs args)
        {
            if (sender == null ||
                args.Target == null ||
                !sender.IsAlly ||
                !args.Target.IsEnemy ||
                !sender.UnitSkinName.ToLower().Contains("turret") ||
                !args.Target.Name.ToLower().Contains("minion"))
            {
                return;
            }

            if (Global.Player.Distance(args.Target) <= SpellManager.Q.Range)
            {
                _turret = sender;
                _turretTarget = args.Target as Obj_AI_Minion;
            }
            else
            {
                _turret = null;
                _turretTarget = null;
            }
        }
    }
}