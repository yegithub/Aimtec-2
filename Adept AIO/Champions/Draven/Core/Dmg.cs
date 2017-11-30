namespace Adept_AIO.Champions.Draven.Core
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

            if (SpellManager.E.Ready)
            {
                dmg += Global.Player.GetSpellDamage(target, SpellSlot.E);
            }

            return dmg + Ult(target);
        }

        public static double Ult(Obj_AI_Base target)
        {
            return SpellManager.R.Ready ? Global.Player.GetSpellDamage(target, SpellSlot.R) + Global.Player.GetSpellDamage(target, SpellSlot.R, DamageStage.SecondForm) : 0d;
        }
    }
}