namespace Adept_AIO.Champions.Twitch
{
    using System;
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

            SpellBook.OnCastSpell += OnCastSpell;

            Render.OnPresent += DrawManager.OnPresent;
            Render.OnRender += DrawManager.OnRender;
           
            Game.OnUpdate += Manager.OnUpdate;
        }

        private static void OnCastSpell(Obj_AI_Base sender, SpellBookCastSpellEventArgs args)
        {
            if (sender.IsMe && SpellManager.Q.Ready && args.Slot == SpellSlot.Recall && MenuConfig.mainMenu["Stealth"].Enabled)
            {
                SpellManager.Q.Cast();
            }
        }
    }
}