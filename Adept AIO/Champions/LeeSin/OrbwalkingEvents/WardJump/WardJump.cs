using Adept_AIO.Champions.LeeSin.Core.Spells;
using Adept_AIO.Champions.LeeSin.Ward_Manager;
using Adept_AIO.SDK.Unit_Extensions;
using Aimtec;
using Aimtec.SDK.Extensions;

namespace Adept_AIO.Champions.LeeSin.OrbwalkingEvents.WardJump
{
    internal class WardJump : IWardJump
    {
        public bool Enabled { get; set; }
     
        private readonly IWardTracker _wardTracker;

        private readonly IWardManager _wardManager;

        private readonly ISpellConfig _spellConfig;

        public WardJump(IWardTracker wardTracker, IWardManager wardManager, ISpellConfig spellConfig)
        {
            _wardTracker = wardTracker;
            _wardManager = wardManager;
            _spellConfig = spellConfig;
        }

        public void OnKeyPressed()
        {
            if (!Enabled)
            {
                return;
            }

            if (_spellConfig.W.Ready && _spellConfig.IsFirst(_spellConfig.W) && _wardTracker.IsWardReady())
            {
                var cursorDist = (int) Global.Player.Distance(Game.CursorPos);
                var dist = cursorDist <= _spellConfig.WardRange ? cursorDist : _spellConfig.WardRange; 
                _wardManager.WardJump(Game.CursorPos,  dist);
            }
        }
    }
}
