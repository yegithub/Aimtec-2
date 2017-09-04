using Adept_AIO.SDK.Extensions;
using Aimtec;
using Aimtec.SDK.Extensions;
using Aimtec.SDK.Prediction.Skillshots;
using Spell = Aimtec.SDK.Spell;

namespace Adept_AIO.Champions.Jinx.Core
{
    internal class SpellConfig
    {
        public Spell Q, W, E, R;

        public bool IsQ2 => Global.Player.HasBuff("JinxQ");
        public int Q2Range => 665 + 25 * Global.Player.GetSpell(SpellSlot.Q).Level;
        public int DefaultAuotAttackRange = 590;

        public void Load()
        {
            Q = new Spell(SpellSlot.Q);

            W = new Spell(SpellSlot.W, 1500);
            W.SetSkillshot(0.5f, 80, 3300, true, SkillshotType.Line);

            E = new Spell(SpellSlot.E, 900);
            E.SetSkillshot(0.5f, 325, 1750, false, SkillshotType.Circle);

            R = new Spell(SpellSlot.R, int.MaxValue);
            R.SetSkillshot(0.25f, 140, 2200, false, SkillshotType.Line);
        }
    }
}
