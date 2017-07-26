using System.Threading;
using Aimtec;

namespace Adept_AIO.Champions.Riven.Core
{
    internal class Extensions
    {
        public static int FlashRange(Obj_AI_Base target)
        {
            if (AllIn)
            {
                return (int) (425 + target.BoundingRadius + SpellConfig.W.Range);
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
                range += ObjectManager.GetLocalPlayer().AttackRange;
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

        public static bool AllIn;
        public static int CurrentQCount = 1;
        public static float LastQTime = 0;
        public static float LastETime = 0;
        public static float LastWTime = 0;
        public static bool AttackedStructure;
        public static string[] InvulnerableList = { "FioraW", "kindrednodeathbuff", "Undying Rage", "JudicatorIntervention" };
       // public static IEnumerable<Obj_AI_Hero> Enemies = GameObjects.EnemyHeroes.Where(x => x.IsValidTarget(GetRange()));
        public static HarassPattern Current;
        public static UltimateMode UltimateMode;
        public static CancellationToken CancellationToken;
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