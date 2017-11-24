namespace Adept_AIO.Champions.Xerath.Core
{
    using Aimtec;
    using Aimtec.SDK.Damage;
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

            switch (MenuConfig.Drawings["Dmg"].Value)
            {
                case 1:
                    dmg = Ult(target);
                    break;
                case 2:
                    dmg = Ult(target) + Qwe(target);
                    break;
                case 3:
                    dmg = Qwe(target);
                    break;
            }

            return dmg;
        }

        private static double Ult(Obj_AI_Base target)
        {
            var dmg = 0d;
            if (SpellManager.R.Ready)
            {
                dmg += Global.Player.GetSpellDamage(target, SpellSlot.R) * SpellManager.GetUltiShots();
            }
            return dmg;
        }

        private static double Qwe(Obj_AI_Base target)
        {
            var dmg = 0d;

            if (SpellManager.Q.Ready || SpellManager.Q.IsCharging)
            {
                dmg += Global.Player.GetSpellDamage(target, SpellSlot.Q);
            }

            if (SpellManager.W.Ready)
            {
                dmg += Global.Player.GetSpellDamage(target, SpellSlot.W);
            }

            if (SpellManager.E.Ready)
            {
                dmg += Global.Player.GetSpellDamage(target, SpellSlot.E);
            }

            return dmg;
        }
    }
}