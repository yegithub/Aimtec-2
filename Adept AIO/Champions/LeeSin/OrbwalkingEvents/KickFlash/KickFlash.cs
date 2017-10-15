namespace Adept_AIO.Champions.LeeSin.OrbwalkingEvents.KickFlash
{
    using Aimtec;
    using Aimtec.SDK.Extensions;
    using Core.Insec_Manager;
    using Core.Spells;
    using SDK.Unit_Extensions;
    using SDK.Usables;

    class KickFlash : IKickFlash
    {
        private readonly IInsecManager _insecManager;
        private readonly ISpellConfig _spellConfig;

        public KickFlash(ISpellConfig spellConfig, IInsecManager insecManager)
        {
            _spellConfig = spellConfig;
            _insecManager = insecManager;
        }

        private Obj_AI_Hero Target => Global.TargetSelector.GetSelectedTarget();

        public void OnKeyPressed()
        {
            if (!this.Enabled ||
                this.Target == null ||
                !_spellConfig.R.Ready ||
                !this.Target.IsValidTarget(_spellConfig.R.Range) ||
                SummonerSpells.Flash == null ||
                !SummonerSpells.Flash.Ready)
            {
                return;
            }

            _spellConfig.R.CastOnUnit(this.Target);
        }

        public void OnProcessSpellCast(Obj_AI_Base sender, Obj_AI_BaseMissileClientDataEventArgs args)
        {
            if (sender == null || !sender.IsMe || args.SpellSlot != SpellSlot.R || !this.Enabled)
            {
                return;
            }

            SummonerSpells.Flash.Cast(_insecManager.InsecPosition(this.Target));
        }

        public bool Enabled { get; set; }
    }
}