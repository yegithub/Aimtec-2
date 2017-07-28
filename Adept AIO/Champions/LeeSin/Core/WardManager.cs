using System;
using System.Linq;
using Adept_AIO.SDK.Extensions;
using Adept_AIO.SDK.Usables;
using Aimtec;
using Aimtec.SDK.Extensions;

namespace Adept_AIO.Champions.LeeSin.Core
{
    internal class WardManager
    {
        public static float LastWardCreated;
        public static Vector3 WardPosition;
       
        public static bool IsWardReady => WardNames.Any(Items.CanUseItem);

        public static void OnUpdate()
        {
            if (Extension.IsFirst(SpellConfig.W) && Environment.TickCount - LastWardCreated < 1500 && LastWardCreated > 0 && Environment.TickCount - LastWardCreated > Game.Ping + 5)
            {
                JumpToVector(WardPosition);
            }
            else if (Environment.TickCount - LastWardCreated > 1500)
            {
                WardPosition = Vector3.Zero;
                LastWardCreated = 0;
            }
        }

        public static Obj_AI_Minion GetBestObject(Vector3 position, bool allowMinions = true)
        {
            var wards = GameObjects.AllyWards.Where(x => x.IsValid).OrderBy(x => x.Distance(position)).FirstOrDefault(x => x.Distance(position) <= 600 && ObjectManager.GetLocalPlayer().Distance(x) <= 600 && x.Distance(WardPosition) <= 10);

            if (wards != null)
            {
                return wards;
            }

            var minions = GameObjects.EnemyMinions.Where(x => ObjectManager.GetLocalPlayer().Distance(x) <= SpellConfig.W.Range && x.Distance(position) <= 400)
                .OrderBy(x => x.Distance(position))
                .FirstOrDefault();

            return allowMinions ? minions : null;
        }

        public static void WardJump(Vector3 position, bool maxRange)
        {
            if (Environment.TickCount - LastWardCreated < 1500)
            {
                return;
            }

            if (maxRange)
            {
                position = ObjectManager.GetLocalPlayer().ServerPosition.Extend(position, 490);
            }

            foreach (var wardName in WardNames)
            {
                if (!Items.CanUseItem(wardName))
                {
                    continue;
                }

                LastWardCreated = Environment.TickCount;
                WardPosition = position;
                Items.CastItem(wardName, position);
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
