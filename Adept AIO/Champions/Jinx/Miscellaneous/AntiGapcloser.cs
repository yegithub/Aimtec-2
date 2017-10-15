namespace Adept_AIO.Champions.Jinx.Miscellaneous
{
    using Aimtec;
    using Aimtec.SDK.Extensions;
    using Core;
    using SDK.Delegates;
    using SDK.Unit_Extensions;

    class AntiGapcloser
    {
        private readonly SpellConfig _spellConfig;

        public AntiGapcloser(SpellConfig spellConfig) { _spellConfig = spellConfig; }

        public void OnGapcloser(Obj_AI_Hero sender, GapcloserArgs args)
        {
            if (!sender.IsEnemy ||
                !_spellConfig.E.Ready ||
                args.EndPosition.Distance(Global.Player) > _spellConfig.E.Range)
            {
                return;
            }

            _spellConfig.E.Cast(args.EndPosition);
        }
    }
}