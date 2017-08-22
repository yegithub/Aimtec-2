using Adept_AIO.Champions.Jinx.Core;
using Adept_AIO.SDK.Delegates;
using Adept_AIO.SDK.Extensions;
using Aimtec;
using Aimtec.SDK.Extensions;

namespace Adept_AIO.Champions.Jinx.Update.Miscellaneous
{
    internal class AntiGapcloser
    {
        private readonly SpellConfig SpellConfig;
      
        public AntiGapcloser(SpellConfig spellConfig)
        {
            SpellConfig = spellConfig;
        }

        public void OnGapcloser(Obj_AI_Hero sender, GapcloserArgs args)
        {
            if (sender.IsMe || !sender.IsEnemy || !SpellConfig.E.Ready || args.EndPosition.Distance(Global.Player) > SpellConfig.E.Range)
            {
                return;
            }

            SpellConfig.E.Cast(args.EndPosition);
        }
    }
}
