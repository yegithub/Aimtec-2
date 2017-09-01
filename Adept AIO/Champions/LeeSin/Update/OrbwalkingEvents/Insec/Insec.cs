using System;
using System.Linq;
using Adept_AIO.Champions.LeeSin.Core;
using Adept_AIO.Champions.LeeSin.Core.Insec_Manager;
using Adept_AIO.Champions.LeeSin.Core.Spells;
using Adept_AIO.Champions.LeeSin.Update.Ward_Manager;
using Adept_AIO.SDK.Extensions;
using Adept_AIO.SDK.Usables;
using Aimtec;
using Aimtec.SDK.Damage;
using Aimtec.SDK.Extensions;

namespace Adept_AIO.Champions.LeeSin.Update.OrbwalkingEvents.Insec
{
    internal class Insec : IInsec
    {
        public bool Enabled { get; set; } 
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

        private bool _wardFlash;

        private static bool FlashReady => SummonerSpells.IsValid(SummonerSpells.Flash);

        private bool CanWardJump(Vector3 source)
        {
            var temp = 0;
            if (_wardManager.IsWardReady() && _spellConfig.W.Ready && _spellConfig.IsFirst(_spellConfig.W))
            {
                temp += 600;
            }

            if (FlashReady)
            {
                temp += 425;
            }
            return GetInsecPosition().Distance(source) <= temp;
        }

        private Vector3 GetInsecPosition()
        {
            if (Bk && !_insecManager.BKPosition(Target).IsZero)
            {
                return _insecManager.BKPosition(Target);
            }
            return _insecManager.InsecPosition(Target);
        }

        private static Obj_AI_Hero Target => Global.TargetSelector.GetSelectedTarget();

        private Obj_AI_Base EnemyObject => ObjectManager.Get<Obj_AI_Base>().Where(x =>
                CanWardJump(x.ServerPosition) && x.IsEnemy && !x.IsDead &&
                x.Health > Global.Player.GetSpellDamage(x, SpellSlot.Q) && x.MaxHealth > 7 &&
                Global.Player.Distance(x) <= _spellConfig.Q.Range + 600)
            .OrderBy(x => x.Distance(GetInsecPosition()))
            .FirstOrDefault(x => x.Distance(GetInsecPosition()) < Global.Player.Distance(GetInsecPosition()));

        public void OnProcessSpellCast(Obj_AI_Base sender, Obj_AI_BaseMissileClientDataEventArgs args)
        {
            if (!Enabled || sender == null || !sender.IsMe || _insecManager.InsecKickValue != 1)
            {
                return;
            }

            if (Target == null
                || args.SpellSlot != SpellSlot.R
                || _wardManager.IsWardReady() && _spellConfig.IsFirst(_spellConfig.W)
                || FlashReady && Global.Player.Distance(GetInsecPosition()) <= 125
                || Game.TickCount - _wardTracker.LastWardCreated < 1200 && !_wardFlash)
            {
                return;
            }

            SummonerSpells.Flash.Cast(GetInsecPosition());
        }

        public void OnKeyPressed()
        {
            Temp.IsBubbaKush = Bk;

            if (Target == null || !Enabled)
            {
                return;
            }

            if (_spellConfig.Q.Ready && !CanWardJump(Global.Player.ServerPosition))
            {
                Q();
            }

            if (_spellConfig.W.Ready && _spellConfig.IsFirst(_spellConfig.W) && _wardManager.IsWardReady())
            {
                if (Game.TickCount - _spellConfig.Q.LastCastAttemptT <= 600 && !_spellConfig.Q.Ready)
                {
                    return; // Avoids Q2 -> Wardjump cancelling Q2 mid air
                }

                if (GetInsecPosition().Distance(Global.Player) <= 600)
                {
                    var dist = (int) GetInsecPosition().Distance(Global.Player);

                    _insecManager.InsecWPosition = Global.Player.ServerPosition.Extend(GetInsecPosition(), dist);
                    _wardManager.WardJump(GetInsecPosition(), dist);
                    _wardFlash = false;
                }
                else if (FlashReady && Game.TickCount - _lastQ1TickCount > 1500 && !_spellConfig.HasQ2(Target))
                {
                    if (Game.TickCount - _spellConfig.LastQ1CastAttempt >= 1000 &&
                        GetInsecPosition().Distance(Global.Player) <= _spellConfig.Q.Range + 400)
                    {
                        if (EnemyObject != null && _spellConfig.HasQ2(EnemyObject) &&
                            GetInsecPosition().Distance(EnemyObject.ServerPosition) <= 600 ||
                            _spellConfig.HasQ2(Target) && GetInsecPosition().Distance(Target.ServerPosition) <= 600)
                        {
                            return;
                        }


                        _insecManager.InsecWPosition = Global.Player.ServerPosition.Extend(GetInsecPosition(), 600);

                        _wardManager.WardJump(GetInsecPosition(), 600);
                        _wardFlash = true;
                    }
                }
            }

            if (!_spellConfig.R.Ready)
            {
                return;
            }

            if (_insecManager.InsecKickValue == 0 && FlashReady && GetInsecPosition().Distance(Global.Player) <= 425 &&
                !(Game.TickCount - _wardTracker.LastWardCreated <= 500 && !_wardFlash))
            {
                SummonerSpells.Flash?.Cast(GetInsecPosition());
            }

            if (!Target.IsValidTarget(_spellConfig.R.Range) || Global.Player.Distance(GetInsecPosition()) >=
                (SummonerSpells.IsValid(SummonerSpells.Flash) ? 425 : 150))
            {
                return;
            }

            _spellConfig.R.CastOnUnit(Target);
        }

        private int _lastQ1TickCount;

        private void Q()
        {
            if (_spellConfig.IsQ2() && Game.TickCount - _spellConfig.LastQ1CastAttempt >= 500)
            {
                _spellConfig.Q.Cast();
            }
            else if (!_spellConfig.IsQ2())
            {
                if (Target.IsValidTarget(_spellConfig.Q.Range + 600))
                {
                    if (_spellConfig.W.Ready && _spellConfig.IsFirst(_spellConfig.W) && _wardManager.IsWardReady() &&
                        QLast)
                    {
                        return;
                    }

                    _insecManager.InsecQPosition = Target.ServerPosition;

                    if (!Target.IsValidTarget(_spellConfig.Q.Range))
                    {
                        return;
                    }

                    _spellConfig.QSmite(Target);
                    _spellConfig.Q.Cast(Target.ServerPosition);
                    _lastQ1TickCount = Game.TickCount;
                }

                if (!ObjectEnabled || !_spellConfig.R.Ready || EnemyObject == null)
                {
                    return;
                }

                if (EnemyObject.Distance(GetInsecPosition()) <= 600)
                {
                    _lastQ1TickCount = Game.TickCount;
                }

                _insecManager.InsecQPosition = EnemyObject.ServerPosition;

                if (!EnemyObject.IsValidTarget(_spellConfig.Q.Range))
                {
                    return;
                }

                _spellConfig.Q.Cast(EnemyObject.ServerPosition);
            }
        }
    }
}