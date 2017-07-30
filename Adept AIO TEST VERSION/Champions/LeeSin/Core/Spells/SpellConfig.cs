using System;
using Adept_AIO.SDK.Extensions;
using Adept_AIO.SDK.Usables;
using Aimtec;
using Aimtec.SDK.Extensions;
using Aimtec.SDK.Orbwalking;
using Aimtec.SDK.Prediction.Skillshots;
using Spell = Aimtec.SDK.Spell;

namespace Adept_AIO.Champions.LeeSin.Core.Spells
{
    internal class SpellConfig : ISpellConfig
    {
        public bool QAboutToEnd => Environment.TickCount - LastQ >= 1900 + Game.Ping / 2f && LastQ > 0;

        public Spell W { get; private set; }
        public Spell Q { get; private set; }
        public Spell E { get; private set; }
        public Spell R { get; private set; }

        public OrbwalkerMode InsecMode { get; set; }
        public OrbwalkerMode WardjumpMode { get; set; }
        public OrbwalkerMode KickFlashMode { get; set; }

        public float LastQ { get; set; }
        public float LastW { get; set; }
        public float LastR { get; set; }
        public float LastFlash { get; set; }

        public bool IsQ2()
        {
            return !IsFirst(Q) && Q.Ready;
        }

        public bool IsFirst(Spell spell)
        {
            return GlobalExtension.Player
                .SpellBook.GetSpell(spell.Slot)
                .SpellData.Name.ToLower()
                .Contains("one");
        }

        public bool HasQ2(Obj_AI_Base target)
        {
            return target.HasBuff("BlindMonkSonicWave");
        }

        private const string PassiveName = "blindmonkpassive_cosmetic";

        public int PassiveStack()
        {
           return GlobalExtension.Player.HasBuff(PassiveName) ? GlobalExtension.Player.GetBuffCount(PassiveName) : 0;
        }

        public Vector3 InsecPosition { get; set; }

        public void Load()
        {
            Q = new Spell(SpellSlot.Q, 1100);
            Q.SetSkillshot(0.25f, 65, 1800, true, SkillshotType.Line);

            W = new Spell(SpellSlot.W, 700);
         
            E = new Spell(SpellSlot.E, 350);

            R = new Spell(SpellSlot.R, 375);
        }

        public void OnProcessSpellCast(Obj_AI_Base sender, Obj_AI_BaseMissileClientDataEventArgs args)
        {
            if (sender == null || !sender.IsMe)
            {
                return;
            }

            switch (args.SpellSlot)
            {
                case SpellSlot.Q:
                    LastQ = IsQ2() ? 0 : Environment.TickCount;
                    break;
                case SpellSlot.W:
                    LastW = Environment.TickCount;
                    break;
                case SpellSlot.R:
                    LastR = Environment.TickCount;
                    break;
            }

            if (SummonerSpells.Flash != null && args.SpellSlot == SummonerSpells.Flash.Slot)
            {
                LastFlash = Environment.TickCount;
            }
        }
    }
}
