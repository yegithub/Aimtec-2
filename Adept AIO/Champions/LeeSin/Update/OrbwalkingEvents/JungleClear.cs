using System;
using System.Collections.Generic;
using System.Linq;
using Adept_AIO.Champions.LeeSin.Core;
using Adept_AIO.SDK.Extensions;
using Adept_AIO.SDK.Usables;
using Aimtec;
using Aimtec.SDK.Damage;
using Aimtec.SDK.Damage.JSON;
using Aimtec.SDK.Events;
using Aimtec.SDK.Extensions;

namespace Adept_AIO.Champions.LeeSin.Update.OrbwalkingEvents
{
    internal class JungleClear
    {
        private static int ShittyHelper;

        public static void OnPostAttack(AttackableUnit mob)
        {
            if (mob == null)
            {
                return;
            }

            ShittyHelper++;

            if (SpellConfig.Q.Ready && Extension.IsQ2 && ShittyHelper >= 2)
            {
                SpellConfig.Q.Cast();
            }
            else if (SpellConfig.W.Ready && MenuConfig.JungleClear["W"].Enabled)
            {
                SpellConfig.W.CastOnUnit(ObjectManager.GetLocalPlayer());
            }
            else if (SpellConfig.E.Ready && MenuConfig.JungleClear["E"].Enabled)
            {
                SpellConfig.E.Cast();
            }
        }

        public static void OnUpdate()
        {
            if (!SpellConfig.Q.Ready || !MenuConfig.JungleClear["Q"].Enabled)
            {
                return;
            }

                var mob = GameObjects.JungleLarge.FirstOrDefault(x => x.Distance(ObjectManager.GetLocalPlayer()) < SpellConfig.Q.Range / 2 &&
                                                             x.MaxHealth > 5);

            var normal = GameObjects.Jungle.FirstOrDefault(x => x.Distance(ObjectManager.GetLocalPlayer()) < SpellConfig.Q.Range / 2 &&
                                                                     x.MaxHealth > 5);
            if (mob != null)
            {
                if (Extension.IsQ2 && mob.Health > ObjectManager.GetLocalPlayer().GetSpellDamage(mob, SpellSlot.Q, DamageStage.SecondCast))
                {
                    return;
                }
                ShittyHelper = 0;
                SpellConfig.Q.Cast(mob);
            }
            else if (normal != null)
            {
                if (Extension.IsQ2 && normal.UnitSkinName != "Sru_Crab")
                {
                    return;
                }

                ShittyHelper = 0;
                SpellConfig.Q.Cast(normal);
            }
        }

        private static readonly IOrderedEnumerable<Vector3> Positions = new List<Vector3>()
        {
            new Vector3(5772, 10660, 56),
            new Vector3(5373, 11180, 57),
            new Vector3(9107, 4506, 52),
            new Vector3(9220, 3900, 55),
            new Vector3(9493, 3569, 64)
        }.OrderBy(x => x.Distance(ObjectManager.GetLocalPlayer().Position));

        private static double StealDamage(Obj_AI_Base mob)
        {
           return SummonerSpells.SmiteMonsters() + (Extension.IsQ2? ObjectManager.GetLocalPlayer().GetSpellDamage(mob, SpellSlot.Q) : 0);
        }

        public static void StealLegendary()
        {
            if (!SpellConfig.Q.Ready)
            {
                return;
            }

            var mob = GameObjects.JungleLegendary.FirstOrDefault(x => x.IsValidTarget(1300));
            if (mob == null)
            {
                return;
            }

            if (mob.Position.CountAllyHeroesInRange(700) <= 1 && StealDamage(mob) > mob.Health)
            {
                if (ObjectManager.GetLocalPlayer().IsDashing())
                {
                    if (SummonerSpells.Smite != null && SummonerSpells.Smite.Ready)
                    {
                        SummonerSpells.Smite.CastOnUnit(mob);
                    }
                    WardManager.WardJump(Positions.FirstOrDefault(), false);
                }

                if (Extension.IsQ2)
                {
                    SpellConfig.Q.Cast();
                }
            }
        }
    }
}
