namespace Adept_AIO.Champions.Graves.Miscellaneous
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
            if (!SpellManager.E.Ready || !sender.IsEnemy || args.EndPosition.Distance(Global.Player) > SpellManager.E.Range)
            {
                return;
            }

            var pos = Global.Player.ServerPosition + (Global.Player.ServerPosition - args.EndPosition).Normalized() * SpellManager.E.Range;
            SpellManager.E.Cast(pos);
        }
    }
}