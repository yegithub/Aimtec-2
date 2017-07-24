using System.Linq;
using Aimtec;
using Spell = Aimtec.SDK.Spell;

namespace Adept_AIO.SDK.Usables
{
    internal class SummonerSpells
    {
        public static Spell Flash, Ignite, Smite, Exhaust;

        public static int IgniteDamage = 50 + 20 * ObjectManager.GetLocalPlayer().Level;

        //Todo: Improve this bullshit.

        public static void Init()
        {
            var spellbookName1 = ObjectManager.GetLocalPlayer().SpellBook.GetSpell(SpellSlot.Summoner1).Name.ToLower();
            var spellbookName2 = ObjectManager.GetLocalPlayer().SpellBook.GetSpell(SpellSlot.Summoner2).Name.ToLower();

            switch (spellbookName1)
            {
                case "summonerflash":
                    Flash = new Spell(SpellSlot.Summoner1, 425);
                    break;
                case "summonerdot":
                    Ignite = new Spell(SpellSlot.Summoner1, 600);
                    break;
                case "summonerexhaust":
                    Exhaust = new Spell(SpellSlot.Summoner1, 650);
                    break;
                case "summonersmite":
                    Smite = new Spell(SpellSlot.Summoner1, 700);
                    break;
            }

            switch (spellbookName2)
            {
                case "summonerflash":
                    Flash = new Spell(SpellSlot.Summoner2, 425);
                    break;
                case "summonerdot":
                    Ignite = new Spell(SpellSlot.Summoner2, 600);
                    break;
                case "summonerexhaust":
                    Exhaust = new Spell(SpellSlot.Summoner2, 650);
                    break;
                case "summonersmite":
                    Smite = new Spell(SpellSlot.Summoner1, 700);
                    break;
            }
        }
    }
}
