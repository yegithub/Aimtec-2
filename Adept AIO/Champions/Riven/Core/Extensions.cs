using System.Collections.Generic;
using System.Linq;
using System.Threading;
using Adept_AIO.SDK.Extensions;
using Aimtec;
using Aimtec.SDK.Extensions;

namespace Adept_AIO.Champions.Riven.Core
{
    internal class Extensions
    {
        /// <summary>
        /// Fuck it
        /// </summary>
        /// <returns></returns>
        public static int GetRange()
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