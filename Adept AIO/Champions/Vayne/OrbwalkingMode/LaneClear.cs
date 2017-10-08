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

        public static void PostAttack(object sender, PostAttackEventArgs args)
        {
            if (!SpellManager.Q.Ready || MenuConfig.LaneClear["Q"].Value == 1)
            {
                return;
            }

            var minion = GameObjects.EnemyMinions.FirstOrDefault(x => args.Target.NetworkId != x.NetworkId && x.Health < Global.Player.GetAutoAttackDamage(x) && x.Distance(Global.Player) <= SpellManager.Q.Range);
            if (minion == null || !minion.IsValidTarget())
            {
                return;
            }
            SpellManager.CastQ(minion, MenuConfig.LaneClear["QMode"].Value);
        }

        public static void OnUpdate()
        {
            var minion = GameObjects.EnemyMinions.FirstOrDefault(x => x.Health < Global.Player.GetAutoAttackDamage(x) && x.Distance(Global.Player) <= SpellManager.Q.Range + Global.Player.AttackRange);
            if (minion == null || !minion.IsValidTarget())
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
                    var inAaRange = _turretTarget.Distance(Global.Player) <= Global.Player.AttackRange + 115;

                    if (!inAaRange)
                    {
                        return;
                    }

                    if (_turretTarget.Health < playerDamage * 2 + turretDamage &&
                        _turretTarget.Health > turretDamage + playerDamage && Global.Orbwalker.CanAttack())
                    {
                        Global.Orbwalker.Attack(_turretTarget);
                    }

                    else if (SpellManager.Q.Ready && _turretTarget.Health < Global.Player.GetSpellDamage(_turretTarget, SpellSlot.Q) + playerDamage)
                    {
                        SpellManager.CastQ(_turretTarget);
                    }
                }
            }
            else if (SpellManager.Q.Ready && MenuConfig.LaneClear["Q"].Value == 1)
            {
                SpellManager.CastQ(minion, MenuConfig.LaneClear["QMode"].Value);
            }
        }

        public static void OnProcessSpellCast(Obj_AI_Base sender, Obj_AI_BaseMissileClientDataEventArgs args)
        {
            if (sender == null
                || args.Target == null
                || !sender.IsAlly
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
            }
            else
            {
                _turret = null;
                _turretTarget = null;
            }
        }
    }
}
