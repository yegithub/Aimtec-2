namespace Adept_AIO.Champions.Riven.Core
{
    public class Enums
    {
        public static HarassPattern Current;
        public static UltimateMode UltimateMode;
        public static ComboPattern ComboPattern;
        public static BurstPattern BurstPattern;
    }

    public enum HarassPattern
    {
        SemiCombo = 0,
        AvoidTarget = 1,
        BackToTarget = 2
    }

    public enum UltimateMode
    {
        First,
        Second
    }

    public enum ComboPattern // Use .MaxHealth & Get % dmg of MaxHealth. Use that % to determine combo
    {
        MaximizeDmg,
        Normal,
        FastCombo
    }

    public enum BurstPattern
    {
        TheShy,
        Execution // R -> ER (1s Delay) -> Flash -> WQ
    }
}