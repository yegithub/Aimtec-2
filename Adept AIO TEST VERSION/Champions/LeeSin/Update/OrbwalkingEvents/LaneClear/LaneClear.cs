﻿using System.Linq;
using Adept_AIO.Champions.LeeSin.Core.Spells;
using Adept_AIO.SDK.Extensions;
using Aimtec;
using Aimtec.SDK.Damage;
using Aimtec.SDK.Extensions;

namespace Adept_AIO.Champions.LeeSin.Update.OrbwalkingEvents.LaneClear
{
    internal class LaneClear : ILaneClear
    {
        public bool Q1Enabled { get; set; }
        public bool WEnabled { get; set; }
        public bool EEnabled { get; set; }
        public bool CheckEnabled { get; set; }

        private readonly ISpellConfig SpellConfig;

        public LaneClear(ISpellConfig spellConfig)
        {
            SpellConfig = spellConfig;
        }

        public void OnPostAttack()
        {
            var minion = GameObjects.EnemyMinions.FirstOrDefault(x => x.Distance(GlobalExtension.Player) < GlobalExtension.Player.AttackRange + x.BoundingRadius &&
                                                                      x.Health > GlobalExtension.Player.GetAutoAttackDamage(x));

            if (minion == null || !CheckEnabled && GlobalExtension.Player.CountEnemyHeroesInRange(2000) >= 1)
            {
                return;
            }

            if (SpellConfig.E.Ready && EEnabled && minion.Health < GlobalExtension.Player.GetSpellDamage(minion, SpellSlot.E))
            {
                SpellConfig.E.Cast(minion);
            }
            else if (SpellConfig.W.Ready && WEnabled)
            {
                SpellConfig.W.CastOnUnit(GlobalExtension.Player);
            }
        }

        public void OnUpdate()
        {
            var minion = GameObjects.EnemyMinions.FirstOrDefault(x => x.Distance(GlobalExtension.Player) < SpellConfig.Q.Range + x.BoundingRadius);

            if (minion == null || !CheckEnabled && GlobalExtension.Player.CountEnemyHeroesInRange(2000) >= 1)
            {
                return;
            }

            if (SpellConfig.Q.Ready && Q1Enabled)
            {
                SpellConfig.Q.Cast(minion);
            }
        }
    }
}