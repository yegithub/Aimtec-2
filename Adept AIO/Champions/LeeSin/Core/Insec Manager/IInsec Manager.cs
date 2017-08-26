using Aimtec;

namespace Adept_AIO.Champions.LeeSin.Core.Insec_Manager
{
    internal interface IInsecManager
    {
        float DistanceBehindTarget(Obj_AI_Base target);
        float DistanceBehindTarget();

        Vector3 InsecQPosition { get; set; }
        Vector3 InsecWPosition { get; set; }

        Vector3 InsecPosition(Obj_AI_Base target);
        Vector3 BKPosition(Obj_AI_Base target);
        Vector3 GetTargetEndPosition();

        int InsecKickValue { get; set; }
        int InsecPositionValue { get; set; }
    }
}
