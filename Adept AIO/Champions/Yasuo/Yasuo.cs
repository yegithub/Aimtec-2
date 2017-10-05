using System;
using Adept_AIO.Champions.Riven.Update.Miscellaneous;
using Adept_AIO.Champions.Yasuo.Core;
using Adept_AIO.Champions.Yasuo.Drawings;
using Adept_AIO.Champions.Yasuo.Update.Miscellaneous;
using Adept_AIO.SDK.Delegates;
using Adept_AIO.SDK.Generic;
using Adept_AIO.SDK.Unit_Extensions;
using Aimtec;
using Aimtec.SDK.Events;
using AntiGapcloser = Adept_AIO.Champions.Yasuo.Update.Miscellaneous.AntiGapcloser;
using Manager = Adept_AIO.Champions.Yasuo.Update.Miscellaneous.Manager;

namespace Adept_AIO.Champions.Yasuo
{
    internal class Yasuo
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
                Maths.DisableAutoAttack();
            }
        }
    }
}
