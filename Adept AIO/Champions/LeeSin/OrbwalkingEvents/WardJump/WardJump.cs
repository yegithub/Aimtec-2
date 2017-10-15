namespace Adept_AIO.Champions.LeeSin.OrbwalkingEvents.WardJump
{
    using Aimtec;
    using Aimtec.SDK.Extensions;
    using Core.Spells;
    using SDK.Unit_Extensions;
    using Ward_Manager;

    class WardJump : IWardJump
    {
        private readonly ISpellConfig _spellConfig;

        private readonly IWardManager _wardManager;

        private readonly IWardTracker _wardTracker;

        public WardJump(IWardTracker wardTracker, IWardManager wardManager, ISpellConfig spellConfig)
        {
            _wardTracker = wardTracker;
            _wardManager = wardManager;
            _spellConfig = spellConfig;
        }

        public bool Enabled { get; set; }

        public void OnKeyPressed()
        {
            if (!this.Enabled)
            {
                return;
            }

            if (_spellConfig.W.Ready && _spellConfig.IsFirst(_spellConfig.W) && _wardTracker.IsWardReady())
            {
                var cursorDist = (int) Global.Player.Distance(Game.CursorPos);
                var dist = cursorDist <= _spellConfig.WardRange ? cursorDist : _spellConfig.WardRange;
                _wardManager.WardJump(Game.CursorPos, dist);
            }
        }
    }
}