using Adept_AIO.Champions.Yasuo.Core;
using Adept_AIO.Champions.Yasuo.Update.OrbwalkingEvents;
using Aimtec;
using Aimtec.SDK.Orbwalking;
using Aimtec.SDK.Util;

namespace Adept_AIO.Champions.Yasuo.Update.Miscellaneous
{
    class Manager
    {
        public static void PostAttack(object sender, PostAttackEventArgs args)
        {
            switch (Orbwalker.Implementation.Mode)
            {
                case OrbwalkingMode.Combo:
                    Combo.OnPostAttack();
                    break;
                case OrbwalkingMode.Mixed:
                    Harass.OnPostAttack();
                    break;
                case OrbwalkingMode.Laneclear:
                    LaneClear.OnPostAttack();
                    JungleClear.OnPostAttack();
                    break;
            }
        }

        public static void OnUpdate()
        {
            if (ObjectManager.GetLocalPlayer().IsDead)
            {
                return;
            }
            //Console.WriteLine(Extension.CurrentMode);
            switch (Orbwalker.Implementation.Mode)
            {
                case OrbwalkingMode.Combo:
                    Combo.OnUpdate();
                    break;
                case OrbwalkingMode.Mixed:
                    Harass.OnUpdate();
                    break;
                case OrbwalkingMode.Laneclear:
                    LaneClear.OnUpdate();
                    JungleClear.OnUpdate();
                    break;
            }
        }

        public static void BuffManagerOnOnAddBuff(Obj_AI_Base sender, Buff buff)
        {
            if (sender == null)
            {
                return;
            }

            if (sender.IsMe)
            {
                switch (buff.Name)
                {
                    case "YasuoQ3W":
                        Extension.CurrentMode = Mode.Tornado;
                        SpellConfig.SetSkill(Mode.Tornado);
                        break;
                }
            }
        }

        public static void BuffManagerOnOnRemoveBuff(Obj_AI_Base sender, Buff buff)
        {
            if (sender == null)
            {
                return;
            }

            if (sender.IsMe)
            {
                switch (buff.Name)
                {
                    case "YasuoQ3W":
                        Extension.CurrentMode = Mode.Normal;
                        SpellConfig.SetSkill(Mode.Normal);
                        break;
                }
            }
        }

        public static void OnPlayAnimation(Obj_AI_Base sender, Obj_AI_BasePlayAnimationEventArgs args)
        {
            if (sender == null || !sender.IsMe)
            {
                return;
            }

            switch (args.Animation)
            {
                case "Spell3":
                    if (Extension.CurrentMode == Mode.Tornado)
                    {
                        Extension.CurrentMode = Mode.DashingTornado;
                        SpellConfig.SetSkill(Mode.DashingTornado);
                    }
                    else
                    {
                        Extension.CurrentMode = Mode.Dashing;
                        SpellConfig.SetSkill(Mode.Dashing);
                    }

                    DelayAction.Queue(500, () => Extension.CurrentMode = Mode.Normal);
                    break;
            }
        }
    }
}
