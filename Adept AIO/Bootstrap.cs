using Adept_AIO.Champions.Azir;
using Adept_AIO.Champions.Ezreal;
using Adept_AIO.SDK.Junk;

namespace Adept_AIO
{
    using System.Linq;
    using SDK.Usables;
    using Aimtec.SDK.Events;

    using Champions.Jinx;
    using Champions.Tristana;
    using Champions.Kayn;
    using Champions.LeeSin;
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

        private static readonly string[] Valid = { "Ezreal", "Azir", "Riven", "Irelia", "Jax", "Rengar", "Yasuo", "Kayn", "LeeSin", "Jinx", "Tristana" };

        private static void GameEvents_GameStart()
        {
            if (Valid.All(x => Global.Player.ChampionName != x))
            {
                return;
            }

            SummonerSpells.Init();
            GameObjects.Init();
            Global.Init();
            GetRandom.Init();

            switch (Global.Player.ChampionName)
            {
                case "Ezreal":
                    Ezreal.Init();
                    break;
                case "Azir":
                    Azir.Init();
                    break;
                case "Irelia":
                    Irelia.Init();
                    break;
                case "Jax":
                    Jax.Init();
                    break;
                case "Jinx":
                    var jinx = new Jinx();
                    jinx.Init();
                    break;
                case "Kayn":
                    Kayn.Init();
                    break;
                case "LeeSin":
                    var lee = new LeeSin();
                    lee.Init();
                    break;
                case "Rengar":
                    Rengar.Init();
                    break;
                case "Riven":
                    Riven.Init();
                    break;
                case "Tristana":
                    var tristana = new Tristana();
                    tristana.Init();
                    break;
                case "Yasuo":
                    Yasuo.Init();
                    break;
            }
        }
    }
}