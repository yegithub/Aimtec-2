using System.Linq;
using Aimtec;
using Aimtec.SDK.Extensions;

namespace Adept_AIO.SDK.Extensions
{
    internal class TargetState
    {
        private static readonly BuffType[] HardCC = { BuffType.Invulnerability, BuffType.Charm, BuffType.Blind, BuffType.Fear, BuffType.Knockup, BuffType.Polymorph };

        public static bool IsHardCC(Obj_AI_Hero target)
        {
            return HardCC.Select(target.HasBuffOfType).FirstOrDefault();
        }
    }
}
