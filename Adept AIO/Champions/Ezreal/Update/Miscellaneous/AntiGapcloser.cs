using System.Linq;
using Adept_AIO.Champions.Ezreal.Core;
using Adept_AIO.SDK.Delegates;
using Adept_AIO.SDK.Unit_Extensions;
using Aimtec;
using Aimtec.SDK.Extensions;

namespace Adept_AIO.Champions.Ezreal.Update.Miscellaneous
{
    internal class AntiGapcloser
    {
        public static void OnGapcloser(Obj_AI_Hero sender, GapcloserArgs args)
        {
            if (!sender.IsEnemy || !SpellConfig.E.Ready || args.EndPosition.Distance(Global.Player) > SpellConfig.E.Range)
            {
                return;
            }

            var allyTurret = GameObjects.AllyTurrets.FirstOrDefault(x => x.IsValid);

            SpellConfig.E.Cast(allyTurret != null ? allyTurret.ServerPosition : Game.CursorPos);
        }
    }
}
