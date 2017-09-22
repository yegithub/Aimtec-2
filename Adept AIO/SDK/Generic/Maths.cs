namespace Adept_AIO.SDK.Generic
{
    internal class Maths
    {
        public static int Percent(double value1, double value2, int multiplier = 100)
        {
            return (int)(value2 / value1 * multiplier);
        }
    }
}
