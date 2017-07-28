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
                Items.CastTiamat();
                SpellConfig.E.Cast();
            }
        }

        public static void OnUpdate()
        {
            if (!SpellConfig.Q.Ready || !MenuConfig.JungleClear["Q"].Enabled)
            {
                return;
            }

            var mob = ObjectManager.Get<Obj_AI_Minion>().FirstOrDefault(x => x.Distance(ObjectManager.GetLocalPlayer()) < SpellConfig.Q.Range / 2 && x.GetJungleType() != GameObjects.JungleType.Unknown && x.MaxHealth > 5);

            if (mob != null)
            {
                if (!SmiteOptional.Contains(mob.UnitSkinName))
                {
                    return;
                }

                if (Extension.IsQ2 && mob.UnitSkinName != "Sru_Crab" && mob.Health > ObjectManager.GetLocalPlayer().GetSpellDamage(mob, SpellSlot.Q, DamageStage.SecondCast))
                {
                    return;
                }

                SpellConfig.Q.Cast(mob);

                ShittyHelper = 0;
            }
        }

        private static readonly Vector3[] Positions =
        {
            new Vector3(5740, 56, 10629),
            new Vector3(5808, 54, 10319),
            new Vector3(5384, 57, 11282),
            new Vector3(9076, 53, 4446),
            new Vector3(9058, 53, 4117),
            new Vector3(9687, 56, 3490)
        };

        private static float Q2Time;

        private static double StealDamage(Obj_AI_Base mob)
        {
           return SummonerSpells.SmiteMonsters() + (Extension.IsQ2? ObjectManager.GetLocalPlayer().GetSpellDamage(mob, SpellSlot.Q, DamageStage.SecondCast) : 0);
        }

        private static readonly string[] SmiteAlways = { "SRU_Dragon_Air", "SRU_Dragon_Fire", "SRU_Dragon_Earth", "SRU_Dragon_Water", "SRU_Dragon_Elder", "SRU_Baron", "SRU_RiftHerald" };
        private static readonly string[] SmiteOptional = {"Sru_Crab", "SRU_Razorbreak", "SRU_Krug", "SRU_Murkwolf", "SRU_Gromp", "SRU_Blue", "SRU_Red"};

        public static void StealMobs()
        {
            var smiteAbleMob = ObjectManager.Get<Obj_AI_Minion>().FirstOrDefault(x => x.Distance(ObjectManager.GetLocalPlayer()) < 1300);
            if (smiteAbleMob != null)
            {
                if (!SmiteAlways.Contains(smiteAbleMob.UnitSkinName) && !SmiteOptional.Contains(smiteAbleMob.UnitSkinName))
                {
                    return;
                }

                Console.WriteLine("??");
                if (smiteAbleMob.Health < StealDamage(smiteAbleMob) && SummonerSpells.Smite != null && SummonerSpells.Smite.Ready)
                {
                    if (SmiteAlways.Contains(smiteAbleMob.UnitSkinName) && SummonerSpells.Ammo("Smite") <= 1 ||
                        smiteAbleMob.UnitSkinName.ToLower().Contains("blue") && !MenuConfig.JungleClear["Blue"].Enabled)
                    {
                        return;
                    }

                    if (Extension.IsQ2)
                    {
                        SpellConfig.Q.Cast();
                    }

                    SummonerSpells.Smite.CastOnUnit(smiteAbleMob);
                }
            }

            var mob = GameObjects.JungleLegendary.FirstOrDefault(x => x.Distance(ObjectManager.GetLocalPlayer()) <= 1500);
          
            if (mob == null)
            {
                return;
            }
          
            if (Q2Time > 0 && Environment.TickCount - Q2Time <= 1500 && SummonerSpells.Smite != null && SummonerSpells.Smite.Ready && StealDamage(mob) > mob.Health)
            {
                if (SpellConfig.W.Ready && Extension.IsFirst(SpellConfig.W) && ObjectManager.GetLocalPlayer().Distance(mob) <= 500)
                {
                    SummonerSpells.Smite.CastOnUnit(mob);
                    WardManager.WardJump(Positions.FirstOrDefault(), false);
                }
            }

            if (mob.Position.CountAllyHeroesInRange(700) <= 1 && SpellConfig.Q.Ready && Extension.IsQ2)
            {
                SpellConfig.Q.Cast();
                Q2Time = Environment.TickCount;
            }
        }
    }
}
