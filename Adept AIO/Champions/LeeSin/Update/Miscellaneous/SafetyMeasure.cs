using Adept_AIO.Champions.LeeSin.Core;
using Aimtec;

namespace Adept_AIO.Champions.LeeSin.Update.Miscellaneous
{
    class SafetyMeasure
    {
        public static void OnProcessSpellCast(Obj_AI_Base sender, Obj_AI_BaseMissileClientDataEventArgs args)
        {
            if (!MenuConfig.Miscellaneous["Interrupt"].Enabled ||
                !SpellConfig.R.Ready ||
                args == null ||
                args.Sender == null ||
                sender == null ||
                args.SpellData == null ||
                sender.IsMe ||            // Yes. Something prints null. so i check everything. Leave. Me. Alone.
                sender.IsAlly ||
                args.Sender.IsAlly ||
                !args.Sender.IsHero ||
                args.SpellData.ChannelDuration <= 0) // Todo: look into this
            {
                return;
            }

            SpellConfig.R.CastOnUnit(sender);
        }
    }
}
