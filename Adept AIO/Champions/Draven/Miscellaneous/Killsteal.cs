namespace Adept_AIO.Champions.Draven.Miscellaneous
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

            if (SpellManager.E.Ready && target.Health < Global.Player.GetSpellDamage(target, SpellSlot.E) && MenuConfig.Killsteal["E"].Enabled && target.IsValidTarget(SpellManager.E.Range))
            {
                SpellManager.CastE(target);
            }
            else if (SpellManager.R.Ready 
                && target.Health < Dmg.Ult(target) && MenuConfig.Killsteal["R"].Enabled 
                && !target.IsValidAutoRange())
            {
                SpellManager.CastR(target);
            }
        }
    }
}