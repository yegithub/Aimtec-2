using Aimtec;

namespace Adept_AIO.Champions.LeeSin.Core.Insec_Manager
{
    internal interface IInsec_Manager
    {
        float DistanceBehindTarget(Obj_AI_Base target);

        Vector3 InsecQPosition { get; set; }
        Vector3 InsecWPosition { get; set; }
        Vector3 InsecPosition(Obj_AI_Base target);
        Vector3 GetTargetEndPosition();

        int InsecKickValue { get; set; }
        int InsecPositionValue { get; set; }
    }
}
