using Adept_AIO.Champions.Tristana.Core;
using Adept_AIO.SDK.Delegates;
using Adept_AIO.SDK.Extensions;
using Adept_AIO.SDK.Methods;
using Aimtec;
using Aimtec.SDK.Extensions;

namespace Adept_AIO.Champions.Tristana.Update.Miscellaneous
{
    internal class AntiGapcloser
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
                _spellConfig.W.Cast(Mixed.GetFountainPos(Global.Player));
            }
            else if (_spellConfig.R.Ready)
            {
                _spellConfig.R.CastOnUnit(sender);
            }
        }
    }
}
