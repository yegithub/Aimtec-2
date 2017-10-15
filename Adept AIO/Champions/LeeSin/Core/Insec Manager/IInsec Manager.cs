namespace Adept_AIO.Champions.LeeSin.Core.Insec_Manager
{
    using Aimtec;

    interface IInsecManager
    {
        int InsecKickValue { get; set; }
        int InsecPositionValue { get; set; }
        float DistanceBehindTarget(Obj_AI_Base target);

        Vector3 InsecPosition(Obj_AI_Base target);
        Vector3 BkPosition(Obj_AI_Base target);
        Vector3 GetTargetEndPosition();
    }
}