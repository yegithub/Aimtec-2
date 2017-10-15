using Adept_AIO.Champions.Vayne.Core;
using Adept_AIO.SDK.Delegates;
using Adept_AIO.SDK.Unit_Extensions;
using Aimtec;
using Aimtec.SDK.Extensions;

namespace Adept_AIO.Champions.Vayne.Miscellaneous
{
    internal class AntiGapcloser
    {
        public static void OnGapcloser(Obj_AI_Hero sender, GapcloserArgs args)
        {
            if (!sender.IsEnemy || args.EndPosition.Distance(Global.Player) > SpellManager.E.Range)
            {
                return;
            }

            if (SpellManager.Q.Ready && MenuConfig.Misc["Q"].Enabled)
            {
                var pos = Global.Player.ServerPosition + (Global.Player.ServerPosition - args.EndPosition).Normalized() * SpellManager.Q.Range;
                SpellManager.Q.Cast(pos);
            }

            else if (SpellManager.E.Ready && MenuConfig.Misc["E"].Enabled && args.EndPosition.Distance(Global.Player) < args.StartPosition.Distance(Global.Player) && args.EndPosition.Distance(Global.Player) <= 150)
            {
                SpellManager.E.CastOnUnit(sender);
            }
        }
    }
}
