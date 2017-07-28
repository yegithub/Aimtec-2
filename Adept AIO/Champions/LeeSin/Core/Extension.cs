using Adept_AIO.SDK.Usables;
using Aimtec;
using Aimtec.SDK.Extensions;
using Aimtec.SDK.Orbwalking;

namespace Adept_AIO.Champions.LeeSin.Core
{
    internal class Extension
    {
        public static OrbwalkerMode InsecMode, WardjumpMode, KickFlashMode;

        public static bool IsQ2 => !IsFirst(SpellConfig.Q) && SpellConfig.Q.Ready;

        public static bool HasQ2(Obj_AI_Base target)
        {
            return target.HasBuff("BlindMonkSonicWave");
        }

        private const string PassiveName = "blindmonkpassive_cosmetic";
        public static int PassiveStack => ObjectManager.GetLocalPlayer().HasBuff(PassiveName) ? ObjectManager.GetLocalPlayer().GetBuffCount(PassiveName) : 0;

        public static bool IsFirst(Aimtec.SDK.Spell spell)
        {
            return ObjectManager.GetLocalPlayer()
                   .SpellBook.GetSpell(spell.Slot)
                   .SpellData.Name.ToLower()
                   .Contains("one");
        }

        public static void QSmite(Obj_AI_Hero target)
        {
            var minion = SpellConfig.Q.GetPrediction(target).CollisionObjects[0];

            if (minion != target && SummonerSpells.Smite != null && SummonerSpells.Smite.Ready)
            {
                SummonerSpells.Smite.CastOnUnit(minion);
            }
        }
    }
}
