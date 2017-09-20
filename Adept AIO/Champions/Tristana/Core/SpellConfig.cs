using Adept_AIO.SDK.Unit_Extensions;
using Aimtec;
using Aimtec.SDK.Extensions;
using Aimtec.SDK.Prediction.Skillshots;
using Spell = Aimtec.SDK.Spell;

namespace Adept_AIO.Champions.Tristana.Core
{
    internal class SpellConfig
    {
        public Spell Q, W, E, R;

        public int FullRange
        {
            get
            {
                var target = Global.Orbwalker.GetOrbwalkingTarget();

                return (int)(target == null ? Global.Player.AttackRange : Global.Player.GetFullAttackRange(target) + 65);
            }
        }

        public SpellConfig()
        {
            Q = new Spell(SpellSlot.Q);

            W = new Spell(SpellSlot.W, 900);
            W.SetSkillshot(0.75f, 60, 1000, false, SkillshotType.Circle);

            E = new Spell(SpellSlot.E, FullRange);

            R = new Spell(SpellSlot.R, FullRange);
        }
    }
}
