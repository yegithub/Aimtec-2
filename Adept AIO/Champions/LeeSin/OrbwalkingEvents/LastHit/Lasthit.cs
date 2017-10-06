using System.Linq;
using Adept_AIO.Champions.LeeSin.Core.Spells;
using Adept_AIO.SDK.Unit_Extensions;
using Aimtec;
using Aimtec.SDK.Damage;
using Aimtec.SDK.Extensions;

namespace Adept_AIO.Champions.LeeSin.OrbwalkingEvents.LastHit
{
    internal class Lasthit : ILasthit
    {
        public bool Enabled { get; set; }

        private readonly ISpellConfig _spellConfig;

        public Lasthit(ISpellConfig spellConfig)
        {
            _spellConfig = spellConfig;
        }

        public void OnUpdate()
        {
            if (!Enabled || Global.Orbwalker.IsWindingUp)
            {
                return;
            }

            var minions = GameObjects.EnemyMinions.LastOrDefault(x => x.IsValidTarget(_spellConfig.Q.Range) && x.Distance(Global.Player) > 300 &&  x.Health * 0.9 < Global.Player.GetSpellDamage(x, SpellSlot.Q) &&
                                                                                              x.MaxHealth > 6);
            if (minions == null || !_spellConfig.Q.Ready || _spellConfig.IsQ2())
            {
                return;
            }           
            _spellConfig.Q.Cast(minions);
        }
    }
}
