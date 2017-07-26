using System;
using System.Collections.Generic;
using System.Linq;
using Adept_AIO.SDK.Usables;
using Aimtec;
using Aimtec.SDK.Damage;
using Aimtec.SDK.Extensions;

namespace Adept_AIO.Champions.LeeSin.Core
{
    class WardManager
    {
        public static List<Obj_AI_Minion> JumpableObjects;

        public static float LastJumpTick;
        public static float LastWardCreated;
        private static Vector3 LastPosition;

        public static bool IsJumping => Environment.TickCount - LastJumpTick < Game.Ping / 2f;
        public static bool CanWardJump => CanCastWard && SpellConfig.W.Ready;
        public static bool CanCastWard => Environment.TickCount - LastJumpTick > Game.Ping / 2f && Extension.IsFirst(SpellConfig.W);

        public static bool IsWardReady => WardNames.Any(Items.CanUseItem);

        public static void OnUpdate()
        {
            if (IsJumping)
            {
                JumpToVector(LastPosition);
            }
        }

        public static void OnProcessSpellCast(Obj_AI_Base sender, Obj_AI_BaseMissileClientDataEventArgs args)
        {
            if (!sender.IsMe)
            {
                return;
            }

            if (args.SpellData.Name == ObjectManager.GetLocalPlayer().SpellBook.GetSpell(SpellSlot.W).Name &&
                args.SpellData.Name.ToLower().Contains("one"))
            {
                LastPosition = Vector3.Zero;
            }
        }

        public static Obj_AI_Minion GetBestObject(Vector3 position)
        {
            return JumpableObjects.Where(x => x.IsValid && !x.IsDead && ObjectManager.GetLocalPlayer().Distance(x) <= SpellConfig.W.Range)
                         .OrderBy(x => x.Distance(position))
                         .LastOrDefault(x => x.Distance(position) <= ObjectManager.GetLocalPlayer().Distance(position));
        }

        public static void WardJump(Vector3 position)
        {
            if (CanWardJump)
            {
                var end = ObjectManager.GetLocalPlayer().ServerPosition +
                          (position - ObjectManager.GetLocalPlayer().ServerPosition).Normalized() *
                          Math.Min(600, ObjectManager.GetLocalPlayer().Distance(position));

                foreach (var wardName in WardNames)
                {
                    if (!Items.CanUseItem(wardName) || Environment.TickCount - LastWardCreated < 1000)
                    {
                        continue;
                    }

                    Items.CastItem(wardName, end);
                    LastWardCreated = Environment.TickCount;
                    LastJumpTick = Environment.TickCount;
                    LastPosition = end;
                }
            }
        }

        public static void JumpToVector(Vector3 position)
        {
            if (Extension.IsFirst(SpellConfig.W))
            {
                var bestobject = GetBestObject(position);
                if (bestobject != null && position.Distance(bestobject.ServerPosition) < 600)
                {
                    SpellConfig.W.CastOnUnit(bestobject);
                }
            }
        }

        public static void OnCreate(GameObject sender)
        {
            if (sender.Name.ToLower().Contains("ward") && sender.IsAlly)
            {
                var ward = (Obj_AI_Minion) sender;
                JumpableObjects.Add(ward);
            }
        }

        public static void OnDestroy(GameObject sender)
        {
            if (sender.Name.ToLower().Contains("ward") && sender.IsAlly)
            {
                var ward = (Obj_AI_Minion)sender;
                JumpableObjects.Remove(ward);
            }
        }

        private static readonly uint[] WardsItems =
        {
            ItemId.RubySightstone,
            ItemId.Sightstone,
            ItemId.EyeoftheWatchers,
            ItemId.TrackersKnife,
            ItemId.GreaterStealthTotemTrinket,
            ItemId.GreaterVisionTotemTrinket,
            ItemId.ControlWard,
            ItemId.ExplorersWard,
        };

        private static readonly string[] WardNames =
        {
            ItemId.RubySightstone.ToString(),
            ItemId.Sightstone.ToString(),
            ItemId.EyeoftheWatchers.ToString(),
            ItemId.TrackersKnife.ToString(),
            ItemId.GreaterStealthTotemTrinket.ToString(),
            ItemId.GreaterVisionTotemTrinket.ToString(),
            ItemId.ControlWard.ToString(),
            ItemId.ExplorersWard.ToString(),
        };
    }
}
