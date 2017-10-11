using System.Linq;
using System.Runtime.Serialization.Formatters;
using Adept_AIO.Champions.Vayne.Core;
using Adept_AIO.SDK.Generic;
using Adept_AIO.SDK.Unit_Extensions;
using Aimtec;
using Aimtec.SDK.Damage;
using Aimtec.SDK.Extensions;
using Aimtec.SDK.Orbwalking;

namespace Adept_AIO.Champions.Vayne.OrbwalkingMode
{
    internal class LaneClear
    {
        private static Obj_AI_Minion _turretTarget;
        private static Obj_AI_Base _turret;
        private static float LastTurretShotTick;

        public static void PostAttack(object sender, PostAttackEventArgs args)
        {
            if (!SpellManager.Q.Ready || MenuConfig.LaneClear["Q"].Value == 1)
            {
                return;
            }

            var minion = GameObjects.EnemyMinions.FirstOrDefault(x => args.Target.NetworkId != x.NetworkId && x.Health > 0 && x.MaxHealth > 0 && x.Health < Global.Player.GetAutoAttackDamage(x) && x.Distance(Global.Player) <= SpellManager.Q.Range);
            if (minion == null)
            {
                return;
            }
            SpellManager.CastQ(minion, MenuConfig.LaneClear["QMode"].Value);
        }

        private static bool TurretTargetKillable(double damage)
        {
            if (_turretTarget == null || _turret == null)
            {
                return false;
            }

            if (_turretTarget.IsDead)
            {
                _turret = null;
                _turretTarget = null;
            }
            else if (_turretTarget.Health < damage)
            {
                return true;
            }
            return false;
        }

        public static void PreAttack(object sender, PreAttackEventArgs args)
        {
            if (MenuConfig.LaneClear["TurretFarm"].Enabled && _turretTarget != null && _turret != null)
            {
                DebugConsole.Write("??????");
                var turretDamage = _turret.GetAutoAttackDamage(_turretTarget);
                var playerDamage = Global.Player.GetAutoAttackDamage(_turretTarget);

                if (TurretTargetKillable(playerDamage))
                {
                    args.Target = _turretTarget;
                }
                else if (Game.TickCount - LastTurretShotTick >= 800 && Global.Player.IsUnderAllyTurret() && Global.Orbwalker.CanAttack())
                {
                    args.Cancel = true;
                }
                else if (TurretTargetKillable(turretDamage * 2) && !TurretTargetKillable(playerDamage))
                {
                    args.Cancel = true;
                }
            }
        }

        public static void OnUpdate()
        {
            var minion = GameObjects.EnemyMinions.FirstOrDefault(x => x.Distance(Global.Player) <= SpellManager.Q.Range + Global.Player.AttackRange && x.Health > 0 && x.MaxHealth > 0);
            if (minion == null || !SpellManager.Q.Ready)
            {
                return;
            }

            if (_turret != null && _turretTarget != null && TurretTargetKillable(_turret.GetAutoAttackDamage(_turretTarget)) && Game.TickCount - LastTurretShotTick <= 300)
            {
                DebugConsole.Write("????!!!!!!!!!!");
                SpellManager.CastQ(_turretTarget);
            }
           
            if (MenuConfig.LaneClear["Q"].Value == 1 && minion.Health < Global.Player.GetAutoAttackDamage(minion))
            {
                SpellManager.CastQ(minion, MenuConfig.LaneClear["QMode"].Value);
            }
        }

        public static void OnProcessSpellCast(Obj_AI_Base sender, Obj_AI_BaseMissileClientDataEventArgs args)
        {
            if (sender == null
                || args.Target == null
                || !args.Target.IsEnemy
                || !sender.UnitSkinName.ToLower().Contains("turret")
                || !args.Target.Name.ToLower().Contains("minion"))
            {
                return;
            }

            if (Global.Player.Distance(args.Target) <= SpellManager.Q.Range + Global.Player.AttackRange)
            {
                _turret = sender;
                _turretTarget = args.Target as Obj_AI_Minion;
                LastTurretShotTick = Game.TickCount;
            }
            else
            {
                _turret = null;
                _turretTarget = null;
            }
        }
    }
}
