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

        public bool IsWardReady()
        {
            return _wardTracker.WardNames.Any(Items.CanUseItem) && Game.TickCount - _wardTracker.LastWardCreated > 1500;
        }

        public WardManager(IWardTracker wardTracker)
        {
            _wardTracker = wardTracker;
        }

        public void WardJump(Vector3 position, int range)
        {
            if (Game.TickCount - _wardTracker.LastWardCreated < 1500)
            {
                DebugConsole.Print("DEBUG: [Warning] Just Created A Ward. Failed to continue.", ConsoleColor.Yellow); 
                return;
            }

            if (!_wardTracker.WardNames.Any(Items.CanUseItem))
            {
                DebugConsole.Print("DEBUG: [Error] CANNOT CAST ANY WARD AT ALL.", ConsoleColor.Red);
            }

            var ward = _wardTracker.WardNames.FirstOrDefault(Items.CanUseItem);
            
            if (ward == null)
            {
                return;
            }

            position = Global.Player.ServerPosition.Extend(position, range);

             LastTimeCasted = Game.TickCount;
            _wardTracker.LastWardCreated = Game.TickCount;
            _wardTracker.WardPosition = position;

            DebugConsole.Print("DEBUG: Calling 'CastItem(...)' Method!");
          
            Items.CastItem(ward, position);

            if (!NavMesh.WorldToCell(position).Flags.HasFlag(NavCellFlags.Wall))
            {
                _wardTracker.IsAtWall = false;
                Global.Player.SpellBook.CastSpell(SpellSlot.W, position);
            }
            else
            {
                _wardTracker.IsAtWall = true;
            }
        }
    }
}
