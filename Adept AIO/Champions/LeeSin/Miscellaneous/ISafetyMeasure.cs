﻿namespace Adept_AIO.Champions.LeeSin.Miscellaneous
{
    using Aimtec;

    interface ISafetyMeasure
    {
        void OnProcessSpellCast(Obj_AI_Base sender, Obj_AI_BaseMissileClientDataEventArgs args);
    }
}