namespace Adept_AIO.Champions.LeeSin.Miscellaneous
{
    using System.Linq;
    using Aimtec;
    using Aimtec.SDK.Damage;
    using Aimtec.SDK.Damage.JSON;
    using Aimtec.SDK.Extensions;
    using Core.Spells;
    using SDK.Unit_Extensions;
    using SDK.Usables;

    interface IKillsteal
    {
        void OnUpdate();
    }

    class Killsteal : IKillsteal
    {
        private readonly ISpellConfig _spellConfig;

        public Killsteal(ISpellConfig spellConfig)
        {
            _spellConfig = spellConfig;
        }

        public bool IgniteEnabled { get; set; }
        public bool SmiteEnabled { get; set; }
        public bool QEnabled { get; set; }
        public bool EEnabled { get; set; }
        public bool REnabled { get; set; }

        public void OnUpdate()
        {
            var target = GameObjects.EnemyHeroes.FirstOrDefault(x => x.Distance(Global.Player) < _spellConfig.R.Range && x.HealthPercent() <= 40);

            if (target == null || !target.IsValidTarget())
            {
                return;
            }

            if (this.SmiteEnabled && SummonerSpells.IsValid(SummonerSpells.Smite) && target.Health < SummonerSpells.SmiteChampions())
            {
                SummonerSpells.Smite.CastOnUnit(target);
            }
            if (_spellConfig.Q.Ready &&
                (_spellConfig.IsQ2() ? target.Health < Global.Player.GetSpellDamage(target, SpellSlot.Q, DamageStage.SecondCast) : target.Health < Global.Player.GetSpellDamage(target, SpellSlot.Q)) &&
                target.IsValidTarget(_spellConfig.Q.Range) &&
                this.QEnabled)
            {
                _spellConfig.Q.Cast(target);
            }
            else if (_spellConfig.E.Ready && target.Health < Global.Player.GetSpellDamage(target, SpellSlot.E) && target.IsValidTarget(_spellConfig.E.Range) && this.EEnabled)
            {
                _spellConfig.E.Cast();
            }
            else if (_spellConfig.R.Ready && target.Health < Global.Player.GetSpellDamage(target, SpellSlot.R) && target.IsValidTarget(_spellConfig.R.Range) && this.REnabled)
            {
                _spellConfig.R.CastOnUnit(target);
            }
            else if (this.IgniteEnabled && SummonerSpells.IsValid(SummonerSpells.Ignite) && target.Health < SummonerSpells.IgniteDamage(target))
            {
                SummonerSpells.Ignite.Cast(target);
            }
        }
    }
}