using System.Linq;
using Adept_AIO.Champions.LeeSin.Core;
using Adept_AIO.Champions.LeeSin.Core.Insec_Manager;
using Adept_AIO.Champions.LeeSin.Core.Spells;
using Adept_AIO.Champions.LeeSin.Update.Ward_Manager;
using Adept_AIO.SDK.Junk;
using Adept_AIO.SDK.Usables;
using Aimtec;
using Aimtec.SDK.Damage;
using Aimtec.SDK.Events;
using Aimtec.SDK.Extensions;

namespace Adept_AIO.Champions.LeeSin.Update.OrbwalkingEvents.Insec
{
    internal class Insec : IInsec
    {
        public bool Enabled { get; set; }
        public bool FlashEnabled { get; set; }
        public bool Bk { get; set; } = false;
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

        private int InsecRange()
        {
            var temp = 0;

            if (FlashReady)
            {
                temp += 425;
            }

            if (_wardTracker.IsWardReady() && _spellConfig.W.Ready && _spellConfig.IsFirst(_spellConfig.W))
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
                return _insecManager.BkPosition(Target);
            }
            return _insecManager.InsecPosition(Target);
        }

        private static Obj_AI_Hero Target => Global.TargetSelector.GetSelectedTarget();

        private Obj_AI_Base EnemyObject => GameObjects.Enemy.FirstOrDefault(x => InsecInRange(x.ServerPosition) 
        && !x.IsDead 
        && x.IsValid
        && x.NetworkId != Target.NetworkId 
        && x.Health * 0.9 > Global.Player.GetSpellDamage(x, SpellSlot.Q)
        && x.MaxHealth > 7
        && Global.Player.Distance(x) <= _spellConfig.Q.Range 
        && x.Distance(GetInsecPosition()) < Global.Player.Distance(GetInsecPosition()));

        // R Flash
        public void OnProcessSpellCast(Obj_AI_Base sender, Obj_AI_BaseMissileClientDataEventArgs args)
        {
            if (!Enabled || !FlashReady || sender == null || !sender.IsMe || _insecManager.InsecKickValue != 1)
            {
                return;
            }

            if (CanWardJump || _wardTracker.DidJustWard && Global.Player.Distance(GetInsecPosition()) < 175)
            {
                return;
            }

            if (Target == null
                || args.SpellSlot != SpellSlot.R
                ||  Global.Player.Distance(GetInsecPosition()) <= 80)
            {
                return;
            }

            SummonerSpells.Flash.Cast(GetInsecPosition());
        }

        public void OnKeyPressed()
        {
            if (Target == null || !Enabled)
            {
                return;
            }

            Temp.IsBubbaKush = Bk;

            var dist = GetInsecPosition().Distance(Global.Player);
          
            if (_spellConfig.R.Ready)
            {
                if (Target.IsValidTarget(_spellConfig.R.Range) && (dist <= 150 || FlashReady && _insecManager.InsecKickValue == 1))
                {
                    _spellConfig.R.CastOnUnit(Target);
                }

                if (_insecManager.InsecKickValue == 0 && FlashReady &&
                    GetInsecPosition().Distance(Global.Player) <= 500)
                {
                    if (Global.Player.GetDashInfo().EndPos.Distance(GetInsecPosition()) <= 100
                     || GetInsecPosition().Distance(Global.Player) <= 100
                     || CanWardJump
                    )
                    {
                        return;
                    }

                    SummonerSpells.Flash.Cast(GetInsecPosition());
                    _spellConfig.R.CastOnUnit(Target);
                } 
            }

            if (_spellConfig.Q.Ready)
            {
                Q();
            }

            if (!CanWardJump
             || _spellConfig.Q.Ready && !_spellConfig.IsQ2() && EnemyObject != null && EnemyObject.IsValidTarget(_spellConfig.Q.Range)
             || Game.TickCount - _spellConfig.LastQ1CastAttempt <= 500)
            {
                return;
            }
        
            if (dist < _spellConfig.WardRange)   
            {
                _wardManager.WardJump(GetInsecPosition(), (int)dist);
            }
            else if (dist <= InsecRange())
            {
                if (!FlashReady || Game.TickCount - _spellConfig.Q.LastCastAttemptT <= 1000)
                {
                    return;
                }
             
                _wardManager.WardJump(GetInsecPosition(), _spellConfig.WardRange);
            }
        }

        private void Q()
        {
            if (_spellConfig.IsQ2())
            {
                _spellConfig.Q.Cast();
            }
            else 
            {
                if(InsecInRange(Global.Player.ServerPosition) && _wardTracker.IsWardReady() && !_wardTracker.DidJustWard && _spellConfig.W.Ready && _spellConfig.IsFirst(_spellConfig.W))
                {
                    return;
                }

                if (Target.IsValidTarget(_spellConfig.Q.Range))
                {
                    if (GetInsecPosition().Distance(Global.Player) <= InsecRange() && _spellConfig.W.Ready && _spellConfig.IsFirst(_spellConfig.W) && _wardTracker.IsWardReady() && QLast)
                    {
                        return;
                    }

                    if (_spellConfig.Q.GetPrediction(Target).CollisionObjects.Count == 1)
                    {
                        _spellConfig.QSmite(Target);
                    }
                    _spellConfig.Q.Cast(Target);
                }

                if (!ObjectEnabled || EnemyObject == null)
                {
                    return;
                }

                _spellConfig.Q.Cast(EnemyObject.ServerPosition);
            }
        }
    }
}