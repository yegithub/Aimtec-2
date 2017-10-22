namespace Adept_AIO.Champions.LeeSin.OrbwalkingEvents.Insec
{
    using System.Linq;
    using Aimtec;
    using Aimtec.SDK.Damage;
    using Aimtec.SDK.Events;
    using Aimtec.SDK.Extensions;
    using Core;
    using Core.Insec_Manager;
    using Core.Spells;
    using SDK.Unit_Extensions;
    using SDK.Usables;
    using Ward_Manager;

    class Insec : IInsec
    {
        private readonly IInsecManager _insecManager;
        private readonly ISpellConfig _spellConfig;
        private readonly IWardManager _wardManager;

        private readonly IWardTracker _wardTracker;

        private Obj_AI_Base _lastQUnit;

        private bool IsBKActive;

        public Insec(IWardTracker wardTracker, IWardManager wardManager, ISpellConfig spellConfig, IInsecManager insecManager)
        {
            _wardTracker = wardTracker;
            _wardManager = wardManager;
            _spellConfig = spellConfig;
            _insecManager = insecManager;
        }

        public bool FlashEnabled { get; set; }
        public bool Bk { get; set; }
        public bool QLast { get; set; }
        public bool ObjectEnabled { get; set; }

        private bool FlashReady => SummonerSpells.IsValid(SummonerSpells.Flash) && this.FlashEnabled;

        private bool CanWardJump => _spellConfig.W.Ready && _spellConfig.IsFirst(_spellConfig.W) && _wardTracker.IsWardReady();

        private static Obj_AI_Hero Target => Global.TargetSelector.GetSelectedTarget();

        private Obj_AI_Base EnemyObject => GameObjects.EnemyMinions.OrderBy(x => x.Health).
            LastOrDefault(x =>
                InsecInRange(x.ServerPosition) &&
                !x.IsDead &&
                x.IsValid &&
                !x.IsTurret &&
                x.NetworkId != Target.NetworkId &&
                x.Health * 0.9 > Global.Player.GetSpellDamage(x, SpellSlot.Q) &&
                x.MaxHealth > 7 &&
                Global.Player.Distance(x) <= _spellConfig.Q.Range &&
                x.Distance(GetInsecPosition()) < Global.Player.Distance(GetInsecPosition()));

        public bool Enabled { get; set; }

        // R Flash
        public void OnProcessSpellCast(Obj_AI_Base sender, Obj_AI_BaseMissileClientDataEventArgs args)
        {
            if (!this.Enabled ||
                !this.FlashReady ||
                sender == null ||
                !sender.IsMe ||
                _insecManager.InsecKickValue != 1 ||
                this.CanWardJump && !_wardTracker.DidJustWard ||
                _wardTracker.DidJustWard ||
                Global.Player.Distance(GetInsecPosition()) <= 220 ||
                Target == null ||
                args.SpellSlot != SpellSlot.R ||
                Global.Player.Distance(GetInsecPosition()) <= 80)
            {
                return;
            }

            SummonerSpells.Flash.Cast(GetInsecPosition());
        }

        public void OnKeyPressed()
        {
            if (!this.Enabled || !Target.IsValidTarget() || Global.Player.Level < 6)
            {
                return;
            }

            Temp.IsBubbaKush = this.Bk;

            var dist = GetInsecPosition().Distance(Global.Player);

            if (_spellConfig.Q.Ready && !(this.CanWardJump && dist <= _spellConfig.WardRange && this.QLast))
            {
                if (_spellConfig.IsQ2())
                {
                    _spellConfig.Q.Cast();
                }
                else if (!Global.Player.IsDashing())
                {
                    if (Target.IsValidTarget(_spellConfig.Q.Range))
                    {
                        _lastQUnit = Target;

                        _spellConfig.QSmite(Target);
                        _spellConfig.Q.Cast(Target);
                    }

                    if (!this.ObjectEnabled || this.EnemyObject == null)
                    {
                        return;
                    }

                    _lastQUnit = this.EnemyObject;
                    _spellConfig.Q.Cast(this.EnemyObject.ServerPosition);
                }
            }

            if (this.CanWardJump && dist <= InsecRange())
            {
                if (dist <= _spellConfig.WardRange)
                {
                    _wardManager.WardJump(GetInsecPosition(), (int) dist);
                }
                else
                {
                    if (Game.TickCount - _spellConfig.LastQ1CastAttempt <= 900 ||
                        _lastQUnit != null && _spellConfig.IsQ2() && InsecInRange(_lastQUnit.ServerPosition) ||
                        this.ObjectEnabled && _spellConfig.Q.Ready)
                    {
                        return;
                    }

                    if (!this.FlashReady || Game.TickCount - _spellConfig.Q.LastCastAttemptT <= 1000)
                    {
                        return;
                    }

                    _wardManager.WardJump(GetInsecPosition(), _spellConfig.WardRange);
                }
            }

            if (_spellConfig.R.Ready)
            {
                if (dist <= 125 || this.FlashReady)
                {
                    if (IsBKActive)
                    {
                        var enemy = GameObjects.EnemyHeroes.FirstOrDefault(x => x.IsValidTarget(_spellConfig.R.Range) && x.NetworkId != Target.NetworkId);
                        if (enemy != null)
                        {
                            _spellConfig.R.CastOnUnit(enemy);
                        }
                    }
                    else if (Target.IsValidTarget(_spellConfig.R.Range))
                    {
                        _spellConfig.R.CastOnUnit(Target);
                    }
                }

                if (_insecManager.InsecKickValue == 0 &&
                    this.FlashReady &&
                    GetInsecPosition().Distance(Global.Player) <= 425 &&
                    GetInsecPosition().Distance(Global.Player) > 220 &&
                    (!this.CanWardJump || _wardTracker.DidJustWard))
                {
                    if (Global.Player.GetDashInfo().EndPos.Distance(GetInsecPosition()) <= 215 || this.CanWardJump)
                    {
                        return;
                    }

                    SummonerSpells.Flash.Cast(GetInsecPosition());
                    _spellConfig.R.CastOnUnit(Target);
                }
            }
        }

        private int InsecRange()
        {
            var temp = 65;

            if (this.FlashReady)
            {
                temp += 425;
            }

            if (this.CanWardJump)
            {
                temp += _spellConfig.WardRange;
            }

            return temp;
        }

        private bool InsecInRange(Vector3 source) { return GetInsecPosition().Distance(source) <= InsecRange(); }

        private Vector3 GetInsecPosition()
        {
            if (this.Bk && _insecManager.BkPosition(Target) != Vector3.Zero)
            {
                IsBKActive = true;
                return _insecManager.BkPosition(Target);
            }
            IsBKActive = false;
            return _insecManager.InsecPosition(Target);
        }
    }
}