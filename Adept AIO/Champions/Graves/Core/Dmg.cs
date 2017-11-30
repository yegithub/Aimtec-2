namespace Adept_AIO.Champions.Graves.Core
{
    using Aimtec;
    using Aimtec.SDK.Damage;
    using Aimtec.SDK.Damage.JSON;
    using SDK.Unit_Extensions;

    class Dmg
    {
        public static double Damage(Obj_AI_Base target)
        {
            if (target == null)
            {
                return 0;
            }

            var dmg = Global.Orbwalker.CanAttack() ? Global.Player.GetAutoAttackDamage(target) : 0d;

            if (SpellManager.Q.Ready)
            {
                dmg += Global.Player.GetSpellDamage(target, SpellSlot.Q) + (SpellManager.WallForQ.IsZero ? 0 : Global.Player.GetSpellDamage(target, SpellSlot.Q, DamageStage.WayBack));
            }

            if (SpellManager.R.Ready)
            {
                dmg += Global.Player.GetSpellDamage(target, SpellSlot.R);
            }

            return dmg;
        }

        public static double Ult(Obj_AI_Base target)
        {
            return SpellManager.R.Ready ? Global.Player.GetSpellDamage(target, SpellSlot.R) : 0d;
        }
    }
}