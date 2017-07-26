﻿using System;
using Adept_AIO.SDK.Usables;
using Aimtec;
using Aimtec.SDK.Prediction.Skillshots;
using Spell = Aimtec.SDK.Spell;

namespace Adept_AIO.Champions.Kayn.Core
{
    class SpellConfig
    {
        public static float LastQCast;

        public static Spell Q, W, E, R;

        public static void Load()
        {
            Q = new Spell(SpellSlot.Q, 400);
            Q.SetSkillshot(0.25f, 600, 2400, false, SkillshotType.Circle);

            W = new Spell(SpellSlot.W, 700);
            W.SetSkillshot(0.25f, 120, 1600, false, SkillshotType.Line);

            E = new Spell(SpellSlot.E, 800);

            R = new Spell(SpellSlot.R, 750); // 550 unless Shadow Asssassin
        }

        public static void CastTiamat()
        {
            if (Environment.TickCount - LastQCast > 600)
            {
                Items.CastTiamat();
            }
        }
    }
}
