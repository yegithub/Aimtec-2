using System.Linq;
using Adept_AIO.Champions.Vayne.Core;
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
        private static double _turretDamage;
      
        public static void PostAttack(object sender, PostAttackEventArgs args)
        {
            if (!SpellManager.Q.Ready || MenuConfig.LaneClear["Q3"].Value == 1)
            {
                return;
            }

            var minion = GameObjects.EnemyMinions.FirstOrDefault(x => args.Target.NetworkId != x.NetworkId && x.Health > 0 && x.MaxHealth > 0 && x.Health < Global.Player.GetAutoAttackDamage(x) + Global.Player.GetSpellDamage(x, SpellSlot.Q) && x.Distance(Global.Player) <= Global.Player.AttackRange + 80);
            if (minion == null)
            {
                return;
            }
            SpellManager.CastQ(minion, MenuConfig.LaneClear["QMode3"].Value);
        }

        public static void PreAttack(object sender, PreAttackEventArgs args)
        {
            if (MenuConfig.LaneClear["TurretFarm"].Enabled)
            {
                if (_turretTarget != null && _turret != null)
                {
                    var playerDamage = Global.Player.GetAutoAttackDamage(_turretTarget);

                    if (_turretTarget.Health < playerDamage)
                    {
                        args.Target = _turretTarget;
                    }

                    var minion = GameObjects.EnemyMinions.FirstOrDefault(x => x.IsUnderAllyTurret());
                    if (minion != null)
                    {
                        var turretDamage = _turret.GetAutoAttackDamage(minion);
                       
                        if (minion.Health < turretDamage + Global.Player.GetAutoAttackDamage(minion) && minion.Health > turretDamage)
                        {
                            args.Target = minion;
                        }
                    }
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

            if (_turret != null && _turretTarget != null && MenuConfig.LaneClear["TurretFarm"].Enabled)
            {
                if (_turretTarget.Health > Global.Player.GetAutoAttackDamage(_turretTarget) && _turretTarget.Health <
                    Global.Player.GetSpellDamage(_turretTarget, SpellSlot.Q) +
                    Global.Player.GetAutoAttackDamage(_turretTarget))
                {
                    SpellManager.CastQ(_turretTarget);
                }

                if (_turret.IsDead)
                {
                    _turret = null;
                    _turretTarget = null;
                }
            }
           
            if (MenuConfig.LaneClear["Q3"].Value == 1 && minion.Health < Global.Player.GetAutoAttackDamage(minion))
            {
                SpellManager.CastQ(minion, MenuConfig.LaneClear["QMode3"].Value);
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
                _turretDamage = args.Sender.GetAutoAttackDamage(_turretTarget);
            }
            else
            {
                _turret = null;
                _turretTarget = null;
            }
        }
    }
}
