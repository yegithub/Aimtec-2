namespace Adept_AIO.Champions.Riven.Core
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

            if (SpellConfig.R.Ready)
            {
                dmg += Global.Player.GetSpellDamage(target, SpellSlot.R, DamageStage.SecondCast);
            }

            if (SpellConfig.W.Ready)
            {
                dmg += Global.Player.GetSpellDamage(target, SpellSlot.W);
            }

            if (SpellConfig.Q.Ready)
            {
                var count = 4 - Extensions.CurrentQCount;
                dmg += (Global.Player.GetSpellDamage(target, SpellSlot.Q) + dmg) * count;
            }
        
            if (SummonerSpells.IsValid(SummonerSpells.Ignite))
            {
                dmg += SummonerSpells.IgniteDamage(target);
            }

            return dmg;
        }
    }
}