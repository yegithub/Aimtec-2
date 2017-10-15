namespace Adept_AIO
{
    using System.Linq;
    using Aimtec.SDK.Events;
    using Champions.Azir;
    using Champions.Ezreal;
    using Champions.Gragas;
    using Champions.Irelia;
    using Champions.Jax;
    using Champions.Jinx;
    using Champions.Kayn;
    using Champions.LeeSin;
    using Champions.Lucian;
    using Champions.Rengar;
    using Champions.Riven;
    using Champions.Tristana;
    using Champions.Twitch;
    using Champions.Vayne;
    using Champions.Yasuo;
    using Champions.Zed;
    using SDK.Generic;
    using SDK.Unit_Extensions;
    using SDK.Usables;

    class Bootstrap
    {
        private static readonly string[] Valid =
        {
            "Twitch",
            "Lucian",
            "Gragas",
            "Ezreal",
            "Azir",
            "Riven",
            "Irelia",
            "Jax",
            "Rengar",
            "Yasuo",
            "Kayn",
            "LeeSin",
            "Jinx",
            "Tristana",
            "Zed",
            "Vayne"
        };

        private static void Main() { GameEvents.GameStart += GameEvents_GameStart; }

        private static void GameEvents_GameStart()
        {
            if (Valid.All(x => Global.Player.ChampionName != x))
            {
                return;
            }

            new SummonerSpells();
            GameObjects.Init();
            new Global();
            new GetRandom();

            switch (Global.Player.ChampionName)
            {
                case "Twitch":
                    new Twitch();
                    break;
                case "Lucian":
                    new Lucian();
                    break;
                case "Gragas":
                    Gragas.Init();
                    break;
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
                case "Zed":
                    Zed.Init();
                    break;
                case "Vayne":
                    Vayne.Init();
                    break;
            }
        }
    }
}