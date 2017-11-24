namespace Adept_AIO.Champions.Xerath.Miscellaneous
{
    using System.Linq;
    using Aimtec;
    using Aimtec.SDK.Damage;
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
            var target = GameObjects.EnemyHeroes.OrderBy(x => x.Health).FirstOrDefault(x => x.IsValidTarget(SpellManager.Q.ChargedMaxRange));

            if (target == null || !target.IsValidTarget())
            {
                return;
            }

            if (SpellManager.Q.Ready && target.Health < Global.Player.GetSpellDamage(target, SpellSlot.Q) && MenuConfig.Killsteal["Q"].Enabled)
            {
                SpellManager.CastQ(target);
            }
            else if (SpellManager.W.Ready && target.Health < Global.Player.GetSpellDamage(target, SpellSlot.W) && MenuConfig.Killsteal["W"].Enabled && target.IsValidTarget(SpellManager.W.Range))
            {
                SpellManager.CastW(target);
            }
            else if (SpellManager.E.Ready && target.Health < Global.Player.GetSpellDamage(target, SpellSlot.E) && MenuConfig.Killsteal["E"].Enabled && target.IsValidTarget(SpellManager.E.Range))
            {
                SpellManager.CastE(target);
            }
            else if (SpellManager.R.Ready && target.Health < Global.Player.GetSpellDamage(target, SpellSlot.R) && MenuConfig.Killsteal["R"].Enabled && target.IsValidTarget(SpellManager.R.Range))
            {
                SpellManager.CastR(target);
            }
        }
    }
}