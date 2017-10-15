namespace Adept_AIO.Champions.Azir.Core
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

            if (SoldierManager.Soldiers != null && SoldierManager.Soldiers.Any())
            {
                dmg += (SoldierManager.Soldiers.Count + Global.Player.GetSpell(SpellSlot.W).Ammo) *
                       Global.Player.GetSpellDamage(target, SpellSlot.W);
            }

            if (SpellConfig.E.Ready)
            {
                dmg += Global.Player.GetSpellDamage(target, SpellSlot.E);
            }

            if (SpellConfig.Q.Ready)
            {
                dmg += Global.Player.GetSpellDamage(target, SpellSlot.Q);
            }

            if (SummonerSpells.IsValid(SummonerSpells.Ignite))
            {
                dmg += SummonerSpells.IgniteDamage(target);
            }

            return dmg;
        }
    }
}