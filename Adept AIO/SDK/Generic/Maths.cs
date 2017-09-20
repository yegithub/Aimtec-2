using Aimtec;

namespace Adept_AIO.SDK.Generic
{
    class Maths
    {
        public static int PercentDmg(Obj_AI_Base target, double dmg)
        {
            return (int)(dmg / target.Health * 100);
        }
    }
}
