namespace Adept_AIO.Champions.Yasuo
{
    using Aimtec;
    using Core;
    using Drawings;
    using Miscellaneous;
    using SDK.Delegates;
    using SDK.Generic;
    using SDK.Unit_Extensions;

    class Yasuo
    {
        public static void Init()
        {
            MenuConfig.Attach();
            SpellConfig.Load();

            Game.OnUpdate += Manager.OnUpdate;
            Obj_AI_Base.OnPlayAnimation += Manager.OnPlayAnimation;
            Obj_AI_Base.OnProcessSpellCast += Evade.OnProcessSpellCast;
            Obj_AI_Base.OnProcessSpellCast += Cast;
            Global.Orbwalker.PostAttack += Manager.PostAttack;

            Render.OnRender += DrawManager.OnRender;
            Render.OnPresent += DrawManager.OnPresent;
            BuffManager.OnAddBuff += Manager.BuffManagerOnOnAddBuff;
            BuffManager.OnRemoveBuff += Manager.BuffManagerOnOnRemoveBuff;

            Gapcloser.OnGapcloser += AntiGapcloser.OnGapcloser;
        }

        private static void Cast(Obj_AI_Base sender, Obj_AI_BaseMissileClientDataEventArgs args)
        {
            if (!sender.IsMe)
            {
                return;
            }

            if (args.SpellSlot == SpellSlot.E)
            {
                Maths.DisableAutoAttack(SpellConfig.Q.Ready ? 600 : 250);
            }
        }
    }
}