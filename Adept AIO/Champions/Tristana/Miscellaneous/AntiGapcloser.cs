namespace Adept_AIO.Champions.Tristana.Miscellaneous
{
    using Aimtec;
    using Aimtec.SDK.Extensions;
    using Core;
    using SDK.Delegates;
    using SDK.Spell_DB;
    using SDK.Unit_Extensions;

    class AntiGapcloser
    {
        private readonly SpellConfig _spellConfig;

        public AntiGapcloser(SpellConfig spellConfig)
        {
            _spellConfig = spellConfig;
        }

        public void OnGapcloser(Obj_AI_Hero sender, GapcloserArgs args)
        {
            if (sender.IsMe || sender.IsAlly || args.EndPosition.Distance(Global.Player) > _spellConfig.FullRange)
            {
                return;
            }

            var missile = SpellDatabase.GetByName(args.SpellName);
            if (missile == null || !missile.IsDangerous)
            {
                return;
            }

            if (_spellConfig.W.Ready)
            {
                _spellConfig.W.Cast(Global.Player.GetFountainPos());
            }
            else if (_spellConfig.R.Ready)
            {
                _spellConfig.R.CastOnUnit(sender);
            }
        }
    }
}