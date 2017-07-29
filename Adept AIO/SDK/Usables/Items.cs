using System;
using System.Linq;
using Aimtec;

namespace Adept_AIO.SDK.Usables
{
    internal class Items
    {
        private static readonly string[] Tiamats = {"ItemTiamatCleave", "ItemTitanicHydraCleave", "ItemTiamatCleave"};
        public static float TiamatCastTime;
        public static void CastTiamat()
        {
            SpellSlot? slot = null;
          
            foreach (var tiamat in Tiamats)
            {
                if (CanUseItem(tiamat))
                {
                    slot = GetItemSlot(tiamat);
                }
            }
            
            if (slot != null)
            {
                ObjectManager.GetLocalPlayer().SpellBook.CastSpell((SpellSlot) slot);
                TiamatCastTime = Environment.TickCount;
            }
        }

        public static void CastItem(string itemName, Vector3 position = new Vector3())
        {
            var slot = GetItemSlot(itemName);
            if (!CanUseItem(itemName))
            {
                return;
            }

            if (position == Vector3.Zero)
            {
                ObjectManager.GetLocalPlayer().SpellBook.CastSpell(slot);
            }
            else
            {
                ObjectManager.GetLocalPlayer().SpellBook.CastSpell(slot, position);
            }
        }

        public static bool CanUseItem(string itemName)
        {
            var slot = GetItemSlot(itemName);
        
            if (slot != SpellSlot.Unknown)
            {
                return ObjectManager.GetLocalPlayer().SpellBook.GetSpellState(slot) == SpellState.Ready;
            }
            return false;
        }

        private static SpellSlot GetItemSlot(string itemName)
        {
            var slot = ObjectManager.GetLocalPlayer().Inventory.Slots.FirstOrDefault(x => string.Equals(itemName, x.SpellName, StringComparison.CurrentCultureIgnoreCase));
           
            if (slot != null && slot.SpellSlot != SpellSlot.Unknown)
            {
                return slot.SpellSlot;
            }
            return SpellSlot.Unknown;
        }
    }
}
