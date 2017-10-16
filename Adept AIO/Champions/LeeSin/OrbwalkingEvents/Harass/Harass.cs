namespace Adept_AIO.Champions.LeeSin.OrbwalkingEvents.Harass
{
    using System.Linq;
    using Aimtec;
    using Aimtec.SDK.Extensions;
    using Core.Spells;
    using SDK.Unit_Extensions;
    using Ward_Manager;

    class Harass : IHarass
    {
        private readonly ISpellConfig _spellConfig;

        private readonly IWardManager _wardManager;

        public Harass(IWardManager wardManager, ISpellConfig spellConfig)
        {
            _wardManager = wardManager;
            _spellConfig = spellConfig;
        }

        public bool Q1Enabled { get; set; }
        public bool Q2Enabled { get; set; }
        public int Mode { get; set; }
        public bool EEnabled { get; set; }
        public bool E2Enabled { get; set; }

        public void OnPostAttack(AttackableUnit target)
        {
            if (target == null || !target.IsHero)
            {
                return;
            }
            if (_spellConfig.E.Ready && this.E2Enabled && !_spellConfig.IsFirst(_spellConfig.E))
            {
                _spellConfig.E.Cast();
            }
            else if (_spellConfig.W.Ready && this.Mode == 1)
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

            if (_spellConfig.Q.Ready && this.Q1Enabled)
            {
                if (_spellConfig.IsQ2() && this.Q2Enabled || !_spellConfig.IsQ2())
                {
                    _spellConfig.Q.Cast(target);
                }
            }

            if (_spellConfig.E.Ready)
            {
                if (_spellConfig.IsFirst(_spellConfig.E) && this.EEnabled && target.IsValidTarget(_spellConfig.E.Range))
                {
                    _spellConfig.E.Cast(target);
                }
            }

            if (_spellConfig.W.Ready && _spellConfig.IsFirst(_spellConfig.W) && !_spellConfig.E.Ready && !_spellConfig.Q.Ready && this.Mode == 0)
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