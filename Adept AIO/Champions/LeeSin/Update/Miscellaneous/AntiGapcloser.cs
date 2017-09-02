using Adept_AIO.Champions.LeeSin.Core.Spells;
using Adept_AIO.Champions.LeeSin.Update.Ward_Manager;
using Adept_AIO.SDK.Delegates;
using Adept_AIO.SDK.Extensions;
using Adept_AIO.SDK.Methods;
using Aimtec;
using Aimtec.SDK.Extensions;

namespace Adept_AIO.Champions.LeeSin.Update.Miscellaneous
{
    internal class AntiGapcloser
    {
        private readonly ISpellConfig _spellConfig;
        private readonly IWardManager _wardManager;
        private readonly IWardTracker _wardTracker;

        public AntiGapcloser(ISpellConfig spellConfig, IWardManager wardManager, IWardTracker wardTracker)
        {
            _spellConfig = spellConfig;
            _wardManager = wardManager;
            _wardTracker = wardTracker;
        }

        public void OnGapcloser(Obj_AI_Hero sender, GapcloserArgs args)
        {
            if (sender.IsMe 
            || !sender.IsEnemy 
            || !_spellConfig.W.Ready 
            || !_spellConfig.IsFirst(_spellConfig.W)
            || !_wardTracker.IsWardReady() 
            ||  args.EndPosition.Distance(Global.Player) > _spellConfig.WardRange)
            {
                return;
            }

            var missile = SpellDatabase.GetByName(args.SpellName);
            if (missile == null || !missile.IsDangerous)
            {
                return;
            }

            _wardManager.WardJump(Game.CursorPos, _spellConfig.WardRange);
        }
    }
}
