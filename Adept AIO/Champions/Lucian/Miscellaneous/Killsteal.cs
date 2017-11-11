namespace Adept_AIO.Champions.Lucian.Miscellaneous
{
    using System.Linq;
    using Aimtec;
    using Aimtec.SDK.Damage;
    using Aimtec.SDK.Extensions;
    using Core;
    using SDK.Unit_Extensions;

    class Killsteal
    {
        public static void OnUpdate()
        {
            var target = GameObjects.EnemyHeroes.FirstOrDefault(x => x.IsValidTarget(1500));

            if (target == null || Global.Orbwalker.IsWindingUp)
            {
                return;
            }

            if (SpellManager.Q.Ready && target.Health < Global.Player.GetSpellDamage(target, SpellSlot.Q) && target.IsValidTarget(SpellManager.ExtendedRange) &&
                MenuConfig.Killsteal["Q"].Enabled)
            {
                SpellManager.CastQ(target);
            }
            else if (SpellManager.W.Ready && target.Health < Global.Player.GetSpellDamage(target, SpellSlot.W) && target.IsValidTarget(SpellManager.W.Range) &&
                     MenuConfig.Killsteal["W"].Enabled)
            {
                SpellManager.W.Cast(target);
            }
            else if (SpellManager.E.Ready && target.Health < Global.Player.GetAutoAttackDamage(target) * 1.3f && target.IsValidTarget(SpellManager.E.Range) &&
                     MenuConfig.Killsteal["E"].Enabled)
            {
                SpellManager.E.Cast(target.ServerPosition);
                Global.Orbwalker.Attack(target);
            }
            else if (SpellManager.R.Ready && target.Health < Global.Player.GetSpellDamage(target, SpellSlot.R) && target.IsValidTarget(SpellManager.R.Range) &&
                     MenuConfig.Killsteal["R"].Enabled)
            {
                SpellManager.CastR(target);
            }
        }
    }
}