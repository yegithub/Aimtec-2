using System;
using System.Collections.Generic;
using System.Linq;
using Adept_AIO.Champions.LeeSin.Core.Spells;
using Adept_AIO.SDK.Junk;
using Adept_AIO.SDK.Methods;
using Adept_AIO.SDK.Usables;
using Aimtec;

namespace Adept_AIO.Champions.LeeSin.Update.Ward_Manager
{
    public class WardTracker : IWardTracker
    {
        private readonly ISpellConfig _spellConfig;

        public WardTracker(ISpellConfig spellConfig)
        {
            _spellConfig = spellConfig;
        }

        public bool DidJustWard => Game.TickCount - LastWardCreated <= 800 + Game.Ping / 2f;

        public bool IsWardReady()
        {
            return _wardNames.Any(Items.CanUseItem);
        }

        public string Ward()
        {
            return _wardNames.FirstOrDefault(Items.CanUseItem);
        }

        private readonly IEnumerable<string> _wardNames = new List<string>
        {
            "TrinketTotemLvl1",
            "ItemGhostWard",
            "JammerDevice",
        };

        public void OnCreate(GameObject sender)
        {
            if (DidJustWard || !_spellConfig.IsFirst(_spellConfig.W) || !IsAtWall)
            {
                return;
            }

            var ward = sender as Obj_AI_Minion;

            if (ward != null && ward.Name.ToLower().Contains("ward"))
            {
                LastWardCreated = Game.TickCount;
                WardName = ward.Name;
                WardPosition = ward.ServerPosition;

                DebugConsole.Print("Located Ally Ward.", ConsoleColor.Green);
                Global.Player.SpellBook.CastSpell(SpellSlot.W, WardPosition); // Bug: This position is unrealistic and does not work.
            }
        }

        public bool IsAtWall { get; set; }

        public float LastWardCreated { get; set; }

        public string WardName { get; private set; }

        public Vector3 WardPosition { get; set; }
    }
}
