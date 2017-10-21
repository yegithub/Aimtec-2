namespace Adept_AIO.Champions.Riven.Miscellaneous
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
            if (!sender.IsEnemy)
            {
                return;
            }

            if (SpellConfig.E.Ready && args.EndPosition.Distance(Global.Player) <= 100)
            {
                var pos = Global.Player.ServerPosition + (Global.Player.ServerPosition - args.EndPosition).Normalized() * SpellConfig.E.Range;
                SpellConfig.E.Cast(pos);
            }

            if (SpellConfig.W.Ready && sender.IsValidTarget(SpellConfig.W.Range + 75))
            {
                SpellManager.CastW(sender);
            }
        }
    }
}