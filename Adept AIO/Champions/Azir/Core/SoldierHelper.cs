using System.Collections.Generic;
using System.Linq;
using Adept_AIO.SDK.Junk;
using Aimtec;
using Aimtec.SDK.Extensions;

namespace Adept_AIO.Champions.Azir.Core
{
    class SoldierHelper
    {
        public static List<Obj_AI_Minion> Soldiers;


        private static bool IsSoldier(Obj_AI_Minion soldier)
        {
            if(soldier == null)
            {
                return false;
            }
            return soldier.IsAlly && soldier.UnitSkinName.ToLower().Contains("soldier");
        }

        public static void OnDestroy(GameObject sender)
        {
            Soldiers.RemoveAll(x => x.NetworkId == sender.NetworkId);
        }

        public static void OnCreate(GameObject sender)
        {
            var soldier = sender as Obj_AI_Minion;
            if (soldier != null && IsSoldier(soldier) && Game.TickCount - AzirHelper.LastR >= 100)
            {
                Soldiers.Add(soldier);
            }
        }

        public static Vector3 GetSoldierNearestTo(Vector3 pos)
        {
            foreach (var minion in GameObjects.AllyMinions.Where(IsSoldier))
            {
                var soldier = Soldiers.OrderBy(x => x.Distance(pos)).FirstOrDefault(x => x.NetworkId == minion.NetworkId);
                return soldier != null ? soldier.ServerPosition : Vector3.Zero;
            }
            return Vector3.Zero;
        }
    }
}
