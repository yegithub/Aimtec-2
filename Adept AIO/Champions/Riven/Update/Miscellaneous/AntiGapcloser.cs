using Adept_AIO.Champions.Riven.Core;
using Adept_AIO.SDK.Delegates;
using Adept_AIO.SDK.Junk;
using Aimtec;
using Aimtec.SDK.Extensions;

namespace Adept_AIO.Champions.Riven.Update.Miscellaneous
{
    internal class AntiGapcloser
    {
        public static void OnGapcloser(Obj_AI_Hero sender, GapcloserArgs args)
        {
            if (!sender.IsEnemy)
            {
                return;
            }

            var pos = Global.Player.ServerPosition + (Global.Player.ServerPosition - args.EndPosition).Normalized();

            if (SpellConfig.E.Ready && args.EndPosition.Distance(Global.Player) < 65)
            {
                SpellConfig.E.Cast(pos);
            }
            else if (SpellConfig.W.Ready && SpellManager.InsideKiBurst(sender.ServerPosition, sender.BoundingRadius))
            {
                SpellConfig.W.Cast();
            }
        }
    }
}
