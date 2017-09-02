using System;
using System.Collections.Generic;
using System.Linq;
using Adept_AIO.Champions.LeeSin.Core.Spells;
using Adept_AIO.SDK.Extensions;
using Adept_AIO.SDK.Methods;
using Adept_AIO.SDK.Usables;
using Aimtec;
using Aimtec.SDK.Extensions;

namespace Adept_AIO.Champions.LeeSin.Update.Ward_Manager
{
    public class WardTracker : IWardTracker
    {
        private readonly ISpellConfig _spellConfig;

        public WardTracker(ISpellConfig spellConfig)
        {
            _spellConfig = spellConfig;
        }

        public bool ActiveWardInSlot()
        {
            return _wardNames.Any(Items.CanUseItem);
        }

        public bool IsWardReady()
        {
            return ActiveWardInSlot() && Game.TickCount - LastWardCreated > 500;
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
            var ward = sender as Obj_AI_Minion;

            if (ward == null || WardPosition.Distance(ward.Position) > 700 ||
                Game.TickCount - LastWardCreated > 800 ||
                !_spellConfig.IsFirst(_spellConfig.W) || !IsAtWall)
            {
                return;
            }
     
            if (ward.Team != GameObjectTeam.Neutral && ward.Name.ToLower().Contains("ward"))
            {
                DebugConsole.Print("Located Ally Ward.", ConsoleColor.Green);
                LastWardCreated = Game.TickCount;
                WardName = ward.Name;
                WardPosition = ward.Position;
                Global.Player.SpellBook.CastSpell(SpellSlot.W, sender.Position);
            }
            else
            {
                DebugConsole.Print("Could Not Locate Ally Ward. Object: " + ward.Name, ConsoleColor.Yellow);
            }
        }

        public bool IsAtWall { get; set; }

        public float LastWardCreated { get; set; }

        public string WardName { get; private set; }

        public Vector3 WardPosition { get; set; }
    }
}
