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
        public static void OnPostAttack(AttackableUnit target)
        {
            if (target == null)
            {
                return;
            }

            Items.CastTiamat();

            if (target.HealthPercent() >= 40 || !MenuConfig.Combo["R"].Enabled || !SpellConfig.R.Ready)
            {
                return;
            }

            SpellConfig.R.CastOnUnit(target);
        }

        public static void OnUpdate()
        {
            var target = GlobalExtension.TargetSelector.GetTarget(SpellConfig.R.Range);
            if (target == null)
            {
                return;
            }

            if (SpellConfig.E.Ready && MenuConfig.Combo["E"].Enabled)
            {
                var end = ObjectManager.GetLocalPlayer().Position.Extend(Game.CursorPos, 500);
                var point = WallExtension.GeneratePoint(ObjectManager.GetLocalPlayer().Position, end).OrderBy(x => x.Distance(ObjectManager.GetLocalPlayer().Position)).FirstOrDefault();

                if (ObjectManager.GetLocalPlayer().HasBuffOfType(BuffType.Slow) || point != Vector3.Zero)
                {
                    SpellConfig.E.Cast();
                }
            }

            if (SpellConfig.W.Ready && MenuConfig.Combo["W"].Enabled && target.IsValidTarget(SpellConfig.W.Range))
            {
                SpellConfig.W.Cast(target);
            }

            if (SpellConfig.Q.Ready && MenuConfig.Combo["Q"].Enabled)
            {
                if (SpellConfig.R.Ready && MenuConfig.Combo["Beyblade"].Enabled && SummonerSpells.Flash != null &&
                    SummonerSpells.Flash.Ready && target.Distance(ObjectManager.GetLocalPlayer()) > SpellConfig.Q.Range && Dmg.Damage(target) * 1.5f >= target.Health)
                {
                    ObjectManager.GetLocalPlayer().SpellBook.CastSpell(SpellSlot.Q, target.ServerPosition);
                    SummonerSpells.Flash.Cast(target.ServerPosition);
                }
                else
                {
                    if (target.IsValidTarget(SpellConfig.Q.Range))
                    {
                        SpellConfig.Q.Cast(target);  
                        SpellConfig.CastTiamat();
                    }
                }
            }

            if (SpellConfig.R.Ready && MenuConfig.Combo["R"].Enabled && (MenuConfig.Combo["R"].Value >=
                ObjectManager.GetLocalPlayer().HealthPercent() || Dmg.Damage(target) > target.Health))
            {
                if (MenuConfig.Whitelist[target.ChampionName].Enabled)
                {
                    SpellConfig.R.CastOnUnit(target);
                }
            }
        }
    }
}
