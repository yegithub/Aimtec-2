using System;
using Adept_AIO.SDK.Junk;
using Adept_AIO.SDK.Usables;

namespace Adept_AIO.Champions.Riven.Core
{
    internal class Extensions
    {
        public static float FlashRange()
        {
            switch (Enums.BurstPattern)
            {
                case BurstPattern.TheShy:
                    return SummonerSpells.Flash.Range + SpellConfig.W.Range + 35;

                case BurstPattern.Execution:
                    return 800;

                default: throw new ArgumentOutOfRangeException();
            }
        }

        public static int EngageRange
        {
            get
            {
                var range = 0f;

                //switch (Enums.ComboPattern)
                //{
                //    case ComboPattern.MaximizeDmg:
                //        break;
                //    case ComboPattern.FastCombo:
                //        break;
                //    case ComboPattern.FastCombo:
                //        break;
                //    default: throw new ArgumentOutOfRangeException();
                //}

                if (AllIn)
                {
                    range += 425;
                }
                else
                {
                    range += Global.Player.AttackRange;
                }

                if (SpellConfig.E.Ready)
                {
                    range += SpellConfig.E.Range - 50;
                }
                else if (SpellConfig.Q.Ready && !SpellConfig.E.Ready)
                {
                    range += SpellConfig.Q.Range;
                }

                return (int)range;
            }
        }

        public static bool DidJustAuto;
        public static bool AllIn;

        public static int CurrentQCount = 1;
        public static int LastQCastAttempt;

        public static string[] InvulnerableList = { "FioraW", "kindrednodeathbuff", "Undying Rage", "JudicatorIntervention" };
    }
}