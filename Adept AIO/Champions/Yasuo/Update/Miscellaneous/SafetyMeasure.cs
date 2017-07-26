using Adept_AIO.Champions.Yasuo.Core;
using Aimtec;
using Aimtec.SDK.Extensions;

namespace Adept_AIO.Champions.Yasuo.Update.Miscellaneous
{
    internal class SafetyMeasure
    {
        public static void OnProcessSpellCast(Obj_AI_Base sender, Obj_AI_BaseMissileClientDataEventArgs args)
        {
            if (!MenuConfig.Combo["Dodge"].Enabled ||
               !SpellConfig.W.Ready ||
                args == null ||
                args.Sender == null ||
                sender == null ||
                args.SpellData == null ||
                 sender.IsMe ||            // Yes. Something prints null. so i check everything. Leave. Me. Alone.
                 sender.IsAlly || 
                 args.Sender.IsAlly ||
                !args.Sender.IsHero ||
                args.Target.IsMe && sender.Distance(ObjectManager.GetLocalPlayer()) < 150 || 
                ObjectManager.GetLocalPlayer().HasBuff("YasuoPassive") && args.SpellData.ConsideredAsAutoAttack)
            {
                return;
            }

            if (args.End.Distance(ObjectManager.GetLocalPlayer().ServerPosition) <= 250)
            {
                SpellConfig.W.Cast(sender.ServerPosition);
            }
        }
    }
}
