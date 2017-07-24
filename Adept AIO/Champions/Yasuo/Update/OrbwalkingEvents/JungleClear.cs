using System.Linq;
using Adept_AIO.Champions.Yasuo.Core;
using Aimtec;
using Aimtec.SDK.Extensions;
using Aimtec.SDK.Orbwalking;
using GameObjects = Adept_AIO.SDK.Extensions.GameObjects;

namespace Adept_AIO.Champions.Yasuo.Update.OrbwalkingEvents
{
    class JungleClear
    {
        public static void OnPostAttack()
        {
            if (SpellConfig.Q.Ready)
            {
                var minion = GameObjects.Jungle.FirstOrDefault(x => x.IsEnemy && x.IsValidTarget(SpellConfig.Q.Range));
                if (minion == null)
                {
                    return;
                }

                if (Extension.CurrentMode == Mode.Tornado && !MenuConfig.JungleClear["Q3"].Enabled)
                {
                    return;
                }

                if (Extension.CurrentMode == Mode.Normal && !MenuConfig.JungleClear["Q"].Enabled)
                {
                    return;
                }

                SpellConfig.Q.Cast(minion);
            }
        }

        public static void OnUpdate()
        {
            if (!SpellConfig.E.Ready || !MenuConfig.JungleClear["E"].Enabled || Orbwalker.Implementation.IsWindingUp)
            {
                return;
            }

            var minion = GameObjects.Jungle.FirstOrDefault(x => x.IsValid && x.Distance(ObjectManager.GetLocalPlayer()) <= SpellConfig.E.Range && !x.HasBuff("YasuoDashWrapper"));

            if (minion == null)
            {
                return;
            }
         
            SpellConfig.E.CastOnUnit(minion);
        }
    }
}
