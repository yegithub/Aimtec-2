namespace Adept_AIO.Champions.Xerath.Miscellaneous
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
            if (!SpellManager.E.Ready || !sender.IsEnemy || args.EndPosition.Distance(Global.Player) > SpellManager.E.Range || Global.Player.Distance(args.EndPosition) > Global.Player.Distance(args.StartPosition))
            {
                return;
            }

            var rect = SpellManager.ERect(sender);
            if (rect == null)
            {
                return;
            }

            if (GameObjects.EnemyMinions.OrderBy(x => x.Distance(Global.Player)).Any(x => rect.IsInside(x.ServerPosition.To2D()) && Global.Player.Distance(x) < Global.Player.Distance(args.EndPosition)))
            {
                return; // Collision check.
            }

            SpellManager.E.Cast(args.EndPosition);
        }
    }
}