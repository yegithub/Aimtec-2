using System.Linq;
using Adept_AIO.Champions.Kayn;
using Adept_AIO.SDK.Extensions;
using Adept_AIO.SDK.Usables;
using Aimtec;
using Aimtec.SDK.Events;

namespace Adept_AIO
{
    using Champions.Irelia;
    using Champions.Jax;
    using Champions.Rengar;
    using Champions.Riven;
    using Champions.Yasuo;

    internal class Bootstrap
    {
        private static void Main()
        {
            GameEvents.GameStart += GameEvents_GameStart;
        }

        private static readonly string[] Valid = { "Riven", "Irelia", "Jax", "Rengar", "Yasuo", "Kayn" };

        private static void GameEvents_GameStart()
        {
            if (Valid.All(x => ObjectManager.GetLocalPlayer().ChampionName != x))
            {
                return;
            }

            SummonerSpells.Init();
            GameObjects.Init();
            GlobalExtension.Init();

            switch (ObjectManager.GetLocalPlayer().ChampionName)
            {
                case "Irelia":
                    Irelia.Init();
                    break;
                case "Jax":
                    Jax.Init();
                    break;
                case "Kayn":
                    Kayn.Init();
                    break;
                case "Rengar":
                    Rengar.Init();
                    break;
                case "Riven":
                    Riven.Init();
                    break;
                case "Yasuo":
                    Yasuo.Init();
                    break;
            }
        }
    }
}