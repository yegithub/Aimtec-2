namespace Adept_AIO.Champions.LeeSin.Ward_Manager
{
    using Aimtec;
    using Aimtec.SDK.Extensions;
    using SDK.Generic;
    using SDK.Unit_Extensions;
    using SDK.Usables;

    class WardManager : IWardManager
    {
        private readonly IWardTracker _wardTracker;

        public WardManager(IWardTracker wardTracker)
        {
            _wardTracker = wardTracker;
        }

        public float LastTimeCasted { get; private set; }

        public void WardJump(Vector3 position, int range)
        {
            if (Game.TickCount - _wardTracker.LastWardCreated < 500)
            {
                return;
            }

            var ward = _wardTracker.Ward();

            if (ward == null)
            {
                DebugConsole.WriteLine("There are no wards. Failed to continue.", MessageState.Warn);
                return;
            }

            position = Global.Player.ServerPosition.Extend(position, range);

            this.LastTimeCasted = Game.TickCount;
            _wardTracker.LastWardCreated = Game.TickCount;
            _wardTracker.WardPosition = position;

            Items.CastItem(ward, position);

            if (NavMesh.WorldToCell(position).Flags.HasFlag(NavCellFlags.Wall))
            {
                _wardTracker.IsAtWall = true;
            }
            else
            {
                _wardTracker.IsAtWall = false;
                Global.Player.SpellBook.CastSpell(SpellSlot.W, position);
            }
        }
    }
}