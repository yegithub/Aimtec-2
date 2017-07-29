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
        public static void OnPostAttack(AttackableUnit mob)
        {
            if (mob == null || mob.Health < ObjectManager.GetLocalPlayer().GetAutoAttackDamage((Obj_AI_Base)mob))
            {
                return;
            }
           
            if (SpellConfig.Q.Ready && Extension.IsQ2 && SpellConfig.QAboutToEnd)
            {
                ObjectManager.GetLocalPlayer().SpellBook.CastSpell(SpellSlot.Q);
            }

            if (ObjectManager.GetLocalPlayer().Level <= 12)
            {
                if (Extension.PassiveStack > 0)
                {
                    return;
                }
                if (SpellConfig.W.Ready && MenuConfig.JungleClear["W"].Enabled && !SpellConfig.Q.Ready)
                {
                    SpellConfig.W.CastOnUnit(ObjectManager.GetLocalPlayer());
                }
                else if (SpellConfig.E.Ready && MenuConfig.JungleClear["E"].Enabled && !SpellConfig.W.Ready)
                {
                    if (Extension.IsFirst(SpellConfig.E))
                    {
                       SpellConfig.CastE((Obj_AI_Base)mob);
                    }
                   else if (SpellConfig.W.Ready || SpellConfig.Q.Ready)
                    {
                        return;
                    }
                    SpellConfig.E.Cast();
                }
            }
            else 
            {
                if (SpellConfig.E.Ready && MenuConfig.JungleClear["E"].Enabled)
                {
                    SpellConfig.CastE((Obj_AI_Base)mob);
                }
                else if (SpellConfig.W.Ready && MenuConfig.JungleClear["W"].Enabled && !Extension.IsQ2)
                {
                    SpellConfig.W.CastOnUnit(ObjectManager.GetLocalPlayer());
                }
            }
        }

        public static void OnUpdate()
        {
            if (!SpellConfig.Q.Ready || !MenuConfig.JungleClear["Q"].Enabled)
            {
                return;
            }

            var mob = ObjectManager.Get<Obj_AI_Minion>().FirstOrDefault(x => x.Distance(ObjectManager.GetLocalPlayer()) < SpellConfig.Q.Range / 2 && x.GetJungleType() != GameObjects.JungleType.Unknown && x.MaxHealth > 5);

            if (mob == null)
            {
                return;
            }

            if (!SmiteOptional.Contains(mob.UnitSkinName))
            {
                return;
            }

            if (SpellConfig.Q.Ready && Extension.IsQ2 && mob.Health < ObjectManager.GetLocalPlayer().GetSpellDamage(mob, SpellSlot.Q, DamageStage.SecondCast))
            {
                ObjectManager.GetLocalPlayer().SpellBook.CastSpell(SpellSlot.Q);
            }

            if (!Extension.IsQ2 && mob.Distance(ObjectManager.GetLocalPlayer()) >= ObjectManager.GetLocalPlayer().AttackRange + mob.BoundingRadius)
            {
                ObjectManager.GetLocalPlayer().SpellBook.CastSpell(SpellSlot.Q, mob.ServerPosition);
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

        private static double StealDamage(Obj_AI_Base mob)
        {
           return SummonerSpells.SmiteMonsters() + (Extension.IsQ2? ObjectManager.GetLocalPlayer().GetSpellDamage(mob, SpellSlot.Q, DamageStage.SecondCast) : 0);
        }

        private static readonly string[] SmiteAlways = { "SRU_Dragon_Air", "SRU_Dragon_Fire", "SRU_Dragon_Earth", "SRU_Dragon_Water", "SRU_Dragon_Elder", "SRU_Baron", "SRU_RiftHerald" };
        private static readonly string[] SmiteOptional = {"Sru_Crab", "SRU_Razorbeak", "SRU_Krug", "SRU_Murkwolf", "SRU_Gromp", "SRU_Blue", "SRU_Red"};
        private static float Q2Time;

        public static void StealMobs()
        {
            if (ObjectManager.GetLocalPlayer().Level == 1)
            {
                return;
            }

            var smiteAbleMob = ObjectManager.Get<Obj_AI_Minion>().FirstOrDefault(x => x.Distance(ObjectManager.GetLocalPlayer()) < 1300);
            if (smiteAbleMob != null)
            {
                if (!SmiteAlways.Contains(smiteAbleMob.UnitSkinName) && !SmiteOptional.Contains(smiteAbleMob.UnitSkinName))
                {
                    return;
                }

                if (smiteAbleMob.Health < StealDamage(smiteAbleMob) && SummonerSpells.Smite != null && SummonerSpells.Smite.Ready)
                {
                    if (SmiteOptional.Contains(smiteAbleMob.UnitSkinName) && SummonerSpells.Ammo("Smite") <= 1 || 
                        smiteAbleMob.UnitSkinName.ToLower().Contains("blue") && !MenuConfig.JungleClear["Blue"].Enabled ||
                        smiteAbleMob.UnitSkinName.ToLower().Contains("red"))
                    {
                        return;
                    }

                    if (SmiteOptional.Contains(smiteAbleMob.UnitSkinName) &&
                        ObjectManager.GetLocalPlayer().HealthPercent() >= 70)
                    {
                        return;
                    }

                    if (Extension.IsQ2)
                    {
                        SpellConfig.Q.Cast();
                    }

                    if (MenuConfig.JungleClear["Smite"].Enabled)
                    {
                        SummonerSpells.Smite.CastOnUnit(smiteAbleMob);
                    }
                }
            }

            var mob = GameObjects.JungleLegendary.FirstOrDefault(x => x.Distance(ObjectManager.GetLocalPlayer()) <= 1500);
          
            if (mob == null || !MenuConfig.JungleClear["Smite"].Enabled)
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
