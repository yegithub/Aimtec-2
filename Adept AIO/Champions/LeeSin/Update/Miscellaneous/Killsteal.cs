using System.Linq;
using Adept_AIO.Champions.LeeSin.Core.Spells;
using Adept_AIO.SDK.Junk;
using Adept_AIO.SDK.Usables;
using Aimtec;
using Aimtec.SDK.Damage;
using Aimtec.SDK.Damage.JSON;
using Aimtec.SDK.Extensions;

namespace Adept_AIO.Champions.LeeSin.Update.Miscellaneous
{
    internal interface IKillsteal
    {
        void OnUpdate();
    }

    internal class Killsteal : IKillsteal
    {
        public bool IgniteEnabled { get; set; }
        public bool SmiteEnabled { get; set; }
        public bool QEnabled { get; set; }
        public bool EEnabled { get; set; }
        public bool REnabled { get; set; }

        private readonly ISpellConfig _spellConfig;

        public Killsteal(ISpellConfig spellConfig)
        {
            _spellConfig = spellConfig;
        }

        public void OnUpdate()
        {
            var target = GameObjects.EnemyHeroes.FirstOrDefault(x => x.Distance(Global.Player) < _spellConfig.R.Range && x.HealthPercent() <= 40);

            if (target == null || !target.IsValidTarget())
            {
                return;
            }

            if (SmiteEnabled && SummonerSpells.IsValid(SummonerSpells.Smite) && target.Health < SummonerSpells.SmiteChampions())
            {
                SummonerSpells.Smite.CastOnUnit(target);
            }
            if (_spellConfig.Q.Ready && (_spellConfig.IsQ2() ? target.Health < Global.Player.GetSpellDamage(target, SpellSlot.Q, DamageStage.SecondCast) : target.Health < Global.Player.GetSpellDamage(target, SpellSlot.Q)) &&
                target.IsValidTarget(_spellConfig.Q.Range) && QEnabled)
            {
                _spellConfig.Q.Cast(target);
            }
            else if (_spellConfig.E.Ready && target.Health < Global.Player.GetSpellDamage(target, SpellSlot.E) &&
                     target.IsValidTarget(_spellConfig.E.Range) && EEnabled)
            {
                _spellConfig.E.Cast();
            }
            else if (_spellConfig.R.Ready && target.Health < Global.Player.GetSpellDamage(target, SpellSlot.R) &&
                     target.IsValidTarget(_spellConfig.R.Range) && REnabled)
            {
                _spellConfig.R.CastOnUnit(target);
            }
            else if (IgniteEnabled && SummonerSpells.IsValid(SummonerSpells.Ignite) && target.Health < SummonerSpells.IgniteDamage(target))
            {
                SummonerSpells.Ignite.Cast(target);
            }
        }
    }
}
