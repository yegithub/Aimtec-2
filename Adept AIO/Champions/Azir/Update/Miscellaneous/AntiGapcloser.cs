using System.Linq;
using Adept_AIO.Champions.Azir.Core;
using Adept_AIO.SDK.Delegates;
using Adept_AIO.SDK.Unit_Extensions;
using Aimtec;
using Aimtec.SDK.Extensions;

namespace Adept_AIO.Champions.Azir.Update.Miscellaneous
{
    internal class AntiGapcloser
    {
        public static void OnGapcloser(Obj_AI_Hero sender, GapcloserArgs args)
        {
            if (!sender.IsEnemy)
            {
                return;
            }
            var soldier = SoldierHelper.Soldiers.OrderBy(x => x.Distance(Game.CursorPos)).FirstOrDefault();
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
