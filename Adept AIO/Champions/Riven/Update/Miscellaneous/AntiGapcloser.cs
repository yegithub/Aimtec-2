using Adept_AIO.Champions.Riven.Core;
using Adept_AIO.SDK.Delegates;
using Adept_AIO.SDK.Extensions;
using Aimtec;
using Aimtec.SDK.Extensions;

namespace Adept_AIO.Champions.Riven.Update.Miscellaneous
{
    class AntiGapcloser
    {
        public static void OnGapcloser(Obj_AI_Hero sender, GapcloserArgs args)
        {
            if (sender.IsMe || !sender.IsEnemy || args.EndPosition.Distance(Global.Player) > SpellConfig.E.Range)
            {
                return;
            }

            var pos = Global.Player.ServerPosition + (Global.Player.ServerPosition - args.EndPosition).Normalized();

            if (SpellConfig.E.Ready)
            {
                SpellConfig.E.Cast(pos);
            }
            else if (SpellConfig.W.Ready && args.EndPosition.Distance(Global.Player) <= SpellConfig.W.Range)
            {
                SpellConfig.W.Cast();
            }
        }
    }
}
