namespace Adept_AIO.Champions.Azir.Core
{
    using System;
    using Aimtec;
    using Aimtec.SDK.Orbwalking;
    using SDK.Geometry_Related;

    class AzirHelper
    {
        public static OrbwalkerMode JumpMode, InsecMode;
        public static Geometry.Rectangle Rect;

        public static int LastR, LastQ, LastW, LastE;

        public static void OnProcessSpellCast(Obj_AI_Base sender, Obj_AI_BaseMissileClientDataEventArgs args)
        {
            if (!sender.IsMe)
            {
                return;
            }

            switch (args.SpellSlot)
            {
                case SpellSlot.Q:
                    LastQ = Environment.TickCount;
                    break;
                case SpellSlot.W:
                    LastW = Environment.TickCount;
                    break;
                case SpellSlot.E:
                    LastE = Environment.TickCount;
                    break;
                case SpellSlot.R:
                    LastR = Environment.TickCount;
                    break;
            }
        }
    }
}