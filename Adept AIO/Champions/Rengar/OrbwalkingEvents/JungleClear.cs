namespace Adept_AIO.Champions.Rengar.OrbwalkingEvents
{
    using System.Linq;
    using Aimtec.SDK.Damage;
    using Aimtec.SDK.Extensions;
    using Core;
    using SDK.Unit_Extensions;

    class JungleClear
    {
        public static void OnPostAttack()
        {
            var mob = GameObjects.Jungle.FirstOrDefault(x => x.IsValidTarget(SpellConfig.Q.Range) && x.IsEnemy);
            if (mob == null)
            {
                return;
            }

            if (SpellConfig.Q.Ready && mob.Health > Global.Player.GetAutoAttackDamage(mob))
            {
                if (Extensions.Ferocity() == 4 && !MenuConfig.JungleClear["Q"].Enabled)
                {
                    return;
                }

                SpellConfig.CastQ(mob);
            }
        }

        public static void OnUpdate()
        {
            var mob = GameObjects.Jungle.FirstOrDefault(x => x.IsValidTarget(SpellConfig.E.Range) && x.IsEnemy);
            if (mob == null)
            {
                return;
            }

            var distance = mob.Distance(Global.Player);

            if (SpellConfig.W.Ready && distance < SpellConfig.W.Range)
            {
                if (Extensions.Ferocity() == 4 && (Global.Player.HealthPercent() >= 35 || !MenuConfig.JungleClear["W"].Enabled))
                {
                    return;
                }

                SpellConfig.CastW(mob);
            }

            if (SpellConfig.E.Ready && Extensions.Ferocity() <= 3)
            {
                SpellConfig.CastE(mob);
            }
        }
    }
}