using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Adept_AIO.Champions.Yasuo.Core;
using Adept_AIO.SDK.Delegates;
using Adept_AIO.SDK.Extensions;
using Aimtec;
using Aimtec.SDK.Extensions;

namespace Adept_AIO.Champions.Yasuo.Update.Miscellaneous
{
    class AntiGapcloser
    {
        public static void OnGapcloser(Obj_AI_Hero sender, GapcloserArgs args)
        {
            if (sender.IsMe || !sender.IsEnemy || args.EndPosition.Distance(Global.Player) > SpellConfig.E.Range)
            {
                return;
            }

            var minion = GameObjects.Minions.Where(x => x.Distance(Global.Player) <= SpellConfig.E.Range && !x.HasBuff("YasuoDashWrapper")).OrderBy(x => x.Distance(Game.CursorPos)).FirstOrDefault();

            if (SpellConfig.E.Ready && minion != null)
            {
                SpellConfig.E.CastOnUnit(minion);
            }
        }
    }
}
