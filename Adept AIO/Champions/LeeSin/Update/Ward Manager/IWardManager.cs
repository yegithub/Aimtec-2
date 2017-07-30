using Aimtec;

namespace Adept_AIO.Champions.LeeSin.Update.Ward_Manager
{
    internal interface IWardManager
    {
        Obj_AI_Minion LocateObject(Vector3 position, bool allowMinions = true);
        void WardJump(Vector3 position, bool maxRange);

        bool IsWardReady();
    }
}