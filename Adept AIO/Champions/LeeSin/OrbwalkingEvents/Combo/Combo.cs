namespace Adept_AIO.Champions.LeeSin.OrbwalkingEvents.Combo
{
    using System;
    using Aimtec;
    using Aimtec.SDK.Damage;
    using Aimtec.SDK.Damage.JSON;
    using Aimtec.SDK.Extensions;
    using Aimtec.SDK.Util;
    using Core.Spells;
    using SDK.Unit_Extensions;
    using SDK.Usables;
    using Ward_Manager;

    class Combo : ICombo
    {
        private readonly ISpellConfig _spellConfig;

        private readonly IWardManager _wardManager;
        private readonly IWardTracker _wardTracker;

        public Combo(IWardManager wardManager, ISpellConfig spellConfig, IWardTracker wardTracker)
        {
            _wardManager = wardManager;
            _spellConfig = spellConfig;
            _wardTracker = wardTracker;
        }

        public bool TurretCheckEnabled { get; set; }
        public bool Q1Enabled { get; set; }
        public bool Q2Enabled { get; set; }
        public bool WEnabled { get; set; }
        public bool WardEnabled { get; set; }
        public bool EEnabled { get; set; }

        public void OnPostAttack(AttackableUnit target)
        {
            if (target == null)
            {
                return;
            }

            if (_spellConfig.Q.Ready && !_spellConfig.IsQ2() && target.IsValidTarget(_spellConfig.Q.Range))
            {
                _spellConfig.Q.Cast();
            }

            else if (_spellConfig.W.Ready && this.WEnabled)
            {
                _spellConfig.W.Cast(Global.Player);
            }
            else if (_spellConfig.E.Ready && this.EEnabled)
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
            if (!target.IsValidTarget())
            {
                return;
            }

            var distance = target.Distance(Global.Player);

            if (_spellConfig.Q.Ready && this.Q1Enabled)
            {
                if (distance > 1300)
                {
                    return;
                }

                if (_spellConfig.IsQ2())
                {
                    if (this.TurretCheckEnabled && target.IsUnderEnemyTurret() || !this.Q2Enabled)
                    {
                        return;
                    }

                    if (_spellConfig.QAboutToEnd || distance >= Global.Player.AttackRange + 100)
                    {
                        _spellConfig.Q.Cast();
                    }
                }
                else if (target.IsValidTarget(_spellConfig.Q.Range))
                {
                    _spellConfig.QSmite(target);
                    _spellConfig.Q.Cast(target);
                }
            }

            if (_spellConfig.R.Ready && _spellConfig.Q.Ready && this.Q1Enabled && distance <= 550 && target.Health <= Global.Player.GetSpellDamage(target, SpellSlot.R) +
                Global.Player.GetSpellDamage(target, SpellSlot.Q) + Global.Player.GetSpellDamage(target, SpellSlot.Q, DamageStage.SecondCast))
            {
                _spellConfig.R.CastOnUnit(target);
                _spellConfig.Q.Cast(target);
            }

            if (_spellConfig.E.Ready && this.EEnabled && _spellConfig.IsFirst(_spellConfig.E) && distance <= 350)
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

            if (_spellConfig.W.Ready && _spellConfig.IsFirst(_spellConfig.W) && _wardTracker.IsWardReady() && this.WEnabled && this.WardEnabled &&
                distance > (_spellConfig.Q.Ready ? 1000 : _spellConfig.WardRange))
            {
                if (Game.TickCount - _spellConfig.Q.LastCastAttemptT <= 3000 || target.Position.CountEnemyHeroesInRange(2000) > 1)
                {
                    return;
                }

                _wardManager.WardJump(target.Position, _spellConfig.WardRange);
            }
        }
    }
}