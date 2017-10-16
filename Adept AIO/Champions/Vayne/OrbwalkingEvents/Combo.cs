namespace Adept_AIO.Champions.Vayne.OrbwalkingEvents
{
    using System.Linq;
    using System.Threading;
    using Aimtec;
    using Aimtec.SDK.Extensions;
    using Aimtec.SDK.Orbwalking;
    using Aimtec.SDK.Util;
    using Core;
    using SDK.Unit_Extensions;
    using SDK.Usables;

    class Combo
    {
        public static void PostAttack(object sender, PostAttackEventArgs args)
        {
            if (!SpellManager.Q.Ready || MenuConfig.Combo["Q1"].Value == 1)
            {
                return;
            }

            SpellManager.CastQ(args.Target as Obj_AI_Base, MenuConfig.Combo["Mode1"].Value, MenuConfig.Combo["ToE"].Enabled);
        }

        public static void OnUpdate()
        {
            var target = Global.TargetSelector.GetTarget(1000);
            if (!target.IsValidTarget())
            {
                return;
            }

            if (MenuConfig.Combo["Flash"].Enabled && SpellManager.E.Ready && SummonerSpells.IsValid(SummonerSpells.Flash))
            {
                var allyT = GameObjects.AllyTurrets.FirstOrDefault(x => x.Distance(target) <= 700);
                if (allyT != null)
                {
                    var pos = target.ServerPosition + (target.ServerPosition - allyT.ServerPosition).Normalized() * 200;
                    if (pos.Distance(Global.Player) <= 425)
                    {
                        SpellManager.E.CastOnUnit(target);
                        DelayAction.Queue(100, () => SummonerSpells.Flash.Cast(pos), new CancellationToken(false));
                    }
                }
            }

            if (SpellManager.Q.Ready && MenuConfig.Combo["Q1"].Value == 1)
            {
                SpellManager.CastQ(target, MenuConfig.Combo["Mode1"].Value, MenuConfig.Combo["ToE"].Enabled);
            }

            else if (SpellManager.E.Ready && MenuConfig.Combo["E"].Enabled && MenuConfig.Combo[target.ChampionName].Enabled)
            {
                SpellManager.CastE(target);
            }

            else if (SpellManager.R.Ready && MenuConfig.Combo["R"].Enabled)
            {
                if (target.Health > Dmg.Damage(target) && MenuConfig.Combo["Killable"].Enabled)
                {
                    return;
                }

                if (Global.Player.CountEnemyHeroesInRange(1500) >= MenuConfig.Combo["Count"].Value && target.HealthPercent() >= 25)
                {
                    SpellManager.R.Cast();
                }
            }
        }
    }
}