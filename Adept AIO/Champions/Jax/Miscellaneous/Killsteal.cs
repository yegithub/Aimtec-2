namespace Adept_AIO.Champions.Jax.Miscellaneous
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
            if (!SpellConfig.Q.Ready || !MenuConfig.Killsteal["Q"].Enabled)
            {
                return;
            }

            var target = GameObjects.EnemyHeroes.FirstOrDefault(x =>
                x.Distance(Global.Player) < SpellConfig.Q.Range &&
                x.Health < Global.Player.GetSpellDamage(x, SpellSlot.Q));

            if (target == null || !target.IsValidTarget())
            {
                return;
            }

            SpellConfig.Q.CastOnUnit(target);
        }
    }
}