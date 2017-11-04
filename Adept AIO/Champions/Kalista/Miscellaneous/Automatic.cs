namespace Adept_AIO.Champions.Kalista.Miscellaneous
{
    using System.Linq;
    using Aimtec;
    using Aimtec.SDK.Damage;
    using Aimtec.SDK.Damage.JSON;
    using Aimtec.SDK.Extensions;
    using Aimtec.SDK.Orbwalking;
    using Aimtec.SDK.Prediction.Health;
    using Core;
    using SDK.Generic;
    using SDK.Unit_Extensions;

    class Automatic
    {
        private const string KalistaBuffName = "kalistaexpungemarker";

        public static void Test()
        {
            if (Global.Player.IsRecalling())
            {
                return;
            }

            switch (Global.Orbwalker.Mode)
            {
                case OrbwalkingMode.Lasthit:
                case OrbwalkingMode.Laneclear:
                    if (SpellManager.E.Ready && GameObjects.EnemyMinions.Any(x => x.HasBuff(KalistaBuffName) && x.IsValidTarget(SpellManager.E.Range) && Dmg.EDmg(x) > x.Health) &&
                        MenuConfig.LaneClear["E"].Enabled)
                    {
                        SpellManager.E.Cast();
                    }
                    break;
                case OrbwalkingMode.Combo:
                    var m = GameObjects.EnemyMinions.FirstOrDefault(x => x.Distance(Global.Player) <= 2000);
                    if (m != null && Global.Orbwalker.CanAttack() && Global.Player.CountEnemyHeroesInRange(Global.Player.AttackRange) <= 0 && MenuConfig.Combo["Minions"].Enabled &&
                        m.IsValidAutoRange())
                    {
                        Global.Orbwalker.Attack(m);
                    }
                    break;

                case OrbwalkingMode.None:
                    if (SpellManager.W.Ready && MenuConfig.Misc["W"].Enabled && Global.Player.CountEnemyHeroesInRange(1800) <= 0)
                    {
                        SpellManager.CastW();
                    }
                    break;
            }
        }

        public static void OnUpdate()
        {
            if (Global.Player.IsRecalling())
            {
                return;
            }

            if (SpellManager.E.Ready && MenuConfig.Misc["E"].Enabled && GameObjects.EnemyHeroes.Any(x => x.HasBuff(KalistaBuffName)))
            {
                var m = GameObjects.EnemyMinions.FirstOrDefault(x => x.HasBuff(KalistaBuffName) && x.Health < Dmg.EDmg(x) && x.IsValidTarget(SpellManager.E.Range));
                if (m != null)
                {
                    SpellManager.E.Cast();
                }
            }

            if (SpellManager.E.Ready &&
                GameObjects.Jungle.Count(x => x.HasBuff(KalistaBuffName) && Dmg.EDmg(x) > x.Health &&
                                              x.GetJungleType() != GameObjects.JungleType.Small) >= 1 && MenuConfig.JungleClear["E"].Enabled)
            {
                if (Global.Player.Level == 1 && Global.Player.CountAllyHeroesInRange(2000) >= 1)
                {
                    return;
                }
                SpellManager.E.Cast();
            }

            if (SpellManager.R.Ready && MenuConfig.Misc["R"].Enabled)
            {
                var soulbound = ObjectManager.Get<GameObject>().FirstOrDefault(x => x.Name == "Kalista_Base_P_LinkIcon.troy") as Obj_AI_Hero;
                if (soulbound != null && (soulbound.HealthPercent() <= MenuConfig.Misc["R"].Value ||
                                          soulbound.ChampionName == "Blitzcrank" && GameObjects.EnemyHeroes.Any(x => x.HasBuff("rocketgrab2"))))
                {
                    SpellManager.R.Cast();
                }
            }
        }

        public static void PreAttack(object sender, PreAttackEventArgs args)
        {
            if (!SpellManager.E.Ready)
            {
                return;
            }

            switch (Global.Orbwalker.Mode)
            {
                case OrbwalkingMode.Combo:
                {
                    var target = args.Target as Obj_AI_Hero;
                    if (target != null && target.HasBuff(KalistaBuffName))
                    {
                        var m = GameObjects.EnemyMinions.FirstOrDefault(x =>
                            x.IsValidAutoRange() && x.Health < Global.Player.GetAutoAttackDamage(x) + Global.Player.GetSpellDamage(x, SpellSlot.E));
                        if (m != null)
                        {
                            args.Target = m;
                            DebugConsole.WriteLine($"AUTO MINION TO SLOW TARGET", MessageState.Debug);
                        }
                    }
                }
                    break;
                case OrbwalkingMode.Laneclear:
                case OrbwalkingMode.Lasthit:

                    var minion = GameObjects.EnemyMinions.OrderBy(x => x.Health).FirstOrDefault(x => x.IsValidAutoRange());
                    if (minion == null || args.Target.NetworkId == minion.NetworkId || Global.Player.IsUnderAllyTurret())
                    {
                        return;
                    }

                    args.Target = minion;
                    DebugConsole.WriteLine($"Got new target {minion.UnitSkinName}", MessageState.Debug);
                    break;
            }
        }
    }
}