namespace Adept_AIO.Champions.Jhin.Miscellaneous
{
    using Aimtec;
    using Aimtec.SDK.Extensions;
    using Core;
    using SDK.Delegates;
    using SDK.Unit_Extensions;

    class AntiGapcloser
    {
        public AntiGapcloser()
        {
            Gapcloser.OnGapcloser += OnGapcloser;
        }

        private static void OnGapcloser(Obj_AI_Hero sender, GapcloserArgs args)
        {
            if (!sender.IsEnemy || args.EndPosition.Distance(Global.Player) > SpellManager.E.Range + 500 || sender.Distance(Global.Player) < Global.Player.AttackRange + 200)
            {
                return;
            }

            if (SpellManager.E.Ready && MenuConfig.Misc["E"].Enabled && sender.IsValidTarget(SpellManager.E.Range))
            {
                SpellManager.E.Cast(args.EndPosition);
            }
        }
    }
}