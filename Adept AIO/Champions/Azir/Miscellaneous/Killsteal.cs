﻿namespace Adept_AIO.Champions.Azir.Miscellaneous
{
    using System.Linq;
    using Aimtec;
    using Aimtec.SDK.Damage;
    using Aimtec.SDK.Extensions;
    using Core;
    using SDK.Unit_Extensions;

    class Killsteal
    {
        public static void OnUpdate()
        {
            var target = GameObjects.EnemyHeroes.FirstOrDefault(x => x.IsValidTarget(SpellConfig.Q.Range));

            if (target == null || Global.Orbwalker.IsWindingUp || Global.Player.IsDead)
            {
                return;
            }

            if (SpellConfig.Q.Ready &&
                target.Health < Global.Player.GetSpellDamage(target, SpellSlot.Q) &&
                MenuConfig.Killsteal["Q"].Enabled)
            {
                SpellConfig.CastQ(target);
            }
            else if (SpellConfig.E.Ready &&
                     target.Health < Global.Player.GetSpellDamage(target, SpellSlot.E) &&
                     MenuConfig.Killsteal["E"].Enabled)
            {
                var nearest = SoldierManager.GetSoldierNearestTo(target.ServerPosition);
                if (nearest != Vector3.Zero)
                {
                    SpellConfig.E.Cast(nearest);
                }
            }
        }
    }
}