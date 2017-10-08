using Aimtec;
using Aimtec.SDK.Prediction.Health;
using Aimtec.SDK.TargetSelector;

namespace Adept_BaseUlt.Local_SDK
{
    internal class Global
    {
        public static ITargetSelector TargetSelector;
        public static IHealthPrediction HealthPrediction;
        public static Obj_AI_Hero Player = ObjectManager.GetLocalPlayer();
      
        public static void Init()
        {
            TargetSelector = Aimtec.SDK.TargetSelector.TargetSelector.Implementation;
            HealthPrediction = new HealthPrediction();
        }

        //public static Recalls Recall;
        //public static List<Obj_AI_Hero> LastEnemyChecked;

        //public static int TimeUntilCastingUlt = -1;
        //public static int LastSeenTick;

        //public static Vector3 LastSeenPosition;
        //public static Vector3 PredictedPosition;
        //public static Vector3 CastPos;

        //public static Spell Ultimate;

        //public static float Speed;
        //public static float Width;
        //public static float Delay;
        //public static float Range;
        //public static int MaxCollisionObjects;
    }
}
