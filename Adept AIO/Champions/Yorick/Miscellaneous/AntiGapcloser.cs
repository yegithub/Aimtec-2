namespace Adept_AIO.Champions.Yorick.Miscellaneous
{
    using Aimtec;
    using Aimtec.SDK.Extensions;
    using Core;
    using SDK.Delegates;
    using SDK.Unit_Extensions;

    class AntiGapcloser
    {
        public static void OnGapcloser(Obj_AI_Hero sender, GapcloserArgs args)
        {
            if (!SpellManager.W.Ready || !sender.IsEnemy || args.EndPosition.Distance(Global.Player) > SpellManager.W.Range)
            {
                return;
            }

      
            SpellManager.E.Cast(args.EndPosition);
        }
    }
}