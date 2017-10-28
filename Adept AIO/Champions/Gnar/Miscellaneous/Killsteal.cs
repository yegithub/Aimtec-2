namespace Adept_AIO.Champions.Gnar.Miscellaneous
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
        public Killsteal()
        {
            Game.OnUpdate += OnUpdate;
        }

        private static void OnUpdate()
        {
            var target = GameObjects.EnemyHeroes.FirstOrDefault(x => x.IsValidTarget(SpellManager.Q.Range));
            if (target == null)
            {
                return;
            }

            if (SpellManager.Q.Ready &&
                MenuConfig.Killsteal["Q"].Enabled &&
                target.Health <
                (SpellManager.GnarState == GnarState.Small
                     ? Global.Player.GetSpellDamage(target, SpellSlot.Q)
                     : Global.Player.GetSpellDamage(target, SpellSlot.Q, DamageStage.SecondForm)))
            {
                SpellManager.CastQ(target);
            }
        }
    }
}