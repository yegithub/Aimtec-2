namespace Adept_AIO.Champions.Vayne.Miscellaneous
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
            if (!sender.IsEnemy || sender.Distance(Global.Player) > SpellManager.E.Range)
            {
                return;
            }

            if (SpellManager.Q.Ready && MenuConfig.Misc["Q"].Enabled)
            {
                var pos = Global.Player.ServerPosition + (Global.Player.ServerPosition - args.EndPosition).Normalized() * SpellManager.Q.Range;
                SpellManager.Q.Cast(pos);
            }

            if (SpellManager.E.Ready && MenuConfig.Misc["E"].Enabled && args.EndPosition.Distance(Global.Player) < args.StartPosition.Distance(Global.Player))
            {
                SpellManager.E.CastOnUnit(sender);
            }
        }
    }
}