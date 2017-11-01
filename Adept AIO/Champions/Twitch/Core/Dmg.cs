namespace Adept_AIO.Champions.Twitch.Core
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
            "JaxCounterStrike", "FioraW", "BansheesVeil", "BlackShield", "KindredrNoDeathBuff", "malzaharpassiveshield", "NocturneShroudofDarkness", "OlafRagnarock", "UndyingRage", "SivirE"
        };

        public static double EDmg(Obj_AI_Base target)
        {
            if (Buffs.Any(target.HasBuff) || !target.HasBuff("twitchdeadlyvenom"))
            {
                return 0;
            }

            return Global.Player.GetSpellDamage(target, SpellSlot.E) + Global.Player.GetSpellDamage(target, SpellSlot.E, DamageStage.Buff);
        }
    }
}