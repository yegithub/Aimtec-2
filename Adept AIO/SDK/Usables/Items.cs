using System;
using System.Linq;
using Adept_AIO.SDK.Extensions;
using Aimtec;

namespace Adept_AIO.SDK.Usables
{
    internal class Items
    {
        private static readonly string[] Tiamats = {"ItemTiamatCleave", "ItemTitanicHydraCleave", "ItemTiamatCleave"};
        public static float TiamatCastTime;

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

        public static void CastTiamat(bool cancelAA = true)
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

            if (cancelAA)
            {
                Global.Orbwalker.ResetAutoAttackTimer();
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
