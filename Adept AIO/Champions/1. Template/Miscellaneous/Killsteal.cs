namespace Adept_AIO.Champions._1._Template.Miscellaneous
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
            var target = GameObjects.EnemyHeroes.OrderBy(x => x.Distance(Global.Player)).FirstOrDefault(x => x.IsValidTarget(2000));

            if (target == null || Global.Player.IsDead || Global.Orbwalker.IsWindingUp)
            {
                return;
            }

            if (SpellManager.Q.Ready && target.Health < Global.Player.GetSpellDamage(target, SpellSlot.Q) && MenuConfig.Killsteal["Q"].Enabled && target.IsValidTarget(SpellManager.Q.Range))
            {
                SpellManager.CastE(target);
            }
            else if (SpellManager.R.Ready 
                && target.Health < Dmg.Ult(target) && MenuConfig.Killsteal["R"].Enabled)
            {
                SpellManager.CastR(target);
            }
        }
    }
}