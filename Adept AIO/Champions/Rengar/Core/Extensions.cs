using Aimtec;
using Aimtec.SDK.Extensions;

namespace Adept_AIO.Champions.Rengar.Core
{
    internal class Extensions
    {
        public static Obj_AI_Hero AssassinTarget = null;

        public static bool HardCC()
        {
            var me = ObjectManager.GetLocalPlayer();
            return (me.HasBuffOfType(BuffType.Blind) ||
                    me.HasBuffOfType(BuffType.Charm) ||
                    me.HasBuffOfType(BuffType.Fear)  ||
                    me.HasBuffOfType(BuffType.Knockback) ||
                    me.HasBuffOfType(BuffType.Silence) ||
                    me.HasBuffOfType(BuffType.Stun)    ||
                    me.HasBuffOfType(BuffType.Taunt))  &&
                   !me.HasBuff("sorakapacify");
        }

        public static int Ferocity()
        {
            return (int) ObjectManager.GetLocalPlayer().Mana;
        }

        public static float ShieldPercent()
        {
            var player = ObjectManager.GetLocalPlayer();
            return 100 * (player.AllShield / player.MaxHealth);
        }
    }
}
