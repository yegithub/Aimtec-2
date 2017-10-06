using Aimtec;

namespace Adept_AIO.Champions.LeeSin.Miscellaneous
{
    internal interface ISafetyMeasure
    {
        void OnProcessSpellCast(Obj_AI_Base sender, Obj_AI_BaseMissileClientDataEventArgs args);
    }
}