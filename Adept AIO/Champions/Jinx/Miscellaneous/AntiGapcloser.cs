﻿using Adept_AIO.Champions.Jinx.Core;
using Adept_AIO.SDK.Delegates;
using Adept_AIO.SDK.Unit_Extensions;
using Aimtec;
using Aimtec.SDK.Extensions;

namespace Adept_AIO.Champions.Jinx.Miscellaneous
{
    internal class AntiGapcloser
    {
        private readonly SpellConfig _spellConfig;
      
        public AntiGapcloser(SpellConfig spellConfig)
        {
            _spellConfig = spellConfig;
        }

        public void OnGapcloser(Obj_AI_Hero sender, GapcloserArgs args)
        {
            if (!sender.IsEnemy || !_spellConfig.E.Ready || args.EndPosition.Distance(Global.Player) > _spellConfig.E.Range )
            {
                return;
            }

            _spellConfig.E.Cast(args.EndPosition);
        }
    }
}
