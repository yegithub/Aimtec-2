namespace Adept_AIO.Champions.LeeSin.OrbwalkingEvents.LastHit
{
    using System.Linq;
    using Aimtec;
    using Aimtec.SDK.Damage;
    using Aimtec.SDK.Extensions;
    using Core.Spells;
    using SDK.Unit_Extensions;

    class Lasthit : ILasthit
    {
        private readonly ISpellConfig _spellConfig;

        public Lasthit(ISpellConfig spellConfig)
        {
            _spellConfig = spellConfig;
        }

        public bool Enabled { get; set; }

        public void OnUpdate()
        {
            if (!this.Enabled || Global.Orbwalker.IsWindingUp)
            {
                return;
            }

            var minions = GameObjects.EnemyMinions.LastOrDefault(x => x.IsValidTarget(_spellConfig.Q.Range) &&
                                                                      x.Distance(Global.Player) > 300 &&
                                                                      x.Health * 0.9 < Global.Player.GetSpellDamage(x, SpellSlot.Q) &&
                                                                      x.MaxHealth > 6);
            if (minions == null || !_spellConfig.Q.Ready || _spellConfig.IsQ2())
            {
                return;
            }
            _spellConfig.Q.Cast(minions);
        }
    }
}