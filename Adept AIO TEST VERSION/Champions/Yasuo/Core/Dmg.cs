using Adept_AIO.SDK.Extensions;
using Aimtec;
using Aimtec.SDK.Damage;

namespace Adept_AIO.Champions.Yasuo.Core
{
    internal class Dmg
    {
        public static double Damage(Obj_AI_Base target)
        {
            if (target == null)
            {
                return 0;
            }

            var dmg = GlobalExtension.Player.GetAutoAttackDamage(target);

            if (SpellConfig.Q.Ready)
            {
                dmg += GlobalExtension.Player.GetSpellDamage(target, SpellSlot.Q) + dmg;
            }

            if (SpellConfig.E.Ready)
            {
                dmg += GlobalExtension.Player.GetSpellDamage(target, SpellSlot.E);
            }

            if (SpellConfig.R.Ready)
            {
                dmg += GlobalExtension.Player.GetSpellDamage(target, SpellSlot.R) + dmg;
            }
            return dmg;
        }
    }
}
