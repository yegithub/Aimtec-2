namespace Adept_AIO.Champions.Kalista.Core
{
    using System.Linq;
    using Aimtec;
    using Aimtec.SDK.Damage;
    using Aimtec.SDK.Damage.JSON;
    using Aimtec.SDK.Extensions;
    using SDK.Unit_Extensions;

    class Dmg
    {
        private static readonly string[] Buffs =
        {
            "JaxCounterStrike",
            "FioraW",
            "BansheesVeil",
            "BlackShield",
            "KindredrNoDeathBuff",
            "malzaharpassiveshield",
            "NocturneShroudofDarkness",
            "OlafRagnarock",
            "UndyingRage",
            "SivirE"
        };


        private static readonly string[] ReviveBuffs =
        {
            "chronorevive",
            "zhonyasringshield",
            "AatroxPassiveDeath",
            "rebirth"
        };

        public static double EDmg(Obj_AI_Base target)
        {
            if (target.ValidActiveBuffs().Any(x => Buffs.Contains(x.Name)) || target.ValidActiveBuffs().Any(x => ReviveBuffs.Contains(x.Name)))
            {
                return 0;
            }

            var dmg = Global.Player.GetSpellDamage(target, SpellSlot.E) + Global.Player.GetSpellDamage(target, SpellSlot.E, DamageStage.Buff);

            var legendary = GameObjects.JungleLegendary.FirstOrDefault(x => x.HasBuff("kalistaexpungemarker"));
            if (legendary != null && legendary.NetworkId == target.NetworkId)
            {
                dmg *= 0.85f;
            }
          
            return dmg;
        }
    }
}