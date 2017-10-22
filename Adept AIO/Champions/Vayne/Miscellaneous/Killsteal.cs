namespace Adept_AIO.Champions.Vayne.Miscellaneous
{
    using System.Linq;
    using Aimtec;
    using Aimtec.SDK.Damage;
    using Aimtec.SDK.Damage.JSON;
    using Aimtec.SDK.Extensions;
    using Core;
    using SDK.Unit_Extensions;

    class Killsteal
    {
        public static void OnUpdate()
        {
            var target = GameObjects.EnemyHeroes.FirstOrDefault(x => x.IsValidTarget(SpellManager.E.Range) && !x.IsDead);

            if (target == null)
            {
                return;
            }

            if (SpellManager.Q.Ready && MenuConfig.Killsteal["Q"].Enabled)
            {
                if (target.Health <= Global.Player.GetSpellDamage(target, SpellSlot.Q) ||
                    target.Health <= Global.Player.GetSpellDamage(target, SpellSlot.Q) + Global.Player.GetAutoAttackDamage(target))
                {
                    SpellManager.CastQ(target);
                }
            }
            else if (SpellManager.E.Ready && MenuConfig.Killsteal["E"].Enabled)
            {
                if (target.Health <= Global.Player.GetSpellDamage(target, SpellSlot.E) * 0.75 ||
                    SpellManager.CanStun(target) &&
                    target.Health < Global.Player.GetSpellDamage(target, SpellSlot.E) * 0.75f + Global.Player.GetSpellDamage(target, SpellSlot.E, DamageStage.Collision))
                {
                    SpellManager.CastE(target);
                }
            }
        }
    }
}