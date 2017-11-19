namespace Adept_AIO.Champions.Zed.Core
{
    using System.Linq;
    using Aimtec;
    using Aimtec.SDK.Damage;
    using Aimtec.SDK.Extensions;
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
            var shadowCount = ShadowManager.Shadows.Count + 1;

            if (SpellManager.W.Ready && ShadowManager.CanCastFirst(SpellSlot.W))
            {
                shadowCount += 1;
            }

            if (SpellManager.R.Ready && ShadowManager.CanCastFirst(SpellSlot.R))
            {
                shadowCount += 1;
            }

            if (Global.Orbwalker.CanAttack())
            {
                dmg += Global.Player.GetAutoAttackDamage(target);
            }

            if (SpellManager.Q.Ready)
            {
                dmg += Global.Player.GetSpellDamage(target, SpellSlot.Q) * shadowCount;
            }

            if (SpellManager.E.Ready)
            {
                dmg += Global.Player.GetSpellDamage(target, SpellSlot.E);
            }

            if (SpellManager.R.Ready)
            {
                dmg += Global.Player.GetSpellDamage(target, SpellSlot.R) + dmg;
            }

            if (SummonerSpells.IsValid(SummonerSpells.Ignite))
            {
                dmg += SummonerSpells.IgniteDamage(target);
            }
            return dmg;
        }
    }
}