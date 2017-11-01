namespace Adept_AIO.SDK.Usables
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Aimtec;
    using Unit_Extensions;

    class Items
    {
        private static readonly string[] Tiamats =
        {
            "ItemTiamatCleave", "ItemTitanicHydraCleave", "ItemTiamatCleave"
        };

        public static float TiamatCastTime;

        private static readonly IEnumerable<string> WardNames = new List<string>
        {
            "TrinketTotemLvl1", "ItemGhostWard", "JammerDevice"
        };

        public static bool CanUseTiamat()
        {
            SpellSlot? slot = null;

            foreach (var tiamat in Tiamats)
            {
                if (CanUseItem(tiamat))
                {
                    slot = GetItemSlot(tiamat);
                }
            }

            return slot != null;
        }

        public static void CastTiamat(bool cancelAa = true)
        {
            SpellSlot? slot = null;

            foreach (var tiamat in Tiamats)
            {
                if (CanUseItem(tiamat))
                {
                    slot = GetItemSlot(tiamat);
                }
            }

            if (slot == null)
            {
                return;
            }

            Global.Player.SpellBook.CastSpell((SpellSlot) slot);
            TiamatCastTime = Game.TickCount;

            if (cancelAa)
            {
                Global.Orbwalker.ResetAutoAttackTimer();
            }
        }

        public static void WardJump(Vector3 position)
        {
            foreach (var wardName in WardNames)
            {
                if (CanUseItem(wardName))
                {
                    CastItem(wardName, position);
                }
            }
        }

        public static void CastItem(string itemName, Vector3 position = new Vector3())
        {
            var slot = GetItemSlot(itemName);

            if (!CanUseItem(itemName))
            {
                return;
            }

            if (position.IsZero)
            {
                Global.Player.SpellBook.CastSpell(slot);
            }
            else
            {
                Global.Player.SpellBook.CastSpell(slot, position);
            }
        }

        public static bool CanUseItem(string itemName)
        {
            var slot = GetItemSlot(itemName);

            if (slot != SpellSlot.Unknown)
            {
                return Global.Player.SpellBook.GetSpellState(slot) == SpellState.Ready;
            }
            return false;
        }

        private static SpellSlot GetItemSlot(string itemName)
        {
            var slot = Global.Player.SpellBook.Spells.FirstOrDefault(x => !string.IsNullOrEmpty(x.Name) && string.Equals(x.Name, itemName, StringComparison.CurrentCultureIgnoreCase));

            return slot?.Slot ?? SpellSlot.Unknown;
        }
    }
}