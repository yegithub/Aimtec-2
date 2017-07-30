using Aimtec;
using Aimtec.SDK.Orbwalking;
using Aimtec.SDK.Prediction.Health;
using Aimtec.SDK.TargetSelector;

namespace Adept_AIO.SDK.Extensions
{
    internal class GlobalExtension
    {
        public static IOrbwalker Orbwalker;
        public static ITargetSelector TargetSelector;
        public static IHealthPrediction HealthPrediction;
        public static Obj_AI_Hero Player = ObjectManager.GetLocalPlayer();
      
        public static void Init()
        {
            Orbwalker = new Orbwalker();
            TargetSelector = Aimtec.SDK.TargetSelector.TargetSelector.Implementation;
            HealthPrediction = new HealthPrediction();
        }
    }
}
