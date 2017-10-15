namespace Adept_AIO.Champions.LeeSin.OrbwalkingEvents.LaneClear
{
    using System.Linq;
    using Aimtec.SDK.Damage;
    using Aimtec.SDK.Extensions;
    using Aimtec.SDK.Util;
    using Core.Spells;
    using SDK.Unit_Extensions;
    using SDK.Usables;

    class LaneClear : ILaneClear
    {
        private readonly ISpellConfig _spellConfig;

        public LaneClear(ISpellConfig spellConfig) { _spellConfig = spellConfig; }

        public bool Q1Enabled { get; set; }
        public bool WEnabled { get; set; }
        public bool EEnabled { get; set; }
        public bool CheckEnabled { get; set; }

        public void OnPostAttack()
        {
            var minion = GameObjects.EnemyMinions.FirstOrDefault(x =>
                x.Distance(Global.Player) < Global.Player.AttackRange + x.BoundingRadius &&
                x.Health > Global.Player.GetAutoAttackDamage(x));

            if (minion == null || this.CheckEnabled && Global.Player.CountEnemyHeroesInRange(2000) >= 1)
            {
                return;
            }

            if (_spellConfig.E.Ready && this.EEnabled)
            {
                if (Items.CanUseTiamat())
                {
                    Items.CastTiamat(false);
                    DelayAction.Queue(50, () => _spellConfig.E.Cast(minion));
                }
                else
                {
                    _spellConfig.E.Cast(minion);
                }
            }
            else if (_spellConfig.W.Ready && this.WEnabled)
            {
                _spellConfig.W.CastOnUnit(Global.Player);
            }
        }

        public void OnUpdate()
        {
            if (_spellConfig.Q.Ready && this.Q1Enabled ||
                Global.Orbwalker.IsWindingUp ||
                this.CheckEnabled && Global.Player.CountEnemyHeroesInRange(2000) >= 1)
            {
                return;
            }

            var minion = GameObjects.EnemyMinions.FirstOrDefault(x =>
                x.Distance(Global.Player) <
                (Global.Player.IsUnderEnemyTurret() ? _spellConfig.Q.Range : _spellConfig.Q.Range / 2f));

            if (minion == null)
            {
                return;
            }

            _spellConfig.Q.Cast(minion);
        }
    }
}