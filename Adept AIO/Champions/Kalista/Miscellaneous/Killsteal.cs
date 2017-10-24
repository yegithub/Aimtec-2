namespace Adept_AIO.Champions.Kalista.Miscellaneous
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
            if (Global.Player.IsDead)
            {
                return;
            }

            if (MenuConfig.Killsteal["E"].Enabled && SpellManager.E.Ready && GameObjects.EnemyHeroes.Any(x => x.HasBuff("kalistaexpungemarker")))
            {
                var t = GameObjects.EnemyHeroes.FirstOrDefault(x => x.Health <= Dmg.EDmg(x) && x.IsValidTarget(SpellManager.E.Range));
                if (t != null)
                {
                    SpellManager.E.Cast();
                }
            }

            if (MenuConfig.Killsteal["Q"].Enabled && SpellManager.Q.Ready && !Global.Orbwalker.IsWindingUp)
            {
                var t = GameObjects.EnemyHeroes.FirstOrDefault(x => x.Health < Global.Player.GetSpellDamage(x, SpellSlot.Q) && x.IsValidTarget(SpellManager.Q.Range));
                if (t == null)
                {
                    return;
                }
                SpellManager.CastQ(t);
            }
        }
    }
}