using System;
using Adept_AIO.Champions.LeeSin.Core;
using Adept_AIO.Champions.LeeSin.Drawings;
using Adept_AIO.Champions.LeeSin.Update.Miscellaneous;
using Adept_AIO.Champions.LeeSin.Update.OrbwalkingEvents;
using Adept_AIO.SDK.Extensions;
using Aimtec;
using Aimtec.SDK.Extensions;

namespace Adept_AIO.Champions.LeeSin
{
    internal class LeeSin
    {
        public static void Init()
        {
            MenuConfig.Attach();
            SpellConfig.Load();

            Game.OnUpdate += Manager.OnUpdate;
            Game.OnUpdate += Killsteal.OnUpdate;

            GlobalExtension.Orbwalker.PostAttack += Manager.PostAttack;

            Render.OnRender += DrawManager.RenderManager;

            Obj_AI_Base.OnProcessSpellCast += Insec.OnProcessSpellCast;
            Obj_AI_Base.OnProcessSpellCast += SpellConfig.OnProcessSpellCast;

            GameObject.OnCreate += WardManager.OnCreate;

            AttackableUnit.OnLeaveVisible += OnLeaveVisible;
        }

        private static void OnLeaveVisible(AttackableUnit sender, EventArgs eventArgs)
        {
            if (sender.Distance(GlobalExtension.Player) <= 350 && SpellConfig.E.Ready &&
                Extension.IsFirst(SpellConfig.E) && MenuConfig.Miscellaneous["Stealth"].Enabled)
            {
                SpellConfig.E.Cast();
            }
        }
    }
}
