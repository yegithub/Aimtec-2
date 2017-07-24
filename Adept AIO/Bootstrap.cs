using System.Linq;
using Adept_AIO.Champions.Irelia;
using Adept_AIO.Champions.Jax;
using Adept_AIO.Champions.Riven;
using Adept_AIO.SDK.Usables;
using Aimtec;
using Aimtec.SDK.Events;
using Adept_AIO.Champions.Rengar;
using Adept_AIO.Champions.Yasuo;

namespace Adept_AIO
{
    internal class Bootstrap
    {
        private static void Main(string[] args)
        {
            GameEvents.GameStart += GameEvents_GameStart;
        }

        private static void GameEvents_GameStart()
        {
            if (Valid.All(x => ObjectManager.GetLocalPlayer().ChampionName != x))
            {
                return;
            }

           
            
            switch (ObjectManager.GetLocalPlayer().ChampionName)
            {
                case "Riven":
                    Riven.Init();
                    break;
                case "Irelia":
                    Irelia.Init();
                    break;
                case "Jax":
                    Jax.Init();
                    break;
                case "Rengar":
                    Rengar.Init();
                    break;
                case "Yasuo":
                    Yasuo.Init();
                    break;
            }

            SummonerSpells.Init();
        }

        private static readonly string[] Valid = { "Riven", "Irelia", "Jax", "Rengar", "Yasuo"};
    }
}