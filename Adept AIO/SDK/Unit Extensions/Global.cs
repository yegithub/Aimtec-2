﻿namespace Adept_AIO.SDK.Unit_Extensions
{
    using Aimtec;
    using Aimtec.SDK.Orbwalking;
    using Aimtec.SDK.Prediction.Health;
    using Aimtec.SDK.TargetSelector;
    using Draw_Extension;

    class Global
    {
        public static IOrbwalker Orbwalker;
        public static ITargetSelector TargetSelector;
        public static IHealthPrediction HealthPrediction;
        public static Obj_AI_Hero Player = ObjectManager.GetLocalPlayer();
        public static DamageIndicator DamageIndicator;

        public Global()
        {
            Orbwalker = new Orbwalker();
            TargetSelector = Aimtec.SDK.TargetSelector.TargetSelector.Implementation;
            HealthPrediction = new HealthPrediction();
            DamageIndicator = new DamageIndicator();
        }
    }
}