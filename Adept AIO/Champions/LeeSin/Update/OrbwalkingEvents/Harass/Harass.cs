using System.Linq;
using Adept_AIO.Champions.LeeSin.Core.Spells;
using Adept_AIO.Champions.LeeSin.Update.Ward_Manager;
using Adept_AIO.SDK.Extensions;
using Aimtec;
using Aimtec.SDK.Extensions;

namespace Adept_AIO.Champions.LeeSin.Update.OrbwalkingEvents.Harass
{
    internal class Harass : IHarass
    {
        public bool Q1Enabled { get; set; }
        public bool Q2Enabled { get; set; }
        public int Mode{ get; set; }
        public bool EEnabled { get; set; }
        public bool E2Enabled { get; set; }

        private readonly IWardManager _wardManager;
        private readonly ISpellConfig _spellConfig;

        public Harass(IWardManager wardManager, ISpellConfig spellConfig)
        {
            _wardManager = wardManager;
            _spellConfig = spellConfig;
        }

        public void OnPostAttack(AttackableUnit target)
        {
            if (target == null || !target.IsHero)
            {
                return;
            }
            if (_spellConfig.E.Ready && E2Enabled && !_spellConfig.IsFirst(_spellConfig.E))
            {
                _spellConfig.E.Cast();
            }
            else if (_spellConfig.W.Ready && Mode == 1)
            {
                _spellConfig.W.CastOnUnit(Global.Player);
            }
        }

        public void OnUpdate()
        {
            var target = Global.TargetSelector.GetTarget(_spellConfig.Q.Range);
            if (target == null)
            {
                return;
            }

            if (_spellConfig.Q.Ready && Q1Enabled)
            {
                if (_spellConfig.IsQ2() && Q2Enabled || !_spellConfig.IsQ2())
                {
                    _spellConfig.Q.Cast(target);
                }
            }

            if (_spellConfig.E.Ready)
            {
                if (_spellConfig.IsFirst(_spellConfig.E) && EEnabled && target.IsValidTarget(_spellConfig.E.Range))
                {
                    _spellConfig.E.Cast(target);
                }
            }

            if (_spellConfig.W.Ready && _spellConfig.IsFirst(_spellConfig.W) && !_spellConfig.E.Ready && !_spellConfig.Q.Ready && Mode == 0)
            {
                var turret = GameObjects.AllyTurrets.OrderBy(x => x.Distance(Global.Player)).FirstOrDefault();
                if (turret != null)
                {
                    _wardManager.WardJump(turret.ServerPosition, _spellConfig.WardRange);
                }
            }
        }
    }
}
