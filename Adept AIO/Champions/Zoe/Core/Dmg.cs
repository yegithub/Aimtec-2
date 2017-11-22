namespace Adept_AIO.Champions.Zoe.Core
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

            var dmg = 0d;
           
            if (Global.Orbwalker.CanAttack())
            {
                dmg += Global.Player.GetAutoAttackDamage(target);
            }

            if (SpellManager.Q.Ready)
            {
                dmg += Global.Player.GetSpellDamage(target, SpellSlot.Q);
            }

            if (SpellManager.E.Ready)
            {
                dmg += Global.Player.GetSpellDamage(target, SpellSlot.E) + Global.Player.GetSpellDamage(target, SpellSlot.E, DamageStage.Collision);
            }

            if (SummonerSpells.IsValid(SummonerSpells.Ignite))
            {
                dmg += SummonerSpells.IgniteDamage(target);
            }
            return dmg;
        }
    }
}