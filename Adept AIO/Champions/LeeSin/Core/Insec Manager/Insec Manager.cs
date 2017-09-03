using System;
using System.Linq;
using Adept_AIO.Champions.LeeSin.Core.Spells;
using Adept_AIO.SDK.Extensions;
using Aimtec;
using Aimtec.SDK.Extensions;

namespace Adept_AIO.Champions.LeeSin.Core.Insec_Manager
{
    internal class InsecManager : IInsecManager
    {
        public int InsecKickValue { get; set; }
        public int InsecPositionValue { get; set; }

        private readonly ISpellConfig _spellConfig;

        public InsecManager(ISpellConfig spellConfig)
        {
            _spellConfig = spellConfig;
        }

        public float DistanceBehindTarget(Obj_AI_Base target = null)
        {
            return Math.Min((Global.Player.BoundingRadius + (target == null ? 65 : target.BoundingRadius) + 50) * 1.25f, _spellConfig.R.Range);
        }

        public Vector3 InsecPosition(Obj_AI_Base target)
        {
            return target.ServerPosition + (target.ServerPosition - GetTargetEndPosition()).Normalized() *
                   DistanceBehindTarget(target);
        }

        public Vector3 BkPosition(Obj_AI_Base target)
        {
            if (target == null || GameObjects.EnemyHeroes.Count() < 2)
            {
                return Vector3.Zero;
            }

            var secondEnemy = GameObjects.EnemyHeroes.FirstOrDefault(x => x.NetworkId != target.NetworkId && x.IsValid && x.Distance(target) <= _spellConfig.R2.Range);

            if (secondEnemy == null)
            {
                return Vector3.Zero;
            }

            var pred = _spellConfig.R2.GetPrediction(secondEnemy, target.ServerPosition, target.ServerPosition).UnitPosition;
            var final = target.ServerPosition.Extend(target.ServerPosition + (target.ServerPosition - pred).Normalized(), DistanceBehindTarget());
            return final;
        }

        public Vector3 GetTargetEndPosition()
        {
            var ally = GameObjects.AllyHeroes.FirstOrDefault(x => x.Distance(Global.Player) <= 2000);
            var turret = GameObjects.AllyTurrets.OrderBy(x => x.Distance(Global.Player)).FirstOrDefault();

            switch (InsecPositionValue)
            {
                case 0:
                    if (turret != null)
                    {
                        Temp.IsAlly = false;
                        return turret.ServerPosition;
                    }
                    else if (ally != null)
                    {
                        Temp.IsAlly = true;
                        return ally.ServerPosition;
                    }
                    break;
                case 1:
                    if (ally != null)
                    {
                        Temp.IsAlly = true;
                        return ally.ServerPosition;
                    }
                    else if (turret != null)
                    {
                        Temp.IsAlly = false;
                        return turret.ServerPosition;
                    }
                    break;
            }
            return Vector3.Zero;
        }
    }
}
