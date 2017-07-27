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
        public static Vector3 LastPosition;
      
        public static bool IsWardReady => WardNames.Any(Items.CanUseItem);
        private static Obj_AI_Base RecentWard;

        public static void OnUpdate()
        {
            if (Environment.TickCount - LastWardCreated < 500)
            {
                JumpToVector(LastPosition);
            }

            if (Environment.TickCount - LastWardCreated > 1000)
            {
                LastPosition = Vector3.Zero;
            }
        }

        public static Obj_AI_Base GetBestObject(Vector3 position, bool insec = false)
        {
            if (RecentWard != null)
            {
                return RecentWard;
            }

            var wards = ObjectManager.Get<Obj_AI_Base>().Where(x => !x.IsHero && !x.IsMe && x.IsAlly && 
            ObjectManager.GetLocalPlayer().Distance(x) <= Math.Pow(700 + 120, 2))
                .OrderBy(x => x.Distance(position))
                .FirstOrDefault();

            if (wards != null)
            {
                return wards;
            }

            if (insec)
            {
                return null;
            }

            var minions = GameObjects.EnemyMinions.Where(x => x.IsAlly && x.IsValid && ObjectManager.GetLocalPlayer().Distance(x) <= Math.Pow(700 + 120, 2))
                .OrderBy(x => x.Distance(position))
                .FirstOrDefault();

            return minions;
        }

        public static void Jump(Vector3 position, bool insec = false)
        {
            var bestobject = GetBestObject(position, insec);
            if (!insec && bestobject != null && !bestobject.IsMe && bestobject.IsAlly && position.Distance(bestobject.ServerPosition) < 600)
            {
                JumpToVector(position);
            }
            else if (Environment.TickCount - LastWardCreated > 500)
            {
                if (!insec)
                {
                    position = ObjectManager.GetLocalPlayer().ServerPosition.Extend(position, 500);
                }

                foreach (var wardName in WardNames)
                {
                    if (!Items.CanUseItem(wardName))
                    {
                        continue;
                    }

                    Items.CastItem(wardName, position);
                    LastWardCreated = Environment.TickCount;
                    LastPosition = position;
                }
            }
        }

        public static void JumpToVector(Vector3 position)
        {
            if(!Extension.IsFirst(SpellConfig.W))
            {
                return;
            }

            var bestobject = GetBestObject(position);

            if (RecentWard != null)
            {
                SpellConfig.W.CastOnUnit(RecentWard);
                LastPosition = Vector3.Zero;
            }
            else if (bestobject != null && !bestobject.IsMe && bestobject.IsAlly)
            {
                SpellConfig.W.CastOnUnit(bestobject);
                LastPosition = Vector3.Zero;
            }
        }

        public static void OnCreate(GameObject sender)
        {
            if (sender == null || !sender.IsAlly || sender.Distance(ObjectManager.GetLocalPlayer()) > 600 || !WardNames.Contains(sender.Name))
            {
                return;
            }

            RecentWard = sender as Obj_AI_Base;
            LastPosition = sender.ServerPosition;
            LastWardCreated = Environment.TickCount;
            DelayAction.Queue(500, () => RecentWard = null);
        }

        private static readonly string[] WardNames =
        {
            "TrinketTotemLvl1",
            "ItemGhostWard",
            "JammerDevice",
        };
    }
}
