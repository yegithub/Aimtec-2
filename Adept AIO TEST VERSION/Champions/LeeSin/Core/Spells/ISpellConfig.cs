using Aimtec;
using Aimtec.SDK.Orbwalking;
using Spell = Aimtec.SDK.Spell;

namespace Adept_AIO.Champions.LeeSin.Core.Spells
{
    public interface ISpellConfig
    {
        bool QAboutToEnd { get; }
        void Load();

        OrbwalkerMode InsecMode { get; }
        OrbwalkerMode WardjumpMode { get; }
        OrbwalkerMode KickFlashMode { get; }

        float LastQ { get; set; }
        float LastW { get; set; }
        float LastR { get; set; }
        float LastFlash { get; set; }

        Vector3 InsecPosition { get; set; }

        int PassiveStack();
        bool IsFirst(Spell spell);
        bool IsQ2();

        Spell Q { get; }
        Spell W { get; }
        Spell E { get; }
        Spell R { get; }

       
    }
}