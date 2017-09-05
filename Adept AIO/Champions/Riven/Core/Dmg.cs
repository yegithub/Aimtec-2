using Adept_AIO.SDK.Extensions;
using Adept_AIO.SDK.Usables;
using Aimtec;
using Aimtec.SDK.Damage;
using Aimtec.SDK.Damage.JSON;

namespace Adept_AIO.Champions.Riven.Core
{
    internal class Dmg
    {   
        public static double Damage(Obj_AI_Base target)
        {
            if (target == null)
            {
                return 0;
            }

            var dmg = Global.Player.GetAutoAttackDamage(target);

            dmg += Global.Player.GetSpellDamage(target, SpellSlot.R, DamageStage.SecondCast);

            dmg += Global.Player.GetSpellDamage(target, SpellSlot.W);

            var count = 4 - Extensions.CurrentQCount;
            dmg += (Global.Player.GetSpellDamage(target, SpellSlot.Q) + dmg) * count;

            if (SummonerSpells.IsValid(SummonerSpells.Ignite))
            {
                dmg += SummonerSpells.IgniteDamage(target);
            }

            return dmg;
        }
    }
}
