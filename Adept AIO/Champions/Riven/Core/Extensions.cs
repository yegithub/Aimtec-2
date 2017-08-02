using Adept_AIO.SDK.Extensions;

namespace Adept_AIO.Champions.Riven.Core
{
    internal class Extensions
    {
        public static float FlashRange()
        {
            if (AllIn)
            {
                return (425 + SpellConfig.W.Range + 120);
            }
            return 0;
        }

        public static int EngageRange()
        {
            var range = 0f;
           
            if (AllIn)
            {
                range += 425;
            }
            else
            {
                range += GlobalExtension.Player.AttackRange;
            }
            if (SpellConfig.E.Ready)
            {
                range += SpellConfig.E.Range;
            }
            else if (SpellConfig.Q.Ready && !SpellConfig.E.Ready)
            {
                range += SpellConfig.Q.Range;
            }
          
            return (int)range;
        }

        public static bool DidJustAuto;
        public static bool AllIn;
        public static int CurrentQCount = 1;
        public static int LastQCastAttempt;

        public static string[] InvulnerableList = { "FioraW", "kindrednodeathbuff", "Undying Rage", "JudicatorIntervention" };
   
        public static HarassPattern Current;
        public static UltimateMode UltimateMode;
    }

    public enum HarassPattern
    {
        SemiCombo = 0, // Semi Combo
        AvoidTarget = 1, // Avoid target
        BackToTarget = 2, // Back to target
    }

    public enum UltimateMode
    {
        First,
        Second
    }
}