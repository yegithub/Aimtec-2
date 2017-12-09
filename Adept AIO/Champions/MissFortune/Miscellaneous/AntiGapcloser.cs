namespace Adept_AIO.Champions.MissFortune.Miscellaneous
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
            if (!sender.IsEnemy || args.EndPosition.Distance(Global.Player) > SpellManager.E.Range)
            {
                return;
            }
            if (SpellManager.E.Ready && sender.IsValidTarget(SpellManager.E.Range))
            {
                SpellManager.E.Cast(args.EndPosition);
            }
        }
    }
}