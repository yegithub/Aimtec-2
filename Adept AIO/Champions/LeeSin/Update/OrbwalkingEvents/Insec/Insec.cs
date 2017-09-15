using System.Linq;
using Adept_AIO.Champions.LeeSin.Core;
using Adept_AIO.Champions.LeeSin.Core.Insec_Manager;
using Adept_AIO.Champions.LeeSin.Core.Spells;
using Adept_AIO.Champions.LeeSin.Update.Ward_Manager;
using Adept_AIO.SDK.Junk;
using Adept_AIO.SDK.Usables;
using Aimtec;
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

        private int InsecRange()
        {
            var temp = 0;
            if (_wardTracker.IsWardReady() && _spellConfig.W.Ready && _spellConfig.IsFirst(_spellConfig.W))
            {
                temp += _spellConfig.WardRange;
            }

            if (FlashReady)
            {
                temp += 425;
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
        && !Mixed.KillableBySpell(x, _spellConfig.Q)
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

            if (Target == null
                || args.SpellSlot != SpellSlot.R
                || _wardTracker.DidJustWard && (Global.Player.GetDashInfo().EndPos.Distance(GetInsecPosition()) <= 200 || _wardTracker.WardPosition.Distance(GetInsecPosition()) <= 180)
                || _wardTracker.IsWardReady() && _spellConfig.IsFirst(_spellConfig.W) && _spellConfig.W.Ready
                ||  Global.Player.Distance(GetInsecPosition()) <= 100)
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

            if (_spellConfig.R.Ready)
            {
                if (Target.IsValidTarget(_spellConfig.R.Range) && (_wardTracker.DidJustWard || Game.TickCount - SummonerSpells.Flash.LastCastAttemptT <= 1000 || GetInsecPosition().Distance(Global.Player) <= 200))
                {
                    _spellConfig.R.CastOnUnit(Target);
                }

                if (_insecManager.InsecKickValue == 0 && FlashReady &&
                    GetInsecPosition().Distance(Global.Player) <= 500)
                {
                    if (_wardTracker.DidJustWard && (Global.Player.GetDashInfo().EndPos.Distance(GetInsecPosition()) <= 200 || _wardTracker.WardPosition.Distance(GetInsecPosition()) <= 180)
                     || GetInsecPosition().Distance(Global.Player) <= 150
                     || _spellConfig.W.Ready && _spellConfig.IsFirst(_spellConfig.W) && _wardTracker.IsWardReady())
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

            if (!_spellConfig.W.Ready 
             || !_spellConfig.IsFirst(_spellConfig.W) 
             || !_wardTracker.IsWardReady() 
             ||  Game.TickCount - _spellConfig.LastQ1CastAttempt <= 800)
            {
                return;
            }

            if (GetInsecPosition().Distance(Global.Player) <= _spellConfig.WardRange)   
            {
                _wardManager.WardJump(GetInsecPosition(), (int)GetInsecPosition().Distance(Global.Player));
            }

            if (!FlashReady || Global.Player.Distance(GetInsecPosition()) > InsecRange() || Game.TickCount - _spellConfig.Q.LastCastAttemptT <= 1000)
            {
                return;
            }

            _wardManager.WardJump(GetInsecPosition(), _spellConfig.WardRange);
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
                    if (Target.IsValidTarget(_spellConfig.W.Range) && _spellConfig.W.Ready && _spellConfig.IsFirst(_spellConfig.W) && _wardTracker.IsWardReady() && QLast)
                    {
                        return;
                    }

                    _spellConfig.QSmite(Target);
                    _spellConfig.Q.CastOnUnit(Target);
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