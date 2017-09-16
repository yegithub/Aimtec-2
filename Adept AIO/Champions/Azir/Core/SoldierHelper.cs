using System.Collections.Generic;
using Aimtec;

namespace Adept_AIO.Champions.Azir.Core
{
    class SoldierHelper
    {
        public static List<Obj_AI_Minion> Soldiers;

        private static bool IsSoldier(Obj_AI_Minion soldier)
        {
            return soldier.IsAlly && soldier.UnitSkinName.ToLower().Contains("soldier");
        }

        public static void OnDestroy(GameObject sender)
        {
            var potentialSoldier = sender as Obj_AI_Minion;

            if (sender != null && IsSoldier(potentialSoldier))
            {
                Soldiers.Add(potentialSoldier);
            }
        }

        public static void OnCreate(GameObject sender)
        {
            Soldiers.RemoveAll(x => x.NetworkId == sender.NetworkId);
        }
    }
}
