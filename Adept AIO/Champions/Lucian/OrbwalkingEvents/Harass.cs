﻿namespace Adept_AIO.Champions.Lucian.OrbwalkingEvents
{
    using System.Linq;
    using Aimtec;
    using Aimtec.SDK.Extensions;
    using Aimtec.SDK.Orbwalking;
    using Core;
    using SDK.Unit_Extensions;

    class Harass
    {
        public static void PostAttack(object sender, PostAttackEventArgs args)
        {
            var target = args.Target as Obj_AI_Base;
            if (target != null && !target.IsHero)
            {
                return;
            }

            if (SpellManager.E.Ready && MenuConfig.Harass["E2"].Value == 0)
            {
                SpellManager.CastE(target, MenuConfig.Harass["Mode2"].Value);
            }
            else if (SpellManager.Q.Ready)
            {
                SpellManager.CastQ(target);
            }
            else if (SpellManager.W.Ready)
            {
                SpellManager.W.Cast(target);
            }
        }

        public static void OnUpdate()
        {
            var target = GameObjects.EnemyHeroes.FirstOrDefault(x => x.IsValidTarget(SpellManager.ExtendedRange));
            if (target != null)
            {
                if (SpellManager.E.Ready && MenuConfig.Harass["E2"].Value == 1)
                {
                    SpellManager.CastE(target, MenuConfig.Harass["Mode2"].Value);
                }
                else if (SpellManager.Q.Ready && !target.IsValidAutoRange())
                {
                    SpellManager.CastQExtended(target);
                }
            }
        }
    }
}