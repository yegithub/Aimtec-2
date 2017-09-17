using System;
using Adept_AIO.SDK.Junk;
using Aimtec;

namespace Adept_AIO.Champions.Riven.Core
{
    internal class Extensions
    {
        public static float FlashRange()
        {
            switch (Enums.BurstPattern)
            {
                case BurstPattern.TheShy:
                    return 650;

                case BurstPattern.Execution:
                    return 750;

                default: throw new ArgumentOutOfRangeException();
            }
        }

        public static int EngageRange
        {
            get
            {
                var range = 0f;

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

        public static Vector3 FleePos;

        
    }
}