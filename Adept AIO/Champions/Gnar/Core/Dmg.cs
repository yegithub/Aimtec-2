namespace Adept_AIO.Champions.Gnar.Core
{
    using Aimtec;
    using Aimtec.SDK.Damage;
    using Aimtec.SDK.Damage.JSON;
    using SDK.Unit_Extensions;
    using SDK.Usables;

    class Dmg
    {
        public static double Damage(Obj_AI_Base target)
        {
            if (target == null)
            {
                return 0;
            }

            var dmg = Global.Player.GetAutoAttackDamage(target);

            if (SpellManager.Q.Ready)
            {
                dmg += SpellManager.GnarState == GnarState.Small
                    ? Global.Player.GetSpellDamage(target, SpellSlot.Q)
                    : Global.Player.GetSpellDamage(target, SpellSlot.Q, DamageStage.SecondForm);
            }

            if (SpellManager.W.Ready)
            {
                dmg += SpellManager.GnarState == GnarState.Small
                    ? Global.Player.GetSpellDamage(target, SpellSlot.W)
                    : Global.Player.GetSpellDamage(target, SpellSlot.W, DamageStage.SecondForm);
            }

            if (SpellManager.E.Ready)
            {
                dmg += SpellManager.GnarState == GnarState.Small
                    ? Global.Player.GetSpellDamage(target, SpellSlot.E)
                    : Global.Player.GetSpellDamage(target, SpellSlot.E, DamageStage.SecondForm);
            }

            if (SpellManager.R.Ready && SpellManager.GnarState == GnarState.Mega)
            {
                dmg += Global.Player.GetSpellDamage(target, SpellSlot.R) + Global.Player.GetSpellDamage(target, SpellSlot.R, DamageStage.Collision);
            }

            if (SummonerSpells.IsValid(SummonerSpells.Ignite))
            {
                dmg += SummonerSpells.IgniteDamage(target);
            }

            return dmg;
        }
    }
}