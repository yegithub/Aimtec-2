namespace EzEvade_Port.Helpers
{
    using Aimtec;
    using Aimtec.SDK.Events;
    using Aimtec.SDK.Extensions;
    using Aimtec.SDK.Menu.Components;
    using Core;
    using Utils;

    public static class Situation
    {
        private static Obj_AI_Hero myHero => ObjectManager.GetLocalPlayer();

        public static bool CheckTeam(this Obj_AI_Base unit)
        {
            return unit.Team != myHero.Team || Evade.DevModeOn;
        }

        public static bool CheckTeam(this GameObject unit)
        {
            return unit.Team != myHero.Team || Evade.DevModeOn;
        }

        //public static bool CheckTeam(this Obj_GeneralParticleEmitter emitter)
        //{
        //    return emitter.Name.ToLower().Contains("red") || 
        //          (emitter.Name.ToLower().Contains("green") || emitter.Name.ToLower().Contains("ally")) && Evade.devModeOn ||
        //          !emitter.Name.ToLower().Contains("green") && !emitter.Name.ToLower().Contains("ally");
        //}

        public static string EmitterColor()
        {
            return Evade.DevModeOn ? "green" : "red";
        }

        public static string EmitterTeam()
        {
            return Evade.DevModeOn ? "ally" : "enemy";
        }

        public static bool isNearEnemy(this Vector2 pos, float distance, bool alreadyNear = true)
        {
            if (ObjectCache.menuCache.cache["PreventDodgingNearEnemy"].Enabled)
            {
                var curDistToEnemies = ObjectCache.myHeroCache.serverPos2D.GetDistanceToChampions();
                var posDistToEnemies = pos.GetDistanceToChampions();

                if (curDistToEnemies < distance)
                {
                    if (curDistToEnemies > posDistToEnemies)
                    {
                        return true;
                    }
                }
                else
                {
                    if (posDistToEnemies < distance)
                    {
                        return true;
                    }
                }
            }

            return false;
        }

        public static bool IsUnderTurret(this Vector2 pos, bool checkEnemy = true)
        {
            if (!ObjectCache.menuCache.cache["PreventDodgingUnderTower"].Enabled)
            {
                return false;
            }

            var turretRange = 875 + ObjectCache.myHeroCache.boundingRadius;

            foreach (var entry in ObjectCache.turrets)
            {
                var turret = entry.Value;
                if (turret == null || !turret.IsValid || turret.IsDead)
                {
                    DelayAction.Add(1, () => ObjectCache.turrets.Remove(entry.Key));
                    continue;
                }

                if (checkEnemy && turret.IsAlly)
                {
                    continue;
                }

                var distToTurret = pos.Distance(turret.Position.To2D());
                if (distToTurret <= turretRange)
                {
                    return true;
                }
            }

            return false;
        }

        public static bool ShouldDodge()
        {
            // fix
            if (ObjectCache.menuCache.cache["DontDodgeKeyEnabled"].Enabled && ObjectCache.menuCache.cache["DontDodgeKey"].As<MenuKeyBind>().Enabled)
            {
                return false;
            }

            if (ObjectCache.menuCache.cache["DodgeSkillShots"].Enabled == false || CommonChecks())
            {
                return false;
            }

            return true;
        }

        public static bool ShouldUseEvadeSpell()
        {
            // fix
            if (ObjectCache.menuCache.cache["DontDodgeKeyEnabled"].Enabled && ObjectCache.menuCache.cache["DontDodgeKey"].As<MenuKeyBind>().Enabled)
            {
                return false;
            }

            if (!ObjectCache.menuCache.cache["ActivateEvadeSpells"].Enabled || CommonChecks() || Evade.LastWindupTime - EvadeUtils.TickCount > 0)
            {
                return false;
            }

            return true;
        }

        public static bool CommonChecks()
        {
            return Evade.IsChanneling || !ObjectCache.menuCache.cache["DodgeOnlyOnComboKeyEnabled"].Enabled && !ObjectCache.menuCache.cache["DodgeComboKey"].As<MenuKeyBind>().Enabled ||
                   myHero.IsDead || myHero.IsInvulnerable || myHero.IsTargetable == false || HasSpellShield(myHero) || ChampionSpecificChecks() || myHero.IsDashing() || Evade.HasGameEnded;
        }

        public static bool ChampionSpecificChecks()
        {
            return myHero.ChampionName == "Sion" && myHero.HasBuff("SionR");
        }

        //from Evade by Esk0r
        public static bool HasSpellShield(Obj_AI_Hero unit)
        {
            if (ObjectManager.GetLocalPlayer().HasBuffOfType(BuffType.SpellShield))
            {
                return true;
            }

            if (ObjectManager.GetLocalPlayer().HasBuffOfType(BuffType.SpellImmunity))
            {
                return true;
            }

            if (unit.LastCastedSpellName() == "SivirE" && EvadeUtils.TickCount - Evade.LastSpellCastTime < 300)
            {
                return true;
            }

            if (unit.LastCastedSpellName() == "BlackShield" && EvadeUtils.TickCount - Evade.LastSpellCastTime < 300)
            {
                return true;
            }

            if (unit.LastCastedSpellName() == "NocturneShit" && EvadeUtils.TickCount - Evade.LastSpellCastTime < 300)
            {
                return true;
            }

            return false;
        }
    }
}