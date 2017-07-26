using System.Linq;
using Adept_AIO.Champions.Kayn.Core;
using Adept_AIO.SDK.Extensions;
using Adept_AIO.SDK.Usables;
using Aimtec;
using Aimtec.SDK.Extensions;
using Aimtec.SDK.Util;

namespace Adept_AIO.Champions.Kayn.Update.OrbwalkingEvents
{
    class Combo
    {
        private static bool BeybladeActive;

        public static void OnPostAttack(AttackableUnit target)
        {
            if (!BeybladeActive || !MenuConfig.Combo["R"].Enabled || !SpellConfig.R.Ready || target == null)
            {
                return;
            }

            SpellConfig.R.CastOnUnit(target);
            BeybladeActive = false;
        }

        public static void OnUpdate()
        {

            if (SpellConfig.E.Ready && MenuConfig.Combo["E"].Enabled)
            {
                var point = WallExtension.GeneratePoint(ObjectManager.GetLocalPlayer().ServerPosition, ObjectManager.GetLocalPlayer().ServerPosition.Extend(Game.CursorPos, SpellConfig.E.Range)).FirstOrDefault();

                if (ObjectManager.GetLocalPlayer().HasBuffOfType(BuffType.Slow) || point != Vector3.Zero)
                {
                    SpellConfig.E.Cast();
                }
            }

            if (SpellConfig.W.Ready && MenuConfig.Combo["W"].Enabled)
            {
                var target = GlobalExtension.TargetSelector.GetTarget(SpellConfig.W.Range);
                if (target != null)
                {
                    SpellConfig.W.Cast(target);
                }
            }

            if (SpellConfig.Q.Ready && MenuConfig.Combo["Q"].Enabled)
            {
                if (SpellConfig.R.Ready && MenuConfig.Combo["Beyblade"].Enabled && SummonerSpells.Flash != null &&
                    SummonerSpells.Flash.Ready)
                {
                    var target = GlobalExtension.TargetSelector.GetTarget(425 + 65 + 550);
                    if (target != null && target.Distance(ObjectManager.GetLocalPlayer()) > SpellConfig.Q.Range && Dmg.Damage(target) * 1.2f >= target.Health)
                    {
                        BeybladeActive = true;
                        ObjectManager.GetLocalPlayer().SpellBook.CastSpell(SpellSlot.Q, target.ServerPosition);
                        SummonerSpells.Flash.Cast(target.ServerPosition);

                        DelayAction.Queue(3500, () =>
                        {
                            BeybladeActive = false;
                        });
                    }
                }
                else
                {
                    var target = GlobalExtension.TargetSelector.GetTarget(SpellConfig.Q.Range);
                    if (target != null)
                    {
                        SpellConfig.Q.Cast(target);
                    }
                }
            }

            if (SpellConfig.R.Ready && MenuConfig.Combo["R"].Enabled && MenuConfig.Combo["R"].Value <=
                ObjectManager.GetLocalPlayer().HealthPercent())
            {
                var target = GameObjects.EnemyHeroes.FirstOrDefault(x => x.IsValidTarget(SpellConfig.R.Range));
                if (target != null && MenuConfig.Whitelist[target.ChampionName].Enabled)
                {
                    SpellConfig.R.CastOnUnit(target);
                }
            }
        }
    }
}
