namespace Adept_AIO.SDK.Menu_Extension
{
    using Aimtec.SDK.Menu;
    using Aimtec.SDK.Menu.Components;

    class MenuShortcut
    {
        public static Menu Credits = new Menu("Credits", "Credits")
        {
            new MenuSeperator("WhyAreYouReadingThis", "Written by: Nechrito | Haki | Adept"), new MenuSeperator("ThisStringIsUtterlyUseless", "Platform: LeageTec 2017"),
            new MenuSeperator("pp", "Paypal: Nechrito@live.se")
        };
    }
}