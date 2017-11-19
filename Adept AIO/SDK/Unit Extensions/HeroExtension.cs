namespace Adept_AIO.SDK.Unit_Extensions
{
    using System.Linq;
    using Aimtec;
    using Aimtec.SDK.Damage;
    using Aimtec.SDK.Extensions;

    public static class HeroExtension
    {
        private static readonly BuffType[] HardCc =
        {
            BuffType.Flee,
            BuffType.Taunt,
            BuffType.Suppression,
            BuffType.Snare,
            BuffType.Stun,
            BuffType.Charm,
            BuffType.Blind,
            BuffType.Fear,
            BuffType.Knockup,
            BuffType.Polymorph
        };

        private static readonly uint[] TearId =
        {
            ItemId.TearoftheGoddess,
            ItemId.Manamune,
            ItemId.ArchangelsStaff,
            ItemId.TearoftheGoddessQuickCharge,
            ItemId.ArchangelsStaffQuickCharge
        };

        public static bool IsAirbone(this Obj_AI_Base unit)
        {
            return unit.HasBuffOfType(BuffType.Knockup) || unit.HasBuffOfType(BuffType.Knockback);
        }

        public static bool HasTear()
        {
            return TearId.Any(u => Global.Player.HasItem(u));
        }

        public static bool IsHardCc(this Obj_AI_Hero target)
        {
            return HardCc.Select(target.HasBuffOfType).FirstOrDefault();
        }

        public static Vector3 GetFountainPos(this GameObject target)
        {
            switch (Game.MapId)
            {
                case GameMapId.SummonersRift: return target.Team == GameObjectTeam.Order ? new Vector3(396, 185.1325f, 462) : new Vector3(14340, 171.9777f, 14390);

                case GameMapId.TwistedTreeline: return target.Team == GameObjectTeam.Order ? new Vector3(1058, 150.8638f, 7297) : new Vector3(14320, 151.9291f, 7235);
            }
            return Vector3.Zero;
        }
    }
}