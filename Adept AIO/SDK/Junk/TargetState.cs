using System.Linq;
using Aimtec;
using Aimtec.SDK.Extensions;

namespace Adept_AIO.SDK.Junk
{
    internal class TargetState
    {
        private static readonly BuffType[] HardCc = { BuffType.Invulnerability, BuffType.Charm, BuffType.Blind, BuffType.Fear, BuffType.Knockup, BuffType.Polymorph };

        public static bool IsHardCc(Obj_AI_Hero target)
        {
            return HardCc.Select(target.HasBuffOfType).FirstOrDefault();
        }
    }
}
