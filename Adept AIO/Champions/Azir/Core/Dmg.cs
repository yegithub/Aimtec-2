using Adept_AIO.SDK.Junk;
using Adept_AIO.SDK.Usables;
using Aimtec;
using Aimtec.SDK.Damage;

namespace Adept_AIO.Champions.Azir.Core
{
    class Dmg
    {
        public static double Damage(Obj_AI_Base target)
        {
            if (target == null)
            {
                return 0;
            }

            var dmg = SoldierHelper.Soldiers.Count * Global.Player.GetSpellDamage(target, SpellSlot.W);

            if (SpellConfig.E.Ready)
            {
                dmg += Global.Player.GetSpellDamage(target, SpellSlot.E);
            }

            if (SpellConfig.Q.Ready)
            {
                dmg += Global.Player.GetSpellDamage(target, SpellSlot.Q);
            }

            if (SummonerSpells.IsValid(SummonerSpells.Ignite))
            {
                dmg += SummonerSpells.IgniteDamage(target);
            }

            return dmg;
        }
    }
}
