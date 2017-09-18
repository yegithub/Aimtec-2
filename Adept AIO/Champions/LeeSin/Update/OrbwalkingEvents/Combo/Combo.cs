using Adept_AIO.Champions.LeeSin.Core.Spells;
using Adept_AIO.Champions.LeeSin.Update.Ward_Manager;
using Adept_AIO.SDK.Junk;
using Adept_AIO.SDK.Usables;
using Aimtec;
using Aimtec.SDK.Damage;
using Aimtec.SDK.Damage.JSON;
using Aimtec.SDK.Extensions;
using Aimtec.SDK.Util;

namespace Adept_AIO.Champions.LeeSin.Update.OrbwalkingEvents.Combo
{
    internal class Combo : ICombo
    {
        public bool TurretCheckEnabled { get; set; }
        public bool Q1Enabled { get; set; }
        public bool Q2Enabled { get; set; }
        public bool WEnabled { get; set; }
        public bool WardEnabled { get; set; }
        public bool EEnabled { get; set; }

        private readonly IWardManager _wardManager;
        private readonly IWardTracker _wardTracker;
        private readonly ISpellConfig _spellConfig;
      
        public Combo(IWardManager wardManager, ISpellConfig spellConfig, IWardTracker wardTracker)
        {
            _wardManager = wardManager;
            _spellConfig = spellConfig;
            _wardTracker = wardTracker;
        }

        public void OnPostAttack(AttackableUnit target)
        {
            if (target == null)
            {
                return;
            }

            if (_spellConfig.W.Ready && WEnabled)
            {
                _spellConfig.W.Cast(Global.Player);
            }
            else if (_spellConfig.E.Ready && EEnabled)
            {
                if (!_spellConfig.IsFirst(_spellConfig.E))
                {
                    _spellConfig.E.Cast();
                }
            }
        }

        public void OnUpdate()
        {
            var target = Global.TargetSelector.GetTarget(1600);
            if (target == null)
            {
                return;
            }
        
            var distance = target.Distance(Global.Player);

            if (_spellConfig.Q.Ready && Q1Enabled)
            {
                if (distance > 1300)
                {
                    return;
                }

                if (_spellConfig.IsQ2())
                {
                    if (TurretCheckEnabled && target.IsUnderEnemyTurret() || !Q2Enabled)
                    {
                        return;
                    }

                    _spellConfig.Q.Cast();
                }
                else
                {
                    _spellConfig.QSmite(target);
                    _spellConfig.Q.Cast(target);
                }
            }

            if (_spellConfig.R.Ready && _spellConfig.Q.Ready && Q1Enabled && distance <= 550 && target.Health <= Global.Player.GetSpellDamage(target, SpellSlot.R) + 
                                                                                                               Global.Player.GetSpellDamage(target, SpellSlot.Q) + 
                                                                                                               Global.Player.GetSpellDamage(target, SpellSlot.Q, DamageStage.SecondCast))
            {
                _spellConfig.R.CastOnUnit(target);
                _spellConfig.Q.Cast(target); 
            }

          
            if (_spellConfig.W.Ready && _spellConfig.IsFirst(_spellConfig.W) && _wardTracker.IsWardReady() && WEnabled && WardEnabled && distance > (_spellConfig.Q.Ready ? 1000 : _spellConfig.WardRange))
            {
                if (Game.TickCount - _spellConfig.Q.LastCastAttemptT <= 3000 || target.Position.CountEnemyHeroesInRange(2000) > 1)
                {
                    return;
                }

                _wardManager.WardJump(target.Position, _spellConfig.WardRange);
            }

            if (_spellConfig.E.Ready && EEnabled && _spellConfig.IsFirst(_spellConfig.E) && distance <= 350)
            {
                if (Items.CanUseTiamat())
                {
                    Items.CastTiamat(false);
                    DelayAction.Queue(50, () => _spellConfig.E.Cast(target));
                }
                else
                {
                    _spellConfig.E.Cast(target);
                }
            }
        }
    }
}
