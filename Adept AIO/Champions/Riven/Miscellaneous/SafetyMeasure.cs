namespace Adept_AIO.Champions.Riven.Miscellaneous
{
    using System.Linq;
    using Aimtec;
    using Aimtec.SDK.Extensions;
    using Core;

    class SafetyMeasure
    {
        private static readonly string[] DamageSpells =
        {
            "MonkeyKingSpinToWin",
            "KatarinaRTrigger",
            "HungeringStrike",
            "TwitchEParticle",
            "RengarPassiveBuffDashAADummy",
            "RengarPassiveBuffDash",
            "BraumBasicAttackPassiveOverride",
            "gnarwproc",
            "hecarimrampattack",
            "illaoiwattack",
            "JaxEmpowerTwo",
            "JayceThunderingBlow",
            "RenektonSuperExecute",
            "vaynesilvereddebuff"
        };

        private static readonly string[] TargetedSpells =
        {
            "MonkeyKingQAttack",
            "FizzPiercingStrike",
            "IreliaEquilibriumStrike",
            "RengarQ",
            "GarenQAttack",
            "GarenRPreCast",
            "PoppyPassiveAttack",
            "viktorqbuff",
            "FioraEAttack",
            "TeemoQ"
        };

        private static readonly string[] InterrupterSpell = {"RenektonPreExecute", "TalonCutthroat", "XenZhaoThrust3", "KatarinaRTrigger", "KatarinaE"};

        public static void OnProcessSpellCast(Obj_AI_Base sender, Obj_AI_BaseMissileClientDataEventArgs args)
        {
            if (!MenuConfig.Miscellaneous["Interrupt"].Enabled || sender == null)
            {
                return;
            }

            if (SpellConfig.E.Ready && (TargetedSpells.Contains(args.SpellData.Name) || DamageSpells.Contains(args.SpellData.Name)) && args.Target.IsMe)
            {
                SpellConfig.E.Cast(Game.CursorPos);
            }

            if (SpellConfig.W.Ready && sender.IsValidTarget(SpellConfig.W.Range) && InterrupterSpell.Contains(args.SpellData.Name))
            {
                SpellConfig.W.Cast();
            }
        }
    }
}