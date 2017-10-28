namespace Adept_AIO.Champions.Jinx.Core
{
    using Aimtec;
    using Aimtec.SDK.Damage;
    using SDK.Unit_Extensions;

    class Dmg
    {
        private readonly SpellConfig _spellConfig;

        public Dmg(SpellConfig spellConfig)
        {
            _spellConfig = spellConfig;
        }

        public double Damage(Obj_AI_Base target)
        {
            if (target == null)
            {
                return 0;
            }

            var dmg = 0d;

            if (Global.Orbwalker.CanAttack())
            {
                dmg += Global.Player.GetAutoAttackDamage(target);
            }

            if (_spellConfig.W.Ready)
            {
                dmg += Global.Player.GetSpellDamage(target, SpellSlot.W);
            }

            if (_spellConfig.E.Ready)
            {
                dmg += Global.Player.GetSpellDamage(target, SpellSlot.E);
            }

            if (_spellConfig.R.Ready)
            {
                dmg += Global.Player.GetSpellDamage(target, SpellSlot.R);
            }
            return dmg;
        }
    }
}