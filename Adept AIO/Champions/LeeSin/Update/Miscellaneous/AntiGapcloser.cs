using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Adept_AIO.Champions.LeeSin.Core.Spells;
using Adept_AIO.Champions.LeeSin.Update.Ward_Manager;
using Adept_AIO.SDK.Delegates;
using Adept_AIO.SDK.Extensions;
using Aimtec;
using Aimtec.SDK.Extensions;

namespace Adept_AIO.Champions.LeeSin.Update.Miscellaneous
{
    class AntiGapcloser
    {
        private readonly ISpellConfig SpellConfig;
        private readonly IWardManager WardManager;

        public AntiGapcloser(ISpellConfig spellConfig, IWardManager wardManager)
        {
            SpellConfig = spellConfig;
            WardManager = wardManager;
        }

        public void OnGapcloser(Obj_AI_Hero sender, GapcloserArgs args)
        {
            if (sender.IsMe 
            || !sender.IsEnemy 
            || !SpellConfig.W.Ready 
            || !SpellConfig.IsFirst(SpellConfig.W)
            || !WardManager.IsWardReady() 
            ||  args.EndPosition.Distance(Global.Player) > 600)
            {
                return;
            }

            WardManager.WardJump(Game.CursorPos, 600);
        }
    }
}
