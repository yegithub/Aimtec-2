namespace Adept_AIO.Champions.LeeSin.Core.Damage
{
    using Aimtec;
    using Aimtec.SDK.Damage;
    using Aimtec.SDK.Damage.JSON;
    using SDK.Unit_Extensions;
    using Spells;

    class Dmg : IDmg
    {
        private readonly ISpellConfig _spellConfig;

        public Dmg(ISpellConfig spellConfig) { _spellConfig = spellConfig; }

        public double Damage(Obj_AI_Base target)
        {
            if (target == null)
            {
                return 0;
            }

            var dmg = Global.Player.GetAutoAttackDamage(target);

            if (_spellConfig.E.Ready)
            {
                dmg += Global.Player.GetSpellDamage(target, SpellSlot.E) + dmg;
            }

            if (_spellConfig.Q.Ready)
            {
                if (_spellConfig.IsQ2())
                {
                    dmg += Global.Player.GetSpellDamage(target, SpellSlot.Q, DamageStage.SecondCast) + dmg;
                }
                else
                {
                    dmg += Global.Player.GetSpellDamage(target, SpellSlot.Q) +
                           Global.Player.GetSpellDamage(target, SpellSlot.Q, DamageStage.SecondCast) +
                           dmg;
                }
            }

            if (_spellConfig.R.Ready)
            {
                dmg += Global.Player.GetSpellDamage(target, SpellSlot.R);
            }
            return dmg;
        }
    }
}