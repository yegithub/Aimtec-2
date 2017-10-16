namespace Adept_AIO.Champions.Azir.Miscellaneous
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
            if (!sender.IsEnemy)
            {
                return;
            }
            var soldier = SoldierManager.Soldiers.OrderBy(x => x.Distance(Game.CursorPos)).FirstOrDefault();
            if (SpellConfig.E.Ready && soldier != null && soldier.Distance(Global.Player) - soldier.BoundingRadius < SpellConfig.E.Range)
            {
                SpellConfig.E.CastOnUnit(soldier);
            }
            else if (SpellConfig.R.Ready && args.EndPosition.Distance(Global.Player) < SpellConfig.R.Range)
            {
                SpellConfig.R.Cast(sender.ServerPosition);
            }
        }
    }
}