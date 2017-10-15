namespace Adept_AIO.Champions.Twitch
{
    using Aimtec;
    using Core;
    using Drawings;
    using Miscellaneous;

    class Twitch
    {
        public Twitch()
        {
            new SpellManager();
            new MenuConfig();

            Render.OnPresent += DrawManager.OnPresent;
            Render.OnRender += DrawManager.OnRender;

            Game.OnUpdate += Manager.OnUpdate;
        }
    }
}