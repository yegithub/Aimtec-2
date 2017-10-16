namespace Adept_AIO.Champions.Zed.Miscellaneous
{
    using System.Linq;
    using Aimtec;
    using Aimtec.SDK.Extensions;
    using Core;
    using SDK.Delegates;
    using SDK.Unit_Extensions;

    class AntiGapcloser
    {
        public static void OnGapcloser(Obj_AI_Hero sender, GapcloserArgs args)
        {
            if (sender.IsMe || !sender.IsEnemy || args.EndPosition.Distance(Global.Player) > SpellManager.E.Range)
            {
                return;
            }

            if (SpellManager.W.Ready && MenuConfig.Misc["W"].Enabled)
            {
                var allyT = GameObjects.AllyTurrets.FirstOrDefault(x => x.IsValid);
                if (allyT != null && ShadowManager.CanCastW1())
                {
                    SpellManager.W.Cast(allyT.ServerPosition);
                }
                else if (ShadowManager.CanSwitchToShadow())
                {
                    SpellManager.W.Cast();
                }
            }

            else if (SpellManager.R.Ready)
            {
                var enemy = GameObjects.EnemyHeroes.OrderBy(x => x.Health).FirstOrDefault(x => x.IsValidTarget(SpellManager.R.Range));
                if (enemy == null)
                {
                    return;
                }

                SpellManager.R.Cast(enemy);
            }
        }
    }
}