using System.Linq;
using Adept_AIO.Champions.LeeSin.Core.Spells;
using Adept_AIO.Champions.LeeSin.Update.Ward_Manager;
using Adept_AIO.SDK.Unit_Extensions;
using Adept_AIO.SDK.Usables;
using Aimtec;
using Aimtec.SDK.Damage;
using Aimtec.SDK.Damage.JSON;
using Aimtec.SDK.Extensions;
using Aimtec.SDK.Util;

namespace Adept_AIO.Champions.LeeSin.Update.OrbwalkingEvents.JungleClear
{
    internal class JungleClear : IJungleClear
    {
        public bool StealEnabled { get; set; }
        public bool SmiteEnabled { get; set; }
        public bool BlueEnabled { get; set; }
        public bool Q1Enabled { get; set; }
        public bool WEnabled { get; set; }
        public bool EEnabled { get; set; }

        private readonly IWardManager _wardManager;
        private readonly ISpellConfig _spellConfig;

        public JungleClear(IWardManager wardManager, ISpellConfig spellConfig)
        {
            _wardManager = wardManager;
            _spellConfig = spellConfig;
        }

        public void OnPostAttack(AttackableUnit mobPre)
        {
            var mob = mobPre as Obj_AI_Minion;

            if (mob == null)
            {
                return;
            }

            var count = GameObjects.Jungle.Count(x => x.Distance(Global.Player) <= _spellConfig.Q.Range / 2f);

            if (count <= 1 && mob.Health < Global.Player.GetAutoAttackDamage(mob))
            {
                return;
            }

            if (Global.Player.Level <= 4)
            {
                if (_spellConfig.PassiveStack() >= 1)
                {
                    return;
                }

                if (_spellConfig.Q.Ready)
                {
                    _spellConfig.Q.Cast(mob);
                }

                if (_spellConfig.W.Ready && WEnabled && !_spellConfig.IsQ2())
                {
                    _spellConfig.W.CastOnUnit(Global.Player);
                }
                else if (_spellConfig.E.Ready && EEnabled && !_spellConfig.IsQ2())
                {
                    if (_spellConfig.IsFirst(_spellConfig.E))
                    {
                        if (Items.CanUseTiamat())
                        {
                            Items.CastTiamat(false);
                            DelayAction.Queue(50, () => _spellConfig.E.Cast(mob));
                        }
                        else
                        {
                            _spellConfig.E.Cast(mob);
                        }
                    }
                    else
                    {
                        _spellConfig.E.Cast();
                    }
                }
            }
            else if (Global.Player.Level <= 8)
            {
                if (_spellConfig.PassiveStack() >= 1)
                {
                    return;
                }

                if (_spellConfig.Q.Ready)
                {
                    _spellConfig.Q.Cast(mob);
                }

                if (_spellConfig.W.Ready && WEnabled)
                {
                    _spellConfig.W.CastOnUnit(Global.Player);
                }
                else if (_spellConfig.E.Ready && EEnabled)
                {
                    if (_spellConfig.IsFirst(_spellConfig.E))
                    {
                        if (Items.CanUseTiamat())
                        {
                            Items.CastTiamat(false);
                            DelayAction.Queue(50, () => _spellConfig.E.Cast(mob));
                        }
                        else
                        {
                            _spellConfig.E.Cast(mob);
                        }
                    }
                    else
                    {
                        _spellConfig.E.Cast();
                    }
                }
            }
            else 
            {
                if (_spellConfig.Q.Ready)
                {
                    _spellConfig.Q.Cast(mob);
                }
                else
                {
                    if (_spellConfig.E.Ready && EEnabled)
                    {
                        if (Items.CanUseTiamat())
                        {
                            Items.CastTiamat(false);
                            DelayAction.Queue(50, () => _spellConfig.E.Cast(mob));
                        }
                        else
                        {
                            _spellConfig.E.Cast(mob);
                        }
                    }

                    if (_spellConfig.W.Ready && WEnabled)
                    {
                        if (_spellConfig.E.Ready && EEnabled && !_spellConfig.IsFirst(_spellConfig.E))
                        {
                            return;
                        }
                        _spellConfig.W.CastOnUnit(Global.Player);
                    }
                }
            }
        }

        public void OnUpdate()
        {
            if (!_spellConfig.Q.Ready || !Q1Enabled)
            {
                return;
            }

            var mob = ObjectManager.Get<Obj_AI_Minion>().FirstOrDefault(x => x.Distance(Global.Player) < _spellConfig.Q.Range / 2 && x.GetJungleType() != GameObjects.JungleType.Unknown && x.MaxHealth > 7);

            if (mob == null)
            {
                return;
            }

            if (_spellConfig.Q.Ready && _spellConfig.IsQ2() && (_spellConfig.QAboutToEnd || Global.Player.GetSpellDamage(mob, SpellSlot.Q, DamageStage.SecondCast) + Global.Player.GetAutoAttackDamage(mob) > mob.Health))
            {
                _spellConfig.Q.CastOnUnit(mob);
            }

            if (!_smiteOptional.Contains(mob.UnitSkinName) && !_smiteAlways.Contains(mob.UnitSkinName))
            {
                return;
            }

            if (_spellConfig.Q.Ready && _spellConfig.IsQ2() && mob.Health < Global.Player.GetSpellDamage(mob, SpellSlot.Q, DamageStage.SecondCast))
            {
                Global.Player.SpellBook.CastSpell(SpellSlot.Q);
            }

            if (!_spellConfig.IsQ2() && Game.TickCount - _spellConfig.LastQ1CastAttempt > 500)
            {
                Global.Player.SpellBook.CastSpell(SpellSlot.Q, mob.Position);
            }
        }

        private readonly Vector3[] _positions =
        {
            new Vector3(5740, 56, 10629),
            new Vector3(5808, 54, 10319),
            new Vector3(5384, 57, 11282),
            new Vector3(9076, 53, 4446),
            new Vector3(9058, 53, 4117),
            new Vector3(9687, 56, 3490)
        };

        private double StealDamage(Obj_AI_Base mob)
        {
           return SummonerSpells.SmiteMonsters() + (_spellConfig.IsQ2() ? Global.Player.GetSpellDamage(mob, SpellSlot.Q, DamageStage.SecondCast) : 0);
        }

        private readonly string[] _smiteAlways   = { "SRU_Dragon_Air", "SRU_Dragon_Fire", "SRU_Dragon_Earth", "SRU_Dragon_Water", "SRU_Dragon_Elder", "SRU_Baron", "SRU_RiftHerald" };
        private readonly string[] _smiteOptional = {"Sru_Crab", "SRU_Razorbeak", "SRU_Krug", "SRU_Murkwolf", "SRU_Gromp", "SRU_Blue", "SRU_Red"};
        private float _q2Time;

        public void SmiteMob()
        {
            var smiteAbleMob = ObjectManager.Get<Obj_AI_Minion>().FirstOrDefault(x => x.Distance(Global.Player) < 1300);

            if (smiteAbleMob != null && (_smiteAlways.Contains(smiteAbleMob.UnitSkinName) || _smiteOptional.Contains(smiteAbleMob.UnitSkinName)))
            {
                if (smiteAbleMob.Health < StealDamage(smiteAbleMob))
                {
                    if (_smiteOptional.Contains(smiteAbleMob.UnitSkinName) && Global.Player.HealthPercent() >= (SummonerSpells.Ammo("Smite") <= 1 ? 40 : 50) || 
                        smiteAbleMob.UnitSkinName.ToLower().Contains("blue") && !BlueEnabled)
                    {
                        return;
                    }

                    if (SmiteEnabled && SummonerSpells.IsValid(SummonerSpells.Smite))
                    {
                        SummonerSpells.Smite.CastOnUnit(smiteAbleMob);
                    }

                    if (_spellConfig.IsQ2() && _spellConfig.Q.Ready)
                    {
                        _spellConfig.Q.Cast();
                    }
                }
            }

            var mob = GameObjects.JungleLegendary.FirstOrDefault(x => x.Distance(Global.Player) <= 1500);
          
            if (mob == null || !SmiteEnabled)
            {
                return;
            }
          
            if (_q2Time > 0 && Game.TickCount - _q2Time <= 1500 && SummonerSpells.IsValid(SummonerSpells.Smite) && StealDamage(mob) > mob.Health)
            {
                if (_spellConfig.W.Ready && _spellConfig.IsFirst(_spellConfig.W) && Global.Player.Distance(mob) <= 500)
                {
                    SummonerSpells.Smite.CastOnUnit(mob);
                    _wardManager.WardJump(_positions.FirstOrDefault(), (int)mob.Distance(Global.Player));
                }
            }

            if (mob.Position.CountAllyHeroesInRange(700) <= 1 && _spellConfig.Q.Ready && _spellConfig.IsQ2() && StealDamage(mob) > mob.Health)
            {
                _spellConfig.Q.Cast();
                _q2Time = Game.TickCount;
            }
        }
    }
}
