namespace Adept_BaseUlt.Local_SDK
{
    using Aimtec;
    using Aimtec.SDK.Prediction.Health;
    using Aimtec.SDK.TargetSelector;

    class Global
    {
        public static ITargetSelector TargetSelector;
        public static IHealthPrediction HealthPrediction;
        public static Obj_AI_Hero Player = ObjectManager.GetLocalPlayer();

        public static void Init()
        {
            TargetSelector = Aimtec.SDK.TargetSelector.TargetSelector.Implementation;
            HealthPrediction = new HealthPrediction();
        }
    }
}