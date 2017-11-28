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

            var dmg = 0d;

            if (SpellManager.Q.Ready || SpellManager.Q.IsCharging)
            {
                dmg += Global.Player.GetSpellDamage(target, SpellSlot.Q);
            }

            if (SpellManager.E.Ready)
            {
                dmg += Global.Player.GetSpellDamage(target, SpellSlot.E);
            }

            return dmg;
        }

        public static double Ult(Obj_AI_Base target)
        {
            var dmg = 0d;

            if (SpellManager.R.Ready)
            {
                dmg += Global.Player.GetSpellDamage(target, SpellSlot.R) + Global.Player.GetSpellDamage(target, SpellSlot.R, DamageStage.SecondForm);
            }

            return dmg;
        }
    }
}