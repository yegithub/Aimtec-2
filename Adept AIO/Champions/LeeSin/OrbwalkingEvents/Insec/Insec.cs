using System.Linq;
using Adept_AIO.Champions.LeeSin.Core;
using Adept_AIO.Champions.LeeSin.Core.Insec_Manager;
using Adept_AIO.Champions.LeeSin.Core.Spells;
using Adept_AIO.Champions.LeeSin.Ward_Manager;
using Adept_AIO.SDK.Unit_Extensions;
using Adept_AIO.SDK.Usables;
using Aimtec;
using Aimtec.SDK.Damage;
using Aimtec.SDK.Events;
using Aimtec.SDK.Extensions;

namespace Adept_AIO.Champions.LeeSin.OrbwalkingEvents.Insec
{
    internal class Insec : IInsec
    {
        public bool Enabled { get; set; }
        public bool FlashEnabled { get; set; }
        public bool Bk { get; set; }
        public bool QLast { get; set; }
        public bool ObjectEnabled { get; set; }

        private readonly IWardTracker _wardTracker;
        private readonly IWardManager _wardManager;
        private readonly ISpellConfig _spellConfig;
        private readonly IInsecManager _insecManager;

        public Insec(IWardTracker wardTracker, IWardManager wardManager, ISpellConfig spellConfig,
            IInsecManager insecManager)
        {
            _wardTracker = wardTracker;
            _wardManager = wardManager;
            _spellConfig = spellConfig;
            _insecManager = insecManager;
        }

        private bool FlashReady => SummonerSpells.IsValid(SummonerSpells.Flash) && FlashEnabled;

        private bool CanWardJump => _spellConfig.W.Ready && _spellConfig.IsFirst(_spellConfig.W) && _wardTracker.IsWardReady();

        private bool IsBKActive;

        private int InsecRange()
        {
            var temp = 65;

            if (FlashReady)
            {
                temp += 425;
            }

            if (CanWardJump)
            {
                temp += _spellConfig.WardRange;
            }
          
            return temp;
        }

        private bool InsecInRange(Vector3 source)
        {
            return GetInsecPosition().Distance(source) <= InsecRange();
        }

        private Vector3 GetInsecPosition()
        {
            if (Bk && _insecManager.BkPosition(Target) != Vector3.Zero)
            {
                IsBKActive = true;
                return _insecManager.BkPosition(Target);
            }
            IsBKActive = false;
            return _insecManager.InsecPosition(Target);
        }

        private static Obj_AI_Hero Target => Global.TargetSelector.GetSelectedTarget();

        private Obj_AI_Base EnemyObject => GameObjects.EnemyMinions.OrderBy(x => x.Health).LastOrDefault(x => InsecInRange(x.ServerPosition) 
        && !x.IsDead 
        && x.IsValid
        && !x.IsTurret
        && x.NetworkId != Target.NetworkId 
        && x.Health * 0.9 > Global.Player.GetSpellDamage(x, SpellSlot.Q)
        && x.MaxHealth > 7
        && Global.Player.Distance(x) <= _spellConfig.Q.Range 
        && x.Distance(GetInsecPosition()) < Global.Player.Distance(GetInsecPosition()));

        private Obj_AI_Base _lastQUnit;

        // R Flash
        public void OnProcessSpellCast(Obj_AI_Base sender, Obj_AI_BaseMissileClientDataEventArgs args)
        {
            if (!Enabled
             || !FlashReady
             ||  sender == null 
             || !sender.IsMe
             ||  _insecManager.InsecKickValue != 1 
             ||  CanWardJump && !_wardTracker.DidJustWard
             ||  _wardTracker.DidJustWard
             || Global.Player.Distance(GetInsecPosition()) <= 220
             || Target== null
             || args.SpellSlot != SpellSlot.R
             || Global.Player.Distance(GetInsecPosition()) <= 80)
            {
                return;
            }

            SummonerSpells.Flash.Cast(GetInsecPosition());
        }

        public void OnKeyPressed()
        {
            if (!Enabled || Target== null || Global.Player.Level < 6)
            {
                return;
            }

            Temp.IsBubbaKush = Bk;

            var dist = GetInsecPosition().Distance(Global.Player);

            if (_spellConfig.Q.Ready && !(CanWardJump && dist <= _spellConfig.WardRange && QLast))
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

                    if (!ObjectEnabled || EnemyObject == null)
                    {
                        return;
                    }

                    _lastQUnit = EnemyObject;
                    _spellConfig.Q.Cast(EnemyObject.ServerPosition);
                }
            }

            if (CanWardJump && dist <= InsecRange())
            {
                if (dist <= _spellConfig.WardRange)
                {
                    _wardManager.WardJump(GetInsecPosition(), (int)dist);
                }
                else
                {
                    if (Game.TickCount - _spellConfig.LastQ1CastAttempt <= 900 ||
                        _lastQUnit != null && _spellConfig.IsQ2() && InsecInRange(_lastQUnit.ServerPosition) ||
                        ObjectEnabled && _spellConfig.Q.Ready)
                    {
                        return;
                    }

                    if (!FlashReady || Game.TickCount - _spellConfig.Q.LastCastAttemptT <= 1000)
                    {
                        return;
                    }

                    _wardManager.WardJump(GetInsecPosition(), _spellConfig.WardRange);
                }
            }

            if (_spellConfig.R.Ready)
            {
                if (dist <= 125 || FlashReady)
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

                if (_insecManager.InsecKickValue == 0
                    && FlashReady
                    && GetInsecPosition().Distance(Global.Player) <= 425
                    && GetInsecPosition().Distance(Global.Player) > 220
                    && (!CanWardJump || _wardTracker.DidJustWard))
                {
                    if (Global.Player.GetDashInfo().EndPos.Distance(GetInsecPosition()) <= 215 || CanWardJump)
                    {
                        return;
                    }

                    SummonerSpells.Flash.Cast(GetInsecPosition());
                    _spellConfig.R.CastOnUnit(Target);
                }
            }
        }
    }
}