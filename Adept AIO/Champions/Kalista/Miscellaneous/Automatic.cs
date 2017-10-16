namespace Adept_AIO.Champions.Kalista.Miscellaneous
{
    using System.Linq;
    using Aimtec;
    using Aimtec.SDK.Extensions;
    using Aimtec.SDK.Orbwalking;
    using Core;
    using SDK.Unit_Extensions;

    class Automatic
    {
        public static void OnUpdate()
        {
            if (Global.Player.IsRecalling())
            {
                return;
            }

            if (SpellManager.E.Ready && MenuConfig.Misc["E"].Enabled && GameObjects.EnemyHeroes.Count(x => x.HasBuff("kalistaexpungemarker")) >= 1)
            {
                var m = GameObjects.EnemyMinions.FirstOrDefault(x => x.HasBuff("kalistaexpungemarker") && x.Health < Dmg.EDmg(x) && x.IsValidTarget(SpellManager.E.Range));
                if (m != null)
                {
                    SpellManager.E.Cast();
                }
            }

            switch (Global.Orbwalker.Mode)
            {
                case OrbwalkingMode.Combo when Global.Player.CountEnemyHeroesInRange(Global.Player.AttackRange) <= 0 && MenuConfig.Combo["Minions"].Enabled:
                    var m = GameObjects.EnemyMinions.FirstOrDefault(x => x.Distance(Global.Player) <= 2000);
                    if (m != null)
                    {
                        Global.Orbwalker.Attack(m);
                    }
                    break;
                case OrbwalkingMode.Laneclear when SpellManager.E.Ready &&
                                                   GameObjects.EnemyMinions.Count(x => x.HasBuff("kalistaexpungemarker") && Dmg.EDmg(x) > x.Health) >= 1 &&
                                                   MenuConfig.LaneClear["E"].Enabled:
                    SpellManager.E.Cast();
                    break;
            }

            if (SpellManager.R.Ready && MenuConfig.Misc["R"].Enabled)
            {
                var soulbound = GameObjects.AllGameObjects.FirstOrDefault(x => x.Name == "Kalista_Base_P_LinkIcon.troy") as Obj_AI_Hero;
                if (soulbound != null && soulbound.HealthPercent() >= MenuConfig.Misc["R"].Value)
                {
                    SpellManager.R.Cast();
                }
            }

            if (SpellManager.W.Ready && MenuConfig.Misc["W"].Enabled && Global.Player.CountEnemyHeroesInRange(1200) <= 0)
            {
                SpellManager.CastW();
            }
        }
    }
}