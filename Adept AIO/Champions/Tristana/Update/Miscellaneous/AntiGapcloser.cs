using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Adept_AIO.Champions.Tristana.Core;
using Adept_AIO.SDK.Delegates;
using Adept_AIO.SDK.Extensions;
using Aimtec;
using Aimtec.SDK.Extensions;

namespace Adept_AIO.Champions.Tristana.Update.Miscellaneous
{
    class AntiGapcloser
    {
        private readonly SpellConfig SpellConfig;

        public AntiGapcloser(SpellConfig spellConfig)
        {
            SpellConfig = spellConfig;
        }

        public void OnGapcloser(Obj_AI_Hero sender, GapcloserArgs args)
        {
            if (sender.IsMe || sender.IsAlly || args.EndPosition.Distance(Global.Player) > SpellConfig.FullRange)
            {
                return;
            }

            if (SpellConfig.R.Ready)
            {
                SpellConfig.R.CastOnUnit(sender);
            }
            else if (SpellConfig.W.Ready)
            {
                SpellConfig.W.Cast(Mixed.GetFountainPos(Global.Player));
            }
        }
    }
}
