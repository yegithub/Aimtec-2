using System;
using System.Collections.Generic;
using System.Linq;
using Adept_AIO.SDK.Extensions;
using Adept_AIO.SDK.Usables;
using Aimtec;
using Aimtec.SDK.Extensions;
using Aimtec.SDK.Util;

namespace Adept_AIO.Champions.LeeSin.Core
{
    class WardManager
    {
        public static float LastWardCreated;
        public static Vector3 WardPosition;
       
        public static bool IsWardReady => WardNames.Any(Items.CanUseItem);

        public static Obj_AI_Base Ward(Vector3 Position)
        {
            return ObjectManager.Get<Obj_AI_Base>()
                .Where(x => x.IsAlly && x.Name.ToLower().Contains("ward"))
                .OrderBy(x => x.Distance(Position) < 920)
                .FirstOrDefault();
        }

        public static void OnUpdate()
        {
            if (Environment.TickCount - LastWardCreated < 1000 && WardPosition != Vector3.Zero)
            {
                JumpToVector(WardPosition);
            }
            else if (Environment.TickCount - LastWardCreated > 1000)
            {
                WardPosition = Vector3.Zero;
            }
        }

        public static Obj_AI_Minion GetBestObject(Vector3 position, bool allowMinions = true)
        {
            var wards = GameObjects.AllyWards.FirstOrDefault(x => ObjectManager.GetLocalPlayer().Distance(x) <= SpellConfig.W.Range && x.Distance(position) <= 920);

            if (wards != null)
            {
                return wards;
            }

            if (Extension.InsecMode.Active && (SummonerSpells.Flash == null || !SummonerSpells.Flash.Ready))
            {
                return null;
            }

            var minions = GameObjects.EnemyMinions.Where(x => ObjectManager.GetLocalPlayer().Distance(x) <= SpellConfig.W.Range && x.Distance(position) <= 150)
                .OrderBy(x => x.Distance(position))
                .FirstOrDefault();

            return allowMinions ? minions : null;
        }

        public static void WardJump(Vector3 position, bool maxRange)
        {
            if (Environment.TickCount - LastWardCreated < 500)
            {
                return;
            }

            if (maxRange)
            {
                position = ObjectManager.GetLocalPlayer().ServerPosition.Extend(position, 600);
            }

            foreach (var wardName in WardNames)
            {
                if (!Items.CanUseItem(wardName))
                {
                    continue;
                }

                Items.CastItem(wardName, position);
                LastWardCreated = Environment.TickCount;
                WardPosition = position;
            }
        }

        public static void JumpToVector(Vector3 position)
        {
            if(!Extension.IsFirst(SpellConfig.W))
            {
                return;
            }

            var bestobject = GetBestObject(position);

            if (bestobject != null && !bestobject.IsMe)
            {
                SpellConfig.W.CastOnUnit(bestobject);
            }
        }

        private static readonly string[] WardNames =
        {
            "TrinketTotemLvl1",
            "ItemGhostWard",
            "JammerDevice",
        };
    }
}
