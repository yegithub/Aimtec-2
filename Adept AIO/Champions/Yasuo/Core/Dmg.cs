using Aimtec;
using Aimtec.SDK.Damage;

namespace Adept_AIO.Champions.Yasuo.Core
{
    class Dmg
    {
        public static double Damage(Obj_AI_Base target)
        {
            if (target == null)
            {
                return 0;
            }

            var dmg = ObjectManager.GetLocalPlayer().GetAutoAttackDamage(target);

            if (SpellConfig.Q.Ready)
            {
                dmg += ObjectManager.GetLocalPlayer().GetSpellDamage(target, SpellSlot.Q) + dmg;
            }

            if (SpellConfig.E.Ready)
            {
                dmg += ObjectManager.GetLocalPlayer().GetSpellDamage(target, SpellSlot.E);
            }

            if (SpellConfig.R.Ready)
            {
                dmg += ObjectManager.GetLocalPlayer().GetSpellDamage(target, SpellSlot.R) + dmg;
            }
            return dmg;
        }
    }
}
