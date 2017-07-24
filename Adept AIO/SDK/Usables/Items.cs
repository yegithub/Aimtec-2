using System.Linq;
using Aimtec;

namespace Adept_AIO.SDK.Usables
{
    internal class Items
    {
        public static bool CastItem(uint item)
        {
            if (item == 0)
            {
                return false;
            }
            return false; // Fuck this shit.
        }

        public static bool CastItemHOTFIX(string name)
        {
            return ObjectManager.GetLocalPlayer()
                .SpellBook.Spells.Where(x => x.Name == name)
                .Select(x => ObjectManager.GetLocalPlayer().SpellBook.CastSpell(x.Slot))
                .FirstOrDefault();
        }
    }
}
