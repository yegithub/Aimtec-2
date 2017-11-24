namespace Adept_AIO.Champions.Zoe.Miscellaneous
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
            if (sender.IsMe || !SpellManager.R.Ready || !sender.IsEnemy || args.EndPosition.Distance(Global.Player) > 350 || Global.Player.Distance(args.EndPosition) > Global.Player.Distance(args.StartPosition))
            {
                return;
            }

            var allyT = GameObjects.AllyTurrets.OrderBy(x => x.Distance(Global.Player)).FirstOrDefault(x => x.IsValid);
            if (allyT != null)
            {
                SpellManager.R.Cast(allyT.ServerPosition);
            }
        }
    }
}