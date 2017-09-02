using Aimtec;

namespace Adept_AIO.Champions.LeeSin.Core.Insec_Manager
{
    internal interface IInsecManager
    {
        float DistanceBehindTarget(Obj_AI_Base target);
      
        Vector3 InsecPosition(Obj_AI_Base target);
        Vector3 BkPosition(Obj_AI_Base target);
        Vector3 GetTargetEndPosition();

        int InsecKickValue { get; set; }
        int InsecPositionValue { get; set; }
    }
}
