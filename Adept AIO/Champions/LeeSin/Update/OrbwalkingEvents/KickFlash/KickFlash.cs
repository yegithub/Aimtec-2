using Adept_AIO.Champions.LeeSin.Core.Insec_Manager;
using Adept_AIO.Champions.LeeSin.Core.Spells;
using Adept_AIO.SDK.Extensions;
using Adept_AIO.SDK.Usables;
using Aimtec;
using Aimtec.SDK.Extensions;

namespace Adept_AIO.Champions.LeeSin.Update.OrbwalkingEvents.KickFlash
{
    internal class KickFlash : IKickFlash
    {
        private readonly ISpellConfig _spellConfig;
        private readonly IInsecManager _insecManager;

        public KickFlash(ISpellConfig spellConfig, IInsecManager insecManager)
        {
            _spellConfig = spellConfig;
            _insecManager = insecManager;
        }

        public void OnKeyPressed()
        {
            if (!Enabled || 
                Target == null || 
                !_spellConfig.R.Ready || 
                !Target.IsValidTarget(_spellConfig.R.Range) || 
                SummonerSpells.Flash == null || 
                !SummonerSpells.Flash.Ready)
            {
                return;
            }

            _spellConfig.R.CastOnUnit(Target);
        }

        public void OnProcessSpellCast(Obj_AI_Base sender, Obj_AI_BaseMissileClientDataEventArgs args)
        {
            if (sender == null || !sender.IsMe || args.SpellSlot != SpellSlot.R || !Enabled)
            {
                return;
            }

            SummonerSpells.Flash.Cast(_insecManager.InsecPosition(Target));
        }

        public bool Enabled { get; set; }

        private Obj_AI_Hero Target => Global.TargetSelector.GetSelectedTarget();
    }
}
