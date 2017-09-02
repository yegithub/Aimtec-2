using System;
using System.Linq;
using Adept_AIO.SDK.Extensions;
using Adept_AIO.SDK.Methods;
using Adept_AIO.SDK.Usables;
using Aimtec;
using Aimtec.SDK.Extensions;

namespace Adept_AIO.Champions.LeeSin.Update.Ward_Manager
{
    internal class WardManager : IWardManager
    {
        private readonly IWardTracker _wardTracker; 

        public float LastTimeCasted { get; private set; }

        public WardManager(IWardTracker wardTracker)
        {
            _wardTracker = wardTracker;
        }

        public void WardJump(Vector3 position, int range)
        {
            if (Game.TickCount - _wardTracker.LastWardCreated < 500)
            {
                return;
            }

            var ward = _wardTracker.Ward();
            
            if (ward == null)
            {
                DebugConsole.Print("DEBUG: [Warning] There are no wards. Failed to continue.", ConsoleColor.Yellow);
                return;
            }

            position = Global.Player.ServerPosition.Extend(position, range);

             LastTimeCasted = Game.TickCount;
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
